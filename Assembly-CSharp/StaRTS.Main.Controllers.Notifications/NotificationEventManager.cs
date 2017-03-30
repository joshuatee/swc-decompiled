using Net.RichardLord.Ash.Core;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Perks;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers.Notifications
{
	public class NotificationEventManager : IEventObserver
	{
		private const double CONTRACT_TIME_BUFFER = 2.0;

		private const float UNIT_CONTRACT_TIME_RECALCULATE_BUFFER = 2f;

		private const int SIX_MONTHS_IN_HOURS = 4383;

		private const int FIFTEEN_SECOND_LOCAL_NOTIF_OVERRIDE = 15;

		private const int CRATE_EXPIRE_WINDOW_DELAY = 10;

		private const string NOTIFICATION_IN_PROGRESS_SUFFIX = "_progress";

		private const string NOTIFICATION_PREFIX = "notif_";

		private const string EMPIRE_SUFFIX = "_empire";

		private const string REBEL_SUFFIX = "_rebel";

		private const string BUILDING_NOTIFICATIONS = "notif1";

		private const string UNITS_READY_NOTIFICATION = "notif2";

		private const string RESEARCH_NOTIFICATIONS = "notif3";

		private const string SHORT_REENGAGEMENT_NOTIFICATION = "notif4";

		private const string LONG_REENGAGEMENT_NOTIFICATION = "notif5";

		private const string RESOURCES_READY_NOTIFICATION = "notif6";

		private const string TOURNAMENT_ENDED_NOTIFICATION = "notif_tournament_ended";

		private const string NEXT_RAID_NOTIFICATION = "raid_start_next";

		private const string SQUAD_WAR_USE_TURN = "squadwars_action_turns_reminder";

		private const string PERKS_INACTIVE_NOTIFICATION = "perk_all_slots_empty";

		private const string CRATE_EXPIRE_NOTIFICATION = "crate_expiration_warning";

		private const string DAILY_CRATE_NOTIFICATION = "daily_crate_next";

		private NotificationController notificationController;

		private EventManager eventManager;

		private Lang lang;

		private string notifMessageKeySuffix;

		public void Init()
		{
			this.notificationController = Service.Get<NotificationController>();
			this.eventManager = Service.Get<EventManager>();
			this.lang = Service.Get<Lang>();
			this.eventManager.RegisterObserver(this, EventId.ApplicationPauseToggled, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.ApplicationQuit, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.BattleLoadStart, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.TournamentEntered, EventPriority.Notification);
			this.eventManager.RegisterObserver(this, EventId.VisitPlayer, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Notification);
			if (Service.Get<ServerPlayerPrefs>().GetPref(ServerPref.FactionFlipped) == "1")
			{
				FactionType faction = Service.Get<CurrentPlayer>().Faction;
				if (faction != FactionType.Empire)
				{
					if (faction == FactionType.Rebel)
					{
						this.notifMessageKeySuffix = "_rebel";
					}
				}
				else
				{
					this.notifMessageKeySuffix = "_empire";
				}
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.ApplicationPauseToggled)
			{
				if (id != EventId.ApplicationQuit && id != EventId.VisitPlayer)
				{
					if (id == EventId.WorldLoadComplete)
					{
						IState currentState = Service.Get<GameStateMachine>().CurrentState;
						if (currentState is ApplicationLoadState)
						{
							this.RescheduleAllLocalNotifications();
						}
						return EatResponse.NotEaten;
					}
					if (id != EventId.BattleLoadStart)
					{
						return EatResponse.NotEaten;
					}
				}
				this.RescheduleAllLocalNotifications();
			}
			else if ((bool)cookie)
			{
				this.RescheduleAllLocalNotifications();
			}
			return EatResponse.NotEaten;
		}

		private NotificationObject CreateSupportNotification(string productUid, int remainingTime, string buildingKey, string notifTypeKey, string displayName, int level)
		{
			NotificationTypeVO notificationTypeVO = Service.Get<IDataController>().Get<NotificationTypeVO>(notifTypeKey);
			if (remainingTime >= notificationTypeVO.MinCompletionTime)
			{
				string id = (this.notifMessageKeySuffix == null) ? ("notif_" + notifTypeKey) : ("notif_" + notifTypeKey + this.notifMessageKeySuffix);
				string message = this.lang.Get(id, new object[]
				{
					displayName,
					level
				});
				string inProgressMessage = this.lang.Get("notif_" + notifTypeKey + "_progress", new object[]
				{
					displayName,
					level
				});
				DateTime time = DateTime.Now.AddSeconds((double)remainingTime + 2.0);
				return new NotificationObject(notifTypeKey, inProgressMessage, message, notificationTypeVO.SoundName, time, buildingKey, productUid);
			}
			return null;
		}

		private NotificationObject CreateBuildingNotification(string productUid, int remainingTime, string buildingKey)
		{
			BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(productUid);
			string buildingDisplayName = LangUtils.GetBuildingDisplayName(buildingTypeVO);
			return this.CreateSupportNotification(productUid, remainingTime, buildingKey, "notif1", buildingDisplayName, buildingTypeVO.Lvl);
		}

		private NotificationObject GetSquadWarUseTurnNotification()
		{
			uint time = ServerTime.Time;
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			if (warManager == null || warManager.CurrentSquadWar == null)
			{
				return null;
			}
			SquadWarParticipantState currentParticipantState = warManager.GetCurrentParticipantState();
			if (currentParticipantState == null || currentParticipantState.TurnsLeft <= 0)
			{
				return null;
			}
			if ((long)warManager.CurrentSquadWar.ActionEndTimeStamp - (long)((ulong)time) <= 0L)
			{
				return null;
			}
			int num = warManager.CurrentSquadWar.ActionEndTimeStamp - GameConstants.WAR_NOTIF_ACTION_TURNS_REMINDER * 3600 - (int)time;
			if (num <= 0)
			{
				return null;
			}
			string text = "squadwars_action_turns_reminder";
			NotificationTypeVO notificationTypeVO = Service.Get<IDataController>().Get<NotificationTypeVO>(text);
			string id = "notif_" + text;
			string message = this.lang.Get(id, new object[0]);
			string inProgressMessage = this.lang.Get("notif_" + text + "_progress", new object[0]);
			DateTime time2 = DateTime.Now.AddSeconds((double)num);
			if (!this.CheckValidNotificationTime(notificationTypeVO, time2))
			{
				return null;
			}
			return new NotificationObject(text, inProgressMessage, message, notificationTypeVO.SoundName, time2, text, text);
		}

		private NotificationObject CreateResearchNotification(string productUid, DeliveryType contractType, int remainingTime, string buildingKey)
		{
			IDataController dataController = Service.Get<IDataController>();
			string displayName = null;
			int level = 0;
			switch (contractType)
			{
			case DeliveryType.UpgradeTroop:
			{
				TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(productUid);
				displayName = LangUtils.GetTroopDisplayName(troopTypeVO);
				level = troopTypeVO.Lvl;
				break;
			}
			case DeliveryType.UpgradeStarship:
			{
				SpecialAttackTypeVO specialAttackTypeVO = dataController.Get<SpecialAttackTypeVO>(productUid);
				displayName = LangUtils.GetStarshipDisplayName(specialAttackTypeVO);
				level = specialAttackTypeVO.Lvl;
				break;
			}
			case DeliveryType.UpgradeEquipment:
			{
				EquipmentVO equipmentVO = dataController.Get<EquipmentVO>(productUid);
				displayName = LangUtils.GetEquipmentDisplayName(equipmentVO);
				level = equipmentVO.Lvl;
				break;
			}
			}
			return this.CreateSupportNotification(productUid, remainingTime, buildingKey, "notif3", displayName, level);
		}

		private NotificationObject CreateReengagementNotification(string notifTypeKey, DateTime time, bool tryMessageSuffix)
		{
			NotificationTypeVO notificationTypeVO = Service.Get<IDataController>().Get<NotificationTypeVO>(notifTypeKey);
			bool flag = tryMessageSuffix && this.notifMessageKeySuffix != null;
			string id = (!flag) ? ("notif_" + notifTypeKey) : ("notif_" + notifTypeKey + this.notifMessageKeySuffix);
			string message = this.lang.Get(id, new object[0]);
			string inProgressMessage = this.lang.Get("notif_" + notifTypeKey + "_progress", new object[0]);
			return new NotificationObject(notifTypeKey, inProgressMessage, message, notificationTypeVO.SoundName, time, notifTypeKey, notifTypeKey);
		}

		private List<NotificationObject> GetBuildingNotifications()
		{
			if (!Service.IsSet<CurrentPlayer>() || Service.Get<CurrentPlayer>().CampaignProgress == null)
			{
				return null;
			}
			if (Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				return null;
			}
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (!(currentState is HomeState) && !(currentState is ApplicationLoadState))
			{
				return null;
			}
			List<NotificationObject> list = new List<NotificationObject>();
			NodeList<SupportNode> nodeList = Service.Get<EntityController>().GetNodeList<SupportNode>();
			ISupportController supportController = Service.Get<ISupportController>();
			for (SupportNode supportNode = nodeList.Head; supportNode != null; supportNode = supportNode.Next)
			{
				BuildingComponent buildingComp = supportNode.BuildingComp;
				string key = buildingComp.BuildingTO.Key;
				Contract contract = supportController.FindCurrentContract(key);
				if (contract != null)
				{
					NotificationObject notificationObject = null;
					DeliveryType deliveryType = contract.DeliveryType;
					int remainingTimeForSim = contract.GetRemainingTimeForSim();
					switch (deliveryType)
					{
					case DeliveryType.Building:
					case DeliveryType.UpgradeBuilding:
						notificationObject = this.CreateBuildingNotification(contract.ProductUid, remainingTimeForSim, key);
						break;
					case DeliveryType.UpgradeTroop:
					case DeliveryType.UpgradeStarship:
					case DeliveryType.UpgradeEquipment:
						notificationObject = this.CreateResearchNotification(contract.ProductUid, deliveryType, remainingTimeForSim, key);
						break;
					}
					if (notificationObject != null)
					{
						list.Add(notificationObject);
					}
				}
			}
			return list;
		}

		private NotificationObject GetUnitsCompleteNotification()
		{
			if (!Service.IsSet<CurrentPlayer>() || Service.Get<CurrentPlayer>().CampaignProgress == null)
			{
				return null;
			}
			if (Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress || !(Service.Get<GameStateMachine>().CurrentState is HomeState))
			{
				return null;
			}
			NotificationTypeVO notificationTypeVO = Service.Get<IDataController>().Get<NotificationTypeVO>("notif2");
			int num = 0;
			NodeList<SupportNode> nodeList = Service.Get<EntityController>().GetNodeList<SupportNode>();
			ISupportController supportController = Service.Get<ISupportController>();
			for (SupportNode supportNode = nodeList.Head; supportNode != null; supportNode = supportNode.Next)
			{
				BuildingComponent buildingComp = supportNode.BuildingComp;
				string key = buildingComp.BuildingTO.Key;
				if (!ContractUtils.IsBuildingUpgrading(supportNode.Entity) && !ContractUtils.IsBuildingConstructing(supportNode.Entity) && !supportController.IsBuildingFrozen(key))
				{
					Contract contract = supportController.FindCurrentContract(key);
					if (contract != null && supportController.IsContractValidForStorage(contract))
					{
						int num2 = ContractUtils.CalculateRemainingTimeOfAllTroopContracts(supportNode.Entity);
						if (num2 > num)
						{
							num = num2;
						}
					}
				}
			}
			if (num >= notificationTypeVO.MinCompletionTime)
			{
				DateTime time = DateTime.Now.AddSeconds((double)num + 2.0);
				return this.CreateReengagementNotification("notif2", time, true);
			}
			return null;
		}

		private NotificationObject GetLastPerkExpiredNotification()
		{
			if (!Service.IsSet<CurrentPlayer>())
			{
				return null;
			}
			if (Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress || !(Service.Get<GameStateMachine>().CurrentState is HomeState))
			{
				return null;
			}
			NotificationTypeVO notificationTypeVO = Service.Get<IDataController>().Get<NotificationTypeVO>("perk_all_slots_empty");
			List<ActivatedPerkData> playerActivePerks = Service.Get<PerkManager>().GetPlayerActivePerks();
			uint num = 0u;
			if (playerActivePerks != null)
			{
				int count = playerActivePerks.Count;
				for (int i = 0; i < count; i++)
				{
					uint time = ServerTime.Time;
					uint num2 = playerActivePerks[i].EndTime - time;
					if (playerActivePerks[i].EndTime > time && num2 > num)
					{
						num = num2;
					}
				}
			}
			if (num > 0u && (ulong)num >= (ulong)((long)notificationTypeVO.MinCompletionTime))
			{
				DateTime time2 = DateTime.Now.AddSeconds(num);
				return this.CreateReengagementNotification("perk_all_slots_empty", time2, false);
			}
			return null;
		}

		private NotificationObject GetGeneratorNotification()
		{
			if (!Service.IsSet<CurrentPlayer>() || Service.Get<CurrentPlayer>().CampaignProgress == null)
			{
				return null;
			}
			if (Service.Get<CurrentPlayer>().FirstTimePlayer || Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				return null;
			}
			ICurrencyController currencyController = Service.Get<ICurrencyController>();
			int num = currencyController.CalculateTimeUntilAllGeneratorsFull();
			NotificationTypeVO notificationTypeVO = Service.Get<IDataController>().Get<NotificationTypeVO>("notif6");
			if (num > notificationTypeVO.MinCompletionTime)
			{
				DateTime time = DateTime.Now.AddSeconds((double)num + 2.0);
				return this.CreateReengagementNotification("notif6", time, true);
			}
			return null;
		}

		private List<NotificationObject> GetShortReengagementNotifs()
		{
			string text = "notif4";
			string uid = "notif5";
			NotificationTypeVO notificationTypeVO = Service.Get<IDataController>().Get<NotificationTypeVO>(text);
			NotificationTypeVO notificationTypeVO2 = Service.Get<IDataController>().Get<NotificationTypeVO>(uid);
			if (notificationTypeVO.RepeatTime < 1)
			{
				return null;
			}
			if (notificationTypeVO2.RepeatTime < 1)
			{
				return null;
			}
			List<NotificationObject> list = new List<NotificationObject>();
			DateTime time = DateTime.Now.AddHours((double)notificationTypeVO.RepeatTime);
			this.EnsureValidNotificationTime(notificationTypeVO, time);
			string id = (this.notifMessageKeySuffix == null) ? ("notif_" + text) : ("notif_" + text + this.notifMessageKeySuffix);
			string message = this.lang.Get(id, new object[0]);
			string inProgressMessage = this.lang.Get("notif_" + text + "_progress", new object[0]);
			int i = 0;
			while (i < notificationTypeVO2.RepeatTime)
			{
				NotificationObject item = new NotificationObject(text, inProgressMessage, message, notificationTypeVO.SoundName, time, text, text);
				list.Add(item);
				i += notificationTypeVO.RepeatTime;
				time = time.AddHours((double)notificationTypeVO.RepeatTime);
			}
			return list;
		}

		private List<NotificationObject> GetLongReengagementNotifs()
		{
			string text = "notif5";
			NotificationTypeVO notificationTypeVO = Service.Get<IDataController>().Get<NotificationTypeVO>(text);
			if (notificationTypeVO.RepeatTime < 1)
			{
				Service.Get<Logger>().Error("LongTermNotification has 0 repeat time");
				return null;
			}
			List<NotificationObject> list = new List<NotificationObject>();
			string id = (this.notifMessageKeySuffix == null) ? ("notif_" + text) : ("notif_" + text + this.notifMessageKeySuffix);
			string message = this.lang.Get(id, new object[0]);
			string inProgressMessage = this.lang.Get("notif_" + text + "_progress", new object[0]);
			DateTime time = DateTime.Now.AddHours((double)notificationTypeVO.RepeatTime);
			this.EnsureValidNotificationTime(notificationTypeVO, time);
			int i = 0;
			while (i <= 4383)
			{
				NotificationObject item = new NotificationObject(text, inProgressMessage, message, notificationTypeVO.SoundName, time, text, text);
				list.Add(item);
				i += notificationTypeVO.RepeatTime;
				time = time.AddHours((double)notificationTypeVO.RepeatTime);
			}
			return list;
		}

		private NotificationObject GetNextRaidNotification()
		{
			RaidDefenseController raidDefenseController = Service.Get<RaidDefenseController>();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (!raidDefenseController.AreRaidsAccessible())
			{
				return null;
			}
			uint secondsTillNextRaid = raidDefenseController.GetSecondsTillNextRaid();
			if (secondsTillNextRaid == 0u)
			{
				Service.Get<Logger>().Error("Failed to schedule raid notif due to invalid start time: " + currentPlayer.CurrentRaid.Uid);
				return null;
			}
			string text = "raid_start_next";
			NotificationTypeVO notificationTypeVO = Service.Get<IDataController>().Get<NotificationTypeVO>(text);
			string id = "notif_" + text;
			string message = this.lang.Get(id, new object[0]);
			string inProgressMessage = this.lang.Get("notif_" + text + "_progress", new object[0]);
			DateTime time = DateTime.Now.AddSeconds(secondsTillNextRaid);
			if (!this.CheckValidNotificationTime(notificationTypeVO, time))
			{
				return null;
			}
			return new NotificationObject(text, inProgressMessage, message, notificationTypeVO.SoundName, time, text, text);
		}

		private NotificationObject GetInventoryCrateExpirationNotification()
		{
			uint time = ServerTime.Time;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			InventoryCrates crates = currentPlayer.Prizes.Crates;
			uint num = (uint)(GameConstants.CRATE_EXPIRATION_WARNING_NOTIF * 60);
			uint cRATE_EXPIRATION_WARNING_NOTIF_MINIMUM = (uint)GameConstants.CRATE_EXPIRATION_WARNING_NOTIF_MINIMUM;
			CrateData nextInventoryCrateToExpire = GameUtils.GetNextInventoryCrateToExpire(crates, time + cRATE_EXPIRATION_WARNING_NOTIF_MINIMUM);
			if (nextInventoryCrateToExpire == null)
			{
				return null;
			}
			uint expiresTimeStamp = nextInventoryCrateToExpire.ExpiresTimeStamp;
			uint num2 = expiresTimeStamp - time;
			if (num2 > num)
			{
				num2 -= num;
				if (num2 < num)
				{
					num2 = num;
				}
			}
			else
			{
				num2 = 10u;
			}
			string text = "crate_expiration_warning";
			NotificationTypeVO notificationTypeVO = Service.Get<IDataController>().Get<NotificationTypeVO>(text);
			string id = "notif_" + text;
			string message = this.lang.Get(id, new object[0]);
			string inProgressMessage = this.lang.Get("notif_" + text + "_progress", new object[0]);
			DateTime time2 = DateTime.Now.AddSeconds(num2);
			if (!this.CheckValidNotificationTime(notificationTypeVO, time2))
			{
				return null;
			}
			return new NotificationObject(text, inProgressMessage, message, notificationTypeVO.SoundName, time2, text, text);
		}

		private NotificationObject GetNextDailyCrateNotification()
		{
			if (!GameConstants.CRATE_DAILY_CRATE_ENABLED)
			{
				return null;
			}
			if (!Service.IsSet<CurrentPlayer>() || Service.Get<CurrentPlayer>().CampaignProgress == null || Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				return null;
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			uint nextDailyCrateTime = currentPlayer.Prizes.Crates.NextDailyCrateTime;
			if (nextDailyCrateTime == 0u)
			{
				Service.Get<Logger>().Error("Did not schedule next daily crate notif due to invalid scheduled time");
				return null;
			}
			uint time = ServerTime.Time;
			uint num = nextDailyCrateTime + (uint)(GameConstants.CRATE_DAILY_CRATE_NOTIF_OFFSET * 60);
			uint num2 = num - time;
			if (num2 <= 0u)
			{
				return null;
			}
			string text = "daily_crate_next";
			NotificationTypeVO notificationTypeVO = Service.Get<IDataController>().Get<NotificationTypeVO>(text);
			DateTime time2 = DateTime.Now.AddSeconds(num2);
			if (!this.CheckValidNotificationTime(notificationTypeVO, time2))
			{
				return null;
			}
			if ((ulong)num2 > (ulong)((long)notificationTypeVO.MinCompletionTime))
			{
				return this.CreateReengagementNotification(text, time2, false);
			}
			return null;
		}

		private void RescheduleAllLocalNotifications()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (!(currentState is HomeState))
			{
				return;
			}
			List<NotificationObject> list = new List<NotificationObject>();
			List<NotificationObject> buildingNotifications = this.GetBuildingNotifications();
			if (buildingNotifications != null)
			{
				list.AddRange(buildingNotifications);
			}
			NotificationObject generatorNotification = this.GetGeneratorNotification();
			if (generatorNotification != null)
			{
				list.Add(generatorNotification);
			}
			NotificationObject nextRaidNotification = this.GetNextRaidNotification();
			if (nextRaidNotification != null)
			{
				list.Add(nextRaidNotification);
			}
			NotificationObject inventoryCrateExpirationNotification = this.GetInventoryCrateExpirationNotification();
			if (inventoryCrateExpirationNotification != null)
			{
				list.Add(inventoryCrateExpirationNotification);
			}
			NotificationObject nextDailyCrateNotification = this.GetNextDailyCrateNotification();
			if (nextDailyCrateNotification != null)
			{
				list.Add(nextDailyCrateNotification);
			}
			NotificationObject squadWarUseTurnNotification = this.GetSquadWarUseTurnNotification();
			if (squadWarUseTurnNotification != null)
			{
				list.Add(squadWarUseTurnNotification);
			}
			NotificationObject unitsCompleteNotification = this.GetUnitsCompleteNotification();
			if (unitsCompleteNotification != null)
			{
				list.Add(unitsCompleteNotification);
			}
			NotificationObject lastPerkExpiredNotification = this.GetLastPerkExpiredNotification();
			if (lastPerkExpiredNotification != null)
			{
				list.Add(lastPerkExpiredNotification);
			}
			List<NotificationObject> shortReengagementNotifs = this.GetShortReengagementNotifs();
			if (shortReengagementNotifs != null)
			{
				list.AddRange(shortReengagementNotifs);
			}
			List<NotificationObject> longReengagementNotifs = this.GetLongReengagementNotifs();
			if (longReengagementNotifs != null)
			{
				list.AddRange(longReengagementNotifs);
			}
			this.notificationController.BatchScheduleLocalNotifications(list);
		}

		private void EnsureValidNotificationTime(NotificationTypeVO notifType, DateTime time)
		{
			int earliestValidTime = notifType.EarliestValidTime;
			int latestValidTime = notifType.LatestValidTime;
			int num = 0;
			if (time.Hour < earliestValidTime)
			{
				num = earliestValidTime - time.Hour;
			}
			else if (time.Hour > latestValidTime)
			{
				num = 24 - time.Hour + earliestValidTime;
			}
			time = time.AddHours((double)num);
		}

		private bool CheckValidNotificationTime(NotificationTypeVO notifType, DateTime time)
		{
			bool result = true;
			int earliestValidTime = notifType.EarliestValidTime;
			int latestValidTime = notifType.LatestValidTime;
			if (time.Hour < earliestValidTime)
			{
				result = false;
			}
			else if (time.Hour > latestValidTime)
			{
				result = false;
			}
			return result;
		}
	}
}
