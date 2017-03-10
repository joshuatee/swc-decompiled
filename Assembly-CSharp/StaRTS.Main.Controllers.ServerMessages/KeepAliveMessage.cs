using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers.ServerMessages
{
	public class KeepAliveMessage : AbstractMessage
	{
		private const string KEEP_ALIVE_TIME = "keepAliveTime";

		private const string MESSAGE = "message";

		private uint keepAliveTime;

		public override object MessageCookie
		{
			get
			{
				return this.keepAliveTime;
			}
		}

		public override EventId MessageEventId
		{
			get
			{
				return EventId.Nop;
			}
		}

		public override ISerializable FromObject(object obj)
		{
			List<object> list = obj as List<object>;
			for (int i = 0; i < list.Count; i++)
			{
				Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
				Dictionary<string, object> dictionary2 = dictionary["message"] as Dictionary<string, object>;
				this.keepAliveTime = Convert.ToUInt32(dictionary2["keepAliveTime"], CultureInfo.InvariantCulture);
			}
			return this;
		}

		public KeepAliveMessage()
		{
		}

		protected internal KeepAliveMessage(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((KeepAliveMessage)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((KeepAliveMessage)GCHandledObjects.GCHandleToObject(instance)).MessageCookie);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((KeepAliveMessage)GCHandledObjects.GCHandleToObject(instance)).MessageEventId);
		}
	}
}
