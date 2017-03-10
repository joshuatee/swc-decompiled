using StaRTS.Main.Controllers.Performance;
using StaRTS.Utils;
using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class SchedulingStartupTask : StartupTask
	{
		public SchedulingStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			new Rand();
			new SimTimerManager();
			new ViewTimerManager();
			new PerformanceMonitor();
			base.Complete();
		}

		protected internal SchedulingStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SchedulingStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
