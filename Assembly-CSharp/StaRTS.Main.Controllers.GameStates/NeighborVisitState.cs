using StaRTS.Main.Models;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;

namespace StaRTS.Main.Controllers.GameStates
{
	public class NeighborVisitState : IGameState, IState
	{
		public void OnEnter()
		{
			Service.Get<UXController>().HUD.Visible = true;
			Service.Get<UXController>().HUD.ConfigureControls(new HudConfig(new string[]
			{
				"FriendInfo",
				"ButtonHome"
			}));
			Service.Get<BuildingController>().EnterSelectMode();
		}

		public void OnExit(IState nextState)
		{
		}

		public bool CanUpdateHomeContracts()
		{
			return false;
		}
	}
}
