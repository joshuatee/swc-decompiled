using StaRTS.Main.Controllers.GameStates;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public abstract class AbstractDeployableController
	{
		protected void EnsureBattlePlayState()
		{
			GameStateMachine gameStateMachine = Service.Get<GameStateMachine>();
			if (gameStateMachine.CurrentState is BattleStartState)
			{
				gameStateMachine.SetState(new BattlePlayState());
			}
		}

		protected AbstractDeployableController()
		{
		}

		protected internal AbstractDeployableController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractDeployableController)GCHandledObjects.GCHandleToObject(instance)).EnsureBattlePlayState();
			return -1L;
		}
	}
}
