using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class DefaultLightingVO : IValueObject
	{
		public static int COLUMN_description
		{
			get;
			private set;
		}

		public static int COLUMN_buildingColorDark
		{
			get;
			private set;
		}

		public static int COLUMN_buildingColorLight
		{
			get;
			private set;
		}

		public static int COLUMN_unitColor
		{
			get;
			private set;
		}

		public static int COLUMN_shadowColor
		{
			get;
			private set;
		}

		public static int COLUMN_groundColor
		{
			get;
			private set;
		}

		public static int COLUMN_groundColorLight
		{
			get;
			private set;
		}

		public static int COLUMN_gridColor
		{
			get;
			private set;
		}

		public static int COLUMN_buildingGridColor
		{
			get;
			private set;
		}

		public static int COLUMN_wallGridColor
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string LightingColorDark
		{
			get;
			set;
		}

		public string LightingColorLight
		{
			get;
			set;
		}

		public string LightingColorMedian
		{
			get;
			set;
		}

		public string LightingColorShadow
		{
			get;
			set;
		}

		public string LightingColorGround
		{
			get;
			set;
		}

		public string LightingColorGroundLight
		{
			get;
			set;
		}

		public string LightingColorGrid
		{
			get;
			set;
		}

		public string LightingColorBuildingGrid
		{
			get;
			set;
		}

		public string LightingColorWallGrid
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Description = row.TryGetString(DefaultLightingVO.COLUMN_description);
			this.LightingColorDark = row.TryGetHexValueString(DefaultLightingVO.COLUMN_buildingColorDark);
			this.LightingColorLight = row.TryGetHexValueString(DefaultLightingVO.COLUMN_buildingColorLight);
			this.LightingColorMedian = row.TryGetHexValueString(DefaultLightingVO.COLUMN_unitColor);
			this.LightingColorShadow = row.TryGetHexValueString(DefaultLightingVO.COLUMN_shadowColor);
			this.LightingColorGround = row.TryGetHexValueString(DefaultLightingVO.COLUMN_groundColor);
			this.LightingColorGroundLight = row.TryGetHexValueString(DefaultLightingVO.COLUMN_groundColorLight);
			this.LightingColorGrid = row.TryGetHexValueString(DefaultLightingVO.COLUMN_gridColor);
			this.LightingColorBuildingGrid = row.TryGetHexValueString(DefaultLightingVO.COLUMN_buildingGridColor);
			this.LightingColorWallGrid = row.TryGetHexValueString(DefaultLightingVO.COLUMN_wallGridColor);
		}

		public DefaultLightingVO()
		{
		}

		protected internal DefaultLightingVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DefaultLightingVO.COLUMN_buildingColorDark);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DefaultLightingVO.COLUMN_buildingColorLight);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DefaultLightingVO.COLUMN_buildingGridColor);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DefaultLightingVO.COLUMN_description);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DefaultLightingVO.COLUMN_gridColor);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DefaultLightingVO.COLUMN_groundColor);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DefaultLightingVO.COLUMN_groundColorLight);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DefaultLightingVO.COLUMN_shadowColor);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DefaultLightingVO.COLUMN_unitColor);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DefaultLightingVO.COLUMN_wallGridColor);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorBuildingGrid);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorDark);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorGrid);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorGround);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorGroundLight);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorLight);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorMedian);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorShadow);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorWallGrid);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			DefaultLightingVO.COLUMN_buildingColorDark = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			DefaultLightingVO.COLUMN_buildingColorLight = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			DefaultLightingVO.COLUMN_buildingGridColor = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			DefaultLightingVO.COLUMN_description = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			DefaultLightingVO.COLUMN_gridColor = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			DefaultLightingVO.COLUMN_groundColor = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			DefaultLightingVO.COLUMN_groundColorLight = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			DefaultLightingVO.COLUMN_shadowColor = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			DefaultLightingVO.COLUMN_unitColor = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			DefaultLightingVO.COLUMN_wallGridColor = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).Description = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorBuildingGrid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorDark = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorGrid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorGround = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorGroundLight = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorLight = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorMedian = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorShadow = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).LightingColorWallGrid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((DefaultLightingVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
