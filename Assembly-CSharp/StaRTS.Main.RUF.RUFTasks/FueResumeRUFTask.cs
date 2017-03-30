using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.RUF.RUFTasks
{
	public class FueResumeRUFTask : AbstractRUFTask
	{
		private const string CONTRABAND_TRIGGER_PREFIX = "tut_con_";

		public FueResumeRUFTask()
		{
			base.Priority = 60;
			base.ShouldPurgeQueue = this.CheckPendingTriggersShouldPurge();
			string restoredQuest = Service.Get<CurrentPlayer>().RestoredQuest;
			base.ShouldProcess = (!string.IsNullOrEmpty(restoredQuest) || Service.Get<QuestController>().HasPendingTriggers());
		}

		public override void Process(bool continueProcessing)
		{
			if (base.ShouldProcess)
			{
				Service.Get<QuestController>().RestoreLastQuest();
				Service.Get<QuestController>().StartPendingTriggers();
			}
			base.Process(continueProcessing);
		}

		private bool CheckPendingTriggersShouldPurge()
		{
			QuestController questController = Service.Get<QuestController>();
			return !questController.HasPendingTriggers() || !questController.DoesPendingTriggersContainPrefix("tut_con_");
		}
	}
}
