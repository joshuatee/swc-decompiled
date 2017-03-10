using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class StartupTaskController : IViewFrameTimeObserver
	{
		private StartupTaskProgress progressCallback;

		private StartupTaskComplete completeCallback;

		private Queue<StartupTask> tasks;

		private StartupTask currentTask;

		private bool allStartsMustComplete;

		private bool currentTaskCompleted;

		public StartupTaskController(StartupTaskProgress progressCallback, StartupTaskComplete completeCallback)
		{
			this.progressCallback = progressCallback;
			this.completeCallback = completeCallback;
			this.tasks = new Queue<StartupTask>();
			this.currentTask = null;
			this.allStartsMustComplete = false;
			this.currentTaskCompleted = false;
		}

		public void AddTask(StartupTask task)
		{
			task.Startup = this;
			if (this.tasks.Count == 0)
			{
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			}
			if (task != null)
			{
				this.tasks.Enqueue(task);
			}
		}

		public void KillStartup()
		{
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		public void OnViewFrameTime(float dt)
		{
			if (this.currentTask == null)
			{
				if (this.tasks.Count == 0)
				{
					if (this.completeCallback != null)
					{
						this.completeCallback();
					}
					Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
				}
				else
				{
					this.currentTask = this.tasks.Dequeue();
					float endPercentage = (this.tasks.Count == 0) ? 100f : this.tasks.Peek().Percentage;
					this.currentTask.EndPercentage = endPercentage;
					this.currentTaskCompleted = false;
					this.currentTask.Start();
				}
			}
			if (this.progressCallback != null && this.currentTask != null)
			{
				this.progressCallback(this.currentTask.Percentage, this.currentTask.Description);
			}
		}

		public void OnTaskComplete(StartupTask task)
		{
			if (task == this.currentTask)
			{
				this.currentTaskCompleted = true;
				if (this.progressCallback != null)
				{
					this.progressCallback(this.currentTask.EndPercentage, this.currentTask.Description);
				}
				task.Startup = null;
				this.currentTask = null;
			}
		}

		public void AllStartsMustNowComplete()
		{
			this.allStartsMustComplete = true;
		}

		protected internal StartupTaskController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((StartupTaskController)GCHandledObjects.GCHandleToObject(instance)).AddTask((StartupTask)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((StartupTaskController)GCHandledObjects.GCHandleToObject(instance)).AllStartsMustNowComplete();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((StartupTaskController)GCHandledObjects.GCHandleToObject(instance)).KillStartup();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((StartupTaskController)GCHandledObjects.GCHandleToObject(instance)).OnTaskComplete((StartupTask)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((StartupTaskController)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}
	}
}
