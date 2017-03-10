using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class CurrencyIconVO : IValueObject, IGeometryVO
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

		public static int COLUMN_iconCameraPosition
		{
			get;
			private set;
		}

		public static int COLUMN_iconLookatPosition
		{
			get;
			private set;
		}

		public static int COLUMN_iconCloseupCameraPosition
		{
			get;
			private set;
		}

		public static int COLUMN_iconCloseupLookatPosition
		{
			get;
			private set;
		}

		public static int COLUMN_iconAssetName
		{
			get;
			private set;
		}

		public static int COLUMN_iconBundleName
		{
			get;
			private set;
		}

		public static int COLUMN_iconRotationSpeed
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

		public Vector3 IconCameraPosition
		{
			get;
			set;
		}

		public Vector3 IconLookatPosition
		{
			get;
			set;
		}

		public Vector3 IconCloseupCameraPosition
		{
			get;
			set;
		}

		public Vector3 IconCloseupLookatPosition
		{
			get;
			set;
		}

		public string IconAssetName
		{
			get;
			set;
		}

		public string IconBundleName
		{
			get;
			set;
		}

		public float IconRotationSpeed
		{
			get;
			set;
		}

		public CurrencyIconVO()
		{
		}

		public CurrencyIconVO(string iconAssetName)
		{
			this.IconCameraPosition = new Vector3(0f, 0f, 40f);
			this.IconLookatPosition = Vector3.zero;
			this.IconAssetName = iconAssetName;
			this.IconBundleName = iconAssetName;
			this.AssetName = iconAssetName;
			this.BundleName = iconAssetName;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.AssetName = row.TryGetString(CurrencyIconVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(CurrencyIconVO.COLUMN_bundleName);
			this.IconAssetName = row.TryGetString(CurrencyIconVO.COLUMN_iconAssetName, this.AssetName);
			this.IconBundleName = row.TryGetString(CurrencyIconVO.COLUMN_iconBundleName, this.BundleName);
			this.IconCameraPosition = row.TryGetVector3(CurrencyIconVO.COLUMN_iconCameraPosition);
			this.IconLookatPosition = row.TryGetVector3(CurrencyIconVO.COLUMN_iconLookatPosition);
			this.IconCloseupCameraPosition = row.TryGetVector3(CurrencyIconVO.COLUMN_iconCloseupCameraPosition, this.IconCameraPosition);
			this.IconCloseupLookatPosition = row.TryGetVector3(CurrencyIconVO.COLUMN_iconCloseupLookatPosition, this.IconLookatPosition);
			this.IconRotationSpeed = row.TryGetFloat(CurrencyIconVO.COLUMN_iconRotationSpeed, 0f);
		}

		protected internal CurrencyIconVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CurrencyIconVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CurrencyIconVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CurrencyIconVO.COLUMN_iconAssetName);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CurrencyIconVO.COLUMN_iconBundleName);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CurrencyIconVO.COLUMN_iconCameraPosition);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CurrencyIconVO.COLUMN_iconCloseupCameraPosition);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CurrencyIconVO.COLUMN_iconCloseupLookatPosition);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CurrencyIconVO.COLUMN_iconLookatPosition);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CurrencyIconVO.COLUMN_iconRotationSpeed);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			CurrencyIconVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			CurrencyIconVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			CurrencyIconVO.COLUMN_iconAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			CurrencyIconVO.COLUMN_iconBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			CurrencyIconVO.COLUMN_iconCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			CurrencyIconVO.COLUMN_iconCloseupCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			CurrencyIconVO.COLUMN_iconCloseupLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			CurrencyIconVO.COLUMN_iconLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			CurrencyIconVO.COLUMN_iconRotationSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((CurrencyIconVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
