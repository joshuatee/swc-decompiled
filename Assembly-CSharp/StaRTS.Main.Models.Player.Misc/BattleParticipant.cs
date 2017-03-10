using StaRTS.Utils;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Misc
{
	public class BattleParticipant : ISerializable
	{
		public string PlayerId
		{
			get;
			set;
		}

		public string PlayerName
		{
			get;
			set;
		}

		public string GuildId
		{
			get;
			set;
		}

		public string GuildName
		{
			get;
			set;
		}

		public int AttackRating
		{
			get;
			set;
		}

		public int AttackRatingDelta
		{
			get;
			set;
		}

		public int DefenseRating
		{
			get;
			set;
		}

		public int DefenseRatingDelta
		{
			get;
			set;
		}

		public int TournamentRating
		{
			get;
			set;
		}

		public int TournamentRatingDelta
		{
			get;
			set;
		}

		public FactionType PlayerFaction
		{
			get;
			set;
		}

		public BattleParticipant(string id, string name, FactionType faction)
		{
			this.PlayerId = id;
			this.PlayerName = name;
			this.PlayerFaction = faction;
			if (string.IsNullOrEmpty(this.PlayerName))
			{
				this.PlayerName = this.PlayerId;
			}
		}

		public static BattleParticipant CreateFromObject(object obj)
		{
			return new BattleParticipant(null, null, FactionType.Invalid).FromObject(obj) as BattleParticipant;
		}

		public string ToJson()
		{
			return Serializer.Start().AddString("playerId", this.PlayerId).AddString("name", this.PlayerName).AddString("guildId", this.GuildId).AddString("guildName", this.GuildName).Add<int>("attackRating", this.AttackRating).Add<int>("attackRatingDelta", this.AttackRatingDelta).Add<int>("defenseRating", this.DefenseRating).Add<int>("defenseRatingDelta", this.DefenseRatingDelta).Add<int>("tournamentRating", this.TournamentRating).Add<int>("tournamentRatingDelta", this.TournamentRatingDelta).Add<string>("faction", this.PlayerFaction.ToString()).End().ToString();
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				this.PlayerId = Convert.ToString(dictionary["playerId"], CultureInfo.InvariantCulture);
				this.PlayerName = Convert.ToString(dictionary["name"], CultureInfo.InvariantCulture);
				this.GuildId = Convert.ToString(dictionary["guildId"], CultureInfo.InvariantCulture);
				this.GuildName = WWW.UnEscapeURL(Convert.ToString(dictionary["guildName"], CultureInfo.InvariantCulture));
				this.AttackRating = Convert.ToInt32(dictionary["attackRating"], CultureInfo.InvariantCulture);
				this.AttackRatingDelta = Convert.ToInt32(dictionary["attackRatingDelta"], CultureInfo.InvariantCulture);
				this.DefenseRating = Convert.ToInt32(dictionary["defenseRating"], CultureInfo.InvariantCulture);
				this.DefenseRatingDelta = Convert.ToInt32(dictionary["defenseRatingDelta"], CultureInfo.InvariantCulture);
				if (string.IsNullOrEmpty(this.PlayerName))
				{
					this.PlayerName = this.PlayerId;
				}
				if (string.IsNullOrEmpty(this.GuildName))
				{
					this.GuildName = this.GuildId;
				}
				if (dictionary.ContainsKey("faction"))
				{
					string name = Convert.ToString(dictionary["faction"], CultureInfo.InvariantCulture);
					this.PlayerFaction = StringUtils.ParseEnum<FactionType>(name);
				}
				if (dictionary.ContainsKey("tournamentRating"))
				{
					this.TournamentRating = Convert.ToInt32(dictionary["tournamentRating"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("tournamentRatingDelta"))
				{
					this.TournamentRatingDelta = Convert.ToInt32(dictionary["tournamentRatingDelta"], CultureInfo.InvariantCulture);
				}
			}
			return this;
		}

		protected internal BattleParticipant(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleParticipant.CreateFromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).AttackRating);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).AttackRatingDelta);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).DefenseRating);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).DefenseRatingDelta);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).GuildId);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).GuildName);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).PlayerFaction);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).PlayerId);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).PlayerName);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).TournamentRating);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).TournamentRatingDelta);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).AttackRating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).AttackRatingDelta = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).DefenseRating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).DefenseRatingDelta = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).GuildId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).GuildName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).PlayerFaction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).PlayerId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).PlayerName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).TournamentRating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).TournamentRatingDelta = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleParticipant)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
