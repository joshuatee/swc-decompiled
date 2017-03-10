using StaRTS.Assets;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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

		protected internal PreloadStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PreloadStartupTask)GCHandledObjects.GCHandleToObject(instance)).OnPreloadComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PreloadStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
