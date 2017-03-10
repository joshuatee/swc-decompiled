using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class DisableGridScrollingStoryAction : AbstractStoryAction
	{
		public DisableGridScrollingStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(1);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			string elementName = this.prepareArgs[0];
			UXGrid uXGrid = Service.Get<ScreenController>().FindElement<UXGrid>(elementName);
			uXGrid.IsScrollable = false;
			this.parent.ChildComplete(this);
		}

		protected internal DisableGridScrollingStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DisableGridScrollingStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DisableGridScrollingStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
