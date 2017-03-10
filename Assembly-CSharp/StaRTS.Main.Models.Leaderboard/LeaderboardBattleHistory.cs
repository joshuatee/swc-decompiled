using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Leaderboard
{
	public class LeaderboardBattleHistory
	{
		public int AttacksWon
		{
			get;
			private set;
		}

		public int DefensesWon
		{
			get;
			private set;
		}

		public LeaderboardBattleHistory(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				if (dictionary.ContainsKey("attacksWon"))
				{
					this.AttacksWon = Convert.ToInt32(dictionary["attacksWon"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("defensesWon"))
				{
					this.DefensesWon = Convert.ToInt32(dictionary["defensesWon"], CultureInfo.InvariantCulture);
				}
			}
		}

		protected internal LeaderboardBattleHistory(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardBattleHistory)GCHandledObjects.GCHandleToObject(instance)).AttacksWon);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardBattleHistory)GCHandledObjects.GCHandleToObject(instance)).DefensesWon);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((LeaderboardBattleHistory)GCHandledObjects.GCHandleToObject(instance)).AttacksWon = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LeaderboardBattleHistory)GCHandledObjects.GCHandleToObject(instance)).DefensesWon = *(int*)args;
			return -1L;
		}
	}
}
