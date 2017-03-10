using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.RUF.RUFTasks
{
	public class GoToHomeStateRUFTask : AbstractRUFTask
	{
		private bool continueProcessing;

		public GoToHomeStateRUFTask()
		{
			base.Priority = 50;
			base.ShouldProcess = true;
			base.ShouldPurgeQueue = false;
			base.ShouldPlayFromLoadState = true;
		}

		public override void Process(bool continueProcessing)
		{
			this.continueProcessing = continueProcessing;
			if (base.ShouldProcess)
			{
				Service.Get<RUFManager>().UpdateProcessingLoadState(false);
				if (Service.Get<GameStateMachine>().CurrentState is HomeState)
				{
					base.Process(continueProcessing);
					return;
				}
				HomeState.GoToHomeState(new TransitionCompleteDelegate(this.OnHomeLoaded), true);
			}
		}

		private void OnHomeLoaded()
		{
			base.Process(this.continueProcessing);
		}

		protected internal GoToHomeStateRUFTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((GoToHomeStateRUFTask)GCHandledObjects.GCHandleToObject(instance)).OnHomeLoaded();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GoToHomeStateRUFTask)GCHandledObjects.GCHandleToObject(instance)).Process(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
