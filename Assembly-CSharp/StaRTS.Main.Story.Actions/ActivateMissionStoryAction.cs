using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Story.Actions
{
	public class ActivateMissionStoryAction : AbstractStoryAction
	{
		private const int CONDITION_UID_ARG = 0;

		private CampaignMissionVO missionVO;

		public ActivateMissionStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(1);
			IDataController dataController = Service.Get<IDataController>();
			this.missionVO = dataController.Get<CampaignMissionVO>(this.prepareArgs[0]);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<CampaignController>().StartMission(this.missionVO);
			this.parent.ChildComplete(this);
		}
	}
}
