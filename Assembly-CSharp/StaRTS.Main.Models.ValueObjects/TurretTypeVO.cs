using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class TurretTypeVO : IValueObject, IShooterVO, ITrackerVO
	{
		public static int COLUMN_trackerName
		{
			get;
			private set;
		}

		public static int COLUMN_gunSequence
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

		public static int COLUMN_strictCoolDown
		{
			get;
			private set;
		}

		public static int COLUMN_clipRetargeting
		{
			get;
			private set;
		}

		public static int COLUMN_projectileType
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string TrackerName
		{
			get;
			set;
		}

		public int[] GunSequence
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

		public bool OverWalls
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

		public ProjectileTypeVO ProjectileType
		{
			get;
			set;
		}

		public Dictionary<int, int> Sequences
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.TrackerName = row.TryGetString(TurretTypeVO.COLUMN_trackerName);
			this.Preference = new int[23];
			int i = 0;
			int num = 23;
			while (i < num)
			{
				this.Preference[i] = 0;
				i++;
			}
			this.Preference[1] = row.TryGetInt(TurretTypeVO.COLUMN_wall);
			this.Preference[2] = row.TryGetInt(TurretTypeVO.COLUMN_building);
			this.Preference[3] = row.TryGetInt(TurretTypeVO.COLUMN_storage);
			this.Preference[4] = row.TryGetInt(TurretTypeVO.COLUMN_resource);
			this.Preference[5] = row.TryGetInt(TurretTypeVO.COLUMN_turret);
			this.Preference[6] = row.TryGetInt(TurretTypeVO.COLUMN_HQ);
			this.Preference[7] = row.TryGetInt(TurretTypeVO.COLUMN_shield);
			this.Preference[8] = row.TryGetInt(TurretTypeVO.COLUMN_shieldGenerator);
			this.Preference[9] = row.TryGetInt(TurretTypeVO.COLUMN_infantry);
			this.Preference[10] = row.TryGetInt(TurretTypeVO.COLUMN_bruiserInfantry);
			this.Preference[11] = row.TryGetInt(TurretTypeVO.COLUMN_vehicle);
			this.Preference[12] = row.TryGetInt(TurretTypeVO.COLUMN_bruiserVehicle);
			this.Preference[13] = row.TryGetInt(TurretTypeVO.COLUMN_heroInfantry);
			this.Preference[14] = row.TryGetInt(TurretTypeVO.COLUMN_heroVehicle);
			this.Preference[15] = row.TryGetInt(TurretTypeVO.COLUMN_heroBruiserInfantry);
			this.Preference[16] = row.TryGetInt(TurretTypeVO.COLUMN_heroBruiserVehicle);
			this.Preference[17] = row.TryGetInt(TurretTypeVO.COLUMN_flierInfantry);
			this.Preference[18] = row.TryGetInt(TurretTypeVO.COLUMN_flierVehicle);
			this.Preference[19] = row.TryGetInt(TurretTypeVO.COLUMN_healerInfantry);
			this.Preference[20] = row.TryGetInt(TurretTypeVO.COLUMN_trap);
			this.Preference[21] = row.TryGetInt(TurretTypeVO.COLUMN_champion);
			this.PreferencePercentile = row.TryGetInt(TurretTypeVO.COLUMN_targetPreferenceStrength);
			this.NearnessPercentile = 100 - this.PreferencePercentile;
			this.Damage = row.TryGetInt(TurretTypeVO.COLUMN_damage);
			this.DPS = row.TryGetInt(TurretTypeVO.COLUMN_dps);
			this.FavoriteTargetType = row.TryGetString(TurretTypeVO.COLUMN_favoriteTargetType);
			this.MinAttackRange = row.TryGetUint(TurretTypeVO.COLUMN_minAttackRange);
			this.MaxAttackRange = row.TryGetUint(TurretTypeVO.COLUMN_maxAttackRange);
			this.ShotCount = row.TryGetUint(TurretTypeVO.COLUMN_shotCount);
			this.WarmupDelay = row.TryGetUint(TurretTypeVO.COLUMN_chargeTime);
			this.AnimationDelay = row.TryGetUint(TurretTypeVO.COLUMN_animationDelay);
			this.ShotDelay = row.TryGetUint(TurretTypeVO.COLUMN_shotDelay);
			this.CooldownDelay = row.TryGetUint(TurretTypeVO.COLUMN_reload);
			this.StrictCooldown = row.TryGetBool(TurretTypeVO.COLUMN_strictCoolDown);
			this.ClipRetargeting = row.TryGetBool(TurretTypeVO.COLUMN_clipRetargeting);
			this.ProjectileType = Service.Get<IDataController>().Get<ProjectileTypeVO>(row.TryGetString(TurretTypeVO.COLUMN_projectileType));
			if (this.ProjectileType.IsBeam && (long)this.ProjectileType.BeamDamageLength < (long)((ulong)this.MaxAttackRange))
			{
				Service.Get<StaRTSLogger>().WarnFormat("Turret {0} can target something it can't damage", new object[]
				{
					this.Uid
				});
			}
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			SequencePair gunSequences = valueObjectController.GetGunSequences(this.Uid, row.TryGetString(TurretTypeVO.COLUMN_gunSequence));
			this.GunSequence = gunSequences.GunSequence;
			this.Sequences = gunSequences.Sequences;
		}

		public TurretTypeVO()
		{
		}

		protected internal TurretTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).ClipRetargeting);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_animationDelay);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_bruiserInfantry);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_bruiserVehicle);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_building);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_champion);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_chargeTime);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_clipRetargeting);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_damage);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_dps);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_favoriteTargetType);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_flierInfantry);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_flierVehicle);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_gunSequence);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_healerInfantry);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_heroBruiserInfantry);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_heroBruiserVehicle);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_heroInfantry);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_heroVehicle);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_HQ);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_infantry);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_maxAttackRange);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_minAttackRange);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_projectileType);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_reload);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_resource);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_shield);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_shieldGenerator);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_shotCount);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_shotDelay);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_storage);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_strictCoolDown);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_targetPreferenceStrength);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_trackerName);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_trap);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_turret);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_vehicle);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TurretTypeVO.COLUMN_wall);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).Damage);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).DPS);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).FavoriteTargetType);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).GunSequence);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).NearnessPercentile);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).OverWalls);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).Preference);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).PreferencePercentile);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileType);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).Sequences);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).StrictCooldown);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).TrackerName);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).ClipRetargeting = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			TurretTypeVO.COLUMN_animationDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			TurretTypeVO.COLUMN_bruiserInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			TurretTypeVO.COLUMN_bruiserVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			TurretTypeVO.COLUMN_building = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			TurretTypeVO.COLUMN_champion = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			TurretTypeVO.COLUMN_chargeTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			TurretTypeVO.COLUMN_clipRetargeting = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			TurretTypeVO.COLUMN_damage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			TurretTypeVO.COLUMN_dps = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			TurretTypeVO.COLUMN_favoriteTargetType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			TurretTypeVO.COLUMN_flierInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			TurretTypeVO.COLUMN_flierVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			TurretTypeVO.COLUMN_gunSequence = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			TurretTypeVO.COLUMN_healerInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			TurretTypeVO.COLUMN_heroBruiserInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			TurretTypeVO.COLUMN_heroBruiserVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			TurretTypeVO.COLUMN_heroInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			TurretTypeVO.COLUMN_heroVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			TurretTypeVO.COLUMN_HQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			TurretTypeVO.COLUMN_infantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			TurretTypeVO.COLUMN_maxAttackRange = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			TurretTypeVO.COLUMN_minAttackRange = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			TurretTypeVO.COLUMN_projectileType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			TurretTypeVO.COLUMN_reload = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			TurretTypeVO.COLUMN_resource = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			TurretTypeVO.COLUMN_shield = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			TurretTypeVO.COLUMN_shieldGenerator = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			TurretTypeVO.COLUMN_shotCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			TurretTypeVO.COLUMN_shotDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			TurretTypeVO.COLUMN_storage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			TurretTypeVO.COLUMN_strictCoolDown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			TurretTypeVO.COLUMN_targetPreferenceStrength = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			TurretTypeVO.COLUMN_trackerName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			TurretTypeVO.COLUMN_trap = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			TurretTypeVO.COLUMN_turret = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			TurretTypeVO.COLUMN_vehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			TurretTypeVO.COLUMN_wall = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).Damage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).DPS = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).FavoriteTargetType = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).GunSequence = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).NearnessPercentile = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).OverWalls = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).Preference = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).PreferencePercentile = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileType = (ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).Sequences = (Dictionary<int, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).StrictCooldown = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).TrackerName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			((TurretTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
