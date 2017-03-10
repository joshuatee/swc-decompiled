using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.MetaData;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class WarScheduleVO : IValueObject
	{
		public static int COLUMN_warPlanetUid
		{
			get;
			private set;
		}

		public static int COLUMN_startDate
		{
			get;
			private set;
		}

		public static int COLUMN_endDate
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string WarPlanetUid
		{
			get;
			set;
		}

		public int StartTime
		{
			get;
			set;
		}

		public int EndTime
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.WarPlanetUid = row.TryGetString(WarScheduleVO.COLUMN_warPlanetUid);
			string text = row.TryGetString(WarScheduleVO.COLUMN_startDate);
			if (!string.IsNullOrEmpty(text))
			{
				try
				{
					DateTime date = DateTime.ParseExact(text, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
					this.StartTime = DateUtils.GetSecondsFromEpoch(date);
					goto IL_95;
				}
				catch (Exception)
				{
					this.StartTime = 0;
					Service.Get<StaRTSLogger>().Warn("WarScheduleVO:: War Schedule CMS Start Date Format Error: " + this.Uid);
					goto IL_95;
				}
			}
			this.StartTime = 0;
			Service.Get<StaRTSLogger>().Warn("WarScheduleVO:: War Schedule CMS Start Date Not Specified For: " + this.Uid);
			IL_95:
			string text2 = row.TryGetString(WarScheduleVO.COLUMN_endDate);
			if (!string.IsNullOrEmpty(text2))
			{
				try
				{
					DateTime date2 = DateTime.ParseExact(text2, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
					this.EndTime = DateUtils.GetSecondsFromEpoch(date2);
					return;
				}
				catch (Exception)
				{
					this.EndTime = 2147483647;
					Service.Get<StaRTSLogger>().Warn("WarScheduleVO:: War Schedule CMS End Date Format Error: " + this.Uid);
					return;
				}
			}
			this.EndTime = 2147483647;
		}

		public WarScheduleVO()
		{
		}

		protected internal WarScheduleVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarScheduleVO.COLUMN_endDate);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarScheduleVO.COLUMN_startDate);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarScheduleVO.COLUMN_warPlanetUid);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarScheduleVO)GCHandledObjects.GCHandleToObject(instance)).EndTime);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarScheduleVO)GCHandledObjects.GCHandleToObject(instance)).StartTime);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarScheduleVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarScheduleVO)GCHandledObjects.GCHandleToObject(instance)).WarPlanetUid);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((WarScheduleVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			WarScheduleVO.COLUMN_endDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			WarScheduleVO.COLUMN_startDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			WarScheduleVO.COLUMN_warPlanetUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((WarScheduleVO)GCHandledObjects.GCHandleToObject(instance)).EndTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((WarScheduleVO)GCHandledObjects.GCHandleToObject(instance)).StartTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((WarScheduleVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((WarScheduleVO)GCHandledObjects.GCHandleToObject(instance)).WarPlanetUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
