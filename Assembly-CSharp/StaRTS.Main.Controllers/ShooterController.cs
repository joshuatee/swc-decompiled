using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class ShooterController
	{
		public ShooterController()
		{
			Service.Set<ShooterController>(this);
		}

		public Target GetTargetToAttack(SmartEntity entity)
		{
			ShooterComponent shooterComp = entity.ShooterComp;
			if (shooterComp == null)
			{
				return null;
			}
			SecondaryTargetsComponent secondaryTargetsComp = entity.SecondaryTargetsComp;
			SmartEntity targetEntity;
			if (secondaryTargetsComp != null)
			{
				targetEntity = this.GetTroopTarget(secondaryTargetsComp, shooterComp);
				return Target.CreateTargetForTroop(entity, targetEntity, secondaryTargetsComp);
			}
			targetEntity = this.GetTurretTarget(shooterComp);
			return Target.CreateTargetForTurret(targetEntity);
		}

		public SmartEntity GetPrimaryTarget(ShooterComponent shooterComp)
		{
			if (shooterComp == null)
			{
				return null;
			}
			return shooterComp.Target;
		}

		public SmartEntity GetPrimaryTarget(SmartEntity shooter)
		{
			return this.GetPrimaryTarget(shooter.ShooterComp);
		}

		public SmartEntity GetTroopTarget(SmartEntity troopEntity)
		{
			return this.GetTroopTarget(troopEntity.SecondaryTargetsComp, troopEntity.ShooterComp);
		}

		public SmartEntity GetTroopWallCrushingTarget(SmartEntity troopEntity)
		{
			if (troopEntity.SecondaryTargetsComp == null)
			{
				return null;
			}
			if (troopEntity.SecondaryTargetsComp.WallTargets != null && troopEntity.SecondaryTargetsComp.WallTargets.Count > 0)
			{
				if (troopEntity.SecondaryTargetsComp.CurrentWallTarget == null)
				{
					troopEntity.SecondaryTargetsComp.CurrentWallTarget = (SmartEntity)troopEntity.SecondaryTargetsComp.WallTargets.First.Value;
					troopEntity.SecondaryTargetsComp.WallTargets.RemoveFirst();
				}
				return (SmartEntity)troopEntity.SecondaryTargetsComp.CurrentWallTarget;
			}
			return null;
		}

		public SmartEntity GetTroopTarget(SecondaryTargetsComponent secondaryTargets, ShooterComponent shooterComp)
		{
			if (secondaryTargets == null)
			{
				return null;
			}
			if (shooterComp == null || shooterComp.Entity == null)
			{
				return null;
			}
			if (secondaryTargets.ObstacleTarget != null)
			{
				return (SmartEntity)secondaryTargets.ObstacleTarget;
			}
			if (secondaryTargets.CurrentAlternateTarget != null)
			{
				return (SmartEntity)secondaryTargets.CurrentAlternateTarget;
			}
			if (secondaryTargets.AlternateTargets != null)
			{
				while (secondaryTargets.AlternateTargets.First != null)
				{
					Entity value = secondaryTargets.AlternateTargets.First.Value;
					secondaryTargets.AlternateTargets.RemoveFirst();
					if (this.ShouldAttackAlternateTarget(shooterComp, value))
					{
						secondaryTargets.CurrentAlternateTarget = value;
						return (SmartEntity)secondaryTargets.CurrentAlternateTarget;
					}
				}
			}
			return shooterComp.Target;
		}

		public SmartEntity GetTurretTarget(ShooterComponent shooterComp)
		{
			return shooterComp.Target;
		}

		public void StopSearch(ShooterComponent shooterComp)
		{
			shooterComp.Searching = false;
		}

		public void Reload(ShooterComponent shooterComp)
		{
			if (shooterComp.ShouldCountClips && shooterComp.ShotsRemainingInClip == 0u)
			{
				uint numClipsUsed = shooterComp.NumClipsUsed;
				shooterComp.NumClipsUsed = numClipsUsed + 1u;
				Service.Get<EventManager>().SendEvent(EventId.ShooterClipUsed, shooterComp);
			}
			shooterComp.ShotsRemainingInClip = shooterComp.ShooterVO.ShotCount;
		}

		public void DecreaseShotsRemainingInClip(ShooterComponent shooterComp)
		{
			uint shotsRemainingInClip = shooterComp.ShotsRemainingInClip;
			shooterComp.ShotsRemainingInClip = shotsRemainingInClip - 1u;
		}

		public bool NeedsReload(ShooterComponent shooterComp)
		{
			return shooterComp.ShotsRemainingInClip == 0u && shooterComp.ShooterVO.ShotCount > 0u;
		}

		public void StartMoving(SmartEntity entity)
		{
			ITroopDeployableVO troopType = entity.TroopComp.TroopType;
			entity.StateComp.CurState = EntityState.Moving;
			entity.StateComp.IsRunning = false;
			entity.PathingComp.MaxSpeed = entity.TroopComp.SpeedVO.MaxSpeed;
			if (troopType.RunSpeed > 0)
			{
				entity.StateComp.ForceUpdateAnimation = true;
				if (entity.PathingComp.CurrentPath.CellCount > troopType.RunThreshold && entity.ShooterComp.Target != null)
				{
					entity.StateComp.IsRunning = true;
					entity.PathingComp.MaxSpeed = troopType.RunSpeed;
					if (entity.TroopShieldComp != null)
					{
						entity.Remove<TroopShieldComponent>();
					}
				}
			}
		}

		public void StopMoving(StateComponent stateComp)
		{
			stateComp.CurState = EntityState.Idle;
		}

		public void StopAttacking(StateComponent stateComp)
		{
			Service.Get<EventManager>().SendEvent(EventId.ShooterStoppedAttacking, (SmartEntity)stateComp.Entity);
			stateComp.CurState = EntityState.Idle;
		}

		public bool InRange(int distanceSquared, ShooterComponent shooterComp)
		{
			return (long)distanceSquared >= (long)((ulong)shooterComp.MinAttackRangeSquared) && (long)distanceSquared <= (long)((ulong)shooterComp.MaxAttackRangeSquared);
		}

		public bool InRange(int distanceSquared, ShooterComponent shooterComp, uint maxAttackRange)
		{
			return (long)distanceSquared >= (long)((ulong)shooterComp.MinAttackRangeSquared) && (long)distanceSquared <= (long)((ulong)(maxAttackRange * maxAttackRange));
		}

		public void OnCooldownExit(ShooterComponent shooterComp)
		{
			this.Reload(shooterComp);
		}

		private bool ShouldAttackAlternateTarget(ShooterComponent shooterComp, Entity alternateTarget)
		{
			if (alternateTarget == null)
			{
				return false;
			}
			BuildingComponent buildingComponent = alternateTarget.Get<BuildingComponent>();
			TransformComponent transformComponent = alternateTarget.Get<TransformComponent>();
			if (buildingComponent == null || buildingComponent.BuildingType == null || transformComponent == null)
			{
				return false;
			}
			if (buildingComponent.BuildingType.Type == BuildingType.Blocker)
			{
				return false;
			}
			if (buildingComponent.BuildingType.Type != BuildingType.Wall)
			{
				return true;
			}
			SmartEntity primaryTarget = this.GetPrimaryTarget(shooterComp);
			return primaryTarget != null && primaryTarget.Get<TransformComponent>() != null;
		}

		protected internal ShooterController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ShooterController)GCHandledObjects.GCHandleToObject(instance)).DecreaseShotsRemainingInClip((ShooterComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterController)GCHandledObjects.GCHandleToObject(instance)).GetPrimaryTarget((ShooterComponent)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterController)GCHandledObjects.GCHandleToObject(instance)).GetPrimaryTarget((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterController)GCHandledObjects.GCHandleToObject(instance)).GetTargetToAttack((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterController)GCHandledObjects.GCHandleToObject(instance)).GetTroopTarget((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterController)GCHandledObjects.GCHandleToObject(instance)).GetTroopTarget((SecondaryTargetsComponent)GCHandledObjects.GCHandleToObject(*args), (ShooterComponent)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterController)GCHandledObjects.GCHandleToObject(instance)).GetTroopWallCrushingTarget((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterController)GCHandledObjects.GCHandleToObject(instance)).GetTurretTarget((ShooterComponent)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterController)GCHandledObjects.GCHandleToObject(instance)).InRange(*(int*)args, (ShooterComponent)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterController)GCHandledObjects.GCHandleToObject(instance)).NeedsReload((ShooterComponent)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ShooterController)GCHandledObjects.GCHandleToObject(instance)).OnCooldownExit((ShooterComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ShooterController)GCHandledObjects.GCHandleToObject(instance)).Reload((ShooterComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterController)GCHandledObjects.GCHandleToObject(instance)).ShouldAttackAlternateTarget((ShooterComponent)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ShooterController)GCHandledObjects.GCHandleToObject(instance)).StartMoving((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ShooterController)GCHandledObjects.GCHandleToObject(instance)).StopAttacking((StateComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ShooterController)GCHandledObjects.GCHandleToObject(instance)).StopMoving((StateComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((ShooterController)GCHandledObjects.GCHandleToObject(instance)).StopSearch((ShooterComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
