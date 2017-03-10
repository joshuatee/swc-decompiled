using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class UnderAttackScreen : ScreenBase, IViewClockTimeObserver
	{
		private const string LABEL_TITLE = "LabelTitle";

		private const string LABEL_DESCRIPTION = "LabelBody";

		private const string LABEL_TIME_LEFT = "LabelTimeLeft";

		private const string LABEL_TIME_LEFT_COUNT = "LabelTimeLeftCount";

		private int totalSecondsLeft;

		private int intervalSecondsLeft;

		private UXLabel timeCountLabel;

		public UnderAttackScreen(uint expiration) : base("gui_battle_underattack")
		{
			Service.Get<CameraManager>().SetCameraOrderForPreloadScreens();
			this.intervalSecondsLeft = GameConstants.UNDER_ATTACK_STATUS_CHECK_INTERVAL;
			if (this.intervalSecondsLeft == 0)
			{
				this.intervalSecondsLeft = 15;
			}
			if (expiration > ServerTime.Time)
			{
				this.totalSecondsLeft = (int)(expiration - ServerTime.Time);
				return;
			}
			this.totalSecondsLeft = GameConstants.PVP_MATCH_DURATION;
		}

		protected override void OnScreenLoaded()
		{
			UXLabel element = base.GetElement<UXLabel>("LabelTitle");
			element.Text = this.lang.Get("PVP_YOU_ARE_UNDER_ATTACK", new object[0]);
			UXLabel element2 = base.GetElement<UXLabel>("LabelBody");
			element2.Text = LangUtils.ProcessStringWithNewlines(this.lang.Get("PVP_WAIT_MESSAGE", new object[0]));
			UXLabel element3 = base.GetElement<UXLabel>("LabelTimeLeft");
			element3.Text = this.lang.Get("s_TimeRemaining", new object[0]);
			this.timeCountLabel = base.GetElement<UXLabel>("LabelTimeLeftCount");
			this.timeCountLabel.Text = GameUtils.GetTimeLabelFromSeconds(this.totalSecondsLeft);
			Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
		}

		public void OnViewClockTime(float dt)
		{
			int num = this.intervalSecondsLeft - 1;
			this.intervalSecondsLeft = num;
			if (num < 1)
			{
				GetPlayerPvpStatusCommand command = new GetPlayerPvpStatusCommand(new PlayerIdRequest
				{
					PlayerId = Service.Get<CurrentPlayer>().PlayerId
				});
				Service.Get<ServerAPI>().Sync(command);
				this.intervalSecondsLeft = GameConstants.UNDER_ATTACK_STATUS_CHECK_INTERVAL;
				if (this.intervalSecondsLeft == 0)
				{
					this.intervalSecondsLeft = 15;
					return;
				}
			}
			else
			{
				num = this.totalSecondsLeft - 1;
				this.totalSecondsLeft = num;
				if (num < 1)
				{
					Service.Get<Engine>().Reload();
					this.Close(null);
					return;
				}
				this.timeCountLabel.Text = GameUtils.GetTimeLabelFromSeconds(this.totalSecondsLeft);
			}
		}

		public override void OnDestroyElement()
		{
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
		}

		protected internal UnderAttackScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UnderAttackScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((UnderAttackScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((UnderAttackScreen)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}
	}
}
