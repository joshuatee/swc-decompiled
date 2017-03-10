using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Commands.Tournament;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Misc
{
	public class TournamentProgress : ISerializable
	{
		private Dictionary<string, Tournament> tournaments;

		public TournamentProgress()
		{
			this.tournaments = new Dictionary<string, Tournament>();
		}

		public bool HasTournament(TournamentVO tournamentVO)
		{
			return this.tournaments.ContainsKey(tournamentVO.Uid);
		}

		public void AddTournament(string uid, Tournament tournament)
		{
			this.tournaments.Add(uid, tournament);
		}

		public Tournament GetTournament(string uid)
		{
			Tournament result = null;
			if (this.tournaments.ContainsKey(uid))
			{
				result = this.tournaments[uid];
			}
			return result;
		}

		public TournamentRank GetTournamentCurrentRank(string uid)
		{
			if (this.tournaments.ContainsKey(uid))
			{
				Tournament tournament = this.tournaments[uid];
				return tournament.CurrentRank;
			}
			return null;
		}

		public TournamentRank GetTournamentFinalRank(string uid)
		{
			if (this.tournaments.ContainsKey(uid))
			{
				Tournament tournament = this.tournaments[uid];
				return tournament.FinalRank;
			}
			return null;
		}

		public AbstractTimedEvent GetTimedEvent(string eventUid)
		{
			if (this.tournaments.ContainsKey(eventUid))
			{
				return this.tournaments[eventUid];
			}
			return null;
		}

		public string ToJson()
		{
			return "{}";
		}

		public void RemoveMissingTournamentData()
		{
			IDataController dataController = Service.Get<IDataController>();
			List<string> list = new List<string>();
			foreach (string current in this.tournaments.Keys)
			{
				if (dataController.GetOptional<TournamentVO>(current) == null)
				{
					list.Add(current);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.tournaments.Remove(list[i]);
			}
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("tournaments"))
			{
				Dictionary<string, object> dictionary2 = dictionary["tournaments"] as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					foreach (KeyValuePair<string, object> current in dictionary2)
					{
						Tournament tournament = new Tournament();
						tournament.FromObject(current.get_Value());
						this.tournaments.Add(current.get_Key(), tournament);
					}
				}
			}
			return this;
		}

		protected internal TournamentProgress(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TournamentProgress)GCHandledObjects.GCHandleToObject(instance)).AddTournament(Marshal.PtrToStringUni(*(IntPtr*)args), (Tournament)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentProgress)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentProgress)GCHandledObjects.GCHandleToObject(instance)).GetTimedEvent(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentProgress)GCHandledObjects.GCHandleToObject(instance)).GetTournament(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentProgress)GCHandledObjects.GCHandleToObject(instance)).GetTournamentCurrentRank(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentProgress)GCHandledObjects.GCHandleToObject(instance)).GetTournamentFinalRank(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentProgress)GCHandledObjects.GCHandleToObject(instance)).HasTournament((TournamentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TournamentProgress)GCHandledObjects.GCHandleToObject(instance)).RemoveMissingTournamentData();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentProgress)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
