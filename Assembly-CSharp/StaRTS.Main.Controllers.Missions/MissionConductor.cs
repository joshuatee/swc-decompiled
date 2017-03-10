using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Story;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Missions
{
	public class MissionConductor : IEventObserver
	{
		private ActionChain introChain;

		private ActionChain successChain;

		private ActionChain failureChain;

		private ActionChain GoalFailureChain;

		private uint introTimer;

		private uint successTimer;

		private uint failureTimer;

		private uint GoalFailureTimer;

		private AbstractMissionProcessor processor;

		private CampaignController campaignController;

		public CampaignMissionVO MissionVO
		{
			get;
			set;
		}

		public MissionConductor(CampaignMissionVO missionVO)
		{
			this.MissionVO = missionVO;
			this.campaignController = Service.Get<CampaignController>();
		}

		public void Start()
		{
			this.processor = MissionProcessorFactory.CreateProcessor(this, this.MissionVO);
			this.processor.Start();
		}

		public void Resume()
		{
			this.processor = MissionProcessorFactory.CreateProcessor(this, this.MissionVO);
			this.processor.Resume();
		}

		public bool OnIntroHook()
		{
			if (!string.IsNullOrEmpty(this.MissionVO.IntroStory))
			{
				this.introTimer = Service.Get<ViewTimerManager>().CreateViewTimer(GameConstants.CAMPAIGN_STORY_INTRO_DELAY, false, new TimerDelegate(this.ExecuteIntroHook), null);
				return true;
			}
			return false;
		}

		public void ExecuteIntroHook(uint id, object cookie)
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.StoryChainCompleted, EventPriority.Default);
			this.introChain = new ActionChain(this.MissionVO.IntroStory);
			this.introTimer = 0u;
			if (!this.introChain.Valid)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Mission {0} has an invalid introStory {1}", new object[]
				{
					this.MissionVO.Uid,
					this.MissionVO.IntroStory
				});
				this.processor.OnIntroHookComplete();
			}
		}

		public bool OnSuccessHook()
		{
			if (!string.IsNullOrEmpty(this.MissionVO.SuccessStory))
			{
				this.successTimer = Service.Get<ViewTimerManager>().CreateViewTimer(GameConstants.CAMPAIGN_STORY_SUCCESS_DELAY, false, new TimerDelegate(this.ExecuteSuccessHook), null);
				return true;
			}
			return false;
		}

		public void ExecuteSuccessHook(uint id, object cookie)
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.StoryChainCompleted, EventPriority.Default);
			this.successChain = new ActionChain(this.MissionVO.SuccessStory);
			this.successTimer = 0u;
			if (!this.successChain.Valid)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Mission {0} has an invalid winStory {1}", new object[]
				{
					this.MissionVO.Uid,
					this.MissionVO.SuccessStory
				});
				this.processor.OnSuccessHookComplete();
			}
		}

		public bool OnFailureHook()
		{
			if (!string.IsNullOrEmpty(this.MissionVO.FailureStory))
			{
				this.failureTimer = Service.Get<ViewTimerManager>().CreateViewTimer(GameConstants.CAMPAIGN_STORY_FAILURE_DELAY, false, new TimerDelegate(this.ExecuteFailureHook), null);
				return true;
			}
			return false;
		}

		public void ExecuteFailureHook(uint id, object cookie)
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.StoryChainCompleted, EventPriority.Default);
			this.failureChain = new ActionChain(this.MissionVO.FailureStory);
			this.failureTimer = 0u;
			if (!this.failureChain.Valid)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Mission {0} has an invalid loseStory {1}", new object[]
				{
					this.MissionVO.Uid,
					this.MissionVO.FailureStory
				});
				this.processor.OnFailureHookComplete();
			}
		}

		public bool OnGoalFailureHook()
		{
			if (!string.IsNullOrEmpty(this.MissionVO.GoalFailureStory))
			{
				this.GoalFailureTimer = Service.Get<ViewTimerManager>().CreateViewTimer(GameConstants.CAMPAIGN_STORY_GoalFailure_DELAY, false, new TimerDelegate(this.ExecuteGoalFailureHook), null);
				return true;
			}
			return false;
		}

		public void ExecuteGoalFailureHook(uint id, object cookie)
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.StoryChainCompleted, EventPriority.Default);
			this.GoalFailureChain = new ActionChain(this.MissionVO.GoalFailureStory);
			this.GoalFailureTimer = 0u;
		}

		public void CancelMission()
		{
			this.processor.OnCancel();
			Service.Get<EventManager>().UnregisterObserver(this, EventId.StoryChainCompleted);
			this.introChain = null;
			this.successChain = null;
			this.failureChain = null;
			if (this.introTimer != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.introTimer);
				this.introTimer = 0u;
			}
			if (this.successTimer != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.successTimer);
				this.successTimer = 0u;
			}
			if (this.failureTimer != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.failureTimer);
				this.failureTimer = 0u;
			}
			if (this.GoalFailureTimer != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.GoalFailureTimer);
				this.GoalFailureTimer = 0u;
			}
			this.campaignController.OnMissionCancelled(this.MissionVO);
		}

		public void CompleteMission(int earnedStars)
		{
			if (this.introChain != null)
			{
				this.introChain.Destroy();
			}
			this.campaignController.CompleteMission(this.MissionVO, earnedStars);
			this.processor.Destroy();
		}

		public void UpdateCounter(string key, int value)
		{
			this.campaignController.UpdateCounter(this.MissionVO, key, value);
		}

		public Dictionary<string, int> GetCounters()
		{
			return this.campaignController.GetCounters(this.MissionVO);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.HomeStateTransitionComplete)
			{
				if (id == EventId.WorldInTransitionComplete || id == EventId.HomeStateTransitionComplete)
				{
					if (Service.Get<GameStateMachine>().CurrentState is HomeState)
					{
						Service.Get<EventManager>().UnregisterObserver(this, EventId.WorldInTransitionComplete);
						Service.Get<EventManager>().UnregisterObserver(this, EventId.HomeStateTransitionComplete);
						if (!this.MissionVO.IsRaidDefense())
						{
							Service.Get<ScreenController>().AddScreen(new MissionCompleteScreen(this.MissionVO));
						}
					}
				}
			}
			else if (id != EventId.StoryChainCompleted)
			{
				if (id == EventId.MissionCompleteScreenDisplayed)
				{
					Service.Get<EventManager>().UnregisterObserver(this, EventId.MissionCompleteScreenDisplayed);
					CampaignVO meta = Service.Get<IDataController>().Get<CampaignVO>(this.MissionVO.CampaignUid);
					Service.Get<ScreenController>().AddScreen(new CampaignCompleteScreen(meta));
				}
			}
			else
			{
				ActionChain chain = cookie as ActionChain;
				this.CompleteChain(chain);
			}
			return EatResponse.NotEaten;
		}

		private void CompleteChain(ActionChain chain)
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.StoryChainCompleted);
			if (chain == this.introChain)
			{
				this.processor.OnIntroHookComplete();
				return;
			}
			if (chain == this.successChain)
			{
				this.processor.OnSuccessHookComplete();
				if (Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
				{
					return;
				}
				CampaignVO campaignVO = Service.Get<IDataController>().Get<CampaignVO>(this.MissionVO.CampaignUid);
				CampaignMissionVO lastMission = Service.Get<CampaignController>().GetLastMission(this.MissionVO.CampaignUid);
				bool flag = this.MissionVO == lastMission;
				bool flag2 = Service.Get<CurrentPlayer>().CampaignProgress.GetTotalCampaignStarsEarned(campaignVO) >= campaignVO.TotalMasteryStars;
				if (flag | flag2)
				{
					if (Service.Get<GameStateMachine>().CurrentState is HomeState)
					{
						Service.Get<ScreenController>().AddScreen(new CampaignCompleteScreen(campaignVO));
					}
					else
					{
						Service.Get<EventManager>().RegisterObserver(this, EventId.MissionCompleteScreenDisplayed, EventPriority.Default);
					}
				}
				if (!(Service.Get<GameStateMachine>().CurrentState is HomeState))
				{
					Service.Get<EventManager>().RegisterObserver(this, EventId.WorldInTransitionComplete, EventPriority.Default);
					Service.Get<EventManager>().RegisterObserver(this, EventId.HomeStateTransitionComplete, EventPriority.Default);
					return;
				}
				if (!lastMission.IsRaidDefense())
				{
					Service.Get<ScreenController>().AddScreen(new MissionCompleteScreen(this.MissionVO));
					return;
				}
			}
			else
			{
				if (chain == this.failureChain)
				{
					this.processor.OnFailureHookComplete();
					return;
				}
				if (chain == this.GoalFailureChain)
				{
					this.processor.OnGoalFailureHookComplete();
				}
			}
		}

		protected internal MissionConductor(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).CancelMission();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).CompleteChain((ActionChain)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).CompleteMission(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).MissionVO);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).GetCounters());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).OnFailureHook());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).OnGoalFailureHook());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).OnIntroHook());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).OnSuccessHook());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).Resume();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).MissionVO = (CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((MissionConductor)GCHandledObjects.GCHandleToObject(instance)).UpdateCounter(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}
	}
}
