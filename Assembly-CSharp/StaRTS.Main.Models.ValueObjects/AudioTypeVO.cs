using StaRTS.Audio;
using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class AudioTypeVO : IValueObject, IAssetVO
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

		public static int COLUMN_category
		{
			get;
			private set;
		}

		public static int COLUMN_loop
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

		public string BundleName
		{
			get;
			set;
		}

		public AudioCategory Category
		{
			get;
			set;
		}

		public bool Loop
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.AssetName = row.TryGetString(AudioTypeVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(AudioTypeVO.COLUMN_bundleName);
			this.Category = StringUtils.ParseEnum<AudioCategory>(row.TryGetString(AudioTypeVO.COLUMN_category));
			this.Loop = row.TryGetBool(AudioTypeVO.COLUMN_loop);
		}

		public AudioTypeVO()
		{
		}

		protected internal AudioTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AudioTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AudioTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AudioTypeVO)GCHandledObjects.GCHandleToObject(instance)).Category);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AudioTypeVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AudioTypeVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AudioTypeVO.COLUMN_category);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AudioTypeVO.COLUMN_loop);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AudioTypeVO)GCHandledObjects.GCHandleToObject(instance)).Loop);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AudioTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AudioTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AudioTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((AudioTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((AudioTypeVO)GCHandledObjects.GCHandleToObject(instance)).Category = (AudioCategory)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			AudioTypeVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			AudioTypeVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			AudioTypeVO.COLUMN_category = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			AudioTypeVO.COLUMN_loop = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((AudioTypeVO)GCHandledObjects.GCHandleToObject(instance)).Loop = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((AudioTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
