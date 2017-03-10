using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class RaidVO : IValueObject
	{
		public static int COLUMN_buildingHoloAsset
		{
			get;
			private set;
		}

		public static int COLUMN_buildingHoloBundle
		{
			get;
			private set;
		}

		public static int COLUMN_squadEnabled
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string BuildingHoloAssetName
		{
			get;
			private set;
		}

		public string BuildingHoloAssetBundle
		{
			get;
			private set;
		}

		public bool SquadEnabled
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.BuildingHoloAssetName = row.TryGetString(RaidVO.COLUMN_buildingHoloAsset, "");
			this.BuildingHoloAssetBundle = row.TryGetString(RaidVO.COLUMN_buildingHoloBundle, "");
			this.SquadEnabled = row.TryGetBool(RaidVO.COLUMN_squadEnabled);
		}

		public RaidVO()
		{
		}

		protected internal RaidVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidVO)GCHandledObjects.GCHandleToObject(instance)).BuildingHoloAssetBundle);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidVO)GCHandledObjects.GCHandleToObject(instance)).BuildingHoloAssetName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RaidVO.COLUMN_buildingHoloAsset);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RaidVO.COLUMN_buildingHoloBundle);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RaidVO.COLUMN_squadEnabled);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidVO)GCHandledObjects.GCHandleToObject(instance)).SquadEnabled);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((RaidVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((RaidVO)GCHandledObjects.GCHandleToObject(instance)).BuildingHoloAssetBundle = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((RaidVO)GCHandledObjects.GCHandleToObject(instance)).BuildingHoloAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			RaidVO.COLUMN_buildingHoloAsset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			RaidVO.COLUMN_buildingHoloBundle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			RaidVO.COLUMN_squadEnabled = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((RaidVO)GCHandledObjects.GCHandleToObject(instance)).SquadEnabled = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((RaidVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
