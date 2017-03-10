using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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

		protected internal ActivateMissionStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ActivateMissionStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ActivateMissionStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
