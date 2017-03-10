using StaRTS.Externals.Maker.Player;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType451 : $UnityType
	{
		public unsafe $UnityType451()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 451888) = ldftn($Invoke0);
			*(data + 451916) = ldftn($Invoke1);
			*(data + 451944) = ldftn($Invoke2);
			*(data + 451972) = ldftn($Invoke3);
			*(data + 452000) = ldftn($Invoke4);
			*(data + 452028) = ldftn($Invoke5);
			*(data + 452056) = ldftn($Invoke6);
			*(data + 452084) = ldftn($Invoke7);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IVideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).EnableAds(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IVideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).Play(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IVideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).SetAdLanguage(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((IVideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).SetAdTagURL(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((IVideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).SetAdZoneId(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((IVideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).SetDoneButtonText(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((IVideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).SetLoadingText(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((IVideoPlayerHelper)GCHandledObjects.GCHandleToObject(instance)).SetLoadingTextTime(*(float*)args);
			return -1L;
		}
	}
}
