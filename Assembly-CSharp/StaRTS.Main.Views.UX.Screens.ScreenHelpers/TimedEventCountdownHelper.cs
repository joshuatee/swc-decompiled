using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers
{
	public class TimedEventCountdownHelper : IViewClockTimeObserver
	{
		private const string CAMPAIGN_BEGINS = "CAMPAIGN_BEGINS_IN";

		private const string CAMPAIGN_ENDS = "CAMPAIGN_ENDS_IN";

		private const string REWARD_DURATION = "REWARD_DURATION";

		private UXLabel label;

		private Lang lang;

		private string timerColor;

		public ITimedEventVO Campaign
		{
			get;
			set;
		}

		public string TimerColor
		{
			get
			{
				return this.timerColor;
			}
			set
			{
				this.timerColor = value;
				if (this.Campaign != null)
				{
					this.UpdateTimeRemaining();
				}
			}
		}

		public TimedEventCountdownHelper(UXLabel label, ITimedEventVO campaign)
		{
			this.label = label;
			this.Campaign = campaign;
			this.lang = Service.Get<Lang>();
			Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
			if (this.Campaign != null)
			{
				this.UpdateTimeRemaining();
			}
		}

		public void OnViewClockTime(float dt)
		{
			if (this.Campaign == null)
			{
				return;
			}
			this.UpdateTimeRemaining();
		}

		private void UpdateTimeRemaining()
		{
			TimedEventState state = TimedEventUtils.GetState(this.Campaign);
			string text = "";
			switch (state)
			{
			case TimedEventState.Upcoming:
			{
				int num = TimedEventUtils.GetSecondsRemaining(this.Campaign);
				string text2 = LangUtils.FormatTime((long)num);
				text = this.lang.Get("CAMPAIGN_BEGINS_IN", new object[]
				{
					text2
				});
				break;
			}
			case TimedEventState.Live:
			{
				int num = TimedEventUtils.GetSecondsRemaining(this.Campaign);
				string text2 = LangUtils.FormatTime((long)num);
				text = this.lang.Get("CAMPAIGN_ENDS_IN", new object[]
				{
					text2
				});
				break;
			}
			case TimedEventState.Closing:
			{
				int num = TimedEventUtils.GetStoreSecondsRemaining(this.Campaign);
				string text2 = LangUtils.FormatTime((long)num);
				text = this.lang.Get("REWARD_DURATION", new object[]
				{
					text2
				});
				break;
			}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.label.Text = UXUtils.WrapTextInColor(text, this.timerColor);
			}
		}

		public void Destroy()
		{
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
			this.label = null;
			this.Campaign = null;
			this.lang = null;
		}

		protected internal TimedEventCountdownHelper(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TimedEventCountdownHelper)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TimedEventCountdownHelper)GCHandledObjects.GCHandleToObject(instance)).Campaign);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TimedEventCountdownHelper)GCHandledObjects.GCHandleToObject(instance)).TimerColor);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TimedEventCountdownHelper)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TimedEventCountdownHelper)GCHandledObjects.GCHandleToObject(instance)).Campaign = (ITimedEventVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TimedEventCountdownHelper)GCHandledObjects.GCHandleToObject(instance)).TimerColor = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TimedEventCountdownHelper)GCHandledObjects.GCHandleToObject(instance)).UpdateTimeRemaining();
			return -1L;
		}
	}
}
