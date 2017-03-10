using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.GameStates
{
	public class BaseLayoutToolState : IGameState, IState
	{
		private const string STRING_INFO_TITLE = "blt_popup_title";

		private const string STRING_INFO_BODY = "blt_popup_body";

		private const string SKIP_CONFIRMATION = "SKIP_FUTURE_CONFIRMATION";

		public void OnEnter()
		{
			HUDBaseLayoutToolView baseLayoutToolView = Service.Get<UXController>().HUD.BaseLayoutToolView;
			baseLayoutToolView.ConfigureBaseLayoutToolStateHUD();
			BaseLayoutToolController baseLayoutToolController = Service.Get<BaseLayoutToolController>();
			baseLayoutToolController.EnterBaseLayoutTool();
			baseLayoutToolController.PauseContractsOnAllBuildings();
			baseLayoutToolController.UpdateLastSavedMap();
			Service.Get<DroidController>().HideAllNonClearableDroids();
			string pref = Service.Get<SharedPlayerPrefs>().GetPref<string>("SkipBLTIntro");
			if (pref != "1")
			{
				Lang lang = Service.Get<Lang>();
				string title = lang.Get("blt_popup_title", new object[0]);
				string message = lang.Get("blt_popup_body", new object[0]);
				AlertWithCheckBoxScreen screen = new AlertWithCheckBoxScreen(title, message, "SKIP_FUTURE_CONFIRMATION", new AlertWithCheckBoxScreen.OnCheckBoxScreenModalResult(this.OnInfoPopupClosed));
				Service.Get<ScreenController>().AddScreen(screen);
			}
			baseLayoutToolView.RefreshStashModeCheckBox();
			baseLayoutToolView.RefreshSaveLayoutButtonStatus();
			this.SaveBLTSeenSharedPref();
			Service.Get<ChampionController>().DestroyAllChampionEntities();
		}

		public void OnExit(IState nextState)
		{
			BaseLayoutToolController baseLayoutToolController = Service.Get<BaseLayoutToolController>();
			if (baseLayoutToolController.ShouldRevertMap)
			{
				Service.Get<BuildingController>().EnsureLoweredLiftedBuilding();
				baseLayoutToolController.RevertToPreviousMapLayout();
			}
			baseLayoutToolController.ClearStashedBuildings();
			Service.Get<UXController>().HUD.BaseLayoutToolView.ClearStashedBuildingTray();
			baseLayoutToolController.ResumeContractsOnAllBuildings();
			Service.Get<DroidController>().ShowAllDroids();
			Service.Get<EditBaseController>().Enable(false);
			Service.Get<WorldInitializer>().View.SetEditModeVantage(false);
			Service.Get<WorldInitializer>().View.DestroyWorldGrid();
			Service.Get<BuildingController>().ExitAllModes();
			baseLayoutToolController.ExitBaseLayoutTool();
			Service.Get<EventManager>().SendEvent(EventId.ExitBaseLayoutToolMode, null);
		}

		private void OnInfoPopupClosed(object result, bool selected)
		{
			if (result != null && selected)
			{
				Service.Get<SharedPlayerPrefs>().SetPref("SkipBLTIntro", "1");
			}
		}

		private void SaveBLTSeenSharedPref()
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			string pref = sharedPlayerPrefs.GetPref<string>("BLTSeen");
			if (pref != "1")
			{
				sharedPlayerPrefs.SetPref("BLTSeen", "1");
			}
		}

		public bool CanUpdateHomeContracts()
		{
			return true;
		}

		public BaseLayoutToolState()
		{
		}

		protected internal BaseLayoutToolState(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BaseLayoutToolState)GCHandledObjects.GCHandleToObject(instance)).CanUpdateHomeContracts());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BaseLayoutToolState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BaseLayoutToolState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BaseLayoutToolState)GCHandledObjects.GCHandleToObject(instance)).OnInfoPopupClosed(GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((BaseLayoutToolState)GCHandledObjects.GCHandleToObject(instance)).SaveBLTSeenSharedPref();
			return -1L;
		}
	}
}
