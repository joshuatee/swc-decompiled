using StaRTS.Main.Controllers.Entities.StateMachines.Attack;
using StaRTS.Main.Models.Entities;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public abstract class AbstractAttackController
	{
		protected ShooterController shooterController;

		protected BoardController boardController;

		public AbstractAttackController()
		{
			this.shooterController = Service.Get<ShooterController>();
			this.boardController = Service.Get<BoardController>();
		}

		protected void UpdateShooter(SmartEntity entity)
		{
			this.TryStartAttack(entity);
			this.UpdateAttackFSM(entity.ShooterComp.AttackFSM);
		}

		private void TryStartAttack(SmartEntity entity)
		{
			if (entity.ShooterComp.AttackFSM.IsAttacking)
			{
				return;
			}
			this.OnBeforeAttack(entity);
			bool flag = entity.ShooterComp.AttackFSM.StartAttack();
			if (flag)
			{
				this.shooterController.Reload(entity.ShooterComp);
				this.OnAttackBegin(entity);
			}
		}

		private void UpdateAttackFSM(AttackFSM attackFSM)
		{
			if (!attackFSM.IsAttacking)
			{
				return;
			}
			if (attackFSM.IsUnlocked())
			{
				attackFSM.Update();
			}
		}

		protected abstract void OnTargetIsNull(SmartEntity entity);

		protected abstract void OnTargetIsDead(SmartEntity entity);

		protected abstract void OnBeforeAttack(SmartEntity entity);

		protected abstract void OnAttackBegin(SmartEntity entity);

		protected abstract void StartSearch(SmartEntity entity);

		protected internal AbstractAttackController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractAttackController)GCHandledObjects.GCHandleToObject(instance)).OnAttackBegin((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractAttackController)GCHandledObjects.GCHandleToObject(instance)).OnBeforeAttack((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AbstractAttackController)GCHandledObjects.GCHandleToObject(instance)).OnTargetIsDead((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractAttackController)GCHandledObjects.GCHandleToObject(instance)).OnTargetIsNull((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AbstractAttackController)GCHandledObjects.GCHandleToObject(instance)).StartSearch((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AbstractAttackController)GCHandledObjects.GCHandleToObject(instance)).TryStartAttack((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AbstractAttackController)GCHandledObjects.GCHandleToObject(instance)).UpdateAttackFSM((AttackFSM)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AbstractAttackController)GCHandledObjects.GCHandleToObject(instance)).UpdateShooter((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
