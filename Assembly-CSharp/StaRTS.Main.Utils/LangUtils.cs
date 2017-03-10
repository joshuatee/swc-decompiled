using StaRTS.Assets;
using StaRTS.Externals.EnvironmentManager;
using StaRTS.Main.Configs;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public static class LangUtils
	{
		public const string LANG_PATH = "strings";

		public const string LANG_FILE = "strings_{0}.json.joe";

		public const string HN_LANG_FILE = "strings-hn_{0}.json.joe";

		private const string LOCAL_STRINGS_FILE = "strings_local_{0}.local.json";

		public static readonly string DESYNC_BATCH_MAX_RETRY = "DESYNC_BATCH_MAX_RETRY";

		public const string CONTEXT_PREFIX = "context_";

		public const string CONTEXT_ID_INFO = "Info";

		public const string CONTEXT_ID_SWAP = "Swap";

		public const string CONTEXT_ID_MOVE = "Move";

		public const string CONTEXT_ID_WALL_SELECT_ROW = "SelectRow";

		public const string CONTEXT_ID_WALL_ROTATE = "RotateWall";

		public const string CONTEXT_ID_CANCEL = "Cancel";

		public const string CONTEXT_ID_FINISH_NOW = "Finish_Now";

		public const string CONTEXT_ID_GALAXY = "Galaxy";

		public const string CONTEXT_ID_BUY_DROID = "Buy_Droid";

		public const string CONTEXT_ID_TRAIN = "Train";

		public const string CONTEXT_ID_BUILD = "Build";

		public const string CONTEXT_ID_JOIN = "Join";

		public const string CONTEXT_ID_STASH = "Stash";

		public const string CONTEXT_ID_SQUAD = "Squad";

		public const string CONTEXT_ID_NAVIGATE = "Navigate";

		public const string CONTEXT_ID_RAID_DEFEND = "RaidDefend";

		public const string CONTEXT_ID_RAID_INFO = "RaidBriefing";

		public const string CONTEXT_ID_NEXT_RAID_INFO = "NextRaid";

		public const string CONTEXT_ID_REQUEST_TROOPS = "RequestTroops";

		public const string CONTEXT_ID_REQUEST_TROOPS_PAID = "RequestTroopsPaid";

		public const string CONTEXT_ID_COMMISSION = "Commission";

		public const string CONTEXT_ID_MOBILIZE = "Mobilize";

		public const string CONTEXT_ID_REPAIR = "Repair";

		public const string CONTEXT_ID_UPGRADE_BUILDING = "Upgrade";

		public const string CONTEXT_ID_UPGRADE_TROOPS = "Upgrade_Troops";

		public const string CONTEXT_ID_UPGRADE_DEFENSE = "Upgrade_Defense";

		public const string CONTEXT_ID_COLLECT_CREDITS = "Credits";

		public const string CONTEXT_ID_COLLECT_MATERIALS = "Materials";

		public const string CONTEXT_ID_COLLECT_CONTRABAND = "Contraband";

		public const string CONTEXT_ID_CLEAR = "Clear";

		public const string CONTEXT_ID_UPGRADE_HEALTH = "Upgrade_Health";

		public const string CONTEXT_ID_UPGRADE_RANGE = "Upgrade_Range";

		public const string CONTEXT_ID_INVENTORY = "Inventory";

		public const string CONTEXT_ID_REARM_TRAP = "Trap_Rearm";

		public const string CONTEXT_ID_REARM_ALL_TRAPS = "Trap_RearmAll";

		public const string CONTEXT_ID_HIRE = "Hire";

		public const string CONTEXT_ID_ARMORY = "Armory";

		public const string TITLE_PREFIX_TROOP = "trp_title_";

		public const string DESC_PREFIX_TROOP = "trp_desc_";

		public const string NAME_PREFIX_PLANET = "planet_name_";

		public const string DESC_PREFIX_PLANET = "planet_desc_";

		public const string NAME_PREFIX_BUILDING = "bld_title_";

		public const string NAME_PREFIX_CRATE = "crate_title_";

		public const string NAME_PREFIX_LEI = "lei_title_";

		public const string TITLE_PREFIX_SPECIAL_ATTACK = "shp_title_";

		public const string DESC_PREFIX_SPECIAL_ATTACK = "shp_desc_";

		public const string TITLE_PREFIX_SKIN = "skn_title_";

		public const string FACTION_NAME_PREFIX = "FACTION_NAME_";

		public const string FACTION_SUFFIX_EMPIRE = "_e";

		public const string FACTION_SUFFIX_REBEL = "_r";

		private const string SQUAD_OWNER = "SQUAD_OWNER";

		private const string SQUAD_OFFICER = "SQUAD_OFFICER";

		private const string SQUAD_MEMBER = "SQUAD_MEMBER";

		private const string LANG_BATTLE_ON_PLANET = "LANG_BATTLE_ON_PLANET";

		public const string HOLONET_BATTLE_TRANSMISSION_BODY_PREFIX = "hn_battle_transmission_desc_";

		public const string HN_CHARACTER_NAME_TITLE = "hn_character_name_title";

		private const string EQUIPMENT_NAME_FORMAT = "{0}_name";

		private const string EQUIPMENT_DESCRIPTION_FORMAT = "{0}_description";

		private const string EQUIPMENT_LOCKED = "EQUIPMENT_LOCKED";

		private const string EQUIPMENT_QUALITY_BASIC = "EQUIPMENT_QUALITY_BASIC";

		private const string EQUIPMENT_QUALITY_ADVANCED = "EQUIPMENT_QUALITY_ADVANCED";

		private const string EQUIPMENT_QUALITY_ELITE = "EQUIPMENT_QUALITY_ELITE";

		public static void CreateLangService()
		{
			new EnvironmentController();
			string locale = PlayerSettings.GetLocaleCopy();
			string json;
			if (!LangUtils.TryLoadLocalStringsFile(locale, out json))
			{
				locale = Service.Get<EnvironmentController>().GetLocale();
				if (!LangUtils.TryLoadLocalStringsFile(locale, out json))
				{
					locale = "en_US";
					LangUtils.TryLoadLocalStringsFile(locale, out json);
				}
			}
			Lang lang = new Lang(locale);
			lang.AddStringData(json);
		}

		public static void AddLocalStringsData(string locale)
		{
			string json;
			if (LangUtils.TryLoadLocalStringsFile(locale, out json))
			{
				Service.Get<Lang>().AddStringData(json);
			}
		}

		private static bool TryLoadLocalStringsFile(string locale, out string json)
		{
			if (!string.IsNullOrEmpty(locale))
			{
				TextAsset textAsset = Resources.Load(string.Format("strings_local_{0}.local.json", new object[]
				{
					Lang.ToDotNetLocale(locale)
				})) as TextAsset;
				if (textAsset != null)
				{
					json = textAsset.text;
					return !string.IsNullOrEmpty(json);
				}
			}
			json = null;
			return false;
		}

		public static void LoadStringData(AssetsCompleteDelegate onComplete)
		{
			Lang lang = Service.Get<Lang>();
			List<string> list = new List<string>();
			list.Add(string.Format("strings_{0}.json.joe", new object[]
			{
				lang.DotNetLocale
			}));
			list.Add(string.Format("strings-hn_{0}.json.joe", new object[]
			{
				lang.DotNetLocale
			}));
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				string assetName = list[i];
				Service.Get<AssetManager>().AddJoeFileToManifest(assetName, "strings");
				Service.Get<AssetManager>().RegisterPreloadableAsset(assetName);
				i++;
			}
			lang.LoadAssets(list, new AssetSuccessDelegate(LangUtils.OnLangComplete), new AssetFailureDelegate(LangUtils.OnLangFailure), onComplete);
		}

		private static void OnLangComplete(object asset, object cookie)
		{
			byte[] binaryContents = Service.Get<AssetManager>().GetBinaryContents(asset);
			if (binaryContents == null)
			{
				AlertScreen.ShowModal(true, null, Service.Get<Lang>().Get(LangUtils.DESYNC_BATCH_MAX_RETRY, new object[0]), null, null);
				return;
			}
			JoeFile joe = new JoeFile(binaryContents);
			Lang lang = Service.Get<Lang>();
			lang.AddStringData(joe);
		}

		private static void OnLangFailure(object cookie)
		{
			Lang lang = Service.Get<Lang>();
			string text = "en_US";
			if (lang.Locale != text)
			{
				lang.Locale = text;
				Service.Get<ServerPlayerPrefs>().SetPref(ServerPref.Locale, text);
				LangUtils.AddLocalStringsData(text);
				lang.UnloadAssets();
				LangUtils.LoadStringData((AssetsCompleteDelegate)cookie);
				return;
			}
			LangUtils.OnLangComplete(null, cookie);
		}

		public static string ProcessStringWithNewlines(string str)
		{
			return str.Replace("\\n", "\n");
		}

		public static string GetLevelText(int level)
		{
			return Service.Get<Lang>().Get("TROOP_LVL", new object[]
			{
				StringUtils.GetRomanNumeral(level)
			});
		}

		public static string GetMissionButtonDisplayText(MissionType type)
		{
			switch (type)
			{
			case MissionType.Attack:
			case MissionType.Pvp:
				return Service.Get<Lang>().Get("BUTTON_MISSION_ATTACK", new object[0]);
			case MissionType.Defend:
			case MissionType.RaidDefend:
				return Service.Get<Lang>().Get("BUTTON_MISSION_DEFEND", new object[0]);
			case MissionType.Collect:
				return Service.Get<Lang>().Get("BUTTON_MISSION_COLLECT", new object[0]);
			case MissionType.Own:
				return Service.Get<Lang>().Get("BUTTON_MISSION_OWN", new object[0]);
			case MissionType.Event:
				return Service.Get<Lang>().Get("BUTTON_MISSION_EVENT", new object[0]);
			default:
				return Service.Get<Lang>().Get("BUTTON_MISSION_ATTACK", new object[0]);
			}
		}

		public static string GetMissionGoal(CampaignMissionVO mission)
		{
			if (string.IsNullOrEmpty(mission.GoalString))
			{
				return string.Empty;
			}
			return Service.Get<Lang>().Get(mission.GoalString, new object[0]);
		}

		public static string GetMissionFailureMessage(CampaignMissionVO mission)
		{
			if (string.IsNullOrEmpty(mission.GoalFailureString))
			{
				return string.Empty;
			}
			return Service.Get<Lang>().Get(mission.GoalFailureString, new object[0]);
		}

		public static string GetContextButtonText(string contextId)
		{
			string text = "context_" + contextId;
			string result;
			if (!Service.Get<Lang>().GetOptional(text, out result))
			{
				result = text;
			}
			return result;
		}

		public static string GetHolonetTransmissionCharacterName(TransmissionVO vo)
		{
			Lang lang = Service.Get<Lang>();
			string characterID = vo.CharacterID;
			IDataController dataController = Service.Get<IDataController>();
			foreach (TransmissionCharacterVO current in dataController.GetAll<TransmissionCharacterVO>())
			{
				if (current.CharacterId == characterID)
				{
					return lang.Get("hn_character_name_title", new object[]
					{
						lang.Get(current.CharacterName, new object[0])
					});
				}
			}
			return "";
		}

		public static string GetHolonetBattleTransmissionDescText(TransmissionVO vo)
		{
			Lang lang = Service.Get<Lang>();
			StringBuilder stringBuilder = new StringBuilder("hn_battle_transmission_desc_");
			stringBuilder.Append(vo.Faction.ToString().ToLower());
			return lang.Get(stringBuilder.ToString(), new object[0]);
		}

		public static string GetPlanetDisplayName(PlanetVO planetInfo)
		{
			return LangUtils.GetPlanetDisplayName(planetInfo.Uid);
		}

		public static string GetPlanetDisplayName(string Uid)
		{
			return Service.Get<Lang>().Get("planet_name_" + Uid, new object[0]);
		}

		public static string GetPlanetDisplayNameKey(string Uid)
		{
			return "planet_name_" + Uid;
		}

		public static string GetPlanetDescription(PlanetVO planetInfo)
		{
			return Service.Get<Lang>().Get("planet_desc_" + planetInfo.Uid, new object[0]);
		}

		public static string GetBuildingDisplayName(BuildingTypeVO buildingInfo)
		{
			return Service.Get<Lang>().Get("bld_title_" + buildingInfo.BuildingID, new object[0]);
		}

		public static string GetCrateDisplayName(string crateId)
		{
			IDataController dataController = Service.Get<IDataController>();
			CrateVO crateVO = dataController.Get<CrateVO>(crateId);
			return LangUtils.GetCrateDisplayName(crateVO);
		}

		public static string GetCrateDisplayName(CrateVO crateVO)
		{
			return Service.Get<Lang>().Get("crate_title_" + crateVO.Uid, new object[0]);
		}

		public static string GetLEIDisplayName(string uid)
		{
			return Service.Get<Lang>().Get("lei_title_" + uid, new object[0]);
		}

		public static string GetClearableDisplayName(BuildingTypeVO buildingInfo)
		{
			StringBuilder stringBuilder = new StringBuilder("bld_title_");
			stringBuilder.Append(buildingInfo.BuildingID);
			stringBuilder.Append("-");
			stringBuilder.Append(Service.Get<CurrentPlayer>().Planet.Abbreviation);
			return Service.Get<Lang>().Get(stringBuilder.ToString(), new object[0]);
		}

		public static string GetBuildingDescription(BuildingTypeVO buildingInfo)
		{
			return Service.Get<Lang>().Get("bld_desc_" + buildingInfo.BuildingID, new object[0]);
		}

		public static string GetHeroAbilityDisplayName(TroopAbilityVO abilityInfo)
		{
			return Service.Get<Lang>().Get("ability_title_" + abilityInfo.Uid, new object[0]);
		}

		public static string GetHeroAbilityDescription(TroopAbilityVO abilityInfo)
		{
			return Service.Get<Lang>().Get("ability_desc_" + abilityInfo.Uid, new object[0]);
		}

		public static string GetBuildingVerb(BuildingType buildingType)
		{
			Lang lang = Service.Get<Lang>();
			switch (buildingType)
			{
			case BuildingType.Barracks:
				return lang.Get("context_Train", new object[0]);
			case BuildingType.Factory:
				return lang.Get("context_Build", new object[0]);
			case BuildingType.FleetCommand:
				return lang.Get("context_Commission", new object[0]);
			case BuildingType.HeroMobilizer:
				return lang.Get("context_Mobilize", new object[0]);
			case BuildingType.ChampionPlatform:
				return lang.Get("context_Repair", new object[0]);
			case BuildingType.Housing:
			case BuildingType.Starport:
			case BuildingType.Wall:
			case BuildingType.Turret:
				break;
			case BuildingType.Squad:
				return lang.Get("context_Squad", new object[0]);
			case BuildingType.DroidHut:
				return lang.Get("context_Buy_Droid", new object[0]);
			case BuildingType.TroopResearch:
				return lang.Get("context_Upgrade_Troops", new object[0]);
			case BuildingType.DefenseResearch:
				return lang.Get("context_Upgrade_Defense", new object[0]);
			default:
				if (buildingType == BuildingType.Cantina)
				{
					return lang.Get("context_Hire", new object[0]);
				}
				if (buildingType == BuildingType.Armory)
				{
					return lang.Get("context_Armory", new object[0]);
				}
				break;
			}
			return null;
		}

		public static string GetTroopDisplayName(TroopTypeVO troopInfo)
		{
			return Service.Get<Lang>().Get("trp_title_" + troopInfo.TroopID, new object[0]);
		}

		public static string GetTroopDisplayNameFromTroopID(string troopID)
		{
			return Service.Get<Lang>().Get("trp_title_" + troopID, new object[0]);
		}

		public static string GetTroopDescription(TroopTypeVO troopInfo)
		{
			return Service.Get<Lang>().Get("trp_desc_" + troopInfo.TroopID, new object[0]);
		}

		public static string GetStarshipDisplayName(SpecialAttackTypeVO starshipInfo)
		{
			return Service.Get<Lang>().Get("shp_title_" + starshipInfo.SpecialAttackID, new object[0]);
		}

		public static string GetStarshipDisplayNameFromAttackID(string specialAttackID)
		{
			return Service.Get<Lang>().Get("shp_title_" + specialAttackID, new object[0]);
		}

		public static string GetStarshipDescription(SpecialAttackTypeVO starshipInfo)
		{
			return Service.Get<Lang>().Get("shp_desc_" + starshipInfo.SpecialAttackID, new object[0]);
		}

		public static string GetSkinDisplayName(SkinTypeVO skinfo)
		{
			return Service.Get<Lang>().Get("skn_title_" + skinfo.Uid, new object[0]);
		}

		public static string GetCampaignTitle(CampaignVO campaignType)
		{
			return Service.Get<Lang>().Get("cmp_title_" + campaignType.Uid, new object[0]);
		}

		public static string GetCampaignDescription(CampaignVO campaignType)
		{
			return Service.Get<Lang>().Get("cmp_desc_" + campaignType.Uid, new object[0]);
		}

		public static string GetTournamentTitle(TournamentVO tournamentVO)
		{
			return Service.Get<Lang>().Get("tournament_title_" + tournamentVO.Uid, new object[0]);
		}

		public static string GetMissionTitle(CampaignMissionVO missionType)
		{
			return Service.Get<Lang>().Get("mis_title_" + missionType.Uid, new object[0]);
		}

		public static string GetBattleName(BattleTypeVO battleType)
		{
			return Service.Get<Lang>().Get(battleType.BattleName, new object[0]);
		}

		public static string GetBattleOnPlanetName(BattleTypeVO battle)
		{
			return Service.Get<Lang>().Get("LANG_BATTLE_ON_PLANET", new object[]
			{
				LangUtils.GetBattleName(battle),
				LangUtils.GetPlanetDisplayName(battle.Planet)
			});
		}

		public static string GetMissionDescription(CampaignMissionVO missionType)
		{
			return Service.Get<Lang>().Get("mis_desc_" + missionType.Uid, new object[0]);
		}

		public static string GetMultiplierText(int count)
		{
			return Service.Get<Lang>().Get("TROOP_MULTIPLIER", new object[]
			{
				count
			});
		}

		public static string GetCurrencyStringId(CurrencyType currencyType)
		{
			return currencyType.ToString().ToUpper();
		}

		public static string GetSquadRoleDisplayName(SquadRole role)
		{
			string result = null;
			switch (role)
			{
			case SquadRole.Member:
				result = Service.Get<Lang>().Get("SQUAD_MEMBER", new object[0]);
				break;
			case SquadRole.Officer:
				result = Service.Get<Lang>().Get("SQUAD_OFFICER", new object[0]);
				break;
			case SquadRole.Owner:
				result = Service.Get<Lang>().Get("SQUAD_OWNER", new object[0]);
				break;
			}
			return result;
		}

		public static string GetEquipmentDisplayNameById(string equipmentId)
		{
			EquipmentVO currentEquipmentDataByID = ArmoryUtils.GetCurrentEquipmentDataByID(equipmentId);
			return LangUtils.GetEquipmentDisplayName(currentEquipmentDataByID);
		}

		public static string GetEquipmentDisplayName(EquipmentVO vo)
		{
			return Service.Get<Lang>().Get(string.Format("{0}_name", new object[]
			{
				vo.EquipmentID
			}), new object[0]);
		}

		public static string GetEquipmentDescription(EquipmentVO vo)
		{
			return Service.Get<Lang>().Get(string.Format("{0}_description", new object[]
			{
				vo.EquipmentID
			}), new object[0]);
		}

		public static string FormatTime(long seconds)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)seconds);
			int days = timeSpan.get_Days();
			int hours = timeSpan.get_Hours();
			int minutes = timeSpan.get_Minutes();
			int seconds2 = timeSpan.get_Seconds();
			Lang lang = Service.Get<Lang>();
			string result;
			if (days > 0)
			{
				result = lang.Get("s_timespan_days", new object[]
				{
					days,
					hours,
					minutes,
					seconds2
				});
			}
			else if (hours > 0)
			{
				result = lang.Get("s_timespan_hours", new object[]
				{
					hours,
					minutes,
					seconds2
				});
			}
			else if (minutes > 0)
			{
				result = lang.Get("s_timespan_minutes", new object[]
				{
					minutes,
					seconds2
				});
			}
			else
			{
				result = lang.Get("s_timespan_seconds", new object[]
				{
					seconds2
				});
			}
			return result;
		}

		public static string GetDeltaString(int delta)
		{
			string result;
			if (delta >= 0)
			{
				result = Service.Get<Lang>().Get("PLUS", new object[]
				{
					delta
				});
			}
			else
			{
				result = delta.ToString();
			}
			return result;
		}

		public static char OnValidateWNewLines(string text, int charIndex, char addedChar)
		{
			if (addedChar == '\n' || addedChar == '\r')
			{
				return '\0';
			}
			return addedChar;
		}

		public static char OnValidateWSpaces(string text, int charIndex, char addedChar)
		{
			if (addedChar == ' ')
			{
				return '\0';
			}
			return LangUtils.OnValidate(text, charIndex, addedChar);
		}

		public static char OnValidate(string text, int charIndex, char addedChar)
		{
			if (addedChar != '/' && addedChar != '\\' && addedChar != '@' && addedChar != '*' && addedChar != '?' && addedChar != '!' && addedChar != '#' && addedChar != '<' && addedChar != '>' && addedChar != '&' && addedChar != '%' && addedChar != '$' && addedChar != '"')
			{
				return addedChar;
			}
			return '\0';
		}

		public static bool ShouldPlayVOClips()
		{
			string locale = Service.Get<Lang>().Locale;
			return !GameConstants.VO_BLACKLIST.Contains(locale);
		}

		public static string GetMissionDifficultyLabel(string missionUid)
		{
			string[] array = missionUid.Split(new char[]
			{
				'_'
			});
			int num = Convert.ToInt32(array[array.Length - 1], CultureInfo.InvariantCulture);
			Lang lang = Service.Get<Lang>();
			switch (num)
			{
			case 1:
				return lang.Get("MISSION_DIFFICULTY_EASY", new object[0]);
			case 2:
				return lang.Get("MISSION_DIFFICULTY_MEDIUM", new object[0]);
			case 3:
				return lang.Get("MISSION_DIFFICULTY_HARD", new object[0]);
			default:
				return null;
			}
		}

		public static string GetFactionName(FactionType faction)
		{
			Lang lang = Service.Get<Lang>();
			return lang.Get("FACTION_NAME_" + faction.ToString().ToUpper(), new object[0]);
		}

		public static string GetEnemyFactionName(FactionType faction)
		{
			Lang lang = Service.Get<Lang>();
			if (faction == FactionType.Empire)
			{
				return lang.Get("FACTION_NAME_" + FactionType.Rebel.ToString().ToUpper(), new object[0]);
			}
			if (faction != FactionType.Rebel)
			{
				return FactionType.Neutral.ToString();
			}
			return lang.Get("FACTION_NAME_" + FactionType.Empire.ToString().ToUpper(), new object[0]);
		}

		public static string AppendPlayerFactionToKey(string key)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			StringBuilder stringBuilder = new StringBuilder(key);
			if (currentPlayer.Faction == FactionType.Empire)
			{
				stringBuilder.Append("_e");
			}
			else
			{
				stringBuilder.Append("_r");
			}
			return stringBuilder.ToString();
		}

		public static string GetShardLockedDeployableString(IDeployableVO vo)
		{
			int upgradeShardCount = vo.UpgradeShardCount;
			return LangUtils.GetShardLockedItemString(vo.UpgradeShardUid, upgradeShardCount);
		}

		public static string GetShardLockedEquipmentString(EquipmentVO equipmentVO)
		{
			return LangUtils.GetShardLockedItemString(equipmentVO.EquipmentID, equipmentVO.UpgradeShards);
		}

		private static string GetShardLockedItemString(string shardId, int requiredShards)
		{
			string result = string.Empty;
			int shardAmount = Service.Get<DeployableShardUnlockController>().GetShardAmount(shardId);
			int num = requiredShards - shardAmount;
			if (num > 0)
			{
				result = Service.Get<Lang>().Get("EQUIPMENT_LOCKED", new object[]
				{
					num
				});
			}
			return result;
		}

		public static string GetShardQuality(ShardQuality quality)
		{
			Lang lang = Service.Get<Lang>();
			string id;
			if (quality != ShardQuality.Advanced)
			{
				if (quality != ShardQuality.Elite)
				{
					id = "EQUIPMENT_QUALITY_BASIC";
				}
				else
				{
					id = "EQUIPMENT_QUALITY_ELITE";
				}
			}
			else
			{
				id = "EQUIPMENT_QUALITY_ADVANCED";
			}
			return lang.Get(id, new object[0]);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			LangUtils.AddLocalStringsData(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.AppendPlayerFactionToKey(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			LangUtils.CreateLangService();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.FormatTime(*args));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetBattleName((BattleTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetBattleOnPlanetName((BattleTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetBuildingDescription((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetBuildingDisplayName((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetBuildingVerb((BuildingType)(*(int*)args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetCampaignDescription((CampaignVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetCampaignTitle((CampaignVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetClearableDisplayName((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetContextButtonText(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetCrateDisplayName((CrateVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetCrateDisplayName(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetCurrencyStringId((CurrencyType)(*(int*)args)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetDeltaString(*(int*)args));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetEnemyFactionName((FactionType)(*(int*)args)));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetEquipmentDescription((EquipmentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetEquipmentDisplayName((EquipmentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetEquipmentDisplayNameById(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetFactionName((FactionType)(*(int*)args)));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetHeroAbilityDescription((TroopAbilityVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetHeroAbilityDisplayName((TroopAbilityVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetHolonetBattleTransmissionDescText((TransmissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetHolonetTransmissionCharacterName((TransmissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetLEIDisplayName(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetLevelText(*(int*)args));
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetMissionButtonDisplayText((MissionType)(*(int*)args)));
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetMissionDescription((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetMissionDifficultyLabel(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetMissionFailureMessage((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetMissionGoal((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetMissionTitle((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetMultiplierText(*(int*)args));
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetPlanetDescription((PlanetVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetPlanetDisplayName((PlanetVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetPlanetDisplayName(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetPlanetDisplayNameKey(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetShardLockedDeployableString((IDeployableVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetShardLockedEquipmentString((EquipmentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetShardLockedItemString(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetShardQuality((ShardQuality)(*(int*)args)));
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetSkinDisplayName((SkinTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetSquadRoleDisplayName((SquadRole)(*(int*)args)));
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetStarshipDescription((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetStarshipDisplayName((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetStarshipDisplayNameFromAttackID(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetTournamentTitle((TournamentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetTroopDescription((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetTroopDisplayName((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.GetTroopDisplayNameFromTroopID(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			LangUtils.LoadStringData((AssetsCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			LangUtils.OnLangComplete(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			LangUtils.OnLangFailure(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.ProcessStringWithNewlines(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LangUtils.ShouldPlayVOClips());
		}
	}
}
