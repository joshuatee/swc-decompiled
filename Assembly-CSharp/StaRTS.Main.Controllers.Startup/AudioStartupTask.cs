using StaRTS.Assets;
using StaRTS.Audio;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class AudioStartupTask : StartupTask
	{
		public AudioStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			Service.Get<EventManager>().SendEvent(EventId.InitializeAudioStart, null);
			new AudioManager(new AssetsCompleteDelegate(this.OnAudioComplete));
		}

		private void OnAudioComplete(object cookie)
		{
			Service.Get<IDataController>().Unload<AssetTypeVO>();
			base.Complete();
			Service.Get<EventManager>().SendEvent(EventId.InitializeAudioEnd, null);
		}

		protected internal AudioStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AudioStartupTask)GCHandledObjects.GCHandleToObject(instance)).OnAudioComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AudioStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
