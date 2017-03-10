using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Raids
{
	public class RaidDefenseStartRequest : PlayerIdChecksumRequest
	{
		private string planetId;

		private string raidMissionId;

		public RaidDefenseStartRequest(string planetId, string raidMissionId)
		{
			this.planetId = planetId;
			this.raidMissionId = raidMissionId;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("planetId", this.planetId);
			startedSerializer.AddString("raidMissionId", this.raidMissionId);
			return startedSerializer.End().ToString();
		}

		protected internal RaidDefenseStartRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseStartRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
