using StaRTS.Externals.GameServices;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.RUF.RUFTasks;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.RUF
{
	public class RUFManager : IEventObserver
	{
		private const string AUTO_TRIGGER_TYPE = "Auto";

		private const string AUTO_LOAD_TRIGGER_TYPE = "AutoLoad";

		private List<AbstractRUFTask> queue;

		private List<AbstractRUFTask> loadStateQueue;

		private bool processingLoadState;

		public List<int> OmitRateAppLevels
		{
			get;
			set;
		}

		public RUFManager()
		{
			this.queue = new List<AbstractRUFTask>();
			this.loadStateQueue = new List<AbstractRUFTask>();
			this.OmitRateAppLevels = new List<int>();
			Service.Set<RUFManager>(this);
		}

		public void PrepareReturningUserFlow()
		{
			this.processingLoadState = true;
			Service.Get<EventManager>().RegisterObserver(this, EventId.WorldInTransitionComplete, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.PurgeHomeStateRUFTask, EventPriority.Default);
			this.AddOfflineContractTasksToQueue();
			this.AddTaskToAppropriateQueue(new FueRUFTask());
			this.AddTaskToAppropriateQueue(new FueResumeRUFTask());
			this.AddTaskToAppropriateQueue(new CallsignRUFTask());
			this.AddTaskToAppropriateQueue(new HolonetRUFTask());
			this.AddTaskToAppropriateQueue(new GoToHomeStateRUFTask());
			this.AddAutoTriggerTasksToQueue();
			this.SortTaskQueues();
			this.ProcessQueue(true);
		}

		private void AddTaskToAppropriateQueue(AbstractRUFTask task)
		{
			if (task.ShouldPlayFromLoadState)
			{
				this.loadStateQueue.Add(task);
				return;
			}
			this.queue.Add(task);
		}

		private void SortTaskQueues()
		{
			this.loadStateQueue.Sort(new Comparison<AbstractRUFTask>(RUFManager.CompareRUFTasks));
			this.queue.Sort(new Comparison<AbstractRUFTask>(RUFManager.CompareRUFTasks));
		}

		private void AddOfflineContractTasksToQueue()
		{
			List<ContractTO> contractEventsThatHappenedOffline = Service.Get<ISupportController>().GetContractEventsThatHappenedOffline();
			if (contractEventsThatHappenedOffline == null || contractEventsThatHappenedOffline.Count < 1)
			{
				return;
			}
			int count = contractEventsThatHappenedOffline.Count;
			IDataController dataController = Service.Get<IDataController>();
			for (int i = 0; i < count; i++)
			{
				ContractTO contractTO = contractEventsThatHappenedOffline[i];
				if (contractTO.ContractType == ContractType.Upgrade)
				{
					BuildingTypeVO buildingTypeVO = dataController.Get<BuildingTypeVO>(contractTO.Uid);
					if (buildingTypeVO.Type == BuildingType.HQ)
					{
						this.AddTaskToAppropriateQueue(new HQCelebRUFTask());
					}
				}
			}
		}

		public void UpdateProcessingLoadState(bool processLoadState)
		{
			this.processingLoadState = processLoadState;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.WorldInTransitionComplete)
			{
				this.ProcessQueue(true);
				GameServicesManager.AttemptAutomaticSignInPrompt();
				Service.Get<EventManager>().UnregisterObserver(this, EventId.WorldInTransitionComplete);
			}
			else if (id == EventId.PurgeHomeStateRUFTask)
			{
				this.PurgeQueueByPriority(50);
			}
			return EatResponse.NotEaten;
		}

		public void AddAutoTriggerTasksToQueue()
		{
			Dictionary<string, StoryTriggerVO>.ValueCollection all = Service.Get<IDataController>().GetAll<StoryTriggerVO>();
			foreach (StoryTriggerVO current in all)
			{
				bool flag = current.TriggerType.Equals("Auto");
				bool flag2 = current.TriggerType.Equals("AutoLoad");
				if (flag | flag2)
				{
					AbstractRUFTask rUFTaskForAutoTrigger = AutoStoryTriggerUtils.GetRUFTaskForAutoTrigger(current);
					if (rUFTaskForAutoTrigger != null)
					{
						this.AddTaskToAppropriateQueue(rUFTaskForAutoTrigger);
					}
				}
			}
		}

		public void ProcessQueue(bool continueProcessing)
		{
			if (!continueProcessing)
			{
				return;
			}
			AbstractRUFTask task;
			if (this.processingLoadState)
			{
				if (this.loadStateQueue.Count < 1)
				{
					return;
				}
				task = this.loadStateQueue[0];
			}
			else
			{
				if (this.queue.Count < 1)
				{
					this.OnComplete();
					return;
				}
				task = this.queue[0];
			}
			this.ProcessTask(task, continueProcessing);
		}

		private void RemoveTaskFromQueue(AbstractRUFTask task)
		{
			if (task.ShouldPlayFromLoadState)
			{
				this.loadStateQueue.Remove(task);
				return;
			}
			this.queue.Remove(task);
		}

		private void ProcessTask(AbstractRUFTask task, bool continueProcessing)
		{
			if (task != null)
			{
				this.RemoveTaskFromQueue(task);
				if (task.ShouldProcess && task.ShouldPurgeQueue)
				{
					if (task.PriorityPurgeThreshold == 0)
					{
						this.queue.Clear();
					}
					else
					{
						this.PurgeQueueByPriority(task.PriorityPurgeThreshold);
					}
				}
				task.Process(continueProcessing);
			}
		}

		private void PurgeQueueByPriority(int priority)
		{
			List<AbstractRUFTask> list = new List<AbstractRUFTask>();
			for (int i = this.queue.Count - 1; i >= 0; i--)
			{
				if (priority <= this.queue[i].Priority)
				{
					list.Add(this.queue[i]);
				}
			}
			int count = list.Count;
			for (int j = 0; j < count; j++)
			{
				this.queue.Remove(list[j]);
			}
			list.Clear();
			for (int k = this.loadStateQueue.Count - 1; k >= 0; k--)
			{
				if (priority <= this.loadStateQueue[k].Priority)
				{
					list.Add(this.loadStateQueue[k]);
				}
			}
			count = list.Count;
			for (int l = 0; l < count; l++)
			{
				this.loadStateQueue.Remove(list[l]);
			}
			list.Clear();
		}

		private void OnComplete()
		{
			Service.Get<ISupportController>().ReleaseContractEventsThatHappnedOffline();
		}

		private static int CompareRUFTasks(AbstractRUFTask a, AbstractRUFTask b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a.Priority != b.Priority)
			{
				return a.Priority - b.Priority;
			}
			if (!a.ShouldPurgeQueue && !b.ShouldPurgeQueue)
			{
				return 0;
			}
			if (a.ShouldPurgeQueue && b.ShouldPurgeQueue)
			{
				return 0;
			}
			if (a.ShouldPurgeQueue)
			{
				return 1;
			}
			return -1;
		}

		protected internal RUFManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((RUFManager)GCHandledObjects.GCHandleToObject(instance)).AddAutoTriggerTasksToQueue();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((RUFManager)GCHandledObjects.GCHandleToObject(instance)).AddOfflineContractTasksToQueue();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((RUFManager)GCHandledObjects.GCHandleToObject(instance)).AddTaskToAppropriateQueue((AbstractRUFTask)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RUFManager.CompareRUFTasks((AbstractRUFTask)GCHandledObjects.GCHandleToObject(*args), (AbstractRUFTask)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RUFManager)GCHandledObjects.GCHandleToObject(instance)).OmitRateAppLevels);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((RUFManager)GCHandledObjects.GCHandleToObject(instance)).OnComplete();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RUFManager)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((RUFManager)GCHandledObjects.GCHandleToObject(instance)).PrepareReturningUserFlow();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((RUFManager)GCHandledObjects.GCHandleToObject(instance)).ProcessQueue(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((RUFManager)GCHandledObjects.GCHandleToObject(instance)).ProcessTask((AbstractRUFTask)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((RUFManager)GCHandledObjects.GCHandleToObject(instance)).PurgeQueueByPriority(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((RUFManager)GCHandledObjects.GCHandleToObject(instance)).RemoveTaskFromQueue((AbstractRUFTask)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((RUFManager)GCHandledObjects.GCHandleToObject(instance)).OmitRateAppLevels = (List<int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((RUFManager)GCHandledObjects.GCHandleToObject(instance)).SortTaskQueues();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((RUFManager)GCHandledObjects.GCHandleToObject(instance)).UpdateProcessingLoadState(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
