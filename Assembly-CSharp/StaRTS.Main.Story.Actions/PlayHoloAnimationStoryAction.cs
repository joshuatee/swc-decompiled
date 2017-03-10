using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class PlayHoloAnimationStoryAction : AbstractStoryAction
	{
		private const int ANIM_NAME_ARG = 0;

		public string AnimName
		{
			get
			{
				return this.prepareArgs[0];
			}
		}

		public PlayHoloAnimationStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
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
			Service.Get<EventManager>().SendEvent(EventId.PlayHologramAnimation, this);
			this.parent.ChildComplete(this);
		}

		protected internal PlayHoloAnimationStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PlayHoloAnimationStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayHoloAnimationStoryAction)GCHandledObjects.GCHandleToObject(instance)).AnimName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PlayHoloAnimationStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
