using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.ServerMessages
{
	public abstract class AbstractMessage : IMessage, ISerializable
	{
		public abstract EventId MessageEventId
		{
			get;
		}

		public abstract object MessageCookie
		{
			get;
		}

		public abstract ISerializable FromObject(object obj);

		public string ToJson()
		{
			return string.Empty;
		}

		protected AbstractMessage()
		{
		}

		protected internal AbstractMessage(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractMessage)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractMessage)GCHandledObjects.GCHandleToObject(instance)).MessageCookie);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractMessage)GCHandledObjects.GCHandleToObject(instance)).MessageEventId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractMessage)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
