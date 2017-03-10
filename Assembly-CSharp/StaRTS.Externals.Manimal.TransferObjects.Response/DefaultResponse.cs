using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Externals.Manimal.TransferObjects.Response
{
	public class DefaultResponse : AbstractResponse
	{
		public override ISerializable FromObject(object obj)
		{
			return this;
		}

		public DefaultResponse()
		{
		}

		protected internal DefaultResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
