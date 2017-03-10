using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class SaleItemTypeVO : IValueObject
	{
		public static int COLUMN_title
		{
			get;
			private set;
		}

		public static int COLUMN_bonusMultiplier
		{
			get;
			private set;
		}

		public static int COLUMN_productId
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

		public double BonusMultiplier
		{
			get;
			set;
		}

		public string ProductId
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Title = row.TryGetString(SaleItemTypeVO.COLUMN_title);
			this.BonusMultiplier = (double)row.TryGetFloat(SaleItemTypeVO.COLUMN_bonusMultiplier, 1f);
			this.ProductId = row.TryGetString(SaleItemTypeVO.COLUMN_productId);
		}

		public SaleItemTypeVO()
		{
		}

		protected internal SaleItemTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SaleItemTypeVO.COLUMN_bonusMultiplier);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SaleItemTypeVO.COLUMN_productId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SaleItemTypeVO.COLUMN_title);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SaleItemTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProductId);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SaleItemTypeVO)GCHandledObjects.GCHandleToObject(instance)).Title);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SaleItemTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SaleItemTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SaleItemTypeVO)GCHandledObjects.GCHandleToObject(instance)).BonusMultiplier = *(double*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			SaleItemTypeVO.COLUMN_bonusMultiplier = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			SaleItemTypeVO.COLUMN_productId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			SaleItemTypeVO.COLUMN_title = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SaleItemTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProductId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SaleItemTypeVO)GCHandledObjects.GCHandleToObject(instance)).Title = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SaleItemTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
