using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class DefendBaseStoryAction : AbstractStoryAction
	{
		private const int DEFENSE_ENCOUNTER_UID_ARG = 0;

		private CampaignMissionVO encounterVO;

		public DefendBaseStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(1);
			IDataController dataController = Service.Get<IDataController>();
			this.encounterVO = dataController.Get<CampaignMissionVO>(this.prepareArgs[0]);
			BattleInitializationData data = BattleInitializationData.CreateFromDefensiveCampaignMissionVO(this.encounterVO.Uid);
			BattleStartState.GoToBattleStartState(data, null);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<DefensiveBattleController>().StartDefenseMissionAfterLoadingAssets(this.encounterVO);
			this.parent.ChildComplete(this);
		}

		protected internal DefendBaseStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DefendBaseStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DefendBaseStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
