using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player.World;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Pvp
{
	public class PvpTarget : AbstractResponse
	{
		private const string BATTLE_ID = "battleId";

		private const string EQUIPMENT = "equipment";

		private const string PLAYER_ID = "playerId";

		private const string PLAYER_NAME = "name";

		private const string PLAYER_LEVEL = "level";

		private const string PLAYER_XP = "xp";

		private const string PLAYER_FACTION = "faction";

		private const string PLAYER_ATTACK_RATING = "attackRating";

		private const string PLAYER_DEFENSE_RATING = "defenseRating";

		private const string PLAYER_ATTACKS_WON = "attacksWon";

		private const string PLAYER_DEFENSES_WON = "defensesWon";

		private const string GUILD_ID = "guildId";

		private const string GUILD_NAME = "guildName";

		private const string GUILD_TROOPS = "guildTroops";

		private const string CHAMPIONS = "champions";

		private const string POTENTIAL_POINTS = "potentialPoints";

		private const string POTENTIAL_SCORE_WIN = "potentialScoreWin";

		private const string POTENTIAL_SCORE_LOSE = "potentialScoreLose";

		private const string POTENTIAL_TOURNAMENT_RATING_DELTA_WIN = "potentialPointsWin";

		private const string POTENTIAL_TOURNAMENT_RATING_DELTA_LOSE = "potentialPointsLose";

		private const string BASE_MAP = "map";

		private const string CREDITS = "credits";

		private const string MATERIALS = "materials";

		private const string CONTRABAND = "contraband";

		private const string BUILDING_LOOT_MAP = "resources";

		private const string CREDITS_CHARGED = "creditsCharged";

		public string BattleId
		{
			get;
			private set;
		}

		public string PlayerId
		{
			get;
			private set;
		}

		public string PlayerName
		{
			get;
			private set;
		}

		public int PlayerLevel
		{
			get;
			private set;
		}

		public int PlayerXp
		{
			get;
			private set;
		}

		public FactionType PlayerFaction
		{
			get;
			private set;
		}

		public int PlayerAttackRating
		{
			get;
			private set;
		}

		public int PlayerDefenseRating
		{
			get;
			private set;
		}

		public int PlayerAttacksWon
		{
			get;
			private set;
		}

		public int PlayerDefensesWon
		{
			get;
			private set;
		}

		public string GuildId
		{
			get;
			private set;
		}

		public string GuildName
		{
			get;
			private set;
		}

		public Dictionary<string, int> GuildDonatedTroops
		{
			get;
			private set;
		}

		public Dictionary<string, int> Champions
		{
			get;
			private set;
		}

		public int PotentialMedalsToGain
		{
			get;
			private set;
		}

		public int PotentialMedalsToLose
		{
			get;
			private set;
		}

		public int PotentialTournamentRatingDeltaWin
		{
			get;
			private set;
		}

		public int PotentialTournamentRatingDeltaLose
		{
			get;
			private set;
		}

		public Map BaseMap
		{
			get;
			private set;
		}

		public int AvailableCredits
		{
			get;
			private set;
		}

		public int AvailableMaterials
		{
			get;
			private set;
		}

		public int AvailableContraband
		{
			get;
			private set;
		}

		public Dictionary<string, int> BuildingLootCreditsMap
		{
			get;
			private set;
		}

		public Dictionary<string, int> BuildingLootMaterialsMap
		{
			get;
			private set;
		}

		public Dictionary<string, int> BuildingLootContrabandMap
		{
			get;
			private set;
		}

		public int CreditsCharged
		{
			get;
			private set;
		}

		public List<ContractTO> Contracts
		{
			get;
			private set;
		}

		public List<string> Equipment
		{
			get;
			private set;
		}

		public Dictionary<string, object> AttackerDeployableServerData
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				Service.Get<StaRTSLogger>().Error("Attempted to create invalid PvpTarget.");
				return null;
			}
			if (dictionary.ContainsKey("battleId"))
			{
				this.BattleId = (string)dictionary["battleId"];
			}
			if (dictionary.ContainsKey("playerId"))
			{
				this.PlayerId = (string)dictionary["playerId"];
			}
			if (dictionary.ContainsKey("name"))
			{
				this.PlayerName = (string)dictionary["name"];
			}
			if (dictionary.ContainsKey("level"))
			{
				this.PlayerLevel = Convert.ToInt32(dictionary["level"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("xp"))
			{
				this.PlayerXp = Convert.ToInt32(dictionary["xp"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("attackRating"))
			{
				this.PlayerAttackRating = Convert.ToInt32(dictionary["attackRating"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("defenseRating"))
			{
				this.PlayerDefenseRating = Convert.ToInt32(dictionary["defenseRating"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("attacksWon"))
			{
				this.PlayerAttacksWon = Convert.ToInt32(dictionary["attacksWon"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("defensesWon"))
			{
				this.PlayerDefensesWon = Convert.ToInt32(dictionary["defensesWon"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("guildId"))
			{
				this.GuildId = (string)dictionary["guildId"];
			}
			if (dictionary.ContainsKey("guildName"))
			{
				this.GuildName = (string)dictionary["guildName"];
			}
			if (dictionary.ContainsKey("map"))
			{
				this.BaseMap = new Map();
				this.BaseMap.FromObject(dictionary["map"]);
				this.BaseMap.InitializePlanet();
			}
			if (dictionary.ContainsKey("resources"))
			{
				this.BuildingLootCreditsMap = new Dictionary<string, int>();
				this.BuildingLootMaterialsMap = new Dictionary<string, int>();
				this.BuildingLootContrabandMap = new Dictionary<string, int>();
				Dictionary<string, object> dictionary2 = dictionary["resources"] as Dictionary<string, object>;
				foreach (KeyValuePair<string, object> current in dictionary2)
				{
					Dictionary<string, object> dictionary3 = current.get_Value() as Dictionary<string, object>;
					if (dictionary3 != null)
					{
						if (dictionary3.ContainsKey("credits"))
						{
							this.BuildingLootCreditsMap.Add(current.get_Key(), Convert.ToInt32(dictionary3["credits"], CultureInfo.InvariantCulture));
							this.AvailableCredits += Convert.ToInt32(dictionary3["credits"], CultureInfo.InvariantCulture);
						}
						if (dictionary3.ContainsKey("materials"))
						{
							this.BuildingLootMaterialsMap.Add(current.get_Key(), Convert.ToInt32(dictionary3["materials"], CultureInfo.InvariantCulture));
							this.AvailableMaterials += Convert.ToInt32(dictionary3["materials"], CultureInfo.InvariantCulture);
						}
						if (dictionary3.ContainsKey("contraband"))
						{
							this.BuildingLootContrabandMap.Add(current.get_Key(), Convert.ToInt32(dictionary3["contraband"], CultureInfo.InvariantCulture));
							this.AvailableContraband += Convert.ToInt32(dictionary3["contraband"], CultureInfo.InvariantCulture);
						}
					}
				}
			}
			if (dictionary.ContainsKey("potentialPoints"))
			{
				Dictionary<string, object> dictionary4 = dictionary["potentialPoints"] as Dictionary<string, object>;
				if (dictionary4 != null)
				{
					if (dictionary4.ContainsKey("potentialScoreWin"))
					{
						this.PotentialMedalsToGain = Convert.ToInt32(dictionary4["potentialScoreWin"], CultureInfo.InvariantCulture);
					}
					if (dictionary4.ContainsKey("potentialScoreLose"))
					{
						this.PotentialMedalsToLose = Convert.ToInt32(dictionary4["potentialScoreLose"], CultureInfo.InvariantCulture);
					}
					if (dictionary4.ContainsKey("potentialPointsWin"))
					{
						this.PotentialTournamentRatingDeltaWin = Convert.ToInt32(dictionary4["potentialPointsWin"], CultureInfo.InvariantCulture);
					}
					if (dictionary4.ContainsKey("potentialPointsLose"))
					{
						this.PotentialTournamentRatingDeltaLose = Convert.ToInt32(dictionary4["potentialPointsLose"], CultureInfo.InvariantCulture);
					}
				}
			}
			if (dictionary.ContainsKey("guildTroops"))
			{
				this.GuildDonatedTroops = new Dictionary<string, int>();
				Dictionary<string, object> dictionary5 = dictionary["guildTroops"] as Dictionary<string, object>;
				if (dictionary5 != null)
				{
					foreach (KeyValuePair<string, object> current2 in dictionary5)
					{
						string key = current2.get_Key();
						int num = 0;
						Dictionary<string, object> dictionary6 = current2.get_Value() as Dictionary<string, object>;
						if (dictionary6 != null)
						{
							foreach (KeyValuePair<string, object> current3 in dictionary6)
							{
								num += Convert.ToInt32(current3.get_Value(), CultureInfo.InvariantCulture);
							}
							this.GuildDonatedTroops.Add(key, num);
						}
					}
				}
			}
			if (dictionary.ContainsKey("champions"))
			{
				this.Champions = new Dictionary<string, int>(StringComparer.Ordinal);
				Dictionary<string, object> dictionary7 = dictionary["champions"] as Dictionary<string, object>;
				if (dictionary7 != null)
				{
					foreach (KeyValuePair<string, object> current4 in dictionary7)
					{
						string key2 = current4.get_Key();
						this.Champions.Add(key2, Convert.ToInt32(current4.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			if (dictionary.ContainsKey("creditsCharged"))
			{
				this.CreditsCharged = Convert.ToInt32(dictionary["creditsCharged"], CultureInfo.InvariantCulture);
			}
			this.Contracts = new List<ContractTO>();
			if (dictionary.ContainsKey("contracts"))
			{
				List<object> list = dictionary["contracts"] as List<object>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						ContractTO item = new ContractTO().FromObject(list[i]) as ContractTO;
						this.Contracts.Add(item);
					}
				}
			}
			if (dictionary.ContainsKey("faction"))
			{
				string name = Convert.ToString(dictionary["faction"], CultureInfo.InvariantCulture);
				this.PlayerFaction = StringUtils.ParseEnum<FactionType>(name);
			}
			if (dictionary.ContainsKey("attackerDeployables"))
			{
				this.AttackerDeployableServerData = (dictionary["attackerDeployables"] as Dictionary<string, object>);
			}
			if (dictionary.ContainsKey("equipment"))
			{
				List<object> list2 = dictionary["equipment"] as List<object>;
				if (list2 != null)
				{
					this.Equipment = new List<string>();
					int j = 0;
					int count = list2.Count;
					while (j < count)
					{
						this.Equipment.Add(list2[j] as string);
						j++;
					}
				}
			}
			return this;
		}

		public PvpTarget()
		{
		}

		protected internal PvpTarget(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).AttackerDeployableServerData);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).AvailableContraband);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).AvailableCredits);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).AvailableMaterials);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).BaseMap);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).BattleId);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).BuildingLootContrabandMap);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).BuildingLootCreditsMap);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).BuildingLootMaterialsMap);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).Champions);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).Contracts);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).CreditsCharged);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).Equipment);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).GuildDonatedTroops);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).GuildId);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).GuildName);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerAttackRating);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerAttacksWon);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerDefenseRating);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerDefensesWon);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerFaction);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerId);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerLevel);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerName);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerXp);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PotentialMedalsToGain);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PotentialMedalsToLose);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PotentialTournamentRatingDeltaLose);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PotentialTournamentRatingDeltaWin);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).AttackerDeployableServerData = (Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).AvailableContraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).AvailableCredits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).AvailableMaterials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).BaseMap = (Map)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).BattleId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).BuildingLootContrabandMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).BuildingLootCreditsMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).BuildingLootMaterialsMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).Champions = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).Contracts = (List<ContractTO>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).CreditsCharged = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).Equipment = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).GuildDonatedTroops = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).GuildId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).GuildName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerAttackRating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerAttacksWon = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerDefenseRating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerDefensesWon = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerFaction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerLevel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PlayerXp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PotentialMedalsToGain = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PotentialMedalsToLose = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PotentialTournamentRatingDeltaLose = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((PvpTarget)GCHandledObjects.GCHandleToObject(instance)).PotentialTournamentRatingDeltaWin = *(int*)args;
			return -1L;
		}
	}
}
