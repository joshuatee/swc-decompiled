using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class RebelEmpireForkingStoryAction : AbstractStoryAction
	{
		private string reactionUID;

		public override string Reaction
		{
			get
			{
				return this.reactionUID;
			}
		}

		public RebelEmpireForkingStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
			char[] array = new char[]
			{
				'|'
			};
			string[] array2 = vo.PrepareString.Split(array, 0);
			if (array2.Length < 2)
			{
				Service.Get<StaRTSLogger>().Error("RebelEmpireForkingStoryAction lacking params: " + this.vo.Uid);
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (currentPlayer.CampaignProgress.FueInProgress)
			{
				Service.Get<StaRTSLogger>().Error("Cannot do forking actions in FUE only later guided experiences");
				this.Execute();
			}
			FactionType faction = currentPlayer.Faction;
			if (faction == FactionType.Rebel)
			{
				this.reactionUID = array2[0];
				return;
			}
			this.reactionUID = array2[1];
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

		protected internal RebelEmpireForkingStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((RebelEmpireForkingStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RebelEmpireForkingStoryAction)GCHandledObjects.GCHandleToObject(instance)).Reaction);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((RebelEmpireForkingStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
