using StaRTS.Main.Models.Squads.War;
using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class WarBuffVO : IValueObject
	{
		public static int COLUMN_planetId
		{
			get;
			private set;
		}

		public static int COLUMN_masterNeutralBuildingUid
		{
			get;
			private set;
		}

		public static int COLUMN_masterEmpireBuildingUid
		{
			get;
			private set;
		}

		public static int COLUMN_masterRebelBuildingUid
		{
			get;
			private set;
		}

		public static int COLUMN_troopBuffUid
		{
			get;
			private set;
		}

		public static int COLUMN_buildingBuffUid
		{
			get;
			private set;
		}

		public static int COLUMN_buffType
		{
			get;
			private set;
		}

		public static int COLUMN_buffBaseName
		{
			get;
			private set;
		}

		public static int COLUMN_buffStringTitle
		{
			get;
			private set;
		}

		public static int COLUMN_buffStringDesc
		{
			get;
			private set;
		}

		public static int COLUMN_buffIcon
		{
			get;
			private set;
		}

		public static int COLUMN_empireBattlesByLevel
		{
			get;
			private set;
		}

		public static int COLUMN_rebelBattlesByLevel
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string PlanetId
		{
			get;
			set;
		}

		public string MasterNeutralBuildingUid
		{
			get;
			set;
		}

		public string MasterEmpireBuildingUid
		{
			get;
			set;
		}

		public string MasterRebelBuildingUid
		{
			get;
			set;
		}

		public string TroopBuffUid
		{
			get;
			set;
		}

		public string BuildingBuffUid
		{
			get;
			set;
		}

		public SquadWarBuffType BuffType
		{
			get;
			set;
		}

		public string BuffBaseName
		{
			get;
			set;
		}

		public string BuffStringTitle
		{
			get;
			set;
		}

		public string BuffStringDesc
		{
			get;
			set;
		}

		public string BuffIcon
		{
			get;
			set;
		}

		public string[] EmpireBattlesByLevel
		{
			get;
			set;
		}

		public string[] RebelBattlesByLevel
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.PlanetId = row.TryGetString(WarBuffVO.COLUMN_planetId);
			this.MasterNeutralBuildingUid = row.TryGetString(WarBuffVO.COLUMN_masterNeutralBuildingUid);
			this.MasterEmpireBuildingUid = row.TryGetString(WarBuffVO.COLUMN_masterEmpireBuildingUid);
			this.MasterRebelBuildingUid = row.TryGetString(WarBuffVO.COLUMN_masterRebelBuildingUid);
			this.TroopBuffUid = row.TryGetString(WarBuffVO.COLUMN_troopBuffUid);
			this.BuildingBuffUid = row.TryGetString(WarBuffVO.COLUMN_buildingBuffUid);
			this.BuffType = StringUtils.ParseEnum<SquadWarBuffType>(row.TryGetString(WarBuffVO.COLUMN_buffType));
			this.BuffBaseName = row.TryGetString(WarBuffVO.COLUMN_buffBaseName);
			this.BuffStringDesc = row.TryGetString(WarBuffVO.COLUMN_buffStringDesc);
			this.BuffStringTitle = row.TryGetString(WarBuffVO.COLUMN_buffStringTitle);
			this.BuffIcon = row.TryGetString(WarBuffVO.COLUMN_buffIcon);
			string text = row.TryGetString(WarBuffVO.COLUMN_empireBattlesByLevel);
			if (!string.IsNullOrEmpty(text))
			{
				this.EmpireBattlesByLevel = text.Split(new char[0]);
			}
			string text2 = row.TryGetString(WarBuffVO.COLUMN_rebelBattlesByLevel);
			if (!string.IsNullOrEmpty(text2))
			{
				this.RebelBattlesByLevel = text2.Split(new char[0]);
			}
		}

		public WarBuffVO()
		{
		}

		protected internal WarBuffVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).BuffBaseName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).BuffIcon);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).BuffStringDesc);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).BuffStringTitle);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).BuffType);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).BuildingBuffUid);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_buffBaseName);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_buffIcon);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_buffStringDesc);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_buffStringTitle);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_buffType);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_buildingBuffUid);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_empireBattlesByLevel);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_masterEmpireBuildingUid);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_masterNeutralBuildingUid);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_masterRebelBuildingUid);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_planetId);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_rebelBattlesByLevel);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WarBuffVO.COLUMN_troopBuffUid);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).EmpireBattlesByLevel);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).MasterEmpireBuildingUid);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).MasterNeutralBuildingUid);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).MasterRebelBuildingUid);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).PlanetId);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).RebelBattlesByLevel);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).TroopBuffUid);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).BuffBaseName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).BuffIcon = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).BuffStringDesc = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).BuffStringTitle = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).BuffType = (SquadWarBuffType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).BuildingBuffUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			WarBuffVO.COLUMN_buffBaseName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			WarBuffVO.COLUMN_buffIcon = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			WarBuffVO.COLUMN_buffStringDesc = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			WarBuffVO.COLUMN_buffStringTitle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			WarBuffVO.COLUMN_buffType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			WarBuffVO.COLUMN_buildingBuffUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			WarBuffVO.COLUMN_empireBattlesByLevel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			WarBuffVO.COLUMN_masterEmpireBuildingUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			WarBuffVO.COLUMN_masterNeutralBuildingUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			WarBuffVO.COLUMN_masterRebelBuildingUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			WarBuffVO.COLUMN_planetId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			WarBuffVO.COLUMN_rebelBattlesByLevel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			WarBuffVO.COLUMN_troopBuffUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).EmpireBattlesByLevel = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).MasterEmpireBuildingUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).MasterNeutralBuildingUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).MasterRebelBuildingUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).PlanetId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).RebelBattlesByLevel = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).TroopBuffUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((WarBuffVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
