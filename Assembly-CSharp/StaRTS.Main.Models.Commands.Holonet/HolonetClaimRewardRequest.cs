using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Holonet
{
	public class HolonetClaimRewardRequest : PlayerIdChecksumRequest
	{
		private string limitedTimeRewardUid;

		private string contextUid;

		public HolonetClaimRewardRequest(string limitedTimeRewardUid, string contextUid)
		{
			this.limitedTimeRewardUid = limitedTimeRewardUid;
			this.contextUid = contextUid;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("uid", this.limitedTimeRewardUid);
			startedSerializer.AddString("rewardContext", this.contextUid);
			return startedSerializer.End().ToString();
		}

		protected internal HolonetClaimRewardRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolonetClaimRewardRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
