using StaRTS.Main.Controllers;
using StaRTS.Main.Story.Trigger;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.RUF.RUFTasks
{
	public class AutoTriggerRUFTask : AbstractRUFTask
	{
		private AutoStoryTrigger trigger;

		public AutoTriggerRUFTask(AutoStoryTrigger trigger)
		{
			this.trigger = trigger;
			base.Priority = 40;
			base.ShouldProcess = trigger.IsPreSatisfied();
			base.ShouldPlayFromLoadState = true;
			base.ShouldPurgeQueue = false;
		}

		public override void Process(bool continueProcessing)
		{
			if (base.ShouldProcess)
			{
				Service.Get<QuestController>().ActivateTrigger(this.trigger, false);
			}
			base.Process(continueProcessing);
		}

		protected internal AutoTriggerRUFTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AutoTriggerRUFTask)GCHandledObjects.GCHandleToObject(instance)).Process(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
