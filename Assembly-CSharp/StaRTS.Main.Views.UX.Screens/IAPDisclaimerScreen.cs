using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class IAPDisclaimerScreen : AlertScreen
	{
		public IAPDisclaimerScreen(OnScreenModalResult onModalResult) : base(true, null, null, null, false)
		{
			base.OnModalResult = onModalResult;
			this.title = this.lang.Get("IAP_DISCLAIMER_TITLE", new object[0]);
			this.message = this.lang.Get("IAP_DISCLAIMER_DESC", new object[0]);
		}

		protected override void SetupControls()
		{
			base.SetupControls();
			this.primaryButton.OnClicked = new UXButtonClickedDelegate(this.OnButtonClicked);
			this.primaryLabel.Text = this.lang.Get("OK", new object[0]);
			Service.Get<EventManager>().SendEvent(EventId.UIIAPDisclaimerViewed, null);
		}

		private void OnButtonClicked(UXButton button)
		{
			this.Close(true);
			Service.Get<EventManager>().SendEvent(EventId.UIIAPDisclaimerClosed, null);
		}

		protected internal IAPDisclaimerScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IAPDisclaimerScreen)GCHandledObjects.GCHandleToObject(instance)).OnButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IAPDisclaimerScreen)GCHandledObjects.GCHandleToObject(instance)).SetupControls();
			return -1L;
		}
	}
}
