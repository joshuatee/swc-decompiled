using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Campaign
{
	public class CampaignIdRequest : PlayerIdChecksumRequest
	{
		private string campaignUid;

		public CampaignIdRequest(string campaignUid)
		{
			this.campaignUid = campaignUid;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("campaignUid", this.campaignUid);
			return startedSerializer.End().ToString();
		}

		protected internal CampaignIdRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CampaignIdRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
