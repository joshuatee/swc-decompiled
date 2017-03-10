using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

namespace StaRTS.Main.RUF.RUFTasks
{
	public class CallsignRUFTask : AbstractRUFTask
	{
		private bool continueProcessing;

		public CallsignRUFTask()
		{
			base.Priority = 30;
			base.ShouldProcess = false;
			base.ShouldPurgeQueue = false;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (currentPlayer.CampaignProgress.FueInProgress && currentPlayer.NumIdentities == 1)
			{
				base.ShouldProcess = false;
				return;
			}
			if (currentPlayer.PlayerNameInvalid)
			{
				base.ShouldProcess = true;
			}
		}

		public override void Process(bool continueProcessing)
		{
			this.continueProcessing = continueProcessing;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (base.ShouldProcess)
			{
				if (!currentPlayer.CampaignProgress.FueInProgress)
				{
					Service.Get<ViewTimerManager>().CreateViewTimer(2f, false, new TimerDelegate(this.ShowCallsignOnTimer), null);
					return;
				}
			}
			else
			{
				base.Process(continueProcessing);
			}
		}

		private void ShowCallsignOnTimer(uint timerId, object data)
		{
			SetCallsignScreen setCallsignScreen = new SetCallsignScreen(true);
			setCallsignScreen.OnModalResult = new OnScreenModalResult(this.OnScreenModalResult);
			Service.Get<ScreenController>().AddScreen(setCallsignScreen);
		}

		private void OnScreenModalResult(object result, object cookie)
		{
			base.Process(this.continueProcessing);
		}

		protected internal CallsignRUFTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CallsignRUFTask)GCHandledObjects.GCHandleToObject(instance)).OnScreenModalResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CallsignRUFTask)GCHandledObjects.GCHandleToObject(instance)).Process(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
