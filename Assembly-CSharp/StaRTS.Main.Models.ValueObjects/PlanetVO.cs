using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class PlanetVO : IValueObject, IAssetVO, IGeometryVO
	{
		public static int COLUMN_ambientMusic
		{
			get;
			private set;
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

		public static int COLUMN_loaderAssetName
		{
			get;
			private set;
		}

		public static int COLUMN_loaderBundleName
		{
			get;
			private set;
		}

		public static int COLUMN_loaderDisplayName
		{
			get;
			private set;
		}

		public static int COLUMN_planetaryLighting
		{
			get;
			private set;
		}

		public static int COLUMN_planetaryFX
		{
			get;
			private set;
		}

		public static int COLUMN_position
		{
			get;
			private set;
		}

		public static int COLUMN_galaxyAssetName
		{
			get;
			private set;
		}

		public static int COLUMN_galaxyBundleName
		{
			get;
			private set;
		}

		public static int COLUMN_footerTexture
		{
			get;
			private set;
		}

		public static int COLUMN_footerConflictTexture
		{
			get;
			private set;
		}

		public static int COLUMN_playerFacing
		{
			get;
			private set;
		}

		public static int COLUMN_order
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

		public static int COLUMN_currencyType
		{
			get;
			private set;
		}

		public static int COLUMN_currencyAmount
		{
			get;
			private set;
		}

		public static int COLUMN_leaderboardAssetBundle
		{
			get;
			private set;
		}

		public static int COLUMN_leaderboardTileTexture
		{
			get;
			private set;
		}

		public static int COLUMN_leaderboardButtonTexture
		{
			get;
			private set;
		}

		public static int COLUMN_abbreviation
		{
			get;
			private set;
		}

		public static int COLUMN_medalIconName
		{
			get;
			private set;
		}

		public static int COLUMN_planetBIName
		{
			get;
			private set;
		}

		public static int COLUMN_rebelMusic
		{
			get;
			private set;
		}

		public static int COLUMN_empireMusic
		{
			get;
			private set;
		}

		public static int COLUMN_battleMusic
		{
			get;
			private set;
		}

		public static int COLUMN_introStoryAction
		{
			get;
			private set;
		}

		public static int COLUMN_nightDuration
		{
			get;
			private set;
		}

		public static int COLUMN_sunriseDuration
		{
			get;
			private set;
		}

		public static int COLUMN_midDayDuration
		{
			get;
			private set;
		}

		public static int COLUMN_sunsetDuration
		{
			get;
			private set;
		}

		public static int COLUMN_cyclesPerDay
		{
			get;
			private set;
		}

		public static int COLUMN_sunriseMidnightOffset
		{
			get;
			private set;
		}

		public static int COLUMN_timeOfDayAssetBundle
		{
			get;
			private set;
		}

		public static int COLUMN_warBoardLightingAssetBundle
		{
			get;
			private set;
		}

		public static int COLUMN_warBoardAssetName
		{
			get;
			private set;
		}

		public static int COLUMN_warBoardBundleName
		{
			get;
			private set;
		}

		public static int COLUMN_perkBuildingHighlightColor
		{
			get;
			private set;
		}

		public static int COLUMN_planetLootUid
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string AmbientMusic
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

		public string LoadingScreenAssetName
		{
			get;
			set;
		}

		public string LoadingScreenBundleName
		{
			get;
			set;
		}

		public string LoadingScreenText
		{
			get;
			set;
		}

		public string PlanetaryLighting
		{
			get;
			set;
		}

		public string PlanetaryFX
		{
			get;
			set;
		}

		public int Population
		{
			get;
			set;
		}

		public float Angle
		{
			get;
			set;
		}

		public float Radius
		{
			get;
			set;
		}

		public float HeightOffset
		{
			get;
			set;
		}

		public string GalaxyAssetName
		{
			get;
			set;
		}

		public string GalaxyBundleName
		{
			get;
			set;
		}

		public string FooterTexture
		{
			get;
			set;
		}

		public string FooterConflictTexture
		{
			get;
			set;
		}

		public bool PlayerFacing
		{
			get;
			set;
		}

		public int Order
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

		public CurrencyType CurrencyType
		{
			get;
			set;
		}

		public int CurrencyAmount
		{
			get;
			set;
		}

		public string LeaderboardAssetBundle
		{
			get;
			set;
		}

		public string LeaderboardTileTexture
		{
			get;
			set;
		}

		public string LeaderboardButtonTexture
		{
			get;
			set;
		}

		public string Abbreviation
		{
			get;
			set;
		}

		public string MedalIconName
		{
			get;
			set;
		}

		public string PlanetBIName
		{
			get;
			set;
		}

		public string RebelMusic
		{
			get;
			set;
		}

		public string EmpireMusic
		{
			get;
			set;
		}

		public string BattleMusic
		{
			get;
			set;
		}

		public string IntroStoryAction
		{
			get;
			set;
		}

		public float NightDuration
		{
			get;
			set;
		}

		public float SunriseDuration
		{
			get;
			set;
		}

		public float MidDayDuration
		{
			get;
			set;
		}

		public float SunsetDuration
		{
			get;
			set;
		}

		public float CyclesPerDay
		{
			get;
			set;
		}

		public float SunriseMidnightOffset
		{
			get;
			set;
		}

		public string TimeOfDayAsset
		{
			get;
			set;
		}

		public string WarBoardLightingAsset
		{
			get;
			set;
		}

		public string WarBoardAssetName
		{
			get;
			set;
		}

		public string WarBoardBundleName
		{
			get;
			set;
		}

		public Color PlanetPerkShaderColor
		{
			get;
			private set;
		}

		public string PlanetLootUid
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.AmbientMusic = row.TryGetString(PlanetVO.COLUMN_ambientMusic);
			this.AssetName = row.TryGetString(PlanetVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(PlanetVO.COLUMN_bundleName);
			this.LoadingScreenAssetName = row.TryGetString(PlanetVO.COLUMN_loaderAssetName);
			this.LoadingScreenBundleName = row.TryGetString(PlanetVO.COLUMN_loaderBundleName);
			this.LoadingScreenText = row.TryGetString(PlanetVO.COLUMN_loaderDisplayName);
			this.PlanetaryLighting = row.TryGetString(PlanetVO.COLUMN_planetaryLighting);
			this.PlanetaryFX = row.TryGetString(PlanetVO.COLUMN_planetaryFX);
			Vector3 vector = row.TryGetVector3(PlanetVO.COLUMN_position);
			this.Angle = vector[0];
			this.Radius = vector[1];
			this.HeightOffset = vector[2];
			this.GalaxyAssetName = row.TryGetString(PlanetVO.COLUMN_galaxyAssetName);
			this.GalaxyBundleName = row.TryGetString(PlanetVO.COLUMN_galaxyBundleName);
			this.FooterTexture = row.TryGetString(PlanetVO.COLUMN_footerTexture);
			this.FooterConflictTexture = row.TryGetString(PlanetVO.COLUMN_footerConflictTexture);
			this.PlayerFacing = row.TryGetBool(PlanetVO.COLUMN_playerFacing);
			this.Order = row.TryGetInt(PlanetVO.COLUMN_order);
			this.IconAssetName = row.TryGetString(PlanetVO.COLUMN_iconAssetName);
			this.IconBundleName = row.TryGetString(PlanetVO.COLUMN_iconBundleName);
			this.IconCameraPosition = row.TryGetVector3(PlanetVO.COLUMN_iconCameraPosition);
			this.IconLookatPosition = row.TryGetVector3(PlanetVO.COLUMN_iconLookatPosition);
			this.IconCloseupCameraPosition = row.TryGetVector3(PlanetVO.COLUMN_iconCloseupCameraPosition, this.IconCameraPosition);
			this.IconCloseupLookatPosition = row.TryGetVector3(PlanetVO.COLUMN_iconCloseupLookatPosition, this.IconLookatPosition);
			this.CurrencyType = StringUtils.ParseEnum<CurrencyType>(row.TryGetString(PlanetVO.COLUMN_currencyType));
			this.CurrencyAmount = row.TryGetInt(PlanetVO.COLUMN_currencyAmount);
			this.LeaderboardAssetBundle = row.TryGetString(PlanetVO.COLUMN_leaderboardAssetBundle);
			this.LeaderboardTileTexture = row.TryGetString(PlanetVO.COLUMN_leaderboardTileTexture);
			this.LeaderboardButtonTexture = row.TryGetString(PlanetVO.COLUMN_leaderboardButtonTexture);
			this.Abbreviation = row.TryGetString(PlanetVO.COLUMN_abbreviation);
			this.MedalIconName = row.TryGetString(PlanetVO.COLUMN_medalIconName);
			this.PlanetBIName = row.TryGetString(PlanetVO.COLUMN_planetBIName);
			this.RebelMusic = row.TryGetString(PlanetVO.COLUMN_rebelMusic);
			this.EmpireMusic = row.TryGetString(PlanetVO.COLUMN_empireMusic);
			this.BattleMusic = row.TryGetString(PlanetVO.COLUMN_battleMusic);
			this.IntroStoryAction = row.TryGetString(PlanetVO.COLUMN_introStoryAction);
			this.NightDuration = row.TryGetFloat(PlanetVO.COLUMN_nightDuration);
			this.SunriseDuration = row.TryGetFloat(PlanetVO.COLUMN_sunriseDuration);
			this.MidDayDuration = row.TryGetFloat(PlanetVO.COLUMN_midDayDuration);
			this.SunsetDuration = row.TryGetFloat(PlanetVO.COLUMN_sunsetDuration);
			this.CyclesPerDay = row.TryGetFloat(PlanetVO.COLUMN_cyclesPerDay);
			this.SunriseMidnightOffset = row.TryGetFloat(PlanetVO.COLUMN_sunriseMidnightOffset);
			this.TimeOfDayAsset = row.TryGetString(PlanetVO.COLUMN_timeOfDayAssetBundle);
			this.WarBoardLightingAsset = row.TryGetString(PlanetVO.COLUMN_warBoardLightingAssetBundle);
			this.WarBoardAssetName = row.TryGetString(PlanetVO.COLUMN_warBoardAssetName);
			this.WarBoardBundleName = row.TryGetString(PlanetVO.COLUMN_warBoardBundleName);
			float[] array = row.TryGetFloatArray(PlanetVO.COLUMN_perkBuildingHighlightColor);
			if (array != null)
			{
				this.PlanetPerkShaderColor = new Color(array[0], array[1], array[2], array[3]);
			}
			else
			{
				this.PlanetPerkShaderColor = new Color(1f, 1f, 1f, 1f);
			}
			this.PlanetLootUid = row.TryGetString(PlanetVO.COLUMN_planetLootUid);
		}

		public long GetSunriseTimestamp(int timeStamp)
		{
			DateTime date = DateUtils.DateFromSeconds(timeStamp);
			TimeSpan timeSpanSinceStartOfDate = DateUtils.GetTimeSpanSinceStartOfDate(date);
			return (long)DateUtils.GetSecondsFromEpoch(date.Subtract(timeSpanSinceStartOfDate).AddHours((double)this.SunriseMidnightOffset));
		}

		public Vector3 GetGalaxyPositionAsVec3()
		{
			return new Vector3(this.Angle, this.Radius, this.HeightOffset);
		}

		public void SetGalaxyPositionFromVec3(Vector3 pos)
		{
			this.Angle = pos.x;
			this.Radius = pos.y;
			this.HeightOffset = pos.z;
		}

		public PlanetVO()
		{
		}

		protected internal PlanetVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).Abbreviation);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).AmbientMusic);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).Angle);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).BattleMusic);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_abbreviation);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_ambientMusic);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_battleMusic);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_currencyAmount);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_currencyType);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_cyclesPerDay);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_empireMusic);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_footerConflictTexture);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_footerTexture);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_galaxyAssetName);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_galaxyBundleName);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_iconAssetName);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_iconBundleName);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_iconCameraPosition);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_iconCloseupCameraPosition);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_iconCloseupLookatPosition);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_iconLookatPosition);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_introStoryAction);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_leaderboardAssetBundle);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_leaderboardButtonTexture);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_leaderboardTileTexture);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_loaderAssetName);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_loaderBundleName);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_loaderDisplayName);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_medalIconName);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_midDayDuration);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_nightDuration);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_order);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_perkBuildingHighlightColor);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_planetaryFX);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_planetaryLighting);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_planetBIName);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_planetLootUid);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_playerFacing);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_position);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_rebelMusic);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_sunriseDuration);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_sunriseMidnightOffset);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_sunsetDuration);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_timeOfDayAssetBundle);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_warBoardAssetName);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_warBoardBundleName);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetVO.COLUMN_warBoardLightingAssetBundle);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).CurrencyAmount);
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).CurrencyType);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).CyclesPerDay);
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).EmpireMusic);
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).FooterConflictTexture);
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).FooterTexture);
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).GalaxyAssetName);
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).GalaxyBundleName);
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).HeightOffset);
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName);
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName);
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition);
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition);
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition);
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition);
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed);
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IntroStoryAction);
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).LeaderboardAssetBundle);
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).LeaderboardButtonTexture);
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).LeaderboardTileTexture);
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).LoadingScreenAssetName);
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).LoadingScreenBundleName);
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).LoadingScreenText);
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).MedalIconName);
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).MidDayDuration);
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).NightDuration);
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).Order);
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).PlanetaryFX);
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).PlanetaryLighting);
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).PlanetBIName);
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).PlanetLootUid);
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).PlanetPerkShaderColor);
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).PlayerFacing);
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).Population);
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).Radius);
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).RebelMusic);
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).SunriseDuration);
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).SunriseMidnightOffset);
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).SunsetDuration);
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).TimeOfDayAsset);
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).WarBoardAssetName);
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).WarBoardBundleName);
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).WarBoardLightingAsset);
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).GetGalaxyPositionAsVec3());
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).GetSunriseTimestamp(*(int*)args));
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).Abbreviation = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).AmbientMusic = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).Angle = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).BattleMusic = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			PlanetVO.COLUMN_abbreviation = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			PlanetVO.COLUMN_ambientMusic = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			PlanetVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			PlanetVO.COLUMN_battleMusic = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			PlanetVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			PlanetVO.COLUMN_currencyAmount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			PlanetVO.COLUMN_currencyType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			PlanetVO.COLUMN_cyclesPerDay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			PlanetVO.COLUMN_empireMusic = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			PlanetVO.COLUMN_footerConflictTexture = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			PlanetVO.COLUMN_footerTexture = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			PlanetVO.COLUMN_galaxyAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			PlanetVO.COLUMN_galaxyBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			PlanetVO.COLUMN_iconAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			PlanetVO.COLUMN_iconBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			PlanetVO.COLUMN_iconCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			PlanetVO.COLUMN_iconCloseupCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			PlanetVO.COLUMN_iconCloseupLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			PlanetVO.COLUMN_iconLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			PlanetVO.COLUMN_introStoryAction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			PlanetVO.COLUMN_leaderboardAssetBundle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			PlanetVO.COLUMN_leaderboardButtonTexture = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			PlanetVO.COLUMN_leaderboardTileTexture = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			PlanetVO.COLUMN_loaderAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			PlanetVO.COLUMN_loaderBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			PlanetVO.COLUMN_loaderDisplayName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			PlanetVO.COLUMN_medalIconName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			PlanetVO.COLUMN_midDayDuration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			PlanetVO.COLUMN_nightDuration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			PlanetVO.COLUMN_order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			PlanetVO.COLUMN_perkBuildingHighlightColor = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			PlanetVO.COLUMN_planetaryFX = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			PlanetVO.COLUMN_planetaryLighting = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			PlanetVO.COLUMN_planetBIName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			PlanetVO.COLUMN_planetLootUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			PlanetVO.COLUMN_playerFacing = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			PlanetVO.COLUMN_position = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke141(long instance, long* args)
		{
			PlanetVO.COLUMN_rebelMusic = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke142(long instance, long* args)
		{
			PlanetVO.COLUMN_sunriseDuration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke143(long instance, long* args)
		{
			PlanetVO.COLUMN_sunriseMidnightOffset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke144(long instance, long* args)
		{
			PlanetVO.COLUMN_sunsetDuration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke145(long instance, long* args)
		{
			PlanetVO.COLUMN_timeOfDayAssetBundle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke146(long instance, long* args)
		{
			PlanetVO.COLUMN_warBoardAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke147(long instance, long* args)
		{
			PlanetVO.COLUMN_warBoardBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke148(long instance, long* args)
		{
			PlanetVO.COLUMN_warBoardLightingAssetBundle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke149(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).CurrencyAmount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke150(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).CurrencyType = (CurrencyType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke151(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).CyclesPerDay = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke152(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).EmpireMusic = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke153(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).FooterConflictTexture = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke154(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).FooterTexture = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke155(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).GalaxyAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke156(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).GalaxyBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke157(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).HeightOffset = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke158(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke159(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke160(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke161(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke162(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke163(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke164(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke165(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).IntroStoryAction = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke166(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).LeaderboardAssetBundle = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke167(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).LeaderboardButtonTexture = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke168(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).LeaderboardTileTexture = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke169(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).LoadingScreenAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke170(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).LoadingScreenBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke171(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).LoadingScreenText = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke172(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).MedalIconName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke173(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).MidDayDuration = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke174(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).NightDuration = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke175(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).Order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke176(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).PlanetaryFX = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke177(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).PlanetaryLighting = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke178(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).PlanetBIName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke179(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).PlanetLootUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke180(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).PlanetPerkShaderColor = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke181(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).PlayerFacing = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke182(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).Population = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke183(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).Radius = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke184(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).RebelMusic = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke185(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).SunriseDuration = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke186(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).SunriseMidnightOffset = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke187(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).SunsetDuration = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke188(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).TimeOfDayAsset = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke189(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke190(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).WarBoardAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke191(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).WarBoardBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke192(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).WarBoardLightingAsset = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke193(long instance, long* args)
		{
			((PlanetVO)GCHandledObjects.GCHandleToObject(instance)).SetGalaxyPositionFromVec3(*(*(IntPtr*)args));
			return -1L;
		}
	}
}
