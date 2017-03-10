using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class TurretAttackController : AbstractAttackController
	{
		public const string ANIM_PROP_MOTIVATOR = "Motivation";

		public TurretAttackController()
		{
			Service.Set<TurretAttackController>(this);
		}

		protected bool IsTargetInRange(ShooterComponent shooterComp, SmartEntity target)
		{
			if (target.ShieldBorderComp != null)
			{
				return true;
			}
			int squaredDistanceToTarget = GameUtils.GetSquaredDistanceToTarget(shooterComp, target);
			return this.shooterController.InRange(squaredDistanceToTarget, shooterComp);
		}

		protected bool IsTargetAliveAndInRange(SmartEntity entity, SmartEntity target)
		{
			if (target == null)
			{
				this.OnTargetIsNull(entity);
				return false;
			}
			HealthComponent healthComp = target.HealthComp;
			if (healthComp == null || healthComp.IsDead())
			{
				this.OnTargetIsDead(entity);
				return false;
			}
			if (!this.IsTargetInRange(entity.ShooterComp, target))
			{
				this.OnTargetIsOutOfRange(entity);
				return false;
			}
			return true;
		}

		public void UpdateTurret(SmartEntity entity)
		{
			SmartEntity turretTarget = this.shooterController.GetTurretTarget(entity.ShooterComp);
			if (this.IsTargetAliveAndInRange(entity, turretTarget))
			{
				base.UpdateShooter(entity);
				Animator anim;
				if (entity.TrapViewComp != null && entity.TrapViewComp.TurretAnim != null)
				{
					anim = entity.TrapViewComp.TurretAnim;
				}
				else
				{
					anim = entity.GameObjectViewComp.MainGameObject.GetComponent<Animator>();
				}
				this.UpdateAnimationState(anim, entity.StateComp);
			}
		}

		private void StopAttackingAndStartSearching(SmartEntity entity)
		{
			if (entity.ShooterComp.AttackFSM.IsAttacking)
			{
				entity.ShooterComp.AttackFSM.StopAttacking(false);
			}
			this.StartSearch(entity);
		}

		protected override void OnTargetIsNull(SmartEntity entity)
		{
			this.StopAttackingAndStartSearching(entity);
		}

		protected override void OnTargetIsDead(SmartEntity entity)
		{
			this.StopAttackingAndStartSearching(entity);
		}

		protected void OnTargetIsOutOfRange(SmartEntity entity)
		{
			this.StopAttackingAndStartSearching(entity);
		}

		protected override void OnBeforeAttack(SmartEntity entity)
		{
		}

		protected override void OnAttackBegin(SmartEntity entity)
		{
		}

		protected override void StartSearch(SmartEntity entity)
		{
			entity.ShooterComp.Searching = true;
			entity.ShooterComp.Target = null;
			entity.TurretShooterComp.TargetWeight = -1;
		}

		public void UpdateAnimationState(Animator anim, StateComponent stateComp)
		{
			if (stateComp.Dirty)
			{
				if (anim == null)
				{
					return;
				}
				if (!anim.gameObject.activeInHierarchy)
				{
					return;
				}
				while (stateComp.Dirty)
				{
					EntityState entityState = stateComp.DequeuePrevState();
					if (entityState == EntityState.AttackingReset)
					{
						anim.Play("", 0, 0f);
					}
				}
				switch (stateComp.CurState)
				{
				case EntityState.Idle:
					anim.SetInteger("Motivation", 0);
					return;
				case EntityState.Moving:
					anim.SetInteger("Motivation", 1);
					return;
				case EntityState.Tracking:
					break;
				case EntityState.Turning:
					anim.SetInteger("Motivation", 1);
					return;
				case EntityState.WarmingUp:
					anim.SetInteger("Motivation", 4);
					return;
				case EntityState.Attacking:
				case EntityState.AttackingReset:
					anim.SetInteger("Motivation", 3);
					return;
				case EntityState.CoolingDown:
					anim.SetInteger("Motivation", 4);
					return;
				case EntityState.Dying:
					anim.SetInteger("Motivation", 2);
					break;
				default:
					return;
				}
			}
		}

		protected internal TurretAttackController(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretAttackController)GCHandledObjects.GCHandleToObject(instance)).IsTargetAliveAndInRange((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretAttackController)GCHandledObjects.GCHandleToObject(instance)).IsTargetInRange((ShooterComponent)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TurretAttackController)GCHandledObjects.GCHandleToObject(instance)).OnAttackBegin((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TurretAttackController)GCHandledObjects.GCHandleToObject(instance)).OnBeforeAttack((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TurretAttackController)GCHandledObjects.GCHandleToObject(instance)).OnTargetIsDead((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TurretAttackController)GCHandledObjects.GCHandleToObject(instance)).OnTargetIsNull((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TurretAttackController)GCHandledObjects.GCHandleToObject(instance)).OnTargetIsOutOfRange((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TurretAttackController)GCHandledObjects.GCHandleToObject(instance)).StartSearch((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TurretAttackController)GCHandledObjects.GCHandleToObject(instance)).StopAttackingAndStartSearching((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TurretAttackController)GCHandledObjects.GCHandleToObject(instance)).UpdateAnimationState((Animator)GCHandledObjects.GCHandleToObject(*args), (StateComponent)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((TurretAttackController)GCHandledObjects.GCHandleToObject(instance)).UpdateTurret((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
