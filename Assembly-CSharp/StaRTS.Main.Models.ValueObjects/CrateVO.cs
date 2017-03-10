using StaRTS.Utils.MetaData;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class CrateVO : IValueObject, IGeometryVO
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

		public static int COLUMN_vfxAssetName
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

		public static int COLUMN_crystals
		{
			get;
			private set;
		}

		public static int COLUMN_purchasable
		{
			get;
			private set;
		}

		public static int COLUMN_supplyPoolUid
		{
			get;
			private set;
		}

		public static int COLUMN_expirationTime
		{
			get;
			private set;
		}

		public static int COLUMN_storeVisibilityConditions
		{
			get;
			private set;
		}

		public static int COLUMN_storePurchasableConditions
		{
			get;
			private set;
		}

		public static int COLUMN_flyoutEmpireItems
		{
			get;
			private set;
		}

		public static int COLUMN_flyoutRebelItems
		{
			get;
			private set;
		}

		public static int COLUMN_rewardAnimAssetName
		{
			get;
			private set;
		}

		public static int COLUMN_crateLandTime
		{
			get;
			private set;
		}

		public static int COLUMN_empireLEIUid
		{
			get;
			private set;
		}

		public static int COLUMN_rebelLEIUid
		{
			get;
			private set;
		}

		public static int COLUMN_holoCameraPosition
		{
			get;
			private set;
		}

		public static int COLUMN_holoLookatPosition
		{
			get;
			private set;
		}

		public static int COLUMN_holoParticleEffect
		{
			get;
			private set;
		}

		public static int COLUMN_holoCrateShadow
		{
			get;
			private set;
		}

		public static int COLUMN_uiColor
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
			private set;
		}

		public string BundleName
		{
			get;
			private set;
		}

		public string VfxAssetName
		{
			get;
			private set;
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

		public Vector3 HoloNetIconCameraPostion
		{
			get;
			set;
		}

		public Vector3 HoloNetIconLookAtPostion
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

		public int Crystals
		{
			get;
			private set;
		}

		public bool Purchasable
		{
			get;
			private set;
		}

		public string[] SupplyPoolUIDs
		{
			get;
			private set;
		}

		public uint ExpirationTimeSec
		{
			get;
			private set;
		}

		public string[] StoreVisibilityConditions
		{
			get;
			set;
		}

		public string[] StorePurchasableConditions
		{
			get;
			set;
		}

		public string[] FlyoutEmpireItems
		{
			get;
			private set;
		}

		public string[] FlyoutRebelItems
		{
			get;
			private set;
		}

		public string RewardAnimationAssetName
		{
			get;
			private set;
		}

		public float CrateRewardAnimLandTime
		{
			get;
			private set;
		}

		public string EmpireLEIUid
		{
			get;
			private set;
		}

		public string RebelLEIUid
		{
			get;
			private set;
		}

		public string HoloParticleEffectId
		{
			get;
			private set;
		}

		public string HoloCrateShadowTextureName
		{
			get;
			private set;
		}

		public string UIColor
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.AssetName = row.TryGetString(CrateVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(CrateVO.COLUMN_bundleName);
			this.VfxAssetName = row.TryGetString(CrateVO.COLUMN_vfxAssetName);
			this.IconAssetName = row.TryGetString(CrateVO.COLUMN_iconAssetName, this.AssetName);
			this.IconBundleName = row.TryGetString(CrateVO.COLUMN_iconBundleName, this.BundleName);
			this.IconCameraPosition = row.TryGetVector3(CrateVO.COLUMN_iconCameraPosition);
			this.IconLookatPosition = row.TryGetVector3(CrateVO.COLUMN_iconLookatPosition);
			this.HoloNetIconCameraPostion = row.TryGetVector3(CrateVO.COLUMN_holoCameraPosition);
			this.HoloNetIconLookAtPostion = row.TryGetVector3(CrateVO.COLUMN_holoLookatPosition);
			this.IconCloseupCameraPosition = row.TryGetVector3(CrateVO.COLUMN_iconCloseupCameraPosition, this.IconCameraPosition);
			this.IconCloseupLookatPosition = row.TryGetVector3(CrateVO.COLUMN_iconCloseupLookatPosition, this.IconLookatPosition);
			this.Crystals = row.TryGetInt(CrateVO.COLUMN_crystals);
			this.Purchasable = row.TryGetBool(CrateVO.COLUMN_purchasable);
			this.SupplyPoolUIDs = row.TryGetStringArray(CrateVO.COLUMN_supplyPoolUid);
			this.ExpirationTimeSec = Convert.ToUInt32(row.TryGetInt(CrateVO.COLUMN_expirationTime) * 60, CultureInfo.InvariantCulture);
			this.StoreVisibilityConditions = row.TryGetStringArray(CrateVO.COLUMN_storeVisibilityConditions);
			this.StorePurchasableConditions = row.TryGetStringArray(CrateVO.COLUMN_storePurchasableConditions);
			this.FlyoutEmpireItems = row.TryGetStringArray(CrateVO.COLUMN_flyoutEmpireItems);
			this.FlyoutRebelItems = row.TryGetStringArray(CrateVO.COLUMN_flyoutRebelItems);
			this.RewardAnimationAssetName = row.TryGetString(CrateVO.COLUMN_rewardAnimAssetName);
			this.CrateRewardAnimLandTime = row.TryGetFloat(CrateVO.COLUMN_crateLandTime);
			this.EmpireLEIUid = row.TryGetString(CrateVO.COLUMN_empireLEIUid);
			this.RebelLEIUid = row.TryGetString(CrateVO.COLUMN_rebelLEIUid);
			this.HoloParticleEffectId = row.TryGetString(CrateVO.COLUMN_holoParticleEffect);
			this.HoloCrateShadowTextureName = row.TryGetString(CrateVO.COLUMN_holoCrateShadow);
			this.UIColor = row.TryGetHexValueString(CrateVO.COLUMN_uiColor);
		}

		public CrateVO()
		{
		}

		protected internal CrateVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_crateLandTime);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_crystals);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_empireLEIUid);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_expirationTime);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_flyoutEmpireItems);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_flyoutRebelItems);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_holoCameraPosition);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_holoCrateShadow);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_holoLookatPosition);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_holoParticleEffect);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_iconAssetName);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_iconBundleName);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_iconCameraPosition);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_iconCloseupCameraPosition);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_iconCloseupLookatPosition);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_iconLookatPosition);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_purchasable);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_rebelLEIUid);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_rewardAnimAssetName);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_storePurchasableConditions);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_storeVisibilityConditions);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_supplyPoolUid);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_uiColor);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateVO.COLUMN_vfxAssetName);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).CrateRewardAnimLandTime);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).Crystals);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).EmpireLEIUid);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).FlyoutEmpireItems);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).FlyoutRebelItems);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).HoloCrateShadowTextureName);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).HoloNetIconCameraPostion);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).HoloNetIconLookAtPostion);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).HoloParticleEffectId);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).Purchasable);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).RebelLEIUid);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).RewardAnimationAssetName);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).StorePurchasableConditions);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).StoreVisibilityConditions);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).SupplyPoolUIDs);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).UIColor);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateVO)GCHandledObjects.GCHandleToObject(instance)).VfxAssetName);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			CrateVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			CrateVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			CrateVO.COLUMN_crateLandTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			CrateVO.COLUMN_crystals = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			CrateVO.COLUMN_empireLEIUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			CrateVO.COLUMN_expirationTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			CrateVO.COLUMN_flyoutEmpireItems = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			CrateVO.COLUMN_flyoutRebelItems = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			CrateVO.COLUMN_holoCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			CrateVO.COLUMN_holoCrateShadow = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			CrateVO.COLUMN_holoLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			CrateVO.COLUMN_holoParticleEffect = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			CrateVO.COLUMN_iconAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			CrateVO.COLUMN_iconBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			CrateVO.COLUMN_iconCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			CrateVO.COLUMN_iconCloseupCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			CrateVO.COLUMN_iconCloseupLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			CrateVO.COLUMN_iconLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			CrateVO.COLUMN_purchasable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			CrateVO.COLUMN_rebelLEIUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			CrateVO.COLUMN_rewardAnimAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			CrateVO.COLUMN_storePurchasableConditions = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			CrateVO.COLUMN_storeVisibilityConditions = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			CrateVO.COLUMN_supplyPoolUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			CrateVO.COLUMN_uiColor = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			CrateVO.COLUMN_vfxAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).CrateRewardAnimLandTime = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).Crystals = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).EmpireLEIUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).FlyoutEmpireItems = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).FlyoutRebelItems = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).HoloCrateShadowTextureName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).HoloNetIconCameraPostion = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).HoloNetIconLookAtPostion = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).HoloParticleEffectId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).Purchasable = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).RebelLEIUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).RewardAnimationAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).StorePurchasableConditions = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).StoreVisibilityConditions = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).SupplyPoolUIDs = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).UIColor = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			((CrateVO)GCHandledObjects.GCHandleToObject(instance)).VfxAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
