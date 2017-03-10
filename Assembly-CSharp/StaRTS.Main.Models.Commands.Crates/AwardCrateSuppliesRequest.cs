using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Crates
{
	public class AwardCrateSuppliesRequest : PlayerIdChecksumRequest
	{
		private string crateUId;

		public AwardCrateSuppliesRequest(string crateUId)
		{
			this.crateUId = crateUId;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("crateUid", this.crateUId);
			return startedSerializer.End().ToString();
		}

		protected internal AwardCrateSuppliesRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AwardCrateSuppliesRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
