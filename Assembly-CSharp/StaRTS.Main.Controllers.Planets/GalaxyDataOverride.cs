using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Planets
{
	public class GalaxyDataOverride : MonoBehaviour, IUnitySerializable
	{
		private PlanetVO tatooinePlanetData;

		private PlanetVO dandoranPlanetData;

		private PlanetVO hothPlanetData;

		private PlanetVO erkitPlanetData;

		private PlanetVO yavinPlanetData;

		public float GalaxyAutoRotateSpeed;

		public float GalaxyPlanetForegroundUIAngle;

		public float GalaxyForegroundAngle;

		public float GalaxyForegroundPlateauAngle;

		public float GalaxyCameraHeightOffset;

		public float GalaxyCameraDistanceOffset;

		public float GalaxyEaseRotationTime;

		public float GalaxyEaseRotationTransitionTime;

		public float GalaxyInitialGalaxyZoomDist;

		public float GalaxyInitialGalaxyZoomTime;

		public float GalaxyPlanetViewHeight;

		public float GalaxyPlanetGalaxyZoomTime;

		public float GalaxyPlanetSwipeMinMove;

		public float GalaxyPlanetSwipeMaxTime;

		public float GalaxyPlanetSwipeTime;

		public string PlanetPositionX;

		public string PlanetPositionY;

		public string PlanetPositionZ;

		public Vector3 TatooinePlanetPos;

		public Vector3 DandoranPlanetPos;

		public Vector3 HothPlanetPos;

		public Vector3 ErkitPlanetPos;

		public Vector3 YavinPlanetPos;

		public GalaxyDataOverride()
		{
			this.PlanetPositionX = "= Rotation Pos About Galaxy";
			this.PlanetPositionY = "= Distance from Center";
			this.PlanetPositionZ = "= Height Above Plane";
			base..ctor();
			this.TatooinePlanetPos = Vector3.zero;
			this.DandoranPlanetPos = Vector3.zero;
			this.HothPlanetPos = Vector3.zero;
			this.ErkitPlanetPos = Vector3.zero;
			this.YavinPlanetPos = Vector3.zero;
			IDataController dataController = Service.Get<IDataController>();
			this.tatooinePlanetData = dataController.Get<PlanetVO>(GameConstants.TATOOINE_PLANET_UID);
			this.dandoranPlanetData = dataController.Get<PlanetVO>(GameConstants.DANDORAN_PLANET_UID);
			this.hothPlanetData = dataController.Get<PlanetVO>(GameConstants.HOTH_PLANET_UID);
			this.erkitPlanetData = dataController.Get<PlanetVO>(GameConstants.ERKIT_PLANET_UID);
			this.yavinPlanetData = dataController.Get<PlanetVO>(GameConstants.YAVIN_PLANET_UID);
		}

		private void Update()
		{
			this.PlanetPositionX = "= Rotation Pos About Galaxy";
			this.PlanetPositionY = "= Distance from Center";
			this.PlanetPositionZ = "= Height Above Plane";
			this.GalaxyAutoRotateSpeed = GameConstants.GALAXY_AUTO_ROTATE_SPEED;
			this.GalaxyForegroundAngle = GameConstants.GALAXY_PLANET_FOREGROUND_THRESHOLD;
			this.GalaxyForegroundPlateauAngle = GameConstants.GALAXY_PLANET_FOREGROUND_PLATEAU_THRESHOLD;
			this.GalaxyPlanetForegroundUIAngle = GameConstants.GALAXY_PLANET_FOREGROUND_UI_THRESHOLD;
			this.GalaxyCameraHeightOffset = GameConstants.GALAXY_CAMERA_HEIGHT_OFFSET;
			this.GalaxyCameraDistanceOffset = GameConstants.GALAXY_CAMERA_DISTANCE_OFFSET;
			this.GalaxyEaseRotationTime = GameConstants.GALAXY_EASE_ROTATION_TIME;
			this.GalaxyEaseRotationTransitionTime = GameConstants.GALAXY_EASE_ROTATION_TRANSITION_TIME;
			this.GalaxyInitialGalaxyZoomDist = GameConstants.GALAXY_INITIAL_GALAXY_ZOOM_DIST;
			this.GalaxyInitialGalaxyZoomTime = GameConstants.GALAXY_INITIAL_GALAXY_ZOOM_TIME;
			this.GalaxyPlanetViewHeight = GameConstants.GALAXY_PLANET_VIEW_HEIGHT;
			this.GalaxyPlanetGalaxyZoomTime = GameConstants.GALAXY_PLANET_GALAXY_ZOOM_TIME;
			this.GalaxyPlanetSwipeMinMove = GameConstants.GALAXY_PLANET_SWIPE_MIN_MOVE;
			this.GalaxyPlanetSwipeMaxTime = GameConstants.GALAXY_PLANET_SWIPE_MAX_TIME;
			this.GalaxyPlanetSwipeTime = GameConstants.GALAXY_PLANET_SWIPE_TIME;
			this.TatooinePlanetPos = this.tatooinePlanetData.GetGalaxyPositionAsVec3();
			this.DandoranPlanetPos = this.dandoranPlanetData.GetGalaxyPositionAsVec3();
			this.HothPlanetPos = this.hothPlanetData.GetGalaxyPositionAsVec3();
			this.ErkitPlanetPos = this.erkitPlanetData.GetGalaxyPositionAsVec3();
			this.YavinPlanetPos = this.yavinPlanetData.GetGalaxyPositionAsVec3();
		}

		private void OnValidate()
		{
			this.tatooinePlanetData.SetGalaxyPositionFromVec3(this.TatooinePlanetPos);
			this.dandoranPlanetData.SetGalaxyPositionFromVec3(this.DandoranPlanetPos);
			this.hothPlanetData.SetGalaxyPositionFromVec3(this.HothPlanetPos);
			this.erkitPlanetData.SetGalaxyPositionFromVec3(this.ErkitPlanetPos);
			this.yavinPlanetData.SetGalaxyPositionFromVec3(this.YavinPlanetPos);
			Service.Get<GalaxyViewController>().UpdateGalaxyConstants();
		}

		public override void Unity_Serialize(int depth)
		{
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyAutoRotateSpeed);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyPlanetForegroundUIAngle);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyForegroundAngle);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyForegroundPlateauAngle);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyCameraHeightOffset);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyCameraDistanceOffset);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyEaseRotationTime);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyEaseRotationTransitionTime);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyInitialGalaxyZoomDist);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyInitialGalaxyZoomTime);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyPlanetViewHeight);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyPlanetGalaxyZoomTime);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyPlanetSwipeMinMove);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyPlanetSwipeMaxTime);
			SerializedStateWriter.Instance.WriteSingle(this.GalaxyPlanetSwipeTime);
			SerializedStateWriter.Instance.WriteString(this.PlanetPositionX);
			SerializedStateWriter.Instance.WriteString(this.PlanetPositionY);
			SerializedStateWriter.Instance.WriteString(this.PlanetPositionZ);
			if (depth <= 7)
			{
				this.TatooinePlanetPos.Unity_Serialize(depth + 1);
			}
			SerializedStateWriter.Instance.Align();
			if (depth <= 7)
			{
				this.DandoranPlanetPos.Unity_Serialize(depth + 1);
			}
			SerializedStateWriter.Instance.Align();
			if (depth <= 7)
			{
				this.HothPlanetPos.Unity_Serialize(depth + 1);
			}
			SerializedStateWriter.Instance.Align();
			if (depth <= 7)
			{
				this.ErkitPlanetPos.Unity_Serialize(depth + 1);
			}
			SerializedStateWriter.Instance.Align();
			if (depth <= 7)
			{
				this.YavinPlanetPos.Unity_Serialize(depth + 1);
			}
			SerializedStateWriter.Instance.Align();
		}

		public override void Unity_Deserialize(int depth)
		{
			this.GalaxyAutoRotateSpeed = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyPlanetForegroundUIAngle = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyForegroundAngle = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyForegroundPlateauAngle = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyCameraHeightOffset = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyCameraDistanceOffset = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyEaseRotationTime = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyEaseRotationTransitionTime = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyInitialGalaxyZoomDist = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyInitialGalaxyZoomTime = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyPlanetViewHeight = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyPlanetGalaxyZoomTime = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyPlanetSwipeMinMove = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyPlanetSwipeMaxTime = SerializedStateReader.Instance.ReadSingle();
			this.GalaxyPlanetSwipeTime = SerializedStateReader.Instance.ReadSingle();
			this.PlanetPositionX = (SerializedStateReader.Instance.ReadString() as string);
			this.PlanetPositionY = (SerializedStateReader.Instance.ReadString() as string);
			this.PlanetPositionZ = (SerializedStateReader.Instance.ReadString() as string);
			if (depth <= 7)
			{
				this.TatooinePlanetPos.Unity_Deserialize(depth + 1);
			}
			SerializedStateReader.Instance.Align();
			if (depth <= 7)
			{
				this.DandoranPlanetPos.Unity_Deserialize(depth + 1);
			}
			SerializedStateReader.Instance.Align();
			if (depth <= 7)
			{
				this.HothPlanetPos.Unity_Deserialize(depth + 1);
			}
			SerializedStateReader.Instance.Align();
			if (depth <= 7)
			{
				this.ErkitPlanetPos.Unity_Deserialize(depth + 1);
			}
			SerializedStateReader.Instance.Align();
			if (depth <= 7)
			{
				this.YavinPlanetPos.Unity_Deserialize(depth + 1);
			}
			SerializedStateReader.Instance.Align();
		}

		public override void Unity_RemapPPtrs(int depth)
		{
		}

		public unsafe override void Unity_NamedSerialize(int depth)
		{
			ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
			float arg_1F_1 = this.GalaxyAutoRotateSpeed;
			byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			int var_0_cp_1 = 0;
			arg_1F_0.WriteSingle(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 5163);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyPlanetForegroundUIAngle, &var_0_cp_0[var_0_cp_1] + 5185);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyForegroundAngle, &var_0_cp_0[var_0_cp_1] + 5215);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyForegroundPlateauAngle, &var_0_cp_0[var_0_cp_1] + 5237);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyCameraHeightOffset, &var_0_cp_0[var_0_cp_1] + 5266);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyCameraDistanceOffset, &var_0_cp_0[var_0_cp_1] + 5291);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyEaseRotationTime, &var_0_cp_0[var_0_cp_1] + 5318);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyEaseRotationTransitionTime, &var_0_cp_0[var_0_cp_1] + 5341);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyInitialGalaxyZoomDist, &var_0_cp_0[var_0_cp_1] + 5374);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyInitialGalaxyZoomTime, &var_0_cp_0[var_0_cp_1] + 5402);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyPlanetViewHeight, &var_0_cp_0[var_0_cp_1] + 5430);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyPlanetGalaxyZoomTime, &var_0_cp_0[var_0_cp_1] + 5453);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyPlanetSwipeMinMove, &var_0_cp_0[var_0_cp_1] + 5480);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyPlanetSwipeMaxTime, &var_0_cp_0[var_0_cp_1] + 5505);
			SerializedNamedStateWriter.Instance.WriteSingle(this.GalaxyPlanetSwipeTime, &var_0_cp_0[var_0_cp_1] + 5530);
			SerializedNamedStateWriter.Instance.WriteString(this.PlanetPositionX, &var_0_cp_0[var_0_cp_1] + 5552);
			SerializedNamedStateWriter.Instance.WriteString(this.PlanetPositionY, &var_0_cp_0[var_0_cp_1] + 5568);
			SerializedNamedStateWriter.Instance.WriteString(this.PlanetPositionZ, &var_0_cp_0[var_0_cp_1] + 5584);
			if (depth <= 7)
			{
				SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 5600);
				this.TatooinePlanetPos.Unity_NamedSerialize(depth + 1);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			SerializedNamedStateWriter.Instance.Align();
			if (depth <= 7)
			{
				SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 5618);
				this.DandoranPlanetPos.Unity_NamedSerialize(depth + 1);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			SerializedNamedStateWriter.Instance.Align();
			if (depth <= 7)
			{
				SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 5636);
				this.HothPlanetPos.Unity_NamedSerialize(depth + 1);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			SerializedNamedStateWriter.Instance.Align();
			if (depth <= 7)
			{
				SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 5650);
				this.ErkitPlanetPos.Unity_NamedSerialize(depth + 1);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			SerializedNamedStateWriter.Instance.Align();
			if (depth <= 7)
			{
				SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 5665);
				this.YavinPlanetPos.Unity_NamedSerialize(depth + 1);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			SerializedNamedStateWriter.Instance.Align();
		}

		public unsafe override void Unity_NamedDeserialize(int depth)
		{
			ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
			byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			int var_0_cp_1 = 0;
			this.GalaxyAutoRotateSpeed = arg_1A_0.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5163);
			this.GalaxyPlanetForegroundUIAngle = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5185);
			this.GalaxyForegroundAngle = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5215);
			this.GalaxyForegroundPlateauAngle = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5237);
			this.GalaxyCameraHeightOffset = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5266);
			this.GalaxyCameraDistanceOffset = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5291);
			this.GalaxyEaseRotationTime = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5318);
			this.GalaxyEaseRotationTransitionTime = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5341);
			this.GalaxyInitialGalaxyZoomDist = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5374);
			this.GalaxyInitialGalaxyZoomTime = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5402);
			this.GalaxyPlanetViewHeight = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5430);
			this.GalaxyPlanetGalaxyZoomTime = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5453);
			this.GalaxyPlanetSwipeMinMove = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5480);
			this.GalaxyPlanetSwipeMaxTime = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5505);
			this.GalaxyPlanetSwipeTime = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5530);
			this.PlanetPositionX = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 5552) as string);
			this.PlanetPositionY = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 5568) as string);
			this.PlanetPositionZ = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 5584) as string);
			if (depth <= 7)
			{
				SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 5600);
				this.TatooinePlanetPos.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
			}
			SerializedNamedStateReader.Instance.Align();
			if (depth <= 7)
			{
				SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 5618);
				this.DandoranPlanetPos.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
			}
			SerializedNamedStateReader.Instance.Align();
			if (depth <= 7)
			{
				SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 5636);
				this.HothPlanetPos.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
			}
			SerializedNamedStateReader.Instance.Align();
			if (depth <= 7)
			{
				SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 5650);
				this.ErkitPlanetPos.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
			}
			SerializedNamedStateReader.Instance.Align();
			if (depth <= 7)
			{
				SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 5665);
				this.YavinPlanetPos.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
			}
			SerializedNamedStateReader.Instance.Align();
		}

		protected internal GalaxyDataOverride(UIntPtr dummy) : base(dummy)
		{
		}

		public static float $Get0(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyAutoRotateSpeed;
		}

		public static void $Set0(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyAutoRotateSpeed = value;
		}

		public static float $Get1(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyPlanetForegroundUIAngle;
		}

		public static void $Set1(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyPlanetForegroundUIAngle = value;
		}

		public static float $Get2(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyForegroundAngle;
		}

		public static void $Set2(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyForegroundAngle = value;
		}

		public static float $Get3(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyForegroundPlateauAngle;
		}

		public static void $Set3(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyForegroundPlateauAngle = value;
		}

		public static float $Get4(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyCameraHeightOffset;
		}

		public static void $Set4(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyCameraHeightOffset = value;
		}

		public static float $Get5(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyCameraDistanceOffset;
		}

		public static void $Set5(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyCameraDistanceOffset = value;
		}

		public static float $Get6(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyEaseRotationTime;
		}

		public static void $Set6(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyEaseRotationTime = value;
		}

		public static float $Get7(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyEaseRotationTransitionTime;
		}

		public static void $Set7(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyEaseRotationTransitionTime = value;
		}

		public static float $Get8(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyInitialGalaxyZoomDist;
		}

		public static void $Set8(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyInitialGalaxyZoomDist = value;
		}

		public static float $Get9(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyInitialGalaxyZoomTime;
		}

		public static void $Set9(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyInitialGalaxyZoomTime = value;
		}

		public static float $Get10(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyPlanetViewHeight;
		}

		public static void $Set10(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyPlanetViewHeight = value;
		}

		public static float $Get11(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyPlanetGalaxyZoomTime;
		}

		public static void $Set11(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyPlanetGalaxyZoomTime = value;
		}

		public static float $Get12(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyPlanetSwipeMinMove;
		}

		public static void $Set12(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyPlanetSwipeMinMove = value;
		}

		public static float $Get13(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyPlanetSwipeMaxTime;
		}

		public static void $Set13(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyPlanetSwipeMaxTime = value;
		}

		public static float $Get14(object instance)
		{
			return ((GalaxyDataOverride)instance).GalaxyPlanetSwipeTime;
		}

		public static void $Set14(object instance, float value)
		{
			((GalaxyDataOverride)instance).GalaxyPlanetSwipeTime = value;
		}

		public static float $Get15(object instance, int index)
		{
			GalaxyDataOverride expr_06_cp_0 = (GalaxyDataOverride)instance;
			switch (index)
			{
			case 0:
				return expr_06_cp_0.TatooinePlanetPos.x;
			case 1:
				return expr_06_cp_0.TatooinePlanetPos.y;
			case 2:
				return expr_06_cp_0.TatooinePlanetPos.z;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public static void $Set15(object instance, float value, int index)
		{
			GalaxyDataOverride expr_06_cp_0 = (GalaxyDataOverride)instance;
			switch (index)
			{
			case 0:
				expr_06_cp_0.TatooinePlanetPos.x = value;
				return;
			case 1:
				expr_06_cp_0.TatooinePlanetPos.y = value;
				return;
			case 2:
				expr_06_cp_0.TatooinePlanetPos.z = value;
				return;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public static float $Get16(object instance, int index)
		{
			GalaxyDataOverride expr_06_cp_0 = (GalaxyDataOverride)instance;
			switch (index)
			{
			case 0:
				return expr_06_cp_0.DandoranPlanetPos.x;
			case 1:
				return expr_06_cp_0.DandoranPlanetPos.y;
			case 2:
				return expr_06_cp_0.DandoranPlanetPos.z;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public static void $Set16(object instance, float value, int index)
		{
			GalaxyDataOverride expr_06_cp_0 = (GalaxyDataOverride)instance;
			switch (index)
			{
			case 0:
				expr_06_cp_0.DandoranPlanetPos.x = value;
				return;
			case 1:
				expr_06_cp_0.DandoranPlanetPos.y = value;
				return;
			case 2:
				expr_06_cp_0.DandoranPlanetPos.z = value;
				return;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public static float $Get17(object instance, int index)
		{
			GalaxyDataOverride expr_06_cp_0 = (GalaxyDataOverride)instance;
			switch (index)
			{
			case 0:
				return expr_06_cp_0.HothPlanetPos.x;
			case 1:
				return expr_06_cp_0.HothPlanetPos.y;
			case 2:
				return expr_06_cp_0.HothPlanetPos.z;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public static void $Set17(object instance, float value, int index)
		{
			GalaxyDataOverride expr_06_cp_0 = (GalaxyDataOverride)instance;
			switch (index)
			{
			case 0:
				expr_06_cp_0.HothPlanetPos.x = value;
				return;
			case 1:
				expr_06_cp_0.HothPlanetPos.y = value;
				return;
			case 2:
				expr_06_cp_0.HothPlanetPos.z = value;
				return;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public static float $Get18(object instance, int index)
		{
			GalaxyDataOverride expr_06_cp_0 = (GalaxyDataOverride)instance;
			switch (index)
			{
			case 0:
				return expr_06_cp_0.ErkitPlanetPos.x;
			case 1:
				return expr_06_cp_0.ErkitPlanetPos.y;
			case 2:
				return expr_06_cp_0.ErkitPlanetPos.z;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public static void $Set18(object instance, float value, int index)
		{
			GalaxyDataOverride expr_06_cp_0 = (GalaxyDataOverride)instance;
			switch (index)
			{
			case 0:
				expr_06_cp_0.ErkitPlanetPos.x = value;
				return;
			case 1:
				expr_06_cp_0.ErkitPlanetPos.y = value;
				return;
			case 2:
				expr_06_cp_0.ErkitPlanetPos.z = value;
				return;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public static float $Get19(object instance, int index)
		{
			GalaxyDataOverride expr_06_cp_0 = (GalaxyDataOverride)instance;
			switch (index)
			{
			case 0:
				return expr_06_cp_0.YavinPlanetPos.x;
			case 1:
				return expr_06_cp_0.YavinPlanetPos.y;
			case 2:
				return expr_06_cp_0.YavinPlanetPos.z;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public static void $Set19(object instance, float value, int index)
		{
			GalaxyDataOverride expr_06_cp_0 = (GalaxyDataOverride)instance;
			switch (index)
			{
			case 0:
				expr_06_cp_0.YavinPlanetPos.x = value;
				return;
			case 1:
				expr_06_cp_0.YavinPlanetPos.y = value;
				return;
			case 2:
				expr_06_cp_0.YavinPlanetPos.z = value;
				return;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((GalaxyDataOverride)GCHandledObjects.GCHandleToObject(instance)).OnValidate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GalaxyDataOverride)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GalaxyDataOverride)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((GalaxyDataOverride)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((GalaxyDataOverride)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((GalaxyDataOverride)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((GalaxyDataOverride)GCHandledObjects.GCHandleToObject(instance)).Update();
			return -1L;
		}
	}
}
