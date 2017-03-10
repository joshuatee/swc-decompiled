using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class UITypeVO : IValueObject, IAssetVO
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

		public static int COLUMN_ShowHUDWhileDisplayed
		{
			get;
			private set;
		}

		public static int COLUMN_HideHUDAfterClosing
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

		public bool ShowHUDWhileDisplayed
		{
			get;
			set;
		}

		public bool HideHUDAfterClosing
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.AssetName = row.TryGetString(UITypeVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(UITypeVO.COLUMN_bundleName);
			this.ShowHUDWhileDisplayed = row.TryGetBool(UITypeVO.COLUMN_ShowHUDWhileDisplayed);
			this.HideHUDAfterClosing = row.TryGetBool(UITypeVO.COLUMN_HideHUDAfterClosing);
		}

		public UITypeVO()
		{
		}

		protected internal UITypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UITypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UITypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UITypeVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UITypeVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UITypeVO.COLUMN_HideHUDAfterClosing);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UITypeVO.COLUMN_ShowHUDWhileDisplayed);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UITypeVO)GCHandledObjects.GCHandleToObject(instance)).HideHUDAfterClosing);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UITypeVO)GCHandledObjects.GCHandleToObject(instance)).ShowHUDWhileDisplayed);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UITypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UITypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UITypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UITypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			UITypeVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			UITypeVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			UITypeVO.COLUMN_HideHUDAfterClosing = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			UITypeVO.COLUMN_ShowHUDWhileDisplayed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((UITypeVO)GCHandledObjects.GCHandleToObject(instance)).HideHUDAfterClosing = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((UITypeVO)GCHandledObjects.GCHandleToObject(instance)).ShowHUDWhileDisplayed = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((UITypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
