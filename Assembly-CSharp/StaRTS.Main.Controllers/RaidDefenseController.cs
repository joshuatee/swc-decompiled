using Net.RichardLord.Ash.Core;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.FX;
using StaRTS.Main.Configs;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player.Raids;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class RaidDefenseController : IEventObserver, IViewClockTimeObserver
	{
		public delegate void OnSuccessCallback(AbstractResponse response, object cookie);

		public const string HERO_TRAINER_DESTROYED = "HERO_TRAINER_DESTROYED";

		public const string STARSHIP_TRAINER_DESTROYED = "STARSHIP_TRAINER_DESTROYED";

		public const string SQUAD_CENTER_DESTROYED = "SQUAD_CENTER_DESTROYED";

		private const string HOLO_LOCATOR = "locator_fx";

		private const string RAID_TIME_REMAINING_ACTIVE = "RAID_TIME_REMAINING_ACTIVE";

		private const string OK = "s_Ok";

		private const string RAID_CONFIRM_TITLE = "RAID_CONFIRM_TITLE";

		private const string RAID_CONFIRM_DESC = "RAID_CONFIRM_DESC";

		private const string RAID_CANCEL = "s_Cancel";

		private const string SKIP_FUTURE_CONFIRMATION = "SKIP_FUTURE_CONFIRMATION";

		private const string RAID_WAIT_TITLE = "RAID_WAIT_TITLE";

		private const string RAID_WAIT_DESC = "RAID_WAIT_DESC";

		private const string RAID_START = "RAID_START";

		private HashSet<BuildingType> raidDefenseTrainerBindings;

		private Action onRaidEndCallback;

		private uint nextUpdateTimestamp;

		private RaidDefenseController.OnSuccessCallback raidStartCallback;

		private string lastAwardedCrateUid;

		private string finalWaveIdOfLastDefense;

		private BuildingHoloEffect scoutHolo;

		public Color ActiveRaidColor
		{
			get;
			private set;
		}

		public Color InactiveColor
		{
			get;
			private set;
		}

		public Color InactiveTickerColor
		{
			get;
			private set;
		}

		public RaidDefenseController()
		{
			this.InactiveColor = new Color(0.95f, 0.84f, 0.18f);
			this.InactiveTickerColor = Color.white;
			this.ActiveRaidColor = new Color(0.9f, 0f, 0f);
			this.nextUpdateTimestamp = 0u;
			Service.Set<RaidDefenseController>(this);
			string[] array = GameConstants.RAID_DEFENSE_TRAINER_BINDINGS.Split(new char[]
			{
				','
			});
			this.raidDefenseTrainerBindings = new HashSet<BuildingType>();
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				this.raidDefenseTrainerBindings.Add(StringUtils.ParseEnum<BuildingType>(array[i]));
				i++;
			}
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.ContractStarted, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ContractContinued, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingCancelled, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingConstructed, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingReplaced, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingViewReady, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.PlanetRelocateStarted, EventPriority.Default);
			Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
			this.SetupRaidUpdateTimer();
			Service.Get<UXController>().MiscElementsManager.AddRaidsTickerStatus(this);
		}

		public bool AreRaidsAccessible()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			int num = currentPlayer.Map.FindHighestHqLevel();
			bool flag = true;
			flag &= (GameConstants.RAIDS_HQ_UNLOCK_LEVEL <= num && currentPlayer.CurrentRaid != null);
			return flag & currentPlayer.Map.ScoutTowerExists();
		}

		public void OnStartRaidDefenseMission()
		{
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
			this.RegisterBattleObservers();
		}

		public void OnEndRaidDefenseMission(string finalWaveId)
		{
			this.finalWaveIdOfLastDefense = finalWaveId;
			this.UnregisterBattleObservers();
		}

		private void StartCurrentRaidDefenseInternal()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (currentPlayer.CurrentRaid == null || !Service.Get<RaidDefenseController>().IsRaidAvailable())
			{
				return;
			}
			Service.Get<CampaignController>().StartMission(currentPlayer.CurrentRaid);
		}

		public int GetRaidTimeSeconds()
		{
			uint result;
			if (this.IsRaidAvailable())
			{
				result = this.GetSecondsLeftBeforeRaidEnds();
			}
			else
			{
				result = this.GetSecondsTillNextRaid();
			}
			return (int)result;
		}

		public void AttemptToShowRaidWaitConfirmation()
		{
			Lang lang = Service.Get<Lang>();
			if (!PlayerSettings.GetSkipRaidWaitConfirmation() && this.IsRaidAvailable())
			{
				AlertWithCheckBoxScreen alertWithCheckBoxScreen = new AlertWithCheckBoxScreen(lang.Get("RAID_WAIT_TITLE", new object[0]), lang.Get("RAID_WAIT_DESC", new object[0]), "SKIP_FUTURE_CONFIRMATION", "RAID_TIME_REMAINING_ACTIVE", this.GetRaidTimeSeconds(), this.ActiveRaidColor, new AlertWithCheckBoxScreen.OnCheckBoxScreenModalResult(this.OnWaitScreenClosed));
				alertWithCheckBoxScreen.SetPrimaryLabelText(lang.Get("s_Ok", new object[0]));
				alertWithCheckBoxScreen.Set2ButtonGroupEnabledState(false);
				Service.Get<ScreenController>().AddScreen(alertWithCheckBoxScreen);
			}
		}

		private void OnWaitScreenClosed(object result, bool skipFuture)
		{
			PlayerSettings.SetSkipRaidWaitConfirmation(skipFuture);
		}

		public void StartCurrentRaidDefense()
		{
			Lang lang = Service.Get<Lang>();
			if (!PlayerSettings.GetSkipRaidDefendConfirmation())
			{
				AlertWithCheckBoxScreen alertWithCheckBoxScreen = new AlertWithCheckBoxScreen(lang.Get("RAID_CONFIRM_TITLE", new object[0]), lang.Get("RAID_CONFIRM_DESC", new object[0]), "SKIP_FUTURE_CONFIRMATION", "RAID_TIME_REMAINING_ACTIVE", this.GetRaidTimeSeconds(), this.ActiveRaidColor, new AlertWithCheckBoxScreen.OnCheckBoxScreenModalResult(this.OnDefendNowScreenClosed));
				alertWithCheckBoxScreen.SetPrimaryLabelText(lang.Get("RAID_START", new object[0]));
				alertWithCheckBoxScreen.SetSecondaryLabelText(lang.Get("s_Cancel", new object[0]));
				alertWithCheckBoxScreen.Set2ButtonGroupEnabledState(true);
				Service.Get<ScreenController>().AddScreen(alertWithCheckBoxScreen);
				return;
			}
			this.StartCurrentRaidDefenseInternal();
		}

		private void OnDefendNowScreenClosed(object result, bool skipFuture)
		{
			if (result != null)
			{
				PlayerSettings.SetSkipRaidDefendConfirmation(skipFuture);
				this.StartCurrentRaidDefenseInternal();
			}
		}

		public void SetupRaidUpdateTimer()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (this.IsRaidAvailable())
			{
				this.nextUpdateTimestamp = currentPlayer.RaidEndTime;
				return;
			}
			this.nextUpdateTimestamp = currentPlayer.RaidStartTime;
		}

		public string GetDisplayForTimeTillNextRaid()
		{
			return GameUtils.GetTimeLabelFromSeconds((int)this.GetSecondsTillNextRaid());
		}

		public string GetDisplayForTimeLeftInRaid()
		{
			return GameUtils.GetTimeLabelFromSeconds((int)this.GetSecondsLeftBeforeRaidEnds());
		}

		public uint GetSecondsTillNextRaid()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			uint result = 0u;
			uint num = currentPlayer.RaidStartTime;
			uint raidEndTime = currentPlayer.RaidEndTime;
			uint time = ServerTime.Time;
			if (this.IsRaidAvailable())
			{
				num = currentPlayer.NextRaidStartTime;
			}
			if (num > raidEndTime && num > time)
			{
				result = num - time;
			}
			return result;
		}

		public uint GetSecondsLeftBeforeRaidEnds()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			uint result = 0u;
			uint raidStartTime = currentPlayer.RaidStartTime;
			uint raidEndTime = currentPlayer.RaidEndTime;
			uint time = ServerTime.Time;
			if (raidEndTime > raidStartTime && raidEndTime > time)
			{
				result = raidEndTime - time;
			}
			return result;
		}

		private void UpdateRaidExpiration()
		{
			if (this.nextUpdateTimestamp > 0u && ServerTime.Time >= this.nextUpdateTimestamp)
			{
				this.nextUpdateTimestamp = 0u;
				this.SendRaidDefenseUpdate();
			}
		}

		public void OnViewClockTime(float dt)
		{
			this.UpdateRaidExpiration();
		}

		public void SendRaidDefenseUpdate()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			RaidUpdateRequest request = new RaidUpdateRequest(currentPlayer.PlanetId);
			RaidUpdateCommand raidUpdateCommand = new RaidUpdateCommand(request);
			raidUpdateCommand.AddSuccessCallback(new AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>.OnSuccessCallback(this.OnRaidUpdateSuccess));
			Service.Get<ServerAPI>().Sync(raidUpdateCommand);
		}

		private void OnRaidUpdateSuccess(RaidUpdateResponse response, object cookie)
		{
			this.SetupRaidUpdateTimer();
			if (this.IsRaidAvailable())
			{
				this.CreateScoutHolo();
				return;
			}
			this.DestroyScoutHolo();
		}

		public void SendRaidDefenseComplete(CampaignMissionVO raidMission, Action endRaidCallback, EndPvEBattleTO endBattleTO)
		{
			CampaignMissionVO currentRaid = Service.Get<CurrentPlayer>().CurrentRaid;
			if (GameUtils.SafeVOEqualityValidation(raidMission, currentRaid))
			{
				RaidDefenseCompleteRequest request = new RaidDefenseCompleteRequest(endBattleTO, this.finalWaveIdOfLastDefense);
				RaidDefenseCompleteCommand raidDefenseCompleteCommand = new RaidDefenseCompleteCommand(request);
				raidDefenseCompleteCommand.AddSuccessCallback(new AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>.OnSuccessCallback(this.OnRaidCompleteSuccessWrapper));
				this.onRaidEndCallback = endRaidCallback;
				Service.Get<ServerAPI>().Sync(raidDefenseCompleteCommand);
				this.finalWaveIdOfLastDefense = "";
				return;
			}
			Service.Get<StaRTSLogger>().Error("Ended Raid Defense does not match the current raid.Ended: " + this.GetUidToLog(raidMission) + ", Scheduled: " + this.GetUidToLog(currentRaid));
		}

		public void SendRaidDefenseStart(CampaignMissionVO raidMission, RaidDefenseController.OnSuccessCallback successCB)
		{
			CampaignMissionVO currentRaid = Service.Get<CurrentPlayer>().CurrentRaid;
			if (GameUtils.SafeVOEqualityValidation(raidMission, currentRaid))
			{
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				RaidDefenseStartRequest request = new RaidDefenseStartRequest(currentPlayer.PlanetId, raidMission.Uid);
				RaidDefenseStartCommand raidDefenseStartCommand = new RaidDefenseStartCommand(request);
				this.raidStartCallback = successCB;
				raidDefenseStartCommand.AddSuccessCallback(new AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>.OnSuccessCallback(this.OnRaidStartSuccessWrapper));
				Service.Get<ServerAPI>().Sync(raidDefenseStartCommand);
				return;
			}
			Service.Get<StaRTSLogger>().Error("Started Raid Defense does not match the next scheduled raid.Started: " + this.GetUidToLog(raidMission) + ", Scheduled: " + this.GetUidToLog(currentRaid));
		}

		private void OnRaidStartSuccessWrapper(RaidDefenseStartResponse response, object cookie)
		{
			if (this.raidStartCallback != null)
			{
				this.raidStartCallback(response, cookie);
			}
		}

		private bool RaidCompleteDidAwardCrate()
		{
			return this.lastAwardedCrateUid != null;
		}

		private void OnRaidCompleteSuccessWrapper(RaidDefenseCompleteResponse response, object cookie)
		{
			this.SetupRaidUpdateTimer();
			Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
			this.UpdateRaidExpiration();
			this.lastAwardedCrateUid = response.AwardedCrateUid;
			if (this.RaidCompleteDidAwardCrate())
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.WorldLoadComplete);
			}
			if (this.onRaidEndCallback != null)
			{
				this.onRaidEndCallback.Invoke();
			}
		}

		public bool IsRaidAvailable()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			uint raidEndTime = currentPlayer.RaidEndTime;
			uint raidStartTime = currentPlayer.RaidStartTime;
			if (raidEndTime > 0u)
			{
				uint time = ServerTime.Time;
				return raidEndTime > time && raidStartTime <= time;
			}
			return false;
		}

		public void ShowRaidInfo()
		{
			if (this.AreRaidsAccessible())
			{
				Service.Get<ScreenController>().AddScreen(new RaidInfoScreen());
				return;
			}
			Service.Get<StaRTSLogger>().Warn("Tried to Show Raid Briefing While Raids Are Not Accessible");
		}

		private string GetUidToLog(IValueObject vo)
		{
			if (vo != null)
			{
				return vo.Uid;
			}
			return "NULL";
		}

		public bool SquadTroopDeployAllowed()
		{
			if (this.AreRaidsAccessible() && this.IsRaidAvailable())
			{
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				RaidVO raidVO = Service.Get<IDataController>().Get<RaidVO>(currentPlayer.CurrentRaidId);
				return raidVO.SquadEnabled;
			}
			return false;
		}

		private void CreateScoutHolo()
		{
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			Entity availableScoutTower = buildingLookupController.GetAvailableScoutTower();
			if (availableScoutTower != null && this.AreRaidsAccessible() && this.IsRaidAvailable())
			{
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				RaidVO raidVO = Service.Get<IDataController>().Get<RaidVO>(currentPlayer.CurrentRaidId);
				this.scoutHolo = new BuildingHoloEffect(availableScoutTower);
				this.scoutHolo.CreateGenericHolo(raidVO.BuildingHoloAssetName, "locator_fx");
			}
		}

		private void DestroyScoutHolo()
		{
			if (this.scoutHolo != null)
			{
				this.scoutHolo.Cleanup();
				this.scoutHolo = null;
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.BuildingReplaced)
			{
				if (id <= EventId.BuildingCancelled)
				{
					if (id != EventId.BuildingViewReady)
					{
						if (id != EventId.BuildingCancelled)
						{
							return EatResponse.NotEaten;
						}
					}
					else
					{
						EntityViewParams entityViewParams = (EntityViewParams)cookie;
						if (entityViewParams.Entity.Has<ScoutTowerComponent>())
						{
							this.CreateScoutHolo();
							return EatResponse.NotEaten;
						}
						return EatResponse.NotEaten;
					}
				}
				else if (id != EventId.EntityKilled)
				{
					if (id != EventId.BuildingConstructed)
					{
						if (id != EventId.BuildingReplaced)
						{
							return EatResponse.NotEaten;
						}
						Entity entity = cookie as Entity;
						if (entity.Has<ScoutTowerComponent>())
						{
							this.CreateScoutHolo();
							return EatResponse.NotEaten;
						}
						return EatResponse.NotEaten;
					}
				}
				else
				{
					SmartEntity smartEntity = (SmartEntity)cookie;
					if (smartEntity.BuildingComp == null)
					{
						return EatResponse.NotEaten;
					}
					BuildingType type = smartEntity.BuildingComp.BuildingType.Type;
					if (!this.raidDefenseTrainerBindings.Contains(type))
					{
						return EatResponse.NotEaten;
					}
					UXController uXController = Service.Get<UXController>();
					Lang lang = Service.Get<Lang>();
					switch (type)
					{
					case BuildingType.FleetCommand:
						Service.Get<DeployerController>().SpecialAttackDeployer.ExitMode();
						uXController.HUD.DisableSpecialAttacks();
						uXController.MiscElementsManager.ShowPlayerInstructions(lang.Get("STARSHIP_TRAINER_DESTROYED", new object[0]));
						return EatResponse.NotEaten;
					case BuildingType.HeroMobilizer:
						Service.Get<DeployerController>().HeroDeployer.ExitMode();
						uXController.HUD.DisableHeroDeploys();
						uXController.MiscElementsManager.ShowPlayerInstructions(lang.Get("HERO_TRAINER_DESTROYED", new object[0]));
						return EatResponse.NotEaten;
					case BuildingType.ChampionPlatform:
					case BuildingType.Housing:
						return EatResponse.NotEaten;
					case BuildingType.Squad:
						Service.Get<DeployerController>().SquadTroopDeployer.ExitMode();
						uXController.HUD.DisableSquadDeploy();
						uXController.MiscElementsManager.ShowPlayerInstructions(lang.Get("SQUAD_CENTER_DESTROYED", new object[0]));
						return EatResponse.NotEaten;
					default:
						return EatResponse.NotEaten;
					}
				}
				ContractEventData contractEventData = (ContractEventData)cookie;
				if (contractEventData.BuildingVO.Type == BuildingType.ScoutTower)
				{
					this.SendRaidDefenseUpdate();
				}
			}
			else
			{
				if (id <= EventId.ContractStarted)
				{
					if (id != EventId.WorldLoadComplete)
					{
						if (id == EventId.WorldReset)
						{
							this.DestroyScoutHolo();
							return EatResponse.NotEaten;
						}
						if (id != EventId.ContractStarted)
						{
							return EatResponse.NotEaten;
						}
					}
					else
					{
						IState currentState = Service.Get<GameStateMachine>().CurrentState;
						if (currentState is HomeState && this.RaidCompleteDidAwardCrate())
						{
							GameUtils.ShowCrateAwardModal(this.lastAwardedCrateUid);
							this.lastAwardedCrateUid = null;
							Service.Get<EventManager>().UnregisterObserver(this, EventId.WorldLoadComplete);
							return EatResponse.NotEaten;
						}
						return EatResponse.NotEaten;
					}
				}
				else if (id != EventId.ContractContinued)
				{
					if (id == EventId.HeroDeployed)
					{
						EntityController entityController = Service.Get<EntityController>();
						NodeList<OffensiveTroopNode> nodeList = entityController.GetNodeList<OffensiveTroopNode>();
						TroopAttackController troopAttackController = Service.Get<TroopAttackController>();
						for (OffensiveTroopNode offensiveTroopNode = nodeList.Head; offensiveTroopNode != null; offensiveTroopNode = offensiveTroopNode.Next)
						{
							troopAttackController.RefreshTarget((SmartEntity)offensiveTroopNode.Entity);
						}
						return EatResponse.NotEaten;
					}
					if (id != EventId.PlanetRelocateStarted)
					{
						return EatResponse.NotEaten;
					}
					if (this.AreRaidsAccessible())
					{
						this.SendRaidDefenseUpdate();
						return EatResponse.NotEaten;
					}
					return EatResponse.NotEaten;
				}
				ContractEventData contractEventData2 = (ContractEventData)cookie;
				if (contractEventData2.BuildingVO.Type == BuildingType.ScoutTower)
				{
					this.DestroyScoutHolo();
				}
			}
			return EatResponse.NotEaten;
		}

		private void RegisterBattleObservers()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.EntityKilled, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.HeroDeployed, EventPriority.AfterDefault);
		}

		private void UnregisterBattleObservers()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.EntityKilled);
			eventManager.UnregisterObserver(this, EventId.HeroDeployed);
		}

		protected internal RaidDefenseController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).AreRaidsAccessible());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).AttemptToShowRaidWaitConfirmation();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).CreateScoutHolo();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).DestroyScoutHolo();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).ActiveRaidColor);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).InactiveColor);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).InactiveTickerColor);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).GetDisplayForTimeLeftInRaid());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).GetDisplayForTimeTillNextRaid());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).GetRaidTimeSeconds());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).GetUidToLog((IValueObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).IsRaidAvailable());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).OnDefendNowScreenClosed(GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).OnEndRaidDefenseMission(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).OnRaidCompleteSuccessWrapper((RaidDefenseCompleteResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).OnRaidStartSuccessWrapper((RaidDefenseStartResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).OnRaidUpdateSuccess((RaidUpdateResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).OnStartRaidDefenseMission();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).OnWaitScreenClosed(GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).RaidCompleteDidAwardCrate());
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).RegisterBattleObservers();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).SendRaidDefenseComplete((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args), (Action)GCHandledObjects.GCHandleToObject(args[1]), (EndPvEBattleTO)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).SendRaidDefenseStart((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args), (RaidDefenseController.OnSuccessCallback)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).SendRaidDefenseUpdate();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).ActiveRaidColor = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).InactiveColor = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).InactiveTickerColor = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).SetupRaidUpdateTimer();
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).ShowRaidInfo();
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).SquadTroopDeployAllowed());
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).StartCurrentRaidDefense();
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).StartCurrentRaidDefenseInternal();
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).UnregisterBattleObservers();
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((RaidDefenseController)GCHandledObjects.GCHandleToObject(instance)).UpdateRaidExpiration();
			return -1L;
		}
	}
}
