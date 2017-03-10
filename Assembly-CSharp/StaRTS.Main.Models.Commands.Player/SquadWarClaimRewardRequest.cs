using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class SquadWarClaimRewardRequest : PlayerIdRequest
	{
		private string warID;

		public SquadWarClaimRewardRequest(string warID)
		{
			this.warID = warID;
		}

		public override string ToJson()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			base.PlayerId = currentPlayer.PlayerId;
			Serializer serializer = Serializer.Start();
			serializer.AddString("warId", this.warID);
			serializer.AddString("playerId", base.PlayerId);
			return serializer.End().ToString();
		}

		protected internal SquadWarClaimRewardRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarClaimRewardRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
