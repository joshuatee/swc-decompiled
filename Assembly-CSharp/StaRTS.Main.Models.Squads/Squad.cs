using StaRTS.Utils;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Squads
{
	public class Squad : ISerializable, IComparable<Squad>
	{
		public const string DEFAULT_SQUAD_SYMBOL = "SquadSymbols_01";

		public string SquadName
		{
			get;
			private set;
		}

		public List<SquadMember> MemberList
		{
			get;
			private set;
		}

		public int MemberCount
		{
			get;
			set;
		}

		public int ActiveMemberCount
		{
			get;
			private set;
		}

		public int MemberMax
		{
			get;
			private set;
		}

		public int BattleScore
		{
			get;
			set;
		}

		public int Rank
		{
			get;
			private set;
		}

		public int HighestRank
		{
			get;
			private set;
		}

		public int InviteType
		{
			get;
			set;
		}

		public int RequiredTrophies
		{
			get;
			set;
		}

		public FactionType Faction
		{
			get;
			private set;
		}

		public string Symbol
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string SquadID
		{
			get;
			private set;
		}

		public string CurrentWarId
		{
			get;
			set;
		}

		public int WarSignUpTime
		{
			get;
			set;
		}

		public List<SquadWarHistoryEntry> WarHistory
		{
			get;
			private set;
		}

		public int Level
		{
			get;
			set;
		}

		public int TotalRepInvested
		{
			get;
			set;
		}

		public SquadPerks Perks
		{
			get;
			private set;
		}

		public Squad(string squadID)
		{
			this.SquadID = squadID;
			this.MemberCount = 0;
			this.ActiveMemberCount = 0;
			this.MemberList = new List<SquadMember>();
			this.MemberMax = GameConstants.SQUAD_MEMBER_LIMIT;
			this.Rank = 0;
			this.HighestRank = 0;
			this.BattleScore = 0;
			this.Level = 1;
			this.TotalRepInvested = 0;
			this.WarHistory = new List<SquadWarHistoryEntry>();
			this.Perks = new SquadPerks();
			this.Perks.Default();
		}

		public string ToJson()
		{
			return Serializer.Start().End().ToString();
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return this;
			}
			if (dictionary.ContainsKey("id"))
			{
				this.SquadID = Convert.ToString(dictionary["id"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("name"))
			{
				this.SquadName = WWW.UnEscapeURL(Convert.ToString(dictionary["name"], CultureInfo.InvariantCulture));
			}
			if (dictionary.ContainsKey("description"))
			{
				this.Description = WWW.UnEscapeURL(Convert.ToString(dictionary["description"], CultureInfo.InvariantCulture));
			}
			if (dictionary.ContainsKey("icon"))
			{
				this.Symbol = Convert.ToString(dictionary["icon"], CultureInfo.InvariantCulture);
				if (string.IsNullOrEmpty(this.Symbol))
				{
					this.Symbol = "SquadSymbols_01";
				}
			}
			if (dictionary.ContainsKey("rank"))
			{
				int num = Convert.ToInt32(dictionary["rank"], CultureInfo.InvariantCulture);
				if (num > 0)
				{
					this.Rank = num;
				}
			}
			this.HighestRank = this.Rank;
			if (dictionary.ContainsKey("highestRankAchieved"))
			{
				object obj2 = dictionary["highestRankAchieved"];
				if (obj2 != null)
				{
					this.HighestRank = Convert.ToInt32(obj2, CultureInfo.InvariantCulture);
				}
			}
			if (dictionary.ContainsKey("score"))
			{
				this.BattleScore = Convert.ToInt32(dictionary["score"], CultureInfo.InvariantCulture);
			}
			else if (dictionary.ContainsKey("value"))
			{
				this.BattleScore = Convert.ToInt32(dictionary["value"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("memberCount"))
			{
				this.DeserializeMemberCount(Convert.ToInt32(dictionary["memberCount"], CultureInfo.InvariantCulture));
			}
			if (dictionary.ContainsKey("currentWarId"))
			{
				this.CurrentWarId = Convert.ToString(dictionary["currentWarId"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("warSignUpTime"))
			{
				this.WarSignUpTime = Convert.ToInt32(dictionary["warSignUpTime"], CultureInfo.InvariantCulture);
			}
			Dictionary<string, object> dictionary2 = dictionary["membershipRestrictions"] as Dictionary<string, object>;
			if (dictionary2 != null)
			{
				if (dictionary2.ContainsKey("maxSize"))
				{
					this.MemberMax = Convert.ToInt32(dictionary2["maxSize"], CultureInfo.InvariantCulture);
				}
				if (dictionary2.ContainsKey("minScoreAtEnrollment"))
				{
					this.RequiredTrophies = Convert.ToInt32(dictionary2["minScoreAtEnrollment"], CultureInfo.InvariantCulture);
				}
				if (dictionary2.ContainsKey("faction"))
				{
					string name = Convert.ToString(dictionary2["faction"], CultureInfo.InvariantCulture);
					this.Faction = StringUtils.ParseEnum<FactionType>(name);
				}
				if (dictionary2.ContainsKey("openEnrollment"))
				{
					if (Convert.ToBoolean(dictionary2["openEnrollment"], CultureInfo.InvariantCulture))
					{
						this.InviteType = 1;
					}
					else
					{
						this.InviteType = 0;
					}
				}
			}
			this.DeserializeMemberList(dictionary["members"] as List<object>);
			if (dictionary.ContainsKey("memberCount"))
			{
				this.DeserializeMemberCount(Convert.ToInt32(dictionary["memberCount"], CultureInfo.InvariantCulture));
			}
			if (dictionary.ContainsKey("activeMemberCount"))
			{
				this.DeserializeActiveMemberCount(Convert.ToInt32(dictionary["activeMemberCount"], CultureInfo.InvariantCulture));
			}
			if (dictionary.ContainsKey("warHistory"))
			{
				this.DeserializeWarHistoryList(dictionary["warHistory"] as List<object>);
			}
			if (dictionary.ContainsKey("level"))
			{
				object obj3 = dictionary["level"];
				if (obj3 != null)
				{
					this.Level = Convert.ToInt32(obj3, CultureInfo.InvariantCulture);
				}
			}
			if (dictionary.ContainsKey("totalRepInvested"))
			{
				object obj4 = dictionary["totalRepInvested"];
				if (obj4 != null)
				{
					this.TotalRepInvested = Convert.ToInt32(obj4, CultureInfo.InvariantCulture);
				}
			}
			if (dictionary.ContainsKey("perks"))
			{
				this.UpdateSquadPerks(dictionary["perks"]);
			}
			return this;
		}

		public void UpdateSquadPerks(object perksDataObject)
		{
			if (perksDataObject != null)
			{
				this.Perks.FromObject(perksDataObject);
			}
		}

		public void UpdateSquadLevel(int squadLevel, int totalRepInvested)
		{
			this.Level = squadLevel;
			this.TotalRepInvested = totalRepInvested;
		}

		public ISerializable FromFeaturedObject(Dictionary<string, object> d)
		{
			if (d.ContainsKey("faction"))
			{
				string name = Convert.ToString(d["faction"], CultureInfo.InvariantCulture);
				this.Faction = StringUtils.ParseEnum<FactionType>(name);
			}
			if (d.ContainsKey("name"))
			{
				this.SquadName = WWW.UnEscapeURL(Convert.ToString(d["name"], CultureInfo.InvariantCulture));
			}
			if (d.ContainsKey("icon"))
			{
				this.Symbol = Convert.ToString(d["icon"], CultureInfo.InvariantCulture);
				if (string.IsNullOrEmpty(this.Symbol))
				{
					this.Symbol = "SquadSymbols_01";
				}
			}
			if (d.ContainsKey("minScore"))
			{
				this.RequiredTrophies = Convert.ToInt32(d["minScore"], CultureInfo.InvariantCulture);
			}
			if (d.ContainsKey("score"))
			{
				this.BattleScore = Convert.ToInt32(d["score"], CultureInfo.InvariantCulture);
			}
			if (d.ContainsKey("rank"))
			{
				this.Rank = Convert.ToInt32(d["rank"], CultureInfo.InvariantCulture);
			}
			if (d.ContainsKey("openEnrollment"))
			{
				this.InviteType = (Convert.ToBoolean(d["openEnrollment"], CultureInfo.InvariantCulture) ? 1 : 0);
			}
			if (d.ContainsKey("members"))
			{
				this.DeserializeMemberCount(Convert.ToInt32(d["members"], CultureInfo.InvariantCulture));
			}
			if (d.ContainsKey("activeMemberCount"))
			{
				this.DeserializeActiveMemberCount(Convert.ToInt32(d["activeMemberCount"], CultureInfo.InvariantCulture));
			}
			if (d.ContainsKey("warHistory"))
			{
				this.DeserializeWarHistoryList(d["warHistory"] as List<object>);
			}
			if (d.ContainsKey("level"))
			{
				object obj = d["level"];
				if (obj != null)
				{
					this.Level = Convert.ToInt32(obj, CultureInfo.InvariantCulture);
				}
				if (this.Level <= 0)
				{
					this.Level = 1;
				}
			}
			return this;
		}

		public ISerializable FromLeaderboardObject(Dictionary<string, object> lbObj)
		{
			if (lbObj.ContainsKey("rank"))
			{
				this.Rank = Convert.ToInt32(lbObj["rank"], CultureInfo.InvariantCulture);
			}
			if (this.Rank < this.HighestRank)
			{
				this.HighestRank = this.Rank;
			}
			if (lbObj.ContainsKey("value"))
			{
				this.BattleScore = Convert.ToInt32(lbObj["value"], CultureInfo.InvariantCulture);
			}
			if (lbObj.ContainsKey("account"))
			{
				Dictionary<string, object> dictionary = lbObj["account"] as Dictionary<string, object>;
				if (dictionary != null && dictionary.ContainsKey("manimal"))
				{
					Dictionary<string, object> dictionary2 = dictionary["manimal"] as Dictionary<string, object>;
					if (dictionary2.ContainsKey("data"))
					{
						Dictionary<string, object> dictionary3 = dictionary2["data"] as Dictionary<string, object>;
						if (dictionary3.ContainsKey("name"))
						{
							this.SquadName = WWW.UnEscapeURL(Convert.ToString(dictionary3["name"], CultureInfo.InvariantCulture));
						}
						if (dictionary3.ContainsKey("level"))
						{
							this.Level = Convert.ToInt32(dictionary3["level"], CultureInfo.InvariantCulture);
							if (this.Level <= 0)
							{
								this.Level = 1;
							}
						}
						if (dictionary3.ContainsKey("icon"))
						{
							this.Symbol = Convert.ToString(dictionary3["icon"], CultureInfo.InvariantCulture);
							if (string.IsNullOrEmpty(this.Symbol))
							{
								this.Symbol = "SquadSymbols_01";
							}
						}
						if (dictionary3.ContainsKey("memberCount"))
						{
							this.DeserializeMemberCount(Convert.ToInt32(dictionary3["memberCount"], CultureInfo.InvariantCulture));
						}
						if (dictionary3.ContainsKey("activeMemberCount"))
						{
							this.DeserializeActiveMemberCount(Convert.ToInt32(dictionary3["activeMemberCount"], CultureInfo.InvariantCulture));
						}
						if (dictionary3.ContainsKey("warHistory"))
						{
							this.DeserializeWarHistoryList(dictionary3["warHistory"] as List<object>);
						}
						if (dictionary3.ContainsKey("membershipRestrictions"))
						{
							Dictionary<string, object> dictionary4 = dictionary3["membershipRestrictions"] as Dictionary<string, object>;
							if (dictionary4.ContainsKey("faction"))
							{
								string name = Convert.ToString(dictionary4["faction"], CultureInfo.InvariantCulture);
								this.Faction = StringUtils.ParseEnum<FactionType>(name);
							}
							if (dictionary4.ContainsKey("minScoreAtEnrollment"))
							{
								this.RequiredTrophies = Convert.ToInt32(dictionary4["minScoreAtEnrollment"], CultureInfo.InvariantCulture);
							}
							if (dictionary4.ContainsKey("openEnrollment"))
							{
								this.InviteType = (Convert.ToBoolean(dictionary4["openEnrollment"], CultureInfo.InvariantCulture) ? 1 : 0);
							}
						}
					}
				}
			}
			return this;
		}

		public ISerializable FromLoginObject(Dictionary<string, object> d)
		{
			if (d.ContainsKey("guildName"))
			{
				string s = d["guildName"] as string;
				this.SquadName = WWW.UnEscapeURL(s);
			}
			return this;
		}

		public ISerializable FromVisitNeighborObject(Dictionary<string, object> d)
		{
			if (d.ContainsKey("guildName"))
			{
				string s = d["guildName"] as string;
				this.SquadName = WWW.UnEscapeURL(s);
			}
			return this;
		}

		private void DeserializeMemberCount(int count)
		{
			this.MemberCount = count;
			if (this.MemberList.Count != 0 && this.MemberCount != this.MemberList.Count)
			{
				this.MemberCount = this.MemberList.Count;
			}
		}

		private void DeserializeActiveMemberCount(int count)
		{
			this.ActiveMemberCount = count;
			if (this.ActiveMemberCount > this.MemberCount)
			{
				this.ActiveMemberCount = this.MemberCount;
			}
		}

		private void DeserializeMemberList(List<object> memberList)
		{
			this.MemberList.Clear();
			if (memberList != null)
			{
				this.MemberCount = memberList.Count;
				int i = 0;
				int count = memberList.Count;
				while (i < count)
				{
					SquadMember squadMember = new SquadMember();
					squadMember.FromObject(memberList[i]);
					this.MemberList.Add(squadMember);
					i++;
				}
				this.MemberList.Sort();
			}
		}

		private void DeserializeWarHistoryList(List<object> warHistoryList)
		{
			if (warHistoryList != null && warHistoryList.Count > 0)
			{
				this.WarHistory.Clear();
				int i = 0;
				int count = warHistoryList.Count;
				while (i < count)
				{
					SquadWarHistoryEntry squadWarHistoryEntry = new SquadWarHistoryEntry();
					squadWarHistoryEntry.FromObject(warHistoryList[i]);
					this.WarHistory.Add(squadWarHistoryEntry);
					i++;
				}
				this.WarHistory.Sort();
			}
		}

		public int CompareTo(Squad compareSquad)
		{
			if (compareSquad == null)
			{
				return -1;
			}
			return compareSquad.BattleScore - this.BattleScore;
		}

		public void ClearSquadWarId()
		{
			this.CurrentWarId = null;
		}

		protected internal Squad(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).ClearSquadWarId();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).CompareTo((Squad)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).DeserializeActiveMemberCount(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).DeserializeMemberCount(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).DeserializeMemberList((List<object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).DeserializeWarHistoryList((List<object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).FromFeaturedObject((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).FromLeaderboardObject((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).FromLoginObject((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).FromVisitNeighborObject((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).ActiveMemberCount);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).BattleScore);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).CurrentWarId);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).HighestRank);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).InviteType);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).Level);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).MemberCount);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).MemberList);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).MemberMax);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).Perks);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).Rank);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).RequiredTrophies);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).SquadID);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).SquadName);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).Symbol);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).TotalRepInvested);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).WarHistory);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).WarSignUpTime);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).ActiveMemberCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).BattleScore = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).CurrentWarId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).Description = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).HighestRank = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).InviteType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).Level = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).MemberCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).MemberList = (List<SquadMember>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).MemberMax = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).Perks = (SquadPerks)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).Rank = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).RequiredTrophies = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).SquadID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).SquadName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).Symbol = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).TotalRepInvested = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).WarHistory = (List<SquadWarHistoryEntry>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).WarSignUpTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Squad)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).UpdateSquadLevel(*(int*)args, *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((Squad)GCHandledObjects.GCHandleToObject(instance)).UpdateSquadPerks(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
