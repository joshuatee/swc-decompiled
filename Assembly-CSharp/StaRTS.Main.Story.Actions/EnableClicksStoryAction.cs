using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class EnableClicksStoryAction : AbstractStoryAction
	{
		public EnableClicksStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
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
			Service.Get<UserInputInhibitor>().AllowAll();
			this.parent.ChildComplete(this);
		}

		protected internal EnableClicksStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EnableClicksStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EnableClicksStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
