using StaRTS.Audio;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;

namespace StaRTS.Main.Controllers.GameStates
{
	public class VideoPlayBackState : IGameState, IState
	{
		public void OnEnter()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Map map = new Map();
			map.Planet = currentPlayer.Planet;
			Service.Get<WorldInitializer>().PrepareWorld(map);
			Service.Get<AudioManager>().ToggleAllSounds(false);
		}

		public void OnExit(IState nextState)
		{
			Service.Get<AudioManager>().ToggleAllSounds(true);
		}

		public bool CanUpdateHomeContracts()
		{
			return false;
		}
	}
}
