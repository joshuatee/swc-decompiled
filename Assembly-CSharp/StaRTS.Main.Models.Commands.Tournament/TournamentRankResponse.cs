using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Tournament
{
	public class TournamentRankResponse : AbstractResponse
	{
		public TournamentRank Rank
		{
			get;
			private set;
		}

		public Tournament TournamentData
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			this.Rank = new TournamentRank();
			this.Rank.FromObject(obj);
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("tournament"))
			{
				this.TournamentData = new Tournament();
				this.TournamentData.FromObject(dictionary["tournament"]);
			}
			return this;
		}

		public TournamentRankResponse()
		{
		}

		protected internal TournamentRankResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRankResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRankResponse)GCHandledObjects.GCHandleToObject(instance)).Rank);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRankResponse)GCHandledObjects.GCHandleToObject(instance)).TournamentData);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TournamentRankResponse)GCHandledObjects.GCHandleToObject(instance)).Rank = (TournamentRank)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TournamentRankResponse)GCHandledObjects.GCHandleToObject(instance)).TournamentData = (Tournament)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
