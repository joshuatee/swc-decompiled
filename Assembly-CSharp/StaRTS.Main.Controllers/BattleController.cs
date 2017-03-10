using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.Externals.FileManagement;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.GameBoard;
using StaRTS.Main.Controllers.CombatTriggers;
using StaRTS.Main.Controllers.Entities.Systems;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Controllers.VictoryConditions;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Battle.Replay;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Commands.Missions;
using StaRTS.Main.Models.Commands.Player.Deployable;
using StaRTS.Main.Models.Commands.Player.Raids;
using StaRTS.Main.Models.Commands.Pve;
using StaRTS.Main.Models.Commands.Pvp;
using StaRTS.Main.Models.Commands.Squads;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.World;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class BattleController : IEventObserver, ISimTimeProvider
	{
		private const int BATTLE_TIMER_INTERVAL = 1000;

		private const string BATTLE_MESSAGE_LABEL_NAME = "BattleMessage";

		private const string DEATH_TRIGGER = "onDeath";

		private const string AREA_TRIGGER = "proximity";

		private const string LOAD_TRIGGER = "onLoad";

		private const string TROOP_DEPLOY_TRIGGER = "onTroopDeploy";

		private const int BUILDING_UID_ARG = 0;

		private const int TRIGGER_TYPE_ARG = 1;

		private const uint DEFAULT_LAST_DITCH_SECONDS = 90u;

		private const uint EFFECTIVELY_NEVER_SECONDS = 1800u;

		private const int BUILDING_DISTRUCTION_RANGE = 8;

		private const int STORY_ACTION_ARG = 2;

		private const string TEST_BATTLE_RECORD_ID = "test";

		private CurrentPlayer currentPlayer;

		private EntityController entityController;

		private EventManager eventManager;

		private VictoryConditionController conditionController;

		private DefensiveBattleController defenseController;

		private IDataController sdc;

		private CombatTriggerManager ctm;

		private uint battleTimer;

		private uint spendTroopTimer;

		private BattleMessageView battleMessageView;

		private Queue<VictoryStarAnimation> starAnimations;

		private bool starAnimating;

		private CurrentBattle currentBattle;

		private bool waitingToSendBattleEndCommand;

		private bool gotBattleEndResponse;

		private bool completedBattleEndDelayTimer;

		private List<DeploymentRecord> troopsSpendDelta;

		private Dictionary<string, int> seededTroopDataRemaining;

		private Dictionary<string, int> seededSaDataRemaining;

		private Dictionary<string, int> seededHeroDataRemaining;

		private Dictionary<string, int> seededChampionDataRemaining;

		private List<Entity> defenderGuildTroopsDeployed;

		public RandSimSeed SimSeed;

		public CameraShake CameraShakeObj
		{
			get;
			private set;
		}

		public LootController LootController
		{
			get;
			set;
		}

		public bool BattleInProgress
		{
			get;
			private set;
		}

		public bool BattleEndProcessing
		{
			get;
			private set;
		}

		public TeamType CurrentPlayerTeamType
		{
			get;
			private set;
		}

		public float ViewTimePassedPreBattle
		{
			get;
			set;
		}

		public uint Now
		{
			get
			{
				return this.currentBattle.TimePassed;
			}
		}

		public BattleController()
		{
			Service.Set<BattleController>(this);
			this.currentPlayer = Service.Get<CurrentPlayer>();
			this.entityController = Service.Get<EntityController>();
			this.eventManager = Service.Get<EventManager>();
			this.conditionController = Service.Get<VictoryConditionController>();
			this.defenseController = Service.Get<DefensiveBattleController>();
			this.sdc = Service.Get<IDataController>();
			this.ctm = Service.Get<CombatTriggerManager>();
			this.eventManager.RegisterObserver(this, EventId.TroopNotPlacedInvalidArea, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.TroopPlacedInsideBuildingError, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.TroopPlacedInsideShieldError, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.TroopNotPlacedInvalidTroop, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.TroopPlacedOnBoard, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.VictoryConditionSuccess, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.VictoryConditionFailure, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.BattleEndRecorded, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.ApplicationQuit, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.ApplicationPauseToggled, EventPriority.Default);
			this.starAnimations = new Queue<VictoryStarAnimation>();
			this.starAnimating = false;
			this.battleMessageView = new BattleMessageView("BattleMessage");
			this.CameraShakeObj = new CameraShake(new OnCameraShake(this.OnCameraShake));
			this.currentBattle = new CurrentBattle();
			this.troopsSpendDelta = new List<DeploymentRecord>();
		}

		public void InitializeCurrentBattle(BattleInitializationData battleInitializationData)
		{
			this.currentBattle.RecordID = battleInitializationData.RecordId;
			this.currentBattle.Type = battleInitializationData.BattleType;
			this.currentBattle.Attacker = battleInitializationData.Attacker;
			this.currentBattle.Defender = battleInitializationData.Defender;
			this.currentBattle.IsReplay = battleInitializationData.IsReplay;
			this.currentBattle.AllowReplay = battleInitializationData.AllowReplay;
			this.currentBattle.MultipleHeroDeploysAllowed = battleInitializationData.AllowMultipleHeroDeploys;
			this.currentBattle.AttackerDeployableData = battleInitializationData.AttackerDeployableData;
			this.currentBattle.DefenderDeployableData = battleInitializationData.DefenderDeployableData;
			this.currentBattle.AttackerDeployedData = BattleDeploymentData.CreateEmpty();
			this.currentBattle.DefenderDeployedData = BattleDeploymentData.CreateEmpty();
			this.currentBattle.LootCreditsAvailable = battleInitializationData.LootCreditsAvailable;
			this.currentBattle.LootMaterialsAvailable = battleInitializationData.LootMaterialsAvailable;
			this.currentBattle.LootContrabandAvailable = battleInitializationData.LootContrabandAvailable;
			this.currentBattle.BuildingLootCreditsMap = battleInitializationData.BuildingLootCreditsMap;
			this.currentBattle.BuildingLootMaterialsMap = battleInitializationData.BuildingLootMaterialsMap;
			this.currentBattle.BuildingLootContrabandMap = battleInitializationData.BuildingLootContrabandMap;
			if (this.currentBattle.IsReplay)
			{
				this.currentBattle.LootCreditsEarned = battleInitializationData.LootCreditsEarned;
				this.currentBattle.LootMaterialsEarned = battleInitializationData.LootMaterialsEarned;
				this.currentBattle.LootContrabandEarned = battleInitializationData.LootContrabandEarned;
				this.currentBattle.LootCreditsDeducted = battleInitializationData.LootCreditsDeducted;
				this.currentBattle.LootMaterialsDeducted = battleInitializationData.LootMaterialsDeducted;
				this.currentBattle.LootContrabandDeducted = battleInitializationData.LootContrabandDeducted;
			}
			this.currentBattle.LootCreditsDiscarded = 0;
			this.currentBattle.LootMaterialsDiscarded = 0;
			this.currentBattle.LootContrabandDiscarded = 0;
			this.currentBattle.PlanetId = battleInitializationData.PlanetId;
			this.currentBattle.Won = false;
			this.currentBattle.Canceled = false;
			this.currentBattle.TimePassed = 0u;
			this.currentBattle.TimeLeft = battleInitializationData.BattleLength;
			this.currentBattle.BattleMusic = battleInitializationData.BattleMusic;
			this.currentBattle.AmbientMusic = battleInitializationData.AmbientMusic;
			this.currentBattle.EndBattleServerTime = ServerTime.Time;
			this.currentBattle.FailedConditionUid = null;
			this.currentBattle.MissionId = battleInitializationData.MissionUid;
			this.currentBattle.DeadBuildingKeys = new Dictionary<string, string>();
			this.currentBattle.DefendingUnitsKilled = new Dictionary<string, int>();
			this.currentBattle.AttackingUnitsKilled = new Dictionary<string, int>();
			this.currentBattle.DefenderChampionsAvailable = battleInitializationData.DefenderChampionsAvailable;
			this.currentBattle.DefenderGuildTroopsAvailable = battleInitializationData.DefenderGuildTroopsAvailable;
			this.currentBattle.AttackerGuildTroopsAvailable = battleInitializationData.AttackerGuildTroopsAvailable;
			this.currentBattle.DefenderGuildTroopsDestroyed = new Dictionary<string, int>();
			this.currentBattle.AttackerGuildTroopsDeployed = new Dictionary<string, int>();
			this.currentBattle.PlayerDeployedGuildTroops = false;
			this.currentBattle.DisabledBuildings = battleInitializationData.DisabledBuildings;
			this.currentBattle.UnarmedTraps = new List<string>();
			this.currentBattle.CampaignPointsEarn = 0u;
			this.currentBattle.PvpMissionUid = null;
			HUD hUD = Service.Get<UXController>().HUD;
			PvpTarget pvpTarget = battleInitializationData.PvpTarget;
			if (pvpTarget != null)
			{
				hUD.UpdateMedalsAvailable(pvpTarget.PotentialMedalsToGain, pvpTarget.PotentialMedalsToLose);
				hUD.UpdateTournamentRatingBattleDelta(pvpTarget.PotentialTournamentRatingDeltaWin, pvpTarget.PotentialTournamentRatingDeltaLose, battleInitializationData.PlanetId);
				this.currentBattle.PotentialMedalsToGain = pvpTarget.PotentialMedalsToGain;
				this.currentBattle.DefenderBaseScore = pvpTarget.PlayerXp;
			}
			else
			{
				hUD.UpdateMedalsAvailable(0, 0);
				hUD.UpdateTournamentRatingBattleDelta(0, 0, null);
			}
			this.currentBattle.VictoryConditions = battleInitializationData.VictoryConditions;
			this.currentBattle.FailureCondition = battleInitializationData.FailureCondition;
			this.currentBattle.SeededPlayerDeployableData = BattleDeploymentData.Copy(this.GetPlayerDeployableData());
			if (!battleInitializationData.OverrideDeployables)
			{
				this.InitPlayerDeployables();
			}
			this.currentBattle.DefenseEncounterProfile = battleInitializationData.DefenseEncounterProfile;
			this.currentBattle.BattleScript = battleInitializationData.BattleScript;
			if (battleInitializationData.BattleVO != null)
			{
				this.currentBattle.BattleUid = battleInitializationData.BattleVO.Uid;
			}
			if (this.currentBattle.Type != BattleType.PveDefend)
			{
				this.CurrentPlayerTeamType = TeamType.Attacker;
			}
			else
			{
				this.CurrentPlayerTeamType = TeamType.Defender;
			}
			if (this.currentBattle.Type == BattleType.Pvp && string.IsNullOrEmpty(this.currentBattle.RecordID))
			{
				Service.Get<StaRTSLogger>().Error("No Battle Record ID (" + this.currentBattle.RecordID + ") Set by InitializeCurrentBattle.");
			}
			if (this.IsSquadWarsBattle())
			{
				this.currentBattle.AttackerWarBuffs = battleInitializationData.AttackerWarBuffs;
				this.currentBattle.DefenderWarBuffs = battleInitializationData.DefenderWarBuffs;
				if (battleInitializationData.MemberWarData != null)
				{
					this.currentBattle.WarVictoryPointsAvailable = battleInitializationData.MemberWarData.VictoryPointsLeft;
				}
			}
			this.currentBattle.AttackerEquipment = battleInitializationData.AttackerEquipment;
			this.currentBattle.DefenderEquipment = battleInitializationData.DefenderEquipment;
		}

		private void InitPlayerDeployables()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			CampaignMissionVO campaignMissionVO = null;
			if (!string.IsNullOrEmpty(this.currentBattle.MissionId))
			{
				campaignMissionVO = Service.Get<IDataController>().Get<CampaignMissionVO>(this.currentBattle.MissionId);
			}
			BattleDeploymentData playerDeployableData = this.GetPlayerDeployableData();
			if (this.currentBattle.Type != BattleType.PveDefend)
			{
				playerDeployableData.TroopData = this.AddDeployablesFromInventory(playerDeployableData.TroopData, currentPlayer.GetAllTroops());
				playerDeployableData.ChampionData = this.AddDeployablesFromInventory(playerDeployableData.ChampionData, currentPlayer.GetAllChampions());
			}
			if (this.currentBattle.Type == BattleType.Pvp || this.currentBattle.Type == BattleType.PvpAttackSquadWar || this.currentBattle.Type == BattleType.PveBuffBase || (this.currentBattle.Type != BattleType.PveDefend && this.currentBattle.IsSpecOps()) || (campaignMissionVO != null && campaignMissionVO.IsRaidDefense()))
			{
				playerDeployableData.HeroData = this.AddDeployablesFromInventory(playerDeployableData.HeroData, currentPlayer.GetAllHeroes());
			}
			playerDeployableData.SpecialAttackData = this.AddDeployablesFromInventory(playerDeployableData.SpecialAttackData, currentPlayer.GetAllSpecialAttacks());
		}

		private Dictionary<string, int> AddDeployablesFromInventory(Dictionary<string, int> deployables, IEnumerable<KeyValuePair<string, InventoryEntry>> inventoryDeployables)
		{
			if (inventoryDeployables != null)
			{
				if (deployables == null)
				{
					deployables = new Dictionary<string, int>();
				}
				using (IEnumerator<KeyValuePair<string, InventoryEntry>> enumerator = inventoryDeployables.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, InventoryEntry> current = enumerator.get_Current();
						if (deployables.ContainsKey(current.get_Key()))
						{
							Dictionary<string, int> dictionary = deployables;
							string key = current.get_Key();
							dictionary[key] += current.get_Value().Amount;
						}
						else
						{
							deployables.Add(current.get_Key(), current.get_Value().Amount);
						}
					}
				}
			}
			return deployables;
		}

		public void PrepareWorldForBattle()
		{
			this.DeinitializeLoot();
			this.InitializeLoot();
			this.SetInitialHealth();
			this.AddDefendingTroops();
			this.AddBattleScript();
			Service.Get<ChampionController>().RegisterChampionPlatforms(this.currentBattle);
			Service.Get<TrapController>().RegisterTraps(this.currentBattle);
			this.PrepareDisabledBuildings();
			Service.Get<ShieldController>().PrepareShieldsForBattle();
			this.ctm.ExecuteLoadTriggers();
		}

		private void PrepareDisabledBuildings()
		{
			if (this.currentBattle.DisabledBuildings == null || this.currentBattle.DisabledBuildings.Count == 0)
			{
				return;
			}
			NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
			for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
			{
				if (this.currentBattle.DisabledBuildings.Contains(buildingNode.BuildingComp.BuildingTO.Key))
				{
					Service.Get<ISupportController>().DisableBuilding(buildingNode.Entity);
				}
			}
		}

		public bool AllowTroopSpend()
		{
			return this.currentBattle.Type != BattleType.PveFue && !this.currentBattle.IsReplay;
		}

		public void StartBattle()
		{
			Service.Get<TargetingController>().SeedRandomNumberGenerator(this.Now);
			this.conditionController.ActivateConditionSet(this.currentBattle.VictoryConditions);
			if (this.currentBattle.FailureCondition != null)
			{
				this.conditionController.ActivateFailureCondition(this.currentBattle.FailureCondition);
			}
			this.waitingToSendBattleEndCommand = false;
			this.gotBattleEndResponse = false;
			this.completedBattleEndDelayTimer = false;
			this.BattleEndProcessing = false;
			this.BattleInProgress = true;
			this.UpdateDamagePercentage();
			Service.Get<SpecialAttackController>().Reset();
			this.troopsSpendDelta.Clear();
			Service.Get<ISupportController>().SimulateCheckAllContractsWithCurrentTime();
			if (!this.currentBattle.IsReplay && this.currentBattle.Defender != null && this.currentBattle.Type == BattleType.Pvp)
			{
				this.eventManager.SendEvent(EventId.PvpBattleStarting, null);
				PvpBattleStartRequest request = new PvpBattleStartRequest(this.currentBattle.Defender.PlayerId, Service.Get<FMS>().GetFileVersion("patches/base.json").ToString(), this.currentBattle.PvpMissionUid);
				PvpBattleStartCommand command = new PvpBattleStartCommand(request);
				Service.Get<ServerAPI>().Sync(command);
				Service.Get<EventManager>().SendEvent(EventId.EquipmentBuffShaderRemove, GameConstants.EQUIPMENT_SHADER_DELAY);
			}
			if (this.currentBattle.TimeLeft > 0)
			{
				this.battleTimer = Service.Get<SimTimerManager>().CreateSimTimer(1000u, true, new TimerDelegate(this.CheckTimeLeft), null);
			}
			if (this.AllowTroopSpend())
			{
				this.spendTroopTimer = Service.Get<SimTimerManager>().CreateSimTimer(3000u, true, new TimerDelegate(this.CallDeployableSpendCommand), null);
			}
			Service.Get<UXController>().HUD.ResetDamageStars();
			if (this.currentBattle.SeededPlayerDeployableData.TroopData != null)
			{
				this.seededTroopDataRemaining = new Dictionary<string, int>(this.currentBattle.SeededPlayerDeployableData.TroopData);
			}
			else
			{
				this.seededTroopDataRemaining = null;
			}
			if (this.currentBattle.SeededPlayerDeployableData.SpecialAttackData != null)
			{
				this.seededSaDataRemaining = new Dictionary<string, int>(this.currentBattle.SeededPlayerDeployableData.SpecialAttackData);
			}
			else
			{
				this.seededSaDataRemaining = null;
			}
			if (this.currentBattle.SeededPlayerDeployableData.HeroData != null)
			{
				this.seededHeroDataRemaining = new Dictionary<string, int>(this.currentBattle.SeededPlayerDeployableData.HeroData);
			}
			else
			{
				this.seededHeroDataRemaining = null;
			}
			if (this.currentBattle.SeededPlayerDeployableData.ChampionData != null)
			{
				this.seededChampionDataRemaining = new Dictionary<string, int>(this.currentBattle.SeededPlayerDeployableData.ChampionData);
			}
			else
			{
				this.seededChampionDataRemaining = null;
			}
			this.eventManager.RegisterObserver(this, EventId.BattleCancelRequested, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.EntityKilled, EventPriority.Notification);
			this.eventManager.RegisterObserver(this, EventId.LootEarnedUpdated, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.SquadTroopsDeployedByPlayer, EventPriority.Default);
			if (!this.currentBattle.IsReplay)
			{
				this.eventManager.RegisterObserver(this, EventId.DefenderTriggeredInBattle, EventPriority.Default);
			}
			this.entityController.AddSimSystem(new TargetingSystem(), 1020, 65535);
			this.entityController.AddSimSystem(new HealerTargetingSystem(), 1030, 4369);
			this.entityController.AddSimSystem(new AreaTriggerSystem(), 1080, 21845);
			this.entityController.AddSimSystem(new AttackSystem(), 1040, 43690);
			this.entityController.AddSimSystem(new BattleSystem(), 1010, 65535);
			this.entityController.RestartThreading();
			this.ctm.Enable(true);
			if (this.currentBattle.Type == BattleType.Pvp || this.currentBattle.Type == BattleType.PvpAttackSquadWar)
			{
				this.SetupSquadBuildingTrigger();
			}
			else if (this.currentBattle.Type != BattleType.PveBuffBase)
			{
				if (this.currentBattle.MissionId != null)
				{
					CampaignMissionVO campaignMissionVO = this.sdc.Get<CampaignMissionVO>(this.currentBattle.MissionId);
					if (campaignMissionVO.IsRaidDefense())
					{
						Service.Get<RaidDefenseController>().SendRaidDefenseStart(campaignMissionVO, new RaidDefenseController.OnSuccessCallback(this.OnRaidDefenseStarted));
					}
					else
					{
						MissionIdRequest request2;
						if (campaignMissionVO.Grind)
						{
							request2 = new MissionIdRequest(this.currentBattle.MissionId, this.currentBattle.BattleUid);
						}
						else
						{
							request2 = new MissionIdRequest(this.currentBattle.MissionId);
						}
						PveMissionStartCommand pveMissionStartCommand = new PveMissionStartCommand(request2);
						pveMissionStartCommand.AddSuccessCallback(new AbstractCommand<MissionIdRequest, BattleIdResponse>.OnSuccessCallback(this.OnPveMissionStarted));
						if (!Service.Get<ServerAPI>().Enabled && Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
						{
							Service.Get<ServerAPI>().Enabled = true;
							Service.Get<ServerAPI>().Sync(pveMissionStartCommand);
						}
						else
						{
							Service.Get<ServerAPI>().Sync(pveMissionStartCommand);
						}
					}
				}
				else
				{
					this.currentBattle.RecordID = "test";
					this.waitingToSendBattleEndCommand = false;
				}
			}
			this.defenderGuildTroopsDeployed = null;
		}

		private void OnRaidDefenseStarted(AbstractResponse response, object cookie)
		{
			RaidDefenseStartResponse raidDefenseStartResponse = response as RaidDefenseStartResponse;
			this.HandleMissionStarted(raidDefenseStartResponse.BattleId);
		}

		private void OnPveMissionStarted(BattleIdResponse response, object cookie)
		{
			if (Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				Service.Get<ServerAPI>().Enabled = false;
			}
			this.HandleMissionStarted(response.BattleId);
		}

		private void HandleMissionStarted(string battleId)
		{
			this.currentBattle.RecordID = battleId;
			if (this.waitingToSendBattleEndCommand)
			{
				Service.Get<StaRTSLogger>().Debug("Battle Record ID (" + this.currentBattle.RecordID + ") Set After Initial EndBattle");
				Service.Get<SimTimeEngine>().ScaleTime(1u);
				Service.Get<UserInputInhibitor>().AllowAll();
				this.EndBattle();
			}
		}

		private void SetupSquadBuildingTrigger()
		{
			Dictionary<string, int> defenderGuildTroopsAvailable = this.currentBattle.DefenderGuildTroopsAvailable;
			if (defenderGuildTroopsAvailable != null)
			{
				Entity entity = null;
				NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
				for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
				{
					if (buildingNode.BuildingComp.BuildingType.Type == BuildingType.Squad)
					{
						entity = buildingNode.Entity;
						break;
					}
				}
				if (entity != null)
				{
					foreach (KeyValuePair<string, int> current in defenderGuildTroopsAvailable)
					{
						if (current.get_Value() >= 1)
						{
							TroopTypeVO troop = this.sdc.Get<TroopTypeVO>(current.get_Key());
							this.ctm.RegisterTrigger(new DefendedBuildingCombatTrigger(entity, CombatTriggerType.Area, false, troop, current.get_Value(), true, GameConstants.SQUAD_TROOP_DEPLOY_STAGGER, 1800u));
						}
					}
				}
			}
		}

		private void SetInitialHealth()
		{
			this.currentBattle.InitialHealth = 0;
			LinkedList<BoardItem<Entity>> children = Service.Get<BoardController>().Board.Children;
			if (children != null)
			{
				foreach (BoardItem<Entity> current in children)
				{
					this.currentBattle.InitialHealth += this.GetHealth((SmartEntity)current.Data);
				}
			}
			this.currentBattle.CurrentHealth = this.currentBattle.InitialHealth;
		}

		private void AddDefendingTroops()
		{
			if (!string.IsNullOrEmpty(this.currentBattle.DefenseEncounterProfile))
			{
				EncounterProfileVO optional = this.sdc.GetOptional<EncounterProfileVO>(this.currentBattle.DefenseEncounterProfile);
				if (optional == null || optional.GroupString == null)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Battle {0} has an invalid encounter profile {1}", new object[]
					{
						this.currentBattle.BattleUid,
						this.currentBattle.DefenseEncounterProfile
					});
					return;
				}
				string[] array = optional.GroupString.Split(new char[]
				{
					'|'
				});
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					string[] array2 = array[i].Split(new char[]
					{
						','
					});
					BuildingTypeVO buildingTypeVO = this.sdc.Get<BuildingTypeVO>(array2[0]);
					NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
					for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
					{
						if (buildingNode.BuildingComp.BuildingType == buildingTypeVO)
						{
							string text = array2[1];
							CombatTriggerType type;
							if (text == "onDeath")
							{
								type = CombatTriggerType.Death;
							}
							else if (text == "proximity")
							{
								type = CombatTriggerType.Area;
							}
							else if (text == "onLoad")
							{
								type = CombatTriggerType.Load;
							}
							else
							{
								type = CombatTriggerType.Death;
							}
							TroopTypeVO troop = Service.Get<IDataController>().Get<TroopTypeVO>(array2[2]);
							int troopCount = Convert.ToInt32(array2[3], CultureInfo.InvariantCulture);
							bool leashed = array2[4].ToLower() == "true";
							uint stagger = Convert.ToUInt32(array2[5], CultureInfo.InvariantCulture);
							uint lastDitchDelaySeconds = 90u;
							if (array2.Length > 6)
							{
								uint num2 = Convert.ToUInt32(array2[6], CultureInfo.InvariantCulture);
								if (num2 > 0u)
								{
									lastDitchDelaySeconds = num2;
								}
							}
							this.ctm.RegisterTrigger(new DefendedBuildingCombatTrigger(buildingNode.Entity, type, true, troop, troopCount, leashed, stagger, lastDitchDelaySeconds));
						}
					}
					i++;
				}
			}
		}

		private void AddBattleScript()
		{
			if (!string.IsNullOrEmpty(this.currentBattle.BattleScript))
			{
				BattleScriptVO optional = this.sdc.GetOptional<BattleScriptVO>(this.currentBattle.BattleScript);
				if (optional == null || optional.Scripts == null)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Battle {0} has an invalid battle script {1}", new object[]
					{
						this.currentBattle.BattleUid,
						this.currentBattle.BattleScript
					});
					return;
				}
				string[] array = optional.Scripts.Split(new char[]
				{
					'|'
				});
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					string[] array2 = array[i].Split(new char[]
					{
						','
					});
					if (array2[1] == "onTroopDeploy")
					{
						this.ctm.RegisterTrigger(new StoryActionCombatTrigger(array2[0].ToLower(), CombatTriggerType.TroopDeploy, array2[2]));
					}
					else if (array2[1] == "onLoad")
					{
						Service.Get<StaRTSLogger>().ErrorFormat("Load triggers are not allowed in battle scripts.  Please use the intro story hook for script {0} instead", new object[]
						{
							this.currentBattle.BattleScript
						});
					}
					else if (array2[1] == "onDeath" || array2[1] == "proximity")
					{
						BuildingTypeVO buildingTypeVO = this.sdc.Get<BuildingTypeVO>(array2[0]);
						NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
						for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
						{
							if (buildingNode.BuildingComp.BuildingType == buildingTypeVO)
							{
								string text = array2[1];
								CombatTriggerType triggerType;
								if (text == "onDeath")
								{
									triggerType = CombatTriggerType.Death;
								}
								else if (text == "proximity")
								{
									triggerType = CombatTriggerType.Area;
								}
								else
								{
									triggerType = CombatTriggerType.Death;
								}
								this.ctm.RegisterTrigger(new StoryActionCombatTrigger(buildingNode.Entity, triggerType, array2[2]));
							}
						}
					}
					i++;
				}
			}
		}

		public void CancelBattle()
		{
			this.currentBattle.Canceled = true;
			this.EndBattleWithDelay();
		}

		public void CancelBattleRightAway()
		{
			this.currentBattle.Canceled = true;
			this.EndBattleRightAway();
		}

		public void OnAllTroopsDead()
		{
			if (this.defenseController.Active)
			{
				if (this.defenseController.AllWavesClear)
				{
					this.EndBattleWithDelay();
					return;
				}
			}
			else if (this.AllPlayerTroopsDeployed())
			{
				this.EndBattleWithDelay();
			}
		}

		private void EndBattleWithDelay()
		{
			if (this.BattleInProgress)
			{
				this.EndBattle();
				Service.Get<SimTimerManager>().CreateSimTimer((uint)(GameConstants.BATTLE_END_DELAY * 1000), false, new TimerDelegate(this.OnEndBattleDelayTimerFinished), null);
			}
		}

		public void EndBattleRightAway()
		{
			if (this.BattleInProgress)
			{
				this.EndBattle();
				this.OnEndBattleDelayTimerFinished(0u, null);
			}
		}

		private void EndBattle()
		{
			if (this.currentBattle.Type != BattleType.ClientBattle && string.IsNullOrEmpty(this.currentBattle.RecordID))
			{
				Service.Get<SimTimeEngine>().ScaleTime(0u);
				Service.Get<UserInputInhibitor>().DenyAll();
				if (this.currentBattle.Type == BattleType.Pvp)
				{
					Service.Get<StaRTSLogger>().Error("No Battle Record ID (" + this.currentBattle.RecordID + ") Set by EndBattle.");
				}
				this.waitingToSendBattleEndCommand = true;
				return;
			}
			this.BattleEndProcessing = true;
			if (this.AllowTroopSpend())
			{
				this.CallDeployableSpendCommand(0u, null);
			}
			this.BattleInProgress = false;
			this.UpdateCurrentBattleResult();
			Service.Get<SimTimerManager>().KillSimTimer(this.battleTimer);
			Service.Get<SimTimerManager>().KillSimTimer(this.spendTroopTimer);
			this.eventManager.UnregisterObserver(this, EventId.BattleCancelRequested);
			this.eventManager.UnregisterObserver(this, EventId.LootEarnedUpdated);
			this.eventManager.UnregisterObserver(this, EventId.SquadTroopsDeployedByPlayer);
			if (!this.currentBattle.IsReplay)
			{
				this.eventManager.UnregisterObserver(this, EventId.DefenderTriggeredInBattle);
			}
			else
			{
				this.gotBattleEndResponse = true;
			}
			this.entityController.RemoveSimSystem<TargetingSystem>();
			this.entityController.RemoveSimSystem<HealerTargetingSystem>();
			this.entityController.RemoveSimSystem<AreaTriggerSystem>();
			this.entityController.RemoveSimSystem<AttackSystem>();
			this.entityController.RemoveSimSystem<BattleSystem>();
			this.IdleAllEntities();
			this.conditionController.CancelCurrentConditions();
			this.ctm.Enable(false);
			this.ctm.UnregisterAllTriggers();
			this.defenseController.EndEncounter();
			Service.Get<ShieldController>().StopAllEffects();
			this.eventManager.SendEvent(EventId.BattleEndProcessing, this.currentBattle.IsReplay);
		}

		private void UpdateCurrentBattleResult()
		{
			if (!string.IsNullOrEmpty(this.currentBattle.FailedConditionUid))
			{
				this.currentBattle.Won = false;
				this.currentBattle.EarnedStars = 0;
			}
			else if (this.currentBattle.Type == BattleType.PveDefend && this.currentBattle.Canceled)
			{
				this.currentBattle.Won = false;
				this.currentBattle.EarnedStars = 0;
			}
			else
			{
				this.currentBattle.EarnedStars = Math.Min(3, this.conditionController.Successes.Count);
				if (this.conditionController.ActiveConditions != null)
				{
					foreach (AbstractCondition current in this.conditionController.ActiveConditions)
					{
						if (!this.conditionController.Successes.Contains(current.GetConditionVo().Uid) && current.IsConditionSatisfied())
						{
							this.currentBattle.EarnedStars = Math.Min(3, this.currentBattle.EarnedStars + 1);
						}
					}
				}
				this.currentBattle.Won = (this.currentBattle.EarnedStars > 0);
			}
			if (!this.currentBattle.IsReplay)
			{
				this.LootController.RefreshEarnedLoot();
				this.currentBattle.LootCreditsDeducted = this.LootController.GetLootEarned(CurrencyType.Credits);
				this.currentBattle.LootMaterialsDeducted = this.LootController.GetLootEarned(CurrencyType.Materials);
				this.currentBattle.LootContrabandDeducted = this.LootController.GetLootEarned(CurrencyType.Contraband);
				this.currentBattle.LootCreditsEarned = this.currentBattle.LootCreditsDeducted - this.currentBattle.LootCreditsDiscarded;
				this.currentBattle.LootMaterialsEarned = this.currentBattle.LootMaterialsDeducted - this.currentBattle.LootMaterialsDiscarded;
				this.currentBattle.LootContrabandEarned = this.currentBattle.LootContrabandDeducted - this.currentBattle.LootContrabandDiscarded;
				if (this.currentBattle.Won && this.currentBattle.IsPvP())
				{
					Service.Get<CurrentPlayer>().AddRelocationStars(this.currentBattle.EarnedStars);
				}
			}
		}

		private void IdleAllEntities()
		{
			EntityList allEntities = Service.Get<EntityController>().GetAllEntities();
			for (Entity entity = allEntities.Head; entity != null; entity = entity.Next)
			{
				StateComponent stateComponent = entity.Get<StateComponent>();
				if (stateComponent == null)
				{
					GameUtils.LogComponentsAsError("Stateless Entity encountered", entity);
				}
				else if (stateComponent.CurState != EntityState.Dying)
				{
					stateComponent.CurState = EntityState.Idle;
				}
			}
		}

		public void TryRegisterTriggeredTrap(Entity entity)
		{
			if (this.currentBattle.UnarmedTraps == null)
			{
				this.currentBattle.UnarmedTraps = new List<string>();
			}
			TrapComponent trapComponent = entity.Get<TrapComponent>();
			if (trapComponent != null && trapComponent.CurrentState != TrapState.Armed)
			{
				BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
				this.currentBattle.UnarmedTraps.Add(buildingComponent.BuildingTO.Key);
			}
		}

		private void OnBattleEndRecorded()
		{
			if (!this.currentBattle.Canceled && this.currentBattle.Type != BattleType.PveDefend)
			{
				this.RefundSurvivorTroops();
			}
			this.ExpendPlayerDeployedUnits();
			Dictionary<string, int> seededTroopsDeployed = this.CountExpendedSeededUnits();
			Dictionary<string, int> defendingUnitsKilled = this.currentBattle.DefendingUnitsKilled;
			Dictionary<string, int> attackingUnitsKilled = this.currentBattle.AttackingUnitsKilled;
			Dictionary<string, int> defenderGuildTroopsDestroyed = this.currentBattle.DefenderGuildTroopsDestroyed;
			Dictionary<string, int> attackerGuildTroopsDeployed = this.currentBattle.AttackerGuildTroopsDeployed;
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			Dictionary<string, string> buildingUids = null;
			NodeList<TrapNode> nodeList = Service.Get<EntityController>().GetNodeList<TrapNode>();
			for (TrapNode trapNode = nodeList.Head; trapNode != null; trapNode = trapNode.Next)
			{
				this.TryRegisterTriggeredTrap(trapNode.Entity);
			}
			if (this.currentBattle.Type == BattleType.PveAttack || this.currentBattle.Type == BattleType.PveFue || this.currentBattle.Type == BattleType.PveBuffBase || this.currentBattle.Type == BattleType.PvpAttackSquadWar)
			{
				buildingUids = this.CreateBuildingMapForPve();
			}
			List<string> unarmedTraps = this.currentBattle.UnarmedTraps;
			dictionary.Add("credits", this.currentBattle.LootCreditsEarned);
			dictionary.Add("materials", this.currentBattle.LootMaterialsEarned);
			dictionary.Add("contraband", this.currentBattle.LootContrabandEarned);
			ServerAPI serverAPI = Service.Get<ServerAPI>();
			ISupportController supportController = Service.Get<ISupportController>();
			supportController.UnfreezeAllBuildings(serverAPI.ServerTime);
			CampaignMissionVO campaignMissionVO = null;
			if (!this.currentBattle.IsPvP())
			{
				campaignMissionVO = this.sdc.Get<CampaignMissionVO>(this.currentBattle.MissionId);
			}
			BattleRecord battleRecord = Service.Get<BattleRecordController>().BattleRecord;
			if (campaignMissionVO != null && campaignMissionVO.IsRaidDefense())
			{
				EndPvEBattleTO endPvEBattleTO = new EndPvEBattleTO();
				endPvEBattleTO.Battle = this.currentBattle;
				endPvEBattleTO.SeededTroopsDeployed = seededTroopsDeployed;
				endPvEBattleTO.DefendingUnitsKilled = defendingUnitsKilled;
				endPvEBattleTO.AttackingUnitsKilled = attackingUnitsKilled;
				endPvEBattleTO.DefenderGuildUnitsSpent = defenderGuildTroopsDestroyed;
				endPvEBattleTO.AttackerGuildUnitsSpent = attackerGuildTroopsDeployed;
				endPvEBattleTO.LootEarned = dictionary;
				endPvEBattleTO.BuildingHealthMap = this.GetBuildingDamageMap();
				endPvEBattleTO.BuildingUids = buildingUids;
				endPvEBattleTO.UnarmedTraps = unarmedTraps;
				endPvEBattleTO.ReplayData = battleRecord;
				Service.Get<RaidDefenseController>().SendRaidDefenseComplete(campaignMissionVO, new Action(this.RaidDefenseComplete), endPvEBattleTO);
			}
			else
			{
				BattleEndRequest request = new BattleEndRequest(this.currentBattle, seededTroopsDeployed, defendingUnitsKilled, attackingUnitsKilled, defenderGuildTroopsDestroyed, attackerGuildTroopsDeployed, dictionary, this.GetBuildingDamageMap(), buildingUids, unarmedTraps, battleRecord);
				if (this.currentBattle.Type == BattleType.Pvp)
				{
					PvpBattleEndCommand pvpBattleEndCommand = new PvpBattleEndCommand(request);
					pvpBattleEndCommand.AddSuccessCallback(new AbstractCommand<BattleEndRequest, PvpBattleEndResponse>.OnSuccessCallback(this.OnBattleEndCommandSuccess));
					serverAPI.Enqueue(pvpBattleEndCommand);
				}
				else if (this.currentBattle.Type == BattleType.PveAttack || this.currentBattle.Type == BattleType.PveDefend)
				{
					PveMissionCompleteCommand pveMissionCompleteCommand = new PveMissionCompleteCommand(request);
					pveMissionCompleteCommand.AddSuccessCallback(new AbstractCommand<BattleEndRequest, DefaultResponse>.OnSuccessCallback(this.OnBattleEndCommandSuccess));
					if (this.currentBattle.RecordID == "test")
					{
						this.gotBattleEndResponse = true;
					}
					else if (!Service.Get<ServerAPI>().Enabled && Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
					{
						Service.Get<ServerAPI>().Enabled = true;
						Service.Get<ServerAPI>().Sync(pveMissionCompleteCommand);
					}
					else
					{
						Service.Get<ServerAPI>().Sync(pveMissionCompleteCommand);
					}
				}
				else if (this.currentBattle.Type == BattleType.PveBuffBase)
				{
					SquadWarAttackBuffBaseCompleteCommand squadWarAttackBuffBaseCompleteCommand = new SquadWarAttackBuffBaseCompleteCommand(request);
					squadWarAttackBuffBaseCompleteCommand.AddSuccessCallback(new AbstractCommand<BattleEndRequest, DefaultResponse>.OnSuccessCallback(this.OnBattleEndCommandSuccess));
					Service.Get<ServerAPI>().Sync(squadWarAttackBuffBaseCompleteCommand);
				}
				else if (this.currentBattle.Type == BattleType.PvpAttackSquadWar)
				{
					SquadWarAttackPlayerCompleteCommand squadWarAttackPlayerCompleteCommand = new SquadWarAttackPlayerCompleteCommand(request);
					squadWarAttackPlayerCompleteCommand.AddSuccessCallback(new AbstractCommand<BattleEndRequest, DefaultResponse>.OnSuccessCallback(this.OnBattleEndCommandSuccess));
					Service.Get<ServerAPI>().Sync(squadWarAttackPlayerCompleteCommand);
				}
				else if (this.currentBattle.Type == BattleType.ClientBattle)
				{
					this.gotBattleEndResponse = true;
				}
			}
			this.KillChampions(defendingUnitsKilled, attackingUnitsKilled);
			this.waitingToSendBattleEndCommand = false;
			this.eventManager.SendEvent(EventId.BattleEndFullyProcessed, null);
			if (this.currentBattle.Won && this.currentBattle.Type == BattleType.Pvp)
			{
				this.eventManager.SendEvent(EventId.PvpBattleWon, null);
			}
		}

		private Dictionary<string, string> CreateBuildingMapForPve()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> current in this.currentBattle.DeadBuildingKeys)
			{
				dictionary.Add(current.get_Key(), current.get_Value());
			}
			NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
			for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
			{
				string key = buildingNode.BuildingComp.BuildingTO.Key;
				if (!dictionary.ContainsKey(key))
				{
					dictionary.Add(key, buildingNode.BuildingComp.BuildingType.Uid);
				}
				else
				{
					Service.Get<StaRTSLogger>().Error("Duplicate building key found when creating pve building map");
				}
			}
			return dictionary;
		}

		private void KillChampions(Dictionary<string, int> defendingUnitsKilled, Dictionary<string, int> attackingUnitsKilled)
		{
			Dictionary<string, int> dictionary = (this.CurrentPlayerTeamType == TeamType.Attacker) ? attackingUnitsKilled : defendingUnitsKilled;
			foreach (string current in dictionary.Keys)
			{
				TroopTypeVO troopTypeVO = this.sdc.Get<TroopTypeVO>(current);
				if (troopTypeVO.Type == TroopType.Champion)
				{
					this.currentPlayer.OnChampionKilled(troopTypeVO.Uid);
				}
			}
		}

		private void RaidDefenseComplete()
		{
			this.gotBattleEndResponse = true;
			this.TryFinallyEnd();
		}

		private void OnBattleEndCommandSuccess(AbstractResponse response, object cookie)
		{
			if (this.currentBattle.Type == BattleType.Pvp)
			{
				Service.Get<PvpManager>().OnPvpBattleComplete((PvpBattleEndResponse)response, cookie);
			}
			if (Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				Service.Get<ServerAPI>().Enabled = false;
			}
			this.gotBattleEndResponse = true;
			this.TryFinallyEnd();
		}

		private void OnEndBattleDelayTimerFinished(uint id, object cookie)
		{
			this.completedBattleEndDelayTimer = true;
			this.TryFinallyEnd();
		}

		private void TryFinallyEnd()
		{
			if (!this.gotBattleEndResponse || !this.completedBattleEndDelayTimer)
			{
				return;
			}
			Service.Get<SpecialAttackController>().DestroyAll();
			Service.Get<SquadTroopAttackController>().Reset();
			this.defenderGuildTroopsDeployed = null;
			this.BattleEndProcessing = false;
			bool isSquadWarBattle = this.IsSquadWarsBattle();
			GameStateMachine gameStateMachine = Service.Get<GameStateMachine>();
			if (gameStateMachine.CurrentState is BattlePlaybackState)
			{
				gameStateMachine.SetState(new BattleEndPlaybackState(isSquadWarBattle));
				return;
			}
			gameStateMachine.SetState(new BattleEndState(isSquadWarBattle));
		}

		public Dictionary<string, int> GetBuildingDamageMap()
		{
			NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (KeyValuePair<string, string> current in this.currentBattle.DeadBuildingKeys)
			{
				dictionary.Add(current.get_Key(), 100);
			}
			for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
			{
				if (buildingNode.HealthComp.MaxHealth > 0)
				{
					string key = buildingNode.BuildingComp.BuildingTO.Key;
					if (!string.IsNullOrEmpty(key) && !dictionary.ContainsKey(key))
					{
						int value = GameUtils.CalculateDamagePercentage(buildingNode.HealthComp);
						dictionary.Add(key, value);
					}
				}
			}
			return dictionary;
		}

		public void Clear()
		{
			Service.Get<WorldController>().ResetWorld();
			this.DeinitializeLoot();
		}

		private Dictionary<string, int> CountExpendedSeededUnits()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			BattleDeploymentData playerDeployedData = this.GetPlayerDeployedData();
			Dictionary<string, int> troopData = this.currentBattle.SeededPlayerDeployableData.TroopData;
			Dictionary<string, int> specialAttackData = this.currentBattle.SeededPlayerDeployableData.SpecialAttackData;
			Dictionary<string, int> heroData = this.currentBattle.SeededPlayerDeployableData.HeroData;
			Dictionary<string, int> championData = this.currentBattle.SeededPlayerDeployableData.ChampionData;
			if (troopData != null)
			{
				foreach (string current in troopData.Keys)
				{
					int value = 0;
					if (playerDeployedData.TroopData != null && playerDeployedData.TroopData.ContainsKey(current))
					{
						value = Math.Min(troopData[current], playerDeployedData.TroopData[current]);
					}
					dictionary.Add(current, value);
				}
			}
			if (specialAttackData != null)
			{
				foreach (string current2 in specialAttackData.Keys)
				{
					int value2 = 0;
					if (playerDeployedData.SpecialAttackData != null && playerDeployedData.SpecialAttackData.ContainsKey(current2))
					{
						value2 = Math.Min(specialAttackData[current2], playerDeployedData.SpecialAttackData[current2]);
					}
					dictionary.Add(current2, value2);
				}
			}
			if (heroData != null)
			{
				foreach (string current3 in heroData.Keys)
				{
					int value3 = 0;
					if (playerDeployedData.HeroData != null && playerDeployedData.HeroData.ContainsKey(current3))
					{
						value3 = Math.Min(heroData[current3], playerDeployedData.HeroData[current3]);
					}
					dictionary.Add(current3, value3);
				}
			}
			if (championData != null)
			{
				foreach (string current4 in championData.Keys)
				{
					int value4 = 0;
					if (playerDeployedData.ChampionData != null && playerDeployedData.ChampionData.ContainsKey(current4))
					{
						value4 = Math.Min(championData[current4], playerDeployedData.ChampionData[current4]);
					}
					dictionary.Add(current4, value4);
				}
			}
			return dictionary;
		}

		private void ExpendPlayerDeployedUnits()
		{
			Dictionary<string, int> troopData = this.currentBattle.SeededPlayerDeployableData.TroopData;
			Dictionary<string, int> specialAttackData = this.currentBattle.SeededPlayerDeployableData.SpecialAttackData;
			Dictionary<string, int> heroData = this.currentBattle.SeededPlayerDeployableData.HeroData;
			BattleDeploymentData playerDeployedData = this.GetPlayerDeployedData();
			if (playerDeployedData.TroopData != null)
			{
				foreach (string current in playerDeployedData.TroopData.Keys)
				{
					int amount = playerDeployedData.TroopData[current];
					int numberOfTroopsToDeduct = this.GetNumberOfTroopsToDeduct(current, amount, troopData);
					if (numberOfTroopsToDeduct > 0)
					{
						this.currentPlayer.RemoveTroop(current, numberOfTroopsToDeduct);
					}
				}
			}
			if (playerDeployedData.SpecialAttackData != null)
			{
				foreach (string current2 in playerDeployedData.SpecialAttackData.Keys)
				{
					int amount2 = playerDeployedData.SpecialAttackData[current2];
					int numberOfTroopsToDeduct2 = this.GetNumberOfTroopsToDeduct(current2, amount2, specialAttackData);
					if (numberOfTroopsToDeduct2 > 0)
					{
						this.currentPlayer.RemoveSpecialAttack(current2, numberOfTroopsToDeduct2);
					}
				}
			}
			if (playerDeployedData.HeroData != null)
			{
				foreach (string current3 in playerDeployedData.HeroData.Keys)
				{
					int amount3 = playerDeployedData.HeroData[current3];
					int numberOfTroopsToDeduct3 = this.GetNumberOfTroopsToDeduct(current3, amount3, heroData);
					if (numberOfTroopsToDeduct3 > 0)
					{
						this.currentPlayer.RemoveHero(current3, numberOfTroopsToDeduct3);
					}
				}
			}
		}

		private int GetNumberOfTroopsToDeduct(string unitUid, int amount, Dictionary<string, int> seededUnits)
		{
			int num = amount;
			if (seededUnits != null && seededUnits.ContainsKey(unitUid))
			{
				num -= seededUnits[unitUid];
			}
			return num;
		}

		public void CheckTimeLeft(uint id, object cookie)
		{
			CurrentBattle expr_06 = this.currentBattle;
			int num = expr_06.TimeLeft - 1;
			expr_06.TimeLeft = num;
			bool warning = num <= GameConstants.BATTLE_WARNING_TIME;
			Service.Get<UXController>().HUD.RefreshTimerView(this.currentBattle.TimeLeft, warning);
			if (this.currentBattle.IsReplay)
			{
				Service.Get<UXController>().HUD.RefreshReplayTimerView(this.currentBattle.TimeLeft);
				int timeLeft = Service.Get<BattlePlaybackController>().CurrentBattleRecord.BattleAttributes.TimeLeft;
				if (this.currentBattle.TimeLeft - timeLeft < 0)
				{
					this.EndBattleRightAway();
				}
			}
			if (this.currentBattle.TimeLeft < 1)
			{
				this.EndBattleWithDelay();
			}
		}

		private void OnAlertModalResult(object result, object cookie)
		{
			if (result != null)
			{
				Service.Get<EventManager>().SendEvent(EventId.BattleCanceled, null);
				this.CancelBattle();
			}
		}

		private void RecordBuildingDestroyed(Entity entity)
		{
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			if (buildingComponent != null)
			{
				string key = buildingComponent.BuildingTO.Key;
				if (string.IsNullOrEmpty(key))
				{
					Service.Get<StaRTSLogger>().Error("Received dead building with invalid BuildingTO Key!");
				}
				else if (!this.currentBattle.DeadBuildingKeys.ContainsKey(key))
				{
					this.currentBattle.DeadBuildingKeys.Add(key, buildingComponent.BuildingType.Uid);
					if (entity.Has<TrapComponent>())
					{
						this.TryRegisterTriggeredTrap(entity);
					}
				}
				else
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Entity {0} reported dead twice to LootController.", new object[]
					{
						key
					});
				}
				TransformComponent transformComponent = entity.Get<TransformComponent>();
				if (transformComponent != null)
				{
					int x = transformComponent.X;
					int z = transformComponent.Z;
					TargetingController targetingController = Service.Get<TargetingController>();
					targetingController.UpdateNearbyTroops(8, x, z);
				}
			}
		}

		private void RecordUnitKilled(Entity entity)
		{
			TroopComponent troopComponent = entity.Get<TroopComponent>();
			if (troopComponent == null)
			{
				return;
			}
			TeamComponent teamComponent = entity.Get<TeamComponent>();
			if (teamComponent == null)
			{
				return;
			}
			Dictionary<string, int> dictionary = (teamComponent.TeamType == TeamType.Attacker) ? this.currentBattle.AttackingUnitsKilled : this.currentBattle.DefendingUnitsKilled;
			string uid = troopComponent.TroopType.Uid;
			if (dictionary.ContainsKey(uid))
			{
				Dictionary<string, int> arg_51_0 = dictionary;
				string key = uid;
				int num = arg_51_0[key];
				arg_51_0[key] = num + 1;
				return;
			}
			dictionary.Add(uid, 1);
		}

		private void RecordDefenderGuildTroopDestroyed(Entity entity)
		{
			TroopComponent troopComponent = entity.Get<TroopComponent>();
			string uid = troopComponent.TroopType.Uid;
			if (this.currentBattle.DefenderGuildTroopsDestroyed.ContainsKey(uid))
			{
				Dictionary<string, int> arg_33_0 = this.currentBattle.DefenderGuildTroopsDestroyed;
				string key = uid;
				int num = arg_33_0[key];
				arg_33_0[key] = num + 1;
				return;
			}
			this.currentBattle.DefenderGuildTroopsDestroyed.Add(uid, 1);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			Vector3 worldPosition = Vector3.zero;
			if (id <= EventId.SquadTroopsDeployedByPlayer)
			{
				if (id <= EventId.ApplicationQuit)
				{
					if (id != EventId.ApplicationPauseToggled)
					{
						if (id == EventId.ApplicationQuit)
						{
							if (this.BattleInProgress && GameConstants.PVP_LOSE_ON_QUIT && this.currentBattle.Type == BattleType.Pvp)
							{
								this.CancelBattleRightAway();
							}
						}
					}
					else if (this.BattleInProgress && (bool)cookie && GameConstants.PVP_LOSE_ON_PAUSE && this.currentBattle.Type == BattleType.Pvp)
					{
						this.CancelBattleRightAway();
					}
				}
				else if (id != EventId.EntityKilled)
				{
					if (id != EventId.TroopPlacedOnBoard)
					{
						switch (id)
						{
						case EventId.TroopNotPlacedInvalidArea:
							worldPosition = Units.BoardToWorldVec((IntPosition)cookie);
							this.battleMessageView.Show(worldPosition, true, Service.Get<Lang>().Get("AREA_INVALID", new object[0]));
							break;
						case EventId.TroopNotPlacedInvalidTroop:
							this.battleMessageView.Show((Vector3)cookie, true, Service.Get<Lang>().Get("TROOP_INVALID", new object[0]));
							break;
						case EventId.TroopPlacedInsideShieldError:
							worldPosition = Units.BoardToWorldVec((IntPosition)cookie);
							this.battleMessageView.Show(worldPosition, true, Service.Get<Lang>().Get("INVALID_DEPLOY_SHIELD", new object[0]));
							break;
						case EventId.TroopPlacedInsideBuildingError:
							worldPosition = Units.BoardToWorldVec((IntPosition)cookie);
							this.battleMessageView.Show(worldPosition, true, Service.Get<Lang>().Get("INVALID_DEPLOY_BUILDING", new object[0]));
							break;
						case EventId.SquadTroopsDeployedByPlayer:
							this.currentBattle.PlayerDeployedGuildTroops = true;
							if (!this.currentBattle.IsReplay)
							{
								if (this.currentBattle.IsSquadDeployAllowedInRaid())
								{
									this.currentBattle.DefenderGuildTroopsDestroyed = new Dictionary<string, int>(this.currentBattle.DefenderGuildTroopsAvailable);
									this.currentBattle.DefenderDeployedData.SquadData = new Dictionary<string, int>(this.currentBattle.DefenderGuildTroopsAvailable);
								}
								else
								{
									this.currentBattle.AttackerGuildTroopsDeployed = new Dictionary<string, int>(this.currentBattle.AttackerGuildTroopsAvailable);
									this.currentBattle.AttackerDeployedData.SquadData = new Dictionary<string, int>(this.currentBattle.AttackerGuildTroopsAvailable);
								}
								if (Service.Get<SquadController>().StateManager.Troops != null)
								{
									Service.Get<SquadController>().StateManager.Troops.Clear();
								}
							}
							break;
						}
					}
					else
					{
						this.battleMessageView.HideImmediately();
					}
				}
				else
				{
					Entity entity = cookie as Entity;
					this.PerformShakeEffect(entity);
					this.RecordBuildingDestroyed(entity);
					this.RecordUnitKilled(entity);
					if (this.defenderGuildTroopsDeployed != null && this.defenderGuildTroopsDeployed.Contains(entity))
					{
						this.RecordDefenderGuildTroopDestroyed(entity);
					}
				}
			}
			else if (id <= EventId.LootEarnedUpdated)
			{
				if (id != EventId.BattleEndRecorded)
				{
					if (id != EventId.BattleCancelRequested)
					{
						if (id == EventId.LootEarnedUpdated)
						{
							this.UpdateCurrencyInventory();
						}
					}
					else
					{
						string message = Service.Get<Lang>().Get("CONFIRM_END_BATTLE", new object[0]);
						AlertScreen.ShowModal(false, null, message, new OnScreenModalResult(this.OnAlertModalResult), null);
					}
				}
				else
				{
					this.OnBattleEndRecorded();
				}
			}
			else if (id != EventId.VictoryConditionSuccess)
			{
				if (id != EventId.VictoryConditionFailure)
				{
					if (id == EventId.DefenderTriggeredInBattle)
					{
						DefenderTroopDeployedData defenderTroopDeployedData = (DefenderTroopDeployedData)cookie;
						BuildingComponent buildingComponent = defenderTroopDeployedData.OwnerEntity.Get<BuildingComponent>();
						if (buildingComponent != null && buildingComponent.BuildingType.Type == BuildingType.Squad)
						{
							if (this.defenderGuildTroopsDeployed == null)
							{
								this.defenderGuildTroopsDeployed = new List<Entity>();
							}
							this.defenderGuildTroopsDeployed.Add(defenderTroopDeployedData.TroopEntity);
						}
					}
				}
				else if (this.conditionController.FailureConditionVO == cookie)
				{
					this.currentBattle.FailedConditionUid = ((ConditionVO)cookie).Uid;
					Service.Get<UXController>().HUD.UpdateDamageStars(0);
					this.UpdateHealth();
					this.EndBattleWithDelay();
				}
				else if (this.currentBattle.Type == BattleType.PveDefend)
				{
					int num = 3 - this.conditionController.Failures.Count;
					Service.Get<UXController>().HUD.UpdateDamageStars(num);
					if (num == 0)
					{
						this.EndBattleWithDelay();
					}
				}
			}
			else
			{
				this.currentBattle.Won = true;
				int count = this.conditionController.Successes.Count;
				if (count == 3)
				{
					this.UpdateHealth();
					this.EndBattleWithDelay();
				}
				if (Service.Get<UXController>().HUD.AreBattleStarsVisible())
				{
					string message2 = Service.Get<Lang>().Get("STAR_MESSAGE_" + count, new object[0]);
					this.starAnimations.Enqueue(new VictoryStarAnimation(count, message2));
					this.PumpStarAnimations();
					Service.Get<EventManager>().SendEvent(EventId.StarEarned, count);
				}
			}
			return EatResponse.NotEaten;
		}

		private void PumpStarAnimations()
		{
			if (!this.starAnimating && this.starAnimations.Count > 0)
			{
				this.starAnimating = true;
				VictoryStarAnimation victoryStarAnimation = this.starAnimations.Dequeue();
				victoryStarAnimation.Start(new StarAnimationCompleteDelegate(this.OnStarAnimationComplete));
			}
		}

		private void OnStarAnimationComplete(int starNumber)
		{
			this.starAnimating = false;
			this.PumpStarAnimations();
		}

		private void PerformShakeEffect(Entity entity)
		{
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			if (buildingComponent == null)
			{
				return;
			}
			if (buildingComponent.BuildingType.SizeX >= 4 || buildingComponent.BuildingType.SizeY >= 4)
			{
				this.CameraShakeObj.Shake(1f, 0.75f);
				return;
			}
			if (buildingComponent.BuildingType.SizeX >= 2 || buildingComponent.BuildingType.SizeY >= 2)
			{
				this.CameraShakeObj.Shake(0.5f, 0.25f);
				return;
			}
		}

		private void OnCameraShake(Vector3 shakeOffset)
		{
			Service.Get<CameraManager>().MainCamera.CurrentCameraShakeOffset = shakeOffset;
		}

		private void UpdateHealth()
		{
			NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
			int num = 0;
			for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
			{
				num += this.GetHealth(buildingNode.BuildingComp.BuildingType.Type, buildingNode.HealthComp);
			}
			this.UpdateCurrentHealth(num);
		}

		public void UpdateCurrentHealth(int health)
		{
			if (health == this.currentBattle.CurrentHealth)
			{
				return;
			}
			this.currentBattle.CurrentHealth = health;
			this.UpdateDamagePercentage();
		}

		private void UpdateDamagePercentage()
		{
			int num = 0;
			if (this.currentBattle.InitialHealth != 0)
			{
				if (this.currentBattle.InitialHealth < this.currentBattle.CurrentHealth)
				{
					this.currentBattle.InitialHealth = this.currentBattle.CurrentHealth;
				}
				num = (this.currentBattle.InitialHealth - this.currentBattle.CurrentHealth) * 100 / this.currentBattle.InitialHealth;
			}
			this.currentBattle.DamagePercent = num;
			Service.Get<UXController>().HUD.UpdateDamageValue(this.currentBattle.DamagePercent);
			this.eventManager.SendEvent(EventId.DamagePercentUpdated, num);
		}

		private int GetHealth(SmartEntity buildingEntity)
		{
			BuildingComponent buildingComp = buildingEntity.BuildingComp;
			if (buildingComp == null)
			{
				return 0;
			}
			BuildingType type = buildingComp.BuildingType.Type;
			if (type == BuildingType.Blocker)
			{
				return 0;
			}
			HealthComponent healthComp = buildingEntity.HealthComp;
			if (healthComp == null)
			{
				return 0;
			}
			return this.GetHealth(type, healthComp);
		}

		public int GetHealth(BuildingType buildingType, HealthComponent health)
		{
			if (buildingType == BuildingType.Wall || buildingType == BuildingType.Blocker || buildingType == BuildingType.Trap)
			{
				return 0;
			}
			return health.Health;
		}

		private void InitializeLoot()
		{
			this.LootController = new LootController();
			this.LootController.Initialize(this.currentBattle.LootCreditsAvailable, this.currentBattle.LootMaterialsAvailable, this.currentBattle.LootContrabandAvailable, this.currentBattle.BuildingLootCreditsMap, this.currentBattle.BuildingLootMaterialsMap, this.currentBattle.BuildingLootContrabandMap);
		}

		private void DeinitializeLoot()
		{
			if (this.LootController != null)
			{
				this.LootController.Destroy();
				this.LootController = null;
			}
		}

		private void UpdateCurrencyInventory()
		{
			if (this.currentBattle.Type == BattleType.PveDefend || this.currentBattle.IsReplay)
			{
				return;
			}
			Inventory inventory = this.currentPlayer.Inventory;
			int lootDelta = this.LootController.GetLootDelta(CurrencyType.Credits);
			int lootDelta2 = this.LootController.GetLootDelta(CurrencyType.Materials);
			int lootDelta3 = this.LootController.GetLootDelta(CurrencyType.Contraband);
			int num = inventory.ModifyCredits(lootDelta);
			int num2 = inventory.ModifyMaterials(lootDelta2);
			int num3 = inventory.ModifyContraband(lootDelta3);
			this.currentBattle.LootCreditsDiscarded += num;
			this.currentBattle.LootMaterialsDiscarded += num2;
			this.currentBattle.LootContrabandDiscarded += num3;
			Service.Get<EventManager>().SendEvent(EventId.LootCollected, new LootData(lootDelta, lootDelta2, lootDelta3));
			this.LootController.ResetLastLootEarned(CurrencyType.Credits);
			this.LootController.ResetLastLootEarned(CurrencyType.Materials);
			this.LootController.ResetLastLootEarned(CurrencyType.Contraband);
		}

		public void UpdateBattleTime(uint dt)
		{
			this.currentBattle.TimePassed += dt;
			this.ctm.UpdateCurrentTime(this.currentBattle.TimePassed);
		}

		private bool AllPlayerTroopsDeployed()
		{
			BattleDeploymentData playerDeployableData = this.GetPlayerDeployableData();
			if (playerDeployableData.TroopData != null)
			{
				foreach (KeyValuePair<string, int> current in playerDeployableData.TroopData)
				{
					TroopTypeVO troopTypeVO = this.sdc.Get<TroopTypeVO>(current.get_Key());
					if (!troopTypeVO.IsHealer && current.get_Value() > 0)
					{
						bool result = false;
						return result;
					}
				}
			}
			if (playerDeployableData.SpecialAttackData != null)
			{
				foreach (int current2 in playerDeployableData.SpecialAttackData.Values)
				{
					if (current2 > 0)
					{
						bool result = false;
						return result;
					}
				}
			}
			if (playerDeployableData.HeroData != null && (this.currentBattle.AttackerDeployedData.HeroData == null || this.currentBattle.AttackerDeployedData.HeroData.Count == 0))
			{
				foreach (int current3 in playerDeployableData.HeroData.Values)
				{
					if (current3 > 0)
					{
						bool result = false;
						return result;
					}
				}
			}
			if (playerDeployableData.ChampionData != null)
			{
				foreach (int current4 in playerDeployableData.ChampionData.Values)
				{
					if (current4 > 0)
					{
						bool result = false;
						return result;
					}
				}
			}
			return (!this.currentBattle.PlayerDeployedGuildTroops || !Service.Get<SquadTroopAttackController>().Spawning) && !this.CanPlayerDeploySquadTroops();
		}

		private void RefundSurvivorTroops()
		{
			if (!GameConstants.REFUND_SURVIVORS)
			{
				return;
			}
			BattleDeploymentData playerDeployedData = this.GetPlayerDeployedData();
			NodeList<TroopNode> nodeList = this.entityController.GetNodeList<TroopNode>();
			for (TroopNode troopNode = nodeList.Head; troopNode != null; troopNode = troopNode.Next)
			{
				if (!troopNode.HealthComp.IsDead() && troopNode.TeamComp.TeamType == TeamType.Attacker)
				{
					ITroopDeployableVO troopType = troopNode.TroopComp.TroopType;
					if (troopType.Type != TroopType.Hero && troopType.Type != TroopType.Champion)
					{
						Dictionary<string, int> arg_6B_0 = playerDeployedData.TroopData;
						string uid = troopType.Uid;
						int num = arg_6B_0[uid];
						arg_6B_0[uid] = num - 1;
					}
				}
			}
		}

		public Dictionary<string, int> GetAllPlayerDeployableTroops()
		{
			BattleDeploymentData playerDeployableData = this.GetPlayerDeployableData();
			return playerDeployableData.TroopData;
		}

		public Dictionary<string, int> GetAllPlayerDeployableSpecialAttacks()
		{
			BattleDeploymentData playerDeployableData = this.GetPlayerDeployableData();
			return playerDeployableData.SpecialAttackData;
		}

		public Dictionary<string, int> GetAllPlayerDeployableHeroes()
		{
			BattleDeploymentData playerDeployableData = this.GetPlayerDeployableData();
			return playerDeployableData.HeroData;
		}

		public Dictionary<string, int> GetAllPlayerDeployableChampions()
		{
			BattleDeploymentData playerDeployableData = this.GetPlayerDeployableData();
			return playerDeployableData.ChampionData;
		}

		public bool CanPlayerDeploySquadTroops()
		{
			return (this.currentBattle.Type == BattleType.Pvp || this.currentBattle.Type == BattleType.PveBuffBase || this.currentBattle.Type == BattleType.PvpAttackSquadWar || this.currentBattle.IsSquadDeployAllowedInRaid()) && !this.currentBattle.PlayerDeployedGuildTroops && SquadUtils.GetDonatedTroopStorageUsedByCurrentPlayer() > 0;
		}

		public int GetPlayerDeployableTroopCount(string uid)
		{
			Dictionary<string, int> allPlayerDeployableTroops = this.GetAllPlayerDeployableTroops();
			if (allPlayerDeployableTroops == null || !allPlayerDeployableTroops.ContainsKey(uid))
			{
				return 0;
			}
			return allPlayerDeployableTroops[uid];
		}

		public int GetPlayerDeployableSpecialAttackCount(string uid)
		{
			Dictionary<string, int> allPlayerDeployableSpecialAttacks = this.GetAllPlayerDeployableSpecialAttacks();
			if (allPlayerDeployableSpecialAttacks == null || !allPlayerDeployableSpecialAttacks.ContainsKey(uid))
			{
				return 0;
			}
			return allPlayerDeployableSpecialAttacks[uid];
		}

		public int GetPlayerDeployableHeroCount(string uid)
		{
			Dictionary<string, int> allPlayerDeployableHeroes = this.GetAllPlayerDeployableHeroes();
			if (allPlayerDeployableHeroes == null || !allPlayerDeployableHeroes.ContainsKey(uid))
			{
				return 0;
			}
			return allPlayerDeployableHeroes[uid];
		}

		public int GetPlayerDeployableChampionCount(string uid)
		{
			Dictionary<string, int> allPlayerDeployableChampions = this.GetAllPlayerDeployableChampions();
			if (allPlayerDeployableChampions == null || !allPlayerDeployableChampions.ContainsKey(uid))
			{
				return 0;
			}
			return allPlayerDeployableChampions[uid];
		}

		public void UpdateDeployableSpendDict(string unitTypeUid, Dictionary<string, int> seededData, DeploymentRecord record)
		{
			if (seededData == null || !seededData.ContainsKey(unitTypeUid))
			{
				if (record != null)
				{
					this.troopsSpendDelta.Add(record);
				}
				return;
			}
			int num = seededData[unitTypeUid];
			num--;
			if (num > 0)
			{
				seededData[unitTypeUid] = num;
				return;
			}
			seededData.Remove(unitTypeUid);
		}

		public void SendSquadDeployedCommand(int x, int z)
		{
			if (this.AllowTroopSpend())
			{
				DeployableSpendRequest request = new DeployableSpendRequest(this.currentBattle.RecordID, Service.Get<BattleController>().Now, x, z);
				ICommand command = this.CreateDeployableSpendCommand(request);
				Service.Get<ServerAPI>().Enqueue(command);
			}
		}

		public void CallDeployableSpendCommand(uint id, object cookie)
		{
			if (this.troopsSpendDelta != null && this.troopsSpendDelta.Count > 0 && !string.IsNullOrEmpty(this.currentBattle.RecordID))
			{
				DeployableSpendRequest request = new DeployableSpendRequest(this.currentBattle.RecordID, this.troopsSpendDelta);
				ICommand command = this.CreateDeployableSpendCommand(request);
				Service.Get<ServerAPI>().Enqueue(command);
				this.troopsSpendDelta.Clear();
			}
		}

		private ICommand CreateDeployableSpendCommand(DeployableSpendRequest request)
		{
			ICommand result;
			if (this.IsSquadWarsBattle())
			{
				result = new SquadWarSpendDeployablesCommand(request);
			}
			else
			{
				result = new DeployableSpendCommand(request);
			}
			return result;
		}

		public void OnTroopDeployed(string uid, TeamType teamType, IntPosition boardPosition)
		{
			BattleDeploymentData battleDeploymentData = null;
			BattleDeploymentData battleDeploymentData2 = null;
			this.GetDeploymentDataFromTeamType(teamType, out battleDeploymentData, out battleDeploymentData2);
			Dictionary<string, int> expr_17 = battleDeploymentData.TroopData;
			int num = expr_17[uid];
			expr_17[uid] = num - 1;
			if (battleDeploymentData2.TroopData == null)
			{
				battleDeploymentData2.TroopData = new Dictionary<string, int>();
			}
			if (battleDeploymentData2.TroopDataList == null)
			{
				battleDeploymentData2.TroopDataList = new List<DeploymentRecord>();
			}
			if (battleDeploymentData2.TroopData.ContainsKey(uid))
			{
				Dictionary<string, int> expr_66 = battleDeploymentData2.TroopData;
				num = expr_66[uid];
				expr_66[uid] = num + 1;
			}
			else
			{
				battleDeploymentData2.TroopData[uid] = 1;
			}
			DeploymentRecord deploymentRecord = new DeploymentRecord(uid, "TroopPlaced", this.Now, boardPosition.x, boardPosition.z);
			battleDeploymentData2.TroopDataList.Add(deploymentRecord);
			if (teamType == this.CurrentPlayerTeamType)
			{
				this.UpdateDeployableSpendDict(uid, this.seededTroopDataRemaining, deploymentRecord);
			}
		}

		public void OnSpecialAttackDeployed(string uid, TeamType teamType, IntPosition boardPosition)
		{
			BattleDeploymentData battleDeploymentData = null;
			BattleDeploymentData battleDeploymentData2 = null;
			this.GetDeploymentDataFromTeamType(teamType, out battleDeploymentData, out battleDeploymentData2);
			Dictionary<string, int> expr_17 = battleDeploymentData.SpecialAttackData;
			int num = expr_17[uid];
			expr_17[uid] = num - 1;
			if (battleDeploymentData2.SpecialAttackData == null)
			{
				battleDeploymentData2.SpecialAttackData = new Dictionary<string, int>();
			}
			if (battleDeploymentData2.SpecialAttackDataList == null)
			{
				battleDeploymentData2.SpecialAttackDataList = new List<DeploymentRecord>();
			}
			if (battleDeploymentData2.SpecialAttackData.ContainsKey(uid))
			{
				Dictionary<string, int> expr_66 = battleDeploymentData2.SpecialAttackData;
				num = expr_66[uid];
				expr_66[uid] = num + 1;
			}
			else
			{
				battleDeploymentData2.SpecialAttackData[uid] = 1;
			}
			DeploymentRecord deploymentRecord = new DeploymentRecord(uid, "SpecialAttackDeployed", this.Now, boardPosition.x, boardPosition.z);
			battleDeploymentData2.SpecialAttackDataList.Add(deploymentRecord);
			if (teamType == this.CurrentPlayerTeamType)
			{
				this.UpdateDeployableSpendDict(uid, this.seededSaDataRemaining, deploymentRecord);
			}
		}

		public void OnHeroDeployed(string uid, TeamType teamType, IntPosition boardPosition)
		{
			BattleDeploymentData battleDeploymentData = null;
			BattleDeploymentData battleDeploymentData2 = null;
			this.GetDeploymentDataFromTeamType(teamType, out battleDeploymentData, out battleDeploymentData2);
			Dictionary<string, int> expr_17 = battleDeploymentData.HeroData;
			int num = expr_17[uid];
			expr_17[uid] = num - 1;
			if (battleDeploymentData2.HeroData == null)
			{
				battleDeploymentData2.HeroData = new Dictionary<string, int>();
			}
			if (battleDeploymentData2.HeroDataList == null)
			{
				battleDeploymentData2.HeroDataList = new List<DeploymentRecord>();
			}
			if (battleDeploymentData2.HeroData.ContainsKey(uid))
			{
				Dictionary<string, int> expr_66 = battleDeploymentData2.HeroData;
				num = expr_66[uid];
				expr_66[uid] = num + 1;
			}
			else
			{
				battleDeploymentData2.HeroData[uid] = 1;
			}
			DeploymentRecord deploymentRecord = new DeploymentRecord(uid, "HeroDeployed", this.Now, boardPosition.x, boardPosition.z);
			battleDeploymentData2.HeroDataList.Add(deploymentRecord);
			if (teamType == this.CurrentPlayerTeamType)
			{
				this.UpdateDeployableSpendDict(uid, this.seededHeroDataRemaining, deploymentRecord);
			}
		}

		public void OnChampionDeployed(string uid, TeamType teamType, IntPosition boardPosition)
		{
			BattleDeploymentData battleDeploymentData = null;
			BattleDeploymentData battleDeploymentData2 = null;
			this.GetDeploymentDataFromTeamType(teamType, out battleDeploymentData, out battleDeploymentData2);
			if (battleDeploymentData.ChampionData != null && battleDeploymentData.ChampionData.ContainsKey(uid))
			{
				Dictionary<string, int> expr_2D = battleDeploymentData.ChampionData;
				int num = expr_2D[uid];
				expr_2D[uid] = num - 1;
			}
			else
			{
				Service.Get<StaRTSLogger>().Error("No ChampionData found for : " + uid);
			}
			if (battleDeploymentData2.ChampionData == null)
			{
				battleDeploymentData2.ChampionData = new Dictionary<string, int>();
			}
			if (battleDeploymentData2.ChampionDataList == null)
			{
				battleDeploymentData2.ChampionDataList = new List<DeploymentRecord>();
			}
			if (battleDeploymentData2.ChampionData.ContainsKey(uid))
			{
				Dictionary<string, int> expr_93 = battleDeploymentData2.ChampionData;
				int num = expr_93[uid];
				expr_93[uid] = num + 1;
			}
			else
			{
				battleDeploymentData2.ChampionData[uid] = 1;
			}
			DeploymentRecord deploymentRecord = new DeploymentRecord(uid, "ChampionDeployed", this.Now, boardPosition.x, boardPosition.z);
			battleDeploymentData2.ChampionDataList.Add(deploymentRecord);
			if (teamType == this.CurrentPlayerTeamType)
			{
				this.UpdateDeployableSpendDict(uid, this.seededChampionDataRemaining, deploymentRecord);
			}
		}

		private void GetDeploymentDataFromTeamType(TeamType teamType, out BattleDeploymentData deployableData, out BattleDeploymentData deployedData)
		{
			deployableData = null;
			deployedData = null;
			if (teamType == TeamType.Defender)
			{
				deployableData = this.currentBattle.DefenderDeployableData;
				deployedData = this.currentBattle.DefenderDeployedData;
				return;
			}
			deployableData = this.currentBattle.AttackerDeployableData;
			deployedData = this.currentBattle.AttackerDeployedData;
		}

		private BattleDeploymentData GetPlayerDeployedData()
		{
			BattleDeploymentData result;
			if (this.currentBattle.Type != BattleType.PveDefend)
			{
				result = this.currentBattle.AttackerDeployedData;
			}
			else
			{
				result = this.currentBattle.DefenderDeployedData;
			}
			return result;
		}

		private BattleDeploymentData GetPlayerDeployableData()
		{
			BattleDeploymentData result;
			if (this.currentBattle.Type != BattleType.PveDefend)
			{
				result = this.currentBattle.AttackerDeployableData;
			}
			else
			{
				result = this.currentBattle.DefenderDeployableData;
			}
			return result;
		}

		public CurrentBattle GetCurrentBattle()
		{
			return this.currentBattle;
		}

		public bool WasLastBattleWon()
		{
			return this.currentBattle.Won;
		}

		public bool IsSquadWarsBattle()
		{
			return this.currentBattle.Type == BattleType.PvpAttackSquadWar || this.currentBattle.Type == BattleType.PveBuffBase;
		}

		public bool IsSquadWarsBuffBaseBattle()
		{
			return this.currentBattle.Type == BattleType.PveBuffBase;
		}

		protected internal BattleController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).AddBattleScript();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).AddDefendingTroops();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).AddDeployablesFromInventory((Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args), (IEnumerable<KeyValuePair<string, InventoryEntry>>)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).AllowTroopSpend());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).AllPlayerTroopsDeployed());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).CancelBattle();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).CancelBattleRightAway();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).CanPlayerDeploySquadTroops());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).Clear();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).CountExpendedSeededUnits());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).CreateBuildingMapForPve());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).CreateDeployableSpendCommand((DeployableSpendRequest)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).DeinitializeLoot();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).EndBattle();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).EndBattleRightAway();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).EndBattleWithDelay();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).ExpendPlayerDeployedUnits();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).BattleEndProcessing);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).BattleInProgress);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).CameraShakeObj);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).CurrentPlayerTeamType);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).LootController);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).ViewTimePassedPreBattle);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetAllPlayerDeployableChampions());
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetAllPlayerDeployableHeroes());
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetAllPlayerDeployableSpecialAttacks());
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetAllPlayerDeployableTroops());
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetBuildingDamageMap());
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetCurrentBattle());
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetHealth((BuildingType)(*(int*)args), (HealthComponent)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetHealth((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetNumberOfTroopsToDeduct(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetPlayerDeployableChampionCount(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetPlayerDeployableData());
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetPlayerDeployableHeroCount(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetPlayerDeployableSpecialAttackCount(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetPlayerDeployableTroopCount(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).GetPlayerDeployedData());
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).HandleMissionStarted(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).IdleAllEntities();
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).InitializeCurrentBattle((BattleInitializationData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).InitializeLoot();
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).InitPlayerDeployables();
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).IsSquadWarsBattle());
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).IsSquadWarsBuffBaseBattle());
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).KillChampions((Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnAlertModalResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnAllTroopsDead();
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnBattleEndCommandSuccess((AbstractResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnBattleEndRecorded();
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnCameraShake(*(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnChampionDeployed(Marshal.PtrToStringUni(*(IntPtr*)args), (TeamType)(*(int*)(args + 1)), *(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnHeroDeployed(Marshal.PtrToStringUni(*(IntPtr*)args), (TeamType)(*(int*)(args + 1)), *(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnPveMissionStarted((BattleIdResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnRaidDefenseStarted((AbstractResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnSpecialAttackDeployed(Marshal.PtrToStringUni(*(IntPtr*)args), (TeamType)(*(int*)(args + 1)), *(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnStarAnimationComplete(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).OnTroopDeployed(Marshal.PtrToStringUni(*(IntPtr*)args), (TeamType)(*(int*)(args + 1)), *(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).PerformShakeEffect((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).PrepareDisabledBuildings();
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).PrepareWorldForBattle();
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).PumpStarAnimations();
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).RaidDefenseComplete();
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).RecordBuildingDestroyed((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).RecordDefenderGuildTroopDestroyed((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).RecordUnitKilled((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).RefundSurvivorTroops();
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).SendSquadDeployedCommand(*(int*)args, *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).BattleEndProcessing = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).BattleInProgress = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).CameraShakeObj = (CameraShake)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).CurrentPlayerTeamType = (TeamType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).LootController = (LootController)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).ViewTimePassedPreBattle = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).SetInitialHealth();
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).SetupSquadBuildingTrigger();
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).StartBattle();
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).TryFinallyEnd();
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).TryRegisterTriggeredTrap((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).UpdateCurrencyInventory();
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).UpdateCurrentBattleResult();
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).UpdateCurrentHealth(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).UpdateDamagePercentage();
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).UpdateDeployableSpendDict(Marshal.PtrToStringUni(*(IntPtr*)args), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[1]), (DeploymentRecord)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((BattleController)GCHandledObjects.GCHandleToObject(instance)).UpdateHealth();
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleController)GCHandledObjects.GCHandleToObject(instance)).WasLastBattleWon());
		}
	}
}
