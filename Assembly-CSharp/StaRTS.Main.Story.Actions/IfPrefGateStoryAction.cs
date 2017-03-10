using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class IfPrefGateStoryAction : AbstractStoryAction
	{
		private string reactionUID;

		public override string Reaction
		{
			get
			{
				return this.reactionUID;
			}
		}

		public IfPrefGateStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
			this.reactionUID = "";
			string.IsNullOrEmpty(vo.PrepareString);
			int arg_29_0 = this.prepareArgs.Length;
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			string pref = sharedPlayerPrefs.GetPref<string>(this.prepareArgs[0]);
			string text = this.prepareArgs[1];
			if (text.Equals(pref) || (text.Equals("false") && string.IsNullOrEmpty(pref)))
			{
				this.reactionUID = vo.Reaction;
			}
		}

		public override void Prepare()
		{
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			this.parent.ChildComplete(this);
		}

		protected internal IfPrefGateStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IfPrefGateStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IfPrefGateStoryAction)GCHandledObjects.GCHandleToObject(instance)).Reaction);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IfPrefGateStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
