using Net.RichardLord.Ash.Core;
using StaRTS.Audio;
using StaRTS.FX;
using StaRTS.GameBoard;
using StaRTS.GameBoard.Components;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class ShieldController : IEventObserver
	{
		[CompilerGenerated]
		[System.Serializable]
		private sealed class <>c
		{
			public static readonly ShieldController.<>c <>9 = new ShieldController.<>c();

			public static Func<string, int> <>9__11_0;

			public static Func<string, int> <>9__11_1;

			internal int ctor>b__11_0(string s)
			{
				return int.Parse(s);
			}

			internal int ctor>b__11_1(string s)
			{
				return int.Parse(s);
			}
		}

		private const int RAY_Y_POSITION = 100;

		private List<Entity> replacedEntities;

		private ShieldEffects effects;

		public int[] PointsToRange
		{
			get;
			private set;
		}

		public int[] PointsToHealth
		{
			get;
			private set;
		}

		public ShieldController()
		{
			Service.Set<ShieldController>(this);
			this.effects = Service.Get<ShieldEffects>();
			this.replacedEntities = new List<Entity>();
			Service.Get<EventManager>().RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingSelected, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingReplaced, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingViewReady, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingDeselected, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.EntityHit, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.ShieldBorderDestroyed, EventPriority.Default);
			string[] array = GameConstants.SHIELD_RANGE_PER_POINT.Split(new char[]
			{
				' '
			});
			IEnumerable<string> arg_BD_0 = array;
			Func<string, int> arg_BD_1;
			if ((arg_BD_1 = ShieldController.<>c.<>9__11_0) == null)
			{
				arg_BD_1 = (ShieldController.<>c.<>9__11_0 = new Func<string, int>(ShieldController.<>c.<>9.<.ctor>b__11_0));
			}
			this.PointsToRange = arg_BD_0.Select(arg_BD_1).ToArray<int>();
			string[] array2 = GameConstants.SHIELD_HEALTH_PER_POINT.Split(new char[]
			{
				' '
			});
			IEnumerable<string> arg_103_0 = array2;
			Func<string, int> arg_103_1;
			if ((arg_103_1 = ShieldController.<>c.<>9__11_1) == null)
			{
				arg_103_1 = (ShieldController.<>c.<>9__11_1 = new Func<string, int>(ShieldController.<>c.<>9.<.ctor>b__11_1));
			}
			this.PointsToHealth = arg_103_0.Select(arg_103_1).ToArray<int>();
		}

		public void PrepareShieldsForBattle()
		{
			Board<Entity> board = Service.Get<BoardController>().Board;
			for (int i = 0; i < board.BoardSize; i++)
			{
				for (int j = 0; j < board.BoardSize; j++)
				{
					board.GetCellAt(i, j, true).ClearObstacles();
				}
			}
			NodeList<ShieldGeneratorNode> shieldGeneratorNodeList = Service.Get<BuildingLookupController>().ShieldGeneratorNodeList;
			for (ShieldGeneratorNode shieldGeneratorNode = shieldGeneratorNodeList.Head; shieldGeneratorNode != null; shieldGeneratorNode = shieldGeneratorNode.Next)
			{
				shieldGeneratorNode.ShieldGenComp.CurrentRadius = this.PointsToRange[shieldGeneratorNode.ShieldGenComp.PointsRange];
				this.SetShieldBorderEntities(shieldGeneratorNode);
				this.effects.UpdateShieldScale(shieldGeneratorNode.ShieldGenComp.Entity);
			}
			this.effects.PlayAllEffects(false);
		}

		private void SetShieldBorderEntities(ShieldGeneratorNode node)
		{
			FlagStamp flagStamp = node.BoardItem.BoardItem.FlagStamp;
			Board<Entity> board = Service.Get<BoardController>().Board;
			for (int i = 0; i < flagStamp.Width; i++)
			{
				for (int j = 0; j < flagStamp.Depth; j++)
				{
					if ((flagStamp.FlagsMatrix[i, j] & 8u) == 8u)
					{
						BoardCell<Entity> cellAt = board.GetCellAt(i + flagStamp.X, j + flagStamp.Z, false);
						if (cellAt != null)
						{
							cellAt.AddObstacle(node.ShieldGenComp.ShieldBorderEntity);
						}
					}
				}
			}
		}

		public FlagStamp CreateFlagStampForShield(ShieldGeneratorComponent sgc, SizeComponent size, int walkableGap)
		{
			int num = 1;
			int num2 = 1;
			int num3 = (num2 + num + sgc.CurrentRadius) * 2 - 1;
			FlagStamp flagStamp = new FlagStamp(num3, num3, 0u, true);
			flagStamp.SetFlagsInRectCenter(size.Width - walkableGap + 2, size.Depth - walkableGap + 2, 4u);
			flagStamp.FillCircle(sgc.CurrentRadius + num, 16u, true);
			flagStamp.StrokeHull(8u, 16u);
			flagStamp.FillCircle(sgc.CurrentRadius + num2, 16u, true);
			return flagStamp;
		}

		private void OnDamageShieldBorder(Entity shieldGeneratorEntity, Bullet bullet)
		{
			if (shieldGeneratorEntity == null)
			{
				return;
			}
			HealthComponent healthComp = bullet.Target.HealthComp;
			if (healthComp == null || healthComp.IsDead())
			{
				return;
			}
			Vector3 targetWorldLocation = bullet.TargetWorldLocation;
			if (bullet.GunLocator != null && bullet.Owner != null)
			{
				Vector3 position = bullet.GunLocator.transform.position;
				targetWorldLocation.y = position.y;
			}
			this.effects.ApplyHitEffect(shieldGeneratorEntity, targetWorldLocation);
		}

		private void OnDestroyShieldBorder(ShieldBorderComponent sbc)
		{
			this.effects.PowerDownShieldEffect(sbc.ShieldGeneratorEntity);
			this.effects.PlayDestructionEffect(sbc.ShieldGeneratorEntity);
		}

		public void RecalculateFlagStampsForShieldBorder(Entity shieldGeneratorEntity, bool shieldUp)
		{
			BoardItem<Entity> boardItem = shieldGeneratorEntity.Get<BoardItemComponent>().BoardItem;
			WorldController worldController = Service.Get<WorldController>();
			boardItem.CurrentBoard.RemoveFlagStamp(boardItem.FlagStamp);
			int num = worldController.CalculateWalkableGap(boardItem.Size);
			FlagStamp flagStamp;
			if (shieldUp)
			{
				ShieldGeneratorComponent sgc = shieldGeneratorEntity.Get<ShieldGeneratorComponent>();
				flagStamp = this.CreateFlagStampForShield(sgc, boardItem.Size, num);
			}
			else
			{
				flagStamp = worldController.CreateFlagStamp(null, null, boardItem.Size, num);
			}
			boardItem.FlagStamp = flagStamp;
			worldController.AddUnWalkableUnDestructibleFlags(boardItem.FlagStamp, boardItem.Size, num, false);
			boardItem.FlagStamp.CenterTo(boardItem.BoardX + (boardItem.Width - num) / 2, boardItem.BoardZ + (boardItem.Depth - num) / 2);
			boardItem.CurrentBoard.AddFlagStamp(boardItem.FlagStamp);
		}

		public void StopAllEffects()
		{
			NodeList<ShieldGeneratorNode> shieldGeneratorNodeList = Service.Get<BuildingLookupController>().ShieldGeneratorNodeList;
			for (ShieldGeneratorNode shieldGeneratorNode = shieldGeneratorNodeList.Head; shieldGeneratorNode != null; shieldGeneratorNode = shieldGeneratorNode.Next)
			{
				this.effects.StopEffect(shieldGeneratorNode.Entity);
			}
		}

		public void RemoveShieldEffect(Entity entity)
		{
			this.effects.CleanupShield(entity);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (id <= EventId.BuildingDeselected)
			{
				if (id != EventId.BuildingViewReady)
				{
					if (id != EventId.BuildingSelected)
					{
						if (id == EventId.BuildingDeselected)
						{
							Entity entity = cookie as Entity;
							if (entity.Has<ShieldGeneratorComponent>() && !GameUtils.IsVisitingBase())
							{
								this.StopAllEffects();
							}
						}
					}
					else
					{
						Entity entity2 = cookie as Entity;
						if (entity2.Has<ShieldGeneratorComponent>() && !GameUtils.IsVisitingBase())
						{
							this.effects.PlayAllEffects(true);
							Service.Get<AudioManager>().PlayAudio("sfx_shields_power_up");
						}
					}
				}
				else if (currentState is HomeState || currentState is EditBaseState)
				{
					EntityViewParams entityViewParams = cookie as EntityViewParams;
					if (this.replacedEntities.Contains(entityViewParams.Entity))
					{
						this.InitializeEffects(entityViewParams.Entity);
						this.replacedEntities.Remove(entityViewParams.Entity);
					}
				}
			}
			else if (id <= EventId.BuildingReplaced)
			{
				if (id != EventId.EntityHit)
				{
					if (id == EventId.BuildingReplaced)
					{
						Entity entity3 = cookie as Entity;
						if (entity3.Has<ShieldGeneratorComponent>())
						{
							this.replacedEntities.Add(cookie as Entity);
						}
					}
				}
				else
				{
					Bullet bullet = cookie as Bullet;
					ShieldBorderComponent shieldBorderComp = bullet.Target.ShieldBorderComp;
					if (shieldBorderComp != null)
					{
						this.OnDamageShieldBorder(shieldBorderComp.ShieldGeneratorEntity, bullet);
					}
				}
			}
			else if (id != EventId.WorldLoadComplete)
			{
				if (id == EventId.ShieldBorderDestroyed)
				{
					ShieldBorderComponent sbc = cookie as ShieldBorderComponent;
					this.OnDestroyShieldBorder(sbc);
				}
			}
			else
			{
				this.effects.Cleanup();
				this.replacedEntities.Clear();
				if (GameUtils.IsVisitingBase())
				{
					this.effects.PlayAllEffects(true);
				}
			}
			return EatResponse.NotEaten;
		}

		public void InitializeEffects(Entity entity)
		{
			if (entity != null)
			{
				this.effects.CreateEffect(entity);
			}
			else
			{
				NodeList<ShieldGeneratorNode> shieldGeneratorNodeList = Service.Get<BuildingLookupController>().ShieldGeneratorNodeList;
				for (ShieldGeneratorNode shieldGeneratorNode = shieldGeneratorNodeList.Head; shieldGeneratorNode != null; shieldGeneratorNode = shieldGeneratorNode.Next)
				{
					this.effects.CreateEffect(shieldGeneratorNode.Entity);
				}
			}
			Entity selectedBuilding = Service.Get<BuildingController>().SelectedBuilding;
			if (selectedBuilding != null && selectedBuilding.Has<ShieldGeneratorComponent>())
			{
				this.effects.PlayAllEffects(true);
			}
		}

		public ShieldGeneratorComponent GetActiveShieldAffectingBoardPos(int targetBoardX, int targetBoardZ)
		{
			ShieldGeneratorComponent result = null;
			float num = 3.40282347E+38f;
			NodeList<ShieldGeneratorNode> shieldGeneratorNodeList = Service.Get<BuildingLookupController>().ShieldGeneratorNodeList;
			for (ShieldGeneratorNode shieldGeneratorNode = shieldGeneratorNodeList.Head; shieldGeneratorNode != null; shieldGeneratorNode = shieldGeneratorNode.Next)
			{
				Entity entity = shieldGeneratorNode.Entity;
				if (entity != null && entity.Has<HealthComponent>() && !entity.Get<HealthComponent>().IsDead())
				{
					ShieldGeneratorComponent shieldGenComp = shieldGeneratorNode.ShieldGenComp;
					if (shieldGenComp != null)
					{
						Entity shieldBorderEntity = shieldGenComp.ShieldBorderEntity;
						float num2;
						if (shieldBorderEntity != null && shieldBorderEntity.Has<HealthComponent>() && !shieldBorderEntity.Get<HealthComponent>().IsDead() && this.IsPositionUnderShield(targetBoardX, targetBoardZ, (SmartEntity)entity, out num2) && num2 < num)
						{
							num = num2;
							result = shieldGenComp;
						}
					}
				}
			}
			return result;
		}

		public bool IsPositionUnderShield(int boardX, int boardZ, SmartEntity shieldGenerator, out float distSq)
		{
			TransformComponent transformComponent = shieldGenerator.Get<TransformComponent>();
			distSq = (float)GameUtils.SquaredDistance(boardX, boardZ, transformComponent.CenterGridX(), transformComponent.CenterGridZ());
			return distSq <= (float)shieldGenerator.ShieldGeneratorComp.RadiusSquared;
		}

		public bool IsPositionUnderShield(int boardX, int boardZ, SmartEntity shieldGenerator)
		{
			float num;
			return this.IsPositionUnderShield(boardX, boardZ, shieldGenerator, out num);
		}

		public bool IsTargetCellUnderShield(Vector3 targetPos)
		{
			Vector3 rayPos = new Vector3(targetPos.x, 100f, targetPos.z);
			NodeList<ShieldGeneratorNode> shieldGeneratorNodeList = Service.Get<BuildingLookupController>().ShieldGeneratorNodeList;
			Vector3 zero = Vector3.zero;
			for (ShieldGeneratorNode shieldGeneratorNode = shieldGeneratorNodeList.Head; shieldGeneratorNode != null; shieldGeneratorNode = shieldGeneratorNode.Next)
			{
				SmartEntity smartEntity = (SmartEntity)shieldGeneratorNode.Entity;
				if (smartEntity.HealthComp != null && !smartEntity.HealthComp.IsDead() && this.GetRayShieldIntersection(rayPos, targetPos, smartEntity.ShieldGeneratorComp, out zero))
				{
					return true;
				}
			}
			return false;
		}

		public bool GetRayShieldIntersection(Vector3 rayPos, Vector3 targetPos, ShieldGeneratorComponent shield, out Vector3 intersectionWorldPoint)
		{
			SmartEntity smartEntity = shield.Entity as SmartEntity;
			if (smartEntity == null)
			{
				intersectionWorldPoint = Vector3.zero;
				return false;
			}
			TransformComponent transformComp = smartEntity.TransformComp;
			Vector3 ellipseOrigin = new Vector3(Units.BoardToWorldX(transformComp.CenterX()), 1.25f, Units.BoardToWorldZ(transformComp.CenterZ()));
			float num = 3f * (float)shield.CurrentRadius;
			Vector3 ellipseRadius = new Vector3(num, num, num);
			Vector3 rayDir = targetPos - rayPos;
			targetPos.Normalize();
			return UnityUtils.GetRayEllipsoidIntersection(rayPos, rayDir, ellipseOrigin, ellipseRadius, out intersectionWorldPoint);
		}

		public void ShieldBorderKilled(SmartEntity shieldBorderEntity)
		{
			if (shieldBorderEntity != null)
			{
				ShieldBorderComponent shieldBorderComp = shieldBorderEntity.ShieldBorderComp;
				if (shieldBorderComp != null)
				{
					this.RecalculateFlagStampsForShieldBorder(shieldBorderComp.ShieldGeneratorEntity, false);
					Service.Get<EventManager>().SendEvent(EventId.ShieldBorderDestroyed, shieldBorderComp);
				}
			}
		}

		protected internal ShieldController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldController)GCHandledObjects.GCHandleToObject(instance)).CreateFlagStampForShield((ShieldGeneratorComponent)GCHandledObjects.GCHandleToObject(*args), (SizeComponent)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldController)GCHandledObjects.GCHandleToObject(instance)).PointsToHealth);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldController)GCHandledObjects.GCHandleToObject(instance)).PointsToRange);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldController)GCHandledObjects.GCHandleToObject(instance)).GetActiveShieldAffectingBoardPos(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ShieldController)GCHandledObjects.GCHandleToObject(instance)).InitializeEffects((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldController)GCHandledObjects.GCHandleToObject(instance)).IsPositionUnderShield(*(int*)args, *(int*)(args + 1), (SmartEntity)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldController)GCHandledObjects.GCHandleToObject(instance)).IsTargetCellUnderShield(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ShieldController)GCHandledObjects.GCHandleToObject(instance)).OnDamageShieldBorder((Entity)GCHandledObjects.GCHandleToObject(*args), (Bullet)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ShieldController)GCHandledObjects.GCHandleToObject(instance)).OnDestroyShieldBorder((ShieldBorderComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ShieldController)GCHandledObjects.GCHandleToObject(instance)).PrepareShieldsForBattle();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ShieldController)GCHandledObjects.GCHandleToObject(instance)).RecalculateFlagStampsForShieldBorder((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((ShieldController)GCHandledObjects.GCHandleToObject(instance)).RemoveShieldEffect((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ShieldController)GCHandledObjects.GCHandleToObject(instance)).PointsToHealth = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ShieldController)GCHandledObjects.GCHandleToObject(instance)).PointsToRange = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ShieldController)GCHandledObjects.GCHandleToObject(instance)).SetShieldBorderEntities((ShieldGeneratorNode)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((ShieldController)GCHandledObjects.GCHandleToObject(instance)).ShieldBorderKilled((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((ShieldController)GCHandledObjects.GCHandleToObject(instance)).StopAllEffects();
			return -1L;
		}
	}
}
