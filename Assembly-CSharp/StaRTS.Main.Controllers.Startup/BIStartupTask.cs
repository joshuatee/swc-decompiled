using StaRTS.Externals.BI;
using StaRTS.Externals.DMOAnalytics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class BIStartupTask : StartupTask
	{
		public BIStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			new BILoggingController();
			new DMOAnalyticsController();
			base.Complete();
		}

		protected internal BIStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BIStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
