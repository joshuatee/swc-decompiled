using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Controllers.VictoryConditions;
using StaRTS.Main.Models.Battle.Replay;
using StaRTS.Main.Models.Commands.Pvp;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle
{
	public class BattleInitializationData
	{
		public string RecordId
		{
			get;
			set;
		}

		public BattleType BattleType
		{
			get;
			set;
		}

		public int BattleLength
		{
			get;
			set;
		}

		public int LootCreditsAvailable
		{
			get;
			set;
		}

		public int LootMaterialsAvailable
		{
			get;
			set;
		}

		public int LootContrabandAvailable
		{
			get;
			set;
		}

		public Dictionary<string, int> BuildingLootCreditsMap
		{
			get;
			set;
		}

		public Dictionary<string, int> BuildingLootMaterialsMap
		{
			get;
			set;
		}

		public Dictionary<string, int> BuildingLootContrabandMap
		{
			get;
			set;
		}

		public int LootCreditsEarned
		{
			get;
			set;
		}

		public int LootMaterialsEarned
		{
			get;
			set;
		}

		public int LootContrabandEarned
		{
			get;
			set;
		}

		public int LootCreditsDeducted
		{
			get;
			set;
		}

		public int LootMaterialsDeducted
		{
			get;
			set;
		}

		public int LootContrabandDeducted
		{
			get;
			set;
		}

		public BattleDeploymentData AttackerDeployableData
		{
			get;
			private set;
		}

		public BattleDeploymentData DefenderDeployableData
		{
			get;
			private set;
		}

		public BattleParticipant Attacker
		{
			get;
			set;
		}

		public BattleParticipant Defender
		{
			get;
			set;
		}

		public BattleTypeVO BattleVO
		{
			get;
			set;
		}

		public string PlanetId
		{
			get;
			set;
		}

		public string MissionUid
		{
			get;
			private set;
		}

		public PvpTarget PvpTarget
		{
			get;
			set;
		}

		public SquadMemberWarData MemberWarData
		{
			get;
			set;
		}

		public Dictionary<string, int> DefenderGuildTroopsAvailable
		{
			get;
			set;
		}

		public Dictionary<string, int> AttackerGuildTroopsAvailable
		{
			get;
			set;
		}

		public Dictionary<string, int> DefenderChampionsAvailable
		{
			get;
			set;
		}

		public string BattleMusic
		{
			get;
			set;
		}

		public string AmbientMusic
		{
			get;
			set;
		}

		public List<ConditionVO> VictoryConditions
		{
			get;
			set;
		}

		public ConditionVO FailureCondition
		{
			get;
			set;
		}

		public bool IsReplay
		{
			get;
			set;
		}

		public bool IsRevenge
		{
			get;
			set;
		}

		public string DefenseEncounterProfile
		{
			get;
			private set;
		}

		public string BattleScript
		{
			get;
			private set;
		}

		public bool AllowReplay
		{
			get;
			private set;
		}

		public List<string> DisabledBuildings
		{
			get;
			private set;
		}

		public bool AllowMultipleHeroDeploys
		{
			get;
			private set;
		}

		public bool OverrideDeployables
		{
			get;
			private set;
		}

		public List<string> AttackerWarBuffs
		{
			get;
			set;
		}

		public List<string> DefenderWarBuffs
		{
			get;
			set;
		}

		public List<string> AttackerEquipment
		{
			get;
			set;
		}

		public List<string> DefenderEquipment
		{
			get;
			set;
		}

		public static BattleInitializationData CreateFromDefensiveCampaignMissionVO(string id)
		{
			IDataController dataController = Service.Get<IDataController>();
			CampaignMissionVO vo = dataController.Get<CampaignMissionVO>(id);
			return BattleInitializationData.CreateFromDefensiveCampaignMissionVO(vo);
		}

		public static BattleInitializationData CreateFromDefensiveCampaignMissionVO(CampaignMissionVO vo)
		{
			IDataController dataController = Service.Get<IDataController>();
			string uid = vo.Uid;
			BattleInitializationData battleInitializationData = new BattleInitializationData();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			battleInitializationData.Attacker = new BattleParticipant(vo.OpponentName, vo.OpponentName, FactionType.Invalid);
			battleInitializationData.Defender = new BattleParticipant(currentPlayer.PlayerId, currentPlayer.PlayerName, currentPlayer.Faction);
			battleInitializationData.PlanetId = currentPlayer.Map.Planet.Uid;
			battleInitializationData.BattleType = BattleType.PveDefend;
			battleInitializationData.MissionUid = vo.Uid;
			BattleDeploymentData battleDeploymentData = new BattleDeploymentData();
			battleDeploymentData.TroopData = new Dictionary<string, int>();
			List<DefenseWave> list = DefensiveBattleController.ParseWaves(vo.Waves);
			foreach (DefenseWave current in list)
			{
				List<DefenseTroopGroup> list2 = DefensiveBattleController.ParseTroopGroups(uid, current.Encounter.WaveGroup, 0);
				foreach (DefenseTroopGroup current2 in list2)
				{
					if (battleDeploymentData.TroopData.ContainsKey(current2.TroopUid))
					{
						Dictionary<string, int> dictionary = battleDeploymentData.TroopData;
						string key = current2.TroopUid;
						dictionary[key] += current2.Quantity;
					}
					else
					{
						battleDeploymentData.TroopData.Add(current2.TroopUid, current2.Quantity);
					}
				}
			}
			battleInitializationData.AttackerDeployableData = battleDeploymentData;
			if (vo.Map != null)
			{
				battleInitializationData.BattleVO = dataController.Get<BattleTypeVO>(vo.Map);
				battleInitializationData.AllowMultipleHeroDeploys = (battleInitializationData.BattleVO.MultipleHeroDeploys || vo.IsRaidDefense());
				battleInitializationData.OverrideDeployables = battleInitializationData.BattleVO.OverridePlayerUnits;
				battleInitializationData.BattleLength = battleInitializationData.BattleVO.BattleTime;
				battleInitializationData.DefenderDeployableData = BattleDeploymentData.Copy(battleInitializationData.BattleVO);
			}
			else
			{
				battleInitializationData.DefenderDeployableData = BattleDeploymentData.CreateEmpty();
				battleInitializationData.AllowMultipleHeroDeploys = vo.IsRaidDefense();
				battleInitializationData.OverrideDeployables = false;
			}
			Inventory inventory = currentPlayer.Inventory;
			battleInitializationData.LootCreditsAvailable = inventory.GetItemAmount("credits");
			battleInitializationData.LootMaterialsAvailable = inventory.GetItemAmount("materials");
			battleInitializationData.LootContrabandAvailable = inventory.GetItemAmount("contraband");
			if (battleInitializationData.BattleLength == 0)
			{
				battleInitializationData.BattleLength = GameConstants.PVP_MATCH_DURATION;
			}
			battleInitializationData.BattleMusic = vo.BattleMusic;
			battleInitializationData.AmbientMusic = vo.AmbientMusic;
			battleInitializationData.VictoryConditions = vo.Conditions;
			if (!string.IsNullOrEmpty(vo.FailureCondition))
			{
				battleInitializationData.FailureCondition = dataController.Get<ConditionVO>(vo.FailureCondition);
			}
			battleInitializationData.DisabledBuildings = new List<string>();
			List<Contract> list3 = Service.Get<ISupportController>().FindAllContractsThatConsumeDroids();
			for (int i = 0; i < list3.Count; i++)
			{
				if (list3[i].DeliveryType != DeliveryType.ClearClearable)
				{
					battleInitializationData.DisabledBuildings.Add(list3[i].ContractTO.BuildingKey);
				}
			}
			battleInitializationData.IsReplay = false;
			battleInitializationData.IsRevenge = false;
			battleInitializationData.AllowReplay = false;
			if (vo.IsRaidDefense() && Service.Get<RaidDefenseController>().SquadTroopDeployAllowed())
			{
				battleInitializationData.DefenderGuildTroopsAvailable = new Dictionary<string, int>();
				List<SquadDonatedTroop> troops = Service.Get<SquadController>().StateManager.Troops;
				for (int j = 0; j < troops.Count; j++)
				{
					string troopUid = troops[j].TroopUid;
					int totalAmount = troops[j].GetTotalAmount();
					if (battleInitializationData.DefenderGuildTroopsAvailable.ContainsKey(troopUid))
					{
						Dictionary<string, int> dictionary = battleInitializationData.DefenderGuildTroopsAvailable;
						string key = troopUid;
						dictionary[key] += totalAmount;
					}
					else
					{
						battleInitializationData.DefenderGuildTroopsAvailable.Add(troopUid, totalAmount);
					}
				}
			}
			battleInitializationData.AttackerEquipment = null;
			battleInitializationData.DefenderEquipment = BattleInitializationData.GetCurrentPlayerEquipment(battleInitializationData.PlanetId);
			return battleInitializationData;
		}

		public static BattleInitializationData CreateFromCampaignMissionVO(string id)
		{
			IDataController dataController = Service.Get<IDataController>();
			CampaignMissionVO campaignMissionVO = dataController.Get<CampaignMissionVO>(id);
			return BattleInitializationData.CreateFromCampaignMissionAndBattle(id, campaignMissionVO.Map);
		}

		public static BattleInitializationData CreateFromCampaignMissionAndBattle(string id, string battleUid)
		{
			IDataController dataController = Service.Get<IDataController>();
			CampaignMissionVO campaignMissionVO = dataController.Get<CampaignMissionVO>(id);
			BattleInitializationData battleInitializationData = BattleInitializationData.CreateFromBattleTypeVO(battleUid);
			CampaignProgress campaignProgress = Service.Get<CurrentPlayer>().CampaignProgress;
			int missionLootCreditsRemaining = campaignProgress.GetMissionLootCreditsRemaining(campaignMissionVO);
			int missionLootMaterialsRemaining = campaignProgress.GetMissionLootMaterialsRemaining(campaignMissionVO);
			int missionLootContrabandRemaining = campaignProgress.GetMissionLootContrabandRemaining(campaignMissionVO);
			battleInitializationData.MissionUid = campaignMissionVO.Uid;
			battleInitializationData.LootCreditsAvailable = missionLootCreditsRemaining;
			battleInitializationData.LootMaterialsAvailable = missionLootMaterialsRemaining;
			battleInitializationData.LootContrabandAvailable = missionLootContrabandRemaining;
			battleInitializationData.BattleType = BattleType.PveAttack;
			battleInitializationData.BattleMusic = campaignMissionVO.BattleMusic;
			battleInitializationData.AmbientMusic = campaignMissionVO.AmbientMusic;
			battleInitializationData.BattleVO = dataController.Get<BattleTypeVO>(battleUid);
			battleInitializationData.AllowMultipleHeroDeploys = battleInitializationData.BattleVO.MultipleHeroDeploys;
			battleInitializationData.OverrideDeployables = battleInitializationData.BattleVO.OverridePlayerUnits;
			battleInitializationData.VictoryConditions = campaignMissionVO.Conditions;
			if (!string.IsNullOrEmpty(campaignMissionVO.FailureCondition))
			{
				battleInitializationData.FailureCondition = dataController.Get<ConditionVO>(campaignMissionVO.FailureCondition);
			}
			battleInitializationData.IsReplay = false;
			battleInitializationData.IsRevenge = false;
			battleInitializationData.AllowReplay = false;
			battleInitializationData.AttackerEquipment = BattleInitializationData.GetCurrentPlayerEquipment(battleInitializationData.PlanetId);
			battleInitializationData.DefenderEquipment = null;
			return battleInitializationData;
		}

		public static BattleInitializationData CreateFromPvpTargetData(PvpTarget target, bool isRevenge)
		{
			BattleInitializationData battleInitializationData = new BattleInitializationData();
			battleInitializationData.RecordId = target.BattleId;
			battleInitializationData.BattleType = BattleType.Pvp;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			battleInitializationData.Attacker = new BattleParticipant(currentPlayer.PlayerId, currentPlayer.PlayerName, currentPlayer.Faction);
			battleInitializationData.Attacker.AttackRating = currentPlayer.AttackRating;
			battleInitializationData.Attacker.DefenseRating = currentPlayer.DefenseRating;
			battleInitializationData.Defender = new BattleParticipant(target.PlayerId, target.PlayerName, target.PlayerFaction);
			battleInitializationData.Defender.AttackRating = target.PlayerAttackRating;
			battleInitializationData.Defender.DefenseRating = target.PlayerDefenseRating;
			battleInitializationData.AttackerDeployableData = BattleDeploymentData.CreateEmpty();
			battleInitializationData.DefenderDeployableData = BattleDeploymentData.CreateEmpty();
			battleInitializationData.AllowMultipleHeroDeploys = false;
			battleInitializationData.OverrideDeployables = false;
			battleInitializationData.BattleLength = GameConstants.PVP_MATCH_DURATION;
			battleInitializationData.PlanetId = target.BaseMap.Planet.Uid;
			battleInitializationData.LootCreditsAvailable = target.AvailableCredits;
			battleInitializationData.LootMaterialsAvailable = target.AvailableMaterials;
			battleInitializationData.LootContrabandAvailable = target.AvailableContraband;
			battleInitializationData.BuildingLootCreditsMap = target.BuildingLootCreditsMap;
			battleInitializationData.BuildingLootMaterialsMap = target.BuildingLootMaterialsMap;
			battleInitializationData.BuildingLootContrabandMap = target.BuildingLootContrabandMap;
			battleInitializationData.PvpTarget = target;
			battleInitializationData.VictoryConditions = Service.Get<VictoryConditionController>().GetDefaultConditions();
			battleInitializationData.DefenderGuildTroopsAvailable = target.GuildDonatedTroops;
			battleInitializationData.DefenderChampionsAvailable = target.Champions;
			battleInitializationData.AttackerGuildTroopsAvailable = BattleInitializationData.GetCurrentPlayerGuildTroops();
			battleInitializationData.DisabledBuildings = new List<string>();
			for (int i = 0; i < target.Contracts.Count; i++)
			{
				if (target.Contracts[i].ContractType == ContractType.Build || target.Contracts[i].ContractType == ContractType.Upgrade)
				{
					battleInitializationData.DisabledBuildings.Add(target.Contracts[i].BuildingKey);
				}
			}
			battleInitializationData.IsReplay = false;
			battleInitializationData.IsRevenge = isRevenge;
			battleInitializationData.AllowReplay = true;
			battleInitializationData.AttackerEquipment = BattleInitializationData.GetCurrentPlayerEquipment(battleInitializationData.PlanetId);
			battleInitializationData.DefenderEquipment = target.Equipment;
			return battleInitializationData;
		}

		public static BattleInitializationData CreateFromMemberWarData(SquadMemberWarData memberWarData, Dictionary<string, int> donatedSquadTroops, Dictionary<string, int> champions, FactionType faction, string participantSquadId, List<string> equipment)
		{
			BattleInitializationData battleInitializationData = new BattleInitializationData();
			battleInitializationData.BattleType = BattleType.PvpAttackSquadWar;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			battleInitializationData.Attacker = new BattleParticipant(currentPlayer.PlayerId, currentPlayer.PlayerName, currentPlayer.Faction);
			battleInitializationData.Attacker.GuildId = Service.Get<SquadController>().StateManager.GetCurrentSquad().SquadID;
			battleInitializationData.Defender = new BattleParticipant(memberWarData.SquadMemberId, memberWarData.SquadMemberName, faction);
			battleInitializationData.Defender.GuildId = participantSquadId;
			battleInitializationData.AttackerDeployableData = BattleDeploymentData.CreateEmpty();
			battleInitializationData.DefenderDeployableData = BattleDeploymentData.CreateEmpty();
			battleInitializationData.AllowMultipleHeroDeploys = false;
			battleInitializationData.OverrideDeployables = false;
			battleInitializationData.BattleLength = GameConstants.PVP_MATCH_DURATION;
			battleInitializationData.PlanetId = memberWarData.BaseMap.Planet.Uid;
			battleInitializationData.MemberWarData = memberWarData;
			battleInitializationData.VictoryConditions = Service.Get<VictoryConditionController>().GetDefaultConditions();
			battleInitializationData.DefenderGuildTroopsAvailable = donatedSquadTroops;
			battleInitializationData.DefenderChampionsAvailable = champions;
			battleInitializationData.AttackerGuildTroopsAvailable = BattleInitializationData.GetCurrentPlayerGuildTroops();
			battleInitializationData.DisabledBuildings = null;
			battleInitializationData.IsReplay = false;
			battleInitializationData.IsRevenge = false;
			battleInitializationData.AllowReplay = true;
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			battleInitializationData.AttackerWarBuffs = warManager.GetBuffBasesOwnedBySquad(battleInitializationData.Attacker.GuildId);
			battleInitializationData.DefenderWarBuffs = warManager.GetBuffBasesOwnedBySquad(battleInitializationData.Defender.GuildId);
			battleInitializationData.AttackerEquipment = BattleInitializationData.GetCurrentPlayerEquipment(battleInitializationData.PlanetId);
			battleInitializationData.DefenderEquipment = equipment;
			return battleInitializationData;
		}

		private static Dictionary<string, int> GetCurrentPlayerGuildTroops()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			List<SquadDonatedTroop> troops = Service.Get<SquadController>().StateManager.Troops;
			if (troops != null)
			{
				for (int i = 0; i < troops.Count; i++)
				{
					string troopUid = troops[i].TroopUid;
					int totalAmount = troops[i].GetTotalAmount();
					if (dictionary.ContainsKey(troopUid))
					{
						Dictionary<string, int> dictionary2 = dictionary;
						string key = troopUid;
						dictionary2[key] += totalAmount;
					}
					else
					{
						dictionary.Add(troopUid, totalAmount);
					}
				}
			}
			return dictionary;
		}

		private static List<string> GetCurrentPlayerEquipment(string planetId)
		{
			return ArmoryUtils.GetValidEquipment(Service.Get<CurrentPlayer>(), Service.Get<IDataController>(), planetId);
		}

		public static BattleInitializationData CreateFromBattleTypeVO(string id)
		{
			BattleTypeVO battleTypeVO = Service.Get<IDataController>().Get<BattleTypeVO>(id);
			BattleInitializationData battleInitializationData = new BattleInitializationData();
			battleInitializationData.BattleType = BattleType.PveFue;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			battleInitializationData.Attacker = new BattleParticipant(currentPlayer.PlayerId, currentPlayer.PlayerName, currentPlayer.Faction);
			battleInitializationData.Defender = new BattleParticipant(battleTypeVO.DefenderId, battleTypeVO.DefenderId, FactionType.Invalid);
			battleInitializationData.BattleVO = battleTypeVO;
			battleInitializationData.AttackerDeployableData = BattleDeploymentData.Copy(battleTypeVO);
			battleInitializationData.DefenderDeployableData = BattleDeploymentData.CreateEmpty();
			battleInitializationData.AllowMultipleHeroDeploys = false;
			battleInitializationData.OverrideDeployables = false;
			battleInitializationData.PlanetId = battleTypeVO.Planet;
			battleInitializationData.BattleLength = battleTypeVO.BattleTime;
			if (battleInitializationData.BattleLength == 0)
			{
				battleInitializationData.BattleLength = GameConstants.PVP_MATCH_DURATION;
			}
			battleInitializationData.VictoryConditions = Service.Get<VictoryConditionController>().GetDefaultConditions();
			battleInitializationData.IsReplay = false;
			battleInitializationData.IsRevenge = false;
			battleInitializationData.DefenseEncounterProfile = battleTypeVO.EncounterProfile;
			battleInitializationData.BattleScript = battleTypeVO.BattleScript;
			battleInitializationData.AllowReplay = true;
			battleInitializationData.DisabledBuildings = new List<string>();
			return battleInitializationData;
		}

		public static BattleInitializationData CreateBuffBaseBattleFromCampaignMissionVO(CampaignMissionVO vo, SquadWarBuffBaseData buffBaseData)
		{
			IDataController dataController = Service.Get<IDataController>();
			BattleInitializationData battleInitializationData = BattleInitializationData.CreateBuffBaseBattleFromBattleTypeVO(vo.Map, buffBaseData);
			battleInitializationData.MissionUid = vo.Uid;
			battleInitializationData.BattleMusic = vo.BattleMusic;
			battleInitializationData.AmbientMusic = vo.AmbientMusic;
			battleInitializationData.VictoryConditions = vo.Conditions;
			if (!string.IsNullOrEmpty(vo.FailureCondition))
			{
				battleInitializationData.FailureCondition = dataController.Get<ConditionVO>(vo.FailureCondition);
			}
			return battleInitializationData;
		}

		private static BattleInitializationData CreateBuffBaseBattleFromBattleTypeVO(string id, SquadWarBuffBaseData buffBaseData)
		{
			BattleInitializationData battleInitializationData = BattleInitializationData.CreateFromBattleTypeVO(id);
			battleInitializationData.BattleType = BattleType.PveBuffBase;
			battleInitializationData.Attacker.GuildId = Service.Get<SquadController>().StateManager.GetCurrentSquad().SquadID;
			battleInitializationData.AttackerGuildTroopsAvailable = BattleInitializationData.GetCurrentPlayerGuildTroops();
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			battleInitializationData.AttackerWarBuffs = warManager.GetBuffBasesOwnedBySquad(battleInitializationData.Attacker.GuildId);
			battleInitializationData.DefenderWarBuffs = null;
			SquadWarSquadData squadData = warManager.GetSquadData(SquadWarSquadType.PLAYER_SQUAD);
			SquadWarSquadData squadData2 = warManager.GetSquadData(SquadWarSquadType.OPPONENT_SQUAD);
			string ownerId = buffBaseData.OwnerId;
			if (!string.IsNullOrEmpty(ownerId))
			{
				if (ownerId == squadData2.SquadId)
				{
					battleInitializationData.Defender.PlayerFaction = squadData2.Faction;
				}
				else if (ownerId == squadData.SquadId)
				{
					battleInitializationData.Defender.PlayerFaction = squadData.Faction;
				}
			}
			else
			{
				battleInitializationData.Defender.PlayerFaction = FactionType.Smuggler;
			}
			battleInitializationData.AttackerEquipment = BattleInitializationData.GetCurrentPlayerEquipment(battleInitializationData.PlanetId);
			battleInitializationData.DefenderEquipment = null;
			return battleInitializationData;
		}

		public static BattleInitializationData CreateFromReplay(BattleRecord battleRecord, BattleEntry battleEntry)
		{
			BattleInitializationData battleInitializationData = new BattleInitializationData();
			battleInitializationData.RecordId = battleRecord.RecordId;
			battleInitializationData.BattleType = battleRecord.BattleType;
			battleInitializationData.AttackerDeployableData = battleRecord.AttackerDeploymentData;
			battleInitializationData.DefenderDeployableData = battleRecord.DefenderDeploymentData;
			battleInitializationData.AllowMultipleHeroDeploys = false;
			battleInitializationData.OverrideDeployables = true;
			battleInitializationData.LootCreditsAvailable = battleRecord.LootCreditsAvailable;
			battleInitializationData.LootMaterialsAvailable = battleRecord.LootMaterialsAvailable;
			battleInitializationData.LootContrabandAvailable = battleRecord.LootContrabandAvailable;
			battleInitializationData.BuildingLootCreditsMap = battleRecord.BuildingLootCreditsMap;
			battleInitializationData.BuildingLootMaterialsMap = battleRecord.BuildingLootMaterialsMap;
			battleInitializationData.BuildingLootContrabandMap = battleRecord.BuildingLootContrabandMap;
			battleInitializationData.LootCreditsEarned = battleEntry.LootCreditsEarned;
			battleInitializationData.LootMaterialsEarned = battleEntry.LootMaterialsEarned;
			battleInitializationData.LootContrabandEarned = battleEntry.LootContrabandEarned;
			battleInitializationData.LootCreditsDeducted = battleEntry.LootCreditsDeducted;
			battleInitializationData.LootMaterialsDeducted = battleEntry.LootMaterialsDeducted;
			battleInitializationData.LootContrabandDeducted = battleEntry.LootContrabandDeducted;
			battleInitializationData.Attacker = battleEntry.Attacker;
			battleInitializationData.Defender = battleEntry.Defender;
			battleInitializationData.PlanetId = battleRecord.PlanetId;
			battleInitializationData.BattleLength = battleRecord.BattleLength;
			IDataController dataController = Service.Get<IDataController>();
			battleInitializationData.VictoryConditions = new List<ConditionVO>();
			int i = 0;
			int count = battleRecord.victoryConditionsUids.Count;
			while (i < count)
			{
				battleInitializationData.VictoryConditions.Add(dataController.Get<ConditionVO>(battleRecord.victoryConditionsUids[i]));
				i++;
			}
			battleInitializationData.FailureCondition = (string.IsNullOrEmpty(battleRecord.failureConditionUid) ? null : dataController.Get<ConditionVO>(battleRecord.failureConditionUid));
			battleInitializationData.DefenderGuildTroopsAvailable = battleRecord.DefenderGuildTroops;
			battleInitializationData.AttackerGuildTroopsAvailable = battleRecord.AttackerGuildTroops;
			battleInitializationData.DefenderChampionsAvailable = battleRecord.DefenderChampions;
			battleInitializationData.DisabledBuildings = battleRecord.DisabledBuildings;
			battleInitializationData.IsReplay = true;
			battleInitializationData.IsRevenge = false;
			battleInitializationData.DefenseEncounterProfile = battleRecord.DefenseEncounterProfile;
			battleInitializationData.BattleScript = battleRecord.BattleScript;
			battleInitializationData.AllowReplay = false;
			battleInitializationData.AttackerWarBuffs = battleRecord.AttackerWarBuffs;
			battleInitializationData.DefenderWarBuffs = battleRecord.DefenderWarBuffs;
			battleInitializationData.AttackerEquipment = battleRecord.AttackerEquipment;
			battleInitializationData.DefenderEquipment = battleRecord.DefenderEquipment;
			return battleInitializationData;
		}

		public static BattleInitializationData CreateEmpty(BattleType type)
		{
			return new BattleInitializationData
			{
				BattleType = type,
				AttackerDeployableData = BattleDeploymentData.CreateEmpty(),
				DefenderDeployableData = BattleDeploymentData.CreateEmpty(),
				AllowMultipleHeroDeploys = false,
				OverrideDeployables = false,
				BattleLength = GameConstants.PVP_MATCH_DURATION,
				VictoryConditions = Service.Get<VictoryConditionController>().GetDefaultConditions(),
				IsReplay = false,
				IsRevenge = false,
				AllowReplay = false
			};
		}

		public BattleInitializationData()
		{
		}

		protected internal BattleInitializationData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.CreateBuffBaseBattleFromBattleTypeVO(Marshal.PtrToStringUni(*(IntPtr*)args), (SquadWarBuffBaseData)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.CreateBuffBaseBattleFromCampaignMissionVO((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args), (SquadWarBuffBaseData)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.CreateEmpty((BattleType)(*(int*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.CreateFromBattleTypeVO(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.CreateFromCampaignMissionAndBattle(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.CreateFromCampaignMissionVO(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.CreateFromDefensiveCampaignMissionVO((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.CreateFromDefensiveCampaignMissionVO(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.CreateFromMemberWarData((SquadMemberWarData)GCHandledObjects.GCHandleToObject(*args), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[1]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[2]), (FactionType)(*(int*)(args + 3)), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)), (List<string>)GCHandledObjects.GCHandleToObject(args[5])));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.CreateFromPvpTargetData((PvpTarget)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.CreateFromReplay((BattleRecord)GCHandledObjects.GCHandleToObject(*args), (BattleEntry)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AllowMultipleHeroDeploys);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AllowReplay);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AmbientMusic);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).Attacker);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AttackerDeployableData);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AttackerEquipment);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AttackerGuildTroopsAvailable);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AttackerWarBuffs);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BattleLength);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BattleMusic);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BattleScript);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BattleType);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BattleVO);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BuildingLootContrabandMap);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BuildingLootCreditsMap);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BuildingLootMaterialsMap);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).Defender);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DefenderChampionsAvailable);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DefenderDeployableData);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DefenderEquipment);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DefenderGuildTroopsAvailable);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DefenderWarBuffs);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DefenseEncounterProfile);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DisabledBuildings);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).FailureCondition);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).IsReplay);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).IsRevenge);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootContrabandAvailable);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootContrabandDeducted);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootContrabandEarned);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootCreditsAvailable);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootCreditsDeducted);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootCreditsEarned);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsAvailable);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsDeducted);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsEarned);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).MemberWarData);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).MissionUid);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).OverrideDeployables);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).PlanetId);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).PvpTarget);
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).RecordId);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).VictoryConditions);
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.GetCurrentPlayerEquipment(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleInitializationData.GetCurrentPlayerGuildTroops());
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AllowMultipleHeroDeploys = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AllowReplay = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AmbientMusic = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).Attacker = (BattleParticipant)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AttackerDeployableData = (BattleDeploymentData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AttackerEquipment = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AttackerGuildTroopsAvailable = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).AttackerWarBuffs = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BattleLength = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BattleMusic = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BattleScript = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BattleType = (BattleType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BattleVO = (BattleTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BuildingLootContrabandMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BuildingLootCreditsMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).BuildingLootMaterialsMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).Defender = (BattleParticipant)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DefenderChampionsAvailable = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DefenderDeployableData = (BattleDeploymentData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DefenderEquipment = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DefenderGuildTroopsAvailable = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DefenderWarBuffs = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DefenseEncounterProfile = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).DisabledBuildings = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).FailureCondition = (ConditionVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).IsReplay = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).IsRevenge = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootContrabandAvailable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootContrabandDeducted = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootContrabandEarned = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootCreditsAvailable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootCreditsDeducted = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootCreditsEarned = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsAvailable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsDeducted = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsEarned = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).MemberWarData = (SquadMemberWarData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).MissionUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).OverrideDeployables = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).PlanetId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).PvpTarget = (PvpTarget)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).RecordId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			((BattleInitializationData)GCHandledObjects.GCHandleToObject(instance)).VictoryConditions = (List<ConditionVO>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
