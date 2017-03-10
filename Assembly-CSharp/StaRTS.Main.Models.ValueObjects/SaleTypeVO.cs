using StaRTS.Main.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class SaleTypeVO : ITimedEventVO, IValueObject
	{
		public static int COLUMN_title
		{
			get;
			private set;
		}

		public static int COLUMN_saleItems
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

		public string Title
		{
			get;
			set;
		}

		public string[] SaleItems
		{
			get;
			set;
		}

		public int StartTimestamp
		{
			get;
			set;
		}

		public int EndTimestamp
		{
			get;
			set;
		}

		public bool UseTimeZoneOffset
		{
			get
			{
				return false;
			}
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Title = row.TryGetString(SaleTypeVO.COLUMN_title);
			this.SaleItems = row.TryGetStringArray(SaleTypeVO.COLUMN_saleItems);
			string dateString = row.TryGetString(SaleTypeVO.COLUMN_startDate);
			string dateString2 = row.TryGetString(SaleTypeVO.COLUMN_endDate);
			this.StartTimestamp = TimedEventUtils.GetTimestamp(this.Uid, dateString);
			this.EndTimestamp = TimedEventUtils.GetTimestamp(this.Uid, dateString2);
		}

		public int GetUpcomingDurationSeconds()
		{
			return 0;
		}

		public int GetClosingDurationSeconds()
		{
			return 0;
		}

		public SaleTypeVO()
		{
		}

		protected internal SaleTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SaleTypeVO.COLUMN_endDate);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SaleTypeVO.COLUMN_saleItems);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SaleTypeVO.COLUMN_startDate);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SaleTypeVO.COLUMN_title);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).EndTimestamp);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).SaleItems);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).StartTimestamp);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).Title);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).UseTimeZoneOffset);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).GetClosingDurationSeconds());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).GetUpcomingDurationSeconds());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			SaleTypeVO.COLUMN_endDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			SaleTypeVO.COLUMN_saleItems = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			SaleTypeVO.COLUMN_startDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			SaleTypeVO.COLUMN_title = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).EndTimestamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).SaleItems = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).StartTimestamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).Title = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((SaleTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
