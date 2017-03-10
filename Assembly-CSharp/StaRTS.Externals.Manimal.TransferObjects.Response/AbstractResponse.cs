using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Externals.Manimal.TransferObjects.Response
{
	public abstract class AbstractResponse : ISerializable
	{
		public abstract ISerializable FromObject(object obj);

		public string ToJson()
		{
			return string.Empty;
		}

		protected AbstractResponse()
		{
		}

		protected internal AbstractResponse(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractResponse)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
