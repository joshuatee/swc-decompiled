using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.TargetedBundleOffers
{
	public class BuyTargetedOfferRequest : PlayerIdChecksumRequest
	{
		private string offerUid;

		public BuyTargetedOfferRequest(string offerUid)
		{
			this.offerUid = offerUid;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("offerUid", this.offerUid);
			return startedSerializer.End().ToString();
		}

		protected internal BuyTargetedOfferRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuyTargetedOfferRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
