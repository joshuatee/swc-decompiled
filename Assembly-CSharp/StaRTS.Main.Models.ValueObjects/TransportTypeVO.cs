using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class TransportTypeVO : IValueObject, IAssetVO
	{
		public int SizeX;

		public int SizeY;

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

		public static int COLUMN_faction
		{
			get;
			private set;
		}

		public static int COLUMN_transportID
		{
			get;
			private set;
		}

		public static int COLUMN_transportName
		{
			get;
			private set;
		}

		public static int COLUMN_capacity
		{
			get;
			private set;
		}

		public static int COLUMN_maxSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_lvl
		{
			get;
			private set;
		}

		public static int COLUMN_type
		{
			get;
			private set;
		}

		public static int COLUMN_sizex
		{
			get;
			private set;
		}

		public static int COLUMN_sizey
		{
			get;
			private set;
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

		public string Uid
		{
			get;
			set;
		}

		public FactionType Faction
		{
			get;
			set;
		}

		public string transportID
		{
			get;
			set;
		}

		public string transportName
		{
			get;
			set;
		}

		public int MaxSpeed
		{
			get;
			set;
		}

		public int Capacity
		{
			get;
			set;
		}

		public int Level
		{
			get;
			set;
		}

		public TransportType TransportType
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.AssetName = row.TryGetString(TransportTypeVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(TransportTypeVO.COLUMN_bundleName);
			this.Uid = row.Uid;
			this.Faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(TransportTypeVO.COLUMN_faction));
			this.transportID = row.TryGetString(TransportTypeVO.COLUMN_transportID);
			this.transportName = row.TryGetString(TransportTypeVO.COLUMN_transportName);
			this.Capacity = row.TryGetInt(TransportTypeVO.COLUMN_capacity);
			this.MaxSpeed = row.TryGetInt(TransportTypeVO.COLUMN_maxSpeed);
			this.Level = row.TryGetInt(TransportTypeVO.COLUMN_lvl);
			this.TransportType = StringUtils.ParseEnum<TransportType>(row.TryGetString(TransportTypeVO.COLUMN_type));
			this.SizeX = row.TryGetInt(TransportTypeVO.COLUMN_sizex);
			this.SizeY = row.TryGetInt(TransportTypeVO.COLUMN_sizey);
		}

		public TransportTypeVO()
		{
		}

		protected internal TransportTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).Capacity);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransportTypeVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransportTypeVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransportTypeVO.COLUMN_capacity);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransportTypeVO.COLUMN_faction);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransportTypeVO.COLUMN_lvl);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransportTypeVO.COLUMN_maxSpeed);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransportTypeVO.COLUMN_sizex);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransportTypeVO.COLUMN_sizey);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransportTypeVO.COLUMN_transportID);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransportTypeVO.COLUMN_transportName);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransportTypeVO.COLUMN_type);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).Level);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).transportID);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).transportName);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).TransportType);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).Capacity = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			TransportTypeVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			TransportTypeVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			TransportTypeVO.COLUMN_capacity = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			TransportTypeVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			TransportTypeVO.COLUMN_lvl = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			TransportTypeVO.COLUMN_maxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			TransportTypeVO.COLUMN_sizex = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			TransportTypeVO.COLUMN_sizey = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			TransportTypeVO.COLUMN_transportID = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			TransportTypeVO.COLUMN_transportName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			TransportTypeVO.COLUMN_type = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).Level = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).transportID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).transportName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).TransportType = (TransportType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((TransportTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
