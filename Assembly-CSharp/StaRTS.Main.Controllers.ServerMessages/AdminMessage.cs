using StaRTS.Main.Models;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers.ServerMessages
{
	public class AdminMessage : AbstractMessage
	{
		private const string KEY_CONTENT = "content";

		private const string KEY_FLAGS = "flags";

		private const string KEY_MESSAGE = "message";

		public List<AdminMessageData> Messages
		{
			get;
			private set;
		}

		public override object MessageCookie
		{
			get
			{
				return this;
			}
		}

		public override EventId MessageEventId
		{
			get
			{
				return EventId.ServerAdminMessage;
			}
		}

		public AdminMessage()
		{
			this.Messages = new List<AdminMessageData>();
		}

		public override ISerializable FromObject(object obj)
		{
			List<object> list = (List<object>)obj;
			for (int i = 0; i < list.Count; i++)
			{
				Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
				Dictionary<string, object> dictionary2 = dictionary["message"] as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					foreach (KeyValuePair<string, object> current in dictionary2)
					{
						string key = current.get_Key();
						Dictionary<string, object> dictionary3 = current.get_Value() as Dictionary<string, object>;
						string message = (string)dictionary3["content"];
						bool isCritical = Convert.ToInt32(dictionary3["flags"], CultureInfo.InvariantCulture) == 1;
						this.Messages.Add(new AdminMessageData(key, message, isCritical));
					}
				}
			}
			return this;
		}

		protected internal AdminMessage(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AdminMessage)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AdminMessage)GCHandledObjects.GCHandleToObject(instance)).MessageCookie);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AdminMessage)GCHandledObjects.GCHandleToObject(instance)).MessageEventId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AdminMessage)GCHandledObjects.GCHandleToObject(instance)).Messages);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AdminMessage)GCHandledObjects.GCHandleToObject(instance)).Messages = (List<AdminMessageData>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
