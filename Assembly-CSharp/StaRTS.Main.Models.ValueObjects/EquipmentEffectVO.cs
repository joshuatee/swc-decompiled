using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class EquipmentEffectVO : IValueObject
	{
		public static int COLUMN_affectedTroopIds
		{
			get;
			private set;
		}

		public static int COLUMN_affectedSpecialAttackIds
		{
			get;
			private set;
		}

		public static int COLUMN_affectedBuildingIds
		{
			get;
			private set;
		}

		public static int COLUMN_buffUids
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string[] AffectedTroopIds
		{
			get;
			set;
		}

		public string[] AffectedSpecialAttackIds
		{
			get;
			set;
		}

		public string[] AffectedBuildingIds
		{
			get;
			set;
		}

		public string[] BuffUids
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.AffectedTroopIds = row.TryGetStringArray(EquipmentEffectVO.COLUMN_affectedTroopIds);
			this.AffectedSpecialAttackIds = row.TryGetStringArray(EquipmentEffectVO.COLUMN_affectedSpecialAttackIds);
			this.AffectedBuildingIds = row.TryGetStringArray(EquipmentEffectVO.COLUMN_affectedBuildingIds);
			this.BuffUids = row.TryGetStringArray(EquipmentEffectVO.COLUMN_buffUids);
		}

		public EquipmentEffectVO()
		{
		}

		protected internal EquipmentEffectVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentEffectVO)GCHandledObjects.GCHandleToObject(instance)).AffectedBuildingIds);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentEffectVO)GCHandledObjects.GCHandleToObject(instance)).AffectedSpecialAttackIds);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentEffectVO)GCHandledObjects.GCHandleToObject(instance)).AffectedTroopIds);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentEffectVO)GCHandledObjects.GCHandleToObject(instance)).BuffUids);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentEffectVO.COLUMN_affectedBuildingIds);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentEffectVO.COLUMN_affectedSpecialAttackIds);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentEffectVO.COLUMN_affectedTroopIds);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentEffectVO.COLUMN_buffUids);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentEffectVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((EquipmentEffectVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((EquipmentEffectVO)GCHandledObjects.GCHandleToObject(instance)).AffectedBuildingIds = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((EquipmentEffectVO)GCHandledObjects.GCHandleToObject(instance)).AffectedSpecialAttackIds = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((EquipmentEffectVO)GCHandledObjects.GCHandleToObject(instance)).AffectedTroopIds = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((EquipmentEffectVO)GCHandledObjects.GCHandleToObject(instance)).BuffUids = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			EquipmentEffectVO.COLUMN_affectedBuildingIds = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			EquipmentEffectVO.COLUMN_affectedSpecialAttackIds = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			EquipmentEffectVO.COLUMN_affectedTroopIds = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			EquipmentEffectVO.COLUMN_buffUids = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((EquipmentEffectVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
