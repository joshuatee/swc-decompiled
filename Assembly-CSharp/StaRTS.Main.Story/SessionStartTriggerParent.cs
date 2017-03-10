using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Story
{
	public class SessionStartTriggerParent : ITriggerReactor
	{
		private List<IStoryTrigger> triggerBuffer;

		private bool buffering;

		public SessionStartTriggerParent()
		{
			this.triggerBuffer = new List<IStoryTrigger>();
			this.buffering = true;
		}

		public void SatisfyTrigger(IStoryTrigger trigger)
		{
			if (this.buffering)
			{
				this.triggerBuffer.Add(trigger);
				return;
			}
			Service.Get<QuestController>().SatisfyTrigger(trigger);
		}

		public void ReleaseSatisfiedTriggers()
		{
			this.buffering = false;
			QuestController questController = Service.Get<QuestController>();
			int i = 0;
			int count = this.triggerBuffer.Count;
			while (i < count)
			{
				questController.SatisfyTrigger(this.triggerBuffer[i]);
				i++;
			}
			this.triggerBuffer.Clear();
		}

		public void KillAllTriggers()
		{
			this.buffering = false;
			this.triggerBuffer.Clear();
		}

		protected internal SessionStartTriggerParent(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SessionStartTriggerParent)GCHandledObjects.GCHandleToObject(instance)).KillAllTriggers();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SessionStartTriggerParent)GCHandledObjects.GCHandleToObject(instance)).ReleaseSatisfiedTriggers();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SessionStartTriggerParent)GCHandledObjects.GCHandleToObject(instance)).SatisfyTrigger((IStoryTrigger)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
