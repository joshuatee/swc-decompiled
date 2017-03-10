using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class PerkEffectVO : IValueObject
	{
		public static int COLUMN_type
		{
			get;
			private set;
		}

		public static int COLUMN_building
		{
			get;
			private set;
		}

		public static int COLUMN_currency
		{
			get;
			private set;
		}

		public static int COLUMN_generationRate
		{
			get;
			private set;
		}

		public static int COLUMN_contractTimeReduction
		{
			get;
			private set;
		}

		public static int COLUMN_contractDiscount
		{
			get;
			private set;
		}

		public static int COLUMN_relocationDiscount
		{
			get;
			private set;
		}

		public static int COLUMN_troopRequestAmount
		{
			get;
			private set;
		}

		public static int COLUMN_troopRequestTimeDiscount
		{
			get;
			private set;
		}

		public static int COLUMN_stringId
		{
			get;
			private set;
		}

		public static int COLUMN_stringModId
		{
			get;
			private set;
		}

		public static int COLUMN_stringUpgradeModId
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string Type
		{
			get;
			private set;
		}

		public BuildingType PerkBuilding
		{
			get;
			private set;
		}

		public CurrencyType Currency
		{
			get;
			private set;
		}

		public float GenerationRate
		{
			get;
			private set;
		}

		public float ContractTimeReduction
		{
			get;
			private set;
		}

		public float ContractDiscount
		{
			get;
			private set;
		}

		public int RelocationDiscount
		{
			get;
			private set;
		}

		public int TroopRequestAmount
		{
			get;
			private set;
		}

		public int TroopRequestTimeDiscount
		{
			get;
			private set;
		}

		public string StatStringId
		{
			get;
			private set;
		}

		public string StatValueFormatStringId
		{
			get;
			private set;
		}

		public string StatUpgradeValueFormatStringId
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Type = row.TryGetString(PerkEffectVO.COLUMN_type);
			this.PerkBuilding = StringUtils.ParseEnum<BuildingType>(row.TryGetString(PerkEffectVO.COLUMN_building));
			this.Currency = StringUtils.ParseEnum<CurrencyType>(row.TryGetString(PerkEffectVO.COLUMN_currency));
			this.GenerationRate = row.TryGetFloat(PerkEffectVO.COLUMN_generationRate);
			this.ContractTimeReduction = row.TryGetFloat(PerkEffectVO.COLUMN_contractTimeReduction);
			this.ContractDiscount = row.TryGetFloat(PerkEffectVO.COLUMN_contractDiscount);
			this.RelocationDiscount = row.TryGetInt(PerkEffectVO.COLUMN_relocationDiscount);
			this.TroopRequestAmount = row.TryGetInt(PerkEffectVO.COLUMN_troopRequestAmount);
			this.TroopRequestTimeDiscount = row.TryGetInt(PerkEffectVO.COLUMN_troopRequestTimeDiscount);
			this.StatStringId = row.TryGetString(PerkEffectVO.COLUMN_stringId);
			this.StatValueFormatStringId = row.TryGetString(PerkEffectVO.COLUMN_stringModId);
			this.StatUpgradeValueFormatStringId = row.TryGetString(PerkEffectVO.COLUMN_stringUpgradeModId);
		}

		public PerkEffectVO()
		{
		}

		protected internal PerkEffectVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkEffectVO.COLUMN_building);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkEffectVO.COLUMN_contractDiscount);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkEffectVO.COLUMN_contractTimeReduction);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkEffectVO.COLUMN_currency);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkEffectVO.COLUMN_generationRate);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkEffectVO.COLUMN_relocationDiscount);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkEffectVO.COLUMN_stringId);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkEffectVO.COLUMN_stringModId);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkEffectVO.COLUMN_stringUpgradeModId);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkEffectVO.COLUMN_troopRequestAmount);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkEffectVO.COLUMN_troopRequestTimeDiscount);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkEffectVO.COLUMN_type);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).ContractDiscount);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).ContractTimeReduction);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).Currency);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).GenerationRate);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).PerkBuilding);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).RelocationDiscount);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).StatStringId);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).StatUpgradeValueFormatStringId);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).StatValueFormatStringId);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).TroopRequestAmount);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).TroopRequestTimeDiscount);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			PerkEffectVO.COLUMN_building = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			PerkEffectVO.COLUMN_contractDiscount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			PerkEffectVO.COLUMN_contractTimeReduction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			PerkEffectVO.COLUMN_currency = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			PerkEffectVO.COLUMN_generationRate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			PerkEffectVO.COLUMN_relocationDiscount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			PerkEffectVO.COLUMN_stringId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			PerkEffectVO.COLUMN_stringModId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			PerkEffectVO.COLUMN_stringUpgradeModId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			PerkEffectVO.COLUMN_troopRequestAmount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			PerkEffectVO.COLUMN_troopRequestTimeDiscount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			PerkEffectVO.COLUMN_type = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).ContractDiscount = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).ContractTimeReduction = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).Currency = (CurrencyType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).GenerationRate = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).PerkBuilding = (BuildingType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).RelocationDiscount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).StatStringId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).StatUpgradeValueFormatStringId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).StatValueFormatStringId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).TroopRequestAmount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).TroopRequestTimeDiscount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).Type = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((PerkEffectVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
