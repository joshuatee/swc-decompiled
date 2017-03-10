using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Campaign
{
	public class ClaimCampaignRequest : PlayerIdChecksumRequest
	{
		private string campaignUid;

		private string lastMissionCompletedUid;

		public ClaimCampaignRequest(string campaignUid, string lastMissionCompletedUid)
		{
			this.campaignUid = campaignUid;
			this.lastMissionCompletedUid = lastMissionCompletedUid;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("campaignUid", this.campaignUid);
			startedSerializer.AddString("missionUid", this.lastMissionCompletedUid);
			return startedSerializer.End().ToString();
		}

		protected internal ClaimCampaignRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ClaimCampaignRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
