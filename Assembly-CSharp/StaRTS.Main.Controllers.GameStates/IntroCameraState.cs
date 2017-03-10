using StaRTS.Externals.GameServices;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Animations;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.GameStates
{
	public class IntroCameraState : IGameState, IState, IEventObserver
	{
		private IntroCameraAnimation animation;

		public void OnEnter()
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.IntroComplete, EventPriority.Default);
			Service.Get<EventManager>().SendEvent(EventId.PurgeHomeStateRUFTask, null);
			this.animation = Service.Get<UXController>().Intro;
			this.animation.Start();
		}

		public void OnExit(IState nextState)
		{
			this.Done();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.IntroComplete)
			{
				this.Done();
			}
			return EatResponse.NotEaten;
		}

		private void Done()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.IntroComplete);
			if (this.animation != null)
			{
				this.animation = null;
				Service.Get<UXController>().Intro = null;
				Service.Get<UXController>().HUD.ConfigureControls(new HudConfig(new string[0]));
				Service.Get<UXController>().HUD.Visible = true;
				if (Service.Get<CurrentPlayer>().HasNotCompletedFirstFueStep() && GameConstants.START_FUE_IN_BATTLE_MODE)
				{
					Service.Get<GameStateMachine>().SetState(new FueBattleStartState(GameConstants.FUE_BATTLE));
					Service.Get<BattleController>().PrepareWorldForBattle();
					return;
				}
				GameServicesManager.OnReady();
				HomeState.GoToHomeState(null, true);
			}
		}

		public bool CanUpdateHomeContracts()
		{
			return true;
		}

		public IntroCameraState()
		{
		}

		protected internal IntroCameraState(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IntroCameraState)GCHandledObjects.GCHandleToObject(instance)).CanUpdateHomeContracts());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IntroCameraState)GCHandledObjects.GCHandleToObject(instance)).Done();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IntroCameraState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IntroCameraState)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((IntroCameraState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
