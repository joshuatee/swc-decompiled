using StaRTS.Externals.BI;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Controllers.World.Transitions;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Commands.Squads;
using StaRTS.Main.Models.Commands.Squads.Responses;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;

namespace StaRTS.Main.Controllers.GameStates
{
	public class WarBaseEditorState : IGameState, IState
	{
		private Map warBaseMap;

		public WarBaseEditorState()
		{
			HudConfig config = new HudConfig(new string[0]);
			HUD hUD = Service.Get<UXController>().HUD;
			hUD.ConfigureControls(config);
		}

		public static void GoToWarBaseEditorState()
		{
			WarBaseEditorState warBaseEditorState = new WarBaseEditorState();
			warBaseEditorState.StartTransitionToWarBase();
		}

		private void StartTransitionToWarBase()
		{
			string warPlanetUid = Service.Get<SquadController>().WarManager.GetCurrentWarScheduleData().WarPlanetUid;
			Service.Get<WorldTransitioner>().StartWipe(new WarboardToWarbaseTransition(null, new UserWarBaseMapDataLoader(), null, false, false), Service.Get<IDataController>().Get<PlanetVO>(warPlanetUid));
			this.QueryBaseData();
		}

		private void QueryBaseData()
		{
			PlayerIdChecksumRequest request = new PlayerIdChecksumRequest();
			GetSquadMemberSyncedWarDataCommand getSquadMemberSyncedWarDataCommand = new GetSquadMemberSyncedWarDataCommand(request);
			getSquadMemberSyncedWarDataCommand.AddSuccessCallback(new AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>.OnSuccessCallback(this.OnWarBaseDataSuccess));
			Service.Get<ServerAPI>().Sync(getSquadMemberSyncedWarDataCommand);
			SquadController squadController = Service.Get<SquadController>();
			string warId = squadController.WarManager.CurrentSquadWar.WarId;
			string squadID = squadController.StateManager.GetCurrentSquad().SquadID;
			Service.Get<BILoggingController>().TrackGameAction(warId, "edit_warbase", squadID, null);
			Service.Get<UXController>().MiscElementsManager.HideEventsTickerView();
		}

		private void SetupBaseEditing()
		{
			Service.Get<EditBaseController>().Enable(true);
			Service.Get<BuildingController>().EnterMoveMode(false);
			Service.Get<WorldInitializer>().View.SetEditModeVantage(true);
			Service.Get<WorldInitializer>().View.DrawWorldGrid();
			Service.Get<EventManager>().SendEvent(EventId.EnterEditMode, null);
			Service.Get<ChampionController>().DestroyAllChampionEntities();
		}

		private void SetupEditingMode()
		{
			this.SetupBaseEditing();
			HUDBaseLayoutToolView baseLayoutToolView = Service.Get<UXController>().HUD.BaseLayoutToolView;
			baseLayoutToolView.ConfigureBaseLayoutToolStateHUD();
			baseLayoutToolView.RegisterObservers();
			WarBaseEditController warBaseEditController = Service.Get<WarBaseEditController>();
			warBaseEditController.EnterWarBaseEditing(this.warBaseMap);
			this.warBaseMap = null;
			Service.Get<UXController>().MiscElementsManager.AddSquadWarTickerStatus();
			BaseLayoutToolController baseLayoutToolController = Service.Get<BaseLayoutToolController>();
			baseLayoutToolController.UpdateLastSavedMap();
			Service.Get<DroidController>().HideAllNonClearableDroids();
			baseLayoutToolView.RefreshStashModeCheckBox();
			baseLayoutToolView.RefreshSaveLayoutButtonStatus();
			Service.Get<ChampionController>().DestroyAllChampionEntities();
			warBaseEditController.CheckForNewBuildings();
		}

		private void OnWarBaseDataSuccess(SquadMemberWarDataResponse response, object cookie)
		{
			this.warBaseMap = response.MemberWarData.BaseMap;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			UserWarBaseMapDataLoader userWarBaseMapDataLoader = new UserWarBaseMapDataLoader();
			userWarBaseMapDataLoader.Initialize(this.warBaseMap, currentPlayer.PlayerName, currentPlayer.Faction);
			Service.Get<WorldTransitioner>().ContinueWipe(this, userWarBaseMapDataLoader, new TransitionCompleteDelegate(this.OnTransitionComplete));
		}

		private void OnTransitionComplete()
		{
			this.SetupEditingMode();
		}

		private void CleanupBaseEditing()
		{
			Service.Get<EditBaseController>().Enable(false);
			Service.Get<WorldInitializer>().View.SetEditModeVantage(false);
			Service.Get<WorldInitializer>().View.DestroyWorldGrid();
			Service.Get<BuildingController>().ExitAllModes();
			Service.Get<EventManager>().SendEvent(EventId.ExitEditMode, null);
		}

		public void OnEnter()
		{
		}

		public void OnExit(IState nextState)
		{
			BaseLayoutToolController baseLayoutToolController = Service.Get<BaseLayoutToolController>();
			baseLayoutToolController.ClearStashedBuildings();
			HUDBaseLayoutToolView baseLayoutToolView = Service.Get<UXController>().HUD.BaseLayoutToolView;
			baseLayoutToolView.ClearStashedBuildingTray();
			baseLayoutToolView.UnregisterObservers();
			WarBaseEditController warBaseEditController = Service.Get<WarBaseEditController>();
			warBaseEditController.ExitWarBaseEditing();
			this.CleanupBaseEditing();
			Service.Get<UXController>().MiscElementsManager.HideEventsTickerView();
		}

		public bool CanUpdateHomeContracts()
		{
			return false;
		}
	}
}
