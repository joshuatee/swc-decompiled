using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Perks
{
	public class PlayerPerkSkipCooldownRequest : PlayerIdChecksumRequest
	{
		private string perkId;

		public PlayerPerkSkipCooldownRequest(string perkId)
		{
			this.perkId = perkId;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("perkId", this.perkId);
			return startedSerializer.End().ToString();
		}

		protected internal PlayerPerkSkipCooldownRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerPerkSkipCooldownRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
