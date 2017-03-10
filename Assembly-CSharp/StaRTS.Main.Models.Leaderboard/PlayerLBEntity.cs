using StaRTS.Utils;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Leaderboard
{
	public class PlayerLBEntity : ISerializable, IComparable<PlayerLBEntity>
	{
		public string PlayerID
		{
			get;
			set;
		}

		public string PlayerName
		{
			get;
			set;
		}

		public string SocialID
		{
			get;
			set;
		}

		public string Planet
		{
			get;
			set;
		}

		public int BattleScore
		{
			get;
			set;
		}

		public int Rank
		{
			get;
			set;
		}

		public int Percentile
		{
			get;
			set;
		}

		public LeaderboardBattleHistory BattleHistory
		{
			get;
			private set;
		}

		public FactionType Faction
		{
			get;
			set;
		}

		public string SquadName
		{
			get;
			set;
		}

		public string SquadID
		{
			get;
			set;
		}

		public string Symbol
		{
			get;
			set;
		}

		public Dictionary<string, LeaderboardBattleHistory> TournamentBattleHistory
		{
			get;
			private set;
		}

		public PlayerLBEntity(string playerId)
		{
			this.PlayerName = "";
			this.PlayerID = playerId;
			this.SocialID = "";
			this.Rank = 0;
			this.BattleScore = 0;
			this.Faction = FactionType.Smuggler;
			this.SquadName = "";
			this.SquadID = "";
			this.Symbol = "";
			this.Planet = "";
		}

		public string ToJson()
		{
			return Serializer.Start().End().ToString();
		}

		public ISerializable FromObject(object obj)
		{
			try
			{
				Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
				if (dictionary.ContainsKey("_id"))
				{
					this.PlayerID = Convert.ToString(dictionary["_id"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("snid"))
				{
					this.SocialID = Convert.ToString(dictionary["snid"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("rank"))
				{
					this.Rank = Convert.ToInt32(dictionary["rank"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("value"))
				{
					this.BattleScore = Convert.ToInt32(dictionary["value"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("account"))
				{
					Dictionary<string, object> dictionary2 = dictionary["account"] as Dictionary<string, object>;
					if (dictionary2.ContainsKey("manimal"))
					{
						Dictionary<string, object> dictionary3 = dictionary2["manimal"] as Dictionary<string, object>;
						if (dictionary3.ContainsKey("data"))
						{
							Dictionary<string, object> dictionary4 = dictionary3["data"] as Dictionary<string, object>;
							if (dictionary4 == null)
							{
								return null;
							}
							if (dictionary4.ContainsKey("scalars"))
							{
								this.BattleHistory = new LeaderboardBattleHistory(dictionary4["scalars"]);
							}
							if (dictionary4.ContainsKey("name"))
							{
								this.PlayerName = Convert.ToString(dictionary4["name"], CultureInfo.InvariantCulture);
							}
							if (dictionary4.ContainsKey("playerModel"))
							{
								Dictionary<string, object> dictionary5 = dictionary4["playerModel"] as Dictionary<string, object>;
								if (dictionary5.ContainsKey("faction"))
								{
									string name = Convert.ToString(dictionary5["faction"], CultureInfo.InvariantCulture);
									this.Faction = StringUtils.ParseEnum<FactionType>(name);
								}
								if (dictionary5.ContainsKey("guildInfo"))
								{
									Dictionary<string, object> dictionary6 = dictionary5["guildInfo"] as Dictionary<string, object>;
									if (dictionary6.ContainsKey("guildId"))
									{
										this.SquadID = Convert.ToString(dictionary6["guildId"], CultureInfo.InvariantCulture);
									}
									if (dictionary6.ContainsKey("guildName"))
									{
										this.SquadName = WWW.UnEscapeURL(Convert.ToString(dictionary6["guildName"], CultureInfo.InvariantCulture));
									}
									if (dictionary6.ContainsKey("icon"))
									{
										this.Symbol = Convert.ToString(dictionary6["icon"], CultureInfo.InvariantCulture);
									}
								}
								if (dictionary5.ContainsKey("map"))
								{
									Dictionary<string, object> dictionary7 = dictionary5["map"] as Dictionary<string, object>;
									if (dictionary7.ContainsKey("planet"))
									{
										this.Planet = Convert.ToString(dictionary7["planet"], CultureInfo.InvariantCulture);
									}
								}
								this.ParseTournamentData(dictionary5);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return this;
		}

		private void ParseTournamentData(Dictionary<string, object> playerDict)
		{
			if (playerDict.ContainsKey("tournaments"))
			{
				Dictionary<string, object> dictionary = playerDict["tournaments"] as Dictionary<string, object>;
				if (dictionary != null)
				{
					foreach (KeyValuePair<string, object> current in dictionary)
					{
						if (this.TournamentBattleHistory == null)
						{
							this.TournamentBattleHistory = new Dictionary<string, LeaderboardBattleHistory>();
						}
						LeaderboardBattleHistory value = new LeaderboardBattleHistory(current.get_Value());
						this.TournamentBattleHistory.Add(current.get_Key(), value);
					}
				}
			}
		}

		public int CompareTo(PlayerLBEntity comparePlayer)
		{
			if (comparePlayer == null)
			{
				return -1;
			}
			return comparePlayer.BattleScore - this.BattleScore;
		}

		protected internal PlayerLBEntity(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).CompareTo((PlayerLBEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).BattleHistory);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).BattleScore);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).Percentile);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).Planet);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).PlayerID);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).PlayerName);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).Rank);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).SocialID);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).SquadID);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).SquadName);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).Symbol);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).TournamentBattleHistory);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).ParseTournamentData((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).BattleHistory = (LeaderboardBattleHistory)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).BattleScore = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).Percentile = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).Planet = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).PlayerID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).PlayerName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).Rank = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).SocialID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).SquadID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).SquadName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).Symbol = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).TournamentBattleHistory = (Dictionary<string, LeaderboardBattleHistory>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerLBEntity)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
