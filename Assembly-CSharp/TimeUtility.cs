using System;
using System.Globalization;
using WinRTBridge;

public static class TimeUtility
{
	public static string ToShortDateString(this DateTime dt)
	{
		return dt.ToString(DateTimeFormatInfo.CurrentInfo.ShortDatePattern);
	}

	public static string ToLongDateString(this DateTime dt)
	{
		return dt.ToString(DateTimeFormatInfo.CurrentInfo.LongDatePattern);
	}

	public static string ToLongTimeString(this DateTime dt)
	{
		return dt.ToString(DateTimeFormatInfo.CurrentInfo.LongTimePattern);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle((*(*(IntPtr*)args)).ToLongDateString());
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle((*(*(IntPtr*)args)).ToLongTimeString());
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle((*(*(IntPtr*)args)).ToShortDateString());
	}
}
