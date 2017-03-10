using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Commands.Missions;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Missions
{
	public class OffensiveCombatMissionProcessor : AbstractMissionProcessor, IEventObserver
	{
		private const float GOAL_DELAY = 0.5f;

		private const float GOAL_DURATION = 4f;

		private const float GOAL_FADE = 0.5f;

		private BattleInitializationData data;

		public OffensiveCombatMissionProcessor(MissionConductor parent) : base(parent)
		{
		}

		public override void Start()
		{
			if (this.parent.MissionVO.Grind)
			{
				MissionIdRequest request = new MissionIdRequest(this.parent.MissionVO.Uid);
				GetMissionMapCommand getMissionMapCommand = new GetMissionMapCommand(request);
				getMissionMapCommand.AddSuccessCallback(new AbstractCommand<MissionIdRequest, GetMissionMapResponse>.OnSuccessCallback(this.OnServerGrindSelectionCompleteOnSuccess));
				Service.Get<ServerAPI>().Sync(getMissionMapCommand);
				return;
			}
			this.data = BattleInitializationData.CreateFromCampaignMissionVO(this.parent.MissionVO.Uid);
			this.LoadBattle();
		}

		public override void Resume()
		{
			this.Start();
		}

		private void OnServerGrindSelectionCompleteOnSuccess(GetMissionMapResponse response, object cookie)
		{
			this.data = BattleInitializationData.CreateFromCampaignMissionAndBattle(this.parent.MissionVO.Uid, response.BattleUid);
			this.LoadBattle();
		}

		private void LoadBattle()
		{
			BattleStartState.GoToBattleStartState(this.data, new TransitionCompleteDelegate(this.OnWorldLoaded));
		}

		private void OnWorldLoaded()
		{
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

		public override void OnIntroHookComplete()
		{
			base.ResumeBattle();
			this.StartMission();
		}

		private void StartMission()
		{
			Service.Get<ViewTimerManager>().CreateViewTimer(0.5f, false, new TimerDelegate(this.ShowGoal), null);
			Service.Get<EventManager>().SendEvent(EventId.MissionStarted, null);
		}

		private void ShowGoal(uint id, object cookie)
		{
			string missionGoal = LangUtils.GetMissionGoal(this.parent.MissionVO);
			if (!string.IsNullOrEmpty(missionGoal))
			{
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(missionGoal, 4f, 0.5f);
			}
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
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				currentPlayer.CampaignProgress.UpdateMissionLoot(this.parent.MissionVO.Uid, currentBattle);
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

		private void RemoveListeners()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.GameStateChanged);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.BattleEndFullyProcessed);
		}

		public override void OnCancel()
		{
			this.RemoveListeners();
			Service.Get<BattleController>().CancelBattle();
		}

		protected internal OffensiveCombatMissionProcessor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).LoadBattle();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnCancel();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnFailureHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnGoalFailureHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnIntroHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnServerGrindSelectionCompleteOnSuccess((GetMissionMapResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnSuccessHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnWorldLoaded();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).RemoveListeners();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Resume();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((OffensiveCombatMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).StartMission();
			return -1L;
		}
	}
}
