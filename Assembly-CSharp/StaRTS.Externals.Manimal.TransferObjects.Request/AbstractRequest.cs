using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Externals.Manimal.TransferObjects.Request
{
	public abstract class AbstractRequest : ISerializable
	{
		public abstract string ToJson();

		public ISerializable FromObject(object obj)
		{
			return null;
		}

		protected AbstractRequest()
		{
		}

		protected internal AbstractRequest(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractRequest)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
