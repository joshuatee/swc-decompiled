using StaRTS.Main.Controllers.SquadWar;
using StaRTS.Main.Models;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

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

		public WarBoardState()
		{
		}

		protected internal WarBoardState(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardState)GCHandledObjects.GCHandleToObject(instance)).CanUpdateHomeContracts());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WarBoardState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WarBoardState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
