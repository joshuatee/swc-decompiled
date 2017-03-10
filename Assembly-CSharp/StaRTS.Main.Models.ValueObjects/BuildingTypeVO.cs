using StaRTS.Main.Controllers;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class BuildingTypeVO : IHealthVO, IAudioVO, IUpgradeableVO, IValueObject, IAssetVO, IGeometryVO, IUnlockableVO
	{
		public ArmorType ArmorType;

		public string BuildingID;

		public int Credits;

		public int Materials;

		public int Contraband;

		public int SwapCredits;

		public int SwapMaterials;

		public int SwapContraband;

		public int SwapTime;

		public int CycleTime;

		public int Produce;

		public int SizeX;

		public int SizeY;

		public int Storage;

		public int Xp;

		public CurrencyType Currency;

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

		public static int COLUMN_armorType
		{
			get;
			private set;
		}

		public static int COLUMN_faction
		{
			get;
			private set;
		}

		public static int COLUMN_buildingID
		{
			get;
			private set;
		}

		public static int COLUMN_type
		{
			get;
			private set;
		}

		public static int COLUMN_subType
		{
			get;
			private set;
		}

		public static int COLUMN_storeTab
		{
			get;
			private set;
		}

		public static int COLUMN_credits
		{
			get;
			private set;
		}

		public static int COLUMN_materials
		{
			get;
			private set;
		}

		public static int COLUMN_contraband
		{
			get;
			private set;
		}

		public static int COLUMN_crossCredits
		{
			get;
			private set;
		}

		public static int COLUMN_crossMaterials
		{
			get;
			private set;
		}

		public static int COLUMN_crossContraband
		{
			get;
			private set;
		}

		public static int COLUMN_crossTime
		{
			get;
			private set;
		}

		public static int COLUMN_cycleTime
		{
			get;
			private set;
		}

		public static int COLUMN_collectNotify
		{
			get;
			private set;
		}

		public static int COLUMN_time
		{
			get;
			private set;
		}

		public static int COLUMN_health
		{
			get;
			private set;
		}

		public static int COLUMN_shieldHealthPoints
		{
			get;
			private set;
		}

		public static int COLUMN_shieldRangePoints
		{
			get;
			private set;
		}

		public static int COLUMN_produce
		{
			get;
			private set;
		}

		public static int COLUMN_hideIfLocked
		{
			get;
			private set;
		}

		public static int COLUMN_requirements
		{
			get;
			private set;
		}

		public static int COLUMN_requirements2
		{
			get;
			private set;
		}

		public static int COLUMN_unlockedByEvent
		{
			get;
			private set;
		}

		public static int COLUMN_linkedUnit
		{
			get;
			private set;
		}

		public static int COLUMN_maxQuantity
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

		public static int COLUMN_order
		{
			get;
			private set;
		}

		public static int COLUMN_stashOrder
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

		public static int COLUMN_spawnProtect
		{
			get;
			private set;
		}

		public static int COLUMN_allowDefensiveSpawn
		{
			get;
			private set;
		}

		public static int COLUMN_xp
		{
			get;
			private set;
		}

		public static int COLUMN_storage
		{
			get;
			private set;
		}

		public static int COLUMN_currency
		{
			get;
			private set;
		}

		public static int COLUMN_audioDeath
		{
			get;
			private set;
		}

		public static int COLUMN_audioPlacement
		{
			get;
			private set;
		}

		public static int COLUMN_audioCharge
		{
			get;
			private set;
		}

		public static int COLUMN_audioAttack
		{
			get;
			private set;
		}

		public static int COLUMN_audioImpact
		{
			get;
			private set;
		}

		public static int COLUMN_turretId
		{
			get;
			private set;
		}

		public static int COLUMN_trapID
		{
			get;
			private set;
		}

		public static int COLUMN_activationRadius
		{
			get;
			private set;
		}

		public static int COLUMN_fillStateAssetName
		{
			get;
			private set;
		}

		public static int COLUMN_fillStateBundleName
		{
			get;
			private set;
		}

		public static int COLUMN_destructFX
		{
			get;
			private set;
		}

		public static int COLUMN_tooltipHeightOffset
		{
			get;
			private set;
		}

		public static int COLUMN_buffAssetOffset
		{
			get;
			private set;
		}

		public static int COLUMN_buffAssetBaseOffset
		{
			get;
			private set;
		}

		public static int COLUMN_lvl
		{
			get;
			private set;
		}

		public static int COLUMN_connectors
		{
			get;
			private set;
		}

		public static int COLUMN_forceShowReticle
		{
			get;
			private set;
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

		public string TrackerName
		{
			get;
			set;
		}

		public BuildingType Type
		{
			get;
			private set;
		}

		public BuildingSubType SubType
		{
			get;
			private set;
		}

		public StoreTab StoreTab
		{
			get;
			set;
		}

		public int CollectNotify
		{
			get;
			set;
		}

		public int Time
		{
			get;
			set;
		}

		public int Health
		{
			get;
			set;
		}

		public int ShieldHealthPoints
		{
			get;
			set;
		}

		public int ShieldRangePoints
		{
			get;
			set;
		}

		public bool HideIfLocked
		{
			get;
			private set;
		}

		public bool PlayerFacing
		{
			get
			{
				return true;
			}
		}

		public string BuildingRequirement
		{
			get;
			set;
		}

		public string BuildingRequirement2
		{
			get;
			private set;
		}

		public bool UnlockedByEvent
		{
			get;
			set;
		}

		public int MaxQuantity
		{
			get;
			private set;
		}

		public string LinkedUnit
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

		public float IconRotationSpeed
		{
			get;
			set;
		}

		public List<StrIntPair> AudioPlacement
		{
			get;
			set;
		}

		public List<StrIntPair> AudioDeath
		{
			get;
			set;
		}

		public List<StrIntPair> AudioMovement
		{
			get;
			set;
		}

		public List<StrIntPair> AudioMovementAway
		{
			get;
			set;
		}

		public List<StrIntPair> AudioTrain
		{
			get;
			set;
		}

		public List<StrIntPair> AudioCharge
		{
			get;
			set;
		}

		public List<StrIntPair> AudioAttack
		{
			get;
			set;
		}

		public List<StrIntPair> AudioImpact
		{
			get;
			set;
		}

		public string DestructFX
		{
			get;
			set;
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

		public int StashOrder
		{
			get;
			set;
		}

		public int Size
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public int UpgradeCredits
		{
			get
			{
				return this.Credits;
			}
		}

		public int UpgradeMaterials
		{
			get
			{
				return this.Materials;
			}
		}

		public int UpgradeContraband
		{
			get
			{
				return this.Contraband;
			}
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
				return this.BuildingID;
			}
		}

		public BuildingConnectorTypeVO Connectors
		{
			get;
			set;
		}

		public bool IsLootable
		{
			get;
			private set;
		}

		public int SpawnProtection
		{
			get;
			set;
		}

		public bool AllowDefensiveSpawn
		{
			get;
			private set;
		}

		public string TurretUid
		{
			get;
			private set;
		}

		public string TrapUid
		{
			get;
			private set;
		}

		public uint ActivationRadius
		{
			get;
			private set;
		}

		public string FillStateAssetName
		{
			get;
			private set;
		}

		public string FillStateBundleName
		{
			get;
			private set;
		}

		public float TooltipHeightOffset
		{
			get;
			private set;
		}

		public Vector3 BuffAssetOffset
		{
			get;
			set;
		}

		public Vector3 BuffAssetBaseOffset
		{
			get;
			set;
		}

		public bool ShowReticleWhenTargeted
		{
			get;
			private set;
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

		public void ReadRow(Row row)
		{
			this.AssetName = row.TryGetString(BuildingTypeVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(BuildingTypeVO.COLUMN_bundleName);
			this.IconAssetName = row.TryGetString(BuildingTypeVO.COLUMN_iconAssetName, this.AssetName);
			this.IconBundleName = row.TryGetString(BuildingTypeVO.COLUMN_iconBundleName, this.BundleName);
			this.ArmorType = StringUtils.ParseEnum<ArmorType>(row.TryGetString(BuildingTypeVO.COLUMN_armorType));
			this.Uid = row.Uid;
			this.Faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(BuildingTypeVO.COLUMN_faction));
			this.BuildingID = row.TryGetString(BuildingTypeVO.COLUMN_buildingID);
			this.Type = StringUtils.ParseEnum<BuildingType>(row.TryGetString(BuildingTypeVO.COLUMN_type));
			this.SubType = StringUtils.ParseEnum<BuildingSubType>(row.TryGetString(BuildingTypeVO.COLUMN_subType));
			this.StoreTab = StringUtils.ParseEnum<StoreTab>(row.TryGetString(BuildingTypeVO.COLUMN_storeTab));
			this.Credits = row.TryGetInt(BuildingTypeVO.COLUMN_credits);
			this.Materials = row.TryGetInt(BuildingTypeVO.COLUMN_materials);
			this.Contraband = row.TryGetInt(BuildingTypeVO.COLUMN_contraband);
			this.SwapCredits = row.TryGetInt(BuildingTypeVO.COLUMN_crossCredits);
			this.SwapMaterials = row.TryGetInt(BuildingTypeVO.COLUMN_crossMaterials);
			this.SwapContraband = row.TryGetInt(BuildingTypeVO.COLUMN_crossContraband);
			this.SwapTime = row.TryGetInt(BuildingTypeVO.COLUMN_crossTime);
			this.CycleTime = row.TryGetInt(BuildingTypeVO.COLUMN_cycleTime);
			this.CollectNotify = row.TryGetInt(BuildingTypeVO.COLUMN_collectNotify);
			this.Time = row.TryGetInt(BuildingTypeVO.COLUMN_time);
			this.Health = row.TryGetInt(BuildingTypeVO.COLUMN_health);
			this.ShieldHealthPoints = row.TryGetInt(BuildingTypeVO.COLUMN_shieldHealthPoints);
			this.ShieldRangePoints = row.TryGetInt(BuildingTypeVO.COLUMN_shieldRangePoints);
			this.Produce = row.TryGetInt(BuildingTypeVO.COLUMN_produce);
			this.HideIfLocked = row.TryGetBool(BuildingTypeVO.COLUMN_hideIfLocked);
			string[] array = row.TryGetStringArray(BuildingTypeVO.COLUMN_requirements);
			this.BuildingRequirement = ((array == null || array.Length == 0) ? null : array[0]);
			this.BuildingRequirement2 = row.TryGetString(BuildingTypeVO.COLUMN_requirements2);
			this.UnlockedByEvent = row.TryGetBool(BuildingTypeVO.COLUMN_unlockedByEvent);
			this.LinkedUnit = row.TryGetString(BuildingTypeVO.COLUMN_linkedUnit);
			this.MaxQuantity = row.TryGetInt(BuildingTypeVO.COLUMN_maxQuantity);
			this.IconCameraPosition = row.TryGetVector3(BuildingTypeVO.COLUMN_iconCameraPosition);
			this.IconLookatPosition = row.TryGetVector3(BuildingTypeVO.COLUMN_iconLookatPosition);
			this.IconCloseupCameraPosition = row.TryGetVector3(BuildingTypeVO.COLUMN_iconCloseupCameraPosition, this.IconCameraPosition);
			this.IconCloseupLookatPosition = row.TryGetVector3(BuildingTypeVO.COLUMN_iconCloseupLookatPosition, this.IconLookatPosition);
			this.Order = row.TryGetInt(BuildingTypeVO.COLUMN_order);
			this.StashOrder = row.TryGetInt(BuildingTypeVO.COLUMN_stashOrder);
			this.SizeX = row.TryGetInt(BuildingTypeVO.COLUMN_sizex);
			this.SizeY = row.TryGetInt(BuildingTypeVO.COLUMN_sizey);
			this.SpawnProtection = row.TryGetInt(BuildingTypeVO.COLUMN_spawnProtect);
			this.AllowDefensiveSpawn = row.TryGetBool(BuildingTypeVO.COLUMN_allowDefensiveSpawn);
			this.Xp = row.TryGetInt(BuildingTypeVO.COLUMN_xp);
			this.Storage = row.TryGetInt(BuildingTypeVO.COLUMN_storage);
			this.Currency = StringUtils.ParseEnum<CurrencyType>(row.TryGetString(BuildingTypeVO.COLUMN_currency));
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			this.AudioDeath = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(BuildingTypeVO.COLUMN_audioDeath));
			this.AudioPlacement = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(BuildingTypeVO.COLUMN_audioPlacement));
			this.AudioCharge = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(BuildingTypeVO.COLUMN_audioCharge));
			this.AudioAttack = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(BuildingTypeVO.COLUMN_audioAttack));
			this.AudioImpact = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(BuildingTypeVO.COLUMN_audioImpact));
			this.TurretUid = row.TryGetString(BuildingTypeVO.COLUMN_turretId);
			this.TrapUid = row.TryGetString(BuildingTypeVO.COLUMN_trapID);
			this.ActivationRadius = row.TryGetUint(BuildingTypeVO.COLUMN_activationRadius);
			this.FillStateAssetName = row.TryGetString(BuildingTypeVO.COLUMN_fillStateAssetName);
			this.FillStateBundleName = row.TryGetString(BuildingTypeVO.COLUMN_fillStateBundleName);
			this.DestructFX = row.TryGetString(BuildingTypeVO.COLUMN_destructFX);
			this.TooltipHeightOffset = row.TryGetFloat(BuildingTypeVO.COLUMN_tooltipHeightOffset);
			this.BuffAssetOffset = row.TryGetVector3(BuildingTypeVO.COLUMN_buffAssetOffset);
			this.BuffAssetBaseOffset = row.TryGetVector3(BuildingTypeVO.COLUMN_buffAssetBaseOffset, Vector3.zero);
			this.ShowReticleWhenTargeted = row.TryGetBool(BuildingTypeVO.COLUMN_forceShowReticle);
			this.Lvl = row.TryGetInt(BuildingTypeVO.COLUMN_lvl);
			this.UpgradeTime = ((this.Lvl == 1) ? 0 : this.Time);
			BuildingType type = this.Type;
			if (type == BuildingType.HQ || type == BuildingType.Resource || type == BuildingType.Storage)
			{
				this.IsLootable = true;
			}
			else
			{
				this.IsLootable = false;
			}
			string text = row.TryGetString(BuildingTypeVO.COLUMN_connectors);
			if (!string.IsNullOrEmpty(text))
			{
				this.Connectors = Service.Get<IDataController>().Get<BuildingConnectorTypeVO>(text);
			}
			this.IconUnlockScale = Vector3.one;
			this.IconUnlockRotation = Vector3.zero;
			this.IconUnlockPosition = Vector3.zero;
		}

		public BuildingTypeVO()
		{
		}

		protected internal BuildingTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AllowDefensiveSpawn);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioAttack);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioCharge);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioDeath);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioImpact);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovement);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovementAway);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioPlacement);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioTrain);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuffAssetBaseOffset);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuffAssetOffset);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuildingRequirement);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuildingRequirement2);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).CollectNotify);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_activationRadius);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_allowDefensiveSpawn);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_armorType);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_audioAttack);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_audioCharge);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_audioDeath);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_audioImpact);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_audioPlacement);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_buffAssetBaseOffset);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_buffAssetOffset);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_buildingID);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_collectNotify);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_connectors);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_contraband);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_credits);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_crossContraband);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_crossCredits);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_crossMaterials);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_crossTime);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_currency);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_cycleTime);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_destructFX);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_faction);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_fillStateAssetName);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_fillStateBundleName);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_forceShowReticle);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_health);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_hideIfLocked);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_iconAssetName);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_iconBundleName);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_iconCameraPosition);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_iconCloseupCameraPosition);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_iconCloseupLookatPosition);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_iconLookatPosition);
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_linkedUnit);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_lvl);
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_materials);
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_maxQuantity);
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_order);
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_produce);
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_requirements);
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_requirements2);
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_shieldHealthPoints);
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_shieldRangePoints);
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_sizex);
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_sizey);
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_spawnProtect);
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_stashOrder);
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_storage);
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_storeTab);
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_subType);
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_time);
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_tooltipHeightOffset);
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_trapID);
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_turretId);
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_type);
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_unlockedByEvent);
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuildingTypeVO.COLUMN_xp);
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Connectors);
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).DestructFX);
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).FillStateAssetName);
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).FillStateBundleName);
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Health);
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).HideIfLocked);
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName);
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName);
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition);
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition);
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition);
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition);
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed);
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockPosition);
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockRotation);
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockScale);
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsLootable);
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).LinkedUnit);
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Lvl);
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxQuantity);
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Order);
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).PlayerFacing);
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).ShieldHealthPoints);
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).ShieldRangePoints);
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).ShowReticleWhenTargeted);
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Size);
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).SpawnProtection);
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).StashOrder);
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).StoreTab);
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).SubType);
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Time);
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).TooltipHeightOffset);
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).TrackerName);
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).TrapUid);
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).TurretUid);
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).UnlockedByEvent);
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeContraband);
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeCredits);
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeGroup);
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeMaterials);
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeTime);
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AllowDefensiveSpawn = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioAttack = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioCharge = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioDeath = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioImpact = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovement = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovementAway = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioPlacement = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioTrain = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuffAssetBaseOffset = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuffAssetOffset = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuildingRequirement = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuildingRequirement2 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).CollectNotify = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_activationRadius = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_allowDefensiveSpawn = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_armorType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke141(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_audioAttack = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke142(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_audioCharge = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke143(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_audioDeath = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke144(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_audioImpact = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke145(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_audioPlacement = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke146(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_buffAssetBaseOffset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke147(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_buffAssetOffset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke148(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_buildingID = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke149(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke150(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_collectNotify = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke151(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_connectors = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke152(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_contraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke153(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_credits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke154(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_crossContraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke155(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_crossCredits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke156(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_crossMaterials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke157(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_crossTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke158(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_currency = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke159(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_cycleTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke160(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_destructFX = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke161(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke162(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_fillStateAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke163(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_fillStateBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke164(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_forceShowReticle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke165(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_health = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke166(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_hideIfLocked = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke167(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_iconAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke168(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_iconBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke169(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_iconCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke170(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_iconCloseupCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke171(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_iconCloseupLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke172(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_iconLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke173(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_linkedUnit = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke174(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_lvl = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke175(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_materials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke176(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_maxQuantity = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke177(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke178(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_produce = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke179(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_requirements = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke180(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_requirements2 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke181(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_shieldHealthPoints = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke182(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_shieldRangePoints = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke183(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_sizex = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke184(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_sizey = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke185(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_spawnProtect = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke186(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_stashOrder = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke187(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_storage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke188(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_storeTab = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke189(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_subType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke190(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_time = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke191(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_tooltipHeightOffset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke192(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_trapID = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke193(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_turretId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke194(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_type = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke195(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_unlockedByEvent = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke196(long instance, long* args)
		{
			BuildingTypeVO.COLUMN_xp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke197(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Connectors = (BuildingConnectorTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke198(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).DestructFX = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke199(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke200(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).FillStateAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke201(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).FillStateBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke202(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Health = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke203(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).HideIfLocked = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke204(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke205(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke206(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke207(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke208(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke209(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke210(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke211(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke212(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockRotation = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke213(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockScale = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke214(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsLootable = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke215(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).LinkedUnit = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke216(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Lvl = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke217(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxQuantity = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke218(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke219(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).ShieldHealthPoints = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke220(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).ShieldRangePoints = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke221(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).ShowReticleWhenTargeted = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke222(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Size = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke223(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).SpawnProtection = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke224(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).StashOrder = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke225(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).StoreTab = (StoreTab)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke226(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).SubType = (BuildingSubType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke227(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Time = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke228(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).TooltipHeightOffset = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke229(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).TrackerName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke230(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).TrapUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke231(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).TurretUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke232(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Type = (BuildingType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke233(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke234(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).UnlockedByEvent = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke235(long instance, long* args)
		{
			((BuildingTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeTime = *(int*)args;
			return -1L;
		}
	}
}
