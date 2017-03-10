using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Perks
{
	public class PlayerPerkCancelRequest : PlayerIdChecksumRequest
	{
		private string perkId;

		public PlayerPerkCancelRequest(string perkId)
		{
			this.perkId = perkId;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("perkId", this.perkId);
			return startedSerializer.End().ToString();
		}

		protected internal PlayerPerkCancelRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerPerkCancelRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
