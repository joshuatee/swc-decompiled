using StaRTS.Main.Models;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.TrapConditions
{
	public class IntruderTypeTrapCondition : TrapCondition
	{
		public TroopType IntruderType
		{
			get;
			private set;
		}

		public IntruderTypeTrapCondition(TroopType intruderType)
		{
			this.IntruderType = intruderType;
		}

		protected internal IntruderTypeTrapCondition(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IntruderTypeTrapCondition)GCHandledObjects.GCHandleToObject(instance)).IntruderType);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IntruderTypeTrapCondition)GCHandledObjects.GCHandleToObject(instance)).IntruderType = (TroopType)(*(int*)args);
			return -1L;
		}
	}
}
