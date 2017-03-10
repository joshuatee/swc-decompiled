using StaRTS.Assets;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Story;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class QuestController : ITriggerReactor
	{
		private const string STEP_LABEL = "StepLabel";

		private const int HISTORY_LENGTH = 40;

		private List<string> stepsHistory;

		private List<string> pendingTriggerIds;

		private List<IStoryTrigger> activeTriggers;

		private UXLabel currentStepLabel;

		private int autoIncrement;

		private SessionStartTriggerParent sessionBuffer;

		public bool LogSteps
		{
			get;
			set;
		}

		public QuestController()
		{
			Service.Set<QuestController>(this);
			this.LogSteps = true;
			this.stepsHistory = new List<string>();
			this.pendingTriggerIds = new List<string>();
			this.activeTriggers = new List<IStoryTrigger>();
			this.sessionBuffer = new SessionStartTriggerParent();
		}

		public int AutoIncrement()
		{
			int num = this.autoIncrement;
			this.autoIncrement = num + 1;
			return num;
		}

		public void ActivateTrigger(string triggerUid, bool save)
		{
			StoryTriggerVO vo = Service.Get<IDataController>().Get<StoryTriggerVO>(triggerUid);
			IStoryTrigger trigger = StoryTriggerFactory.GenerateStoryTrigger(vo, this);
			this.ActivateTrigger(trigger, save);
		}

		public void ActivateTrigger(IStoryTrigger trigger, bool save)
		{
			if (save)
			{
				Service.Get<CurrentPlayer>().ActiveSaveTriggers.Add(trigger);
			}
			else
			{
				this.activeTriggers.Add(trigger);
			}
			string eventInfo = string.Format("Activating Story Trigger {0}-{1}", new object[]
			{
				trigger.VO.Uid,
				trigger.VO.TriggerType
			});
			trigger.Activate();
			Service.Get<AssetManager>().Profiler.RecordEvent(eventInfo);
		}

		public void SatisfyTrigger(IStoryTrigger trigger)
		{
			string eventInfo = string.Format("Satisfied Story Trigger {0}", new object[]
			{
				trigger.VO.Uid
			});
			Service.Get<AssetManager>().Profiler.RecordEvent(eventInfo);
			string reaction = trigger.Reaction;
			bool hasReaction = trigger.HasReaction;
			this.DestroyTrigger(trigger);
			if (hasReaction)
			{
				new ActionChain(reaction);
			}
		}

		public void KillTrigger(string uid)
		{
			IStoryTrigger storyTrigger = null;
			int i = 0;
			int count = this.activeTriggers.Count;
			while (i < count)
			{
				if (this.activeTriggers[i].VO.Uid == uid)
				{
					storyTrigger = this.activeTriggers[i];
					break;
				}
				i++;
			}
			if (storyTrigger == null)
			{
				List<IStoryTrigger> activeSaveTriggers = Service.Get<CurrentPlayer>().ActiveSaveTriggers;
				i = 0;
				count = activeSaveTriggers.Count;
				while (i < count)
				{
					if (activeSaveTriggers[i].VO.Uid == uid)
					{
						storyTrigger = activeSaveTriggers[i];
						break;
					}
					i++;
				}
			}
			if (storyTrigger != null)
			{
				this.DestroyTrigger(storyTrigger);
			}
		}

		public void DestroyTrigger(IStoryTrigger trigger)
		{
			if (this.activeTriggers.Contains(trigger))
			{
				this.activeTriggers.Remove(trigger);
			}
			else if (Service.Get<CurrentPlayer>().ActiveSaveTriggers.Contains(trigger))
			{
				Service.Get<CurrentPlayer>().ActiveSaveTriggers.Remove(trigger);
			}
			trigger.Destroy();
		}

		public void KillAllTriggers()
		{
			List<IStoryTrigger> activeSaveTriggers = Service.Get<CurrentPlayer>().ActiveSaveTriggers;
			int i = 0;
			int count = activeSaveTriggers.Count;
			while (i < count)
			{
				activeSaveTriggers[i].Destroy();
				i++;
			}
			activeSaveTriggers.Clear();
			int j = 0;
			int count2 = this.activeTriggers.Count;
			while (j < count2)
			{
				this.activeTriggers[j].Destroy();
				j++;
			}
			this.activeTriggers.Clear();
			this.sessionBuffer.KillAllTriggers();
		}

		public void StartPendingTriggers()
		{
			if (this.pendingTriggerIds == null)
			{
				return;
			}
			int i = 0;
			int count = this.pendingTriggerIds.Count;
			while (i < count)
			{
				if (!string.IsNullOrEmpty(this.pendingTriggerIds[i]))
				{
					this.ActivateTrigger(this.pendingTriggerIds[i], false);
				}
				i++;
			}
		}

		public bool DoesPendingTriggersContainPrefix(string prefix)
		{
			int i = 0;
			int count = this.pendingTriggerIds.Count;
			while (i < count)
			{
				if (this.pendingTriggerIds[i].StartsWith(prefix))
				{
					return true;
				}
				i++;
			}
			return false;
		}

		public bool HasPendingTriggers()
		{
			return this.pendingTriggerIds.Count > 0;
		}

		public void SavePendingTriggers(List<object> triggerArray)
		{
			if (triggerArray == null)
			{
				return;
			}
			int i = 0;
			int count = triggerArray.Count;
			while (i < count)
			{
				string text = triggerArray[i] as string;
				if (!string.IsNullOrEmpty(text))
				{
					this.pendingTriggerIds.Add(text);
				}
				i++;
			}
		}

		public void LogAction(StoryActionVO action)
		{
			if (!this.LogSteps)
			{
				return;
			}
			this.stepsHistory.Add(action.Uid);
			if (this.stepsHistory.Count > 40)
			{
				this.stepsHistory.RemoveAt(0);
			}
			if (this.currentStepLabel != null)
			{
				this.currentStepLabel.Text = action.Uid;
			}
		}

		public void ToggleStoryLabel()
		{
			if (this.currentStepLabel == null)
			{
				this.currentStepLabel = Service.Get<UXController>().MiscElementsManager.CreateGameBoardLabel("StepLabel", Service.Get<UXController>().PerformanceAnchor);
				this.currentStepLabel.Pivot = UIWidget.Pivot.BottomRight;
				this.currentStepLabel.LocalPosition = new Vector3(-120f, 60f, 0f);
				this.currentStepLabel.Text = "Waiting for next step...";
				this.currentStepLabel.Visible = false;
			}
			this.currentStepLabel.Visible = !this.currentStepLabel.Visible;
		}

		public bool RestoreLastQuest()
		{
			bool result = false;
			string restoredQuest = Service.Get<CurrentPlayer>().RestoredQuest;
			Service.Get<CurrentPlayer>().RestoredQuest = string.Empty;
			Service.Get<CurrentPlayer>().CurrentQuest = restoredQuest;
			if (!string.IsNullOrEmpty(restoredQuest))
			{
				new ActionChain(restoredQuest);
				result = true;
			}
			this.sessionBuffer.ReleaseSatisfiedTriggers();
			return result;
		}

		protected internal QuestController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((QuestController)GCHandledObjects.GCHandleToObject(instance)).ActivateTrigger((IStoryTrigger)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((QuestController)GCHandledObjects.GCHandleToObject(instance)).ActivateTrigger(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QuestController)GCHandledObjects.GCHandleToObject(instance)).AutoIncrement());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((QuestController)GCHandledObjects.GCHandleToObject(instance)).DestroyTrigger((IStoryTrigger)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QuestController)GCHandledObjects.GCHandleToObject(instance)).DoesPendingTriggersContainPrefix(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QuestController)GCHandledObjects.GCHandleToObject(instance)).LogSteps);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QuestController)GCHandledObjects.GCHandleToObject(instance)).HasPendingTriggers());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((QuestController)GCHandledObjects.GCHandleToObject(instance)).KillAllTriggers();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((QuestController)GCHandledObjects.GCHandleToObject(instance)).KillTrigger(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((QuestController)GCHandledObjects.GCHandleToObject(instance)).LogAction((StoryActionVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QuestController)GCHandledObjects.GCHandleToObject(instance)).RestoreLastQuest());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((QuestController)GCHandledObjects.GCHandleToObject(instance)).SatisfyTrigger((IStoryTrigger)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((QuestController)GCHandledObjects.GCHandleToObject(instance)).SavePendingTriggers((List<object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((QuestController)GCHandledObjects.GCHandleToObject(instance)).LogSteps = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((QuestController)GCHandledObjects.GCHandleToObject(instance)).StartPendingTriggers();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((QuestController)GCHandledObjects.GCHandleToObject(instance)).ToggleStoryLabel();
			return -1L;
		}
	}
}
