using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class ZoomCameraStoryAction : AbstractStoryAction
	{
		private const int ZOOM_AMOUNT_ARG = 0;

		private float zoomAmount;

		public ZoomCameraStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(1);
			this.zoomAmount = (float)Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture) / 100f;
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<WorldInitializer>().View.ZoomTo(this.zoomAmount);
			this.parent.ChildComplete(this);
		}

		protected internal ZoomCameraStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ZoomCameraStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ZoomCameraStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
