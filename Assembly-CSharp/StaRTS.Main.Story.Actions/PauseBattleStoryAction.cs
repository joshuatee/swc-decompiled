using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;

namespace StaRTS.Main.Story.Actions
{
	public class PauseBattleStoryAction : AbstractStoryAction
	{
		public PauseBattleStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
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
			Service.Get<SimTimeEngine>().ScaleTime(0u);
			this.parent.ChildComplete(this);
		}
	}
}
