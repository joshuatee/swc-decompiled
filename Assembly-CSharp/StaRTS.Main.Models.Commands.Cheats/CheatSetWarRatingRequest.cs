using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatSetWarRatingRequest : PlayerIdRequest
	{
		public string SquadId;

		public int Rating
		{
			get;
			private set;
		}

		public CheatSetWarRatingRequest(string squadId, int rating)
		{
			this.SquadId = squadId;
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.Rating = rating;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("guildId", this.SquadId);
			serializer.AddString("rating", this.Rating.ToString());
			return serializer.End().ToString();
		}

		protected internal CheatSetWarRatingRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetWarRatingRequest)GCHandledObjects.GCHandleToObject(instance)).Rating);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CheatSetWarRatingRequest)GCHandledObjects.GCHandleToObject(instance)).Rating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatSetWarRatingRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
