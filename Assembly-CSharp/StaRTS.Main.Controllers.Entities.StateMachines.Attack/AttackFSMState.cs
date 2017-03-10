using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.StateMachines.Attack
{
	public abstract class AttackFSMState : TimeLockedState
	{
		public AttackFSM AttackFSMOwner
		{
			get;
			protected set;
		}

		public ShooterComponent ShooterComp
		{
			get;
			protected set;
		}

		public AttackFSMState(AttackFSM owner, uint lockDuration) : base(owner, lockDuration)
		{
			this.AttackFSMOwner = owner;
			this.ShooterComp = owner.ShooterComp;
		}

		public override void OnEnter()
		{
			base.NextUnlockTime = base.Owner.Now + base.LockDuration;
		}

		protected internal AttackFSMState(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSMState)GCHandledObjects.GCHandleToObject(instance)).AttackFSMOwner);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AttackFSMState)GCHandledObjects.GCHandleToObject(instance)).ShooterComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AttackFSMState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AttackFSMState)GCHandledObjects.GCHandleToObject(instance)).AttackFSMOwner = (AttackFSM)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AttackFSMState)GCHandledObjects.GCHandleToObject(instance)).ShooterComp = (ShooterComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
