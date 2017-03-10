using StaRTS.Audio;
using StaRTS.Main.Configs;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class SetMusicVolumeStoryAction : AbstractStoryAction
	{
		private const int ARG_VOLUME = 0;

		private float volume;

		public SetMusicVolumeStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(1);
			this.volume = (float)Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture) / 100f;
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			float num = PlayerSettings.GetMusicVolume() * this.volume;
			Service.Get<AudioManager>().SetVolume(AudioCategory.Music, num);
			this.parent.ChildComplete(this);
		}

		protected internal SetMusicVolumeStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SetMusicVolumeStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SetMusicVolumeStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
