using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class EquipmentVO : IValueObject, IGeometryVO, IUpgradeableVO, IAssetVO, IUnlockableVO
	{
		public static int COLUMN_equipmentID
		{
			get;
			private set;
		}

		public static int COLUMN_lvl
		{
			get;
			private set;
		}

		public static int COLUMN_faction
		{
			get;
			private set;
		}

		public static int COLUMN_planetIDs
		{
			get;
			private set;
		}

		public static int COLUMN_effectUids
		{
			get;
			private set;
		}

		public static int COLUMN_upgradeShards
		{
			get;
			private set;
		}

		public static int COLUMN_size
		{
			get;
			private set;
		}

		public static int COLUMN_equipmentName
		{
			get;
			private set;
		}

		public static int COLUMN_equipmentDescription
		{
			get;
			private set;
		}

		public static int COLUMN_skins
		{
			get;
			private set;
		}

		public static int COLUMN_iconUnlockScale
		{
			get;
			private set;
		}

		public static int COLUMN_iconUnlockRotation
		{
			get;
			private set;
		}

		public static int COLUMN_iconUnlockPosition
		{
			get;
			private set;
		}

		public static int COLUMN_quality
		{
			get;
			private set;
		}

		public static int COLUMN_upgradeTime
		{
			get;
			private set;
		}

		public static int COLUMN_buildingID
		{
			get;
			private set;
		}

		public static int COLUMN_order
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string EquipmentID
		{
			get;
			private set;
		}

		public FactionType Faction
		{
			get;
			private set;
		}

		public string[] PlanetIDs
		{
			get;
			private set;
		}

		public string[] EffectUids
		{
			get;
			private set;
		}

		public int UpgradeShards
		{
			get;
			protected set;
		}

		public string EquipmentName
		{
			get;
			set;
		}

		public string EquipmentDescription
		{
			get;
			set;
		}

		public string[] Skins
		{
			get;
			set;
		}

		public ShardQuality Quality
		{
			get;
			set;
		}

		public string BuildingID
		{
			get;
			set;
		}

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

		public static int COLUMN_iconRotationSpeed
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

		public Vector3 IconUnlockScale
		{
			get;
			set;
		}

		public Vector3 IconUnlockRotation
		{
			get;
			set;
		}

		public Vector3 IconUnlockPosition
		{
			get;
			set;
		}

		public string IconBundleName
		{
			get;
			set;
		}

		public string IconAssetName
		{
			get;
			set;
		}

		public float IconRotationSpeed
		{
			get;
			set;
		}

		public static int COLUMN_playerFacing
		{
			get;
			private set;
		}

		public static int COLUMN_requirements
		{
			get;
			private set;
		}

		public int Lvl
		{
			get;
			set;
		}

		public int Order
		{
			get;
			set;
		}

		public int Size
		{
			get;
			set;
		}

		public int UpgradeCredits
		{
			get;
			set;
		}

		public int UpgradeMaterials
		{
			get;
			set;
		}

		public int UpgradeContraband
		{
			get;
			set;
		}

		public int UpgradeTime
		{
			get;
			set;
		}

		public string UpgradeGroup
		{
			get
			{
				return this.EquipmentID;
			}
		}

		public bool PlayerFacing
		{
			get;
			private set;
		}

		public string BuildingRequirement
		{
			get;
			set;
		}

		public bool UnlockedByEvent
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.EquipmentID = row.TryGetString(EquipmentVO.COLUMN_equipmentID);
			this.Lvl = row.TryGetInt(EquipmentVO.COLUMN_lvl);
			this.Faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(EquipmentVO.COLUMN_faction));
			this.PlanetIDs = row.TryGetStringArray(EquipmentVO.COLUMN_planetIDs);
			this.EffectUids = row.TryGetStringArray(EquipmentVO.COLUMN_effectUids);
			this.UpgradeShards = row.TryGetInt(EquipmentVO.COLUMN_upgradeShards);
			this.Size = row.TryGetInt(EquipmentVO.COLUMN_size);
			this.Quality = StringUtils.ParseEnum<ShardQuality>(row.TryGetString(EquipmentVO.COLUMN_quality));
			this.IconUnlockScale = row.TryGetVector3(EquipmentVO.COLUMN_iconUnlockScale, Vector3.one);
			this.IconUnlockRotation = row.TryGetVector3(EquipmentVO.COLUMN_iconUnlockRotation, Vector3.zero);
			this.IconUnlockPosition = row.TryGetVector3(EquipmentVO.COLUMN_iconUnlockPosition, Vector3.zero);
			this.UpgradeTime = row.TryGetInt(EquipmentVO.COLUMN_upgradeTime);
			this.BuildingID = row.TryGetString(EquipmentVO.COLUMN_buildingID);
			this.AssetName = row.TryGetString(EquipmentVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(EquipmentVO.COLUMN_bundleName);
			this.IconAssetName = row.TryGetString(EquipmentVO.COLUMN_iconAssetName, this.AssetName);
			this.IconBundleName = row.TryGetString(EquipmentVO.COLUMN_iconBundleName, this.BundleName);
			this.IconCameraPosition = row.TryGetVector3(EquipmentVO.COLUMN_iconCameraPosition);
			this.IconLookatPosition = row.TryGetVector3(EquipmentVO.COLUMN_iconLookatPosition);
			this.IconCloseupCameraPosition = row.TryGetVector3(EquipmentVO.COLUMN_iconCloseupCameraPosition, this.IconCameraPosition);
			this.IconCloseupLookatPosition = row.TryGetVector3(EquipmentVO.COLUMN_iconCloseupLookatPosition, this.IconLookatPosition);
			this.IconRotationSpeed = row.TryGetFloat(EquipmentVO.COLUMN_iconRotationSpeed);
			this.PlayerFacing = row.TryGetBool(EquipmentVO.COLUMN_playerFacing);
			string[] array = row.TryGetStringArray(EquipmentVO.COLUMN_requirements);
			this.BuildingRequirement = ((array == null || array.Length == 0) ? null : array[0]);
			this.EquipmentName = row.TryGetString(EquipmentVO.COLUMN_equipmentName, this.BundleName);
			this.EquipmentDescription = row.TryGetString(EquipmentVO.COLUMN_equipmentDescription, this.BundleName);
			this.Skins = row.TryGetStringArray(EquipmentVO.COLUMN_skins);
			this.Order = row.TryGetInt(EquipmentVO.COLUMN_order);
		}

		public EquipmentVO()
		{
		}

		protected internal EquipmentVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).BuildingID);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).BuildingRequirement);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_buildingID);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_effectUids);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_equipmentDescription);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_equipmentID);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_equipmentName);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_faction);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_iconAssetName);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_iconBundleName);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_iconCameraPosition);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_iconCloseupCameraPosition);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_iconCloseupLookatPosition);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_iconLookatPosition);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_iconRotationSpeed);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_iconUnlockPosition);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_iconUnlockRotation);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_iconUnlockScale);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_lvl);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_order);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_planetIDs);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_playerFacing);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_quality);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_requirements);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_size);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_skins);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_upgradeShards);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EquipmentVO.COLUMN_upgradeTime);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).EffectUids);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).EquipmentDescription);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).EquipmentID);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).EquipmentName);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockPosition);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockRotation);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockScale);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Lvl);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Order);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).PlanetIDs);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).PlayerFacing);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Quality);
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Size);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Skins);
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UnlockedByEvent);
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeContraband);
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeCredits);
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeGroup);
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeMaterials);
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShards);
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeTime);
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).BuildingID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).BuildingRequirement = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			EquipmentVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			EquipmentVO.COLUMN_buildingID = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			EquipmentVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			EquipmentVO.COLUMN_effectUids = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			EquipmentVO.COLUMN_equipmentDescription = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			EquipmentVO.COLUMN_equipmentID = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			EquipmentVO.COLUMN_equipmentName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			EquipmentVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			EquipmentVO.COLUMN_iconAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			EquipmentVO.COLUMN_iconBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			EquipmentVO.COLUMN_iconCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			EquipmentVO.COLUMN_iconCloseupCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			EquipmentVO.COLUMN_iconCloseupLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			EquipmentVO.COLUMN_iconLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			EquipmentVO.COLUMN_iconRotationSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			EquipmentVO.COLUMN_iconUnlockPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			EquipmentVO.COLUMN_iconUnlockRotation = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			EquipmentVO.COLUMN_iconUnlockScale = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			EquipmentVO.COLUMN_lvl = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			EquipmentVO.COLUMN_order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			EquipmentVO.COLUMN_planetIDs = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			EquipmentVO.COLUMN_playerFacing = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			EquipmentVO.COLUMN_quality = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			EquipmentVO.COLUMN_requirements = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			EquipmentVO.COLUMN_size = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			EquipmentVO.COLUMN_skins = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			EquipmentVO.COLUMN_upgradeShards = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			EquipmentVO.COLUMN_upgradeTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).EffectUids = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).EquipmentDescription = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).EquipmentID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).EquipmentName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockRotation = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockScale = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Lvl = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).PlanetIDs = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).PlayerFacing = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Quality = (ShardQuality)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Size = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Skins = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UnlockedByEvent = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeContraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeCredits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeMaterials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShards = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			((EquipmentVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeTime = *(int*)args;
			return -1L;
		}
	}
}
