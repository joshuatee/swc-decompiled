using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Raids
{
	public class RaidUpdateRequest : PlayerIdChecksumRequest
	{
		private string planetId;

		public RaidUpdateRequest(string planetId)
		{
			this.planetId = planetId;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("planetId", this.planetId);
			return startedSerializer.End().ToString();
		}

		protected internal RaidUpdateRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidUpdateRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
