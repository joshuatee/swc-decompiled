using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class ShardVO : IValueObject
	{
		public static int COLUMN_assetName
		{
			get;
			private set;
		}

		public static int COLUMN_bundleName
		{
			get;
			private set;
		}

		public static int COLUMN_stringName
		{
			get;
			private set;
		}

		public static int COLUMN_stringDesc
		{
			get;
			private set;
		}

		public static int COLUMN_quality
		{
			get;
			private set;
		}

		public static int COLUMN_targetType
		{
			get;
			private set;
		}

		public static int COLUMN_targetGroupId
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string AssetName
		{
			get;
			private set;
		}

		public string BundleName
		{
			get;
			private set;
		}

		public string StringNameId
		{
			get;
			private set;
		}

		public string StringDescId
		{
			get;
			private set;
		}

		public ShardQuality Quality
		{
			get;
			private set;
		}

		public string TargetType
		{
			get;
			private set;
		}

		public string TargetGroupId
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.AssetName = row.TryGetString(ShardVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(ShardVO.COLUMN_bundleName);
			this.StringNameId = row.TryGetString(ShardVO.COLUMN_stringName);
			this.StringDescId = row.TryGetString(ShardVO.COLUMN_stringDesc);
			this.Quality = StringUtils.ParseEnum<ShardQuality>(row.TryGetString(ShardVO.COLUMN_quality));
			this.TargetType = row.TryGetString(ShardVO.COLUMN_targetType);
			this.TargetGroupId = row.TryGetString(ShardVO.COLUMN_targetGroupId);
		}

		public ShardVO()
		{
		}

		protected internal ShardVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShardVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShardVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ShardVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ShardVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ShardVO.COLUMN_quality);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ShardVO.COLUMN_stringDesc);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ShardVO.COLUMN_stringName);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ShardVO.COLUMN_targetGroupId);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ShardVO.COLUMN_targetType);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShardVO)GCHandledObjects.GCHandleToObject(instance)).Quality);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShardVO)GCHandledObjects.GCHandleToObject(instance)).StringDescId);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShardVO)GCHandledObjects.GCHandleToObject(instance)).StringNameId);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShardVO)GCHandledObjects.GCHandleToObject(instance)).TargetGroupId);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShardVO)GCHandledObjects.GCHandleToObject(instance)).TargetType);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShardVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ShardVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((ShardVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((ShardVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			ShardVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			ShardVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			ShardVO.COLUMN_quality = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			ShardVO.COLUMN_stringDesc = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			ShardVO.COLUMN_stringName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			ShardVO.COLUMN_targetGroupId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			ShardVO.COLUMN_targetType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((ShardVO)GCHandledObjects.GCHandleToObject(instance)).Quality = (ShardQuality)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((ShardVO)GCHandledObjects.GCHandleToObject(instance)).StringDescId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((ShardVO)GCHandledObjects.GCHandleToObject(instance)).StringNameId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((ShardVO)GCHandledObjects.GCHandleToObject(instance)).TargetGroupId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((ShardVO)GCHandledObjects.GCHandleToObject(instance)).TargetType = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((ShardVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
