using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class BuildingConnectorTypeVO : IValueObject
	{
		public static int COLUMN_assetConnectorsNE
		{
			get;
			private set;
		}

		public static int COLUMN_bundleConnectorsNE
		{
			get;
			private set;
		}

		public static int COLUMN_assetConnectorsNW
		{
			get;
			private set;
		}

		public static int COLUMN_bundleConnectorsNW
		{
			get;
			private set;
		}

		public static int COLUMN_assetConnectorsBoth
		{
			get;
			private set;
		}

		public static int COLUMN_bundleConnectorsBoth
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string AssetNameNE
		{
			get;
			set;
		}

		public string BundleNameNE
		{
			get;
			set;
		}

		public string AssetNameNW
		{
			get;
			set;
		}

		public string BundleNameNW
		{
			get;
			set;
		}

		public string AssetNameBoth
		{
			get;
			set;
		}

		public string BundleNameBoth
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.AssetNameNE = row.TryGetString(BuildingConnectorTypeVO.COLUMN_assetConnectorsNE);
			this.BundleNameNE = row.TryGetString(BuildingConnectorTypeVO.COLUMN_bundleConnectorsNE);
			this.AssetNameNW = row.TryGetString(BuildingConnectorTypeVO.COLUMN_assetConnectorsNW);
			this.BundleNameNW = row.TryGetString(BuildingConnectorTypeVO.COLUMN_bundleConnectorsNW);
			this.AssetNameBoth = row.TryGetString(BuildingConnectorTypeVO.COLUMN_assetConnectorsBoth);
			this.BundleNameBoth = row.TryGetString(BuildingConnectorTypeVO.COLUMN_bundleConnectorsBoth);
		}

		public BuildingConnectorTypeVO()
		{
		}

		protected internal BuildingConnectorTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetNameBoth);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetNameNE);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetNameNW);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleNameBoth);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleNameNE);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleNameNW);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingConnectorTypeVO.COLUMN_assetConnectorsBoth);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingConnectorTypeVO.COLUMN_assetConnectorsNE);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingConnectorTypeVO.COLUMN_assetConnectorsNW);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingConnectorTypeVO.COLUMN_bundleConnectorsBoth);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingConnectorTypeVO.COLUMN_bundleConnectorsNE);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingConnectorTypeVO.COLUMN_bundleConnectorsNW);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetNameBoth = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetNameNE = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetNameNW = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleNameBoth = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleNameNE = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleNameNW = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			BuildingConnectorTypeVO.COLUMN_assetConnectorsBoth = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			BuildingConnectorTypeVO.COLUMN_assetConnectorsNE = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			BuildingConnectorTypeVO.COLUMN_assetConnectorsNW = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			BuildingConnectorTypeVO.COLUMN_bundleConnectorsBoth = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			BuildingConnectorTypeVO.COLUMN_bundleConnectorsNE = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			BuildingConnectorTypeVO.COLUMN_bundleConnectorsNW = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
