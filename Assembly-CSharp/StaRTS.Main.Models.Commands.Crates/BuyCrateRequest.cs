using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Crates
{
	public class BuyCrateRequest : PlayerIdChecksumRequest
	{
		private string crateId;

		public BuyCrateRequest(string crateId)
		{
			this.crateId = crateId;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("crateId", this.crateId);
			return startedSerializer.End().ToString();
		}

		protected internal BuyCrateRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuyCrateRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
