using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class CrateSupplyScaleVO : IValueObject
	{
		public static int COLUMN_HQ2
		{
			get;
			private set;
		}

		public static int COLUMN_HQ3
		{
			get;
			private set;
		}

		public static int COLUMN_HQ4
		{
			get;
			private set;
		}

		public static int COLUMN_HQ5
		{
			get;
			private set;
		}

		public static int COLUMN_HQ6
		{
			get;
			private set;
		}

		public static int COLUMN_HQ7
		{
			get;
			private set;
		}

		public static int COLUMN_HQ8
		{
			get;
			private set;
		}

		public static int COLUMN_HQ9
		{
			get;
			private set;
		}

		public static int COLUMN_HQ10
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public int HQ2
		{
			get;
			set;
		}

		public int HQ3
		{
			get;
			set;
		}

		public int HQ4
		{
			get;
			set;
		}

		public int HQ5
		{
			get;
			set;
		}

		public int HQ6
		{
			get;
			set;
		}

		public int HQ7
		{
			get;
			set;
		}

		public int HQ8
		{
			get;
			set;
		}

		public int HQ9
		{
			get;
			set;
		}

		public int HQ10
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.HQ2 = row.TryGetInt(CrateSupplyScaleVO.COLUMN_HQ2);
			this.HQ3 = row.TryGetInt(CrateSupplyScaleVO.COLUMN_HQ3);
			this.HQ4 = row.TryGetInt(CrateSupplyScaleVO.COLUMN_HQ4);
			this.HQ5 = row.TryGetInt(CrateSupplyScaleVO.COLUMN_HQ5);
			this.HQ6 = row.TryGetInt(CrateSupplyScaleVO.COLUMN_HQ6);
			this.HQ7 = row.TryGetInt(CrateSupplyScaleVO.COLUMN_HQ7);
			this.HQ8 = row.TryGetInt(CrateSupplyScaleVO.COLUMN_HQ8);
			this.HQ9 = row.TryGetInt(CrateSupplyScaleVO.COLUMN_HQ9);
			this.HQ10 = row.TryGetInt(CrateSupplyScaleVO.COLUMN_HQ10);
		}

		public int GetHQScaling(int hq)
		{
			switch (hq)
			{
			case 3:
				return this.HQ3;
			case 4:
				return this.HQ4;
			case 5:
				return this.HQ5;
			case 6:
				return this.HQ6;
			case 7:
				return this.HQ7;
			case 8:
				return this.HQ8;
			case 9:
				return this.HQ9;
			case 10:
				return this.HQ10;
			}
			return this.HQ2;
		}

		public CrateSupplyScaleVO()
		{
		}

		protected internal CrateSupplyScaleVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyScaleVO.COLUMN_HQ10);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyScaleVO.COLUMN_HQ2);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyScaleVO.COLUMN_HQ3);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyScaleVO.COLUMN_HQ4);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyScaleVO.COLUMN_HQ5);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyScaleVO.COLUMN_HQ6);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyScaleVO.COLUMN_HQ7);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyScaleVO.COLUMN_HQ8);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyScaleVO.COLUMN_HQ9);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ10);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ2);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ3);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ4);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ5);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ6);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ7);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ8);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ9);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).GetHQScaling(*(int*)args));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			CrateSupplyScaleVO.COLUMN_HQ10 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			CrateSupplyScaleVO.COLUMN_HQ2 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			CrateSupplyScaleVO.COLUMN_HQ3 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			CrateSupplyScaleVO.COLUMN_HQ4 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			CrateSupplyScaleVO.COLUMN_HQ5 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			CrateSupplyScaleVO.COLUMN_HQ6 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			CrateSupplyScaleVO.COLUMN_HQ7 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			CrateSupplyScaleVO.COLUMN_HQ8 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			CrateSupplyScaleVO.COLUMN_HQ9 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ10 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ2 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ3 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ4 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ5 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ6 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ7 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ8 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).HQ9 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((CrateSupplyScaleVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
