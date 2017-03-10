using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Utils
{
	public static class DateUtils
	{
		public const int SECONDS_IN_MINUTE = 60;

		public const int SECONDS_IN_HOUR = 3600;

		public const int SECONDS_IN_DAY = 86400;

		private const double SECOND_TO_MILLISECOND = 1000.0;

		private const double MINUTE_TO_MILLISECOND = 60000.0;

		private const double HOUR_TO_MILLISECOND = 3600000.0;

		private static readonly DateTime UnixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, 1);

		public static int GetSecondsFromString(string dateString, int defaultValue)
		{
			if (string.IsNullOrEmpty(dateString))
			{
				Service.Get<StaRTSLogger>().Warn("Missing Date Info!");
				return defaultValue;
			}
			DateTime date = DateTime.ParseExact(dateString, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
			return DateUtils.GetSecondsFromEpoch(date);
		}

		public static TimeSpan GetTimeSpanSinceStartOfDate(DateTime date)
		{
			return TimeSpan.FromMilliseconds((double)date.get_Millisecond() + (double)date.get_Second() * 1000.0 + (double)date.get_Minute() * 60000.0 + (double)date.get_Hour() * 3600000.0);
		}

		public static DateTime GetDefaultDate()
		{
			return default(DateTime);
		}

		public static bool IsDefaultDate(DateTime date)
		{
			return DateUtils.GetDefaultDate().Equals(date);
		}

		public static DateTime DateFromMillis(long millis)
		{
			return new DateTime(DateUtils.UnixStart.get_Ticks() + millis * 10000L);
		}

		public static DateTime DateFromSeconds(uint seconds)
		{
			return DateUtils.DateFromSeconds((int)seconds);
		}

		public static DateTime DateFromSeconds(int seconds)
		{
			return DateUtils.UnixStart.AddSeconds((double)seconds);
		}

		public static float GetRealTimeSinceStartUpInMilliseconds()
		{
			return Mathf.Round(UnityUtils.GetRealTimeSinceStartUp() * 1000f);
		}

		public static int GetMillisFromEpoch(DateTime date)
		{
			return (int)(date - DateUtils.UnixStart).get_TotalMilliseconds();
		}

		public static int GetSecondsFromEpoch(DateTime date)
		{
			return (int)(date - DateUtils.UnixStart).get_TotalSeconds();
		}

		public static double GetNowSecondsPrecise()
		{
			return (DateTime.get_UtcNow() - DateUtils.UnixStart).get_TotalSeconds();
		}

		public static uint GetNowSeconds()
		{
			return (uint)DateUtils.GetSecondsFromEpoch(DateTime.get_UtcNow());
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DateUtils.DateFromMillis(*args));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DateUtils.DateFromSeconds(*(int*)args));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DateUtils.GetDefaultDate());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DateUtils.GetMillisFromEpoch(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DateUtils.GetRealTimeSinceStartUpInMilliseconds());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DateUtils.GetSecondsFromEpoch(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DateUtils.GetSecondsFromString(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DateUtils.GetTimeSpanSinceStartOfDate(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DateUtils.IsDefaultDate(*(*(IntPtr*)args)));
		}
	}
}
