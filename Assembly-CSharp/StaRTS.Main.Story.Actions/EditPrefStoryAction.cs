using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class EditPrefStoryAction : AbstractStoryAction
	{
		private const string TYPE_SET_PREF = "SET_PREF";

		private const string TYPE_CLEAR_PREF = "CLEAR_PREF";

		private string savePrefType;

		private string savePrefName;

		private string saveValue;

		public EditPrefStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.Prepare();
			if (string.IsNullOrEmpty(this.vo.PrepareString))
			{
				Service.Get<StaRTSLogger>().Error("EditVariableStoryAction: Missing Save Variable and Value in PrepareString");
			}
			if (this.prepareArgs.Length < 2)
			{
				Service.Get<StaRTSLogger>().Error("EditVariableStoryAction: Not enough params in PrepareString");
				return;
			}
			this.savePrefType = this.prepareArgs[0];
			this.savePrefName = this.prepareArgs[1];
			if (this.savePrefType == "SET_PREF")
			{
				this.saveValue = this.prepareArgs[2];
			}
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			if (this.savePrefType == "SET_PREF")
			{
				AutoStoryTriggerUtils.SaveTriggerValue(this.savePrefName, this.saveValue);
			}
			else if (this.savePrefType == "CLEAR_PREF")
			{
				AutoStoryTriggerUtils.ClearTriggerValue(this.savePrefName);
			}
			this.parent.ChildComplete(this);
		}

		protected internal EditPrefStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EditPrefStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EditPrefStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
