using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Crates
{
	public class BuyLimitedEditionItemRequest : PlayerIdChecksumRequest
	{
		private string leiUid;

		public BuyLimitedEditionItemRequest(string leiUid)
		{
			this.leiUid = leiUid;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("leUid", this.leiUid);
			return startedSerializer.End().ToString();
		}

		protected internal BuyLimitedEditionItemRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuyLimitedEditionItemRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
