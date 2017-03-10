using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Shared;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class StateComponent : ComponentBase
	{
		public EntityState RawState;

		private Queue<EntityState> prevStates;

		public bool IsRunning
		{
			get;
			set;
		}

		public bool ForceUpdateAnimation
		{
			get;
			set;
		}

		public int DeathAnimationID
		{
			get;
			set;
		}

		public bool Dirty
		{
			get
			{
				return this.prevStates.Count != 0;
			}
		}

		public EntityState CurState
		{
			get
			{
				return this.RawState;
			}
			set
			{
				if (value != EntityState.Moving)
				{
					this.IsRunning = false;
				}
				if (this.RawState != value)
				{
					this.prevStates.Enqueue(this.RawState);
					this.RawState = value;
				}
			}
		}

		public StateComponent(Entity entity)
		{
			this.prevStates = new Queue<EntityState>();
			this.Entity = entity;
			this.Reset();
		}

		public void Reset()
		{
			this.RawState = EntityState.Idle;
			this.IsRunning = false;
			this.ForceUpdateAnimation = false;
			this.prevStates.Clear();
			this.DeathAnimationID = 2;
		}

		public EntityState DequeuePrevState()
		{
			return this.prevStates.Dequeue();
		}

		protected internal StateComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StateComponent)GCHandledObjects.GCHandleToObject(instance)).DequeuePrevState());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StateComponent)GCHandledObjects.GCHandleToObject(instance)).CurState);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StateComponent)GCHandledObjects.GCHandleToObject(instance)).DeathAnimationID);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StateComponent)GCHandledObjects.GCHandleToObject(instance)).Dirty);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StateComponent)GCHandledObjects.GCHandleToObject(instance)).ForceUpdateAnimation);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StateComponent)GCHandledObjects.GCHandleToObject(instance)).IsRunning);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((StateComponent)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((StateComponent)GCHandledObjects.GCHandleToObject(instance)).CurState = (EntityState)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((StateComponent)GCHandledObjects.GCHandleToObject(instance)).DeathAnimationID = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((StateComponent)GCHandledObjects.GCHandleToObject(instance)).ForceUpdateAnimation = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((StateComponent)GCHandledObjects.GCHandleToObject(instance)).IsRunning = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
