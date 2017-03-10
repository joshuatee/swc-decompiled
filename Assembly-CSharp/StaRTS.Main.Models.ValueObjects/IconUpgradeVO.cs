using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class IconUpgradeVO : IValueObject
	{
		public static int COLUMN_level
		{
			get;
			private set;
		}

		public static int COLUMN_rating
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public int Level
		{
			get;
			set;
		}

		public int Rating
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Level = row.TryGetInt(IconUpgradeVO.COLUMN_level);
			this.Rating = row.TryGetInt(IconUpgradeVO.COLUMN_rating);
		}

		public IconUpgradeVO()
		{
		}

		protected internal IconUpgradeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(IconUpgradeVO.COLUMN_level);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(IconUpgradeVO.COLUMN_rating);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IconUpgradeVO)GCHandledObjects.GCHandleToObject(instance)).Level);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IconUpgradeVO)GCHandledObjects.GCHandleToObject(instance)).Rating);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IconUpgradeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((IconUpgradeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			IconUpgradeVO.COLUMN_level = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			IconUpgradeVO.COLUMN_rating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((IconUpgradeVO)GCHandledObjects.GCHandleToObject(instance)).Level = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((IconUpgradeVO)GCHandledObjects.GCHandleToObject(instance)).Rating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((IconUpgradeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
