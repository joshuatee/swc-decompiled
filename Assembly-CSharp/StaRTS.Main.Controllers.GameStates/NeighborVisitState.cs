using StaRTS.Main.Models;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

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

		public NeighborVisitState()
		{
		}

		protected internal NeighborVisitState(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NeighborVisitState)GCHandledObjects.GCHandleToObject(instance)).CanUpdateHomeContracts());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((NeighborVisitState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((NeighborVisitState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
