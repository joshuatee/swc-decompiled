using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class AutoStoryTrigger : AbstractStoryTrigger
	{
		public const string REQ_IF_PREF = "IF_PREF";

		public const string REQ_RESUME_STORY = "RESUME";

		private string savePrefName;

		private string saveValueReq;

		public AutoStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
			if (string.IsNullOrEmpty(vo.PrepareString))
			{
				Service.Get<StaRTSLogger>().Error("AutoStoryTrigger: " + vo.Uid + " is missing prepare string");
			}
			if (this.prepareArgs.Length < 3)
			{
				Service.Get<StaRTSLogger>().Error("AutoStoryTrigger: " + vo.Uid + " doesn't have enough arguments");
			}
			this.savePrefName = this.prepareArgs[1];
			this.saveValueReq = this.prepareArgs[2];
		}

		public override void Activate()
		{
			base.Activate();
			this.parent.SatisfyTrigger(this);
		}

		public override bool IsPreSatisfied()
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			string pref = sharedPlayerPrefs.GetPref<string>(this.savePrefName);
			return this.saveValueReq.Equals(pref) || (this.saveValueReq.Equals("false") && string.IsNullOrEmpty(pref));
		}

		protected internal AutoStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AutoStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AutoStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).IsPreSatisfied());
		}
	}
}
