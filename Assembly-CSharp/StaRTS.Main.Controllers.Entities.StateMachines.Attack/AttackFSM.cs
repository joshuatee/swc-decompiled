using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.StateMachines.Attack
{
	public class AttackFSM : TimeLockedStateMachine
	{
		public delegate void StateChangeCallback(AttackFSM attackFSM, IState prevState, IState curState);

		public const uint MAX_STATE_CHANGES_PER_TICK = 100u;

		private ShooterController shooterController;

		private HealthType healthType;

		private TeamType ownerTeam;

		private int lastLookAtX;

		private int lastLookAtZ;

		private int shotIndex;

		private Dictionary<int, int> gunSequences;

		public SmartEntity Entity
		{
			get;
			protected set;
		}

		public ShooterComponent ShooterComp
		{
			get;
			protected set;
		}

		public StateComponent StateComponent
		{
			get;
			protected set;
		}

		public TransformComponent TransformComponent
		{
			get;
			protected set;
		}

		public IdleState IdleState
		{
			get;
			protected set;
		}

		public TurnState TurnState
		{
			get;
			protected set;
		}

		public WarmupState WarmupState
		{
			get;
			protected set;
		}

		public PreFireState PreFireState
		{
			get;
			protected set;
		}

		public PostFireState PostFireState
		{
			get;
			protected set;
		}

		public CooldownState CooldownState
		{
			get;
			protected set;
		}

		public bool IsAttacking
		{
			get
			{
				return base.CurrentState != this.IdleState;
			}
		}

		public uint getTurnDelayInMs()
		{
			return this.TurnState.GetDuration();
		}

		public AttackFSM(ISimTimeProvider timeProvider, SmartEntity entity, StateComponent stateComponent, ShooterComponent shooterComp, TransformComponent transformComponent, HealthType healthType) : base(timeProvider)
		{
			this.Initialize(entity, shooterComp, stateComponent, transformComponent, healthType);
		}

		private void Initialize(SmartEntity entity, ShooterComponent shooterComp, StateComponent stateComponent, TransformComponent transformComponent, HealthType healthType)
		{
			this.Entity = entity;
			this.shooterController = Service.Get<ShooterController>();
			this.ShooterComp = shooterComp;
			this.StateComponent = stateComponent;
			this.TransformComponent = transformComponent;
			this.IdleState = new IdleState(this);
			this.TurnState = new TurnState(this);
			this.WarmupState = new WarmupState(this);
			this.PreFireState = new PreFireState(this);
			this.PostFireState = new PostFireState(this);
			this.CooldownState = new CooldownState(this);
			this.shooterController.Reload(shooterComp);
			base.SetLegalTransition<IdleState, TurnState>();
			base.SetLegalTransition<IdleState, WarmupState>();
			base.SetLegalTransition<TurnState, IdleState>();
			base.SetLegalTransition<TurnState, WarmupState>();
			base.SetLegalTransition<WarmupState, IdleState>();
			base.SetLegalTransition<WarmupState, PreFireState>();
			base.SetLegalTransition<PreFireState, IdleState>();
			base.SetLegalTransition<PreFireState, PostFireState>();
			base.SetLegalTransition<PostFireState, IdleState>();
			base.SetLegalTransition<PostFireState, PreFireState>();
			base.SetLegalTransition<PostFireState, CooldownState>();
			base.SetLegalTransition<CooldownState, IdleState>();
			base.SetLegalTransition<CooldownState, WarmupState>();
			if (entity.TroopComp != null && entity.TroopComp.TroopType.Type == TroopType.Champion && entity.TeamComp.TeamType == TeamType.Defender)
			{
				entity.StateComp.CurState = EntityState.Disable;
				entity.StateComp.ForceUpdateAnimation = true;
			}
			this.SetState(this.IdleState);
			this.healthType = healthType;
			this.ownerTeam = this.Entity.TeamComp.TeamType;
			this.lastLookAtX = (this.lastLookAtZ = 0);
			this.shotIndex = 0;
			TroopComponent troopComp = this.Entity.TroopComp;
			BuildingComponent buildingComp = this.Entity.BuildingComp;
			if (troopComp != null)
			{
				this.gunSequences = troopComp.TroopShooterVO.Sequences;
				return;
			}
			if (buildingComp != null)
			{
				string uid = null;
				BuildingType type = buildingComp.BuildingType.Type;
				if (type != BuildingType.Turret)
				{
					if (type == BuildingType.Trap)
					{
						uid = this.Entity.TrapComp.Type.TurretTED.TurretUid;
					}
				}
				else
				{
					uid = buildingComp.BuildingType.TurretUid;
				}
				TurretTypeVO turretTypeVO = Service.Get<IDataController>().Get<TurretTypeVO>(uid);
				this.gunSequences = turretTypeVO.Sequences;
				return;
			}
			Service.Get<StaRTSLogger>().Error("Attaching AttackFMS to Unsupported Entity. No Troop, Building, or Trap Componenet found.");
		}

		public bool IsGunSequenceDone()
		{
			return this.shotIndex == 0;
		}

		public bool CanSwitchAbility()
		{
			return base.CurrentState == this.IdleState || base.CurrentState == this.TurnState;
		}

		public void Update()
		{
			if (!this.IsAttacking)
			{
				return;
			}
			if (base.CurrentState == this.TurnState)
			{
				this.SetState(this.WarmupState);
				return;
			}
			if (base.CurrentState == this.WarmupState)
			{
				this.SetState(this.PreFireState);
				return;
			}
			if (base.CurrentState == this.PreFireState)
			{
				this.SetState(this.PostFireState);
				return;
			}
			if (base.CurrentState != this.PostFireState)
			{
				if (base.CurrentState == this.CooldownState)
				{
					this.SetState(this.WarmupState);
				}
				return;
			}
			if (this.shooterController.NeedsReload(this.ShooterComp))
			{
				this.SetState(this.CooldownState);
				return;
			}
			if (this.IsGunSequenceDone())
			{
				this.StateComponent.CurState = EntityState.AttackingReset;
			}
			this.SetState(this.PreFireState);
		}

		public bool StartAttack()
		{
			int num = 0;
			Target targetToAttack = this.shooterController.GetTargetToAttack(this.Entity);
			if (this.Entity.TroopComp != null && this.Entity.TroopComp.TroopShooterVO.TargetSelf)
			{
				return this.SetState(this.WarmupState);
			}
			if (this.StateComponent.CurState == EntityState.Moving)
			{
				PathView pathView = this.Entity.PathingComp.PathView;
				BoardCell<Entity> nextTurn = pathView.GetNextTurn();
				BoardCell<Entity> prevTurn = pathView.GetPrevTurn();
				if (prevTurn != null)
				{
					this.lastLookAtX = nextTurn.X - prevTurn.X;
					this.lastLookAtZ = nextTurn.Z - prevTurn.Z;
				}
			}
			int x = targetToAttack.TargetBoardX - this.TransformComponent.CenterGridX();
			int y = targetToAttack.TargetBoardZ - this.TransformComponent.CenterGridZ();
			if (this.lastLookAtX != 0 || this.lastLookAtZ != 0)
			{
				int num2 = IntMath.Atan2Lookup(this.lastLookAtX, this.lastLookAtZ);
				int num3 = IntMath.Atan2Lookup(x, y);
				int num4 = (num2 > num3) ? (num2 - num3) : (num3 - num2);
				if (num4 > 16384)
				{
					num4 = 32768 - num4;
				}
				WalkerComponent walkerComp = this.Entity.WalkerComp;
				if (walkerComp != null)
				{
					long num5 = (long)walkerComp.SpeedVO.RotationSpeed * 16384L;
					if (num5 > 0L)
					{
						num = (int)((long)num4 * 3142L * 1000L / num5);
						num *= 2;
					}
				}
			}
			this.lastLookAtX = x;
			this.lastLookAtZ = y;
			bool result;
			if (num > 0)
			{
				this.TurnState.SetDefaultLockDuration((uint)num);
				result = this.SetState(this.TurnState);
			}
			else
			{
				result = this.SetState(this.WarmupState);
			}
			return result;
		}

		public bool InStrictCoolDownState()
		{
			return base.CurrentState == this.CooldownState && this.ShooterComp.ShooterVO.StrictCooldown;
		}

		public bool StopAttacking(bool isTroop)
		{
			if (isTroop)
			{
				Service.Get<ShooterController>().StopAttacking(this.Entity.StateComp);
			}
			if (!this.IsAttacking)
			{
				return false;
			}
			if (this.InStrictCoolDownState())
			{
				return false;
			}
			this.SetState(this.IdleState);
			return true;
		}

		public bool IsUnlocked()
		{
			return base.CurrentState.IsUnlocked();
		}

		private void FireAShot(int spawnBoardX, int spawnBoardZ, Vector3 startPos, Target target, GameObject gunLocator)
		{
			FactionType faction = FactionType.Invalid;
			if (this.Entity != null)
			{
				BuildingComponent buildingComp = this.Entity.BuildingComp;
				TroopComponent troopComp = this.Entity.TroopComp;
				if (buildingComp != null)
				{
					faction = buildingComp.BuildingType.Faction;
				}
				else if (troopComp != null)
				{
					faction = troopComp.TroopType.Faction;
				}
			}
			HealthFragment payload = new HealthFragment(this.Entity, this.healthType, this.ShooterComp.ShooterVO.Damage);
			Service.Get<ProjectileController>().SpawnProjectileForTarget(0u, spawnBoardX, spawnBoardZ, startPos, target, payload, this.ownerTeam, this.Entity, this.ShooterComp.ShooterVO.ProjectileType, true, this.Entity.BuffComp.Buffs, faction, gunLocator);
			Service.Get<EventManager>().SendEvent(EventId.EntityAttackedTarget, this.Entity);
			this.shooterController.DecreaseShotsRemainingInClip(this.ShooterComp);
		}

		public void Fire()
		{
			TransformComponent transformComp = this.Entity.TransformComp;
			Vector3 startPos = Vector3.zero;
			Target targetToAttack = this.shooterController.GetTargetToAttack(this.Entity);
			this.shotIndex = (this.shotIndex + 1) % this.gunSequences.Count;
			GameObjectViewComponent gameObjectViewComp = this.Entity.GameObjectViewComp;
			List<GameObject> list;
			if (gameObjectViewComp != null && gameObjectViewComp.GunLocators.Count != 0)
			{
				list = gameObjectViewComp.GunLocators[this.shotIndex];
			}
			else
			{
				list = null;
			}
			for (int i = 0; i < this.gunSequences[this.shotIndex + 1]; i++)
			{
				if (this.ShooterComp.ShotsRemainingInClip > 0u)
				{
					if (list != null)
					{
						startPos = list[i].transform.position;
						this.FireAShot(transformComp.CenterGridX(), transformComp.CenterGridZ(), startPos, targetToAttack, list[i]);
					}
					else
					{
						Vector3 vector = (gameObjectViewComp == null) ? new Vector3(transformComp.CenterX(), 0f, transformComp.CenterZ()) : gameObjectViewComp.MainGameObject.transform.position;
						startPos = new Vector3(vector.x, 2f, vector.z);
						this.FireAShot(transformComp.CenterGridX(), transformComp.CenterGridZ(), startPos, targetToAttack, null);
					}
				}
			}
		}

		public void SetEntityState(EntityState entityState)
		{
			this.StateComponent.CurState = entityState;
		}

		public void Activate()
		{
			this.StateComponent.CurState = EntityState.Idle;
			this.StateComponent.ForceUpdateAnimation = true;
		}

		protected internal AttackFSM(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).CanSwitchAbility());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).Fire();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).FireAShot(*(int*)args, *(int*)(args + 1), *(*(IntPtr*)(args + 2)), (Target)GCHandledObjects.GCHandleToObject(args[3]), (GameObject)GCHandledObjects.GCHandleToObject(args[4]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).CooldownState);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).IdleState);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).IsAttacking);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).PostFireState);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).PreFireState);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).ShooterComp);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).StateComponent);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).TransformComponent);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).TurnState);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).WarmupState);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).Initialize((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (ShooterComponent)GCHandledObjects.GCHandleToObject(args[1]), (StateComponent)GCHandledObjects.GCHandleToObject(args[2]), (TransformComponent)GCHandledObjects.GCHandleToObject(args[3]), (HealthType)(*(int*)(args + 4)));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).InStrictCoolDownState());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).IsGunSequenceDone());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).IsUnlocked());
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).CooldownState = (CooldownState)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).Entity = (SmartEntity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).IdleState = (IdleState)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).PostFireState = (PostFireState)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).PreFireState = (PreFireState)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).ShooterComp = (ShooterComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).StateComponent = (StateComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).TransformComponent = (TransformComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).TurnState = (TurnState)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).WarmupState = (WarmupState)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).SetEntityState((EntityState)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).StartAttack());
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).StopAttacking(*(sbyte*)args != 0));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((AttackFSM)GCHandledObjects.GCHandleToObject(instance)).Update();
			return -1L;
		}
	}
}
