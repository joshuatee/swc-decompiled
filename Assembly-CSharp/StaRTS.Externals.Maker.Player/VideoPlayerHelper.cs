using StaRTS.Main.Models;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.Maker.Player
{
	public class VideoPlayerHelper : IVideoPlayerHelper
	{
		public VideoPlayerHelper()
		{
		}

		public void SetAdTagURL(string url)
		{
		}

		public void SetAdLanguage(string language)
		{
		}

		public void SetAdZoneId(string zoneId)
		{
		}

		public void EnableAds(bool enable)
		{
		}

		public void SetLoadingText(string text)
		{
		}

		public void SetLoadingTextTime(float loadingTextTime)
		{
		}

		public void SetDoneButtonText(string text)
		{
		}

		public void Play(string videoURL, bool isOfficial, string videoId, string action)
		{
			if (!GameConstants.IsMakerVideoEnabled())
			{
				return;
			}
			VideoPlayerKeepAlive.Instance.EndPlayback("0");
		}

		protected internal VideoPlayerHelper(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).EnableAds(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).Play(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((VideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).SetAdLanguage(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((VideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).SetAdTagURL(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((VideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).SetAdZoneId(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((VideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).SetDoneButtonText(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((VideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).SetLoadingText(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((VideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).SetLoadingTextTime(*(float*)args);
			return -1L;
		}
	}
}
