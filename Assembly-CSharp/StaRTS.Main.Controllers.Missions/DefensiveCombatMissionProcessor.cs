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
using WinRTBridge;

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
				return;
			}
			this.StartMission();
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

		protected internal DefensiveCombatMissionProcessor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DefensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnCancel();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DefensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnFailureHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DefensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnGoalFailureHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((DefensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnIntroHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DefensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnSuccessHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((DefensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).RemoveListeners();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((DefensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Resume();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((DefensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((DefensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).StartMission();
			return -1L;
		}
	}
}
