using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class RewardVO : IValueObject
	{
		public static int COLUMN_currencyRewards
		{
			get;
			private set;
		}

		public static int COLUMN_shardRewards
		{
			get;
			private set;
		}

		public static int COLUMN_buildingUnlocks
		{
			get;
			private set;
		}

		public static int COLUMN_troopRewards
		{
			get;
			private set;
		}

		public static int COLUMN_troopUnlocks
		{
			get;
			private set;
		}

		public static int COLUMN_specAttackRewards
		{
			get;
			private set;
		}

		public static int COLUMN_specAttackUnlocks
		{
			get;
			private set;
		}

		public static int COLUMN_heroRewards
		{
			get;
			private set;
		}

		public static int COLUMN_heroUnlocks
		{
			get;
			private set;
		}

		public static int COLUMN_droids
		{
			get;
			private set;
		}

		public static int COLUMN_protectionReward
		{
			get;
			private set;
		}

		public static int COLUMN_buildingInstantRewards
		{
			get;
			private set;
		}

		public static int COLUMN_buildingInstantUpgrades
		{
			get;
			private set;
		}

		public static int COLUMN_troopResearchInstantUpgrades
		{
			get;
			private set;
		}

		public static int COLUMN_heroResearchInstantUpgrades
		{
			get;
			private set;
		}

		public static int COLUMN_specAttackResearchInstantUpgrades
		{
			get;
			private set;
		}

		public static int COLUMN_crateReward
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string[] CurrencyRewards
		{
			get;
			set;
		}

		public string[] ShardRewards
		{
			get;
			set;
		}

		public string[] BuildingUnlocks
		{
			get;
			set;
		}

		public string[] TroopRewards
		{
			get;
			set;
		}

		public string[] TroopUnlocks
		{
			get;
			set;
		}

		public string[] SpecialAttackRewards
		{
			get;
			set;
		}

		public string[] SpecialAttackUnlocks
		{
			get;
			set;
		}

		public string[] HeroRewards
		{
			get;
			set;
		}

		public string[] HeroUnlocks
		{
			get;
			set;
		}

		public string[] BuildingInstantRewards
		{
			get;
			set;
		}

		public string[] BuildingInstantUpgrades
		{
			get;
			set;
		}

		public string[] TroopResearchInstantUpgrades
		{
			get;
			set;
		}

		public string[] HeroResearchInstantUpgrades
		{
			get;
			set;
		}

		public string[] SpecAttackResearchInstantUpgrades
		{
			get;
			set;
		}

		public string DroidRewards
		{
			get;
			set;
		}

		public string ProtectionRewards
		{
			get;
			set;
		}

		public string CrateReward
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.CurrencyRewards = row.TryGetStringArray(RewardVO.COLUMN_currencyRewards);
			this.ShardRewards = row.TryGetStringArray(RewardVO.COLUMN_shardRewards);
			this.BuildingUnlocks = row.TryGetStringArray(RewardVO.COLUMN_buildingUnlocks);
			this.TroopRewards = row.TryGetStringArray(RewardVO.COLUMN_troopRewards);
			this.TroopUnlocks = row.TryGetStringArray(RewardVO.COLUMN_troopUnlocks);
			this.SpecialAttackRewards = row.TryGetStringArray(RewardVO.COLUMN_specAttackRewards);
			this.SpecialAttackUnlocks = row.TryGetStringArray(RewardVO.COLUMN_specAttackUnlocks);
			this.HeroRewards = row.TryGetStringArray(RewardVO.COLUMN_heroRewards);
			this.HeroUnlocks = row.TryGetStringArray(RewardVO.COLUMN_heroUnlocks);
			this.DroidRewards = row.TryGetString(RewardVO.COLUMN_droids);
			this.ProtectionRewards = row.TryGetString(RewardVO.COLUMN_protectionReward);
			this.BuildingInstantRewards = row.TryGetStringArray(RewardVO.COLUMN_buildingInstantRewards);
			this.BuildingInstantUpgrades = row.TryGetStringArray(RewardVO.COLUMN_buildingInstantUpgrades);
			this.TroopResearchInstantUpgrades = row.TryGetStringArray(RewardVO.COLUMN_troopResearchInstantUpgrades);
			this.HeroResearchInstantUpgrades = row.TryGetStringArray(RewardVO.COLUMN_heroResearchInstantUpgrades);
			this.SpecAttackResearchInstantUpgrades = row.TryGetStringArray(RewardVO.COLUMN_specAttackResearchInstantUpgrades);
			this.CrateReward = row.TryGetString(RewardVO.COLUMN_crateReward);
		}

		public RewardVO()
		{
		}

		protected internal RewardVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).BuildingInstantRewards);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).BuildingInstantUpgrades);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).BuildingUnlocks);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_buildingInstantRewards);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_buildingInstantUpgrades);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_buildingUnlocks);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_crateReward);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_currencyRewards);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_droids);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_heroResearchInstantUpgrades);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_heroRewards);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_heroUnlocks);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_protectionReward);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_shardRewards);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_specAttackResearchInstantUpgrades);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_specAttackRewards);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_specAttackUnlocks);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_troopResearchInstantUpgrades);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_troopRewards);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RewardVO.COLUMN_troopUnlocks);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).CrateReward);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).CurrencyRewards);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).DroidRewards);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).HeroResearchInstantUpgrades);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).HeroRewards);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).HeroUnlocks);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).ProtectionRewards);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).ShardRewards);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).SpecAttackResearchInstantUpgrades);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackRewards);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackUnlocks);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).TroopResearchInstantUpgrades);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).TroopRewards);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).TroopUnlocks);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).BuildingInstantRewards = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).BuildingInstantUpgrades = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).BuildingUnlocks = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			RewardVO.COLUMN_buildingInstantRewards = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			RewardVO.COLUMN_buildingInstantUpgrades = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			RewardVO.COLUMN_buildingUnlocks = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			RewardVO.COLUMN_crateReward = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			RewardVO.COLUMN_currencyRewards = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			RewardVO.COLUMN_droids = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			RewardVO.COLUMN_heroResearchInstantUpgrades = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			RewardVO.COLUMN_heroRewards = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			RewardVO.COLUMN_heroUnlocks = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			RewardVO.COLUMN_protectionReward = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			RewardVO.COLUMN_shardRewards = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			RewardVO.COLUMN_specAttackResearchInstantUpgrades = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			RewardVO.COLUMN_specAttackRewards = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			RewardVO.COLUMN_specAttackUnlocks = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			RewardVO.COLUMN_troopResearchInstantUpgrades = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			RewardVO.COLUMN_troopRewards = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			RewardVO.COLUMN_troopUnlocks = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).CrateReward = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).CurrencyRewards = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).DroidRewards = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).HeroResearchInstantUpgrades = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).HeroRewards = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).HeroUnlocks = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).ProtectionRewards = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).ShardRewards = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).SpecAttackResearchInstantUpgrades = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackRewards = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackUnlocks = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).TroopResearchInstantUpgrades = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).TroopRewards = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).TroopUnlocks = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			((RewardVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
