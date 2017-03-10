using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class UndimScreenStoryAction : AbstractStoryAction
	{
		public UndimScreenStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(0);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			this.parent.ChildComplete(this);
		}

		protected internal UndimScreenStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UndimScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((UndimScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
