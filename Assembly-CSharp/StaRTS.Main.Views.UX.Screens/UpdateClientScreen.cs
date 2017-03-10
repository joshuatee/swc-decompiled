using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class UpdateClientScreen : AlertScreen
	{
		public static void ShowModal(string title, string message, OnScreenModalResult onModalResult, object modalResultCookie)
		{
			UpdateClientScreen updateClientScreen = new UpdateClientScreen(title, message);
			updateClientScreen.OnModalResult = onModalResult;
			updateClientScreen.ModalResultCookie = modalResultCookie;
		}

		private UpdateClientScreen(string title, string message) : base(true, title, message, null, false)
		{
		}

		protected override void SetupControls()
		{
			base.SetupControls();
			this.primaryLabel.Text = this.lang.Get("FORCED_UPDATE_BUTTON", new object[0]);
		}

		protected internal UpdateClientScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UpdateClientScreen)GCHandledObjects.GCHandleToObject(instance)).SetupControls();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			UpdateClientScreen.ShowModal(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[2]), GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}
	}
}
