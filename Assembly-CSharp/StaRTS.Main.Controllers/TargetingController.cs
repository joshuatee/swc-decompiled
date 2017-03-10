using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.GameBoard;
using StaRTS.GameBoard.Pathfinding;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class TargetingController
	{
		public delegate void UpdateTarget(ref int numTargetingDone);

		public delegate void OnTargetingDone(SmartEntity entity);

		public delegate bool TargetValidator(SmartEntity target, object cookie);

		private const int MAX_TARGETING_PER_UPDATE = 1;

		private const int MAX_REPATHING_PER_UPDATE = 1;

		private PathingManager pathingManager;

		private ShooterController shooterController;

		private SpatialIndexController spatialIndexController;

		private PriorityList<SmartEntity> buildingsToAttack;

		private NodeList<DefensiveTroopNode> defensiveTroopNodeList;

		private NodeList<OffensiveTroopNode> offensiveTroopNodeList;

		private NodeList<OffensiveHealerNode> offensiveHealerNodeList;

		private NodeList<DefensiveHealerNode> defensiveHealerNodeList;

		private int delayIndex;

		private static readonly int[] randomTargetingDelays = new int[]
		{
			14,
			41,
			32,
			7,
			23,
			16,
			21,
			41,
			8,
			10,
			13,
			8,
			35,
			13,
			29,
			3,
			17,
			42,
			20,
			4,
			26
		};

		private static readonly int[] x_mul = new int[]
		{
			1,
			1000,
			0,
			-1000,
			-1,
			-1000,
			0,
			1000
		};

		private static readonly int[] x_div = new int[]
		{
			1,
			1414,
			1,
			1414,
			1,
			1414,
			1,
			1414
		};

		private static readonly int[] z_mul = new int[]
		{
			0,
			1000,
			1,
			1000,
			0,
			-1000,
			-1,
			-1000
		};

		private static readonly int[] z_div = new int[]
		{
			1,
			1414,
			1,
			1414,
			1,
			1414,
			1,
			1414
		};

		public TargetingController()
		{
			Service.Set<TargetingController>(this);
			this.pathingManager = Service.Get<PathingManager>();
			this.shooterController = Service.Get<ShooterController>();
			this.spatialIndexController = Service.Get<SpatialIndexController>();
			EntityController entityController = Service.Get<EntityController>();
			this.defensiveTroopNodeList = entityController.GetNodeList<DefensiveTroopNode>();
			this.offensiveTroopNodeList = entityController.GetNodeList<OffensiveTroopNode>();
			this.offensiveHealerNodeList = entityController.GetNodeList<OffensiveHealerNode>();
			this.defensiveHealerNodeList = entityController.GetNodeList<DefensiveHealerNode>();
		}

		public void SeedRandomNumberGenerator(uint seed)
		{
			this.delayIndex = (int)(seed % (uint)TargetingController.randomTargetingDelays.Length);
		}

		public void Update(ref int flip, TargetingController.UpdateTarget updateOffensive, TargetingController.UpdateTarget updateDefensive, TargetingController.UpdateTarget updateOffensiveTroopPeriodicUpdate)
		{
			if (this.pathingManager.IsPathingOngoing())
			{
				SmartEntity smartEntity = null;
				bool flag;
				this.pathingManager.UpdatePathing(out flag, out smartEntity);
				if (flag)
				{
					this.OnPathingComplete(smartEntity, smartEntity.SecondaryTargetsComp, smartEntity.StateComp, smartEntity.ShooterComp, this.shooterController.GetPrimaryTarget(smartEntity.ShooterComp));
					return;
				}
			}
			else
			{
				int num = 0;
				if (flip == 0)
				{
					updateOffensive(ref num);
					updateDefensive(ref num);
					updateOffensiveTroopPeriodicUpdate(ref num);
				}
				else
				{
					updateDefensive(ref num);
					updateOffensive(ref num);
				}
				flip ^= 1;
			}
		}

		private SmartEntity FindValidTargetFromTroopNodeList<T>(NodeList<T> troopNodeList, SmartEntity attackerEntity, ref int maxWeight, SmartEntity finalTarget) where T : Node<T>, new()
		{
			for (T t = troopNodeList.Tail; t != null; t = t.Previous)
			{
				SmartEntity smartEntity = (SmartEntity)t.Entity;
				HealthComponent healthComp = smartEntity.HealthComp;
				if (!healthComp.IsDead() && (long)GameUtils.GetSquaredDistanceToTarget(attackerEntity.ShooterComp, smartEntity) >= (long)((ulong)attackerEntity.ShooterComp.MinAttackRangeSquared) && this.CheckTarget(attackerEntity, smartEntity, ref maxWeight))
				{
					finalTarget = smartEntity;
				}
			}
			return finalTarget;
		}

		private bool IsReachedMaxTargetingLimit(ref int numTroopTargetingDone)
		{
			return numTroopTargetingDone >= 1;
		}

		public void UpdateNodes<T>(NodeList<T> troopNodeList, ref int numTroopTargetingDone, TargetingController.OnTargetingDone onTroopTargetingDone, bool updateWallBreakingTroops) where T : Node<T>, new()
		{
			if (!this.IsReachedMaxTargetingLimit(ref numTroopTargetingDone))
			{
				for (T t = troopNodeList.Tail; t != null; t = t.Previous)
				{
					SmartEntity entity = (SmartEntity)t.Entity;
					if (this.UpdateNode(entity, onTroopTargetingDone, updateWallBreakingTroops))
					{
						numTroopTargetingDone++;
						if (this.IsReachedMaxTargetingLimit(ref numTroopTargetingDone))
						{
							break;
						}
					}
				}
			}
		}

		private bool UpdateWallBreakingTroops(SmartEntity entity, TargetingController.OnTargetingDone onTroopTargetingDone)
		{
			if (!entity.TroopComp.UpdateWallAttackerTroop)
			{
				return false;
			}
			entity.TroopComp.UpdateWallAttackerTroop = false;
			if (entity.ShooterComp.Target == null)
			{
				return false;
			}
			HealthComponent healthComp = entity.ShooterComp.Target.HealthComp;
			if (healthComp == null || healthComp.IsDead())
			{
				return false;
			}
			if (entity.ShooterComp.AttackFSM.IsAttacking && entity.SecondaryTargetsComp.CurrentAlternateTarget != null && entity.SecondaryTargetsComp.ObstacleTarget == null)
			{
				entity.ShooterComp.AttackFSM.StopAttacking(true);
				bool flag = false;
				bool flag2 = Service.Get<PathingManager>().RestartPathing(entity, out flag, false);
				if (flag2 & flag)
				{
					SmartEntity primaryTarget = this.shooterController.GetPrimaryTarget(entity.ShooterComp);
					onTroopTargetingDone(entity);
					this.shooterController.StopSearch(entity.ShooterComp);
					this.OnPathingComplete(entity, entity.SecondaryTargetsComp, entity.StateComp, entity.ShooterComp, primaryTarget);
				}
				return true;
			}
			return false;
		}

		private bool UpdateNode(SmartEntity entity, TargetingController.OnTargetingDone onTroopTargetingDone, bool updateWallBreakingTroops)
		{
			if (entity.StateComp.CurState == EntityState.Disable)
			{
				return false;
			}
			bool flag = false;
			if (updateWallBreakingTroops)
			{
				return this.UpdateWallBreakingTroops(entity, onTroopTargetingDone);
			}
			if (!GameUtils.IsEligibleToFindTarget(entity.ShooterComp))
			{
				return false;
			}
			ShooterComponent shooterComp = entity.ShooterComp;
			if (!shooterComp.Searching && !shooterComp.ReevaluateTarget)
			{
				return false;
			}
			bool flag2;
			if (shooterComp.Searching)
			{
				if (shooterComp.TargetingDelayAmount > 0)
				{
					ShooterComponent expr_5B = shooterComp;
					int targetingDelayAmount = expr_5B.TargetingDelayAmount;
					expr_5B.TargetingDelayAmount = targetingDelayAmount - 1;
					return false;
				}
				flag2 = this.FindTargetForTroopNode(entity, false);
			}
			else
			{
				flag2 = this.FindTargetForTroopNode(entity, true);
			}
			shooterComp.ReevaluateTarget = false;
			if (!flag2)
			{
				return false;
			}
			if (!shooterComp.Searching)
			{
				shooterComp.AttackFSM.StopAttacking(true);
			}
			SmartEntity primaryTarget = this.shooterController.GetPrimaryTarget(entity.ShooterComp);
			if (primaryTarget.TransformComp == null)
			{
				return false;
			}
			flag = false;
			TroopComponent troopComp = entity.TroopComp;
			TeamComponent teamComp = entity.TeamComp;
			ITroopDeployableVO troopType = troopComp.TroopType;
			IShooterVO shooterVO = shooterComp.ShooterVO;
			uint maxAttackRange = this.pathingManager.GetMaxAttackRange(entity, primaryTarget);
			PathTroopParams troopParams = new PathTroopParams
			{
				TroopWidth = entity.SizeComp.Width,
				DPS = shooterVO.DPS,
				MinRange = shooterVO.MinAttackRange,
				MaxRange = maxAttackRange,
				MaxSpeed = troopComp.SpeedVO.MaxSpeed,
				PathSearchWidth = troopType.PathSearchWidth,
				IsMelee = shooterComp.IsMelee,
				IsOverWall = shooterComp.ShooterVO.OverWalls,
				IsHealer = troopType.IsHealer,
				SupportRange = troopType.SupportFollowDistance,
				CrushesWalls = troopType.CrushesWalls,
				ProjectileType = shooterVO.ProjectileType,
				IsTargetShield = GameUtils.IsEntityShieldGenerator(primaryTarget),
				TargetInRangeModifier = troopType.TargetInRangeModifier
			};
			PathBoardParams boardParams = new PathBoardParams
			{
				IgnoreWall = (teamComp != null && teamComp.IsDefender()),
				Destructible = entity.TeamComp.CanDestructBuildings()
			};
			bool flag3 = this.pathingManager.StartPathing(entity, primaryTarget, entity.TransformComp, true, out flag, -1, troopParams, boardParams, false, false);
			if (!flag3)
			{
				onTroopTargetingDone(entity);
				this.shooterController.StopSearch(shooterComp);
				return true;
			}
			if (!flag)
			{
				GameUtils.UpdateMinimumFrameCountForNextTargeting(shooterComp);
				return false;
			}
			this.RandomizeTargetingDelay(shooterComp);
			onTroopTargetingDone(entity);
			this.shooterController.StopSearch(shooterComp);
			this.OnPathingComplete(entity, entity.SecondaryTargetsComp, entity.StateComp, shooterComp, primaryTarget);
			return true;
		}

		public void RandomizeTargetingDelay(ShooterComponent ShooterComp)
		{
			if (ShooterComp.TargetingDelayed)
			{
				ShooterComp.TargetingDelayAmount = TargetingController.randomTargetingDelays[this.delayIndex];
				this.delayIndex = (this.delayIndex + 1) % TargetingController.randomTargetingDelays.Length;
			}
		}

		public void OnPathingComplete(SmartEntity self, SecondaryTargetsComponent secondaryTargetsComp, StateComponent stateComp, ShooterComponent shooterComp, SmartEntity target)
		{
			this.UpdateAlterantiveTargets(secondaryTargetsComp, self, target);
			this.shooterController.StartMoving(self);
			if (!shooterComp.FirstTargetAcquired || (target.BuildingComp != null && target.BuildingComp.BuildingType.ShowReticleWhenTargeted))
			{
				shooterComp.FirstTargetAcquired = true;
				this.OnTroopAcquiredFirstTarget(self);
			}
		}

		private void TroopWandering(SmartEntity entity)
		{
			BoardController boardController = Service.Get<BoardController>();
			DefenderComponent defenderComp = entity.DefenderComp;
			if (defenderComp == null || defenderComp.Patrolling)
			{
				return;
			}
			TransformComponent transformComp = entity.TransformComp;
			if (transformComp == null)
			{
				return;
			}
			defenderComp.Patrolling = true;
			int x = transformComp.X;
			int z = transformComp.Z;
			BoardCell<Entity> cellAt = boardController.Board.GetCellAt(x, z);
			int num = defenderComp.PatrolLoc;
			int num2 = (defenderComp.SpawnBuilding == null) ? 8 : 4;
			int viewRange = (int)entity.ShooterComp.ShooterVO.ViewRange;
			for (int i = 0; i < num2; i++)
			{
				BoardCell<Entity> boardCell;
				if (defenderComp.SpawnBuilding == null)
				{
					num = (num + 1) % num2;
					int x2 = defenderComp.SpawnX + viewRange * TargetingController.x_mul[num] / TargetingController.x_div[num] / 2;
					int z2 = defenderComp.SpawnZ + viewRange * TargetingController.z_mul[num] / TargetingController.z_div[num] / 2;
					boardCell = boardController.Board.GetClampedToBoardCellAt(x2, z2, entity.SizeComp.Width);
				}
				else
				{
					boardCell = defenderComp.SpawnBuilding.FindNextPatrolPoint(entity.SizeComp.Width, ref num);
				}
				if (boardCell.IsWalkable() && Service.Get<PathingManager>().StartPathingWorkerOrPatrol(entity, null, cellAt, boardCell, entity.SizeComp.Width, entity.TroopComp != null && entity.TroopComp.TroopType.CrushesWalls))
				{
					Service.Get<ShooterController>().StartMoving(entity);
					defenderComp.PatrolLoc = num;
					Service.Get<ShooterController>().StopSearch(entity.ShooterComp);
					return;
				}
			}
			entity.StateComp.CurState = EntityState.Idle;
		}

		private SmartEntity FindBuildingAsTarget(SmartEntity entity, ref int maxWeight)
		{
			TransformComponent transformComp = entity.TransformComp;
			this.buildingsToAttack = this.spatialIndexController.GetBuildingsToAttack(transformComp.CenterGridX(), transformComp.CenterGridZ());
			SmartEntity result = null;
			if (this.buildingsToAttack != null)
			{
				ShooterComponent shooterComp = entity.ShooterComp;
				result = this.GetPrefferedBuilding(shooterComp, this.buildingsToAttack, ref maxWeight);
			}
			return result;
		}

		private SmartEntity FindTargetForAttacker(SmartEntity entity)
		{
			int num = -1;
			SmartEntity result = null;
			SmartEntity smartEntity = this.FindBuildingAsTarget(entity, ref num);
			smartEntity = this.FindValidTargetFromTroopNodeList<DefensiveTroopNode>(this.defensiveTroopNodeList, entity, ref num, smartEntity);
			smartEntity = this.FindValidTargetFromTroopNodeList<DefensiveHealerNode>(this.defensiveHealerNodeList, entity, ref num, smartEntity);
			if (smartEntity != null)
			{
				result = smartEntity;
			}
			return result;
		}

		private bool UpdateShooterTarget(bool onlyUpdateIfNewTargetFound, SmartEntity shooter, SmartEntity target)
		{
			bool result;
			if (onlyUpdateIfNewTargetFound)
			{
				result = this.UpdateShooterTargetIfDistinct(shooter, target);
			}
			else
			{
				result = this.UpdateShooterTargetIfNotNull(shooter, target);
			}
			return result;
		}

		private bool UpdateShooterTargetIfNotNull(SmartEntity shooter, SmartEntity target)
		{
			if (target != null)
			{
				shooter.ShooterComp.Target = target;
				return true;
			}
			return false;
		}

		private bool UpdateShooterTargetIfDistinct(SmartEntity shooter, SmartEntity target)
		{
			if (target != shooter.ShooterComp.Target)
			{
				this.InvalidateCurrentTarget(shooter);
				shooter.ShooterComp.Target = target;
				return target != null;
			}
			return false;
		}

		public bool FindTargetForTroopNode(SmartEntity entity, bool onlyUpdateIfNewTargetFound)
		{
			if (entity.TroopComp.TroopShooterVO.TargetLocking && entity.ShooterComp.Target != null)
			{
				return !onlyUpdateIfNewTargetFound;
			}
			if (!onlyUpdateIfNewTargetFound)
			{
				entity.ShooterComp.Target = null;
			}
			if (entity.TroopComp.TroopShooterVO.TargetSelf)
			{
				return this.UpdateShooterTarget(onlyUpdateIfNewTargetFound, entity, entity);
			}
			bool flag = false;
			if (entity.TroopComp.TroopType.IsHealer)
			{
				SmartEntity target = this.FindBestTargetForHealer(entity);
				flag = this.UpdateShooterTarget(onlyUpdateIfNewTargetFound, entity, target);
			}
			else if (entity.TeamComp.TeamType == TeamType.Attacker)
			{
				SmartEntity target = this.FindTargetForAttacker(entity);
				flag = this.UpdateShooterTarget(onlyUpdateIfNewTargetFound, entity, target);
			}
			else if (entity.TeamComp.TeamType == TeamType.Defender)
			{
				DefenderComponent defenderComp = entity.DefenderComp;
				SmartEntity target = this.FindOffensiveTroopAsTarget(entity);
				flag = this.UpdateShooterTarget(onlyUpdateIfNewTargetFound, entity, target);
				if (flag)
				{
					entity.DefenderComp.Patrolling = false;
				}
				else if ((defenderComp.Leashed || !entity.ShooterComp.FirstTargetAcquired) && !onlyUpdateIfNewTargetFound && entity.StateComp.CurState != EntityState.Disable)
				{
					this.TroopWandering(entity);
				}
			}
			return flag;
		}

		private SmartEntity GetPrefferedBuilding(ShooterComponent shooterComp, PriorityList<SmartEntity> buildings, ref int maxWeight)
		{
			HashSet<string> hashSet = new HashSet<string>();
			SmartEntity result = null;
			int i = 0;
			int count = buildings.Count;
			while (i < count)
			{
				ElementPriorityPair<SmartEntity> elementPriorityPair = buildings.Get(i);
				SmartEntity element = elementPriorityPair.Element;
				HealthComponent healthComp = element.HealthComp;
				if (healthComp != null && !healthComp.IsDead())
				{
					BuildingComponent buildingComp = element.BuildingComp;
					if (buildingComp.BuildingType.Type != BuildingType.Blocker && (element.TrapComp == null || element.TrapComp.CurrentState == TrapState.Armed) && hashSet.Add(buildingComp.BuildingType.BuildingID))
					{
						int num = this.CalculateWeight(shooterComp, null, healthComp.ArmorType, elementPriorityPair.Priority);
						if (num > maxWeight)
						{
							maxWeight = num;
							result = element;
						}
					}
				}
				i++;
			}
			return result;
		}

		private int CalculateWeight(ShooterComponent shooterComp, HealthComponent healthComp, ArmorType targetArmorType, int targetNearness)
		{
			int num = 0;
			if (shooterComp.ShooterVO.Preference != null)
			{
				num = shooterComp.ShooterVO.Preference[(int)targetArmorType] * 100;
			}
			int num2 = 1;
			if (healthComp != null)
			{
				num2 = (healthComp.MaxHealth - healthComp.Health) * 10000 / healthComp.MaxHealth;
			}
			return num * shooterComp.ShooterVO.PreferencePercentile + targetNearness * shooterComp.ShooterVO.NearnessPercentile + num2;
		}

		public bool CanBeHealed(SmartEntity target, SmartEntity healer)
		{
			if (target == null || healer == null)
			{
				return false;
			}
			TroopComponent troopComp = target.TroopComp;
			ShooterComponent shooterComp = healer.ShooterComp;
			return troopComp != null && shooterComp != null && shooterComp.ShooterVO.Preference[(int)troopComp.TroopType.ArmorType] > 0;
		}

		private bool IsEnemy(SmartEntity target, SmartEntity selfEntity)
		{
			return target.TeamComp != null && target.TeamComp.TeamType != selfEntity.TeamComp.TeamType;
		}

		private bool CheckTarget(SmartEntity shooter, SmartEntity target, ref int maxWeight)
		{
			if (shooter == null || target == null)
			{
				return false;
			}
			TransformComponent transformComp = target.TransformComp;
			TroopComponent troopComp = target.TroopComp;
			if (transformComp == null || troopComp == null)
			{
				return false;
			}
			ShooterComponent shooterComp = shooter.ShooterComp;
			if ((long)GameUtils.GetSquaredDistanceToTarget(shooterComp, target) < (long)((ulong)shooterComp.MinAttackRangeSquared))
			{
				return false;
			}
			TransformComponent transformComp2 = shooter.TransformComp;
			int squaredDistance = GameUtils.SquaredDistance(transformComp2.CenterGridX(), transformComp2.CenterGridZ(), transformComp.CenterGridX(), transformComp.CenterGridZ());
			int targetNearness = this.spatialIndexController.CalcNearness(squaredDistance);
			int num = this.CalculateWeight(shooterComp, null, troopComp.TroopType.ArmorType, targetNearness);
			if (num > maxWeight)
			{
				maxWeight = num;
				return true;
			}
			return false;
		}

		private SmartEntity FindDefensiveTroopAsTarget(SmartEntity attacker, ref int maxWeight)
		{
			SmartEntity result = null;
			if (!this.defensiveTroopNodeList.Empty)
			{
				for (DefensiveTroopNode defensiveTroopNode = this.defensiveTroopNodeList.Head; defensiveTroopNode != null; defensiveTroopNode = defensiveTroopNode.Next)
				{
					if (this.CheckTarget(attacker, (SmartEntity)defensiveTroopNode.Entity, ref maxWeight))
					{
						result = (SmartEntity)defensiveTroopNode.Entity;
					}
				}
			}
			return result;
		}

		private SmartEntity FindOffensiveTroopAsTarget(SmartEntity entity)
		{
			if (this.offensiveTroopNodeList.Empty && this.offensiveHealerNodeList.Empty)
			{
				return null;
			}
			SmartEntity smartEntity = null;
			int num = -1;
			if (entity.WalkerComp != null)
			{
				if (!this.offensiveTroopNodeList.Empty)
				{
					for (OffensiveTroopNode offensiveTroopNode = this.offensiveTroopNodeList.Head; offensiveTroopNode != null; offensiveTroopNode = offensiveTroopNode.Next)
					{
						if (this.CheckTarget(entity, (SmartEntity)offensiveTroopNode.Entity, ref num))
						{
							smartEntity = (SmartEntity)offensiveTroopNode.Entity;
						}
					}
				}
				return this.FindValidTargetFromTroopNodeList<OffensiveHealerNode>(this.offensiveHealerNodeList, entity, ref num, smartEntity);
			}
			ShooterComponent shooterComp = entity.ShooterComp;
			TransformComponent transformComp = entity.TransformComp;
			SmartEntity smartEntity2 = this.TraverseSpiralToFindTarget((int)shooterComp.ShooterVO.ViewRange, transformComp.CenterGridX(), transformComp.CenterGridZ(), new TargetingController.TargetValidator(this.IsAttacker), entity);
			if (smartEntity2 != null && (long)GameUtils.GetSquaredDistanceToTarget(shooterComp, smartEntity2) >= (long)((ulong)shooterComp.MinAttackRangeSquared))
			{
				smartEntity = smartEntity2;
			}
			else
			{
				num = -1;
				smartEntity = this.FindValidTargetFromTroopNodeList<OffensiveTroopNode>(this.offensiveTroopNodeList, entity, ref num, null);
				smartEntity = this.FindValidTargetFromTroopNodeList<OffensiveHealerNode>(this.offensiveHealerNodeList, entity, ref num, smartEntity);
			}
			return smartEntity;
		}

		private SmartEntity FindBestTargetForHealer(SmartEntity entity)
		{
			ShooterComponent shooterComp = entity.ShooterComp;
			TransformComponent transformComp = entity.TransformComp;
			SmartEntity smartEntity = this.TraverseSpiralToFindTarget((int)shooterComp.ShooterVO.ViewRange, transformComp.CenterGridX(), transformComp.CenterGridZ(), new TargetingController.TargetValidator(this.IsHealable), entity);
			if (smartEntity == null)
			{
				if (entity.TeamComp.TeamType == TeamType.Defender)
				{
					for (DefensiveTroopNode defensiveTroopNode = this.defensiveTroopNodeList.Head; defensiveTroopNode != null; defensiveTroopNode = defensiveTroopNode.Next)
					{
						SmartEntity smartEntity2 = (SmartEntity)defensiveTroopNode.Entity;
						if (this.IsHealable(smartEntity2, entity))
						{
							smartEntity = smartEntity2;
							break;
						}
					}
				}
				else
				{
					for (OffensiveTroopNode offensiveTroopNode = this.offensiveTroopNodeList.Head; offensiveTroopNode != null; offensiveTroopNode = offensiveTroopNode.Next)
					{
						SmartEntity smartEntity3 = (SmartEntity)offensiveTroopNode.Entity;
						if (this.IsHealable(smartEntity3, entity))
						{
							smartEntity = smartEntity3;
							break;
						}
					}
				}
			}
			return smartEntity;
		}

		public void InformTurretsAboutTroop(List<ElementPriorityPair<Entity>> turretsInRangeOf, SmartEntity entity, HashSet<ShooterComponent> resetReevaluateTargetSet)
		{
			int i = 0;
			int count = turretsInRangeOf.Count;
			while (i < count)
			{
				ElementPriorityPair<Entity> elementPriorityPair = turretsInRangeOf[i];
				SmartEntity smartEntity = (SmartEntity)elementPriorityPair.Element;
				HealthComponent healthComp = smartEntity.HealthComp;
				if (healthComp != null && !healthComp.IsDead())
				{
					ShooterComponent shooterComp = smartEntity.ShooterComp;
					if (shooterComp != null)
					{
						TurretShooterComponent turretShooterComp = smartEntity.TurretShooterComp;
						if (turretShooterComp != null)
						{
							this.AddTurretTarget(shooterComp, turretShooterComp, entity, elementPriorityPair.Priority, resetReevaluateTargetSet);
						}
					}
				}
				i++;
			}
		}

		private void AddTurretTarget(ShooterComponent shooterComp, TurretShooterComponent turretShooterComp, SmartEntity target, int nearness, HashSet<ShooterComponent> resetReevaluateTargetSet)
		{
			if (!shooterComp.Searching && !shooterComp.ReevaluateTarget)
			{
				return;
			}
			if (shooterComp.ReevaluateTarget)
			{
				resetReevaluateTargetSet.Add(shooterComp);
			}
			TroopComponent troopComp = target.TroopComp;
			if (shooterComp.ShooterVO.Preference[(int)troopComp.TroopType.ArmorType] <= 0)
			{
				return;
			}
			int num = this.CalculateWeight(shooterComp, null, troopComp.TroopType.ArmorType, nearness);
			Entity turretTarget = this.shooterController.GetTurretTarget(shooterComp);
			if (turretTarget == null || num > turretShooterComp.TargetWeight)
			{
				turretShooterComp.TargetWeight = num;
				if (shooterComp.ReevaluateTarget && shooterComp.Target != target)
				{
					shooterComp.AttackFSM.StopAttacking(false);
				}
				shooterComp.Target = target;
			}
		}

		public void ReevaluateTarget(ShooterComponent shooterComp)
		{
			shooterComp.ReevaluateTarget = true;
			SmartEntity smartEntity = (SmartEntity)shooterComp.Entity;
			if (smartEntity.TurretShooterComp != null)
			{
				SmartEntity target = shooterComp.Target;
				int targetWeight = -1;
				if (target != null && !GameUtils.IsEntityDead(target))
				{
					List<ElementPriorityPair<Entity>> turretsInRangeOf = this.spatialIndexController.GetTurretsInRangeOf(target.TransformComp.CenterGridX(), target.TransformComp.CenterGridZ());
					int i = 0;
					int count = turretsInRangeOf.Count;
					while (i < count)
					{
						ElementPriorityPair<Entity> elementPriorityPair = turretsInRangeOf[i];
						if (smartEntity == elementPriorityPair.Element)
						{
							TroopComponent troopComp = target.TroopComp;
							targetWeight = this.CalculateWeight(shooterComp, null, troopComp.TroopType.ArmorType, elementPriorityPair.Priority);
							break;
						}
						i++;
					}
				}
				smartEntity.TurretShooterComp.TargetWeight = targetWeight;
			}
		}

		public void OnTroopAcquiredFirstTarget(Entity troopEntity)
		{
			Service.Get<EventManager>().SendEvent(EventId.TroopAcquiredFirstTarget, troopEntity);
		}

		public void StopSearchIfTargetFound(ShooterComponent shooterComp)
		{
			if (shooterComp.Target == null)
			{
				return;
			}
			this.shooterController.StopSearch(shooterComp);
		}

		public void UpdateAlterantiveTargets(SecondaryTargetsComponent secondarTargets, SmartEntity troop, SmartEntity target)
		{
			if (!this.ShouldSeekAlternativeTarget(troop))
			{
				return;
			}
			secondarTargets.CurrentAlternateTarget = null;
			PathingComponent pathingComp = troop.PathingComp;
			if (pathingComp != null)
			{
				secondarTargets.WallTargets = new LinkedList<Entity>();
				secondarTargets.AlternateTargets = pathingComp.CurrentPath.GetBlockingEntities(target.ID, out secondarTargets.WallTargets);
			}
		}

		private bool ShouldSeekAlternativeTarget(SmartEntity troopEntity)
		{
			TroopComponent troopComp = troopEntity.TroopComp;
			if (troopComp == null)
			{
				Service.Get<StaRTSLogger>().Error("Non troop entity checking for alternative target in TargetingSystem");
				return false;
			}
			return troopEntity.DefenderComp == null && !troopComp.TroopType.IsHealer;
		}

		public bool IsHealable(SmartEntity target, object self)
		{
			if (target == null)
			{
				return false;
			}
			SmartEntity smartEntity = (SmartEntity)self;
			if (target == smartEntity)
			{
				return false;
			}
			TroopComponent troopComp = target.TroopComp;
			HealthComponent healthComp = target.HealthComp;
			TeamComponent teamComp = target.TeamComp;
			if (troopComp == null || healthComp == null || teamComp == null)
			{
				return false;
			}
			if (target.TeamComp.TeamType != smartEntity.TeamComp.TeamType)
			{
				return false;
			}
			if (troopComp.TroopType.IsHealer)
			{
				return false;
			}
			TroopComponent troopComp2 = smartEntity.TroopComp;
			return troopComp2 == null || !troopComp2.TroopType.IsHealer || this.CanBeHealed(target, smartEntity);
		}

		public bool IsAttacker(SmartEntity target, object self)
		{
			return target != null && target.AttackerComp != null;
		}

		public bool IsAttackerThenFlag(SmartEntity target, object self)
		{
			if (target == null)
			{
				return false;
			}
			if (target.ShooterComp != null && target.ShooterComp.Target != null)
			{
				HealthComponent healthComp = target.ShooterComp.Target.HealthComp;
				if (healthComp == null || healthComp.IsDead())
				{
					return false;
				}
			}
			if (target.AttackerComp != null)
			{
				TroopComponent troopComp = target.TroopComp;
				if (troopComp != null && !troopComp.IsAbilityModeActive)
				{
					troopComp.UpdateWallAttackerTroop = true;
				}
			}
			return false;
		}

		public void UpdateNearbyTroops(int radius, int centerX, int centerZ)
		{
			TargetingController.TraverseSpiralToFindTargets(radius, centerX, centerZ, new TargetingController.TargetValidator(this.IsAttackerThenFlag), null, false);
		}

		private SmartEntity TraverseSpiralToFindTarget(int radius, int centerX, int centerZ, TargetingController.TargetValidator validator, object caller)
		{
			List<SmartEntity> list = TargetingController.TraverseSpiralToFindTargets(radius, centerX, centerZ, validator, caller, true);
			if (list != null && list.Count > 0)
			{
				return list[0];
			}
			return null;
		}

		public static List<SmartEntity> TraverseSpiralToFindTargets(int radius, int centerX, int centerZ, TargetingController.TargetValidator validator, object caller, bool returnFirstFound)
		{
			List<BoardCell<Entity>> list = GameUtils.TraverseSpiral(radius, centerX, centerZ);
			List<SmartEntity> list2 = new List<SmartEntity>();
			if (list != null)
			{
				foreach (BoardCell<Entity> current in list)
				{
					if (current.Children != null)
					{
						using (IEnumerator<BoardItem<Entity>> enumerator2 = current.Children.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								BoardItem<Entity> current2 = enumerator2.get_Current();
								if (validator((SmartEntity)current2.Data, caller))
								{
									list2.Add((SmartEntity)current2.Data);
									if (returnFirstFound)
									{
										return list2;
									}
								}
							}
						}
					}
				}
				return list2;
			}
			return list2;
		}

		public void InvalidateCurrentTarget(SmartEntity entity)
		{
			ShooterComponent shooterComp = entity.ShooterComp;
			if (shooterComp == null)
			{
				return;
			}
			shooterComp.Target = null;
			SecondaryTargetsComponent secondaryTargetsComp = entity.SecondaryTargetsComp;
			if (secondaryTargetsComp != null)
			{
				secondaryTargetsComp.ObstacleTarget = null;
				secondaryTargetsComp.CurrentAlternateTarget = null;
				secondaryTargetsComp.AlternateTargets = null;
				secondaryTargetsComp.WallTargets = null;
				secondaryTargetsComp.CurrentWallTarget = null;
			}
		}

		protected internal TargetingController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TargetingController)GCHandledObjects.GCHandleToObject(instance)).AddTurretTarget((ShooterComponent)GCHandledObjects.GCHandleToObject(*args), (TurretShooterComponent)GCHandledObjects.GCHandleToObject(args[1]), (SmartEntity)GCHandledObjects.GCHandleToObject(args[2]), *(int*)(args + 3), (HashSet<ShooterComponent>)GCHandledObjects.GCHandleToObject(args[4]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).CalculateWeight((ShooterComponent)GCHandledObjects.GCHandleToObject(*args), (HealthComponent)GCHandledObjects.GCHandleToObject(args[1]), (ArmorType)(*(int*)(args + 2)), *(int*)(args + 3)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).CanBeHealed((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).FindBestTargetForHealer((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).FindOffensiveTroopAsTarget((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).FindTargetForAttacker((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).FindTargetForTroopNode((SmartEntity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TargetingController)GCHandledObjects.GCHandleToObject(instance)).InformTurretsAboutTroop((List<ElementPriorityPair<Entity>>)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1]), (HashSet<ShooterComponent>)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TargetingController)GCHandledObjects.GCHandleToObject(instance)).InvalidateCurrentTarget((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).IsAttacker((SmartEntity)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).IsAttackerThenFlag((SmartEntity)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).IsEnemy((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).IsHealable((SmartEntity)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((TargetingController)GCHandledObjects.GCHandleToObject(instance)).OnPathingComplete((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (SecondaryTargetsComponent)GCHandledObjects.GCHandleToObject(args[1]), (StateComponent)GCHandledObjects.GCHandleToObject(args[2]), (ShooterComponent)GCHandledObjects.GCHandleToObject(args[3]), (SmartEntity)GCHandledObjects.GCHandleToObject(args[4]));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((TargetingController)GCHandledObjects.GCHandleToObject(instance)).OnTroopAcquiredFirstTarget((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((TargetingController)GCHandledObjects.GCHandleToObject(instance)).RandomizeTargetingDelay((ShooterComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((TargetingController)GCHandledObjects.GCHandleToObject(instance)).ReevaluateTarget((ShooterComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).ShouldSeekAlternativeTarget((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((TargetingController)GCHandledObjects.GCHandleToObject(instance)).StopSearchIfTargetFound((ShooterComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).TraverseSpiralToFindTarget(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), (TargetingController.TargetValidator)GCHandledObjects.GCHandleToObject(args[3]), GCHandledObjects.GCHandleToObject(args[4])));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetingController.TraverseSpiralToFindTargets(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), (TargetingController.TargetValidator)GCHandledObjects.GCHandleToObject(args[3]), GCHandledObjects.GCHandleToObject(args[4]), *(sbyte*)(args + 5) != 0));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((TargetingController)GCHandledObjects.GCHandleToObject(instance)).TroopWandering((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((TargetingController)GCHandledObjects.GCHandleToObject(instance)).UpdateAlterantiveTargets((SecondaryTargetsComponent)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1]), (SmartEntity)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((TargetingController)GCHandledObjects.GCHandleToObject(instance)).UpdateNearbyTroops(*(int*)args, *(int*)(args + 1), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).UpdateNode((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (TargetingController.OnTargetingDone)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).UpdateShooterTarget(*(sbyte*)args != 0, (SmartEntity)GCHandledObjects.GCHandleToObject(args[1]), (SmartEntity)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).UpdateShooterTargetIfDistinct((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).UpdateShooterTargetIfNotNull((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetingController)GCHandledObjects.GCHandleToObject(instance)).UpdateWallBreakingTroops((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (TargetingController.OnTargetingDone)GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
