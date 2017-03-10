using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class ProcessingScreen : ScreenBase
	{
		private static ProcessingScreen instance;

		private static bool showing;

		private static bool loaded;

		private static bool added;

		public static void Show()
		{
			ProcessingScreen.showing = true;
			if (ProcessingScreen.instance == null)
			{
				ProcessingScreen.instance = new ProcessingScreen();
			}
		}

		public static void Hide()
		{
			ProcessingScreen.showing = false;
			if (ProcessingScreen.instance != null)
			{
				ProcessingScreen.instance.RefreshView();
			}
		}

		public static void StaticReset()
		{
			ProcessingScreen.instance = null;
			ProcessingScreen.loaded = false;
			ProcessingScreen.showing = false;
			ProcessingScreen.added = false;
		}

		public ProcessingScreen() : base("gui_loading_small_anim")
		{
		}

		protected override void OnScreenLoaded()
		{
			ProcessingScreen.loaded = true;
			if (ProcessingScreen.instance != null)
			{
				ProcessingScreen.instance.RefreshView();
			}
		}

		public override void RefreshView()
		{
			if (ProcessingScreen.loaded && ProcessingScreen.showing && !ProcessingScreen.added)
			{
				ProcessingScreen.added = true;
				Service.Get<ScreenController>().AddScreen(ProcessingScreen.instance);
				return;
			}
			if (!ProcessingScreen.showing)
			{
				ProcessingScreen.loaded = false;
				ProcessingScreen.instance.Close(false);
				ProcessingScreen.instance = null;
				ProcessingScreen.added = false;
			}
		}

		protected internal ProcessingScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			ProcessingScreen.Hide();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ProcessingScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ProcessingScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			ProcessingScreen.Show();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			ProcessingScreen.StaticReset();
			return -1L;
		}
	}
}
