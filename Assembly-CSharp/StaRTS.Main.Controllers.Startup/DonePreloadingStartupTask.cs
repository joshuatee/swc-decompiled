using StaRTS.Assets;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class DonePreloadingStartupTask : StartupTask
	{
		public DonePreloadingStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			Service.Get<AssetManager>().DonePreloading();
			base.Startup.AllStartsMustNowComplete();
			base.Complete();
		}

		protected internal DonePreloadingStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DonePreloadingStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
