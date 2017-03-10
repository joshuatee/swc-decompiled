using StaRTS.Main.Story;
using System;
using WinRTBridge;

namespace StaRTS.Main.RUF.RUFTasks
{
	public class ResumeTutorialRUFTask : AbstractRUFTask
	{
		private string story;

		public ResumeTutorialRUFTask(string story)
		{
			base.Priority = 70;
			base.ShouldPurgeQueue = true;
			base.ShouldProcess = true;
			this.story = story;
		}

		public override void Process(bool continueProcessing)
		{
			if (base.ShouldProcess)
			{
				new ActionChain(this.story);
			}
			base.Process(continueProcessing);
		}

		protected internal ResumeTutorialRUFTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ResumeTutorialRUFTask)GCHandledObjects.GCHandleToObject(instance)).Process(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
