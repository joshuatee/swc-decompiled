using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class DelayStoryAction : AbstractStoryAction
	{
		private const int TIME_SECONDS_ARG = 0;

		public DelayStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
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
			float delay = Convert.ToSingle(this.prepareArgs[0], CultureInfo.InvariantCulture);
			Service.Get<ViewTimerManager>().CreateViewTimer(delay, false, new TimerDelegate(this.OnComplete), null);
		}

		private void OnComplete(uint id, object cookie)
		{
			this.parent.ChildComplete(this);
		}

		protected internal DelayStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DelayStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DelayStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
