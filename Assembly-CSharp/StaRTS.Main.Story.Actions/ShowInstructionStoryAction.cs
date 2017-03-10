using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class ShowInstructionStoryAction : AbstractStoryAction
	{
		private const int INSTRUCTION_KEY_ARG = 0;

		private const int DELAY_KEY_ARG = 1;

		private const int DURATION_KEY_ARG = 2;

		private string msg;

		private float delay;

		private float duration;

		public ShowInstructionStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(new int[]
			{
				1,
				3
			});
			this.msg = Service.Get<Lang>().Get(this.prepareArgs[0], new object[0]);
			if (this.prepareArgs.Length > 1)
			{
				float.TryParse(this.prepareArgs[1], 167, CultureInfo.InvariantCulture, ref this.delay);
				float.TryParse(this.prepareArgs[2], 167, CultureInfo.InvariantCulture, ref this.duration);
			}
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(this.msg, this.delay, this.duration);
			this.parent.ChildComplete(this);
		}

		protected internal ShowInstructionStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ShowInstructionStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ShowInstructionStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
