using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Story;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.RUF.RUFTasks
{
	public class FueRUFTask : AbstractRUFTask
	{
		public FueRUFTask()
		{
			base.Priority = 20;
			base.ShouldProcess = false;
			base.ShouldPurgeQueue = true;
			base.ShouldPlayFromLoadState = true;
			if (Service.Get<CurrentPlayer>().HasNotCompletedFirstFueStep())
			{
				base.ShouldProcess = true;
			}
		}

		public override void Process(bool continueProcessing)
		{
			if (base.ShouldProcess)
			{
				new ActionChain(GameConstants.FUE_QUEST_UID);
				Service.Get<GameStateMachine>().SetState(new IntroCameraState());
			}
			base.Process(continueProcessing);
		}

		protected internal FueRUFTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((FueRUFTask)GCHandledObjects.GCHandleToObject(instance)).Process(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
