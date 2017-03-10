using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class StartupTask
	{
		protected float curPercentage;

		protected string curDescription;

		protected float startPercentage;

		protected float endPercentage;

		public StartupTaskController Startup
		{
			get;
			set;
		}

		public float EndPercentage
		{
			get
			{
				return this.endPercentage;
			}
			set
			{
				this.endPercentage = value;
			}
		}

		public float Percentage
		{
			get
			{
				return this.curPercentage;
			}
		}

		public string Description
		{
			get
			{
				return this.curDescription;
			}
		}

		public StartupTask(float startPercentage)
		{
			this.startPercentage = startPercentage;
			this.curPercentage = startPercentage;
			this.endPercentage = startPercentage;
			this.curDescription = "";
		}

		public virtual void Start()
		{
		}

		protected void Complete()
		{
			this.curPercentage = this.endPercentage;
			this.Startup.OnTaskComplete(this);
		}

		protected internal StartupTask(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((StartupTask)GCHandledObjects.GCHandleToObject(instance)).Complete();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StartupTask)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StartupTask)GCHandledObjects.GCHandleToObject(instance)).EndPercentage);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StartupTask)GCHandledObjects.GCHandleToObject(instance)).Percentage);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StartupTask)GCHandledObjects.GCHandleToObject(instance)).Startup);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((StartupTask)GCHandledObjects.GCHandleToObject(instance)).EndPercentage = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((StartupTask)GCHandledObjects.GCHandleToObject(instance)).Startup = (StartupTaskController)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((StartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
