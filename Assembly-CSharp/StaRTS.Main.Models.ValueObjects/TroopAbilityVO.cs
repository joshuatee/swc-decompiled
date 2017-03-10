using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class TroopAbilityVO : IValueObject, ITroopShooterVO, IShooterVO, ISpeedVO
	{
		public static int COLUMN_selfBuff
		{
			get;
			private set;
		}

		public static int COLUMN_targetSelf
		{
			get;
			private set;
		}

		public static int COLUMN_duration
		{
			get;
			private set;
		}

		public static int COLUMN_persistentEffect
		{
			get;
			private set;
		}

		public static int COLUMN_persistentScaling
		{
			get;
			private set;
		}

		public static int COLUMN_gunSequence
		{
			get;
			private set;
		}

		public static int COLUMN_damage
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

		public static int COLUMN_projectileType
		{
			get;
			private set;
		}

		public static int COLUMN_overWalls
		{
			get;
			private set;
		}

		public static int COLUMN_favoriteTargetType
		{
			get;
			private set;
		}

		public static int COLUMN_targetLocking
		{
			get;
			private set;
		}

		public static int COLUMN_cooldownTime
		{
			get;
			private set;
		}

		public static int COLUMN_armingDelay
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

		public static int COLUMN_shotCount
		{
			get;
			private set;
		}

		public static int COLUMN_targetPreferenceStrength
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

		public static int COLUMN_heroBruiserVehicle
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

		public static int COLUMN_healerInfantry
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

		public static int COLUMN_audioAbilityActivate
		{
			get;
			private set;
		}

		public static int COLUMN_audioAbilityAttack
		{
			get;
			private set;
		}

		public static int COLUMN_audioAbilityLoop
		{
			get;
			private set;
		}

		public static int COLUMN_maxSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_newRotationSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_auto
		{
			get;
			private set;
		}

		public static int COLUMN_clipCount
		{
			get;
			private set;
		}

		public static int COLUMN_cooldownOnSpawn
		{
			get;
			private set;
		}

		public static int COLUMN_weaponTrailFxParams
		{
			get;
			private set;
		}

		public static int COLUMN_retargetingOffset
		{
			get;
			private set;
		}

		public static int COLUMN_clipRetargeting
		{
			get;
			private set;
		}

		public static int COLUMN_strictCoolDown
		{
			get;
			private set;
		}

		public static int COLUMN_dps
		{
			get;
			private set;
		}

		public static int COLUMN_altGunLocators
		{
			get;
			private set;
		}

		public static int COLUMN_recastAbility
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string SelfBuff
		{
			get;
			set;
		}

		public uint CoolDownTime
		{
			get;
			set;
		}

		public uint Duration
		{
			get;
			set;
		}

		public string PersistentEffect
		{
			get;
			set;
		}

		public float PersistentScaling
		{
			get;
			set;
		}

		public bool Auto
		{
			get;
			private set;
		}

		public int ClipCount
		{
			get;
			private set;
		}

		public bool CooldownOnSpawn
		{
			get;
			private set;
		}

		public float[] WeaponTrailFxParams
		{
			get;
			private set;
		}

		public int[] AltGunLocators
		{
			get;
			private set;
		}

		public int[] GunSequence
		{
			get;
			set;
		}

		public int Damage
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

		public ProjectileTypeVO ProjectileType
		{
			get;
			set;
		}

		public bool OverWalls
		{
			get;
			set;
		}

		public string FavoriteTargetType
		{
			get;
			set;
		}

		public bool TargetLocking
		{
			get;
			set;
		}

		public uint ArmingDelay
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

		public int[] Preference
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

		public List<StrIntPair> AudioAbilityActivate
		{
			get;
			set;
		}

		public List<StrIntPair> AudioAbilityAttack
		{
			get;
			set;
		}

		public List<StrIntPair> AudioAbilityLoop
		{
			get;
			set;
		}

		public bool TargetSelf
		{
			get;
			set;
		}

		public Dictionary<int, int> Sequences
		{
			get;
			private set;
		}

		public uint RetargetingOffset
		{
			get;
			set;
		}

		public bool ClipRetargeting
		{
			get;
			set;
		}

		public bool StrictCooldown
		{
			get;
			set;
		}

		public int DPS
		{
			get;
			set;
		}

		public bool RecastAbility
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.SelfBuff = row.TryGetString(TroopAbilityVO.COLUMN_selfBuff);
			this.TargetSelf = row.TryGetBool(TroopAbilityVO.COLUMN_targetSelf);
			this.Duration = row.TryGetUint(TroopAbilityVO.COLUMN_duration);
			this.PersistentEffect = row.TryGetString(TroopAbilityVO.COLUMN_persistentEffect);
			this.PersistentScaling = (float)(100 + row.TryGetInt(TroopAbilityVO.COLUMN_persistentScaling)) * 0.01f;
			this.Damage = row.TryGetInt(TroopAbilityVO.COLUMN_damage);
			this.ViewRange = row.TryGetUint(TroopAbilityVO.COLUMN_viewRange);
			this.MinAttackRange = row.TryGetUint(TroopAbilityVO.COLUMN_minAttackRange);
			this.MaxAttackRange = row.TryGetUint(TroopAbilityVO.COLUMN_maxAttackRange);
			string text = row.TryGetString(TroopAbilityVO.COLUMN_projectileType);
			if (!string.IsNullOrEmpty(text))
			{
				this.ProjectileType = Service.Get<IDataController>().Get<ProjectileTypeVO>(text);
			}
			this.OverWalls = row.TryGetBool(TroopAbilityVO.COLUMN_overWalls);
			this.FavoriteTargetType = row.TryGetString(TroopAbilityVO.COLUMN_favoriteTargetType);
			this.TargetLocking = row.TryGetBool(TroopAbilityVO.COLUMN_targetLocking);
			this.CoolDownTime = row.TryGetUint(TroopAbilityVO.COLUMN_cooldownTime);
			this.ArmingDelay = row.TryGetUint(TroopAbilityVO.COLUMN_armingDelay);
			this.WarmupDelay = row.TryGetUint(TroopAbilityVO.COLUMN_chargeTime);
			this.AnimationDelay = row.TryGetUint(TroopAbilityVO.COLUMN_animationDelay);
			this.ShotDelay = row.TryGetUint(TroopAbilityVO.COLUMN_shotDelay);
			this.CooldownDelay = row.TryGetUint(TroopAbilityVO.COLUMN_reload);
			this.ShotCount = row.TryGetUint(TroopAbilityVO.COLUMN_shotCount);
			this.PreferencePercentile = row.TryGetInt(TroopAbilityVO.COLUMN_targetPreferenceStrength);
			this.NearnessPercentile = 100 - this.PreferencePercentile;
			this.Preference = new int[23];
			int i = 0;
			int num = 23;
			while (i < num)
			{
				this.Preference[i] = 0;
				i++;
			}
			this.Preference[1] = row.TryGetInt(TroopAbilityVO.COLUMN_wall);
			this.Preference[2] = row.TryGetInt(TroopAbilityVO.COLUMN_building);
			this.Preference[3] = row.TryGetInt(TroopAbilityVO.COLUMN_storage);
			this.Preference[4] = row.TryGetInt(TroopAbilityVO.COLUMN_resource);
			this.Preference[5] = row.TryGetInt(TroopAbilityVO.COLUMN_turret);
			this.Preference[6] = row.TryGetInt(TroopAbilityVO.COLUMN_HQ);
			this.Preference[7] = row.TryGetInt(TroopAbilityVO.COLUMN_shield);
			this.Preference[8] = row.TryGetInt(TroopAbilityVO.COLUMN_shieldGenerator);
			this.Preference[9] = row.TryGetInt(TroopAbilityVO.COLUMN_infantry);
			this.Preference[10] = row.TryGetInt(TroopAbilityVO.COLUMN_bruiserInfantry);
			this.Preference[11] = row.TryGetInt(TroopAbilityVO.COLUMN_vehicle);
			this.Preference[12] = row.TryGetInt(TroopAbilityVO.COLUMN_bruiserVehicle);
			this.Preference[13] = row.TryGetInt(TroopAbilityVO.COLUMN_heroInfantry);
			this.Preference[14] = row.TryGetInt(TroopAbilityVO.COLUMN_heroVehicle);
			this.Preference[15] = row.TryGetInt(TroopAbilityVO.COLUMN_heroBruiserInfantry);
			this.Preference[16] = row.TryGetInt(TroopAbilityVO.COLUMN_heroBruiserVehicle);
			this.Preference[17] = row.TryGetInt(TroopAbilityVO.COLUMN_flierInfantry);
			this.Preference[18] = row.TryGetInt(TroopAbilityVO.COLUMN_flierVehicle);
			this.Preference[19] = row.TryGetInt(TroopAbilityVO.COLUMN_healerInfantry);
			this.Preference[20] = row.TryGetInt(TroopAbilityVO.COLUMN_trap);
			this.Preference[21] = row.TryGetInt(TroopAbilityVO.COLUMN_champion);
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			this.AudioAbilityActivate = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(TroopAbilityVO.COLUMN_audioAbilityActivate));
			this.AudioAbilityAttack = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(TroopAbilityVO.COLUMN_audioAbilityAttack));
			this.AudioAbilityLoop = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(TroopAbilityVO.COLUMN_audioAbilityLoop));
			this.RecastAbility = row.TryGetBool(TroopAbilityVO.COLUMN_recastAbility);
			SequencePair gunSequences = valueObjectController.GetGunSequences(this.Uid, row.TryGetString(TroopAbilityVO.COLUMN_gunSequence));
			this.GunSequence = gunSequences.GunSequence;
			this.Sequences = gunSequences.Sequences;
			this.MaxSpeed = row.TryGetInt(TroopAbilityVO.COLUMN_maxSpeed);
			this.RotationSpeed = row.TryGetInt(TroopAbilityVO.COLUMN_newRotationSpeed);
			this.Auto = row.TryGetBool(TroopAbilityVO.COLUMN_auto);
			this.ClipCount = row.TryGetInt(TroopAbilityVO.COLUMN_clipCount);
			this.CooldownOnSpawn = row.TryGetBool(TroopAbilityVO.COLUMN_cooldownOnSpawn);
			this.WeaponTrailFxParams = row.TryGetFloatArray(TroopAbilityVO.COLUMN_weaponTrailFxParams);
			this.RetargetingOffset = row.TryGetUint(TroopAbilityVO.COLUMN_retargetingOffset);
			this.ClipRetargeting = row.TryGetBool(TroopAbilityVO.COLUMN_clipRetargeting);
			this.StrictCooldown = row.TryGetBool(TroopAbilityVO.COLUMN_strictCoolDown);
			this.DPS = row.TryGetInt(TroopAbilityVO.COLUMN_dps);
			this.AltGunLocators = row.TryGetIntArray(TroopAbilityVO.COLUMN_altGunLocators);
		}

		public TroopAbilityVO()
		{
		}

		protected internal TroopAbilityVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).AltGunLocators);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).AudioAbilityActivate);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).AudioAbilityAttack);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).AudioAbilityLoop);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).Auto);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).ClipCount);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).ClipRetargeting);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_altGunLocators);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_animationDelay);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_armingDelay);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_audioAbilityActivate);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_audioAbilityAttack);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_audioAbilityLoop);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_auto);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_bruiserInfantry);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_bruiserVehicle);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_building);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_champion);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_chargeTime);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_clipCount);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_clipRetargeting);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_cooldownOnSpawn);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_cooldownTime);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_damage);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_dps);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_duration);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_favoriteTargetType);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_flierInfantry);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_flierVehicle);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_gunSequence);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_healerInfantry);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_heroBruiserInfantry);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_heroBruiserVehicle);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_heroInfantry);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_heroVehicle);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_HQ);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_infantry);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_maxAttackRange);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_maxSpeed);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_minAttackRange);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_newRotationSpeed);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_overWalls);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_persistentEffect);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_persistentScaling);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_projectileType);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_recastAbility);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_reload);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_resource);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_retargetingOffset);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_selfBuff);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_shield);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_shieldGenerator);
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_shotCount);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_shotDelay);
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_storage);
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_strictCoolDown);
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_targetLocking);
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_targetPreferenceStrength);
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_targetSelf);
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_trap);
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_turret);
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_vehicle);
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_viewRange);
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_wall);
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopAbilityVO.COLUMN_weaponTrailFxParams);
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).CooldownOnSpawn);
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).Damage);
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).DPS);
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).FavoriteTargetType);
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).GunSequence);
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed);
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).NearnessPercentile);
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).OverWalls);
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).PersistentEffect);
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).PersistentScaling);
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).Preference);
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).PreferencePercentile);
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileType);
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).RecastAbility);
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).RotationSpeed);
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).SelfBuff);
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).Sequences);
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).StrictCooldown);
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).TargetLocking);
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).TargetSelf);
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).WeaponTrailFxParams);
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).AltGunLocators = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).AudioAbilityActivate = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).AudioAbilityAttack = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).AudioAbilityLoop = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).Auto = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).ClipCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).ClipRetargeting = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_altGunLocators = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_animationDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_armingDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_audioAbilityActivate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_audioAbilityAttack = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_audioAbilityLoop = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_auto = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_bruiserInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_bruiserVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_building = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_champion = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_chargeTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_clipCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_clipRetargeting = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_cooldownOnSpawn = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_cooldownTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_damage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_dps = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_duration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_favoriteTargetType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_flierInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_flierVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_gunSequence = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_healerInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_heroBruiserInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_heroBruiserVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_heroInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_heroVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_HQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_infantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_maxAttackRange = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_maxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_minAttackRange = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_newRotationSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_overWalls = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_persistentEffect = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_persistentScaling = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_projectileType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_recastAbility = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_reload = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_resource = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_retargetingOffset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_selfBuff = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_shield = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_shieldGenerator = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_shotCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke141(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_shotDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke142(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_storage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke143(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_strictCoolDown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke144(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_targetLocking = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke145(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_targetPreferenceStrength = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke146(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_targetSelf = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke147(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_trap = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke148(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_turret = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke149(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_vehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke150(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_viewRange = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke151(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_wall = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke152(long instance, long* args)
		{
			TroopAbilityVO.COLUMN_weaponTrailFxParams = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke153(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).CooldownOnSpawn = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke154(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).Damage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke155(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).DPS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke156(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).FavoriteTargetType = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke157(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).GunSequence = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke158(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke159(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).NearnessPercentile = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke160(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).OverWalls = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke161(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).PersistentEffect = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke162(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).PersistentScaling = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke163(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).Preference = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke164(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).PreferencePercentile = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke165(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileType = (ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke166(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).RecastAbility = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke167(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).RotationSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke168(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).SelfBuff = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke169(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).Sequences = (Dictionary<int, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke170(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).StrictCooldown = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke171(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).TargetLocking = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke172(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).TargetSelf = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke173(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke174(long instance, long* args)
		{
			((TroopAbilityVO)GCHandledObjects.GCHandleToObject(instance)).WeaponTrailFxParams = (float[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}
	}
}
