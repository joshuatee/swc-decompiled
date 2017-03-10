using StaRTS.Externals.Manimal;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class TimerAlertScreen : AlertScreen, IViewClockTimeObserver
	{
		private int endTimeStamp;

		private string timerTextUID;

		public TimerAlertScreen(string titleUID, string timerTextUID, string messageUID, string buttonLabelUID, int endTimeStamp, OnScreenModalResult onModalResult) : base(false, string.Empty, string.Empty, string.Empty, false)
		{
			this.endTimeStamp = endTimeStamp;
			this.timerTextUID = timerTextUID;
			this.title = this.lang.Get(titleUID, new object[0]);
			this.message = this.lang.Get(messageUID, new object[0]);
			this.primaryLabelOverride = this.lang.Get(buttonLabelUID, new object[0]);
			base.OnModalResult = onModalResult;
		}

		protected override void SetupControls()
		{
			base.SetupControls();
			this.labelTimer.Visible = true;
			this.UpdateTimeText();
			Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
		}

		private void UpdateTimeText()
		{
			uint serverTime = Service.Get<ServerAPI>().ServerTime;
			int num = this.endTimeStamp - (int)serverTime;
			string text = string.Empty;
			if (!string.IsNullOrEmpty(this.timerTextUID))
			{
				text = this.lang.Get(this.timerTextUID, new object[]
				{
					LangUtils.FormatTime((long)num)
				});
			}
			else
			{
				text = LangUtils.FormatTime((long)num);
			}
			this.labelTimer.Text = text;
		}

		public void OnViewClockTime(float dt)
		{
			this.UpdateTimeText();
		}

		public override void OnDestroyElement()
		{
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
			base.OnDestroyElement();
		}

		protected internal TimerAlertScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TimerAlertScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TimerAlertScreen)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TimerAlertScreen)GCHandledObjects.GCHandleToObject(instance)).SetupControls();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TimerAlertScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateTimeText();
			return -1L;
		}
	}
}
