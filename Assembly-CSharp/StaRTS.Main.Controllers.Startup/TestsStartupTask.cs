using StaRTS.Main.Utils.Test;
using StaRTS.RuntimeTools.BigHeadMode;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class TestsStartupTask : StartupTask
	{
		public TestsStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			new PromoCodeTest();
			new BigHeadModeController();
			base.Complete();
		}

		protected internal TestsStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TestsStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
