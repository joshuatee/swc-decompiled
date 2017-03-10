using StaRTS.Main.Controllers;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class YesNoScreen : AlertScreen
	{
		private bool textOnRight;

		private bool bUseCustomKoreanFont;

		private static string yesString = "";

		private static string noString = "";

		public static void StaticReset()
		{
			YesNoScreen.yesString = "";
			YesNoScreen.noString = "";
		}

		public static void ShowModal(string title, string message, bool textOnRight, OnScreenModalResult onModalResult, object modalResultCookie, bool useCustomKoreanFont = false)
		{
			YesNoScreen.ShowModal(title, message, textOnRight, false, false, onModalResult, modalResultCookie, useCustomKoreanFont);
		}

		public static void ShowModal(string title, string message, bool textOnRight, bool allowFUEBackButton, OnScreenModalResult onModalResult, object modalResultCookie, bool useCustomKoreanFont = false)
		{
			YesNoScreen.yesString = "";
			YesNoScreen.noString = "";
			YesNoScreen.CommonShowModal(title, message, textOnRight, false, allowFUEBackButton, false, onModalResult, modalResultCookie, useCustomKoreanFont);
		}

		public static void ShowModal(string title, string message, bool textOnRight, bool centerTitle, bool allowFUEBackButton, OnScreenModalResult onModalResult, object modalResultCookie, bool useCustomKoreanFont = false)
		{
			YesNoScreen.yesString = "";
			YesNoScreen.noString = "";
			YesNoScreen.CommonShowModal(title, message, textOnRight, centerTitle, allowFUEBackButton, false, onModalResult, modalResultCookie, useCustomKoreanFont);
		}

		public static void ShowModal(string title, string message, bool textOnRight, bool centerTitle, bool allowFUEBackButton, bool alwaysOnTop, OnScreenModalResult onModalResult, object modalResultCookie, bool useCustomKoreanFont = false)
		{
			YesNoScreen.yesString = "";
			YesNoScreen.noString = "";
			YesNoScreen.CommonShowModal(title, message, textOnRight, centerTitle, allowFUEBackButton, alwaysOnTop, onModalResult, modalResultCookie, useCustomKoreanFont);
		}

		public static void ShowModal(string title, string message, bool textOnRight, string confirmString, string gobackString, OnScreenModalResult onModalResult, object modalResultCookie, bool useCustomKoreanFont = false)
		{
			YesNoScreen.yesString = confirmString;
			YesNoScreen.noString = gobackString;
			YesNoScreen.CommonShowModal(title, message, textOnRight, false, false, false, onModalResult, modalResultCookie, useCustomKoreanFont);
		}

		private static void CommonShowModal(string title, string message, bool textOnRight, bool centerTitle, bool allowFUEBackButton, bool alwaysOnTop, OnScreenModalResult onModalResult, object modalResultCookie, bool useCustomKoreanFont = false)
		{
			YesNoScreen yesNoScreen = new YesNoScreen(title, message, textOnRight, alwaysOnTop, useCustomKoreanFont);
			yesNoScreen.centerTitle = centerTitle;
			yesNoScreen.OnModalResult = onModalResult;
			yesNoScreen.ModalResultCookie = modalResultCookie;
			yesNoScreen.AllowFUEBackButton = allowFUEBackButton;
			Service.Get<ScreenController>().AddScreen(yesNoScreen);
		}

		private YesNoScreen(string title, string message, bool textOnRight, bool alwaysOnTop, bool useCustomKoreanFont = false) : base(false, title, message, null, false)
		{
			this.textOnRight = textOnRight;
			base.IsAlwaysOnTop = alwaysOnTop;
			this.bUseCustomKoreanFont = useCustomKoreanFont;
		}

		protected override void SetupControls()
		{
			base.GetElement<UXLabel>("TickerDialogSmall").Visible = false;
			this.primary2OptionButton.Visible = true;
			this.primary2OptionButton.Tag = true;
			this.primary2OptionButton.OnClicked = new UXButtonClickedDelegate(this.OnYesOrNoButtonClicked);
			this.secondary2OptionButton.Visible = true;
			this.secondary2OptionButton.Tag = null;
			this.secondary2OptionButton.OnClicked = new UXButtonClickedDelegate(this.OnYesOrNoButtonClicked);
			if (!string.IsNullOrEmpty(YesNoScreen.yesString))
			{
				this.primary2Option.Text = YesNoScreen.yesString;
			}
			else
			{
				this.primary2Option.Text = this.lang.Get("YES", new object[0]);
			}
			if (!string.IsNullOrEmpty(YesNoScreen.noString))
			{
				this.secondary2Option.Text = YesNoScreen.noString;
			}
			else
			{
				this.secondary2Option.Text = this.lang.Get("NO", new object[0]);
			}
			this.titleLabel.Text = this.title;
			if (this.textOnRight)
			{
				this.rightLabel.Text = this.message;
				if (this.bUseCustomKoreanFont)
				{
					this.rightLabel.Font = Service.Get<Lang>().CustomKoreanFont;
				}
			}
			else
			{
				this.centerLabel.Text = this.message;
				if (this.bUseCustomKoreanFont)
				{
					this.centerLabel.Font = Service.Get<Lang>().CustomKoreanFont;
				}
			}
			if (!base.IsFatal && Service.IsSet<UserInputInhibitor>())
			{
				Service.Get<UserInputInhibitor>().AddToAllow(this.primary2OptionButton);
				Service.Get<UserInputInhibitor>().AddToAllow(this.secondary2OptionButton);
			}
		}

		private void OnYesOrNoButtonClicked(UXButton button)
		{
			button.Enabled = false;
			this.Close(button.Tag);
		}

		protected internal YesNoScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			YesNoScreen.CommonShowModal(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0, *(sbyte*)(args + 4) != 0, *(sbyte*)(args + 5) != 0, (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[6]), GCHandledObjects.GCHandleToObject(args[7]), *(sbyte*)(args + 8) != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((YesNoScreen)GCHandledObjects.GCHandleToObject(instance)).OnYesOrNoButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((YesNoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupControls();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			YesNoScreen.ShowModal(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0, (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[3]), GCHandledObjects.GCHandleToObject(args[4]), *(sbyte*)(args + 5) != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			YesNoScreen.ShowModal(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0, (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[4]), GCHandledObjects.GCHandleToObject(args[5]), *(sbyte*)(args + 6) != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			YesNoScreen.ShowModal(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0, *(sbyte*)(args + 4) != 0, (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[5]), GCHandledObjects.GCHandleToObject(args[6]), *(sbyte*)(args + 7) != 0);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			YesNoScreen.ShowModal(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[5]), GCHandledObjects.GCHandleToObject(args[6]), *(sbyte*)(args + 7) != 0);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			YesNoScreen.ShowModal(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0, *(sbyte*)(args + 4) != 0, *(sbyte*)(args + 5) != 0, (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[6]), GCHandledObjects.GCHandleToObject(args[7]), *(sbyte*)(args + 8) != 0);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			YesNoScreen.StaticReset();
			return -1L;
		}
	}
}
