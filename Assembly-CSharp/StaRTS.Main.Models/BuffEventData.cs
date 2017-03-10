using StaRTS.Main.Models.Entities;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class BuffEventData
	{
		public Buff BuffObj
		{
			get;
			private set;
		}

		public SmartEntity Target
		{
			get;
			private set;
		}

		public BuffEventData(Buff buff, SmartEntity target)
		{
			this.BuffObj = buff;
			this.Target = target;
		}

		protected internal BuffEventData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffEventData)GCHandledObjects.GCHandleToObject(instance)).BuffObj);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffEventData)GCHandledObjects.GCHandleToObject(instance)).Target);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BuffEventData)GCHandledObjects.GCHandleToObject(instance)).BuffObj = (Buff)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BuffEventData)GCHandledObjects.GCHandleToObject(instance)).Target = (SmartEntity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
