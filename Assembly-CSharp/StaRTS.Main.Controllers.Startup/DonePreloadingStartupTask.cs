using StaRTS.Assets;
using StaRTS.Utils.Core;
using System;

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
	}
}
