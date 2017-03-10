using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Squads
{
	public class SquadMember : ISerializable, IComparable<SquadMember>
	{
		public string MemberName
		{
			get;
			set;
		}

		public string MemberID
		{
			get;
			set;
		}

		public string Planet
		{
			get;
			set;
		}

		public SquadRole Role
		{
			get;
			set;
		}

		public int Score
		{
			get;
			set;
		}

		public uint JoinDate
		{
			get;
			set;
		}

		public uint LastLoginTime
		{
			get;
			set;
		}

		public int TroopsDonated
		{
			get;
			set;
		}

		public int TroopsReceived
		{
			get;
			set;
		}

		public int ReputationInvested
		{
			get;
			set;
		}

		public int AttacksWon
		{
			get;
			set;
		}

		public int DefensesWon
		{
			get;
			set;
		}

		public Dictionary<string, int> TournamentScore
		{
			get;
			set;
		}

		public int BaseScore
		{
			get;
			set;
		}

		public int WarParty
		{
			get;
			set;
		}

		public int HQLevel
		{
			get;
			set;
		}

		public SquadMember()
		{
			this.MemberName = "";
			this.MemberID = "0";
			this.Score = 0;
			this.TroopsDonated = 0;
			this.TroopsReceived = 0;
			this.ReputationInvested = 0;
			this.AttacksWon = 0;
			this.DefensesWon = 0;
			this.LastLoginTime = 0u;
			this.TournamentScore = new Dictionary<string, int>();
			this.JoinDate = 0u;
			this.BaseScore = 0;
			this.Role = SquadRole.Member;
			this.WarParty = 0;
			this.HQLevel = 0;
		}

		public int CompareTo(SquadMember compareMember)
		{
			if (compareMember == null)
			{
				return -1;
			}
			return compareMember.Score - this.Score;
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("name"))
			{
				this.MemberName = Convert.ToString(dictionary["name"], CultureInfo.InvariantCulture);
			}
			bool flag = false;
			if (dictionary.ContainsKey("isOwner"))
			{
				flag = Convert.ToBoolean(dictionary["isOwner"], CultureInfo.InvariantCulture);
			}
			bool flag2 = false;
			if (dictionary.ContainsKey("isOfficer"))
			{
				flag2 = Convert.ToBoolean(dictionary["isOfficer"], CultureInfo.InvariantCulture);
			}
			if (flag)
			{
				this.Role = SquadRole.Owner;
			}
			else if (flag2)
			{
				this.Role = SquadRole.Officer;
			}
			else
			{
				this.Role = SquadRole.Member;
			}
			if (dictionary.ContainsKey("score"))
			{
				this.Score = Convert.ToInt32(dictionary["score"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("joinDate"))
			{
				this.JoinDate = Convert.ToUInt32(dictionary["joinDate"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("lastLoginTime"))
			{
				this.LastLoginTime = Convert.ToUInt32(dictionary["lastLoginTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("troopsDonated"))
			{
				this.TroopsDonated = Convert.ToInt32(dictionary["troopsDonated"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("reputationInvested"))
			{
				this.ReputationInvested = Convert.ToInt32(dictionary["reputationInvested"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("troopsReceived"))
			{
				this.TroopsReceived = Convert.ToInt32(dictionary["troopsReceived"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("attacksWon"))
			{
				this.AttacksWon = Convert.ToInt32(dictionary["attacksWon"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("defensesWon"))
			{
				this.DefensesWon = Convert.ToInt32(dictionary["defensesWon"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("tournamentScores"))
			{
				object obj2 = dictionary["tournamentScores"];
				if (obj2 != null)
				{
					Dictionary<string, object> dictionary2 = obj2 as Dictionary<string, object>;
					if (dictionary2 != null)
					{
						foreach (KeyValuePair<string, object> current in dictionary2)
						{
							this.TournamentScore.Add(current.get_Key(), Convert.ToInt32(current.get_Value(), CultureInfo.InvariantCulture));
						}
					}
				}
			}
			if (dictionary.ContainsKey("playerId"))
			{
				this.MemberID = Convert.ToString(dictionary["playerId"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("planet"))
			{
				this.Planet = Convert.ToString(dictionary["planet"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("warParty"))
			{
				this.WarParty = Convert.ToInt32(dictionary["warParty"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("hqLevel"))
			{
				this.HQLevel = Convert.ToInt32(dictionary["hqLevel"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("xp"))
			{
				this.BaseScore = Convert.ToInt32(dictionary["xp"], CultureInfo.InvariantCulture);
			}
			return this;
		}

		public string ToJson()
		{
			return Serializer.Start().End().ToString();
		}

		protected internal SquadMember(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).CompareTo((SquadMember)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).AttacksWon);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).BaseScore);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).DefensesWon);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).HQLevel);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).MemberID);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).MemberName);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).Planet);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).ReputationInvested);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).Role);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).Score);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).TournamentScore);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).TroopsDonated);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).TroopsReceived);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).WarParty);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).AttacksWon = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).BaseScore = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).DefensesWon = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).HQLevel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).MemberID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).MemberName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).Planet = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).ReputationInvested = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).Role = (SquadRole)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).Score = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).TournamentScore = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).TroopsDonated = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).TroopsReceived = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((SquadMember)GCHandledObjects.GCHandleToObject(instance)).WarParty = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMember)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
