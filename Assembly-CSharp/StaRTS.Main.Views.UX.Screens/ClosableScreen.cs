using StaRTS.Main.Views.UX.Elements;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class ClosableScreen : ScreenBase
	{
		protected const string CLOSE_BUTTON = "BtnClose";

		public UXButton CloseButton;

		protected bool allowClose;

		public bool AllowClose
		{
			get
			{
				return this.allowClose;
			}
			set
			{
				this.allowClose = value;
				if (this.CloseButton != null)
				{
					this.CloseButton.Visible = this.allowClose;
				}
			}
		}

		protected ClosableScreen(string assetName) : base(assetName)
		{
			this.allowClose = true;
			this.InitDefaultBackDelegate();
		}

		public void InitDefaultBackDelegate()
		{
			base.CurrentBackDelegate = new UXButtonClickedDelegate(this.HandleClose);
			base.CurrentBackButton = this.CloseButton;
		}

		protected virtual void RefreshScreen()
		{
		}

		public void SetVisibilityAndRefresh(bool visible, bool doRefresh)
		{
			this.Visible = visible;
			if (this.Visible & doRefresh)
			{
				this.RefreshScreen();
			}
		}

		protected virtual void InitButtons()
		{
			this.CloseButton = base.GetElement<UXButton>("BtnClose");
			this.CloseButton.OnClicked = new UXButtonClickedDelegate(this.OnCloseButtonClicked);
			this.CloseButton.Enabled = true;
			base.CurrentBackButton = this.CloseButton;
		}

		protected virtual void HandleClose(UXButton button)
		{
			if (!this.allowClose)
			{
				return;
			}
			this.Close(null);
		}

		protected virtual void OnCloseButtonClicked(UXButton button)
		{
			if (!this.allowClose)
			{
				return;
			}
			if (button != null)
			{
				button.Enabled = false;
			}
			this.HandleClose(button);
		}

		protected internal ClosableScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ClosableScreen)GCHandledObjects.GCHandleToObject(instance)).AllowClose);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ClosableScreen)GCHandledObjects.GCHandleToObject(instance)).HandleClose((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ClosableScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ClosableScreen)GCHandledObjects.GCHandleToObject(instance)).InitDefaultBackDelegate();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ClosableScreen)GCHandledObjects.GCHandleToObject(instance)).OnCloseButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ClosableScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshScreen();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ClosableScreen)GCHandledObjects.GCHandleToObject(instance)).AllowClose = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ClosableScreen)GCHandledObjects.GCHandleToObject(instance)).SetVisibilityAndRefresh(*(sbyte*)args != 0, *(sbyte*)(args + 1) != 0);
			return -1L;
		}
	}
}
