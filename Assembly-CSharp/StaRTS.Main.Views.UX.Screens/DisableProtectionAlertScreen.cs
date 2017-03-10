using StaRTS.Main.Controllers;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Text;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class DisableProtectionAlertScreen : AlertScreen, IViewClockTimeObserver
	{
		public static void ShowModal(OnScreenModalResult onModalResult, object modalResultCookie)
		{
			DisableProtectionAlertScreen disableProtectionAlertScreen = new DisableProtectionAlertScreen();
			disableProtectionAlertScreen.OnModalResult = onModalResult;
			disableProtectionAlertScreen.ModalResultCookie = modalResultCookie;
			Service.Get<ScreenController>().AddScreen(disableProtectionAlertScreen);
		}

		protected DisableProtectionAlertScreen() : base(false, null, DisableProtectionAlertScreen.GetProtectionTimeRemaining(), UXUtils.GetCurrencyItemAssetName("protection"), false)
		{
			Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
		}

		public override void OnDestroyElement()
		{
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
			base.OnDestroyElement();
		}

		public void OnViewClockTime(float dt)
		{
			if (this.rightLabel != null)
			{
				this.rightLabel.Text = DisableProtectionAlertScreen.GetProtectionTimeRemaining();
			}
		}

		private static string GetProtectionTimeRemaining()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int protectionTimeRemaining = GameUtils.GetProtectionTimeRemaining();
			if (protectionTimeRemaining > 0)
			{
				stringBuilder.Append(Service.Get<Lang>().Get("PROTECTION_REMAINING", new object[0]));
				stringBuilder.Append(GameUtils.GetTimeLabelFromSeconds(protectionTimeRemaining));
				stringBuilder.Append("\n");
			}
			stringBuilder.Append(Service.Get<Lang>().Get("PROTECTION_INVALIDATE", new object[0]));
			return stringBuilder.ToString();
		}

		protected override void SetupControls()
		{
			base.GetElement<UXLabel>("TickerDialogSmall").Visible = false;
			this.rightLabel.Text = DisableProtectionAlertScreen.GetProtectionTimeRemaining();
			this.titleLabel.Text = this.lang.Get("ALERT", new object[0]);
			this.primary2Option.Text = this.lang.Get("YES", new object[0]);
			this.primary2OptionButton.Visible = true;
			this.primary2OptionButton.Tag = true;
			this.primary2OptionButton.OnClicked = new UXButtonClickedDelegate(this.OnYesOrNoButtonClicked);
			this.secondary2Option.Text = this.lang.Get("NO", new object[0]);
			this.secondary2OptionButton.Visible = true;
			this.secondary2OptionButton.Tag = null;
			this.secondary2OptionButton.OnClicked = new UXButtonClickedDelegate(this.OnYesOrNoButtonClicked);
			if (!string.IsNullOrEmpty(this.spriteName))
			{
				UXUtils.SetupGeometryForIcon(this.sprite, this.spriteName);
			}
		}

		private void OnYesOrNoButtonClicked(UXButton button)
		{
			button.Enabled = false;
			this.Close(button.Tag);
		}

		protected internal DisableProtectionAlertScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DisableProtectionAlertScreen.GetProtectionTimeRemaining());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DisableProtectionAlertScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DisableProtectionAlertScreen)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DisableProtectionAlertScreen)GCHandledObjects.GCHandleToObject(instance)).OnYesOrNoButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((DisableProtectionAlertScreen)GCHandledObjects.GCHandleToObject(instance)).SetupControls();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			DisableProtectionAlertScreen.ShowModal((OnScreenModalResult)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
