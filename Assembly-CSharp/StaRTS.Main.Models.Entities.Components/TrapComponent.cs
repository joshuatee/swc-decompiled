using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class TrapComponent : ComponentBase
	{
		public TrapTypeVO Type;

		private TrapState state;

		public TrapState PreviousState
		{
			get;
			private set;
		}

		public TrapState CurrentState
		{
			get
			{
				return this.state;
			}
			set
			{
				this.PreviousState = this.state;
				this.state = value;
			}
		}

		public TrapComponent(TrapTypeVO type, TrapState state)
		{
			this.Type = type;
			this.state = state;
			this.PreviousState = TrapState.Spent;
		}

		protected internal TrapComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapComponent)GCHandledObjects.GCHandleToObject(instance)).CurrentState);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapComponent)GCHandledObjects.GCHandleToObject(instance)).PreviousState);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TrapComponent)GCHandledObjects.GCHandleToObject(instance)).CurrentState = (TrapState)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TrapComponent)GCHandledObjects.GCHandleToObject(instance)).PreviousState = (TrapState)(*(int*)args);
			return -1L;
		}
	}
}
