using StaRTS.Main.Utils.Events;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class HoloCommand
	{
		public EventId EventId
		{
			get;
			set;
		}

		public object Cookie
		{
			get;
			set;
		}

		public HoloCommand()
		{
		}

		protected internal HoloCommand(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HoloCommand)GCHandledObjects.GCHandleToObject(instance)).Cookie);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HoloCommand)GCHandledObjects.GCHandleToObject(instance)).EventId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HoloCommand)GCHandledObjects.GCHandleToObject(instance)).Cookie = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HoloCommand)GCHandledObjects.GCHandleToObject(instance)).EventId = (EventId)(*(int*)args);
			return -1L;
		}
	}
}
