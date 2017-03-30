using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;

namespace StaRTS.Main.Controllers.GameStates
{
	public class EditBaseState : IGameState, IState
	{
		private bool autoLiftSelectedBuilding;

		public EditBaseState(bool autoLiftSelectedBuilding)
		{
			this.autoLiftSelectedBuilding = autoLiftSelectedBuilding;
		}

		public void OnEnter()
		{
			Service.Get<EditBaseController>().Enable(true);
			Service.Get<BuildingController>().EnterMoveMode(this.autoLiftSelectedBuilding);
			Service.Get<WorldInitializer>().View.SetEditModeVantage(true);
			Service.Get<WorldInitializer>().View.DrawWorldGrid();
			HudConfig hudConfig = new HudConfig(new string[]
			{
				"PlayerInfo",
				"Currency",
				"Droids",
				"Crystals",
				"ButtonExitEdit",
				"ButtonStore"
			});
			HUD hUD = Service.Get<UXController>().HUD;
			if (!GameConstants.DISABLE_BASE_LAYOUT_TOOL && !hUD.BaseLayoutToolView.IsBuildingPendingPurchase)
			{
				hudConfig.Add("BtnActivateStash");
			}
			hUD.ConfigureControls(hudConfig);
			hUD.BaseLayoutToolView.RefreshNewJewelStatus();
			Service.Get<EventManager>().SendEvent(EventId.EnterEditMode, null);
			Service.Get<ChampionController>().DestroyAllChampionEntities();
		}

		public void OnExit(IState nextState)
		{
			if (!(nextState is BaseLayoutToolState))
			{
				Service.Get<EditBaseController>().Enable(false);
				Service.Get<WorldInitializer>().View.SetEditModeVantage(false);
				Service.Get<WorldInitializer>().View.DestroyWorldGrid();
				Service.Get<BuildingController>().ExitAllModes();
			}
			else
			{
				Service.Get<BuildingController>().EnsureLoweredLiftedBuilding();
			}
			Service.Get<EventManager>().SendEvent(EventId.ExitEditMode, null);
		}

		public bool CanUpdateHomeContracts()
		{
			return true;
		}
	}
}
