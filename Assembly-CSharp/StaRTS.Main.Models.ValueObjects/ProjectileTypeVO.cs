using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class ProjectileTypeVO : IValueObject, ISplashVO, IAssetVO
	{
		private const int BEAM_EXTEND_FACTOR = 4;

		private const int BEAM_MIN_BULLET_LENGTH = 2;

		private const int BEAM_MAX_BULLET_LENGTH = 6;

		public List<int> DamageMultipliers;

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

		public static int COLUMN_bullet
		{
			get;
			private set;
		}

		public static int COLUMN_groundBullet
		{
			get;
			private set;
		}

		public static int COLUMN_chargeAssetName
		{
			get;
			private set;
		}

		public static int COLUMN_muzzleFlash
		{
			get;
			private set;
		}

		public static int COLUMN_hitSpark
		{
			get;
			private set;
		}

		public static int COLUMN_directional
		{
			get;
			private set;
		}

		public static int COLUMN_muzzleFlashFadeTime
		{
			get;
			private set;
		}

		public static int COLUMN_arcs
		{
			get;
			private set;
		}

		public static int COLUMN_arcHeight
		{
			get;
			private set;
		}

		public static int COLUMN_spinSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_arcRange
		{
			get;
			private set;
		}

		public static int COLUMN_maxSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_s1Time
		{
			get;
			private set;
		}

		public static int COLUMN_sTransition
		{
			get;
			private set;
		}

		public static int COLUMN_s2Time
		{
			get;
			private set;
		}

		public static int COLUMN_seeksTarget
		{
			get;
			private set;
		}

		public static int COLUMN_isDeflectable
		{
			get;
			private set;
		}

		public static int COLUMN_passThroughShield
		{
			get;
			private set;
		}

		public static int COLUMN_splashDamagePercentages
		{
			get;
			private set;
		}

		public static int COLUMN_widthSegments
		{
			get;
			private set;
		}

		public static int COLUMN_lengthSegments
		{
			get;
			private set;
		}

		public static int COLUMN_projectileLength
		{
			get;
			private set;
		}

		public static int COLUMN_applyBuffs
		{
			get;
			private set;
		}

		public static int COLUMN_beamDamage
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string BulletAssetName
		{
			get;
			set;
		}

		public string GroundBulletAssetName
		{
			get;
			set;
		}

		public string ChargeAssetName
		{
			get;
			set;
		}

		public string MuzzleFlashAssetName
		{
			get;
			set;
		}

		public string HitSparkAssetName
		{
			get;
			set;
		}

		public string AssetName
		{
			get
			{
				return this.BulletAssetName;
			}
			set
			{
			}
		}

		public float MuzzleFlashFadeTime
		{
			get;
			private set;
		}

		public bool Directional
		{
			get;
			set;
		}

		public bool Arcs
		{
			get;
			set;
		}

		public int ArcHeight
		{
			get;
			set;
		}

		public int ArcRange
		{
			get;
			set;
		}

		public int MaxSpeed
		{
			get;
			set;
		}

		public float SpinSpeed
		{
			get;
			private set;
		}

		public uint Stage1Duration
		{
			get;
			set;
		}

		public uint StageTransitionDuration
		{
			get;
			set;
		}

		public uint Stage2Duration
		{
			get;
			set;
		}

		public bool SeeksTarget
		{
			get;
			set;
		}

		public bool IsDeflectable
		{
			get;
			set;
		}

		public bool PassThroughShield
		{
			get;
			set;
		}

		public int BeamDamage
		{
			get;
			set;
		}

		public int SplashRadius
		{
			get;
			set;
		}

		public int[] SplashDamagePercentages
		{
			get;
			set;
		}

		public int[] BeamWidthSegments
		{
			get;
			private set;
		}

		public int[] BeamLengthSegments
		{
			get;
			private set;
		}

		public bool IsBeam
		{
			get;
			private set;
		}

		public int BeamBulletLength
		{
			get;
			private set;
		}

		public int BeamDamageLength
		{
			get;
			private set;
		}

		public int BeamInitialZeroes
		{
			get;
			private set;
		}

		public int BeamLifeLength
		{
			get;
			private set;
		}

		public int BeamEmitterLength
		{
			get;
			private set;
		}

		public string[] ApplyBuffs
		{
			get;
			private set;
		}

		public bool IsMultiStage
		{
			get
			{
				return this.Stage1Duration > 0u && this.ArcRange > 0 && this.ArcHeight > 0;
			}
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			int num = 23;
			this.DamageMultipliers = new List<int>(num);
			for (int i = 0; i < num; i++)
			{
				this.DamageMultipliers.Add(-1);
			}
			this.DamageMultipliers[1] = row.TryGetInt(ProjectileTypeVO.COLUMN_wall);
			this.DamageMultipliers[2] = row.TryGetInt(ProjectileTypeVO.COLUMN_building);
			this.DamageMultipliers[3] = row.TryGetInt(ProjectileTypeVO.COLUMN_storage);
			this.DamageMultipliers[4] = row.TryGetInt(ProjectileTypeVO.COLUMN_resource);
			this.DamageMultipliers[5] = row.TryGetInt(ProjectileTypeVO.COLUMN_turret);
			this.DamageMultipliers[6] = row.TryGetInt(ProjectileTypeVO.COLUMN_HQ);
			this.DamageMultipliers[7] = row.TryGetInt(ProjectileTypeVO.COLUMN_shield);
			this.DamageMultipliers[8] = row.TryGetInt(ProjectileTypeVO.COLUMN_shieldGenerator);
			this.DamageMultipliers[9] = row.TryGetInt(ProjectileTypeVO.COLUMN_infantry);
			this.DamageMultipliers[10] = row.TryGetInt(ProjectileTypeVO.COLUMN_bruiserInfantry);
			this.DamageMultipliers[11] = row.TryGetInt(ProjectileTypeVO.COLUMN_vehicle);
			this.DamageMultipliers[12] = row.TryGetInt(ProjectileTypeVO.COLUMN_bruiserVehicle);
			this.DamageMultipliers[13] = row.TryGetInt(ProjectileTypeVO.COLUMN_heroInfantry);
			this.DamageMultipliers[14] = row.TryGetInt(ProjectileTypeVO.COLUMN_heroVehicle);
			this.DamageMultipliers[15] = row.TryGetInt(ProjectileTypeVO.COLUMN_heroBruiserInfantry);
			this.DamageMultipliers[16] = row.TryGetInt(ProjectileTypeVO.COLUMN_heroBruiserVehicle);
			this.DamageMultipliers[17] = row.TryGetInt(ProjectileTypeVO.COLUMN_flierInfantry);
			this.DamageMultipliers[18] = row.TryGetInt(ProjectileTypeVO.COLUMN_flierVehicle);
			this.DamageMultipliers[19] = row.TryGetInt(ProjectileTypeVO.COLUMN_healerInfantry);
			this.DamageMultipliers[20] = row.TryGetInt(ProjectileTypeVO.COLUMN_trap);
			this.DamageMultipliers[21] = row.TryGetInt(ProjectileTypeVO.COLUMN_champion);
			this.BulletAssetName = row.TryGetString(ProjectileTypeVO.COLUMN_bullet);
			this.GroundBulletAssetName = row.TryGetString(ProjectileTypeVO.COLUMN_groundBullet);
			this.ChargeAssetName = row.TryGetString(ProjectileTypeVO.COLUMN_chargeAssetName);
			this.MuzzleFlashAssetName = row.TryGetString(ProjectileTypeVO.COLUMN_muzzleFlash);
			this.HitSparkAssetName = row.TryGetString(ProjectileTypeVO.COLUMN_hitSpark);
			this.Directional = row.TryGetBool(ProjectileTypeVO.COLUMN_directional);
			this.MuzzleFlashFadeTime = row.TryGetFloat(ProjectileTypeVO.COLUMN_muzzleFlashFadeTime);
			this.Arcs = row.TryGetBool(ProjectileTypeVO.COLUMN_arcs);
			this.ArcHeight = row.TryGetInt(ProjectileTypeVO.COLUMN_arcHeight);
			this.ArcRange = row.TryGetInt(ProjectileTypeVO.COLUMN_arcRange);
			this.MaxSpeed = row.TryGetInt(ProjectileTypeVO.COLUMN_maxSpeed);
			this.SpinSpeed = row.TryGetFloat(ProjectileTypeVO.COLUMN_spinSpeed, 0f);
			this.Stage1Duration = row.TryGetUint(ProjectileTypeVO.COLUMN_s1Time);
			this.StageTransitionDuration = row.TryGetUint(ProjectileTypeVO.COLUMN_sTransition);
			this.Stage2Duration = row.TryGetUint(ProjectileTypeVO.COLUMN_s2Time);
			this.SeeksTarget = row.TryGetBool(ProjectileTypeVO.COLUMN_seeksTarget);
			this.IsDeflectable = row.TryGetBool(ProjectileTypeVO.COLUMN_isDeflectable);
			this.PassThroughShield = row.TryGetBool(ProjectileTypeVO.COLUMN_passThroughShield);
			this.BeamDamage = row.TryGetInt(ProjectileTypeVO.COLUMN_beamDamage);
			this.SplashDamagePercentages = row.TryGetIntArray(ProjectileTypeVO.COLUMN_splashDamagePercentages);
			if (this.HasSplashDamage())
			{
				this.SplashRadius = this.SplashDamagePercentages.Length - 1;
			}
			else
			{
				this.SplashRadius = 0;
			}
			this.BeamWidthSegments = row.TryGetIntArray(ProjectileTypeVO.COLUMN_widthSegments);
			this.BeamLengthSegments = row.TryGetIntArray(ProjectileTypeVO.COLUMN_lengthSegments);
			this.IsBeam = (this.BeamWidthSegments != null && this.BeamWidthSegments.Length != 0 && this.BeamLengthSegments != null && this.BeamLengthSegments.Length != 0);
			this.BeamBulletLength = row.TryGetInt(ProjectileTypeVO.COLUMN_projectileLength, 0);
			if (this.IsBeam)
			{
				int num2 = this.BeamLengthSegments.Length;
				this.BeamEmitterLength = num2;
				this.BeamLifeLength = num2 * 4;
				this.BeamDamageLength = 0;
				for (int j = num2 - 1; j >= 0; j--)
				{
					int num3 = this.BeamLengthSegments[j];
					this.BeamLengthSegments[j] = ((num3 < 0) ? 0 : num3);
					if (this.BeamDamageLength == 0 && num3 > 0)
					{
						this.BeamDamageLength = j + 1;
					}
				}
				this.BeamInitialZeroes = 0;
				int num4 = 0;
				while (num4 < num2 && this.BeamLengthSegments[num4] == 0)
				{
					int beamInitialZeroes = this.BeamInitialZeroes;
					this.BeamInitialZeroes = beamInitialZeroes + 1;
					num4++;
				}
				if (this.BeamBulletLength < 2)
				{
					this.BeamBulletLength = 2;
				}
				else if (this.BeamBulletLength > 6)
				{
					Service.Get<StaRTSLogger>().WarnFormat("Beam projectileLength too large ({0}) for {1} (max is {2})", new object[]
					{
						this.BeamBulletLength,
						this.Uid,
						6
					});
					this.BeamBulletLength = 6;
				}
			}
			else
			{
				this.BeamWidthSegments = null;
				this.BeamLengthSegments = null;
				this.BeamDamageLength = 0;
				this.BeamInitialZeroes = 0;
				this.BeamLifeLength = 0;
				this.BeamEmitterLength = 0;
				this.BeamBulletLength = 0;
			}
			this.ApplyBuffs = null;
			string text = row.TryGetString(ProjectileTypeVO.COLUMN_applyBuffs);
			if (!string.IsNullOrEmpty(text))
			{
				this.ApplyBuffs = text.Split(new char[]
				{
					','
				});
			}
		}

		public bool HasSplashDamage()
		{
			return this.SplashDamagePercentages != null && this.SplashDamagePercentages.Length != 0;
		}

		public int GetSplashDamagePercent(int distFromImpact)
		{
			int result = 0;
			if (distFromImpact < this.SplashDamagePercentages.Length)
			{
				result = this.SplashDamagePercentages[distFromImpact];
			}
			return result;
		}

		public string GetBulletAssetName(bool isOnGround)
		{
			if (!isOnGround)
			{
				return this.BulletAssetName;
			}
			return this.GroundBulletAssetName;
		}

		public ProjectileTypeVO()
		{
		}

		protected internal ProjectileTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).ApplyBuffs);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).ArcHeight);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).ArcRange);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).Arcs);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamBulletLength);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamDamage);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamDamageLength);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamEmitterLength);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamInitialZeroes);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamLengthSegments);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamLifeLength);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamWidthSegments);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BulletAssetName);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).ChargeAssetName);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_applyBuffs);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_arcHeight);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_arcRange);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_arcs);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_beamDamage);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_bruiserInfantry);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_bruiserVehicle);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_building);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_bullet);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_champion);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_chargeAssetName);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_directional);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_flierInfantry);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_flierVehicle);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_groundBullet);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_healerInfantry);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_heroBruiserInfantry);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_heroBruiserVehicle);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_heroInfantry);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_heroVehicle);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_hitSpark);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_HQ);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_infantry);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_isDeflectable);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_lengthSegments);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_maxSpeed);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_muzzleFlash);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_muzzleFlashFadeTime);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_passThroughShield);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_projectileLength);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_resource);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_s1Time);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_s2Time);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_seeksTarget);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_shield);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_shieldGenerator);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_spinSpeed);
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_splashDamagePercentages);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_storage);
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_sTransition);
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_trap);
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_turret);
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_vehicle);
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_wall);
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileTypeVO.COLUMN_widthSegments);
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).Directional);
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).GroundBulletAssetName);
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).HitSparkAssetName);
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsBeam);
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsDeflectable);
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsMultiStage);
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed);
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).MuzzleFlashAssetName);
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).MuzzleFlashFadeTime);
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).PassThroughShield);
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).SeeksTarget);
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).SpinSpeed);
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).SplashDamagePercentages);
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).SplashRadius);
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).GetBulletAssetName(*(sbyte*)args != 0));
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).GetSplashDamagePercent(*(int*)args));
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).HasSplashDamage());
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).ApplyBuffs = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).ArcHeight = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).ArcRange = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).Arcs = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamBulletLength = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamDamage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamDamageLength = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamEmitterLength = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamInitialZeroes = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamLengthSegments = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamLifeLength = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BeamWidthSegments = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).BulletAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).ChargeAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_applyBuffs = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_arcHeight = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_arcRange = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_arcs = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_beamDamage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_bruiserInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_bruiserVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_building = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_bullet = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_champion = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_chargeAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_directional = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_flierInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_flierVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_groundBullet = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_healerInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_heroBruiserInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_heroBruiserVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_heroInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_heroVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_hitSpark = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_HQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_infantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_isDeflectable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_lengthSegments = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_maxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_muzzleFlash = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_muzzleFlashFadeTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_passThroughShield = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_projectileLength = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_resource = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_s1Time = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_s2Time = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_seeksTarget = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_shield = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_shieldGenerator = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_spinSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_splashDamagePercentages = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_storage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_sTransition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_trap = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_turret = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_vehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_wall = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			ProjectileTypeVO.COLUMN_widthSegments = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).Directional = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).GroundBulletAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke141(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).HitSparkAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke142(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsBeam = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke143(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsDeflectable = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke144(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke145(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).MuzzleFlashAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke146(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).MuzzleFlashFadeTime = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke147(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).PassThroughShield = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke148(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).SeeksTarget = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke149(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).SpinSpeed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke150(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).SplashDamagePercentages = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke151(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).SplashRadius = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke152(long instance, long* args)
		{
			((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
