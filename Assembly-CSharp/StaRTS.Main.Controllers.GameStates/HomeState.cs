using StaRTS.Assets;
using StaRTS.Externals.BI;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Controllers.World.Transitions;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.World;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;

namespace StaRTS.Main.Controllers.GameStates
{
	public class HomeState : IGameState, IState
	{
		private TransitionCompleteDelegate pendingCompletion;

		private bool transitionToHomeState;

		public bool ForceReloadMap
		{
			get;
			private set;
		}

		private HomeState()
		{
			this.transitionToHomeState = false;
			this.ForceReloadMap = false;
		}

		public static bool GoToHomeState(TransitionCompleteDelegate onComplete, bool zoom)
		{
			Service.Get<DeployerController>().ExitAllDeployModes();
			Service.Get<PvpManager>().KillTimer();
			if (zoom)
			{
				PlanetView view = Service.Get<WorldInitializer>().View;
				view.ZoomOutImmediate();
				view.ZoomIn();
			}
			HomeState homeState = new HomeState();
			bool result = homeState.Setup(onComplete);
			Service.Get<AssetManager>().UnloadPreloadables();
			return result;
		}

		public static bool GoToHomeStateAndReloadMap()
		{
			return new HomeState
			{
				ForceReloadMap = true
			}.Setup(null);
		}

		private bool Setup(TransitionCompleteDelegate onComplete)
		{
			this.pendingCompletion = onComplete;
			HomeMapDataLoader mapDataLoader = Service.Get<HomeMapDataLoader>();
			GameStateMachine gameStateMachine = Service.Get<GameStateMachine>();
			bool flag = gameStateMachine.CurrentState is WarBoardState;
			this.transitionToHomeState = (!Service.Get<WorldTransitioner>().IsCurrentWorldHome() || flag);
			if (this.transitionToHomeState)
			{
				AbstractTransition transition;
				if (flag)
				{
					transition = new WarboardToBaseTransition(this, mapDataLoader, this.pendingCompletion);
				}
				else
				{
					transition = new WorldToWorldTransition(this, mapDataLoader, this.pendingCompletion, false, true);
				}
				Service.Get<WorldTransitioner>().StartTransition(transition);
				this.pendingCompletion = null;
			}
			else
			{
				gameStateMachine.SetState(this);
			}
			return this.transitionToHomeState;
		}

		public void OnEnter()
		{
			HudConfig config = new HudConfig(new string[]
			{
				"Currency",
				"Droids",
				"Crystals",
				"PlayerInfo",
				"Shield",
				"ButtonBattle",
				"ButtonWar",
				"ButtonLog",
				"ButtonLeaderboard",
				"ButtonSettings",
				"ButtonClans",
				"ButtonStore",
				"Newspaper",
				"SquadScreen",
				"SpecialPromo"
			});
			Service.Get<UXController>().HUD.ConfigureControls(config);
			if (!this.transitionToHomeState)
			{
				if (Service.Get<WorldTransitioner>().IsCurrentWorldHome() && this.ForceReloadMap)
				{
					Service.Get<ProjectileViewManager>().UnloadProjectileAssetsAndPools();
					Service.Get<SpecialAttackController>().UnloadPreloads();
					Service.Get<WorldInitializer>().ProcessMapData(Service.Get<CurrentPlayer>().Map);
					Service.Get<EventManager>().SendEvent(EventId.HomeStateTransitionComplete, null);
				}
				else
				{
					Service.Get<EventManager>().SendEvent(EventId.WorldInTransitionComplete, null);
				}
				if (this.pendingCompletion != null)
				{
					this.pendingCompletion();
					this.pendingCompletion = null;
				}
			}
			Service.Get<BuildingController>().EnterSelectMode();
			Service.Get<DeployerController>().ExitAllDeployModes();
			Service.Get<BILoggingController>().SchedulePerformanceLogging(true);
			Service.Get<InventoryCrateRewardController>().ScheduleGivingNextDailyCrate();
		}

		public void OnExit(IState nextState)
		{
			Service.Get<BuildingController>().ExitAllModes();
			Service.Get<BILoggingController>().UnschedulePerformanceLogging();
			Service.Get<InventoryCrateRewardController>().CancelDailyCrateScheduledTimer();
		}

		public bool CanUpdateHomeContracts()
		{
			return true;
		}
	}
}
