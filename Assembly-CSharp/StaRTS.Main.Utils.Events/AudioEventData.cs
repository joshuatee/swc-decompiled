using System;
using WinRTBridge;

namespace StaRTS.Main.Utils.Events
{
	public class AudioEventData
	{
		public EventId EventId
		{
			get;
			set;
		}

		public object EventCookie
		{
			get;
			set;
		}

		public AudioEventData(EventId eventId, string eventCookie)
		{
			this.EventId = eventId;
			this.EventCookie = eventCookie;
		}

		protected internal AudioEventData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AudioEventData)GCHandledObjects.GCHandleToObject(instance)).EventCookie);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AudioEventData)GCHandledObjects.GCHandleToObject(instance)).EventId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AudioEventData)GCHandledObjects.GCHandleToObject(instance)).EventCookie = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AudioEventData)GCHandledObjects.GCHandleToObject(instance)).EventId = (EventId)(*(int*)args);
			return -1L;
		}
	}
}
