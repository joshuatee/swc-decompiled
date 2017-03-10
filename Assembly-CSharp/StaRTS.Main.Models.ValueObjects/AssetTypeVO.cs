using StaRTS.Assets;
using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class AssetTypeVO : IValueObject
	{
		public const string UID_PREFIX = "asset_";

		public static int COLUMN_assetName
		{
			get;
			private set;
		}

		public static int COLUMN_assetCategory
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
			set;
		}

		public AssetCategory Category
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.AssetName = row.TryGetString(AssetTypeVO.COLUMN_assetName);
			this.Category = StringUtils.ParseEnum<AssetCategory>(row.TryGetString(AssetTypeVO.COLUMN_assetCategory));
			this.Uid = "asset_" + this.AssetName;
		}

		public AssetTypeVO()
		{
		}

		protected internal AssetTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetTypeVO)GCHandledObjects.GCHandleToObject(instance)).Category);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AssetTypeVO.COLUMN_assetCategory);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AssetTypeVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AssetTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AssetTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AssetTypeVO)GCHandledObjects.GCHandleToObject(instance)).Category = (AssetCategory)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			AssetTypeVO.COLUMN_assetCategory = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			AssetTypeVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AssetTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
