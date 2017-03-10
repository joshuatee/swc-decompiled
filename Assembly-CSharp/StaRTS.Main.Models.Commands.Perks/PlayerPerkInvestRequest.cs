using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Perks
{
	public class PlayerPerkInvestRequest : PlayerIdChecksumRequest
	{
		private string perkId;

		private int repToInvest;

		public PlayerPerkInvestRequest(string perkId, int amount)
		{
			this.perkId = perkId;
			this.repToInvest = amount;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("perkId", this.perkId);
			startedSerializer.AddString("repToInvest", this.repToInvest.ToString());
			return startedSerializer.End().ToString();
		}

		protected internal PlayerPerkInvestRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerPerkInvestRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
