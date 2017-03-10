using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class SendSquadInviteRequest : PlayerIdRequest
	{
		private string recipientPlayerId;

		private string fbFriendId;

		private string accessToken;

		public SendSquadInviteRequest(string playerId, string recipientPlayerId, string fbFriendId, string fbAccessToken)
		{
			base.PlayerId = playerId;
			this.recipientPlayerId = recipientPlayerId;
			this.fbFriendId = fbFriendId;
			this.accessToken = fbAccessToken;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("recipientPlayerId", this.recipientPlayerId);
			if (this.fbFriendId != null)
			{
				serializer.AddString("fbFriendId", this.fbFriendId);
			}
			if (this.accessToken != null)
			{
				serializer.AddString("accessToken", this.accessToken);
			}
			return serializer.End().ToString();
		}

		protected internal SendSquadInviteRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SendSquadInviteRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
