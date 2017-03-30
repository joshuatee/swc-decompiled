using StaRTS.Main.Controllers.SquadWar;
using StaRTS.Main.Models;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;

namespace StaRTS.Main.Controllers.GameStates
{
	public class WarBoardState : IGameState, IState
	{
		public void OnEnter()
		{
			UXController uXController = Service.Get<UXController>();
			uXController.MiscElementsManager.HideEventsTickerView();
			uXController.HUD.ConfigureControls(new HudConfig(new string[]
			{
				"SquadScreen"
			}));
			Service.Get<UXController>().HUD.SetSquadScreenAlwaysOnTop(true);
			ScreenController screenController = Service.Get<ScreenController>();
			screenController.CloseAll();
			screenController.AddScreen(new SquadWarScreen(), false);
			Service.Get<WarBoardViewController>().ShowWarBoard();
		}

		public void OnExit(IState nextState)
		{
			Service.Get<WarBoardViewController>().HideWarBoard();
			Service.Get<UXController>().HUD.SetSquadScreenVisibility(true);
			Service.Get<UXController>().MiscElementsManager.ShowEventsTickerView();
			Service.Get<UXController>().HUD.SetSquadScreenAlwaysOnTop(false);
		}

		public bool CanUpdateHomeContracts()
		{
			return true;
		}
	}
}
