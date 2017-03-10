using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.StateMachines
{
	public class TimeLockedStateMachine : StateMachine, ISimTimeProvider
	{
		private ISimTimeProvider time;

		private TimeLockedState curTimeLockedState;

		private Type prevTimeLockedStateType;

		public uint Now
		{
			get
			{
				return this.time.Now;
			}
		}

		public ISimTimeProvider TimeProvider
		{
			get
			{
				return this.time;
			}
			set
			{
				this.time = value;
			}
		}

		public new TimeLockedState CurrentState
		{
			get
			{
				return this.curTimeLockedState;
			}
		}

		public new Type PreviousStateType
		{
			get
			{
				return this.prevTimeLockedStateType;
			}
		}

		public TimeLockedStateMachine(ISimTimeProvider timeProvider)
		{
			this.time = timeProvider;
		}

		public virtual bool SetState(TimeLockedState timeLockedState)
		{
			TimeLockedState timeLockedState2 = this.curTimeLockedState;
			if (base.SetState(timeLockedState))
			{
				this.prevTimeLockedStateType = ((timeLockedState2 == null) ? null : timeLockedState2.GetType());
				this.curTimeLockedState = timeLockedState;
				return true;
			}
			return false;
		}

		protected internal TimeLockedStateMachine(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TimeLockedStateMachine)GCHandledObjects.GCHandleToObject(instance)).CurrentState);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TimeLockedStateMachine)GCHandledObjects.GCHandleToObject(instance)).PreviousStateType);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TimeLockedStateMachine)GCHandledObjects.GCHandleToObject(instance)).TimeProvider);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TimeLockedStateMachine)GCHandledObjects.GCHandleToObject(instance)).TimeProvider = (ISimTimeProvider)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TimeLockedStateMachine)GCHandledObjects.GCHandleToObject(instance)).SetState((TimeLockedState)GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
