using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Squads
{
	public class SquadWarHistoryEntry : ISerializable, IComparable<SquadWarHistoryEntry>
	{
		public string WarId;

		public uint EndDate;

		public int Score;

		public int OpponentScore;

		public string OpponentName;

		public string OpponentIcon;

		public SquadWarHistoryEntry()
		{
			this.WarId = string.Empty;
			this.EndDate = 0u;
			this.Score = 0;
			this.OpponentScore = 0;
			this.OpponentName = string.Empty;
			this.OpponentIcon = string.Empty;
		}

		public int CompareTo(SquadWarHistoryEntry compareHistory)
		{
			if (compareHistory == null)
			{
				return -1;
			}
			return compareHistory.EndDate.CompareTo(this.EndDate);
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("warId"))
			{
				this.WarId = Convert.ToString(dictionary["warId"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("endDate"))
			{
				this.EndDate = Convert.ToUInt32(dictionary["endDate"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("score"))
			{
				this.Score = Convert.ToInt32(dictionary["score"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("opponentScore"))
			{
				this.OpponentScore = Convert.ToInt32(dictionary["opponentScore"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("opponentName"))
			{
				this.OpponentName = WWW.UnEscapeURL(Convert.ToString(dictionary["opponentName"], CultureInfo.InvariantCulture));
			}
			if (dictionary.ContainsKey("opponentIcon"))
			{
				this.OpponentIcon = Convert.ToString(dictionary["opponentIcon"], CultureInfo.InvariantCulture);
				if (string.IsNullOrEmpty(this.OpponentIcon))
				{
					this.OpponentIcon = "SquadSymbols_01";
				}
			}
			return this;
		}

		public string ToJson()
		{
			return Serializer.Start().End().ToString();
		}

		protected internal SquadWarHistoryEntry(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarHistoryEntry)GCHandledObjects.GCHandleToObject(instance)).CompareTo((SquadWarHistoryEntry)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarHistoryEntry)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarHistoryEntry)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
