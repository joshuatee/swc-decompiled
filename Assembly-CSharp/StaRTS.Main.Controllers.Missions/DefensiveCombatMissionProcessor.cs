using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;

namespace StaRTS.Main.Controllers.Missions
{
	public class DefensiveCombatMissionProcessor : AbstractMissionProcessor, IEventObserver
	{
		private const float GOAL_DELAY = 0.5f;

		private const float GOAL_DURATION = 4f;

		private const float GOAL_FADE = 0.5f;

		public DefensiveCombatMissionProcessor(MissionConductor parent) : base(parent)
		{
		}

		public override void Start()
		{
			PlanetDetailsScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<PlanetDetailsScreen>();
			if (highestLevelScreen != null)
			{
				highestLevelScreen.Close(null);
			}
			if (Service.Get<GameStateMachine>().CurrentState is GalaxyState)
			{
				Service.Get<GalaxyViewController>().GoToHome();
			}
			BattleInitializationData data = BattleInitializationData.CreateFromDefensiveCampaignMissionVO(this.parent.MissionVO);
			BattleStartState.GoToBattleStartState(data, null);
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.BattleEndFullyProcessed, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.GameStateChanged, EventPriority.Default);
			if (this.parent.OnIntroHook())
			{
				base.PauseBattle();
			}
			else
			{
				this.StartMission();
			}
		}

		public override void Resume()
		{
			this.Start();
		}

		public override void OnIntroHookComplete()
		{
			base.ResumeBattle();
			this.StartMission();
		}

		private void StartMission()
		{
			Service.Get<DefensiveBattleController>().StartDefenseMission(this.parent.MissionVO);
			Service.Get<ViewTimerManager>().CreateViewTimer(0.5f, false, new TimerDelegate(this.ShowGoal), null);
		}

		private void ShowGoal(uint id, object cookie)
		{
			string missionGoal = LangUtils.GetMissionGoal(this.parent.MissionVO);
			if (!string.IsNullOrEmpty(missionGoal))
			{
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(missionGoal, 4f, 0.5f);
			}
		}

		public override void OnSuccessHookComplete()
		{
			base.ResumeBattle();
		}

		public override void OnFailureHookComplete()
		{
			base.ResumeBattle();
		}

		public override void OnGoalFailureHookComplete()
		{
			base.ResumeBattle();
		}

		public override void OnCancel()
		{
			this.RemoveListeners();
			Service.Get<BattleController>().CancelBattle();
			Service.Get<DefensiveBattleController>().EndEncounter();
		}

		private void RemoveListeners()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.GameStateChanged);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.BattleEndFullyProcessed);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.BattleEndFullyProcessed)
			{
				if (id == EventId.GameStateChanged)
				{
					if (Service.Get<GameStateMachine>().CurrentState is HomeState)
					{
						this.parent.CancelMission();
					}
				}
			}
			else
			{
				this.RemoveListeners();
				CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
				if (currentBattle.Won)
				{
					this.parent.CompleteMission(currentBattle.EarnedStars);
					if (this.parent.OnSuccessHook())
					{
						base.PauseBattle();
					}
				}
				else if (this.parent.OnFailureHook())
				{
					base.PauseBattle();
				}
			}
			return EatResponse.NotEaten;
		}
	}
}
