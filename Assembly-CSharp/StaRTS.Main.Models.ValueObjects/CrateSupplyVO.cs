using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class CrateSupplyVO : IValueObject
	{
		public static int COLUMN_crateSupplyPoolUid
		{
			get;
			private set;
		}

		public static int COLUMN_faction
		{
			get;
			private set;
		}

		public static int COLUMN_planet
		{
			get;
			private set;
		}

		public static int COLUMN_feature
		{
			get;
			private set;
		}

		public static int COLUMN_minHQ
		{
			get;
			private set;
		}

		public static int COLUMN_maxHQ
		{
			get;
			private set;
		}

		public static int COLUMN_rewardType
		{
			get;
			private set;
		}

		public static int COLUMN_rewardUid
		{
			get;
			private set;
		}

		public static int COLUMN_amount
		{
			get;
			private set;
		}

		public static int COLUMN_scalingUid
		{
			get;
			private set;
		}

		public static int COLUMN_dataCard
		{
			get;
			private set;
		}

		public static int COLUMN_rewardTierSfx
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string[] CrateSupplyPoolUid
		{
			get;
			private set;
		}

		public FactionType Faction
		{
			get;
			private set;
		}

		public string[] PlanetIds
		{
			get;
			private set;
		}

		public FeatureContextType[] FeatureContexts
		{
			get;
			private set;
		}

		public int MinHQLevel
		{
			get;
			private set;
		}

		public int MaxHQLevel
		{
			get;
			private set;
		}

		public SupplyType Type
		{
			get;
			private set;
		}

		public string RewardUid
		{
			get;
			private set;
		}

		public int Amount
		{
			get;
			private set;
		}

		public string ScalingUid
		{
			get;
			private set;
		}

		public string DataCardId
		{
			get;
			private set;
		}

		public string RewardAnimationTierSfx
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.CrateSupplyPoolUid = row.TryGetStringArray(CrateSupplyVO.COLUMN_crateSupplyPoolUid);
			this.Faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(CrateSupplyVO.COLUMN_faction));
			this.PlanetIds = row.TryGetStringArray(CrateSupplyVO.COLUMN_planet);
			string[] array = row.TryGetStringArray(CrateSupplyVO.COLUMN_feature);
			if (array != null)
			{
				int num = array.Length;
				this.FeatureContexts = new FeatureContextType[num];
				for (int i = 0; i < num; i++)
				{
					this.FeatureContexts[i] = StringUtils.ParseEnum<FeatureContextType>(array[i]);
				}
			}
			this.MinHQLevel = row.TryGetInt(CrateSupplyVO.COLUMN_minHQ);
			this.MaxHQLevel = row.TryGetInt(CrateSupplyVO.COLUMN_maxHQ);
			this.Type = StringUtils.ParseEnum<SupplyType>(row.TryGetString(CrateSupplyVO.COLUMN_rewardType));
			this.RewardUid = row.TryGetString(CrateSupplyVO.COLUMN_rewardUid);
			this.Amount = row.TryGetInt(CrateSupplyVO.COLUMN_amount);
			this.ScalingUid = row.TryGetString(CrateSupplyVO.COLUMN_scalingUid);
			this.DataCardId = row.TryGetString(CrateSupplyVO.COLUMN_dataCard);
			this.RewardAnimationTierSfx = row.TryGetString(CrateSupplyVO.COLUMN_rewardTierSfx);
		}

		public CrateSupplyVO()
		{
		}

		protected internal CrateSupplyVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).Amount);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyVO.COLUMN_amount);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyVO.COLUMN_crateSupplyPoolUid);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyVO.COLUMN_dataCard);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyVO.COLUMN_faction);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyVO.COLUMN_feature);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyVO.COLUMN_maxHQ);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyVO.COLUMN_minHQ);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyVO.COLUMN_planet);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyVO.COLUMN_rewardTierSfx);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyVO.COLUMN_rewardType);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyVO.COLUMN_rewardUid);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateSupplyVO.COLUMN_scalingUid);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).CrateSupplyPoolUid);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).DataCardId);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).FeatureContexts);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).MaxHQLevel);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).MinHQLevel);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).PlanetIds);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).RewardAnimationTierSfx);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).RewardUid);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).ScalingUid);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).Amount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			CrateSupplyVO.COLUMN_amount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			CrateSupplyVO.COLUMN_crateSupplyPoolUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			CrateSupplyVO.COLUMN_dataCard = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			CrateSupplyVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			CrateSupplyVO.COLUMN_feature = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			CrateSupplyVO.COLUMN_maxHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			CrateSupplyVO.COLUMN_minHQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			CrateSupplyVO.COLUMN_planet = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			CrateSupplyVO.COLUMN_rewardTierSfx = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			CrateSupplyVO.COLUMN_rewardType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			CrateSupplyVO.COLUMN_rewardUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			CrateSupplyVO.COLUMN_scalingUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).CrateSupplyPoolUid = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).DataCardId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).FeatureContexts = (FeatureContextType[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).MaxHQLevel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).MinHQLevel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).PlanetIds = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).RewardAnimationTierSfx = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).RewardUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).ScalingUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).Type = (SupplyType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((CrateSupplyVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
