using StaRTS.Externals.Kochava;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Commands.Player.Fue;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class EndFueStoryAction : AbstractStoryAction
	{
		public EndFueStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(0);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<ServerAPI>().Enabled = true;
			Service.Get<ServerAPI>().Sync(new PlayerFueCompleteCommand(new PlayerIdChecksumRequest()));
			Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress = false;
			KochavaPlugin.FireEvent("tutorialComplete", "1");
			this.parent.ChildComplete(this);
		}

		protected internal EndFueStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EndFueStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EndFueStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
