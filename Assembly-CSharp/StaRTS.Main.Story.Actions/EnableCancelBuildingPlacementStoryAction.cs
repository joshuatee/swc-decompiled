using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class EnableCancelBuildingPlacementStoryAction : AbstractStoryAction
	{
		private bool enable;

		public EnableCancelBuildingPlacementStoryAction(StoryActionVO vo, IStoryReactor parent, bool enable) : base(vo, parent)
		{
			this.enable = enable;
		}

		public override void Prepare()
		{
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<UXController>().MiscElementsManager.EnableCancelBuildingPlacement = this.enable;
			this.parent.ChildComplete(this);
		}

		protected internal EnableCancelBuildingPlacementStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EnableCancelBuildingPlacementStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EnableCancelBuildingPlacementStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
