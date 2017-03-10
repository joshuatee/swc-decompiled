using StaRTS.Externals.DMOAnalytics;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType385 : $UnityType
	{
		public unsafe $UnityType385()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 437272) = ldftn($Invoke0);
			*(data + 437300) = ldftn($Invoke1);
			*(data + 437328) = ldftn($Invoke2);
			*(data + 437356) = ldftn($Invoke3);
			*(data + 437384) = ldftn($Invoke4);
			*(data + 437412) = ldftn($Invoke5);
			*(data + 437440) = ldftn($Invoke6);
			*(data + 437468) = ldftn($Invoke7);
			*(data + 437496) = ldftn($Invoke8);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).FlushAnalyticsQueue();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogAppBackground();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogAppEnd();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((IDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogAppForeground();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((IDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogAppStart();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((IDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogEvent(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((IDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).LogEventWithContext(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((IDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).SetCanUseNetwork(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((IDMOAnalyticsManager)GCHandledObjects.GCHandleToObject(instance)).SetDebugLogging(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
