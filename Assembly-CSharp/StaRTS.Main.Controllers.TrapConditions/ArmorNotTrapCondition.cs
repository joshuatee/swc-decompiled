using StaRTS.Main.Models;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.TrapConditions
{
	public class ArmorNotTrapCondition : TrapCondition
	{
		public List<ArmorType> IntruderArmorTypes
		{
			get;
			private set;
		}

		public ArmorNotTrapCondition(List<ArmorType> intruderArmorTypes)
		{
			this.IntruderArmorTypes = intruderArmorTypes;
		}

		protected internal ArmorNotTrapCondition(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmorNotTrapCondition)GCHandledObjects.GCHandleToObject(instance)).IntruderArmorTypes);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ArmorNotTrapCondition)GCHandledObjects.GCHandleToObject(instance)).IntruderArmorTypes = (List<ArmorType>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
