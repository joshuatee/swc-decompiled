using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class MainFUEGateStoryAction : AbstractStoryAction
	{
		private string reactionUID;

		public override string Reaction
		{
			get
			{
				return this.reactionUID;
			}
		}

		public MainFUEGateStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
			this.reactionUID = string.Empty;
			string.IsNullOrEmpty(vo.Reaction);
			if (!Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
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

		protected internal MainFUEGateStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((MainFUEGateStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MainFUEGateStoryAction)GCHandledObjects.GCHandleToObject(instance)).Reaction);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((MainFUEGateStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
