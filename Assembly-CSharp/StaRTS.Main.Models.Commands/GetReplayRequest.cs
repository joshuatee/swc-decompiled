using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands
{
	public class GetReplayRequest : PlayerIdRequest
	{
		private string battleId;

		private string participantId;

		public GetReplayRequest(string playerId, string battleId, string participantId)
		{
			base.PlayerId = playerId;
			this.battleId = battleId;
			this.participantId = participantId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("battleId", this.battleId);
			serializer.AddString("participantId", this.participantId);
			return serializer.End().ToString();
		}

		protected internal GetReplayRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetReplayRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
