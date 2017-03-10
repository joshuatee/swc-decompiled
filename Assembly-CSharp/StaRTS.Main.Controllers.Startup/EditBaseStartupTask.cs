using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class EditBaseStartupTask : StartupTask
	{
		public EditBaseStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			new EditBaseController();
			new BaseLayoutToolController();
			new WarBaseEditController();
			base.Complete();
		}

		protected internal EditBaseStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EditBaseStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
