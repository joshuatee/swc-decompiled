using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Commands.Player.Fue;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Story.Actions
{
	public class ActivateTriggerStoryAction : AbstractStoryAction
	{
		private const int TRIGGER_UID_ARG = 0;

		private bool save;

		public ActivateTriggerStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(1);
			this.save = (this.vo.ActionType == "ActivateSaveTrigger");
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<QuestController>().ActivateTrigger(this.prepareArgs[0], this.save);
			if (this.save)
			{
				Service.Get<ServerAPI>().Enqueue(new FueUpdateStateCommand());
			}
			this.parent.ChildComplete(this);
		}
	}
}
