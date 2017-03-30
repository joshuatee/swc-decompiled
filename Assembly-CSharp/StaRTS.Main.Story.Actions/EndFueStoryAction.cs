using StaRTS.Externals.Manimal;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Commands.Player.Fue;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;

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
			Kochava.FireEvent("tutorialComplete", "1");
			this.parent.ChildComplete(this);
		}
	}
}
