using StaRTS.Main.Controllers.Planets;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.GameStates
{
	public class GalaxyState : IGameState, IState
	{
		public void OnEnter()
		{
			Service.Get<UXController>().HUD.Visible = false;
			Service.Get<UXController>().HUD.SetSquadScreenAlwaysOnTop(true);
			Service.Get<UXController>().HUD.SetSquadScreenVisibility(true);
			Action callback = new Action(Service.Get<GalaxyViewController>().GoToHome);
			Service.Get<UXController>().MiscElementsManager.ShowGalaxyCloseButton(callback);
			Service.Get<UXController>().MiscElementsManager.AddGalaxyTournamentStatus();
		}

		public void OnExit(IState nextState)
		{
			Service.Get<UXController>().HUD.SetSquadScreenAlwaysOnTop(false);
			Service.Get<UXController>().MiscElementsManager.RemoveGalaxyTournamentStatus();
			if (!(nextState is VideoPlayBackState))
			{
				Service.Get<UXController>().MiscElementsManager.HideGalaxyCloseButton();
				Service.Get<UXController>().HUD.Visible = true;
			}
		}

		public bool CanUpdateHomeContracts()
		{
			return true;
		}

		public GalaxyState()
		{
		}

		protected internal GalaxyState(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyState)GCHandledObjects.GCHandleToObject(instance)).CanUpdateHomeContracts());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GalaxyState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GalaxyState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
