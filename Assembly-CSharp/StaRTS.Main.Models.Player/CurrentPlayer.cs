using StaRTS.Externals.EnvironmentManager;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models.Perks;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.Player.Objectives;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Story;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Player
{
	public class CurrentPlayer : GamePlayer
	{
		private const int FREE_RELOCATION = -1;

		private const string CURRENT = "current";

		private const string UNLOCKED = "unlocked";

		private const string LOCKED = "locked";

		private string playerName;

		private string nextRaidId;

		private int attackRating;

		private int defenseRating;

		private FactionType faction;

		public BattleHistory BattleHistory
		{
			get;
			set;
		}

		public List<IStoryTrigger> ActiveSaveTriggers
		{
			get;
			private set;
		}

		public List<string> SpecOpIntros
		{
			get;
			set;
		}

		public string CurrentQuest
		{
			get;
			set;
		}

		public string RestoredQuest
		{
			get;
			set;
		}

		public uint FirstLoginTime
		{
			get;
			set;
		}

		public string PlayerId
		{
			get;
			private set;
		}

		public bool PlayerNameInvalid
		{
			get;
			set;
		}

		public uint RaidStartTime
		{
			get;
			private set;
		}

		public uint NextRaidStartTime
		{
			get;
			private set;
		}

		public uint RaidEndTime
		{
			get;
			private set;
		}

		public Dictionary<string, ObjectiveGroup> Objectives
		{
			get;
			set;
		}

		public List<string> UnlockedPlanets
		{
			get;
			private set;
		}

		public List<string> HolonetRewards
		{
			get;
			private set;
		}

		public CampaignMissionVO CurrentRaid
		{
			get;
			private set;
		}

		public string CurrentRaidPoolId
		{
			get;
			private set;
		}

		public string CurrentRaidId
		{
			get;
			private set;
		}

		private int RelocationStarsCount
		{
			get;
			set;
		}

		public PerksInfo PerksInfo
		{
			get;
			private set;
		}

		public ActiveArmory ActiveArmory
		{
			get;
			private set;
		}

		public ArmoryInfo ArmoryInfo
		{
			get;
			private set;
		}

		public Dictionary<string, int> Shards
		{
			get;
			set;
		}

		public TroopDonationProgress TroopDonationProgress
		{
			get;
			private set;
		}

		public override string PlayerName
		{
			get
			{
				return this.playerName;
			}
			set
			{
				this.playerName = value;
				this.PlayerNameInvalid = false;
				Service.Get<EventManager>().SendEvent(EventId.PlayerNameChanged, this.faction);
			}
		}

		public string PlanetId
		{
			get
			{
				return base.Map.PlanetId();
			}
		}

		public PlanetVO Planet
		{
			get
			{
				return base.Map.Planet;
			}
		}

		public override int AttackRating
		{
			get
			{
				return this.attackRating;
			}
			set
			{
				bool flag = this.attackRating != value;
				if (flag)
				{
					this.attackRating = value;
					Service.Get<EventManager>().SendEvent(EventId.PvpRatingChanged, null);
				}
			}
		}

		public override int DefenseRating
		{
			get
			{
				return this.defenseRating;
			}
			set
			{
				bool flag = this.defenseRating != value;
				if (flag)
				{
					this.defenseRating = value;
					Service.Get<EventManager>().SendEvent(EventId.PvpRatingChanged, null);
				}
			}
		}

		public bool CurrentlyDefending
		{
			get;
			set;
		}

		public uint CurrentlyDefendingExpireTime
		{
			get;
			set;
		}

		public CampaignProgress CampaignProgress
		{
			get;
			set;
		}

		public TournamentProgress TournamentProgress
		{
			get;
			set;
		}

		public PrizeInventory Prizes
		{
			get;
			set;
		}

		public bool FirstTimePlayer
		{
			get;
			set;
		}

		public uint ProtectedUntil
		{
			get;
			set;
		}

		public uint ProtectionFrom
		{
			get;
			set;
		}

		private Dictionary<string, uint> ProtectionCooldownUntil
		{
			get;
			set;
		}

		public uint LastLoginTime
		{
			get;
			set;
		}

		public uint LoginTime
		{
			get;
			set;
		}

		public int SessionCountToday
		{
			get;
			set;
		}

		public uint InstallDate
		{
			get;
			set;
		}

		public Dictionary<string, int> DamagedBuildings
		{
			get;
			set;
		}

		public bool IsConnectedAccount
		{
			get;
			set;
		}

		public bool IsRateIncentivized
		{
			get;
			set;
		}

		public bool IsPushIncentivized
		{
			get;
			set;
		}

		public int NumIdentities
		{
			get;
			set;
		}

		public List<string> Patches
		{
			get;
			set;
		}

		public uint LastWarParticipationTime
		{
			get;
			set;
		}

		public string OfferId
		{
			get;
			set;
		}

		public uint TriggerDate
		{
			get;
			set;
		}

		public override FactionType Faction
		{
			get
			{
				return this.faction;
			}
			set
			{
				this.faction = value;
				Service.Get<EventManager>().SendEvent(EventId.PlayerFactionChanged, this.faction);
			}
		}

		public int CurrentCrystalsAmount
		{
			get
			{
				return base.Inventory.GetItemAmount("crystals");
			}
		}

		public int CurrentDroidsAmount
		{
			get
			{
				return base.Inventory.GetItemAmount("droids");
			}
		}

		public int MaxDroidsAmount
		{
			get
			{
				return base.Inventory.GetItemCapacity("droids");
			}
		}

		public bool HasNotCompletedFirstFueStep()
		{
			bool flag = this.FirstTimePlayer || this.RestoredQuest == GameConstants.FUE_QUEST_UID;
			if (flag)
			{
				flag = Service.Get<PlayerIdentityController>().IsFirstIdentity(this.PlayerId);
			}
			return flag;
		}

		public CurrentPlayer()
		{
			Service.Set<CurrentPlayer>(this);
			this.SetPlayerId();
		}

		public void Init()
		{
			base.Map = new Map();
			base.Inventory = new Inventory();
			base.UnlockedLevels = new UnlockedLevelData();
			this.Prizes = new PrizeInventory();
			this.ActiveSaveTriggers = new List<IStoryTrigger>();
			this.FirstTimePlayer = true;
			this.CampaignProgress = new CampaignProgress();
			this.TournamentProgress = new TournamentProgress();
			this.BattleHistory = new BattleHistory();
			this.UnlockedPlanets = new List<string>();
			this.HolonetRewards = new List<string>();
			this.RelocationStarsCount = 0;
			this.NumIdentities = 1;
			this.Objectives = new Dictionary<string, ObjectiveGroup>();
			this.Shards = new Dictionary<string, int>();
		}

		private void SetPlayerId()
		{
			string text = null;
			if (PlayerPrefs.HasKey("prefPlayerId"))
			{
				text = PlayerPrefs.GetString("prefPlayerId");
			}
			if (string.IsNullOrEmpty(text))
			{
				text = Service.Get<EnvironmentController>().GetDeviceId();
			}
			this.PlayerId = text;
			Service.Get<EventManager>().SendEvent(EventId.PlayerIdSet, this.PlayerId);
		}

		public IEnumerable<KeyValuePair<string, InventoryEntry>> GetAllTroops()
		{
			return base.Inventory.Troop.GetInternalStorage();
		}

		public IEnumerable<KeyValuePair<string, InventoryEntry>> GetAllSpecialAttacks()
		{
			return base.Inventory.SpecialAttack.GetInternalStorage();
		}

		public IEnumerable<KeyValuePair<string, InventoryEntry>> GetAllHeroes()
		{
			return base.Inventory.Hero.GetInternalStorage();
		}

		public IEnumerable<KeyValuePair<string, InventoryEntry>> GetAllChampions()
		{
			return base.Inventory.Champion.GetInternalStorage();
		}

		public bool SetTroopCount(string uid, int amount)
		{
			TroopTypeVO optional = Service.Get<IDataController>().GetOptional<TroopTypeVO>(uid);
			if (optional == null)
			{
				Service.Get<StaRTSLogger>().Error("Troop does not exist: " + uid);
				return false;
			}
			return this.SetDeployableCount(uid, amount, base.Inventory.Troop, optional.Size);
		}

		public void SetCurrentRaid(string raidUid)
		{
			this.nextRaidId = raidUid;
			if (!string.IsNullOrEmpty(this.nextRaidId) && Service.IsSet<IDataController>())
			{
				this.CurrentRaid = Service.Get<IDataController>().Get<CampaignMissionVO>(this.nextRaidId);
			}
		}

		public bool SetSpecialAttackCount(string uid, int amount)
		{
			SpecialAttackTypeVO optional = Service.Get<IDataController>().GetOptional<SpecialAttackTypeVO>(uid);
			if (optional == null)
			{
				Service.Get<StaRTSLogger>().Error("Special Attack does not exist: " + uid);
				return false;
			}
			return this.SetDeployableCount(uid, amount, base.Inventory.SpecialAttack, optional.Size);
		}

		public bool SetHeroCount(string uid, int amount)
		{
			TroopTypeVO optional = Service.Get<IDataController>().GetOptional<TroopTypeVO>(uid);
			if (optional == null)
			{
				Service.Get<StaRTSLogger>().Error("Hero does not exist: " + uid);
				return false;
			}
			return this.SetDeployableCount(uid, amount, base.Inventory.Hero, optional.Size);
		}

		public bool SetChampionCount(string uid, int amount)
		{
			TroopTypeVO optional = Service.Get<IDataController>().GetOptional<TroopTypeVO>(uid);
			if (optional == null)
			{
				Service.Get<StaRTSLogger>().Error("Champion does not exist: " + uid);
				return false;
			}
			return this.SetDeployableCount(uid, amount, base.Inventory.Champion, optional.Size);
		}

		private bool SetDeployableCount(string uid, int amount, InventoryStorage storage, int size)
		{
			if (amount < 0)
			{
				Service.Get<StaRTSLogger>().Debug("Cannot set a deployable count less than zero. uid: " + uid);
				return false;
			}
			int num = storage.GetTotalStorageAmount() + amount * size;
			if (num > storage.GetTotalStorageCapacity())
			{
				Service.Get<StaRTSLogger>().Debug("Not enough capacity for deployable. uid: " + uid);
				return false;
			}
			int delta = amount - storage.GetItemAmount(uid);
			storage.ModifyItemAmount(uid, delta);
			return true;
		}

		public void AddProtectionCooldownUntil(string key, uint time)
		{
			if (this.ProtectionCooldownUntil == null)
			{
				this.ProtectionCooldownUntil = new Dictionary<string, uint>();
			}
			if (this.ProtectionCooldownUntil.ContainsKey(key))
			{
				this.ProtectionCooldownUntil[key] = time;
				return;
			}
			this.ProtectionCooldownUntil.Add(key, time);
		}

		public void AddProtectionCooldownUntil(int packNumber, uint time)
		{
			string key = "protection" + packNumber;
			this.AddProtectionCooldownUntil(key, time);
		}

		public uint GetProtectionPurchaseCooldown(int packNumber)
		{
			string key = "protection" + packNumber;
			if (this.ProtectionCooldownUntil != null && this.ProtectionCooldownUntil.ContainsKey(key))
			{
				return this.ProtectionCooldownUntil[key];
			}
			return 0u;
		}

		public void RemoveTroop(string uid, int delta)
		{
			this.RemoveDeployable(uid, base.Inventory.Troop, delta);
		}

		public void RemoveSpecialAttack(string uid, int delta)
		{
			this.RemoveDeployable(uid, base.Inventory.SpecialAttack, delta);
		}

		public void RemoveHero(string uid, int delta)
		{
			this.RemoveDeployable(uid, base.Inventory.Hero, delta);
		}

		public void OnChampionKilled(string championUid)
		{
			this.RemoveChampionFromInventory(championUid);
			base.Inventory.Champion.SetItemCapacity(championUid, 0);
		}

		public void OnChampionRepaired(string championUid)
		{
			this.RemoveChampionFromInventory(championUid);
			base.Inventory.Champion.SetItemCapacity(championUid, 1);
			this.AddChampionToInventoryIfAlive(championUid);
		}

		public void RemoveChampionFromInventory(string championUid)
		{
			InventoryStorage champion = base.Inventory.Champion;
			champion.ModifyItemAmount(championUid, -champion.GetItemAmount(championUid));
		}

		public void AddChampionToInventoryIfAlive(string championUid)
		{
			InventoryStorage champion = base.Inventory.Champion;
			if (champion.GetItemCapacity(championUid) != 0)
			{
				champion.ModifyItemAmount(championUid, 1 - champion.GetItemAmount(championUid));
			}
		}

		private void RemoveDeployable(string uid, InventoryStorage storage, int delta)
		{
			int deployableCount = GameUtils.GetDeployableCount(uid, storage);
			if (deployableCount - delta < 0)
			{
				delta = deployableCount;
				Service.Get<StaRTSLogger>().WarnFormat("Can not set a deployable count less than zero. uid: {0}, amount{1}, delta: {2}", new object[]
				{
					uid,
					deployableCount,
					delta
				});
			}
			storage.ModifyItemAmount(uid, -delta);
		}

		public Dictionary<string, int> RemoveAllDeployables()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			Dictionary<string, InventoryEntry> internalStorage = base.Inventory.Troop.GetInternalStorage();
			foreach (KeyValuePair<string, InventoryEntry> current in internalStorage)
			{
				dictionary.Add(current.get_Key(), 0);
				base.Inventory.Troop.ClearItemAmount(current.get_Key());
			}
			Dictionary<string, InventoryEntry> internalStorage2 = base.Inventory.SpecialAttack.GetInternalStorage();
			foreach (KeyValuePair<string, InventoryEntry> current2 in internalStorage2)
			{
				dictionary.Add(current2.get_Key(), 0);
				base.Inventory.SpecialAttack.ClearItemAmount(current2.get_Key());
			}
			Dictionary<string, InventoryEntry> internalStorage3 = base.Inventory.Hero.GetInternalStorage();
			foreach (KeyValuePair<string, InventoryEntry> current3 in internalStorage3)
			{
				dictionary.Add(current3.get_Key(), 0);
				base.Inventory.Hero.ClearItemAmount(current3.get_Key());
			}
			Dictionary<string, InventoryEntry> internalStorage4 = base.Inventory.Champion.GetInternalStorage();
			foreach (KeyValuePair<string, InventoryEntry> current4 in internalStorage4)
			{
				dictionary.Add(current4.get_Key(), 0);
				base.Inventory.Champion.ClearItemAmount(current4.get_Key());
			}
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is HomeState || currentState is EditBaseState)
			{
				StorageSpreadUtils.UpdateAllStarportFullnessMeters();
			}
			return dictionary;
		}

		public void AddHolonetReward(string uid)
		{
			if (!string.IsNullOrEmpty(uid) && !this.HolonetRewards.Contains(uid))
			{
				this.HolonetRewards.Add(uid);
			}
		}

		public void UpdatePerksInfo(object rawPerksInfo)
		{
			if (rawPerksInfo != null)
			{
				this.PerksInfo = new PerksInfo();
				this.PerksInfo.FromObject(rawPerksInfo);
			}
		}

		public void UpdateActiveArmory(object data)
		{
			if (data != null)
			{
				this.ActiveArmory = new ActiveArmory();
				this.ActiveArmory.FromObject(data);
			}
		}

		public void UpdateArmoryInfo(object data)
		{
			if (data != null)
			{
				this.ArmoryInfo = new ArmoryInfo();
				this.ArmoryInfo.FromObject(data);
			}
		}

		public void UpdateShardsInfo(object rawShardInfo)
		{
			Dictionary<string, object> dictionary = rawShardInfo as Dictionary<string, object>;
			if (dictionary != null)
			{
				foreach (string current in dictionary.Keys)
				{
					this.Shards.Add(current, Convert.ToInt32(dictionary[current], CultureInfo.InvariantCulture));
				}
			}
		}

		public int GetShards(string equipmentID)
		{
			return this.Shards.ContainsKey(equipmentID) ? this.Shards[equipmentID] : 0;
		}

		public void SetTroopDonationProgress(object rawDonationInfo)
		{
			if (rawDonationInfo != null)
			{
				this.TroopDonationProgress = new TroopDonationProgress();
				this.TroopDonationProgress.FromObject(rawDonationInfo);
			}
		}

		public void UpdateTroopDonationProgress(int donationCount, int lastTrackedDonationTime, int repDonationCooldownTime)
		{
			this.TroopDonationProgress.DonationCount = donationCount;
			this.TroopDonationProgress.LastTrackedDonationTime = lastTrackedDonationTime;
			this.TroopDonationProgress.DonationCooldownEndTime = repDonationCooldownTime;
		}

		public void UpdateCurrentRaid(object raidData)
		{
			if (raidData != null)
			{
				Dictionary<string, object> dictionary = raidData as Dictionary<string, object>;
				string key = base.Map.PlanetId();
				object obj = null;
				if (dictionary.TryGetValue(key, out obj))
				{
					Dictionary<string, object> raidData2 = obj as Dictionary<string, object>;
					this.SetupRaidFromDictionary(raidData2);
				}
			}
		}

		public void SetupRaidFromDictionary(Dictionary<string, object> raidData)
		{
			if (raidData.ContainsKey("startTime"))
			{
				this.RaidStartTime = Convert.ToUInt32(raidData["startTime"], CultureInfo.InvariantCulture);
			}
			else
			{
				Service.Get<StaRTSLogger>().Error("Raid Data Missing Start Time");
			}
			if (raidData.ContainsKey("nextRaidStartTime"))
			{
				this.NextRaidStartTime = Convert.ToUInt32(raidData["nextRaidStartTime"], CultureInfo.InvariantCulture);
			}
			else
			{
				Service.Get<StaRTSLogger>().Error("Raid Data Missing Next Start Time");
			}
			if (raidData.ContainsKey("endTime"))
			{
				object obj = raidData["endTime"];
				if (obj == null)
				{
					this.RaidEndTime = 0u;
				}
				else
				{
					this.RaidEndTime = Convert.ToUInt32(obj, CultureInfo.InvariantCulture);
				}
			}
			else
			{
				Service.Get<StaRTSLogger>().Error("Raid Data Missing End Time");
			}
			if (raidData.ContainsKey("raidPoolId"))
			{
				string currentRaidPoolId = "";
				object obj2 = raidData["raidPoolId"];
				if (obj2 != null)
				{
					currentRaidPoolId = (obj2 as string);
				}
				this.CurrentRaidPoolId = currentRaidPoolId;
			}
			else
			{
				Service.Get<StaRTSLogger>().Error("Raid Data Missing Pool ID");
			}
			if (raidData.ContainsKey("raidId"))
			{
				string currentRaidId = "";
				object obj3 = raidData["raidId"];
				if (obj3 != null)
				{
					currentRaidId = (obj3 as string);
				}
				this.CurrentRaidId = currentRaidId;
			}
			else
			{
				Service.Get<StaRTSLogger>().Error("Raid Data Missing Raid ID");
			}
			if (raidData.ContainsKey("raidMissionId"))
			{
				string currentRaid = "";
				object obj4 = raidData["raidMissionId"];
				if (obj4 != null)
				{
					currentRaid = (obj4 as string);
				}
				this.SetCurrentRaid(currentRaid);
				return;
			}
			Service.Get<StaRTSLogger>().Error("Raid Data Missing Mission ID");
		}

		public void UpdateHolonetRewardsFromServer(object holonetRewards)
		{
			if (holonetRewards != null)
			{
				List<object> list = holonetRewards as List<object>;
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					this.HolonetRewards.Add((string)list[i]);
				}
			}
		}

		public void UpdateUnlockedPlanetsFromServer(object serverPlanetsUnlockData)
		{
			if (serverPlanetsUnlockData != null)
			{
				List<object> list = serverPlanetsUnlockData as List<object>;
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					this.UnlockedPlanets.Add((string)list[i]);
					i++;
				}
				if (!this.UnlockedPlanets.Contains("planet1"))
				{
					this.UnlockedPlanets.Add("planet1");
				}
			}
		}

		public void AddUnlockedPlanet(string planetId)
		{
			if (!string.IsNullOrEmpty(planetId))
			{
				if (!this.UnlockedPlanets.Contains(planetId))
				{
					this.UnlockedPlanets.Add(planetId);
				}
				Service.Get<EventManager>().SendEvent(EventId.PlanetUnlocked, planetId);
			}
		}

		public void AddRelocationStars(int stars)
		{
			if (this.RelocationStarsCount == -1)
			{
				return;
			}
			int requiredRelocationStars = this.GetRequiredRelocationStars();
			this.RelocationStarsCount += stars;
			if (this.RelocationStarsCount > requiredRelocationStars)
			{
				this.RelocationStarsCount = requiredRelocationStars;
			}
		}

		public void SetRelocationStartsCount(int value)
		{
			this.RelocationStarsCount = value;
		}

		public int GetRawRelocationStarsCount()
		{
			return this.RelocationStarsCount;
		}

		public int GetDisplayRelocationStarsCount()
		{
			if (this.RelocationStarsCount == -1)
			{
				return this.GetRequiredRelocationStars();
			}
			return Mathf.Clamp(this.RelocationStarsCount, 0, this.GetRequiredRelocationStars());
		}

		public int GetRequiredRelocationStars()
		{
			int num = Service.Get<CurrentPlayer>().Map.FindHighestHqLevel();
			int num2 = num - 1;
			if (num2 < 0 || num2 >= GameConstants.StarsPerRelocation.Length)
			{
				Service.Get<StaRTSLogger>().Warn("StarsPerRelocation index out of bounds! index: " + num2);
				return 50;
			}
			int num3 = GameConstants.StarsPerRelocation[num2];
			int relocationCostDiscount = Service.Get<PerkManager>().GetRelocationCostDiscount();
			int val = num3 - relocationCostDiscount;
			return Math.Max(0, val);
		}

		public void ResetRelocationStars()
		{
			this.RelocationStarsCount = 0;
		}

		public bool IsRelocationRequirementMet()
		{
			return this.RelocationStarsCount >= this.GetRequiredRelocationStars() || this.RelocationStarsCount == -1;
		}

		public void SetFreeRelocation()
		{
			this.RelocationStarsCount = -1;
		}

		public string GetFirstPlanetUnlockedUID()
		{
			if (this.UnlockedPlanets != null && this.UnlockedPlanets.Count > 0)
			{
				int count = this.UnlockedPlanets.Count;
				for (int i = 0; i < count; i++)
				{
					if (!this.UnlockedPlanets[i].Equals("planet1"))
					{
						return this.UnlockedPlanets[i];
					}
				}
			}
			return string.Empty;
		}

		public bool IsPlanetUnlocked(string planetId)
		{
			return this.UnlockedPlanets != null && this.UnlockedPlanets.Count > 0 && this.UnlockedPlanets.Contains(planetId);
		}

		public bool IsCurrentPlanet(string planetUid)
		{
			return planetUid == this.Planet.Uid;
		}

		public bool IsCurrentPlanet(PlanetVO planetVO)
		{
			return planetVO == this.Planet;
		}

		public string GetPlanetStatus(string planetUid)
		{
			if (this.IsCurrentPlanet(planetUid))
			{
				return "current";
			}
			if (this.IsPlanetUnlocked(planetUid))
			{
				return "unlocked";
			}
			return "locked";
		}

		public bool IsRelocationFree()
		{
			return this.RelocationStarsCount == -1;
		}

		public int GetCrystalRelocationCost()
		{
			int num = this.GetRequiredRelocationStars() - this.RelocationStarsCount;
			int num2 = Service.Get<CurrentPlayer>().Map.FindHighestHqLevel();
			int num3 = num2 - 1;
			if (num3 < 0 || num3 >= GameConstants.CrystalsPerRelocationStar.Length)
			{
				Service.Get<StaRTSLogger>().Warn("CrystalsPerRelocationStar index out of bounds! index: " + num3);
				return 15;
			}
			int num4 = GameConstants.CrystalsPerRelocationStar[num3];
			return num * num4;
		}

		public void ModifyShardAmount(string shardId, int newAmount)
		{
			if (this.Shards.ContainsKey(shardId))
			{
				this.Shards[shardId] = newAmount;
				return;
			}
			this.Shards.Add(shardId, newAmount);
		}

		public void DoPostContentInitialization()
		{
			this.SetCurrentRaid(this.nextRaidId);
		}

		protected internal CurrentPlayer(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).AddChampionToInventoryIfAlive(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).AddHolonetReward(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).AddRelocationStars(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).AddUnlockedPlanet(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).DoPostContentInitialization();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).ActiveArmory);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).ActiveSaveTriggers);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).ArmoryInfo);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).AttackRating);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).BattleHistory);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CampaignProgress);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentCrystalsAmount);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentDroidsAmount);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentlyDefending);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentQuest);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentRaid);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentRaidId);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentRaidPoolId);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).DamagedBuildings);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).DefenseRating);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).FirstTimePlayer);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).HolonetRewards);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).IsConnectedAccount);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).IsPushIncentivized);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).IsRateIncentivized);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).MaxDroidsAmount);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).NumIdentities);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).Objectives);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).OfferId);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).Patches);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).PerksInfo);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).Planet);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).PlanetId);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).PlayerId);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).PlayerName);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).PlayerNameInvalid);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).Prizes);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).RelocationStarsCount);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).RestoredQuest);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SessionCountToday);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).Shards);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SpecOpIntros);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).TournamentProgress);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).TroopDonationProgress);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).UnlockedPlanets);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).GetAllChampions());
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).GetAllHeroes());
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).GetAllSpecialAttacks());
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).GetAllTroops());
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).GetCrystalRelocationCost());
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).GetDisplayRelocationStarsCount());
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).GetFirstPlanetUnlockedUID());
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).GetPlanetStatus(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).GetRawRelocationStarsCount());
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).GetRequiredRelocationStars());
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).GetShards(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).HasNotCompletedFirstFueStep());
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).IsCurrentPlanet((PlanetVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).IsCurrentPlanet(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).IsPlanetUnlocked(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).IsRelocationFree());
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).IsRelocationRequirementMet());
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).ModifyShardAmount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).OnChampionKilled(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).OnChampionRepaired(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).RemoveAllDeployables());
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).RemoveChampionFromInventory(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).RemoveDeployable(Marshal.PtrToStringUni(*(IntPtr*)args), (InventoryStorage)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).RemoveHero(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).RemoveSpecialAttack(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).RemoveTroop(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).ResetRelocationStars();
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).ActiveArmory = (ActiveArmory)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).ActiveSaveTriggers = (List<IStoryTrigger>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).ArmoryInfo = (ArmoryInfo)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).AttackRating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).BattleHistory = (BattleHistory)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CampaignProgress = (CampaignProgress)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentlyDefending = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentQuest = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentRaid = (CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentRaidId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentRaidPoolId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).DamagedBuildings = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).DefenseRating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).FirstTimePlayer = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).HolonetRewards = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).IsConnectedAccount = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).IsPushIncentivized = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).IsRateIncentivized = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).NumIdentities = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).Objectives = (Dictionary<string, ObjectiveGroup>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).OfferId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).Patches = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).PerksInfo = (PerksInfo)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).PlayerId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).PlayerName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).PlayerNameInvalid = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).Prizes = (PrizeInventory)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).RelocationStarsCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).RestoredQuest = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SessionCountToday = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).Shards = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SpecOpIntros = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).TournamentProgress = (TournamentProgress)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).TroopDonationProgress = (TroopDonationProgress)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).UnlockedPlanets = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SetChampionCount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SetCurrentRaid(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SetDeployableCount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1), (InventoryStorage)GCHandledObjects.GCHandleToObject(args[2]), *(int*)(args + 3)));
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SetFreeRelocation();
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SetHeroCount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SetPlayerId();
			return -1L;
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SetRelocationStartsCount(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SetSpecialAttackCount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SetTroopCount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SetTroopDonationProgress(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).SetupRaidFromDictionary((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).UpdateActiveArmory(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).UpdateArmoryInfo(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).UpdateCurrentRaid(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).UpdateHolonetRewardsFromServer(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).UpdatePerksInfo(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).UpdateShardsInfo(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).UpdateTroopDonationProgress(*(int*)args, *(int*)(args + 1), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			((CurrentPlayer)GCHandledObjects.GCHandleToObject(instance)).UpdateUnlockedPlanetsFromServer(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
