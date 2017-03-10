using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Tournament
{
	public class TournamentResponse : AbstractResponse
	{
		private const string TOURNAMENTS = "tournaments";

		private const string CRATES = "crateInventory";

		private const string REWARDS = "newCrateUids";

		public List<Tournament> TournamentsData
		{
			get;
			private set;
		}

		public Dictionary<string, object> CratesJsonData
		{
			get;
			private set;
		}

		public List<string> Rewards
		{
			get;
			private set;
		}

		public TournamentResponse()
		{
			this.TournamentsData = new List<Tournament>();
			this.CratesJsonData = new Dictionary<string, object>();
			this.Rewards = new List<string>();
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return this;
			}
			List<object> list = null;
			if (dictionary.ContainsKey("tournaments"))
			{
				list = (dictionary["tournaments"] as List<object>);
			}
			if (list == null || list.Count == 0)
			{
				return this;
			}
			foreach (object current in list)
			{
				Tournament tournament = new Tournament();
				tournament.FromObject(current);
				tournament.FinalRank.FromObject(current);
				this.TournamentsData.Add(tournament);
			}
			Dictionary<string, object> dictionary2 = null;
			if (dictionary.ContainsKey("crateInventory"))
			{
				dictionary2 = (dictionary["crateInventory"] as Dictionary<string, object>);
			}
			if (dictionary2 == null)
			{
				return this;
			}
			this.CratesJsonData = dictionary2;
			List<object> list2 = null;
			if (dictionary.ContainsKey("newCrateUids"))
			{
				list2 = (dictionary["newCrateUids"] as List<object>);
			}
			if (list2 == null)
			{
				return this;
			}
			foreach (object current2 in list2)
			{
				this.Rewards.Add((string)current2);
			}
			return this;
		}

		protected internal TournamentResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentResponse)GCHandledObjects.GCHandleToObject(instance)).CratesJsonData);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentResponse)GCHandledObjects.GCHandleToObject(instance)).Rewards);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentResponse)GCHandledObjects.GCHandleToObject(instance)).TournamentsData);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TournamentResponse)GCHandledObjects.GCHandleToObject(instance)).CratesJsonData = (Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TournamentResponse)GCHandledObjects.GCHandleToObject(instance)).Rewards = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TournamentResponse)GCHandledObjects.GCHandleToObject(instance)).TournamentsData = (List<Tournament>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
