using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class ApplePromoScreen : AlertScreen
	{
		private const string OK_BUTTON_TEXT = "OK";

		private const string APPLE_PROMO_SCREEN_TITLE = "APPLE_PROMO_SCREEN_TITLE";

		private const string APPLE_PROMO_SCREEN_DESC = "APPLE_PROMO_SCREEN_DESC";

		private object modalResult;

		public ApplePromoScreen(OnScreenModalResult onModalResult, object result) : base(true, null, null, null, false)
		{
			base.OnModalResult = onModalResult;
			this.modalResult = result;
			this.title = this.lang.Get("APPLE_PROMO_SCREEN_TITLE", new object[0]);
			this.message = this.lang.Get("APPLE_PROMO_SCREEN_DESC", new object[0]);
		}

		protected override void SetupControls()
		{
			base.SetupControls();
			this.primaryButton.OnClicked = new UXButtonClickedDelegate(this.OnButtonClicked);
			this.primaryLabel.Text = this.lang.Get("OK", new object[0]);
		}

		private void OnButtonClicked(UXButton button)
		{
			this.Close(this.modalResult);
		}

		public override void OnDestroyElement()
		{
			base.OnDestroyElement();
			Service.Get<UserInputManager>().Enable(true);
		}

		protected internal ApplePromoScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ApplePromoScreen)GCHandledObjects.GCHandleToObject(instance)).OnButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ApplePromoScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ApplePromoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupControls();
			return -1L;
		}
	}
}
