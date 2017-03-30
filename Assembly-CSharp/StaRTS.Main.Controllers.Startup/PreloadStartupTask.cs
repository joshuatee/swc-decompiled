using StaRTS.Assets;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Startup
{
	public class PreloadStartupTask : StartupTask
	{
		public PreloadStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			Service.Get<EventManager>().SendEvent(EventId.PreloadAssetsStart, null);
			Service.Get<AssetManager>().PreloadAssets(new AssetsCompleteDelegate(this.OnPreloadComplete), null);
		}

		private void OnPreloadComplete(object cookie)
		{
			base.Complete();
			Service.Get<EventManager>().SendEvent(EventId.PreloadAssetsEnd, null);
		}
	}
}
