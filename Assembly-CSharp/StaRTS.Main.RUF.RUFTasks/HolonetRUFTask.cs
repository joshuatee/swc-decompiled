using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Holonet;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.RUF.RUFTasks
{
	public class HolonetRUFTask : AbstractRUFTask
	{
		private List<BattleEntry> newBattles;

		private bool continueProcessing;

		public HolonetRUFTask()
		{
			base.Priority = 100;
			base.ShouldProcess = false;
			base.ShouldPurgeQueue = true;
			this.newBattles = Service.Get<PvpManager>().GetBattlesThatHappenOffline();
			if (this.newBattles.Count > 0 || Service.Get<HolonetController>().CalculateBadgeCount() > 0)
			{
				base.ShouldProcess = true;
			}
		}

		public override void Process(bool continueProcessing)
		{
			this.continueProcessing = continueProcessing;
			if (base.ShouldProcess)
			{
				Service.Get<ViewTimerManager>().CreateViewTimer(2f, false, new TimerDelegate(this.ShowHolonetOnTimer), this.newBattles);
				return;
			}
			base.Process(continueProcessing);
		}

		public void ShowHolonetOnTimer(uint timerId, object data)
		{
			if (!Service.Get<PerkManager>().WillShowPerkTutorial())
			{
				List<BattleEntry> battles = (List<BattleEntry>)data;
				Service.Get<HolonetController>().InitBattlesTransmission(battles);
				Service.Get<HolonetController>().OpenHolonet();
			}
			base.Process(this.continueProcessing);
		}

		protected internal HolonetRUFTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HolonetRUFTask)GCHandledObjects.GCHandleToObject(instance)).Process(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
