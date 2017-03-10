using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.ServerMessages
{
	public class SquadServerMessage : AbstractMessage
	{
		public List<object> Messages
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
				return EventId.SquadServerMessage;
			}
		}

		public override ISerializable FromObject(object obj)
		{
			List<object> list = obj as List<object>;
			if (list == null)
			{
				return this;
			}
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
				if (dictionary != null && dictionary.ContainsKey("message"))
				{
					if (this.Messages == null)
					{
						this.Messages = new List<object>();
					}
					this.Messages.Add(dictionary["message"]);
				}
				i++;
			}
			return this;
		}

		public SquadServerMessage()
		{
		}

		protected internal SquadServerMessage(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadServerMessage)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadServerMessage)GCHandledObjects.GCHandleToObject(instance)).MessageCookie);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadServerMessage)GCHandledObjects.GCHandleToObject(instance)).MessageEventId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadServerMessage)GCHandledObjects.GCHandleToObject(instance)).Messages);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadServerMessage)GCHandledObjects.GCHandleToObject(instance)).Messages = (List<object>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
