using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Externals.Manimal.TransferObjects.Request
{
	public class DefaultRequest : AbstractRequest
	{
		public override string ToJson()
		{
			return Serializer.Start().End().ToString();
		}

		public DefaultRequest()
		{
		}

		protected internal DefaultRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
