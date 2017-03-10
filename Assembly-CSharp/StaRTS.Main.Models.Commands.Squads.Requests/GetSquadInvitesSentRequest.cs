using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class GetSquadInvitesSentRequest : PlayerIdRequest
	{
		private List<string> friendIds;

		public GetSquadInvitesSentRequest(List<string> ids)
		{
			this.friendIds = ids;
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddArrayOfPrimitives<string>("friendIds", this.friendIds);
			return serializer.End().ToString();
		}

		protected internal GetSquadInvitesSentRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadInvitesSentRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
