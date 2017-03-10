using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class HomeStartupTask : StartupTask
	{
		public HomeStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			new HomeModeController();
			base.Complete();
		}

		protected internal HomeStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HomeStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
