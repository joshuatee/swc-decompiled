using StaRTS.Main.Controllers;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class TroopTypeVO : IHealthVO, IAudioVO, ITroopShooterVO, IShooterVO, ISpeedVO, ITroopDeployableVO, IDeployableVO, IUpgradeableVO, IValueObject, IAssetVO, IGeometryVO, IUnlockableVO
	{
		public const int MELEE_RANGE = 4;

		public int Xp;

		public static int COLUMN_assetName
		{
			get;
			private set;
		}

		public static int COLUMN_shieldAssetName
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

		public static int COLUMN_decalAssetName
		{
			get;
			private set;
		}

		public static int COLUMN_uiDecalAssetName
		{
			get;
			private set;
		}

		public static int COLUMN_decalBundleName
		{
			get;
			private set;
		}

		public static int COLUMN_decalSize
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

		public static int COLUMN_role
		{
			get;
			private set;
		}

		public static int COLUMN_unitID
		{
			get;
			private set;
		}

		public static int COLUMN_gunSequence
		{
			get;
			private set;
		}

		public static int COLUMN_type
		{
			get;
			private set;
		}

		public static int COLUMN_overWalls
		{
			get;
			private set;
		}

		public static int COLUMN_crushesWalls
		{
			get;
			private set;
		}

		public static int COLUMN_attackShieldBorder
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

		public static int COLUMN_health
		{
			get;
			private set;
		}

		public static int COLUMN_shieldHealth
		{
			get;
			private set;
		}

		public static int COLUMN_shieldCooldown
		{
			get;
			private set;
		}

		public static int COLUMN_maxSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_runSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_runThreshold
		{
			get;
			private set;
		}

		public static int COLUMN_newRotationSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_size
		{
			get;
			private set;
		}

		public static int COLUMN_isFlying
		{
			get;
			private set;
		}

		public static int COLUMN_targetLocking
		{
			get;
			private set;
		}

		public static int COLUMN_retargetingOffset
		{
			get;
			private set;
		}

		public static int COLUMN_supportFollowDistance
		{
			get;
			private set;
		}

		public static int COLUMN_clipRetargeting
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

		public static int COLUMN_ability
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

		public static int COLUMN_audioImpact
		{
			get;
			private set;
		}

		public static int COLUMN_audioTrain
		{
			get;
			private set;
		}

		public static int COLUMN_wall
		{
			get;
			private set;
		}

		public static int COLUMN_building
		{
			get;
			private set;
		}

		public static int COLUMN_storage
		{
			get;
			private set;
		}

		public static int COLUMN_resource
		{
			get;
			private set;
		}

		public static int COLUMN_turret
		{
			get;
			private set;
		}

		public static int COLUMN_HQ
		{
			get;
			private set;
		}

		public static int COLUMN_shield
		{
			get;
			private set;
		}

		public static int COLUMN_shieldGenerator
		{
			get;
			private set;
		}

		public static int COLUMN_infantry
		{
			get;
			private set;
		}

		public static int COLUMN_bruiserInfantry
		{
			get;
			private set;
		}

		public static int COLUMN_vehicle
		{
			get;
			private set;
		}

		public static int COLUMN_bruiserVehicle
		{
			get;
			private set;
		}

		public static int COLUMN_healerInfantry
		{
			get;
			private set;
		}

		public static int COLUMN_heroInfantry
		{
			get;
			private set;
		}

		public static int COLUMN_heroVehicle
		{
			get;
			private set;
		}

		public static int COLUMN_heroBruiserInfantry
		{
			get;
			private set;
		}

		public static int COLUMN_heroBruiserVechicle
		{
			get;
			private set;
		}

		public static int COLUMN_flierInfantry
		{
			get;
			private set;
		}

		public static int COLUMN_flierVehicle
		{
			get;
			private set;
		}

		public static int COLUMN_trap
		{
			get;
			private set;
		}

		public static int COLUMN_champion
		{
			get;
			private set;
		}

		public static int COLUMN_targetPreferenceStrength
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

		public static int COLUMN_favoriteTargetType
		{
			get;
			private set;
		}

		public static int COLUMN_viewRange
		{
			get;
			private set;
		}

		public static int COLUMN_minAttackRange
		{
			get;
			private set;
		}

		public static int COLUMN_maxAttackRange
		{
			get;
			private set;
		}

		public static int COLUMN_shotCount
		{
			get;
			private set;
		}

		public static int COLUMN_pathSearchWidth
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

		public static int COLUMN_chargeTime
		{
			get;
			private set;
		}

		public static int COLUMN_animationDelay
		{
			get;
			private set;
		}

		public static int COLUMN_shotDelay
		{
			get;
			private set;
		}

		public static int COLUMN_reload
		{
			get;
			private set;
		}

		public static int COLUMN_playerFacing
		{
			get;
			private set;
		}

		public static int COLUMN_targetSelf
		{
			get;
			private set;
		}

		public static int COLUMN_hologramUid
		{
			get;
			private set;
		}

		public static int COLUMN_factoryScaleFactor
		{
			get;
			private set;
		}

		public static int COLUMN_factoryRotation
		{
			get;
			private set;
		}

		public static int COLUMN_strictCoolDown
		{
			get;
			private set;
		}

		public static int COLUMN_autoSpawnSpreadingScale
		{
			get;
			private set;
		}

		public static int COLUMN_autoSpawnRateScale
		{
			get;
			private set;
		}

		public static int COLUMN_projectileType
		{
			get;
			private set;
		}

		public static int COLUMN_deathProjectile
		{
			get;
			private set;
		}

		public static int COLUMN_deathProjectileDelay
		{
			get;
			private set;
		}

		public static int COLUMN_deathProjectileDistance
		{
			get;
			private set;
		}

		public static int COLUMN_deathProjectileDamage
		{
			get;
			private set;
		}

		public static int COLUMN_deathAnimation
		{
			get;
			private set;
		}

		public static int COLUMN_spawnApplyBuffs
		{
			get;
			private set;
		}

		public static int COLUMN_spawnEffectUid
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

		public static int COLUMN_infoUIType
		{
			get;
			private set;
		}

		public static int COLUMN_unlockPlanet
		{
			get;
			private set;
		}

		public static int COLUMN_targetInRangeModifier
		{
			get;
			private set;
		}

		public static int COLUMN_preventDonation
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

		public string Uid
		{
			get;
			set;
		}

		public string AssetName
		{
			get;
			set;
		}

		public string ShieldAssetName
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

		public string DecalAssetName
		{
			get;
			set;
		}

		public string UIDecalAssetName
		{
			get;
			set;
		}

		public string DecalBundleName
		{
			get;
			set;
		}

		public float DecalSize
		{
			get;
			set;
		}

		public ArmorType ArmorType
		{
			get;
			private set;
		}

		public FactionType Faction
		{
			get;
			set;
		}

		public TroopRole TroopRole
		{
			get;
			set;
		}

		public string TroopID
		{
			get;
			private set;
		}

		public int[] GunSequence
		{
			get;
			set;
		}

		public TroopType Type
		{
			get;
			private set;
		}

		public InfoUIType InfoUIType
		{
			get;
			private set;
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

		public int Health
		{
			get;
			set;
		}

		public int ShieldHealth
		{
			get;
			set;
		}

		public uint ShieldCooldown
		{
			get;
			set;
		}

		public int MaxSpeed
		{
			get;
			set;
		}

		public int RunSpeed
		{
			get;
			set;
		}

		public int RotationSpeed
		{
			get;
			set;
		}

		public int TrainingTime
		{
			get;
			set;
		}

		public int RunThreshold
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

		public string Ability
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

		public int SizeX
		{
			get;
			private set;
		}

		public int SizeY
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
				return this.TroopID;
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

		public int[] Preference
		{
			get;
			set;
		}

		public int PreferencePercentile
		{
			get;
			set;
		}

		public int NearnessPercentile
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

		public string FavoriteTargetType
		{
			get;
			set;
		}

		public uint ViewRange
		{
			get;
			set;
		}

		public uint MinAttackRange
		{
			get;
			set;
		}

		public uint MaxAttackRange
		{
			get;
			set;
		}

		public bool AttackShieldBorder
		{
			get;
			set;
		}

		public bool OverWalls
		{
			get;
			set;
		}

		public bool CrushesWalls
		{
			get;
			set;
		}

		public uint PathSearchWidth
		{
			get;
			set;
		}

		public bool IsHealer
		{
			get;
			set;
		}

		public bool IsFlying
		{
			get;
			set;
		}

		public bool TargetLocking
		{
			get;
			set;
		}

		public bool TargetSelf
		{
			get;
			set;
		}

		public uint RetargetingOffset
		{
			get;
			set;
		}

		public uint SupportFollowDistance
		{
			get;
			private set;
		}

		public bool ClipRetargeting
		{
			get;
			set;
		}

		public uint WarmupDelay
		{
			get;
			set;
		}

		public uint AnimationDelay
		{
			get;
			set;
		}

		public uint ShotDelay
		{
			get;
			set;
		}

		public uint CooldownDelay
		{
			get;
			set;
		}

		public uint ShotCount
		{
			get;
			set;
		}

		public ProjectileTypeVO ProjectileType
		{
			get;
			set;
		}

		public ProjectileTypeVO DeathProjectileType
		{
			get;
			private set;
		}

		public uint DeathProjectileDelay
		{
			get;
			private set;
		}

		public int DeathProjectileDistance
		{
			get;
			private set;
		}

		public int DeathProjectileDamage
		{
			get;
			private set;
		}

		public List<KeyValuePair<string, int>> DeathAnimations
		{
			get;
			private set;
		}

		public string[] SpawnApplyBuffs
		{
			get;
			private set;
		}

		public TroopUniqueAbilityDescVO UniqueAbilityDescVO
		{
			get;
			set;
		}

		public uint AutoSpawnSpreadingScale
		{
			get;
			set;
		}

		public uint AutoSpawnRateScale
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

		public float FactoryScaleFactor
		{
			get;
			private set;
		}

		public float FactoryRotation
		{
			get;
			private set;
		}

		public string SpawnEffectUid
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

		public Dictionary<int, int> Sequences
		{
			get;
			private set;
		}

		public bool StrictCooldown
		{
			get;
			set;
		}

		public uint TargetInRangeModifier
		{
			get;
			set;
		}

		public bool PreventDonation
		{
			get;
			set;
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
			this.AssetName = row.TryGetString(TroopTypeVO.COLUMN_assetName);
			this.ShieldAssetName = row.TryGetString(TroopTypeVO.COLUMN_shieldAssetName);
			this.BundleName = row.TryGetString(TroopTypeVO.COLUMN_bundleName);
			this.IconAssetName = row.TryGetString(TroopTypeVO.COLUMN_iconAssetName, this.AssetName);
			this.IconBundleName = row.TryGetString(TroopTypeVO.COLUMN_iconBundleName, this.BundleName);
			this.DecalAssetName = row.TryGetString(TroopTypeVO.COLUMN_decalAssetName);
			this.UIDecalAssetName = row.TryGetString(TroopTypeVO.COLUMN_uiDecalAssetName);
			this.DecalBundleName = row.TryGetString(TroopTypeVO.COLUMN_decalBundleName);
			this.DecalSize = (float)row.TryGetInt(TroopTypeVO.COLUMN_decalSize) * 0.01f;
			this.ArmorType = StringUtils.ParseEnum<ArmorType>(row.TryGetString(TroopTypeVO.COLUMN_armorType));
			this.Uid = row.Uid;
			this.Faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(TroopTypeVO.COLUMN_faction));
			this.TroopRole = StringUtils.ParseEnum<TroopRole>(row.TryGetString(TroopTypeVO.COLUMN_role));
			this.TroopID = row.TryGetString(TroopTypeVO.COLUMN_unitID);
			this.Type = StringUtils.ParseEnum<TroopType>(row.TryGetString(TroopTypeVO.COLUMN_type));
			this.InfoUIType = StringUtils.ParseEnum<InfoUIType>(row.TryGetString(TroopTypeVO.COLUMN_infoUIType));
			this.OverWalls = row.TryGetBool(TroopTypeVO.COLUMN_overWalls);
			this.CrushesWalls = row.TryGetBool(TroopTypeVO.COLUMN_crushesWalls);
			this.AttackShieldBorder = row.TryGetBool(TroopTypeVO.COLUMN_attackShieldBorder);
			this.Credits = row.TryGetInt(TroopTypeVO.COLUMN_credits);
			this.Materials = row.TryGetInt(TroopTypeVO.COLUMN_materials);
			this.Contraband = row.TryGetInt(TroopTypeVO.COLUMN_contraband);
			this.Health = row.TryGetInt(TroopTypeVO.COLUMN_health);
			this.ShieldHealth = row.TryGetInt(TroopTypeVO.COLUMN_shieldHealth);
			this.ShieldCooldown = row.TryGetUint(TroopTypeVO.COLUMN_shieldCooldown);
			this.MaxSpeed = row.TryGetInt(TroopTypeVO.COLUMN_maxSpeed);
			this.RunSpeed = row.TryGetInt(TroopTypeVO.COLUMN_runSpeed);
			this.RunThreshold = row.TryGetInt(TroopTypeVO.COLUMN_runThreshold);
			this.RotationSpeed = row.TryGetInt(TroopTypeVO.COLUMN_newRotationSpeed);
			this.Size = row.TryGetInt(TroopTypeVO.COLUMN_size);
			this.IsFlying = row.TryGetBool(TroopTypeVO.COLUMN_isFlying);
			this.TargetLocking = row.TryGetBool(TroopTypeVO.COLUMN_targetLocking);
			this.RetargetingOffset = row.TryGetUint(TroopTypeVO.COLUMN_retargetingOffset);
			this.SupportFollowDistance = row.TryGetUint(TroopTypeVO.COLUMN_supportFollowDistance);
			this.ClipRetargeting = row.TryGetBool(TroopTypeVO.COLUMN_clipRetargeting);
			this.TargetInRangeModifier = row.TryGetUint(TroopTypeVO.COLUMN_targetInRangeModifier, 1u);
			this.PreventDonation = row.TryGetBool(TroopTypeVO.COLUMN_preventDonation);
			if (this.TargetInRangeModifier == 0u)
			{
				this.TargetInRangeModifier = 1u;
			}
			if ((this.Type == TroopType.Hero || this.Type == TroopType.Champion) && this.Size != 1)
			{
				Service.Get<StaRTSLogger>().Warn(this.Uid + " must have size 1.  Please fix CMS.");
				this.Size = 1;
			}
			this.TrainingTime = row.TryGetInt(TroopTypeVO.COLUMN_trainingTime);
			this.Xp = row.TryGetInt(TroopTypeVO.COLUMN_xp);
			string[] array = row.TryGetStringArray(TroopTypeVO.COLUMN_requirements);
			this.BuildingRequirement = ((array == null || array.Length == 0) ? null : array[0]);
			this.UnlockedByEvent = row.TryGetBool(TroopTypeVO.COLUMN_unlockedByEvent);
			this.Ability = row.TryGetString(TroopTypeVO.COLUMN_ability);
			this.IconCameraPosition = row.TryGetVector3(TroopTypeVO.COLUMN_iconCameraPosition);
			this.IconLookatPosition = row.TryGetVector3(TroopTypeVO.COLUMN_iconLookatPosition);
			this.IconCloseupCameraPosition = row.TryGetVector3(TroopTypeVO.COLUMN_iconCloseupCameraPosition, this.IconCameraPosition);
			this.IconCloseupLookatPosition = row.TryGetVector3(TroopTypeVO.COLUMN_iconCloseupLookatPosition, this.IconLookatPosition);
			this.Order = row.TryGetInt(TroopTypeVO.COLUMN_order);
			this.SizeX = row.TryGetInt(TroopTypeVO.COLUMN_sizex);
			this.SizeY = row.TryGetInt(TroopTypeVO.COLUMN_sizey);
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			this.AudioCharge = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(TroopTypeVO.COLUMN_audioCharge));
			this.AudioAttack = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(TroopTypeVO.COLUMN_audioAttack));
			this.AudioDeath = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(TroopTypeVO.COLUMN_audioDeath));
			this.AudioPlacement = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(TroopTypeVO.COLUMN_audioPlacement));
			this.AudioImpact = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(TroopTypeVO.COLUMN_audioImpact));
			this.AudioTrain = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(TroopTypeVO.COLUMN_audioTrain));
			this.Preference = new int[23];
			int i = 0;
			int num = 23;
			while (i < num)
			{
				this.Preference[i] = 0;
				i++;
			}
			this.Preference[1] = row.TryGetInt(TroopTypeVO.COLUMN_wall);
			this.Preference[2] = row.TryGetInt(TroopTypeVO.COLUMN_building);
			this.Preference[3] = row.TryGetInt(TroopTypeVO.COLUMN_storage);
			this.Preference[4] = row.TryGetInt(TroopTypeVO.COLUMN_resource);
			this.Preference[5] = row.TryGetInt(TroopTypeVO.COLUMN_turret);
			this.Preference[6] = row.TryGetInt(TroopTypeVO.COLUMN_HQ);
			this.Preference[7] = row.TryGetInt(TroopTypeVO.COLUMN_shield);
			this.Preference[8] = row.TryGetInt(TroopTypeVO.COLUMN_shieldGenerator);
			this.Preference[9] = row.TryGetInt(TroopTypeVO.COLUMN_infantry);
			this.Preference[10] = row.TryGetInt(TroopTypeVO.COLUMN_bruiserInfantry);
			this.Preference[11] = row.TryGetInt(TroopTypeVO.COLUMN_vehicle);
			this.Preference[12] = row.TryGetInt(TroopTypeVO.COLUMN_bruiserVehicle);
			this.Preference[13] = row.TryGetInt(TroopTypeVO.COLUMN_heroInfantry);
			this.Preference[14] = row.TryGetInt(TroopTypeVO.COLUMN_heroVehicle);
			this.Preference[15] = row.TryGetInt(TroopTypeVO.COLUMN_heroBruiserInfantry);
			this.Preference[16] = row.TryGetInt(TroopTypeVO.COLUMN_heroBruiserVechicle);
			this.Preference[17] = row.TryGetInt(TroopTypeVO.COLUMN_flierInfantry);
			this.Preference[18] = row.TryGetInt(TroopTypeVO.COLUMN_flierVehicle);
			this.Preference[19] = row.TryGetInt(TroopTypeVO.COLUMN_healerInfantry);
			this.Preference[20] = row.TryGetInt(TroopTypeVO.COLUMN_trap);
			this.Preference[21] = row.TryGetInt(TroopTypeVO.COLUMN_champion);
			this.PreferencePercentile = row.TryGetInt(TroopTypeVO.COLUMN_targetPreferenceStrength);
			this.NearnessPercentile = 100 - this.PreferencePercentile;
			this.Damage = row.TryGetInt(TroopTypeVO.COLUMN_damage);
			this.DPS = row.TryGetInt(TroopTypeVO.COLUMN_dps);
			this.FavoriteTargetType = row.TryGetString(TroopTypeVO.COLUMN_favoriteTargetType);
			this.ViewRange = row.TryGetUint(TroopTypeVO.COLUMN_viewRange);
			this.MinAttackRange = row.TryGetUint(TroopTypeVO.COLUMN_minAttackRange);
			this.MaxAttackRange = row.TryGetUint(TroopTypeVO.COLUMN_maxAttackRange);
			this.ShotCount = row.TryGetUint(TroopTypeVO.COLUMN_shotCount);
			this.PathSearchWidth = row.TryGetUint(TroopTypeVO.COLUMN_pathSearchWidth);
			this.Lvl = row.TryGetInt(TroopTypeVO.COLUMN_lvl);
			this.UpgradeTime = row.TryGetInt(TroopTypeVO.COLUMN_upgradeTime);
			this.UpgradeCredits = row.TryGetInt(TroopTypeVO.COLUMN_upgradeCredits);
			this.UpgradeMaterials = row.TryGetInt(TroopTypeVO.COLUMN_upgradeMaterials);
			this.UpgradeContraband = row.TryGetInt(TroopTypeVO.COLUMN_upgradeContraband);
			this.IsHealer = (this.TroopRole == TroopRole.Healer);
			this.WarmupDelay = row.TryGetUint(TroopTypeVO.COLUMN_chargeTime);
			this.AnimationDelay = row.TryGetUint(TroopTypeVO.COLUMN_animationDelay);
			this.ShotDelay = row.TryGetUint(TroopTypeVO.COLUMN_shotDelay);
			this.CooldownDelay = row.TryGetUint(TroopTypeVO.COLUMN_reload);
			this.PlayerFacing = row.TryGetBool(TroopTypeVO.COLUMN_playerFacing);
			this.TargetSelf = row.TryGetBool(TroopTypeVO.COLUMN_targetSelf);
			this.HologramUid = row.TryGetString(TroopTypeVO.COLUMN_hologramUid);
			this.FactoryScaleFactor = row.TryGetFloat(TroopTypeVO.COLUMN_factoryScaleFactor);
			this.FactoryRotation = row.TryGetFloat(TroopTypeVO.COLUMN_factoryRotation);
			this.StrictCooldown = row.TryGetBool(TroopTypeVO.COLUMN_strictCoolDown);
			this.AutoSpawnSpreadingScale = row.TryGetUint(TroopTypeVO.COLUMN_autoSpawnSpreadingScale, 1u);
			this.AutoSpawnRateScale = row.TryGetUint(TroopTypeVO.COLUMN_autoSpawnRateScale, 1u);
			IDataController dataController = Service.Get<IDataController>();
			this.ProjectileType = dataController.Get<ProjectileTypeVO>(row.TryGetString(TroopTypeVO.COLUMN_projectileType));
			if (this.ProjectileType.IsBeam && (long)this.ProjectileType.BeamDamageLength < (long)((ulong)this.MaxAttackRange))
			{
				Service.Get<StaRTSLogger>().WarnFormat("Troop {0} can target something it can't damage", new object[]
				{
					this.Uid
				});
			}
			string text = row.TryGetString(TroopTypeVO.COLUMN_deathProjectile);
			this.DeathProjectileType = (string.IsNullOrEmpty(text) ? null : dataController.Get<ProjectileTypeVO>(text));
			this.DeathProjectileDelay = row.TryGetUint(TroopTypeVO.COLUMN_deathProjectileDelay, 0u);
			this.DeathProjectileDistance = row.TryGetInt(TroopTypeVO.COLUMN_deathProjectileDistance, 0);
			this.DeathProjectileDamage = row.TryGetInt(TroopTypeVO.COLUMN_deathProjectileDamage, this.Damage);
			string text2 = row.TryGetString(TroopTypeVO.COLUMN_deathAnimation);
			if (!string.IsNullOrEmpty(text2))
			{
				string[] array2 = text2.Split(new char[]
				{
					','
				});
				int num2 = array2.Length;
				this.DeathAnimations = new List<KeyValuePair<string, int>>(num2);
				for (int j = 0; j < num2; j++)
				{
					string[] array3 = array2[j].Split(new char[]
					{
						':'
					});
					int num3;
					if (array3.Length == 2 && int.TryParse(array3[1], ref num3))
					{
						string text3 = array3[0];
						this.DeathAnimations.Add(new KeyValuePair<string, int>(text3, num3));
					}
				}
			}
			this.SpawnApplyBuffs = null;
			string text4 = row.TryGetString(TroopTypeVO.COLUMN_spawnApplyBuffs);
			if (!string.IsNullOrEmpty(text4))
			{
				this.SpawnApplyBuffs = text4.Split(new char[]
				{
					','
				});
			}
			this.SpawnEffectUid = row.TryGetString(TroopTypeVO.COLUMN_spawnEffectUid);
			this.TooltipHeightOffset = row.TryGetFloat(TroopTypeVO.COLUMN_tooltipHeightOffset);
			this.BuffAssetOffset = row.TryGetVector3(TroopTypeVO.COLUMN_buffAssetOffset);
			this.BuffAssetBaseOffset = row.TryGetVector3(TroopTypeVO.COLUMN_buffAssetBaseOffset, Vector3.zero);
			SequencePair gunSequences = valueObjectController.GetGunSequences(this.Uid, row.TryGetString(TroopTypeVO.COLUMN_gunSequence));
			this.GunSequence = gunSequences.GunSequence;
			this.Sequences = gunSequences.Sequences;
			this.EventFeaturesString = row.TryGetString(TroopTypeVO.COLUMN_eventFeaturesString);
			this.EventButtonAction = row.TryGetString(TroopTypeVO.COLUMN_eventButtonAction);
			this.EventButtonData = row.TryGetString(TroopTypeVO.COLUMN_eventButtonData);
			this.EventButtonString = row.TryGetString(TroopTypeVO.COLUMN_eventButtonString);
			this.UpgradeShardCount = row.TryGetInt(TroopTypeVO.COLUMN_upgradeShards);
			this.UpgradeShardUid = row.TryGetString(TroopTypeVO.COLUMN_upgradeShardUid);
			this.IconUnlockScale = row.TryGetVector3(TroopTypeVO.COLUMN_iconUnlockScale, Vector3.one);
			this.IconUnlockRotation = row.TryGetVector3(TroopTypeVO.COLUMN_iconUnlockRotation, Vector3.zero);
			this.IconUnlockPosition = row.TryGetVector3(TroopTypeVO.COLUMN_iconUnlockPosition, Vector3.zero);
			if (this.RotationSpeed == 0)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Missing rotation speed for troopTypeVO {0}", new object[]
				{
					this.Uid
				});
			}
		}

		public TroopTypeVO()
		{
		}

		protected internal TroopTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Ability);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).ArmorType);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AttackShieldBorder);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioAttack);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioCharge);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioDeath);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioImpact);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovement);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovementAway);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioPlacement);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioTrain);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuffAssetBaseOffset);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuffAssetOffset);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuildingRequirement);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).ClipRetargeting);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_ability);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_animationDelay);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_armorType);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_attackShieldBorder);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_audioAttack);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_audioCharge);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_audioDeath);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_audioImpact);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_audioPlacement);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_audioTrain);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_autoSpawnRateScale);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_autoSpawnSpreadingScale);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_bruiserInfantry);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_bruiserVehicle);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_buffAssetBaseOffset);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_buffAssetOffset);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_building);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_champion);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_chargeTime);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_clipRetargeting);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_contraband);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_credits);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_crushesWalls);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_damage);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_deathAnimation);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_deathProjectile);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_deathProjectileDamage);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_deathProjectileDelay);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_deathProjectileDistance);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_decalAssetName);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_decalBundleName);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_decalSize);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_dps);
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_eventButtonAction);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_eventButtonData);
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_eventButtonString);
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_eventFeaturesString);
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_faction);
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_factoryRotation);
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_factoryScaleFactor);
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_favoriteTargetType);
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_flierInfantry);
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_flierVehicle);
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_gunSequence);
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_healerInfantry);
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_health);
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_heroBruiserInfantry);
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_heroBruiserVechicle);
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_heroInfantry);
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_heroVehicle);
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_hologramUid);
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_HQ);
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_iconAssetName);
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_iconBundleName);
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_iconCameraPosition);
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_iconCloseupCameraPosition);
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_iconCloseupLookatPosition);
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_iconLookatPosition);
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_iconUnlockPosition);
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_iconUnlockRotation);
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_iconUnlockScale);
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_infantry);
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_infoUIType);
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_isFlying);
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_lvl);
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_materials);
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_maxAttackRange);
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_maxSpeed);
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_minAttackRange);
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_newRotationSpeed);
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_order);
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_overWalls);
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_pathSearchWidth);
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_playerFacing);
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_preventDonation);
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_projectileType);
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_reload);
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_requirements);
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_resource);
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_retargetingOffset);
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_role);
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_runSpeed);
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_runThreshold);
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_shield);
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_shieldAssetName);
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_shieldCooldown);
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_shieldGenerator);
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_shieldHealth);
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_shotCount);
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_shotDelay);
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_size);
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_sizex);
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_sizey);
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_spawnApplyBuffs);
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_spawnEffectUid);
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_storage);
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_strictCoolDown);
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_supportFollowDistance);
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_targetInRangeModifier);
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_targetLocking);
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_targetPreferenceStrength);
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_targetSelf);
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_tooltipHeightOffset);
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_trainingTime);
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_trap);
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_turret);
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_type);
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_uiDecalAssetName);
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_unitID);
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_unlockedByEvent);
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_unlockPlanet);
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_upgradeContraband);
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_upgradeCredits);
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_upgradeMaterials);
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_upgradeShards);
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_upgradeShardUid);
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_upgradeTime);
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_vehicle);
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_viewRange);
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_wall);
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopTypeVO.COLUMN_xp);
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Contraband);
		}

		public unsafe static long $Invoke141(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Credits);
		}

		public unsafe static long $Invoke142(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).CrushesWalls);
		}

		public unsafe static long $Invoke143(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Damage);
		}

		public unsafe static long $Invoke144(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DeathAnimations);
		}

		public unsafe static long $Invoke145(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DeathProjectileDamage);
		}

		public unsafe static long $Invoke146(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DeathProjectileDistance);
		}

		public unsafe static long $Invoke147(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DeathProjectileType);
		}

		public unsafe static long $Invoke148(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DecalAssetName);
		}

		public unsafe static long $Invoke149(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DecalBundleName);
		}

		public unsafe static long $Invoke150(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DecalSize);
		}

		public unsafe static long $Invoke151(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DPS);
		}

		public unsafe static long $Invoke152(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonAction);
		}

		public unsafe static long $Invoke153(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonData);
		}

		public unsafe static long $Invoke154(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonString);
		}

		public unsafe static long $Invoke155(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventFeaturesString);
		}

		public unsafe static long $Invoke156(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke157(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).FactoryRotation);
		}

		public unsafe static long $Invoke158(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).FactoryScaleFactor);
		}

		public unsafe static long $Invoke159(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).FavoriteTargetType);
		}

		public unsafe static long $Invoke160(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).GunSequence);
		}

		public unsafe static long $Invoke161(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Health);
		}

		public unsafe static long $Invoke162(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).HologramUid);
		}

		public unsafe static long $Invoke163(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName);
		}

		public unsafe static long $Invoke164(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName);
		}

		public unsafe static long $Invoke165(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition);
		}

		public unsafe static long $Invoke166(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition);
		}

		public unsafe static long $Invoke167(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition);
		}

		public unsafe static long $Invoke168(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition);
		}

		public unsafe static long $Invoke169(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed);
		}

		public unsafe static long $Invoke170(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockPosition);
		}

		public unsafe static long $Invoke171(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockRotation);
		}

		public unsafe static long $Invoke172(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockScale);
		}

		public unsafe static long $Invoke173(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).InfoUIType);
		}

		public unsafe static long $Invoke174(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsFlying);
		}

		public unsafe static long $Invoke175(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsHealer);
		}

		public unsafe static long $Invoke176(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Lvl);
		}

		public unsafe static long $Invoke177(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Materials);
		}

		public unsafe static long $Invoke178(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed);
		}

		public unsafe static long $Invoke179(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).NearnessPercentile);
		}

		public unsafe static long $Invoke180(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Order);
		}

		public unsafe static long $Invoke181(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).OverWalls);
		}

		public unsafe static long $Invoke182(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).PlayerFacing);
		}

		public unsafe static long $Invoke183(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Preference);
		}

		public unsafe static long $Invoke184(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).PreferencePercentile);
		}

		public unsafe static long $Invoke185(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).PreventDonation);
		}

		public unsafe static long $Invoke186(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileType);
		}

		public unsafe static long $Invoke187(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).RotationSpeed);
		}

		public unsafe static long $Invoke188(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).RunSpeed);
		}

		public unsafe static long $Invoke189(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).RunThreshold);
		}

		public unsafe static long $Invoke190(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Sequences);
		}

		public unsafe static long $Invoke191(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).ShieldAssetName);
		}

		public unsafe static long $Invoke192(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).ShieldHealth);
		}

		public unsafe static long $Invoke193(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Size);
		}

		public unsafe static long $Invoke194(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).SizeX);
		}

		public unsafe static long $Invoke195(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).SizeY);
		}

		public unsafe static long $Invoke196(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).SpawnApplyBuffs);
		}

		public unsafe static long $Invoke197(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).SpawnEffectUid);
		}

		public unsafe static long $Invoke198(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).StrictCooldown);
		}

		public unsafe static long $Invoke199(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).TargetLocking);
		}

		public unsafe static long $Invoke200(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).TargetSelf);
		}

		public unsafe static long $Invoke201(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).TooltipHeightOffset);
		}

		public unsafe static long $Invoke202(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).TrainingTime);
		}

		public unsafe static long $Invoke203(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).TroopID);
		}

		public unsafe static long $Invoke204(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).TroopRole);
		}

		public unsafe static long $Invoke205(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke206(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke207(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UIDecalAssetName);
		}

		public unsafe static long $Invoke208(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UniqueAbilityDescVO);
		}

		public unsafe static long $Invoke209(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UnlockedByEvent);
		}

		public unsafe static long $Invoke210(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeContraband);
		}

		public unsafe static long $Invoke211(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeCredits);
		}

		public unsafe static long $Invoke212(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeGroup);
		}

		public unsafe static long $Invoke213(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeMaterials);
		}

		public unsafe static long $Invoke214(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShardCount);
		}

		public unsafe static long $Invoke215(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShardUid);
		}

		public unsafe static long $Invoke216(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeTime);
		}

		public unsafe static long $Invoke217(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke218(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Ability = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke219(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).ArmorType = (ArmorType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke220(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke221(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AttackShieldBorder = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke222(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioAttack = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke223(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioCharge = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke224(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioDeath = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke225(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioImpact = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke226(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovement = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke227(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovementAway = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke228(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioPlacement = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke229(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioTrain = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke230(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuffAssetBaseOffset = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke231(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuffAssetOffset = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke232(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuildingRequirement = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke233(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke234(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).ClipRetargeting = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke235(long instance, long* args)
		{
			TroopTypeVO.COLUMN_ability = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke236(long instance, long* args)
		{
			TroopTypeVO.COLUMN_animationDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke237(long instance, long* args)
		{
			TroopTypeVO.COLUMN_armorType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke238(long instance, long* args)
		{
			TroopTypeVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke239(long instance, long* args)
		{
			TroopTypeVO.COLUMN_attackShieldBorder = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke240(long instance, long* args)
		{
			TroopTypeVO.COLUMN_audioAttack = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke241(long instance, long* args)
		{
			TroopTypeVO.COLUMN_audioCharge = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke242(long instance, long* args)
		{
			TroopTypeVO.COLUMN_audioDeath = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke243(long instance, long* args)
		{
			TroopTypeVO.COLUMN_audioImpact = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke244(long instance, long* args)
		{
			TroopTypeVO.COLUMN_audioPlacement = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke245(long instance, long* args)
		{
			TroopTypeVO.COLUMN_audioTrain = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke246(long instance, long* args)
		{
			TroopTypeVO.COLUMN_autoSpawnRateScale = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke247(long instance, long* args)
		{
			TroopTypeVO.COLUMN_autoSpawnSpreadingScale = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke248(long instance, long* args)
		{
			TroopTypeVO.COLUMN_bruiserInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke249(long instance, long* args)
		{
			TroopTypeVO.COLUMN_bruiserVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke250(long instance, long* args)
		{
			TroopTypeVO.COLUMN_buffAssetBaseOffset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke251(long instance, long* args)
		{
			TroopTypeVO.COLUMN_buffAssetOffset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke252(long instance, long* args)
		{
			TroopTypeVO.COLUMN_building = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke253(long instance, long* args)
		{
			TroopTypeVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke254(long instance, long* args)
		{
			TroopTypeVO.COLUMN_champion = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke255(long instance, long* args)
		{
			TroopTypeVO.COLUMN_chargeTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke256(long instance, long* args)
		{
			TroopTypeVO.COLUMN_clipRetargeting = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke257(long instance, long* args)
		{
			TroopTypeVO.COLUMN_contraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke258(long instance, long* args)
		{
			TroopTypeVO.COLUMN_credits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke259(long instance, long* args)
		{
			TroopTypeVO.COLUMN_crushesWalls = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke260(long instance, long* args)
		{
			TroopTypeVO.COLUMN_damage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke261(long instance, long* args)
		{
			TroopTypeVO.COLUMN_deathAnimation = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke262(long instance, long* args)
		{
			TroopTypeVO.COLUMN_deathProjectile = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke263(long instance, long* args)
		{
			TroopTypeVO.COLUMN_deathProjectileDamage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke264(long instance, long* args)
		{
			TroopTypeVO.COLUMN_deathProjectileDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke265(long instance, long* args)
		{
			TroopTypeVO.COLUMN_deathProjectileDistance = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke266(long instance, long* args)
		{
			TroopTypeVO.COLUMN_decalAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke267(long instance, long* args)
		{
			TroopTypeVO.COLUMN_decalBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke268(long instance, long* args)
		{
			TroopTypeVO.COLUMN_decalSize = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke269(long instance, long* args)
		{
			TroopTypeVO.COLUMN_dps = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke270(long instance, long* args)
		{
			TroopTypeVO.COLUMN_eventButtonAction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke271(long instance, long* args)
		{
			TroopTypeVO.COLUMN_eventButtonData = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke272(long instance, long* args)
		{
			TroopTypeVO.COLUMN_eventButtonString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke273(long instance, long* args)
		{
			TroopTypeVO.COLUMN_eventFeaturesString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke274(long instance, long* args)
		{
			TroopTypeVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke275(long instance, long* args)
		{
			TroopTypeVO.COLUMN_factoryRotation = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke276(long instance, long* args)
		{
			TroopTypeVO.COLUMN_factoryScaleFactor = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke277(long instance, long* args)
		{
			TroopTypeVO.COLUMN_favoriteTargetType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke278(long instance, long* args)
		{
			TroopTypeVO.COLUMN_flierInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke279(long instance, long* args)
		{
			TroopTypeVO.COLUMN_flierVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke280(long instance, long* args)
		{
			TroopTypeVO.COLUMN_gunSequence = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke281(long instance, long* args)
		{
			TroopTypeVO.COLUMN_healerInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke282(long instance, long* args)
		{
			TroopTypeVO.COLUMN_health = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke283(long instance, long* args)
		{
			TroopTypeVO.COLUMN_heroBruiserInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke284(long instance, long* args)
		{
			TroopTypeVO.COLUMN_heroBruiserVechicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke285(long instance, long* args)
		{
			TroopTypeVO.COLUMN_heroInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke286(long instance, long* args)
		{
			TroopTypeVO.COLUMN_heroVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke287(long instance, long* args)
		{
			TroopTypeVO.COLUMN_hologramUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke288(long instance, long* args)
		{
			TroopTypeVO.COLUMN_HQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke289(long instance, long* args)
		{
			TroopTypeVO.COLUMN_iconAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke290(long instance, long* args)
		{
			TroopTypeVO.COLUMN_iconBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke291(long instance, long* args)
		{
			TroopTypeVO.COLUMN_iconCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke292(long instance, long* args)
		{
			TroopTypeVO.COLUMN_iconCloseupCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke293(long instance, long* args)
		{
			TroopTypeVO.COLUMN_iconCloseupLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke294(long instance, long* args)
		{
			TroopTypeVO.COLUMN_iconLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke295(long instance, long* args)
		{
			TroopTypeVO.COLUMN_iconUnlockPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke296(long instance, long* args)
		{
			TroopTypeVO.COLUMN_iconUnlockRotation = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke297(long instance, long* args)
		{
			TroopTypeVO.COLUMN_iconUnlockScale = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke298(long instance, long* args)
		{
			TroopTypeVO.COLUMN_infantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke299(long instance, long* args)
		{
			TroopTypeVO.COLUMN_infoUIType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke300(long instance, long* args)
		{
			TroopTypeVO.COLUMN_isFlying = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke301(long instance, long* args)
		{
			TroopTypeVO.COLUMN_lvl = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke302(long instance, long* args)
		{
			TroopTypeVO.COLUMN_materials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke303(long instance, long* args)
		{
			TroopTypeVO.COLUMN_maxAttackRange = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke304(long instance, long* args)
		{
			TroopTypeVO.COLUMN_maxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke305(long instance, long* args)
		{
			TroopTypeVO.COLUMN_minAttackRange = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke306(long instance, long* args)
		{
			TroopTypeVO.COLUMN_newRotationSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke307(long instance, long* args)
		{
			TroopTypeVO.COLUMN_order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke308(long instance, long* args)
		{
			TroopTypeVO.COLUMN_overWalls = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke309(long instance, long* args)
		{
			TroopTypeVO.COLUMN_pathSearchWidth = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke310(long instance, long* args)
		{
			TroopTypeVO.COLUMN_playerFacing = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke311(long instance, long* args)
		{
			TroopTypeVO.COLUMN_preventDonation = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke312(long instance, long* args)
		{
			TroopTypeVO.COLUMN_projectileType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke313(long instance, long* args)
		{
			TroopTypeVO.COLUMN_reload = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke314(long instance, long* args)
		{
			TroopTypeVO.COLUMN_requirements = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke315(long instance, long* args)
		{
			TroopTypeVO.COLUMN_resource = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke316(long instance, long* args)
		{
			TroopTypeVO.COLUMN_retargetingOffset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke317(long instance, long* args)
		{
			TroopTypeVO.COLUMN_role = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke318(long instance, long* args)
		{
			TroopTypeVO.COLUMN_runSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke319(long instance, long* args)
		{
			TroopTypeVO.COLUMN_runThreshold = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke320(long instance, long* args)
		{
			TroopTypeVO.COLUMN_shield = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke321(long instance, long* args)
		{
			TroopTypeVO.COLUMN_shieldAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke322(long instance, long* args)
		{
			TroopTypeVO.COLUMN_shieldCooldown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke323(long instance, long* args)
		{
			TroopTypeVO.COLUMN_shieldGenerator = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke324(long instance, long* args)
		{
			TroopTypeVO.COLUMN_shieldHealth = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke325(long instance, long* args)
		{
			TroopTypeVO.COLUMN_shotCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke326(long instance, long* args)
		{
			TroopTypeVO.COLUMN_shotDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke327(long instance, long* args)
		{
			TroopTypeVO.COLUMN_size = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke328(long instance, long* args)
		{
			TroopTypeVO.COLUMN_sizex = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke329(long instance, long* args)
		{
			TroopTypeVO.COLUMN_sizey = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke330(long instance, long* args)
		{
			TroopTypeVO.COLUMN_spawnApplyBuffs = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke331(long instance, long* args)
		{
			TroopTypeVO.COLUMN_spawnEffectUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke332(long instance, long* args)
		{
			TroopTypeVO.COLUMN_storage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke333(long instance, long* args)
		{
			TroopTypeVO.COLUMN_strictCoolDown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke334(long instance, long* args)
		{
			TroopTypeVO.COLUMN_supportFollowDistance = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke335(long instance, long* args)
		{
			TroopTypeVO.COLUMN_targetInRangeModifier = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke336(long instance, long* args)
		{
			TroopTypeVO.COLUMN_targetLocking = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke337(long instance, long* args)
		{
			TroopTypeVO.COLUMN_targetPreferenceStrength = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke338(long instance, long* args)
		{
			TroopTypeVO.COLUMN_targetSelf = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke339(long instance, long* args)
		{
			TroopTypeVO.COLUMN_tooltipHeightOffset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke340(long instance, long* args)
		{
			TroopTypeVO.COLUMN_trainingTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke341(long instance, long* args)
		{
			TroopTypeVO.COLUMN_trap = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke342(long instance, long* args)
		{
			TroopTypeVO.COLUMN_turret = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke343(long instance, long* args)
		{
			TroopTypeVO.COLUMN_type = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke344(long instance, long* args)
		{
			TroopTypeVO.COLUMN_uiDecalAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke345(long instance, long* args)
		{
			TroopTypeVO.COLUMN_unitID = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke346(long instance, long* args)
		{
			TroopTypeVO.COLUMN_unlockedByEvent = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke347(long instance, long* args)
		{
			TroopTypeVO.COLUMN_unlockPlanet = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke348(long instance, long* args)
		{
			TroopTypeVO.COLUMN_upgradeContraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke349(long instance, long* args)
		{
			TroopTypeVO.COLUMN_upgradeCredits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke350(long instance, long* args)
		{
			TroopTypeVO.COLUMN_upgradeMaterials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke351(long instance, long* args)
		{
			TroopTypeVO.COLUMN_upgradeShards = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke352(long instance, long* args)
		{
			TroopTypeVO.COLUMN_upgradeShardUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke353(long instance, long* args)
		{
			TroopTypeVO.COLUMN_upgradeTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke354(long instance, long* args)
		{
			TroopTypeVO.COLUMN_vehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke355(long instance, long* args)
		{
			TroopTypeVO.COLUMN_viewRange = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke356(long instance, long* args)
		{
			TroopTypeVO.COLUMN_wall = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke357(long instance, long* args)
		{
			TroopTypeVO.COLUMN_xp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke358(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Contraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke359(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Credits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke360(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).CrushesWalls = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke361(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Damage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke362(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DeathAnimations = (List<KeyValuePair<string, int>>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke363(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DeathProjectileDamage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke364(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DeathProjectileDistance = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke365(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DeathProjectileType = (ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke366(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DecalAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke367(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DecalBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke368(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DecalSize = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke369(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).DPS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke370(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonAction = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke371(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonData = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke372(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventButtonString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke373(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).EventFeaturesString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke374(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke375(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).FactoryRotation = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke376(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).FactoryScaleFactor = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke377(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).FavoriteTargetType = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke378(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).GunSequence = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke379(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Health = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke380(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).HologramUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke381(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke382(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke383(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke384(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke385(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke386(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke387(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke388(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke389(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockRotation = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke390(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconUnlockScale = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke391(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).InfoUIType = (InfoUIType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke392(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsFlying = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke393(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsHealer = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke394(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Lvl = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke395(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Materials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke396(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke397(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).NearnessPercentile = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke398(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke399(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).OverWalls = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke400(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).PlayerFacing = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke401(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Preference = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke402(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).PreferencePercentile = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke403(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).PreventDonation = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke404(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileType = (ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke405(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).RotationSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke406(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).RunSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke407(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).RunThreshold = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke408(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Sequences = (Dictionary<int, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke409(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).ShieldAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke410(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).ShieldHealth = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke411(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Size = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke412(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).SizeX = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke413(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).SizeY = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke414(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).SpawnApplyBuffs = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke415(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).SpawnEffectUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke416(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).StrictCooldown = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke417(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).TargetLocking = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke418(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).TargetSelf = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke419(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).TooltipHeightOffset = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke420(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).TrainingTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke421(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).TroopID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke422(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).TroopRole = (TroopRole)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke423(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Type = (TroopType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke424(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke425(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UIDecalAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke426(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UniqueAbilityDescVO = (TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke427(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UnlockedByEvent = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke428(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeContraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke429(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeCredits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke430(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeMaterials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke431(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShardCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke432(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeShardUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke433(long instance, long* args)
		{
			((TroopTypeVO)GCHandledObjects.GCHandleToObject(instance)).UpgradeTime = *(int*)args;
			return -1L;
		}
	}
}
