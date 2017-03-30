using StaRTS.Assets;
using StaRTS.Audio;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;

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
	}
}
