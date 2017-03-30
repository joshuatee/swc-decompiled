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
						Dictionary<string, int> troopData;
						Dictionary<string, int> expr_F9 = troopData = battleDeploymentData.TroopData;
						string key;
						string expr_103 = key = current2.TroopUid;
						int num = troopData[key];
						expr_F9[expr_103] = num + current2.Quantity;
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
						Dictionary<string, int> defenderGuildTroopsAvailable;
						Dictionary<string, int> expr_3A0 = defenderGuildTroopsAvailable = battleInitializationData.DefenderGuildTroopsAvailable;
						string key;
						string expr_3A5 = key = troopUid;
						int num = defenderGuildTroopsAvailable[key];
						expr_3A0[expr_3A5] = num + totalAmount;
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
						Dictionary<string, int> dictionary2;
						Dictionary<string, int> expr_4B = dictionary2 = dictionary;
						string key;
						string expr_4F = key = troopUid;
						int num = dictionary2[key];
						expr_4B[expr_4F] = num + totalAmount;
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
			battleInitializationData.FailureCondition = ((!string.IsNullOrEmpty(battleRecord.failureConditionUid)) ? dataController.Get<ConditionVO>(battleRecord.failureConditionUid) : null);
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
	}
}
