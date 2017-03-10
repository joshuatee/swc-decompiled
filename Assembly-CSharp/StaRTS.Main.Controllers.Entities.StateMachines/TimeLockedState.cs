using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.StateMachines
{
	public abstract class TimeLockedState : IState
	{
		public TimeLockedStateMachine Owner
		{
			get;
			protected set;
		}

		public uint LockDuration
		{
			get;
			protected set;
		}

		public uint NextUnlockTime
		{
			get;
			protected set;
		}

		public TimeLockedState(TimeLockedStateMachine owner, uint lockDuration)
		{
			this.Owner = owner;
			this.SetDefaultLockDuration(lockDuration);
		}

		public void ResetLock()
		{
			this.ForceUnlock();
		}

		public virtual void OnEnter()
		{
			this.NextUnlockTime = this.Owner.Now + this.LockDuration;
		}

		public virtual void OnExit(IState nextState)
		{
			this.ResetLock();
		}

		public void SetDefaultLockDuration(uint lockDuration)
		{
			this.LockDuration = lockDuration;
		}

		public virtual bool IsUnlocked()
		{
			return this.Owner.Now >= this.NextUnlockTime;
		}

		public void ForceUnlock()
		{
			this.NextUnlockTime = 0u;
		}

		protected internal TimeLockedState(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TimeLockedState)GCHandledObjects.GCHandleToObject(instance)).ForceUnlock();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TimeLockedState)GCHandledObjects.GCHandleToObject(instance)).Owner);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TimeLockedState)GCHandledObjects.GCHandleToObject(instance)).IsUnlocked());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TimeLockedState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TimeLockedState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TimeLockedState)GCHandledObjects.GCHandleToObject(instance)).ResetLock();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TimeLockedState)GCHandledObjects.GCHandleToObject(instance)).Owner = (TimeLockedStateMachine)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
