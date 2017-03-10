using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Crates;
using StaRTS.Main.Models.Planets;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class InventoryCrateRewardController : IEventObserver
	{
		private const string CRATE_EXPIRATION_WARNING_TOAST_TITLE = "CRATE_EXPIRATION_WARNING_TOAST_TITLE";

		private const string CRATE_EXPIRATION_WARNING_TOAST_DESC = "CRATE_EXPIRATION_WARNING_TOAST_DESC";

		private const uint RETRY_TOAST_CLAMP_SEC = 60u;

		private uint nextCrateExpireTime;

		private uint expireToastTimerId;

		private uint expireBadgeTimerId;

		private uint nextDailyCrateTimerId;

		public bool IsCrateAnimationShowingOrPending
		{
			get;
			private set;
		}

		public InventoryCrateRewardController()
		{
			Service.Set<InventoryCrateRewardController>(this);
			Service.Get<EventManager>().RegisterObserver(this, EventId.CrateInventoryUpdated);
			Service.Get<EventManager>().RegisterObserver(this, EventId.InventoryCrateCollectionClosed);
			this.expireToastTimerId = 0u;
			this.expireBadgeTimerId = 0u;
			this.nextDailyCrateTimerId = 0u;
			this.nextCrateExpireTime = 0u;
			this.ScheduleCrateExpireBadgeUpdate();
			this.ScheduleCrateExpireToast();
			this.IsCrateAnimationShowingOrPending = false;
		}

		public InventoryCrateAnimation ShowInventoryCrateAnimation(List<CrateSupplyVO> crateSupplyDataList, CrateData crateData, Dictionary<string, int> shardsOriginal, Dictionary<string, int> equipmentOriginal, Dictionary<string, int> troopUpgradeOriginal, Dictionary<string, int> specialAttackUpgradeOriginal)
		{
			if (crateSupplyDataList == null || crateSupplyDataList.Count <= 0)
			{
				return null;
			}
			if (crateData == null)
			{
				return null;
			}
			return new InventoryCrateAnimation(crateSupplyDataList, crateData, shardsOriginal, equipmentOriginal, troopUpgradeOriginal, specialAttackUpgradeOriginal);
		}

		public InventoryCrateAnimation GrantInventoryCrateReward(List<string> crateSupplyDataList, CrateData crateData)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (crateSupplyDataList == null || crateSupplyDataList.Count <= 0)
			{
				return null;
			}
			if (crateData == null)
			{
				return null;
			}
			this.IsCrateAnimationShowingOrPending = true;
			UnlockedLevelData unlockedLevels = Service.Get<CurrentPlayer>().UnlockedLevels;
			Dictionary<string, int> shardsOriginal = new Dictionary<string, int>(currentPlayer.Shards);
			Dictionary<string, int> troopUpgradeOriginal = new Dictionary<string, int>(unlockedLevels.Troops.Levels);
			Dictionary<string, int> specialAttackUpgradeOriginal = new Dictionary<string, int>(unlockedLevels.Starships.Levels);
			Dictionary<string, int> equipmentOriginal = new Dictionary<string, int>(currentPlayer.UnlockedLevels.Equipment.Levels);
			List<CrateSupplyVO> list = new List<CrateSupplyVO>();
			int hQLevel = crateData.HQLevel;
			int i = 0;
			int count = crateSupplyDataList.Count;
			while (i < count)
			{
				list.Add(this.GrantSingleSupplyCrateReward(crateSupplyDataList[i], hQLevel));
				i++;
			}
			return this.ShowInventoryCrateAnimation(list, crateData, shardsOriginal, equipmentOriginal, troopUpgradeOriginal, specialAttackUpgradeOriginal);
		}

		private CrateSupplyVO GrantSingleSupplyCrateReward(string crateSupplyId, int hqLevel)
		{
			IDataController dataController = Service.Get<IDataController>();
			CrateSupplyVO crateSupplyVO = dataController.Get<CrateSupplyVO>(crateSupplyId);
			switch (crateSupplyVO.Type)
			{
			case SupplyType.Currency:
			case SupplyType.Shard:
			{
				RewardVO vo = this.GenerateRewardFromSupply(crateSupplyVO, hqLevel);
				Service.Get<RewardManager>().TryAndGrantReward(vo, null, null, false);
				break;
			}
			case SupplyType.Troop:
			case SupplyType.Hero:
			case SupplyType.SpecialAttack:
			{
				RewardVO vo2 = this.GenerateRewardFromSupply(crateSupplyVO, hqLevel);
				GameUtils.AddRewardToInventory(vo2);
				break;
			}
			case SupplyType.ShardTroop:
			case SupplyType.ShardSpecialAttack:
				Service.Get<DeployableShardUnlockController>().GrantUnlockShards(crateSupplyVO.RewardUid, this.GetRewardAmount(crateSupplyVO, hqLevel));
				break;
			case SupplyType.Invalid:
				Service.Get<StaRTSLogger>().Error("Supply Type Invalid: " + crateSupplyVO.Uid);
				break;
			}
			return crateSupplyVO;
		}

		public RewardVO GenerateRewardFromSupply(CrateSupplyVO supplyData, int hqLevel)
		{
			RewardVO rewardVO = new RewardVO();
			rewardVO.Uid = (string.IsNullOrEmpty(supplyData.ScalingUid) ? supplyData.RewardUid : supplyData.ScalingUid);
			switch (supplyData.Type)
			{
			case SupplyType.Currency:
				rewardVO.CurrencyRewards = this.GetUnitRewardUid(supplyData, hqLevel);
				break;
			case SupplyType.Shard:
				rewardVO.ShardRewards = this.GetUnitRewardUid(supplyData, hqLevel);
				break;
			case SupplyType.Troop:
				rewardVO.TroopRewards = this.GetUnitRewardUid(supplyData, hqLevel);
				break;
			case SupplyType.Hero:
				rewardVO.HeroRewards = this.GetUnitRewardUid(supplyData, hqLevel);
				break;
			case SupplyType.SpecialAttack:
				rewardVO.SpecialAttackRewards = this.GetUnitRewardUid(supplyData, hqLevel);
				break;
			}
			return rewardVO;
		}

		public string GetCrateSupplyRewardName(CrateSupplyVO supplyData)
		{
			string text = string.Empty;
			if (supplyData == null)
			{
				return text;
			}
			Lang lang = Service.Get<Lang>();
			IDataController dataController = Service.Get<IDataController>();
			string rewardUid = supplyData.RewardUid;
			SupplyType type = supplyData.Type;
			switch (type)
			{
			case SupplyType.Currency:
				text = lang.Get(supplyData.RewardUid.ToUpper(), new object[0]);
				break;
			case SupplyType.Shard:
			{
				EquipmentVO currentEquipmentDataByID = ArmoryUtils.GetCurrentEquipmentDataByID(rewardUid);
				if (currentEquipmentDataByID != null)
				{
					text = lang.Get(currentEquipmentDataByID.EquipmentName, new object[0]);
				}
				break;
			}
			case SupplyType.Troop:
			case SupplyType.Hero:
			{
				TroopTypeVO optional = dataController.GetOptional<TroopTypeVO>(rewardUid);
				if (optional != null)
				{
					text = LangUtils.GetTroopDisplayName(optional);
				}
				break;
			}
			case SupplyType.SpecialAttack:
			{
				SpecialAttackTypeVO optional2 = dataController.GetOptional<SpecialAttackTypeVO>(rewardUid);
				if (optional2 != null)
				{
					text = LangUtils.GetStarshipDisplayName(optional2);
				}
				break;
			}
			case SupplyType.ShardTroop:
			{
				ShardVO optional3 = dataController.GetOptional<ShardVO>(rewardUid);
				if (optional3 != null)
				{
					text = LangUtils.GetTroopDisplayNameFromTroopID(optional3.TargetGroupId);
				}
				break;
			}
			case SupplyType.ShardSpecialAttack:
			{
				ShardVO optional4 = dataController.GetOptional<ShardVO>(rewardUid);
				if (optional4 != null)
				{
					text = LangUtils.GetStarshipDisplayNameFromAttackID(optional4.TargetGroupId);
				}
				break;
			}
			}
			if (string.IsNullOrEmpty(text))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("CrateSupplyVO Uid:{0}, Cannot find reward for RewardUid:{1}, Type:{2}", new object[]
				{
					supplyData.Uid,
					rewardUid,
					type
				});
			}
			return text;
		}

		public int GetRewardAmount(CrateSupplyVO supplyData, int hqLevel)
		{
			if (supplyData == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Crate reward given to GetRewardAmount is null", new object[0]);
				return 0;
			}
			CrateSupplyScaleVO crateSupplyScaleVO = null;
			if (!string.IsNullOrEmpty(supplyData.ScalingUid))
			{
				crateSupplyScaleVO = Service.Get<IDataController>().GetOptional<CrateSupplyScaleVO>(supplyData.ScalingUid);
			}
			int result = 0;
			switch (supplyData.Type)
			{
			case SupplyType.Currency:
			case SupplyType.Shard:
			case SupplyType.ShardTroop:
			case SupplyType.ShardSpecialAttack:
				if (crateSupplyScaleVO == null)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Crate reward {0} requires HQ scaling data, but none found", new object[]
					{
						supplyData.Uid,
						supplyData.ScalingUid
					});
				}
				else
				{
					result = crateSupplyScaleVO.GetHQScaling(hqLevel);
				}
				break;
			case SupplyType.Troop:
			case SupplyType.Hero:
			case SupplyType.SpecialAttack:
				result = supplyData.Amount;
				break;
			}
			return result;
		}

		public string CalculatePlanetRewardChecksum(Planet currentPlanet)
		{
			List<PlanetLootEntryVO> featuredLootEntriesForPlanet = Service.Get<InventoryCrateRewardController>().GetFeaturedLootEntriesForPlanet(currentPlanet);
			featuredLootEntriesForPlanet.Sort(new Comparison<PlanetLootEntryVO>(this.SortFeaturedLootEntries));
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			int count = featuredLootEntriesForPlanet.Count;
			while (i < count)
			{
				stringBuilder.Append(featuredLootEntriesForPlanet[i].Uid);
				i++;
			}
			return CryptographyUtils.ComputeMD5Hash(stringBuilder.ToString());
		}

		public List<PlanetLootEntryVO> GetFeaturedLootEntriesForPlanet(Planet currentPlanet)
		{
			IDataController dataController = Service.Get<IDataController>();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			List<PlanetLootEntryVO> list = new List<PlanetLootEntryVO>();
			PlanetLootVO planetLootVO = dataController.Get<PlanetLootVO>(currentPlanet.VO.PlanetLootUid);
			string[] array;
			if (currentPlayer.Faction == FactionType.Empire)
			{
				array = planetLootVO.EmpirePlanetLootEntryUids;
			}
			else
			{
				array = planetLootVO.RebelPlanetLootEntryUids;
			}
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				PlanetLootEntryVO optional = dataController.GetOptional<PlanetLootEntryVO>(array[i]);
				if (optional == null)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Couldn't find PlanetLootEntryVO: {0} specified in PlanetLoot: {1}", new object[]
					{
						array[i],
						currentPlanet.VO.PlanetLootUid
					});
				}
				else if (this.IsPlanetLootEntryValidToShow(currentPlayer, optional))
				{
					list.Add(optional);
					if (list.Count >= GameConstants.PLANET_REWARDS_ITEM_THROTTLE)
					{
						break;
					}
				}
				i++;
			}
			return list;
		}

		private int SortFeaturedLootEntries(PlanetLootEntryVO vo1, PlanetLootEntryVO vo2)
		{
			return string.Compare(vo1.Uid, vo2.Uid);
		}

		private bool IsPlanetLootEntryValidToShow(CurrentPlayer player, PlanetLootEntryVO lootEntry)
		{
			int num = player.Map.FindHighestHqLevel();
			if (lootEntry.MinHQ > 0 && num < lootEntry.MinHQ)
			{
				return false;
			}
			if (lootEntry.MaxHQ > 0 && num > lootEntry.MaxHQ)
			{
				return false;
			}
			int time = (int)ServerTime.Time;
			return lootEntry.ShowDateTimeStamp <= time && (lootEntry.HideDateTimeStamp <= 0 || time < lootEntry.HideDateTimeStamp);
		}

		private string[] GetUnitRewardUid(CrateSupplyVO supplyData, int hqLevel)
		{
			return new string[]
			{
				supplyData.RewardUid + ":" + this.GetRewardAmount(supplyData, hqLevel)
			};
		}

		private CrateData GetCrateSoonestToExpire(uint afterTime)
		{
			InventoryCrates crates = Service.Get<CurrentPlayer>().Prizes.Crates;
			CrateData crateData = null;
			foreach (CrateData current in crates.Available.Values)
			{
				if (current.DoesExpire && current.ExpiresTimeStamp >= afterTime && (crateData == null || crateData.ExpiresTimeStamp > current.ExpiresTimeStamp))
				{
					crateData = current;
				}
			}
			return crateData;
		}

		private void ScheduleCrateExpireBadgeUpdate()
		{
			ViewTimerManager viewTimerManager = Service.Get<ViewTimerManager>();
			uint time = ServerTime.Time;
			if (this.expireBadgeTimerId != 0u)
			{
				viewTimerManager.KillViewTimer(this.expireBadgeTimerId);
			}
			if (!Service.IsSet<CurrentPlayer>())
			{
				return;
			}
			CrateData crateSoonestToExpire = this.GetCrateSoonestToExpire(time);
			if (crateSoonestToExpire == null)
			{
				return;
			}
			uint expiresTimeStamp = crateSoonestToExpire.ExpiresTimeStamp;
			uint num = expiresTimeStamp - time;
			if (num <= 432000u)
			{
				this.expireBadgeTimerId = viewTimerManager.CreateViewTimer(num, false, new TimerDelegate(this.UpdateBadgesAfterCrateExpire), null);
			}
		}

		private void UpdateBadgesAfterCrateExpire(uint timerId, object cookie)
		{
			InventoryCrates crates = Service.Get<CurrentPlayer>().Prizes.Crates;
			crates.UpdateBadgingBasedOnAvailableCrates();
			this.ScheduleCrateExpireBadgeUpdate();
		}

		public void ScheduleGivingNextDailyCrate()
		{
			this.CancelDailyCrateScheduledTimer();
			ViewTimerManager viewTimerManager = Service.Get<ViewTimerManager>();
			InventoryCrates crates = Service.Get<CurrentPlayer>().Prizes.Crates;
			uint nextDailCrateTimeWithSyncBuffer = crates.GetNextDailCrateTimeWithSyncBuffer();
			uint time = ServerTime.Time;
			uint num = 0u;
			if (nextDailCrateTimeWithSyncBuffer > time)
			{
				num = nextDailCrateTimeWithSyncBuffer - time;
			}
			if (num <= 432000u)
			{
				this.nextDailyCrateTimerId = viewTimerManager.CreateViewTimer(num, false, new TimerDelegate(this.UpdateDailyCrateInventory), null);
			}
		}

		public void CancelDailyCrateScheduledTimer()
		{
			if (this.nextDailyCrateTimerId != 0u)
			{
				ViewTimerManager viewTimerManager = Service.Get<ViewTimerManager>();
				viewTimerManager.KillViewTimer(this.nextDailyCrateTimerId);
				this.nextDailyCrateTimerId = 0u;
			}
		}

		private void UpdateDailyCrateInventory(uint timerId, object cookie)
		{
			this.nextDailyCrateTimerId = 0u;
			CheckDailyCrateRequest request = new CheckDailyCrateRequest();
			CheckDailyCrateCommand command = new CheckDailyCrateCommand(request);
			Service.Get<ServerAPI>().Sync(command);
		}

		private void ScheduleCrateExpireToast()
		{
			ViewTimerManager viewTimerManager = Service.Get<ViewTimerManager>();
			uint num = (uint)(GameConstants.CRATE_EXPIRATION_WARNING_TOAST * 60);
			uint time = ServerTime.Time;
			uint afterTime = time + num;
			if (this.expireToastTimerId != 0u)
			{
				viewTimerManager.KillViewTimer(this.expireToastTimerId);
			}
			if (!Service.IsSet<CurrentPlayer>())
			{
				return;
			}
			CrateData crateSoonestToExpire = this.GetCrateSoonestToExpire(afterTime);
			if (crateSoonestToExpire == null)
			{
				return;
			}
			uint num2 = crateSoonestToExpire.ExpiresTimeStamp - num;
			uint num3 = num2 - time;
			if (num3 <= 432000u)
			{
				this.nextCrateExpireTime = crateSoonestToExpire.ExpiresTimeStamp;
				this.expireToastTimerId = viewTimerManager.CreateViewTimer(num3, false, new TimerDelegate(this.AttemptShowCrateExpireToast), null);
			}
		}

		private bool IsValidGameStateForToast()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			bool flag = Service.Get<UXController>().MiscElementsManager.CanShowToast(currentState);
			bool flag2 = currentState is HomeState || currentState is EditBaseState || currentState is BaseLayoutToolState || currentState is GalaxyState || currentState is WarBoardState;
			if (flag & flag2)
			{
				SelectedBuildingScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<SelectedBuildingScreen>();
				return highestLevelScreen == null;
			}
			return false;
		}

		private void ShowCrateExpireToast()
		{
			uint num = this.nextCrateExpireTime - 60u;
			this.nextCrateExpireTime = 0u;
			if (num < ServerTime.Time)
			{
				return;
			}
			Lang lang = Service.Get<Lang>();
			string toast = lang.Get("CRATE_EXPIRATION_WARNING_TOAST_TITLE", new object[0]);
			string status = lang.Get("CRATE_EXPIRATION_WARNING_TOAST_DESC", new object[0]);
			Service.Get<UXController>().MiscElementsManager.ShowToast(toast, status, string.Empty);
			this.ScheduleCrateExpireToast();
		}

		private void AttemptShowCrateExpireToast(uint TimerId, object cookie)
		{
			if (this.IsValidGameStateForToast())
			{
				this.ShowCrateExpireToast();
				return;
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.WorldInTransitionComplete);
			Service.Get<EventManager>().RegisterObserver(this, EventId.GameStateChanged);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.GameStateChanged)
			{
				if (id == EventId.WorldInTransitionComplete || id == EventId.GameStateChanged)
				{
					if (this.IsValidGameStateForToast())
					{
						Service.Get<EventManager>().UnregisterObserver(this, EventId.WorldInTransitionComplete);
						Service.Get<EventManager>().UnregisterObserver(this, EventId.GameStateChanged);
						this.ShowCrateExpireToast();
					}
				}
			}
			else if (id != EventId.CrateInventoryUpdated)
			{
				if (id == EventId.InventoryCrateCollectionClosed)
				{
					this.IsCrateAnimationShowingOrPending = false;
				}
			}
			else
			{
				this.ScheduleCrateExpireToast();
				this.ScheduleCrateExpireBadgeUpdate();
			}
			return EatResponse.NotEaten;
		}

		protected internal InventoryCrateRewardController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).CalculatePlanetRewardChecksum((Planet)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).CancelDailyCrateScheduledTimer();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).GenerateRewardFromSupply((CrateSupplyVO)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).IsCrateAnimationShowingOrPending);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).GetCrateSupplyRewardName((CrateSupplyVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).GetFeaturedLootEntriesForPlanet((Planet)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).GetRewardAmount((CrateSupplyVO)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).GetUnitRewardUid((CrateSupplyVO)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).GrantInventoryCrateReward((List<string>)GCHandledObjects.GCHandleToObject(*args), (CrateData)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).GrantSingleSupplyCrateReward(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).IsPlanetLootEntryValidToShow((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).IsValidGameStateForToast());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).ScheduleCrateExpireBadgeUpdate();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).ScheduleCrateExpireToast();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).ScheduleGivingNextDailyCrate();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).IsCrateAnimationShowingOrPending = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).ShowCrateExpireToast();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).ShowInventoryCrateAnimation((List<CrateSupplyVO>)GCHandledObjects.GCHandleToObject(*args), (CrateData)GCHandledObjects.GCHandleToObject(args[1]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[2]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[3]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[4]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[5])));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InventoryCrateRewardController)GCHandledObjects.GCHandleToObject(instance)).SortFeaturedLootEntries((PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(*args), (PlanetLootEntryVO)GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
