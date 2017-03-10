using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class PressHereStoryAction : AbstractStoryAction
	{
		private const int BOARDX_ARG = 0;

		private const int BOARDZ_ARG = 1;

		private float boardX;

		private float boardZ;

		private bool screen;

		public PressHereStoryAction(StoryActionVO vo, IStoryReactor parent, bool screen) : base(vo, parent)
		{
			this.screen = screen;
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(2);
			this.boardX = Convert.ToSingle(this.prepareArgs[0], CultureInfo.InvariantCulture);
			this.boardZ = Convert.ToSingle(this.prepareArgs[1], CultureInfo.InvariantCulture);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			if (this.screen)
			{
				Service.Get<UXController>().MiscElementsManager.ShowArrowOnScreen(this.boardX, this.boardZ);
			}
			else
			{
				Service.Get<UXController>().MiscElementsManager.ShowFingerAnimation(this.boardX, this.boardZ);
			}
			this.parent.ChildComplete(this);
		}

		protected internal PressHereStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PressHereStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PressHereStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
