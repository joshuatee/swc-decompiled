using StaRTS.Externals.GameServices;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Animations;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;

namespace StaRTS.Main.Controllers.GameStates
{
	public class IntroCameraState : IGameState, IEventObserver, IState
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
				}
				else
				{
					GameServicesManager.OnReady();
					HomeState.GoToHomeState(null, true);
				}
			}
		}

		public bool CanUpdateHomeContracts()
		{
			return true;
		}
	}
}
