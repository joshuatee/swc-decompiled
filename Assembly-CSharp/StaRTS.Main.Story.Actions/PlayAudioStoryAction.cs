using StaRTS.Audio;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class PlayAudioStoryAction : AbstractStoryAction
	{
		private const int AUDIO_CLIP_ARG = 0;

		public PlayAudioStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(2);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			if (LangUtils.ShouldPlayVOClips())
			{
				Service.Get<AudioManager>().PlayAudio(this.prepareArgs[0]);
			}
			this.parent.ChildComplete(this);
		}

		protected internal PlayAudioStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PlayAudioStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlayAudioStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
