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
	public class ObjectiveSeriesVO : IValueObject
	{
		public static int COLUMN_planetUid
		{
			get;
			private set;
		}

		public static int COLUMN_objCount
		{
			get;
			private set;
		}

		public static int COLUMN_objBucket
		{
			get;
			private set;
		}

		public static int COLUMN_objBucket2
		{
			get;
			private set;
		}

		public static int COLUMN_objBucket3
		{
			get;
			private set;
		}

		public static int COLUMN_startDate
		{
			get;
			private set;
		}

		public static int COLUMN_periodHours
		{
			get;
			private set;
		}

		public static int COLUMN_graceHours
		{
			get;
			private set;
		}

		public static int COLUMN_endDate
		{
			get;
			private set;
		}

		public static int COLUMN_specialEvent
		{
			get;
			private set;
		}

		public static int COLUMN_eventIcon
		{
			get;
			private set;
		}

		public static int COLUMN_eventPlayart
		{
			get;
			private set;
		}

		public static int COLUMN_eventDetailsart
		{
			get;
			private set;
		}

		public static int COLUMN_bundleName
		{
			get;
			private set;
		}

		public static int COLUMN_headerString
		{
			get;
			private set;
		}

		public static int COLUMN_objectiveString
		{
			get;
			private set;
		}

		public static int COLUMN_objectiveExpiringString
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string PlanetUid
		{
			get;
			set;
		}

		public int ObjCount
		{
			get;
			set;
		}

		public string ObjBucket
		{
			get;
			set;
		}

		public string ObjBucket2
		{
			get;
			set;
		}

		public string ObjBucket3
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

		public int PeriodHours
		{
			get;
			set;
		}

		public int GraceHours
		{
			get;
			set;
		}

		public bool SpecialEvent
		{
			get;
			set;
		}

		public string EventIcon
		{
			get;
			set;
		}

		public string EventPlayArt
		{
			get;
			set;
		}

		public string EventDetailsArt
		{
			get;
			set;
		}

		public string BundleName
		{
			get;
			set;
		}

		public string HeaderString
		{
			get;
			set;
		}

		public string ObjectiveString
		{
			get;
			set;
		}

		public string ObjectiveExpiringString
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.PlanetUid = row.TryGetString(ObjectiveSeriesVO.COLUMN_planetUid);
			this.ObjCount = row.TryGetInt(ObjectiveSeriesVO.COLUMN_objCount);
			this.ObjBucket = row.TryGetString(ObjectiveSeriesVO.COLUMN_objBucket);
			this.ObjBucket2 = row.TryGetString(ObjectiveSeriesVO.COLUMN_objBucket2);
			this.ObjBucket3 = row.TryGetString(ObjectiveSeriesVO.COLUMN_objBucket3);
			this.PeriodHours = row.TryGetInt(ObjectiveSeriesVO.COLUMN_periodHours);
			this.GraceHours = row.TryGetInt(ObjectiveSeriesVO.COLUMN_graceHours);
			this.SpecialEvent = row.TryGetBool(ObjectiveSeriesVO.COLUMN_specialEvent);
			this.EventIcon = row.TryGetString(ObjectiveSeriesVO.COLUMN_eventIcon);
			this.EventPlayArt = row.TryGetString(ObjectiveSeriesVO.COLUMN_eventPlayart);
			this.EventDetailsArt = row.TryGetString(ObjectiveSeriesVO.COLUMN_eventDetailsart);
			this.BundleName = row.TryGetString(ObjectiveSeriesVO.COLUMN_bundleName);
			this.HeaderString = row.TryGetString(ObjectiveSeriesVO.COLUMN_headerString);
			this.ObjectiveString = row.TryGetString(ObjectiveSeriesVO.COLUMN_objectiveString);
			this.ObjectiveExpiringString = row.TryGetString(ObjectiveSeriesVO.COLUMN_objectiveExpiringString);
			string text = row.TryGetString(ObjectiveSeriesVO.COLUMN_startDate);
			if (!string.IsNullOrEmpty(text))
			{
				DateTime date = DateTime.ParseExact(text, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
				this.StartTime = DateUtils.GetSecondsFromEpoch(date);
			}
			else
			{
				this.StartTime = 0;
				Service.Get<StaRTSLogger>().Warn("ObjectiveSeries VO Start Date Not Specified");
			}
			string text2 = row.TryGetString(ObjectiveSeriesVO.COLUMN_endDate);
			if (!string.IsNullOrEmpty(text2))
			{
				DateTime date2 = DateTime.ParseExact(text2, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
				this.EndTime = DateUtils.GetSecondsFromEpoch(date2);
				return;
			}
			this.EndTime = 2147483647;
			Service.Get<StaRTSLogger>().Warn("ObjectiveSeries VO End Date Not Specified");
		}

		public ObjectiveSeriesVO()
		{
		}

		protected internal ObjectiveSeriesVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_endDate);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_eventDetailsart);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_eventIcon);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_eventPlayart);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_graceHours);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_headerString);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_objBucket);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_objBucket2);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_objBucket3);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_objCount);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_objectiveExpiringString);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_objectiveString);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_periodHours);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_planetUid);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_specialEvent);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ObjectiveSeriesVO.COLUMN_startDate);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).EndTime);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).EventDetailsArt);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).EventIcon);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).EventPlayArt);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).GraceHours);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).HeaderString);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ObjBucket);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ObjBucket2);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ObjBucket3);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ObjCount);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ObjectiveExpiringString);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ObjectiveString);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).PeriodHours);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).PlanetUid);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).SpecialEvent);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).StartTime);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_endDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_eventDetailsart = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_eventIcon = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_eventPlayart = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_graceHours = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_headerString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_objBucket = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_objBucket2 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_objBucket3 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_objCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_objectiveExpiringString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_objectiveString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_periodHours = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_planetUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_specialEvent = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			ObjectiveSeriesVO.COLUMN_startDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).EndTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).EventDetailsArt = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).EventIcon = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).EventPlayArt = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).GraceHours = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).HeaderString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ObjBucket = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ObjBucket2 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ObjBucket3 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ObjCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ObjectiveExpiringString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).ObjectiveString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).PeriodHours = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).PlanetUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).SpecialEvent = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).StartTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			((ObjectiveSeriesVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
