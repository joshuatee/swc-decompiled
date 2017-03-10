using StaRTS.Audio;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

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

		public VideoPlayBackState()
		{
		}

		protected internal VideoPlayBackState(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoPlayBackState)GCHandledObjects.GCHandleToObject(instance)).CanUpdateHomeContracts());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VideoPlayBackState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((VideoPlayBackState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
