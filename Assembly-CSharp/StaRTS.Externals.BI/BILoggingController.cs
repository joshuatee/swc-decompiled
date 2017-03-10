using StaRTS.Externals.EnvironmentManager;
using StaRTS.Externals.Maker;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Performance;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.BI
{
	public class BILoggingController : IEventObserver
	{
		private const string TWO_VAR_STR = "{0}|{1}";

		private const string THREE_VAR_STR = "{0}|{1}|{2}";

		private const string NO_PLAYER_ID = "NO_PLAYER_ID";

		private uint sampleDelayTimerID;

		private const string REASON_LOW_FPS = "Low FPS";

		private const string REASON_ERROR = "Critical error";

		private const string REASON_WARNING = "Severe warning";

		private DateTime epochDate;

		private MonoBehaviour engine;

		private string locale;

		private IDeviceInfoController deviceInfoController;

		private BIFrameMonitor biFrameMonitor;

		private int pageLoadStepCounter;

		private int iapActionCounter;

		private BILog log;

		private StepTimingController stepTiming;

		private PlaydomLogCreator playdomLogCreator;

		private Event2LogCreator event2LogCreator;

		public BILoggingController()
		{
			this.locale = "";
			base..ctor();
			Service.Set<BILoggingController>(this);
			this.Initialize();
		}

		public void Initialize()
		{
			this.playdomLogCreator = new PlaydomLogCreator("https://n7-starts-client-bi.playdom.com/bi?", "https://stage.api.disney.com/datatech/serverlog/v1/cp?");
			this.event2LogCreator = new Event2LogCreator("http://n7vgd1strtsbil01.general.disney.private/bi_event2_qa", "https://qa.api.disney.com/datatech/log/v1/batch");
			this.engine = Service.Get<Engine>();
			this.epochDate = new DateTime(1970, 1, 1, 0, 0, 0, 1);
			this.log = new BILog();
			this.stepTiming = new StepTimingController();
			this.biFrameMonitor = new BIFrameMonitor();
			EventManager eventManager = Service.Get<EventManager>();
			this.deviceInfoController = new DefaultDeviceInfoController();
			eventManager.RegisterObserver(this, EventId.AssetLoadEnd);
			eventManager.RegisterObserver(this, EventId.AssetLoadStart);
			eventManager.RegisterObserver(this, EventId.BattleLoadEnd);
			eventManager.RegisterObserver(this, EventId.BattleLoadStart);
			eventManager.RegisterObserver(this, EventId.ContractAdded);
			eventManager.RegisterObserver(this, EventId.ContractCanceled);
			eventManager.RegisterObserver(this, EventId.ContractCompleted);
			eventManager.RegisterObserver(this, EventId.FacebookLoggedIn);
			eventManager.RegisterObserver(this, EventId.GameStateChanged);
			eventManager.RegisterObserver(this, EventId.HUDBattleButtonClicked);
			eventManager.RegisterObserver(this, EventId.HUDBattleLogButtonClicked);
			eventManager.RegisterObserver(this, EventId.HUDCrystalButtonClicked);
			eventManager.RegisterObserver(this, EventId.HUDDroidButtonClicked);
			eventManager.RegisterObserver(this, EventId.HUDLeaderboardButtonClicked);
			eventManager.RegisterObserver(this, EventId.HUDHolonetButtonClicked);
			eventManager.RegisterObserver(this, EventId.HUDSettingsButtonClicked);
			eventManager.RegisterObserver(this, EventId.HUDShieldButtonClicked);
			eventManager.RegisterObserver(this, EventId.HUDSquadsButtonClicked);
			eventManager.RegisterObserver(this, EventId.HUDStoreButtonClicked);
			eventManager.RegisterObserver(this, EventId.FactionIconUpgraded);
			eventManager.RegisterObserver(this, EventId.InAppPurchaseSelect);
			eventManager.RegisterObserver(this, EventId.InitialLoadStart);
			eventManager.RegisterObserver(this, EventId.InitializeAudioEnd);
			eventManager.RegisterObserver(this, EventId.InitializeAudioStart);
			eventManager.RegisterObserver(this, EventId.InitializeBoardEnd);
			eventManager.RegisterObserver(this, EventId.InitializeBoardStart);
			eventManager.RegisterObserver(this, EventId.InitializeGameDataStart);
			eventManager.RegisterObserver(this, EventId.InitializeGameDataEnd);
			eventManager.RegisterObserver(this, EventId.InitializeGeneralSystemsEnd);
			eventManager.RegisterObserver(this, EventId.InitializeGeneralSystemsStart);
			eventManager.RegisterObserver(this, EventId.InitializeWorldEnd);
			eventManager.RegisterObserver(this, EventId.InitializeWorldStart);
			eventManager.RegisterObserver(this, EventId.LogStoryActionExecuted);
			eventManager.RegisterObserver(this, EventId.MetaDataLoadEnd);
			eventManager.RegisterObserver(this, EventId.MetaDataLoadStart);
			eventManager.RegisterObserver(this, EventId.PlayerLoginSuccess);
			eventManager.RegisterObserver(this, EventId.PreloadAssetsEnd);
			eventManager.RegisterObserver(this, EventId.PreloadAssetsStart);
			eventManager.RegisterObserver(this, EventId.PvpBattleSkipped);
			eventManager.RegisterObserver(this, EventId.PvpOpponentNotFound);
			eventManager.RegisterObserver(this, EventId.PvpRevengeOpponentNotFound);
			eventManager.RegisterObserver(this, EventId.SettingsAboutButtonClicked);
			eventManager.RegisterObserver(this, EventId.SettingsFacebookLoggedIn);
			eventManager.RegisterObserver(this, EventId.SettingsHelpButtonClicked);
			eventManager.RegisterObserver(this, EventId.SettingsMusicCheckboxSelected);
			eventManager.RegisterObserver(this, EventId.SettingsFanForumsButtonClicked);
			eventManager.RegisterObserver(this, EventId.SettingsSfxCheckboxSelected);
			eventManager.RegisterObserver(this, EventId.ShowOffers);
			eventManager.RegisterObserver(this, EventId.SoftCurrencyPurchaseSelect);
			eventManager.RegisterObserver(this, EventId.StoreCategorySelected);
			eventManager.RegisterObserver(this, EventId.StringsLoadEnd);
			eventManager.RegisterObserver(this, EventId.StringsLoadStart);
			eventManager.RegisterObserver(this, EventId.SquadChatSent);
			eventManager.RegisterObserver(this, EventId.SquadJoinedByCurrentPlayer);
			eventManager.RegisterObserver(this, EventId.SquadJoinApplicationAcceptedByCurrentPlayer);
			eventManager.RegisterObserver(this, EventId.SquadReplaySharedByCurrentPlayer);
			eventManager.RegisterObserver(this, EventId.TapjoyOfferWallSelect);
			eventManager.RegisterObserver(this, EventId.UIAttackScreenSelection);
			eventManager.RegisterObserver(this, EventId.UIConflictStatusClicked);
			eventManager.RegisterObserver(this, EventId.UIIAPDisclaimerClosed);
			eventManager.RegisterObserver(this, EventId.UIIAPDisclaimerViewed);
			eventManager.RegisterObserver(this, EventId.UIFactionFlipAction);
			eventManager.RegisterObserver(this, EventId.UIFactionFlipConfirmAction);
			eventManager.RegisterObserver(this, EventId.UIFactionFlipOpened);
			eventManager.RegisterObserver(this, EventId.UIPvEMissionStart);
			eventManager.RegisterObserver(this, EventId.UILeaderboardExpand);
			eventManager.RegisterObserver(this, EventId.UILeaderboardFriendsTabShown);
			eventManager.RegisterObserver(this, EventId.UILeaderboardInfo);
			eventManager.RegisterObserver(this, EventId.UILeaderboardPlayersTabShown);
			eventManager.RegisterObserver(this, EventId.UILeaderboardTournamentTabShown);
			eventManager.RegisterObserver(this, EventId.UILeaderboardSquadTabShown);
			eventManager.RegisterObserver(this, EventId.UILeaderboardVisit);
			eventManager.RegisterObserver(this, EventId.UINotEnoughDroidBuy);
			eventManager.RegisterObserver(this, EventId.UINotEnoughDroidClose);
			eventManager.RegisterObserver(this, EventId.UINotEnoughDroidSpeedUp);
			eventManager.RegisterObserver(this, EventId.UINotEnoughHardCurrencyBuy);
			eventManager.RegisterObserver(this, EventId.UINotEnoughHardCurrencyClose);
			eventManager.RegisterObserver(this, EventId.UINotEnoughSoftCurrencyBuy);
			eventManager.RegisterObserver(this, EventId.UINotEnoughSoftCurrencyClose);
			eventManager.RegisterObserver(this, EventId.UIPvESelection);
			eventManager.RegisterObserver(this, EventId.UISquadJoinTabShown);
			eventManager.RegisterObserver(this, EventId.InventoryCrateTapped);
			eventManager.RegisterObserver(this, EventId.InventoryCrateStoreOpen);
			eventManager.RegisterObserver(this, EventId.InventoryCrateOpened);
			eventManager.RegisterObserver(this, EventId.HUDInventoryScreenOpened);
			eventManager.RegisterObserver(this, EventId.LootTableButtonTapped);
			eventManager.RegisterObserver(this, EventId.LootTableUnitInfoTapped);
			eventManager.RegisterObserver(this, EventId.LootTableRelocateTapped);
			eventManager.RegisterObserver(this, EventId.UnitInfoGoToGalaxy);
			eventManager.RegisterObserver(this, EventId.UISquadScreenTabShown);
			eventManager.RegisterObserver(this, EventId.UITournamentEndSelection);
			eventManager.RegisterObserver(this, EventId.UITournamentTierSelection);
			eventManager.RegisterObserver(this, EventId.UserIsIdle);
			eventManager.RegisterObserver(this, EventId.UILeaderboardJoin);
			eventManager.RegisterObserver(this, EventId.VisitPlayer);
			eventManager.RegisterObserver(this, EventId.WorldLoadComplete);
			eventManager.RegisterObserver(this, EventId.GalaxyOpenByContextButton);
			eventManager.RegisterObserver(this, EventId.GalaxyOpenByInfoScreen);
			eventManager.RegisterObserver(this, EventId.GalaxyOpenByPlayScreen);
			eventManager.RegisterObserver(this, EventId.GalaxyScreenClosed);
			eventManager.RegisterObserver(this, EventId.GalaxyPlanetTapped);
			eventManager.RegisterObserver(this, EventId.GalaxyPlanetInfoButton);
			eventManager.RegisterObserver(this, EventId.PlanetRelocateButtonPressed);
			eventManager.RegisterObserver(this, EventId.HolonetCommandCenterTab);
			eventManager.RegisterObserver(this, EventId.HolonetCommandCenterFeature);
			eventManager.RegisterObserver(this, EventId.HolonetVideoTab);
			eventManager.RegisterObserver(this, EventId.HolonetDevNotes);
			eventManager.RegisterObserver(this, EventId.HolonetTransmissionLog);
			eventManager.RegisterObserver(this, EventId.HolonetIncomingTransmission);
			eventManager.RegisterObserver(this, EventId.HolonetClosed);
			eventManager.RegisterObserver(this, EventId.HolonetOpened);
			eventManager.RegisterObserver(this, EventId.HolonetTabClosed);
			eventManager.RegisterObserver(this, EventId.HolonetTabOpened);
			eventManager.RegisterObserver(this, EventId.ObjectiveLockedCrateClicked);
			eventManager.RegisterObserver(this, EventId.ObjectiveDetailsClicked);
			eventManager.RegisterObserver(this, EventId.UISquadJoinScreenShown);
			eventManager.RegisterObserver(this, EventId.UISquadLeaveConfirmation);
			eventManager.RegisterObserver(this, EventId.CrateStoreOpen);
			eventManager.RegisterObserver(this, EventId.CrateStoreCancel);
			eventManager.RegisterObserver(this, EventId.CrateStorePurchase);
			eventManager.RegisterObserver(this, EventId.CrateStoreNotEnoughCurrency);
		}

		public void Destroy()
		{
			if (this.log != null)
			{
				this.log.Reset();
				this.log = null;
			}
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.AssetLoadEnd);
			eventManager.UnregisterObserver(this, EventId.AssetLoadStart);
			eventManager.UnregisterObserver(this, EventId.BattleLoadEnd);
			eventManager.UnregisterObserver(this, EventId.BattleLoadStart);
			eventManager.UnregisterObserver(this, EventId.ContractAdded);
			eventManager.UnregisterObserver(this, EventId.ContractCanceled);
			eventManager.UnregisterObserver(this, EventId.ContractCompleted);
			eventManager.UnregisterObserver(this, EventId.FacebookLoggedIn);
			eventManager.UnregisterObserver(this, EventId.GameStateChanged);
			eventManager.UnregisterObserver(this, EventId.HUDBattleButtonClicked);
			eventManager.UnregisterObserver(this, EventId.HUDBattleLogButtonClicked);
			eventManager.UnregisterObserver(this, EventId.HUDCrystalButtonClicked);
			eventManager.UnregisterObserver(this, EventId.HUDDroidButtonClicked);
			eventManager.UnregisterObserver(this, EventId.HUDLeaderboardButtonClicked);
			eventManager.UnregisterObserver(this, EventId.HUDHolonetButtonClicked);
			eventManager.UnregisterObserver(this, EventId.HUDSettingsButtonClicked);
			eventManager.UnregisterObserver(this, EventId.HUDShieldButtonClicked);
			eventManager.UnregisterObserver(this, EventId.HUDSquadsButtonClicked);
			eventManager.UnregisterObserver(this, EventId.HUDStoreButtonClicked);
			eventManager.UnregisterObserver(this, EventId.FactionIconUpgraded);
			eventManager.UnregisterObserver(this, EventId.InAppPurchaseSelect);
			eventManager.UnregisterObserver(this, EventId.InitialLoadStart);
			eventManager.UnregisterObserver(this, EventId.InitializeAudioEnd);
			eventManager.UnregisterObserver(this, EventId.InitializeAudioStart);
			eventManager.UnregisterObserver(this, EventId.InitializeBoardEnd);
			eventManager.UnregisterObserver(this, EventId.InitializeBoardStart);
			eventManager.UnregisterObserver(this, EventId.InitializeGameDataStart);
			eventManager.UnregisterObserver(this, EventId.InitializeGameDataEnd);
			eventManager.UnregisterObserver(this, EventId.InitializeGeneralSystemsEnd);
			eventManager.UnregisterObserver(this, EventId.InitializeGeneralSystemsStart);
			eventManager.UnregisterObserver(this, EventId.InitializeWorldEnd);
			eventManager.UnregisterObserver(this, EventId.InitializeWorldStart);
			eventManager.UnregisterObserver(this, EventId.LogStoryActionExecuted);
			eventManager.UnregisterObserver(this, EventId.MetaDataLoadEnd);
			eventManager.UnregisterObserver(this, EventId.MetaDataLoadStart);
			eventManager.UnregisterObserver(this, EventId.PlayerLoginSuccess);
			eventManager.UnregisterObserver(this, EventId.PreloadAssetsEnd);
			eventManager.UnregisterObserver(this, EventId.PreloadAssetsStart);
			eventManager.UnregisterObserver(this, EventId.PvpBattleSkipped);
			eventManager.UnregisterObserver(this, EventId.PvpOpponentNotFound);
			eventManager.UnregisterObserver(this, EventId.PvpRevengeOpponentNotFound);
			eventManager.UnregisterObserver(this, EventId.SettingsAboutButtonClicked);
			eventManager.UnregisterObserver(this, EventId.SettingsFacebookLoggedIn);
			eventManager.UnregisterObserver(this, EventId.SettingsHelpButtonClicked);
			eventManager.UnregisterObserver(this, EventId.SettingsMusicCheckboxSelected);
			eventManager.UnregisterObserver(this, EventId.SettingsFanForumsButtonClicked);
			eventManager.UnregisterObserver(this, EventId.SettingsSfxCheckboxSelected);
			eventManager.UnregisterObserver(this, EventId.ShowOffers);
			eventManager.UnregisterObserver(this, EventId.SoftCurrencyPurchaseSelect);
			eventManager.UnregisterObserver(this, EventId.StoreCategorySelected);
			eventManager.UnregisterObserver(this, EventId.StringsLoadEnd);
			eventManager.UnregisterObserver(this, EventId.StringsLoadStart);
			eventManager.UnregisterObserver(this, EventId.SquadChatSent);
			eventManager.UnregisterObserver(this, EventId.SquadJoinedByCurrentPlayer);
			eventManager.UnregisterObserver(this, EventId.SquadJoinApplicationAcceptedByCurrentPlayer);
			eventManager.UnregisterObserver(this, EventId.SquadReplaySharedByCurrentPlayer);
			eventManager.UnregisterObserver(this, EventId.TapjoyOfferWallSelect);
			eventManager.UnregisterObserver(this, EventId.UIAttackScreenSelection);
			eventManager.UnregisterObserver(this, EventId.UIConflictStatusClicked);
			eventManager.UnregisterObserver(this, EventId.UIIAPDisclaimerClosed);
			eventManager.UnregisterObserver(this, EventId.UIIAPDisclaimerViewed);
			eventManager.UnregisterObserver(this, EventId.UIFactionFlipAction);
			eventManager.UnregisterObserver(this, EventId.UIFactionFlipConfirmAction);
			eventManager.UnregisterObserver(this, EventId.UIFactionFlipOpened);
			eventManager.UnregisterObserver(this, EventId.UIPvEMissionStart);
			eventManager.UnregisterObserver(this, EventId.UILeaderboardExpand);
			eventManager.UnregisterObserver(this, EventId.UILeaderboardFriendsTabShown);
			eventManager.UnregisterObserver(this, EventId.UILeaderboardInfo);
			eventManager.UnregisterObserver(this, EventId.UILeaderboardPlayersTabShown);
			eventManager.UnregisterObserver(this, EventId.UILeaderboardTournamentTabShown);
			eventManager.UnregisterObserver(this, EventId.UILeaderboardSquadTabShown);
			eventManager.UnregisterObserver(this, EventId.UILeaderboardVisit);
			eventManager.UnregisterObserver(this, EventId.UINotEnoughDroidBuy);
			eventManager.UnregisterObserver(this, EventId.UINotEnoughDroidClose);
			eventManager.UnregisterObserver(this, EventId.UINotEnoughDroidSpeedUp);
			eventManager.UnregisterObserver(this, EventId.UINotEnoughHardCurrencyBuy);
			eventManager.UnregisterObserver(this, EventId.UINotEnoughHardCurrencyClose);
			eventManager.UnregisterObserver(this, EventId.UINotEnoughSoftCurrencyBuy);
			eventManager.UnregisterObserver(this, EventId.UINotEnoughSoftCurrencyClose);
			eventManager.UnregisterObserver(this, EventId.UIPvESelection);
			eventManager.UnregisterObserver(this, EventId.UISquadJoinTabShown);
			eventManager.UnregisterObserver(this, EventId.InventoryCrateTapped);
			eventManager.UnregisterObserver(this, EventId.InventoryCrateStoreOpen);
			eventManager.UnregisterObserver(this, EventId.InventoryCrateOpened);
			eventManager.UnregisterObserver(this, EventId.HUDInventoryScreenOpened);
			eventManager.UnregisterObserver(this, EventId.LootTableButtonTapped);
			eventManager.UnregisterObserver(this, EventId.LootTableUnitInfoTapped);
			eventManager.UnregisterObserver(this, EventId.LootTableRelocateTapped);
			eventManager.UnregisterObserver(this, EventId.UnitInfoGoToGalaxy);
			eventManager.UnregisterObserver(this, EventId.UISquadScreenTabShown);
			eventManager.UnregisterObserver(this, EventId.UITournamentEndSelection);
			eventManager.UnregisterObserver(this, EventId.UITournamentTierSelection);
			eventManager.UnregisterObserver(this, EventId.UserIsIdle);
			eventManager.UnregisterObserver(this, EventId.UILeaderboardJoin);
			eventManager.UnregisterObserver(this, EventId.VisitPlayer);
			eventManager.UnregisterObserver(this, EventId.WorldLoadComplete);
			eventManager.UnregisterObserver(this, EventId.GalaxyOpenByContextButton);
			eventManager.UnregisterObserver(this, EventId.GalaxyOpenByInfoScreen);
			eventManager.UnregisterObserver(this, EventId.GalaxyOpenByPlayScreen);
			eventManager.UnregisterObserver(this, EventId.GalaxyScreenClosed);
			eventManager.UnregisterObserver(this, EventId.GalaxyPlanetTapped);
			eventManager.UnregisterObserver(this, EventId.GalaxyPlanetInfoButton);
			eventManager.UnregisterObserver(this, EventId.PlanetRelocateButtonPressed);
			eventManager.UnregisterObserver(this, EventId.HolonetCommandCenterTab);
			eventManager.UnregisterObserver(this, EventId.HolonetCommandCenterFeature);
			eventManager.UnregisterObserver(this, EventId.HolonetVideoTab);
			eventManager.UnregisterObserver(this, EventId.HolonetDevNotes);
			eventManager.UnregisterObserver(this, EventId.HolonetTransmissionLog);
			eventManager.UnregisterObserver(this, EventId.HolonetIncomingTransmission);
			eventManager.UnregisterObserver(this, EventId.HolonetClosed);
			eventManager.UnregisterObserver(this, EventId.HolonetOpened);
			eventManager.UnregisterObserver(this, EventId.HolonetTabClosed);
			eventManager.UnregisterObserver(this, EventId.HolonetTabOpened);
			eventManager.UnregisterObserver(this, EventId.ObjectiveLockedCrateClicked);
			eventManager.UnregisterObserver(this, EventId.ObjectiveDetailsClicked);
			eventManager.UnregisterObserver(this, EventId.UISquadJoinScreenShown);
			eventManager.UnregisterObserver(this, EventId.UISquadLeaveConfirmation);
			eventManager.UnregisterObserver(this, EventId.CrateStoreOpen);
			eventManager.UnregisterObserver(this, EventId.CrateStoreCancel);
			eventManager.UnregisterObserver(this, EventId.CrateStorePurchase);
			eventManager.UnregisterObserver(this, EventId.CrateStoreNotEnoughCurrency);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.ContractCanceled)
			{
				if (id <= EventId.GameStateChanged)
				{
					if (id != EventId.StoreCategorySelected)
					{
						switch (id)
						{
						case EventId.SquadChatSent:
							this.TrackGameAction("squad_action", "chat", "", this.GetSquadID(), 1);
							break;
						case EventId.SquadDetailsUpdated:
						case EventId.SquadUpdateCompleted:
						case EventId.SquadEdited:
						case EventId.SquadSelect:
						case EventId.SquadSend:
						case EventId.SquadNext:
						case EventId.SquadMore:
						case EventId.SquadFB:
						case EventId.SquadCredits:
						case EventId.TroopViewReady:
						case EventId.TroopLevelUpgraded:
						case EventId.StarshipLevelUpgraded:
						case EventId.BuildingLevelUpgraded:
						case EventId.BuildingSwapped:
						case EventId.BuildingConstructed:
						case EventId.BuildingReplaced:
						case EventId.SpecialAttackSpawned:
						case EventId.SpecialAttackDeployed:
						case EventId.SpecialAttackDropshipFlyingAway:
						case EventId.SpecialAttackFired:
						case EventId.HudComplete:
						case EventId.IntroStarted:
						case EventId.IntroComplete:
						case EventId.InventoryCapacityChanged:
						case EventId.MapDataProcessingStart:
						case EventId.MapDataProcessingEnd:
						case EventId.WorldInTransitionComplete:
						case EventId.WorldOutTransitionComplete:
						case EventId.WorldReset:
						case EventId.HomeStateTransitionComplete:
						case EventId.PlayedLoadedOnDemandAudio:
						case EventId.PreloadedAudioSuccess:
						case EventId.PreloadedAudioFailure:
						case EventId.SuccessfullyResumed:
						case EventId.AllUXElementsCreated:
						case EventId.ElementDestroyed:
						case EventId.UIFilterSelected:
						case EventId.EnterEditMode:
						case EventId.ExitEditMode:
						case EventId.ExitBaseLayoutToolMode:
							break;
						case EventId.UISquadJoinTabShown:
							this.TrackGameAction("UI_squad", cookie as string, this.GetMemberOrNonMember(), "", 1);
							break;
						case EventId.UISquadJoinScreenShown:
							this.TrackGameAction("UI_squad", "featured_access", (cookie as string) + "|" + this.GetMemberOrNonMember(), "", 1);
							break;
						case EventId.UISquadLeaveConfirmation:
							this.TrackGameAction("squad_action", "leave_prompt", cookie as string, "", 1);
							break;
						case EventId.UISquadScreenTabShown:
							this.TrackGameAction("UI_squad", cookie as string, this.GetMemberOrNonMember(), "", 1);
							break;
						case EventId.UILeaderboardSquadTabShown:
							this.TrackGameAction("UI_leaderboard", "squads", cookie as string, "", 1);
							break;
						case EventId.UILeaderboardFriendsTabShown:
							this.TrackGameAction("UI_leaderboard", "friends", cookie as string, "", 1);
							break;
						case EventId.UILeaderboardPlayersTabShown:
							this.TrackGameAction("UI_leaderboard", "players", cookie as string, "", 1);
							break;
						case EventId.UILeaderboardTournamentTabShown:
							this.TrackGameAction("UI_leaderboard", "tournament", cookie as string, "", 1);
							break;
						case EventId.UILeaderboardExpand:
							this.TrackGameAction("UI_leaderboard", "expand", cookie as string, "", 1);
							break;
						case EventId.UILeaderboardVisit:
							this.TrackGameAction("UI_leaderboard_expand", "visit", cookie as string, "", 1);
							break;
						case EventId.UILeaderboardInfo:
							this.TrackGameAction("UI_leaderboard_expand", "info", cookie as string, "", 1);
							break;
						case EventId.UILeaderboardJoin:
							this.TrackGameAction("UI_leaderboard_expand", "join", cookie as string, "", 1);
							break;
						case EventId.VisitPlayer:
						{
							PlayerVisitTag playerVisitTag = cookie as PlayerVisitTag;
							this.TrackPlayerVisit(playerVisitTag.IsSquadMate, playerVisitTag.IsFriend, playerVisitTag.TabName, playerVisitTag.PlayerId);
							break;
						}
						case EventId.WorldLoadComplete:
						{
							IState currentState = Service.Get<GameStateMachine>().CurrentState;
							if (currentState is ApplicationLoadState)
							{
								this.TrackStepTiming("page_load", "end", "default", StepTimingType.End);
								this.TrackUserInfo();
								this.TrackPlayerInfo();
								this.TrackFaction();
							}
							break;
						}
						case EventId.InitialLoadStart:
							this.TrackStepTiming("page_load", "start", "default", StepTimingType.Start);
							break;
						case EventId.MetaDataLoadStart:
							this.TrackStepTiming("page_load", "metadata_start", "default", StepTimingType.Intermediary);
							break;
						case EventId.MetaDataLoadEnd:
							this.TrackStepTiming("page_load", "metadata_end", "default", StepTimingType.Intermediary);
							break;
						case EventId.AssetLoadStart:
							this.TrackStepTiming("page_load", "assetload_start", "default", StepTimingType.Intermediary);
							break;
						case EventId.AssetLoadEnd:
							this.TrackStepTiming("page_load", "assetload_end", "default", StepTimingType.Intermediary);
							break;
						case EventId.StringsLoadStart:
							this.TrackStepTiming("page_load", "string_data_start", "default", StepTimingType.Intermediary);
							break;
						case EventId.StringsLoadEnd:
							this.TrackStepTiming("page_load", "string_data_end", "default", StepTimingType.Intermediary);
							break;
						case EventId.PreloadAssetsStart:
							this.TrackStepTiming("page_load", "preload_assets_start", "default", StepTimingType.Intermediary);
							break;
						case EventId.PreloadAssetsEnd:
							this.TrackStepTiming("page_load", "preload_assets_end", "default", StepTimingType.Intermediary);
							break;
						case EventId.InitializeGameDataStart:
							this.TrackStepTiming("page_load", "init_gamedata_start", "default", StepTimingType.Intermediary);
							break;
						case EventId.InitializeGameDataEnd:
							this.TrackStepTiming("page_load", "init_gamedata_end", "default", StepTimingType.Intermediary);
							break;
						case EventId.InitializeAudioStart:
							this.TrackStepTiming("page_load", "init_audio_start", "default", StepTimingType.Intermediary);
							break;
						case EventId.InitializeAudioEnd:
							this.TrackStepTiming("page_load", "init_audio_end", "default", StepTimingType.Intermediary);
							break;
						case EventId.InitializeBoardStart:
							this.TrackStepTiming("page_load", "init_board_start", "default", StepTimingType.Intermediary);
							break;
						case EventId.InitializeBoardEnd:
							this.TrackStepTiming("page_load", "init_board_end", "default", StepTimingType.Intermediary);
							break;
						case EventId.InitializeGeneralSystemsStart:
							this.TrackStepTiming("page_load", "init_general_sys_start", "default", StepTimingType.Intermediary);
							break;
						case EventId.InitializeGeneralSystemsEnd:
							this.TrackStepTiming("page_load", "init_general_sys_end", "default", StepTimingType.Intermediary);
							break;
						case EventId.InitializeWorldStart:
							this.TrackStepTiming("page_load", "init_world_start", "default", StepTimingType.Intermediary);
							break;
						case EventId.InitializeWorldEnd:
							this.TrackStepTiming("page_load", "init_world_end", "default", StepTimingType.Intermediary);
							break;
						case EventId.UINotEnoughSoftCurrencyBuy:
							this.TrackGameAction("UI_money_flow", "buy", "not_enough_soft_currency", "", 1);
							break;
						case EventId.UINotEnoughSoftCurrencyClose:
							this.TrackGameAction("UI_money_flow", "close", "not_enough_soft_currency", "", 1);
							break;
						case EventId.UINotEnoughHardCurrencyBuy:
							this.TrackGameAction("UI_money_flow", "buy", "not_enough_hard_currency", "", 1);
							break;
						case EventId.UINotEnoughHardCurrencyClose:
							this.TrackGameAction("UI_money_flow", "close", "not_enough_hard_currency", "", 1);
							break;
						case EventId.UINotEnoughDroidBuy:
							this.TrackGameAction("UI_money_flow", "buy", "all_droids_busy", "", 1);
							break;
						case EventId.UINotEnoughDroidClose:
							this.TrackGameAction("UI_money_flow", "close", "all_droids_busy", "", 1);
							break;
						case EventId.UINotEnoughDroidSpeedUp:
							this.TrackGameAction("UI_money_flow", "speed_up", "all_droids_busy", "", 1);
							break;
						case EventId.BattleLoadStart:
							this.TrackBattleLoadStepTiming(StepTimingType.Start);
							break;
						case EventId.BattleLoadEnd:
							this.TrackBattleLoadStepTiming(StepTimingType.End);
							break;
						default:
							switch (id)
							{
							case EventId.PvpOpponentNotFound:
								this.TrackGameAction("UI_PvP", "no_opponent_found", cookie as string, "", 0);
								break;
							case EventId.PvpRevengeOpponentNotFound:
								this.TrackGameAction("UI_Revenge", "no_opponent_found", cookie as string, "", 0);
								break;
							case EventId.PvpBattleSkipped:
								this.TrackPvpGameAction("skip");
								break;
							case EventId.GameStateChanged:
								this.HandleGameStateChanged();
								break;
							}
							break;
						}
					}
					else
					{
						this.HandleStoreCategorySelection((StoreTab)cookie);
					}
				}
				else if (id != EventId.ContractAdded)
				{
					if (id == EventId.ContractCompleted || id == EventId.ContractCanceled)
					{
						this.TrackBuildingContractStepTiming(StepTimingType.End, cookie as ContractEventData);
					}
				}
				else
				{
					this.TrackBuildingContractStepTiming(StepTimingType.Start, cookie as ContractEventData);
				}
			}
			else if (id <= EventId.SettingsSfxCheckboxSelected)
			{
				switch (id)
				{
				case EventId.UIAttackScreenSelection:
				{
					ActionMessageBIData actionMessageBIData = (ActionMessageBIData)cookie;
					this.TrackGameAction("UI_attack", actionMessageBIData.Action, actionMessageBIData.Message, "", 1);
					break;
				}
				case EventId.UISquadWarScreen:
				{
					ActionMessageBIData actionMessageBIData2 = (ActionMessageBIData)cookie;
					this.TrackGameAction("UI_squadwar_attack", actionMessageBIData2.Action, actionMessageBIData2.Message, "");
					break;
				}
				case EventId.UIConflictStatusClicked:
				{
					ActionMessageBIData actionMessageBIData3 = (ActionMessageBIData)cookie;
					this.TrackGameAction("UI_conflict_ticker", actionMessageBIData3.Action, actionMessageBIData3.Message, "", 1);
					break;
				}
				case EventId.UIPvESelection:
				{
					ActionMessageBIData actionMessageBIData4 = (ActionMessageBIData)cookie;
					this.TrackGameAction("UI_PvE_mission", actionMessageBIData4.Action, actionMessageBIData4.Message, "", 1);
					break;
				}
				case EventId.UIPvEMissionStart:
					this.TrackGameAction("PvE", "start", cookie as string, "", 1);
					break;
				case EventId.UITournamentTierSelection:
				{
					ActionMessageBIData actionMessageBIData5 = (ActionMessageBIData)cookie;
					this.TrackGameAction("UI_tournament_tiers", actionMessageBIData5.Action, actionMessageBIData5.Message, "", 1);
					break;
				}
				case EventId.UITournamentEndSelection:
				{
					ActionMessageBIData actionMessageBIData6 = (ActionMessageBIData)cookie;
					this.TrackGameAction("UI_tournament_end", actionMessageBIData6.Action, actionMessageBIData6.Message, "", 1);
					break;
				}
				case EventId.ObjectivesUpdated:
				case EventId.UpdateObjectiveToastData:
				case EventId.ShowObjectiveToast:
				case EventId.ClaimObjectiveFailed:
				case EventId.HoloEvent:
				case EventId.StoryTranscriptDisplayed:
				case EventId.HolocommScreenLoadComplete:
				case EventId.HoloCommScreenDestroyed:
				case EventId.StoryNextButtonClicked:
				case EventId.StoryAttackButtonClicked:
				case EventId.StorySkipButtonClicked:
				case EventId.StoryChainCompleted:
					break;
				case EventId.ShowOffers:
					this.TrackGameAction("UI_show_offers", cookie as string, "", "");
					break;
				case EventId.TapjoyOfferWallSelect:
					this.TrackGameAction("UI_choose_offer", "tapjoy", "", "");
					this.TrackGameAction("UI_shop_treasure", cookie as string, "", "", 1);
					break;
				case EventId.InAppPurchaseSelect:
				case EventId.SoftCurrencyPurchaseSelect:
					this.TrackGameAction("UI_shop_treasure", cookie as string, "", "", 1);
					break;
				case EventId.UserIsIdle:
					this.TrackGameAction("UI_idlepop", "idlepop", "", "");
					break;
				case EventId.LogStoryActionExecuted:
					this.HandleStoryAction(cookie as StoryActionVO);
					break;
				default:
					if (id != EventId.PlayerLoginSuccess)
					{
						switch (id)
						{
						case EventId.HUDBattleButtonClicked:
							this.TrackGameAction("UI_HUD", "attack", null, null, 1);
							break;
						case EventId.HUDBattleLogButtonClicked:
							this.TrackGameAction("UI_HUD", "battle_log", null, null, 1);
							break;
						case EventId.HUDCrystalButtonClicked:
							this.TrackGameAction("UI_HUD", "add_crystals", null, null, 1);
							break;
						case EventId.HUDDroidButtonClicked:
							this.TrackGameAction("UI_HUD", "add_droid", null, null, 1);
							break;
						case EventId.HUDLeaderboardButtonClicked:
							this.TrackGameAction("UI_HUD", "leaderboard", null, null, 1);
							break;
						case EventId.HUDHolonetButtonClicked:
							this.TrackGameAction("UI_HUD", "holonet", null, null, 1);
							break;
						case EventId.HUDSettingsButtonClicked:
							this.TrackGameAction("UI_HUD", "settings", null, null, 1);
							break;
						case EventId.HUDShieldButtonClicked:
							this.TrackGameAction("UI_HUD", "damage_protection", null, null, 1);
							break;
						case EventId.HUDSquadsButtonClicked:
							this.TrackGameAction("UI_HUD", "squad", null, null, 1);
							break;
						case EventId.HUDStoreButtonClicked:
							this.TrackGameAction("UI_HUD", "shop", null, null, 1);
							break;
						case EventId.FactionIconUpgraded:
							this.TrackGameAction("faction_icon", "icon_level", null, null, 1);
							break;
						case EventId.GalaxyOpenByInfoScreen:
							this.TrackGameAction("UI_galaxy_map", "open", "info_screen", "", 1);
							break;
						case EventId.GalaxyOpenByContextButton:
							this.TrackGameAction("UI_galaxy_map", "open", "context_button", "", 1);
							break;
						case EventId.GalaxyOpenByPlayScreen:
							this.TrackGameAction("UI_galaxy_map", "open", "play_screen", "", 1);
							break;
						case EventId.GalaxyScreenClosed:
							this.TrackGameAction("UI_galaxy_map", "close", "", "", 1);
							break;
						case EventId.GalaxyPlanetTapped:
							this.TrackGameAction("UI_galaxy_map", "planet", cookie as string, "", 1);
							break;
						case EventId.GalaxyPlanetInfoButton:
							this.TrackGameAction("UI_attack", "info", cookie as string, "", 1);
							break;
						case EventId.FacebookLoggedIn:
							this.TrackGameAction("facebook_connect", "allow", cookie as string, "");
							break;
						case EventId.SettingsAboutButtonClicked:
							this.TrackGameAction("UI_settings", "about", null, null, 1);
							break;
						case EventId.SettingsFacebookLoggedIn:
							this.HandleSettingsScreenFacebookLogin((bool)cookie);
							break;
						case EventId.SettingsHelpButtonClicked:
							this.TrackGameAction("UI_settings", "help", null, null, 1);
							break;
						case EventId.SettingsMusicCheckboxSelected:
							this.HandleSettingsScreenMusicSetting((bool)cookie);
							break;
						case EventId.SettingsFanForumsButtonClicked:
							this.TrackGameAction("UI_settings", "fan_forums", null, null, 1);
							break;
						case EventId.SettingsSfxCheckboxSelected:
							this.HandleSettingsScreenSfxSetting((bool)cookie);
							break;
						}
					}
					else
					{
						this.TrackLogin();
						this.TrackDeviceInfo();
						this.TrackGeo();
					}
					break;
				}
			}
			else if (id <= EventId.SquadReplaySharedByCurrentPlayer)
			{
				switch (id)
				{
				case EventId.UIIAPDisclaimerClosed:
					this.TrackGameAction("UI_IAP_disclaimer", "close", "", "", 0);
					break;
				case EventId.UIIAPDisclaimerViewed:
					this.TrackGameAction("UI_IAP_disclaimer", "view", "", "", 0);
					break;
				case EventId.UIFactionFlipAction:
					this.TrackGameAction("UI_faction_flip", "flip", cookie as string, "", 1);
					break;
				case EventId.UIFactionFlipConfirmAction:
					this.TrackGameAction("UI_faction_flip", "confirmation", cookie as string, "", 1);
					break;
				case EventId.UIFactionFlipOpened:
					this.TrackGameAction("UI_faction_flip", "menu", cookie as string, "", 1);
					break;
				case EventId.TrapTriggered:
				case EventId.TrapDisarmed:
				case EventId.TrapDestroyed:
					break;
				case EventId.PlanetRelocateButtonPressed:
					this.TrackGameAction("UI_attack", "relocate", cookie as string, "", 1);
					break;
				default:
					switch (id)
					{
					case EventId.HolonetCommandCenterTab:
						this.TrackGameAction("UI_holonet", "command_center", cookie as string, null, 1);
						break;
					case EventId.HolonetCommandCenterFeature:
						this.TrackGameAction("UI_holonet", "command_center", cookie as string, null, 1);
						break;
					case EventId.HolonetVideoTab:
						this.TrackGameAction("UI_holonet", "video", cookie as string, null, 1);
						break;
					case EventId.HolonetDevNotes:
						this.TrackGameAction("UI_holonet", "dev_notes", cookie as string, null, 1);
						break;
					case EventId.HolonetTransmissionLog:
						this.TrackGameAction("UI_holonet", "transmission_log", null, null, 1);
						break;
					case EventId.HolonetIncomingTransmission:
						this.TrackGameAction("UI_holonet", "incoming_transmission", cookie as string, null, 1);
						break;
					case EventId.HolonetOpened:
						this.TrackStepTiming("holonet", "start", "holonet", StepTimingType.Intermediary);
						break;
					case EventId.HolonetClosed:
						this.TrackGameAction("UI_holonet", "close", cookie as string, null, 1);
						this.TrackStepTiming("holonet", "end", "holonet", StepTimingType.Intermediary);
						break;
					case EventId.HolonetTabOpened:
						this.TrackStepTiming("holonet_tab", "start", cookie as string, StepTimingType.Intermediary);
						break;
					case EventId.HolonetTabClosed:
						this.TrackStepTiming("holonet_tab", "end", cookie as string, StepTimingType.Intermediary);
						break;
					case EventId.ObjectiveLockedCrateClicked:
						this.TrackGameAction("UI_objectives", "locked_crate", cookie as string, null, 1);
						break;
					case EventId.ObjectiveDetailsClicked:
						this.TrackGameAction("UI_objectives", "objective_details", cookie as string, null, 1);
						break;
					case EventId.SquadJoinedByCurrentPlayer:
						this.TrackSquadSocialGameAction("squad_membership_social", "join", cookie as string, true);
						break;
					case EventId.SquadJoinApplicationAcceptedByCurrentPlayer:
						this.TrackSquadSocialGameAction("squad_action", "join_accept", cookie as string, false);
						break;
					case EventId.SquadReplaySharedByCurrentPlayer:
					{
						SqmReplayData sqmReplayData = (SqmReplayData)cookie;
						string action = (sqmReplayData.BattleType == SquadBattleReplayType.Defense) ? "defense" : "attack";
						this.TrackGameAction("share_replay", action, "", sqmReplayData.BattleId, 1);
						break;
					}
					}
					break;
				}
			}
			else
			{
				switch (id)
				{
				case EventId.CrateStoreOpen:
					this.TrackGameAction("UI_shop_treasure", cookie as string, "open", "", 1);
					break;
				case EventId.CrateStoreCancel:
					this.TrackGameAction("UI_shop_treasure", cookie as string, "cancel", "", 1);
					break;
				case EventId.CrateStorePurchase:
					this.TrackGameAction("UI_shop_treasure", cookie as string, "purchase", "", 1);
					break;
				case EventId.CrateStoreNotEnoughCurrency:
					this.TrackGameAction("UI_shop_treasure", cookie as string, "not_enough_currency", "", 1);
					break;
				default:
					switch (id)
					{
					case EventId.HUDInventoryScreenOpened:
						this.TrackGameAction("UI_crate_inventory", "inventory_tap", "", "", 1);
						break;
					case EventId.InventoryCrateTapped:
					{
						string message = Convert.ToString(cookie);
						this.TrackGameAction("UI_crate_inventory", "crate_tap", message, "", 1);
						break;
					}
					case EventId.InventoryCrateOpened:
					{
						string message2 = Convert.ToString(cookie);
						this.TrackGameAction("UI_crate_inventory", "crate_open", message2, "", 1);
						break;
					}
					case EventId.InventoryCrateStoreOpen:
					{
						string message3 = Convert.ToString(cookie);
						this.TrackGameAction("UI_crate_inventory", "crate_store", message3, "", 1);
						break;
					}
					case EventId.LootTableButtonTapped:
						this.TrackGameAction("UI_prize_table", "prize_table_tap", "", "", 1);
						break;
					case EventId.LootTableUnitInfoTapped:
					{
						string message4 = Convert.ToString(cookie);
						this.TrackGameAction("UI_prize_table", "unit_info_tap", message4, "", 1);
						break;
					}
					case EventId.LootTableRelocateTapped:
					{
						CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
						string planetId = currentPlayer.PlanetId;
						int rawRelocationStarsCount = currentPlayer.GetRawRelocationStarsCount();
						string text = Convert.ToString(cookie);
						string text2 = currentPlayer.IsRelocationRequirementMet().ToString();
						StringBuilder stringBuilder = new StringBuilder(rawRelocationStarsCount.ToString());
						stringBuilder.Append("|");
						stringBuilder.Append(text2);
						stringBuilder.Append("|");
						stringBuilder.Append(text);
						stringBuilder.Append("|");
						stringBuilder.Append(planetId);
						this.TrackGameAction("UI_prize_table", "relocate", stringBuilder.ToString(), "", 1);
						break;
					}
					case EventId.UnitInfoGoToGalaxy:
					{
						string message5 = Convert.ToString(cookie);
						this.TrackGameAction("UI_unit_info", "galaxy_map", message5, "", 1);
						break;
					}
					}
					break;
				}
			}
			return EatResponse.NotEaten;
		}

		public void SchedulePerformanceLogging(bool home)
		{
			ViewTimerManager viewTimerManager = Service.Get<ViewTimerManager>();
			if (this.sampleDelayTimerID != 0u)
			{
				viewTimerManager.KillViewTimer(this.sampleDelayTimerID);
			}
			this.sampleDelayTimerID = viewTimerManager.CreateViewTimer((float)(home ? GameConstants.PERFORMANCE_SAMPLE_DELAY_HOME : GameConstants.PERFORMANCE_SAMPLE_DELAY_BATTLE), false, new TimerDelegate(this.PerformanceLogCallback), 0);
		}

		public void UnschedulePerformanceLogging()
		{
			if (this.sampleDelayTimerID != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.sampleDelayTimerID);
				this.sampleDelayTimerID = 0u;
			}
		}

		private void PerformanceLogCallback(uint id, object cookie)
		{
			this.sampleDelayTimerID = 0u;
			this.biFrameMonitor.LogFPS();
		}

		public void SetBIUrl(string url, string secondaryUrl, string event2Url, string event2SecondaryUrl)
		{
			this.playdomLogCreator.SetURL(url, secondaryUrl);
			this.event2LogCreator.SetURL(event2Url, event2SecondaryUrl);
		}

		private void AddCommonParameters(BILog biLog)
		{
			biLog.AddParam("app", "qa_starts");
			biLog.AddParam("c_app_version", "4.7.0.2");
			biLog.AddParam("app_locale", this.GetLangLocale());
			biLog.AddParam("network", this.GetNetwork());
			biLog.AddParam("view_network", this.GetViewNetwork());
			biLog.AddParam("user_id", this.GetPlayerId());
			biLog.AddParam("c_server", Service.Get<AppServerEnvironmentController>().Server);
		}

		private void AddLocaleParameter(BILog biLog)
		{
			biLog.AddParam("locale", this.GetDeviceLocale());
		}

		private void AddLangParameter(BILog biLog)
		{
			biLog.AddParam("lang", this.GetLangLocale());
		}

		private void StartCallBICoroutine(BILog biLog)
		{
			BILogData playdomLogData = null;
			BILogData event2LogData = null;
			if (GameConstants.PLAYDOM_BI_ENABLED)
			{
				playdomLogData = this.playdomLogCreator.CreateWWWDataFromBILog(biLog);
			}
			if (GameConstants.EVENT_2_BI_ENABLED)
			{
				event2LogData = this.event2LogCreator.CreateWWWDataFromBILog(biLog);
			}
			this.engine.StartCoroutine(this.CallBI(playdomLogData, event2LogData));
		}

		public void TrackAuthorization(string step, string type)
		{
			this.log.Reset();
			this.AddCommonParameters(this.log);
			this.log.AddParam("tag", "authorization");
			this.log.AddParam("step", step);
			this.log.AddParam("type", type);
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackLogin()
		{
			this.log.Reset();
			this.log.AddParam("tag", "clicked_link");
			this.AddCommonParameters(this.log);
			this.AddLangParameter(this.log);
			this.AddLocaleParameter(this.log);
			this.log.AddParam("is_new_user", this.IsNewUser());
			this.log.AddParam("tracking_code", "mobile");
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
			this.log.AddParam("tag", "clicked_link");
			this.AddCommonParameters(this.log);
			this.AddLangParameter(this.log);
			this.AddLocaleParameter(this.log);
			this.log.AddParam("app", "click_track");
			this.log.AddParam("is_new_user", this.IsNewUser());
			this.log.AddParam("log_app", "qa_starts");
			this.log.AddParam("tracking_code", "mobile");
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackUserInfo()
		{
			this.log.Reset();
			this.log.AddParam("tag", "user_info");
			this.AddCommonParameters(this.log);
			this.AddLangParameter(this.log);
			this.log.AddParam("device_id", this.deviceInfoController.GetDeviceId());
			this.log.AddParam("device_type", this.GetDeviceType());
			this.log.AddParam("os_version", Service.Get<EnvironmentController>().GetOSVersion());
			this.log.AddParam("level", this.GetHQLevel());
			if (Service.IsSet<ISocialDataController>() && Service.Get<ISocialDataController>().HaveSelfData)
			{
				this.log.AddParam("gender", Service.Get<ISocialDataController>().Gender);
			}
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackNetworkMappingInfo(string network, string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				return;
			}
			this.log.Reset();
			this.AddCommonParameters(this.log);
			this.log.AddParam("tag", "network_mapping_info");
			this.log.AddParam("secondary_user_id", userId);
			this.log.AddParam("secondary_network", network);
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackPlayerInfo()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			CampaignController campaignController = Service.Get<CampaignController>();
			this.log.Reset();
			this.log.AddParam("tag", "player_info");
			this.AddCommonParameters(this.log);
			this.log.AddParam("level", this.GetHQLevel());
			this.log.AddParam("credit_balance", currentPlayer.CurrentCreditsAmount.ToString());
			this.log.AddParam("alloy_balance", currentPlayer.CurrentMaterialsAmount.ToString());
			this.log.AddParam("crystal_balance", currentPlayer.CurrentCrystalsAmount.ToString());
			this.log.AddParam("stars_earned", campaignController.GetTotalStarsEarned().ToString());
			this.log.AddParam("droids_available", currentPlayer.CurrentDroidsAmount.ToString());
			this.log.AddParam("faction", currentPlayer.Faction.ToString());
			this.log.AddParam("squad_name", this.GetSquadName());
			this.log.AddParam("squad_id", this.GetSquadID());
			this.log.AddParam("clearable_units", Service.Get<BuildingLookupController>().GetNumberOfClearables().ToString());
			this.log.AddParam("trophy_balance", "5");
			this.log.AddParam("furthest_mission_complete", "-1");
			this.log.AddParam("shield_timer", "-1");
			this.log.AddParam("lifetime_spend", "-1");
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackFaction()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (currentPlayer.Faction == FactionType.Empire || currentPlayer.Faction == FactionType.Rebel)
			{
				this.TrackGameAction("faction_choice", currentPlayer.Faction.ToString(), null, null, 1);
			}
		}

		public void TrackDeviceInfo()
		{
			this.log.Reset();
			this.log.AddParam("tag", "device_info");
			this.AddCommonParameters(this.log);
			this.AddLangParameter(this.log);
			this.log.AddParam("machine", this.GetDeviceType());
			this.log.AddParam("model", this.GetDeviceModel());
			this.log.AddParam("os_version", Service.Get<EnvironmentController>().GetOSVersion());
			this.log.AddParam("timestamp", this.GetTimeStampInMilliseconds());
			this.deviceInfoController.AddDeviceSpecificInfo(this.log);
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackIAPGameAction(string context, string action, string message)
		{
			int num = this.iapActionCounter;
			this.iapActionCounter = num + 1;
			this.TrackGameAction(context, action, message, string.Concat(num));
		}

		private void TrackSquadSocialGameAction(string context, string action, string source, bool addFriendIds)
		{
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			int num = 0;
			string text = "";
			List<string> friendIdsInSquad = SquadUtils.GetFriendIdsInSquad(currentSquad.SquadID, Service.Get<LeaderboardController>());
			if (friendIdsInSquad != null)
			{
				num = friendIdsInSquad.Count;
				if (addFriendIds)
				{
					text = SquadUtils.GetFriendIdsString(friendIdsInSquad);
				}
			}
			string text2 = "lower";
			if (SquadUtils.IsPlayerMedalCountHigherThanSquadAvg(currentSquad, Service.Get<CurrentPlayer>().PlayerMedals))
			{
				text2 = "higher";
			}
			string message = string.Format("{0}|{1}|{2}", new object[]
			{
				num.ToString(),
				text2,
				source.ToLower()
			});
			string otherData = addFriendIds ? string.Format("{0}|{1}", new object[]
			{
				currentSquad.SquadID,
				text
			}) : currentSquad.SquadID;
			this.TrackGameAction(context, action, message, otherData, 1);
		}

		public void TrackGameAction(string context, string action, string message, string otherData)
		{
			this.TrackGameAction(context, action, message, otherData, 0);
		}

		private void AddGameActionParams(string context, string action, string message, string otherData, int engaged)
		{
			this.log.AddParam("tag", "game_action");
			this.AddCommonParameters(this.log);
			this.AddLangParameter(this.log);
			this.log.AddParam("context", context);
			this.log.AddParam("action", action);
			this.log.AddParam("engaged", engaged.ToString());
			if (!string.IsNullOrEmpty(message))
			{
				this.log.AddParam("message", message);
			}
			if (!string.IsNullOrEmpty(otherData))
			{
				this.log.AddParam("other_key", otherData);
			}
		}

		public void TrackGameAction(string context, string action, string message, string otherData, int engaged)
		{
			this.log.Reset();
			this.AddGameActionParams(context, action, message, otherData, engaged);
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackStepTiming(string context, string location, string pathName, StepTimingType type)
		{
			string location2 = location;
			if (context.Equals("page_load"))
			{
				if (type != StepTimingType.Start && type != StepTimingType.End)
				{
					location2 = this.GetPageLoadStepCounter() + location;
				}
				this.pageLoadStepCounter++;
				if (type == StepTimingType.End)
				{
					this.pageLoadStepCounter = 1;
				}
			}
			this.TrackStepTiming(context, location2, pathName, type, 0);
		}

		public void TrackStepTiming(string context, string location, string pathName, StepTimingType type, int engaged)
		{
			this.log.Reset();
			this.log.AddParam("tag", "step_timing");
			this.AddCommonParameters(this.log);
			this.log.AddParam("context", context);
			this.log.AddParam("location", location);
			this.log.AddParam("path_name", pathName);
			this.log.AddParam("timestamp_ms", this.GetTimeStampInSeconds());
			this.log.AddParam("engaged", engaged.ToString());
			switch (type)
			{
			case StepTimingType.Start:
				this.stepTiming.StartStep(context);
				break;
			case StepTimingType.Intermediary:
				this.stepTiming.IntermediaryStep(context, this.log);
				break;
			case StepTimingType.End:
				this.stepTiming.EndStep(context, this.log);
				break;
			}
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackStepTiming(string context, string location, string pathName, int timeElapsed)
		{
			this.log.Reset();
			this.log.AddParam("tag", "step_timing");
			this.AddCommonParameters(this.log);
			this.log.AddParam("context", context);
			this.log.AddParam("location", location);
			this.log.AddParam("path_name", pathName);
			this.log.AddParam("timestamp_ms", this.GetTimeStampInSeconds());
			this.log.AddParam("elapsed_time_ms", timeElapsed.ToString());
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackGeo()
		{
			this.log.Reset();
			this.log.UseSecondaryUrl = true;
			this.log.AddParam("tag", "geo");
			this.AddCommonParameters(this.log);
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackPerformance(float fps, float memoryUsed)
		{
			this.log.Reset();
			double num = Math.Round((double)fps, 2);
			double num2 = Math.Round((double)memoryUsed, 2);
			string value = "battle";
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is HomeState)
			{
				value = "home";
			}
			this.log.AddParam("tag", "performance");
			this.AddCommonParameters(this.log);
			this.log.AddParam("time_since_start", DateUtils.GetRealTimeSinceStartUpInMilliseconds().ToString());
			this.log.AddParam("fps", num.ToString());
			this.log.AddParam("memory_used", num2.ToString());
			this.log.AddParam("display_state", value);
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackLowFPS(string errorMessage)
		{
			this.TrackError("Low FPS", errorMessage);
		}

		public void TrackError(LogLevel logLevel, string errorMessage)
		{
			this.TrackError((logLevel == LogLevel.Error) ? "Critical error" : "Severe warning", errorMessage);
		}

		private void TrackError(string reason, string message)
		{
			this.log.Reset();
			this.log.AddParam("tag", "error");
			this.AddCommonParameters(this.log);
			this.log.AddParam("reason", reason);
			string text = Service.Get<GameStateMachine>().CurrentState.ToString();
			text = text.Substring(text.LastIndexOf(".") + 1);
			this.log.AddParam("context", "Client: " + text);
			this.log.AddParam("message", message);
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackSendMessage(string trackingCode, string recipientIds, int numRecipients)
		{
			this.log.Reset();
			this.AddCommonParameters(this.log);
			this.log.AddParam("tag", "send_message");
			this.log.AddParam("tracking_code", trackingCode);
			this.log.AddParam("send_timestamp", this.GetTimeStampInSeconds());
			this.log.AddParam("target_user_id", recipientIds);
			this.log.AddParam("num_sent", numRecipients.ToString());
			this.StartCallBICoroutine(this.log);
			this.log.Reset();
		}

		public void TrackVideoSharing(VideoSharingNetwork network, string source, string videoGuid)
		{
			string context;
			if (network == VideoSharingNetwork.SQUAD)
			{
				context = "video_squadshare";
			}
			else
			{
				context = "video_socialshare";
			}
			this.TrackGameAction(context, source, videoGuid, null);
		}

		public void TrackAssetBundleCacheClean(int version, bool isSuccess)
		{
			string message = isSuccess ? "success" : "failure";
			this.TrackGameAction("asset_bundle_cache_clean", version.ToString(), message, null);
		}

		[IteratorStateMachine(typeof(BILoggingController.<CallBI>d__53))]
		private IEnumerator CallBI(BILogData playdomLogData, BILogData event2LogData)
		{
			if (playdomLogData != null)
			{
				WWW wWW = new WWW(playdomLogData.url);
				WWWManager.Add(wWW);
				yield return wWW;
				if (WWWManager.Remove(wWW))
				{
					wWW.Dispose();
				}
				wWW = null;
			}
			if (event2LogData != null)
			{
				WWW wWW2 = new WWW(event2LogData.url, event2LogData.postData, event2LogData.headers);
				WWWManager.Add(wWW2);
				yield return wWW2;
				if (WWWManager.Remove(wWW2))
				{
					wWW2.Dispose();
				}
				wWW2 = null;
			}
			yield break;
		}

		private void HandleStoryAction(StoryActionVO vo)
		{
			string logType = vo.LogType;
			if (logType == "start")
			{
				this.TrackStepTiming(vo.LogTag, "start", vo.LogPath, StepTimingType.Start, 1);
				return;
			}
			if (!(logType == "end"))
			{
				if (!this.stepTiming.IsStepStarted(vo.LogTag))
				{
					this.TrackStepTiming(vo.LogTag, "start", vo.LogPath, StepTimingType.Start, 1);
				}
				this.TrackStepTiming(vo.LogTag, vo.Uid, vo.LogPath, StepTimingType.Intermediary, 1);
				return;
			}
			this.TrackStepTiming(vo.LogTag, "end", vo.LogPath, StepTimingType.End, 1);
			this.TrackGameAction(vo.LogTag, vo.LogPath, "", vo.Uid, 1);
		}

		private void TrackBuildingContractStepTiming(StepTimingType type, ContractEventData contractData)
		{
			if (contractData.SendBILog)
			{
				StringBuilder stringBuilder = new StringBuilder();
				int timeElapsed = 0;
				stringBuilder.Append(StringUtils.ToLowerCaseUnderscoreSeperated(contractData.BuildingVO.Type.ToString()));
				string location = "";
				if (type == StepTimingType.Start)
				{
					location = "start";
				}
				else if (type == StepTimingType.End)
				{
					location = "end";
					timeElapsed = (contractData.Contract.TotalTime - contractData.Contract.GetRemainingTimeForSim()) * 1000;
				}
				switch (contractData.Contract.DeliveryType)
				{
				case DeliveryType.Building:
				case DeliveryType.UpgradeBuilding:
					if (!string.IsNullOrEmpty(contractData.BuildingVO.TurretUid))
					{
						stringBuilder.Append("_upgrade");
					}
					stringBuilder.Append("|");
					stringBuilder.Append(contractData.BuildingVO.BuildingID);
					stringBuilder.Append("|");
					stringBuilder.Append(Service.Get<IDataController>().Get<BuildingTypeVO>(contractData.Contract.ProductUid).Lvl);
					break;
				case DeliveryType.SwapBuilding:
					stringBuilder.Append("_cross");
					stringBuilder.Append("|");
					stringBuilder.Append(contractData.BuildingVO.BuildingID);
					stringBuilder.Append("|");
					stringBuilder.Append(contractData.BuildingVO.Lvl);
					break;
				case DeliveryType.ClearClearable:
					stringBuilder.Append("|");
					stringBuilder.Append(contractData.BuildingVO.Uid);
					stringBuilder.Append("|");
					stringBuilder.Append(contractData.BuildingVO.SizeX * contractData.BuildingVO.SizeY);
					break;
				}
				switch (contractData.Contract.DeliveryType)
				{
				case DeliveryType.Building:
				case DeliveryType.UpgradeBuilding:
				case DeliveryType.SwapBuilding:
				case DeliveryType.ClearClearable:
					this.TrackStepTiming("droid", location, stringBuilder.ToString(), timeElapsed);
					break;
				case DeliveryType.UpgradeTroop:
				case DeliveryType.UpgradeStarship:
				case DeliveryType.UpgradeEquipment:
					break;
				default:
					return;
				}
			}
		}

		private void HandleStoreCategorySelection(StoreTab tab)
		{
			string text = null;
			switch (tab)
			{
			case StoreTab.Treasure:
				text = "treasure";
				break;
			case StoreTab.Protection:
				text = "protection";
				break;
			case StoreTab.Resources:
				text = "resources";
				break;
			case StoreTab.Army:
				text = "army";
				break;
			case StoreTab.Defenses:
				text = "defenses";
				break;
			case StoreTab.Decorations:
				text = "turrets";
				break;
			}
			if (text != null)
			{
				this.TrackGameAction("UI_shop", text, null, null, 1);
			}
		}

		private void HandleSettingsScreenFacebookLogin(bool loggedIn)
		{
			string action = "fb_connect";
			string action2 = "allow";
			if (!loggedIn)
			{
				action = "fb_disconnect";
				action2 = "disallow";
			}
			this.TrackGameAction("UI_settings", action, null, null, 1);
			this.TrackGameAction("facebook_connect", action2, "UI_settings", "");
		}

		private void HandleSettingsScreenMusicSetting(bool musicEnabled)
		{
			string action = "music_on";
			if (!musicEnabled)
			{
				action = "music_off";
			}
			this.TrackGameAction("UI_settings", action, null, null, 1);
		}

		private void HandleSettingsScreenSfxSetting(bool sfxEnabled)
		{
			string action = "sfx_on";
			if (!sfxEnabled)
			{
				action = "sfx_off";
			}
			this.TrackGameAction("UI_settings", action, null, null, 1);
		}

		private void TrackPlayerVisit(bool isSquadMate, bool isFriend, string message, string playerId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (isSquadMate)
			{
				stringBuilder.Append("squadmate");
			}
			else
			{
				stringBuilder.Append("not_squadmate");
			}
			stringBuilder.Append("|");
			if (isFriend)
			{
				stringBuilder.Append("friend");
			}
			else
			{
				stringBuilder.Append("not_friend");
			}
			stringBuilder.Append("|");
			stringBuilder.Append(this.GetHQLevel());
			this.TrackGameAction("visit_player", stringBuilder.ToString(), message, playerId, 1);
		}

		private void TrackBattleLoadStepTiming(StepTimingType type)
		{
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			BattleType type2 = currentBattle.Type;
			string context = "";
			switch (type2)
			{
			case BattleType.Pvp:
				context = "load_PvP";
				break;
			case BattleType.PveDefend:
			case BattleType.PveAttack:
			case BattleType.PveFue:
			case BattleType.PveBuffBase:
			case BattleType.PvpAttackSquadWar:
				if (currentBattle.IsSpecOps())
				{
					context = "load_campaign";
				}
				else
				{
					context = "load_PvE";
				}
				break;
			}
			this.TrackBattleStepTiming(type, context);
		}

		private void HandleGameStateChanged()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			BattleType type = currentBattle.Type;
			string context = "";
			if (currentState is BattlePlayState || currentState is BattleEndState)
			{
				switch (type)
				{
				case BattleType.Pvp:
					context = "PvP";
					break;
				case BattleType.PveDefend:
				case BattleType.PveAttack:
				case BattleType.PveFue:
				case BattleType.PveBuffBase:
				case BattleType.PvpAttackSquadWar:
					if (currentBattle.IsSpecOps())
					{
						context = "campaign";
					}
					else
					{
						context = "PvE";
					}
					break;
				}
				if (currentState is BattlePlayState)
				{
					this.TrackBattleStepTiming(StepTimingType.Start, context);
					return;
				}
				if (currentState is BattleEndState)
				{
					this.TrackBattleStepTiming(StepTimingType.End, context);
				}
			}
		}

		private void TrackBattleStepTiming(StepTimingType type, string context)
		{
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			BattleType type2 = currentBattle.Type;
			string text = "";
			string location = "";
			if (type == StepTimingType.Start)
			{
				location = "start";
			}
			else if (type == StepTimingType.End)
			{
				location = "end";
			}
			switch (type2)
			{
			case BattleType.Pvp:
				text = currentBattle.AttackerID;
				if (text == Service.Get<CurrentPlayer>().PlayerId)
				{
					text = currentBattle.DefenderID;
				}
				break;
			case BattleType.PveDefend:
			case BattleType.PveAttack:
			case BattleType.PveFue:
			case BattleType.PveBuffBase:
			case BattleType.PvpAttackSquadWar:
			{
				StringBuilder stringBuilder = new StringBuilder();
				string missionId = currentBattle.MissionId;
				if (!string.IsNullOrEmpty(missionId))
				{
					CampaignMissionVO mission = this.GetMission(missionId);
					stringBuilder.Append(mission.BIChapterId);
					stringBuilder.Append("|");
					stringBuilder.Append(mission.BIMissionId);
					stringBuilder.Append("|");
					stringBuilder.Append(mission.BIMissionName);
					stringBuilder.Append("|");
					stringBuilder.Append(mission.Uid);
					text = stringBuilder.ToString();
				}
				else
				{
					stringBuilder.Append("null|null|null|null");
					text = stringBuilder.ToString();
				}
				break;
			}
			}
			this.TrackStepTiming(context, location, text, type, 1);
		}

		private void TrackPvpGameAction(string action)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			string recordID = currentBattle.RecordID;
			string text = (currentBattle.Defender != null) ? currentBattle.Defender.PlayerId : "";
			int defenderBaseScore = currentBattle.DefenderBaseScore;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(currentPlayer.CurrentXPAmount);
			stringBuilder.Append("|");
			stringBuilder.Append(defenderBaseScore);
			stringBuilder.Append("|");
			stringBuilder.Append(this.GetHQLevel());
			stringBuilder.Append("|");
			stringBuilder.Append(text);
			stringBuilder.Append("|");
			stringBuilder.Append("|");
			stringBuilder.Append("|");
			stringBuilder.Append(currentPlayer.Planet.PlanetBIName);
			this.TrackGameAction("PvP", action, stringBuilder.ToString(), recordID, 1);
		}

		private CampaignMissionVO GetMission(string uid)
		{
			IDataController dataController = Service.Get<IDataController>();
			return dataController.Get<CampaignMissionVO>(uid);
		}

		private string GetPageLoadStepCounter()
		{
			StringBuilder stringBuilder = new StringBuilder("load_");
			if (this.pageLoadStepCounter < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(this.pageLoadStepCounter);
			stringBuilder.Append("_");
			return stringBuilder.ToString();
		}

		private string GetPlayerId()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (currentPlayer == null || currentPlayer.PlayerId == null)
			{
				return "NO_PLAYER_ID";
			}
			return currentPlayer.PlayerId;
		}

		private string GetHQLevel()
		{
			if (Service.IsSet<BuildingLookupController>())
			{
				return Service.Get<BuildingLookupController>().GetHighestLevelHQ().ToString();
			}
			return "0";
		}

		private string GetSquadName()
		{
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad != null && !string.IsNullOrEmpty(currentSquad.SquadName))
			{
				return currentSquad.SquadName;
			}
			return "";
		}

		private string GetSquadID()
		{
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad != null && !string.IsNullOrEmpty(currentSquad.SquadID))
			{
				return currentSquad.SquadID;
			}
			return "";
		}

		private string GetNetwork()
		{
			return "f";
		}

		private string GetViewNetwork()
		{
			return "f";
		}

		private string GetDeviceType()
		{
			return Service.Get<EnvironmentController>().GetMachine();
		}

		private string GetDeviceModel()
		{
			return Service.Get<EnvironmentController>().GetModel();
		}

		private string GetTimeStampInSeconds()
		{
			DateTime utcNow = DateTime.get_UtcNow();
			return ((int)(utcNow - this.epochDate).get_TotalSeconds()).ToString();
		}

		private string GetTimeStampInMilliseconds()
		{
			DateTime utcNow = DateTime.get_UtcNow();
			return ((long)(utcNow - this.epochDate).get_TotalMilliseconds()).ToString();
		}

		private string IsNewUser()
		{
			if (Service.Get<CurrentPlayer>().FirstTimePlayer)
			{
				return "1";
			}
			return "0";
		}

		private string GetDeviceLocale()
		{
			if (string.IsNullOrEmpty(this.locale))
			{
				this.locale = Service.Get<EnvironmentController>().GetLocale();
			}
			return this.locale;
		}

		private string GetMemberOrNonMember()
		{
			if (Service.Get<CurrentPlayer>().Squad != null)
			{
				return "member";
			}
			return "nonmember";
		}

		private string GetLangLocale()
		{
			return Service.Get<Lang>().Locale;
		}

		protected internal BILoggingController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).AddCommonParameters((BILog)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).AddGameActionParams(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), *(int*)(args + 4));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).AddLangParameter((BILog)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).AddLocaleParameter((BILog)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).CallBI((BILogData)GCHandledObjects.GCHandleToObject(*args), (BILogData)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetDeviceLocale());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetDeviceModel());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetDeviceType());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetHQLevel());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetLangLocale());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetMemberOrNonMember());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetMission(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetNetwork());
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetPageLoadStepCounter());
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetPlayerId());
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetSquadID());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetSquadName());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetTimeStampInMilliseconds());
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetTimeStampInSeconds());
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).GetViewNetwork());
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).HandleGameStateChanged();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).HandleSettingsScreenFacebookLogin(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).HandleSettingsScreenMusicSetting(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).HandleSettingsScreenSfxSetting(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).HandleStoreCategorySelection((StoreTab)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).HandleStoryAction((StoryActionVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).Initialize();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).IsNewUser());
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).SchedulePerformanceLogging(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).SetBIUrl(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).StartCallBICoroutine((BILog)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackAssetBundleCacheClean(*(int*)args, *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackAuthorization(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackBattleLoadStepTiming((StepTimingType)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackBattleStepTiming((StepTimingType)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackBuildingContractStepTiming((StepTimingType)(*(int*)args), (ContractEventData)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackDeviceInfo();
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackError((LogLevel)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackError(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackFaction();
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackGameAction(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackGameAction(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), *(int*)(args + 4));
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackGeo();
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackIAPGameAction(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackLogin();
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackLowFPS(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackNetworkMappingInfo(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackPerformance(*(float*)args, *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackPlayerInfo();
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackPlayerVisit(*(sbyte*)args != 0, *(sbyte*)(args + 1) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackPvpGameAction(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackSendMessage(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackSquadSocialGameAction(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackStepTiming(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), (StepTimingType)(*(int*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackStepTiming(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), *(int*)(args + 3));
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackStepTiming(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), (StepTimingType)(*(int*)(args + 3)), *(int*)(args + 4));
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackUserInfo();
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).TrackVideoSharing((VideoSharingNetwork)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((BILoggingController)GCHandledObjects.GCHandleToObject(instance)).UnschedulePerformanceLogging();
			return -1L;
		}
	}
}
