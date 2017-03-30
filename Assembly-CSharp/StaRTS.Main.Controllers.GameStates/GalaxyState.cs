using StaRTS.Main.Controllers.Planets;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;

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
	}
}
