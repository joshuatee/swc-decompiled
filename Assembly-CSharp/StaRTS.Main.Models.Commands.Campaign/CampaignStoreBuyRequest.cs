using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Campaign
{
	public class CampaignStoreBuyRequest : PlayerIdChecksumRequest
	{
		private string uid;

		private string campaignUid;

		public CampaignStoreBuyRequest(string campaignUid, string campaignStoreUid)
		{
			this.uid = campaignStoreUid;
			this.campaignUid = campaignUid;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("uid", this.uid);
			startedSerializer.AddString("campaignUid", this.campaignUid);
			startedSerializer.Add<int>("count", 1);
			return startedSerializer.End().ToString();
		}

		protected internal CampaignStoreBuyRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignStoreBuyRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
