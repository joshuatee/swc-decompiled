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
	public class SpecialAttackTypeVO : ISpeedVO, IAudioVO, IDeployableVO, IUpgradeableVO, IValueObject, IAssetVO, IGeometryVO, IUnlockableVO
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

		public static int COLUMN_faction
		{
			get;
			private set;
		}

		public static int COLUMN_specialAttackID
		{
			get;
			private set;
		}

		public static int COLUMN_specialAttackName
		{
			get;
			private set;
		}

		public static int COLUMN_size
		{
			get;
			private set;
		}

		public static int COLUMN_trainingTime
		{
			get;
			private set;
		}

		public static int COLUMN_xp
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

		public static int COLUMN_acceleration
		{
			get;
			private set;
		}

		public static int COLUMN_maxSpeed
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

		public static int COLUMN_audioPlacement
		{
			get;
			private set;
		}

		public static int COLUMN_audioMovement
		{
			get;
			private set;
		}

		public static int COLUMN_audioMovementAway
		{
			get;
			private set;
		}

		public static int COLUMN_audioImpact
		{
			get;
			private set;
		}

		public static int COLUMN_damage
		{
			get;
			private set;
		}

		public static int COLUMN_dps
		{
			get;
			private set;
		}

		public static int COLUMN_infoUIType
		{
			get;
			private set;
		}

		public static int COLUMN_requirements
		{
			get;
			private set;
		}

		public static int COLUMN_unlockedByEvent
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

		public static int COLUMN_lvl
		{
			get;
			private set;
		}

		public static int COLUMN_upgradeTime
		{
			get;
			private set;
		}

		public static int COLUMN_upgradeCredits
		{
			get;
			private set;
		}

		public static int COLUMN_upgradeMaterials
		{
			get;
			private set;
		}

		public static int COLUMN_upgradeContraband
		{
			get;
			private set;
		}

		public static int COLUMN_shotCount
		{
			get;
			private set;
		}

		public static int COLUMN_shotDelay
		{
			get;
			private set;
		}

		public static int COLUMN_impactDelay
		{
			get;
			private set;
		}

		public static int COLUMN_animationDelay
		{
			get;
			private set;
		}

		public static int COLUMN_destroyDelay
		{
			get;
			private set;
		}

		public static int COLUMN_projectileType
		{
			get;
			private set;
		}

		public static int COLUMN_reticleDuration
		{
			get;
			private set;
		}

		public static int COLUMN_reticleAsset
		{
			get;
			private set;
		}

		public static int COLUMN_reticleScale
		{
			get;
			private set;
		}

		public static int COLUMN_playerFacing
		{
			get;
			private set;
		}

		public static int COLUMN_hologramUid
		{
			get;
			private set;
		}

		public static int COLUMN_numberOfAttackers
		{
			get;
			private set;
		}

		public static int COLUMN_attackerDelay
		{
			get;
			private set;
		}

		public static int COLUMN_attackerOffset
		{
			get;
			private set;
		}

		public static int COLUMN_attackerOffsetVariance
		{
			get;
			private set;
		}

		public static int COLUMN_attackFormation
		{
			get;
			private set;
		}

		public static int COLUMN_angleOfAttack
		{
			get;
			private set;
		}

		public static int COLUMN_angleOfAttackVariance
		{
			get;
			private set;
		}

		public static int COLUMN_angleOfRoll
		{
			get;
			private set;
		}

		public static int COLUMN_angleOfRollVariance
		{
			get;
			private set;
		}

		public static int COLUMN_linkedUnit
		{
			get;
			private set;
		}

		public static int COLUMN_unitCount
		{
			get;
			private set;
		}

		public static int COLUMN_favoriteTargetType
		{
			get;
			private set;
		}

		public static int COLUMN_attachmentAsset
		{
			get;
			private set;
		}

		public static int COLUMN_attachmentAssetBundle
		{
			get;
			private set;
		}

		public static int COLUMN_unlockPlanet
		{
			get;
			private set;
		}

		public static int COLUMN_eventFeaturesString
		{
			get;
			private set;
		}

		public static int COLUMN_eventButtonAction
		{
			get;
			private set;
		}

		public static int COLUMN_eventButtonData
		{
			get;
			private set;
		}

		public static int COLUMN_eventButtonString
		{
			get;
			private set;
		}

		public static int COLUMN_upgradeShards
		{
			get;
			private set;
		}

		public static int COLUMN_upgradeShardUid
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

		public string SpecialAttackID
		{
			get;
			set;
		}

		public string SpecialAttackName
		{
			get;
			set;
		}

		public int TrainingTime
		{
			get;
			set;
		}

		public int Xp
		{
			get;
			set;
		}

		public int Credits
		{
			get;
			set;
		}

		public int Materials
		{
			get;
			set;
		}

		public int Contraband
		{
			get;
			set;
		}

		public int Acceleration
		{
			get;
			set;
		}

		public int MaxSpeed
		{
			get;
			set;
		}

		public int RotationSpeed
		{
			get;
			set;
		}

		public uint NumberOfAttackers
		{
			get;
			set;
		}

		public int AttackerDelay
		{
			get;
			set;
		}

		public int AttackerOffset
		{
			get;
			set;
		}

		public int AttackerOffsetVariance
		{
			get;
			set;
		}

		public AttackFormation AttackFormation
		{
			get;
			set;
		}

		public int AngleOfAttack
		{
			get;
			set;
		}

		public int AngleOfAttackVariance
		{
			get;
			set;
		}

		public int AngleOfRoll
		{
			get;
			set;
		}

		public int AngleOfRollVariance
		{
			get;
			set;
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

		public string LinkedUnit
		{
			get;
			set;
		}

		public uint UnitCount
		{
			get;
			set;
		}

		public bool IsDropship
		{
			get
			{
				return !string.IsNullOrEmpty(this.LinkedUnit) && this.UnitCount > 0u;
			}
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
				return this.SpecialAttackID;
			}
		}

		public List<StrIntPair> AudioPlacement
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

		public List<StrIntPair> AudioImpact
		{
			get;
			set;
		}

		public List<StrIntPair> AudioTrain
		{
			get;
			set;
		}

		public int Damage
		{
			get;
			set;
		}

		public int DPS
		{
			get;
			set;
		}

		public InfoUIType InfoUIType
		{
			get;
			private set;
		}

		public uint ShotCount
		{
			get;
			set;
		}

		public uint ShotDelay
		{
			get;
			set;
		}

		public uint HitDelay
		{
			get;
			set;
		}

		public uint AnimationDelay
		{
			get;
			set;
		}

		public float DestroyDelay
		{
			get;
			set;
		}

		public float ReticleDuration
		{
			get;
			set;
		}

		public string ReticleAsset
		{
			get;
			set;
		}

		public float ReticleScale
		{
			get;
			set;
		}

		public ProjectileTypeVO ProjectileType
		{
			get;
			set;
		}

		public bool PlayerFacing
		{
			get;
			private set;
		}

		public string HologramUid
		{
			get;
			private set;
		}

		public string FavoriteTargetType
		{
			get;
			private set;
		}

		public string DropoffAttachedAssetName
		{
			get;
			private set;
		}

		public string DropoffAttachedBundleName
		{
			get;
			private set;
		}

		public bool HasDropoff
		{
			get
			{
				return !string.IsNullOrEmpty(this.DropoffAttachedAssetName);
			}
		}

		public string EventFeaturesString
		{
			get;
			set;
		}

		public string EventButtonAction
		{
			get;
			set;
		}

		public string EventButtonData
		{
			get;
			set;
		}

		public string EventButtonString
		{
			get;
			set;
		}

		public int UpgradeShardCount
		{
			get;
			set;
		}

		public string UpgradeShardUid
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

		public void ReadRow(Row row)
		{
			this.AssetName = row.TryGetString(SpecialAttackTypeVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(SpecialAttackTypeVO.COLUMN_bundleName);
			this.IconAssetName = row.TryGetString(SpecialAttackTypeVO.COLUMN_iconAssetName, this.AssetName);
			this.IconBundleName = row.TryGetString(SpecialAttackTypeVO.COLUMN_iconBundleName, this.BundleName);
			this.DropoffAttachedAssetName = row.TryGetString(SpecialAttackTypeVO.COLUMN_attachmentAsset);
			this.DropoffAttachedBundleName = row.TryGetString(SpecialAttackTypeVO.COLUMN_attachmentAssetBundle);
			this.Uid = row.Uid;
			this.Faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(SpecialAttackTypeVO.COLUMN_faction));
			this.SpecialAttackID = row.TryGetString(SpecialAttackTypeVO.COLUMN_specialAttackID);
			this.SpecialAttackName = row.TryGetString(SpecialAttackTypeVO.COLUMN_specialAttackName);
			this.Size = row.TryGetInt(SpecialAttackTypeVO.COLUMN_size);
			this.TrainingTime = row.TryGetInt(SpecialAttackTypeVO.COLUMN_trainingTime);
			this.Xp = row.TryGetInt(SpecialAttackTypeVO.COLUMN_xp);
			this.Credits = row.TryGetInt(SpecialAttackTypeVO.COLUMN_credits);
			this.Materials = row.TryGetInt(SpecialAttackTypeVO.COLUMN_materials);
			this.Contraband = row.TryGetInt(SpecialAttackTypeVO.COLUMN_contraband);
			this.Acceleration = row.TryGetInt(SpecialAttackTypeVO.COLUMN_acceleration);
			this.MaxSpeed = row.TryGetInt(SpecialAttackTypeVO.COLUMN_maxSpeed);
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			this.AudioCharge = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(SpecialAttackTypeVO.COLUMN_audioCharge));
			this.AudioAttack = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(SpecialAttackTypeVO.COLUMN_audioAttack));
			this.AudioPlacement = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(SpecialAttackTypeVO.COLUMN_audioPlacement));
			this.AudioMovement = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(SpecialAttackTypeVO.COLUMN_audioMovement));
			this.AudioMovementAway = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(SpecialAttackTypeVO.COLUMN_audioMovementAway));
			this.AudioImpact = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(SpecialAttackTypeVO.COLUMN_audioImpact));
			this.Damage = row.TryGetInt(SpecialAttackTypeVO.COLUMN_damage);
			this.DPS = row.TryGetInt(SpecialAttackTypeVO.COLUMN_dps);
			this.InfoUIType = StringUtils.ParseEnum<InfoUIType>(row.TryGetString(SpecialAttackTypeVO.COLUMN_infoUIType));
			string[] array = row.TryGetStringArray(SpecialAttackTypeVO.COLUMN_requirements);
			this.BuildingRequirement = ((array == null || array.Length == 0) ? null : array[0]);
			this.UnlockedByEvent = row.TryGetBool(SpecialAttackTypeVO.COLUMN_unlockedByEvent);
			this.IconCameraPosition = row.TryGetVector3(SpecialAttackTypeVO.COLUMN_iconCameraPosition);
			this.IconLookatPosition = row.TryGetVector3(SpecialAttackTypeVO.COLUMN_iconLookatPosition);
			this.IconCloseupCameraPosition = row.TryGetVector3(SpecialAttackTypeVO.COLUMN_iconCloseupCameraPosition, this.IconCameraPosition);
			this.IconCloseupLookatPosition = row.TryGetVector3(SpecialAttackTypeVO.COLUMN_iconCloseupLookatPosition, this.IconLookatPosition);
			this.Order = row.TryGetInt(SpecialAttackTypeVO.COLUMN_order);
			this.Lvl = row.TryGetInt(SpecialAttackTypeVO.COLUMN_lvl);
			this.UpgradeTime = row.TryGetInt(SpecialAttackTypeVO.COLUMN_upgradeTime);
			this.UpgradeCredits = row.TryGetInt(SpecialAttackTypeVO.COLUMN_upgradeCredits);
			this.UpgradeMaterials = row.TryGetInt(SpecialAttackTypeVO.COLUMN_upgradeMaterials);
			this.UpgradeContraband = row.TryGetInt(SpecialAttackTypeVO.COLUMN_upgradeContraband);
			this.ShotCount = row.TryGetUint(SpecialAttackTypeVO.COLUMN_shotCount);
			this.ShotDelay = row.TryGetUint(SpecialAttackTypeVO.COLUMN_shotDelay);
			this.HitDelay = row.TryGetUint(SpecialAttackTypeVO.COLUMN_impactDelay);
			this.AnimationDelay = row.TryGetUint(SpecialAttackTypeVO.COLUMN_animationDelay);
			this.DestroyDelay = row.TryGetFloat(SpecialAttackTypeVO.COLUMN_destroyDelay);
			this.ProjectileType = Service.Get<IDataController>().Get<ProjectileTypeVO>(row.TryGetString(SpecialAttackTypeVO.COLUMN_projectileType));
			this.ReticleDuration = row.TryGetFloat(SpecialAttackTypeVO.COLUMN_reticleDuration, 3f);
			this.ReticleAsset = row.TryGetString(SpecialAttackTypeVO.COLUMN_reticleAsset);
			this.ReticleScale = row.TryGetFloat(SpecialAttackTypeVO.COLUMN_reticleScale, 2f);
			this.PlayerFacing = row.TryGetBool(SpecialAttackTypeVO.COLUMN_playerFacing);
			this.HologramUid = row.TryGetString(SpecialAttackTypeVO.COLUMN_hologramUid);
			this.NumberOfAttackers = Math.Max(row.TryGetUint(SpecialAttackTypeVO.COLUMN_numberOfAttackers), 1u);
			this.AttackerDelay = row.TryGetInt(SpecialAttackTypeVO.COLUMN_attackerDelay);
			this.AttackerOffset = row.TryGetInt(SpecialAttackTypeVO.COLUMN_attackerOffset);
			this.AttackerOffsetVariance = row.TryGetInt(SpecialAttackTypeVO.COLUMN_attackerOffsetVariance);
			this.AttackFormation = StringUtils.ParseEnum<AttackFormation>(row.TryGetString(SpecialAttackTypeVO.COLUMN_attackFormation));
			this.AngleOfAttack = row.TryGetInt(SpecialAttackTypeVO.COLUMN_angleOfAttack);
			this.AngleOfAttackVariance = row.TryGetInt(SpecialAttackTypeVO.COLUMN_angleOfAttackVariance);
			this.AngleOfRoll = row.TryGetInt(SpecialAttackTypeVO.COLUMN_angleOfRoll);
			this.AngleOfRollVariance = row.TryGetInt(SpecialAttackTypeVO.COLUMN_angleOfRollVariance);
			this.LinkedUnit = row.TryGetString(SpecialAttackTypeVO.COLUMN_linkedUnit);
			this.UnitCount = row.TryGetUint(SpecialAttackTypeVO.COLUMN_unitCount);
			this.FavoriteTargetType = row.TryGetString(SpecialAttackTypeVO.COLUMN_favoriteTargetType);
			this.EventFeaturesString = row.TryGetString(SpecialAttackTypeVO.COLUMN_eventFeaturesString);
			this.EventButtonAction = row.TryGetString(SpecialAttackTypeVO.COLUMN_eventButtonAction);
			this.EventButtonData = row.TryGetString(SpecialAttackTypeVO.COLUMN_eventButtonData);
			this.EventButtonString = row.TryGetString(SpecialAttackTypeVO.COLUMN_eventButtonString);
			this.UpgradeShardCount = row.TryGetInt(SpecialAttackTypeVO.COLUMN_upgradeShards);
			this.UpgradeShardUid = row.TryGetString(SpecialAttackTypeVO.COLUMN_upgradeShardUid);
			this.IconUnlockScale = row.TryGetVector3(SpecialAttackTypeVO.COLUMN_iconUnlockScale, Vector3.one);
			this.IconUnlockRotation = row.TryGetVector3(SpecialAttackTypeVO.COLUMN_iconUnlockRotation, Vector3.zero);
			this.IconUnlockPosition = row.TryGetVector3(SpecialAttackTypeVO.COLUMN_iconUnlockPosition, Vector3.zero);
		}

		public SpecialAttackTypeVO()
		{
		}

		protected internal SpecialAttackTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Acceleration);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AngleOfAttack);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AngleOfAttackVariance);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AngleOfRoll);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AngleOfRollVariance);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AttackerDelay);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AttackerOffset);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AttackerOffsetVariance);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AttackFormation);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioAttack);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioCharge);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioDeath);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioImpact);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovement);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovementAway);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioPlacement);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioTrain);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuildingRequirement);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_acceleration);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_angleOfAttack);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_angleOfAttackVariance);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_angleOfRoll);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_angleOfRollVariance);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_animationDelay);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_attachmentAsset);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_attachmentAssetBundle);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_attackerDelay);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_attackerOffset);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_attackerOffsetVariance);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_attackFormation);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_audioAttack);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_audioCharge);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_audioImpact);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_audioMovement);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_audioMovementAway);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_audioPlacement);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_contraband);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_credits);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_damage);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_destroyDelay);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_dps);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_eventButtonAction);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_eventButtonData);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_eventButtonString);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_eventFeaturesString);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_faction);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_favoriteTargetType);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_hologramUid);
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_iconAssetName);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_iconBundleName);
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_iconCameraPosition);
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_iconCloseupCameraPosition);
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_iconCloseupLookatPosition);
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_iconLookatPosition);
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_iconUnlockPosition);
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_iconUnlockRotation);
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_iconUnlockScale);
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_impactDelay);
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_infoUIType);
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_linkedUnit);
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_lvl);
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_materials);
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_maxSpeed);
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_numberOfAttackers);
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_order);
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_playerFacing);
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_projectileType);
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_requirements);
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_reticleAsset);
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_reticleDuration);
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_reticleScale);
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_shotCount);
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_shotDelay);
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_size);
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_specialAttackID);
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_specialAttackName);
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_trainingTime);
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_unitCount);
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_unlockedByEvent);
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_unlockPlanet);
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_upgradeContraband);
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_upgradeCredits);
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_upgradeMaterials);
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_upgradeShards);
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_upgradeShardUid);
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_upgradeTime);
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SpecialAttackTypeVO.COLUMN_xp);
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Contraband);
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Credits);
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Damage);
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).DestroyDelay);
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).DPS);
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).DropoffAttachedAssetName);
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).DropoffAttachedBundleName);
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonAction);
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonData);
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonString);
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventFeaturesString);
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).FavoriteTargetType);
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).HasDropoff);
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).HologramUid);
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName);
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName);
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition);
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition);
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition);
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition);
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed);
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockPosition);
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockRotation);
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockScale);
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).InfoUIType);
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsDropship);
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).LinkedUnit);
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Lvl);
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Materials);
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed);
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Order);
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).PlayerFacing);
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileType);
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReticleAsset);
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReticleDuration);
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReticleScale);
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).RotationSpeed);
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Size);
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackID);
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackName);
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).TrainingTime);
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UnlockedByEvent);
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeContraband);
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeCredits);
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeGroup);
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeMaterials);
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShardCount);
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShardUid);
		}

		public unsafe static long $Invoke141(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeTime);
		}

		public unsafe static long $Invoke142(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Xp);
		}

		public unsafe static long $Invoke143(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke144(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Acceleration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke145(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AngleOfAttack = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke146(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AngleOfAttackVariance = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke147(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AngleOfRoll = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke148(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AngleOfRollVariance = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke149(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke150(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AttackerDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke151(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AttackerOffset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke152(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AttackerOffsetVariance = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke153(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AttackFormation = (AttackFormation)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke154(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioAttack = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke155(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioCharge = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke156(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioDeath = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke157(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioImpact = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke158(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovement = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke159(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovementAway = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke160(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioPlacement = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke161(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioTrain = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke162(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuildingRequirement = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke163(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke164(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_acceleration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke165(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_angleOfAttack = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke166(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_angleOfAttackVariance = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke167(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_angleOfRoll = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke168(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_angleOfRollVariance = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke169(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_animationDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke170(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke171(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_attachmentAsset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke172(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_attachmentAssetBundle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke173(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_attackerDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke174(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_attackerOffset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke175(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_attackerOffsetVariance = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke176(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_attackFormation = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke177(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_audioAttack = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke178(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_audioCharge = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke179(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_audioImpact = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke180(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_audioMovement = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke181(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_audioMovementAway = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke182(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_audioPlacement = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke183(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke184(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_contraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke185(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_credits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke186(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_damage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke187(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_destroyDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke188(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_dps = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke189(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_eventButtonAction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke190(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_eventButtonData = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke191(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_eventButtonString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke192(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_eventFeaturesString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke193(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke194(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_favoriteTargetType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke195(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_hologramUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke196(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_iconAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke197(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_iconBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke198(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_iconCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke199(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_iconCloseupCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke200(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_iconCloseupLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke201(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_iconLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke202(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_iconUnlockPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke203(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_iconUnlockRotation = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke204(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_iconUnlockScale = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke205(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_impactDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke206(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_infoUIType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke207(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_linkedUnit = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke208(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_lvl = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke209(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_materials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke210(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_maxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke211(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_numberOfAttackers = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke212(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke213(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_playerFacing = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke214(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_projectileType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke215(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_requirements = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke216(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_reticleAsset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke217(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_reticleDuration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke218(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_reticleScale = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke219(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_shotCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke220(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_shotDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke221(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_size = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke222(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_specialAttackID = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke223(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_specialAttackName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke224(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_trainingTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke225(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_unitCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke226(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_unlockedByEvent = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke227(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_unlockPlanet = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke228(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_upgradeContraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke229(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_upgradeCredits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke230(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_upgradeMaterials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke231(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_upgradeShards = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke232(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_upgradeShardUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke233(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_upgradeTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke234(long instance, long* args)
		{
			SpecialAttackTypeVO.COLUMN_xp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke235(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Contraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke236(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Credits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke237(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Damage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke238(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).DestroyDelay = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke239(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).DPS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke240(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).DropoffAttachedAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke241(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).DropoffAttachedBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke242(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonAction = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke243(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonData = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke244(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke245(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventFeaturesString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke246(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke247(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).FavoriteTargetType = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke248(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).HologramUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke249(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke250(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke251(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke252(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke253(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke254(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke255(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke256(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke257(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockRotation = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke258(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockScale = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke259(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).InfoUIType = (InfoUIType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke260(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).LinkedUnit = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke261(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Lvl = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke262(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Materials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke263(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke264(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke265(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).PlayerFacing = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke266(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileType = (ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke267(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReticleAsset = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke268(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReticleDuration = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke269(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReticleScale = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke270(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).RotationSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke271(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Size = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke272(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke273(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke274(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).TrainingTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke275(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke276(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UnlockedByEvent = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke277(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeContraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke278(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeCredits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke279(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeMaterials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke280(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShardCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke281(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShardUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke282(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke283(long instance, long* args)
		{
			((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(instance)).Xp = *(int*)args;
			return -1L;
		}
	}
}
