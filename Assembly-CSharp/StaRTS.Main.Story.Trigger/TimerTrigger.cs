using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class TimerTrigger : AbstractStoryTrigger
	{
		private const int TIME_SECONDS_ARG = 0;

		private uint timerId;

		public TimerTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public override void Activate()
		{
			base.Activate();
			float delay = Convert.ToSingle(this.prepareArgs[0], CultureInfo.InvariantCulture);
			this.timerId = Service.Get<ViewTimerManager>().CreateViewTimer(delay, false, new TimerDelegate(this.OnComplete), null);
		}

		public override void Destroy()
		{
			Service.Get<ViewTimerManager>().KillViewTimer(this.timerId);
			base.Destroy();
		}

		private void OnComplete(uint id, object cookie)
		{
			this.parent.SatisfyTrigger(this);
		}

		protected internal TimerTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TimerTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TimerTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}
	}
}
