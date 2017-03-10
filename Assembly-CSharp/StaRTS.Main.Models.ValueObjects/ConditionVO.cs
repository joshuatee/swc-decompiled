using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class ConditionVO : IValueObject
	{
		public static int COLUMN_condition_type
		{
			get;
			private set;
		}

		public static int COLUMN_prepare_string
		{
			get;
			private set;
		}

		public static int COLUMN_end_on_fail
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string ConditionType
		{
			get;
			set;
		}

		public string PrepareString
		{
			get;
			set;
		}

		public bool EndOnFail
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.ConditionType = row.TryGetString(ConditionVO.COLUMN_condition_type);
			this.PrepareString = row.TryGetString(ConditionVO.COLUMN_prepare_string);
			this.EndOnFail = row.TryGetBool(ConditionVO.COLUMN_end_on_fail);
		}

		public bool IsPvpType()
		{
			return this.ConditionType == "PvpStart" || this.ConditionType == "PvpWin";
		}

		public ConditionVO()
		{
		}

		protected internal ConditionVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ConditionVO.COLUMN_condition_type);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ConditionVO.COLUMN_end_on_fail);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ConditionVO.COLUMN_prepare_string);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ConditionVO)GCHandledObjects.GCHandleToObject(instance)).ConditionType);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ConditionVO)GCHandledObjects.GCHandleToObject(instance)).EndOnFail);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ConditionVO)GCHandledObjects.GCHandleToObject(instance)).PrepareString);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ConditionVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ConditionVO)GCHandledObjects.GCHandleToObject(instance)).IsPvpType());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ConditionVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			ConditionVO.COLUMN_condition_type = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			ConditionVO.COLUMN_end_on_fail = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			ConditionVO.COLUMN_prepare_string = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((ConditionVO)GCHandledObjects.GCHandleToObject(instance)).ConditionType = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ConditionVO)GCHandledObjects.GCHandleToObject(instance)).EndOnFail = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ConditionVO)GCHandledObjects.GCHandleToObject(instance)).PrepareString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ConditionVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
