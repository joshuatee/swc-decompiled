using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class DeactivateTriggerStoryAction : AbstractStoryAction
	{
		private const int TRIGGER_UID_ARG = 0;

		public DeactivateTriggerStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
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
			string uid = this.prepareArgs[0];
			Service.Get<QuestController>().KillTrigger(uid);
			this.parent.ChildComplete(this);
		}

		protected internal DeactivateTriggerStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DeactivateTriggerStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DeactivateTriggerStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
