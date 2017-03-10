using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Perks
{
	public class PlayerPerkActivateRequest : PlayerIdChecksumRequest
	{
		private string perkId;

		public PlayerPerkActivateRequest(string perkId)
		{
			this.perkId = perkId;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("perkId", this.perkId);
			return startedSerializer.End().ToString();
		}

		protected internal PlayerPerkActivateRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerPerkActivateRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
