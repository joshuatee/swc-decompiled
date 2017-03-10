using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class CircleRegionStoryAction : AbstractStoryAction
	{
		private const int BOARD_X_ARG = 0;

		private const int BOARD_Z_ARG = 1;

		private const int WIDTH_ARG = 2;

		private const int DEPTH_ARG = 3;

		private int boardX;

		private int boardZ;

		private int width;

		private int depth;

		public CircleRegionStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(4);
			this.boardX = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
			this.boardZ = Convert.ToInt32(this.prepareArgs[1], CultureInfo.InvariantCulture);
			this.width = Convert.ToInt32(this.prepareArgs[2], CultureInfo.InvariantCulture);
			this.depth = Convert.ToInt32(this.prepareArgs[3], CultureInfo.InvariantCulture);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<UXController>().MiscElementsManager.HighlightRegion((float)this.boardX, (float)this.boardZ, this.width, this.depth);
			this.parent.ChildComplete(this);
		}

		protected internal CircleRegionStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CircleRegionStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CircleRegionStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
