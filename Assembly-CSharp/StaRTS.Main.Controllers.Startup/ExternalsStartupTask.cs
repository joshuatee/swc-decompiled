using StaRTS.Externals.DMOAnalytics;
using StaRTS.Externals.IAP;
using StaRTS.Main.Controllers.Notifications;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class ExternalsStartupTask : StartupTask
	{
		public ExternalsStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			Service.Get<DMOAnalyticsController>().Init();
			new NetworkConnectionTester().CheckNetworkConnectionAvailable(false);
			new InAppPurchaseController();
			new NotificationController();
			base.Complete();
		}

		protected internal ExternalsStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ExternalsStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
