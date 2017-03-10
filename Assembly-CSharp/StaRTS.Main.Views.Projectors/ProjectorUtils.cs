using StaRTS.Main.Configs;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Projectors
{
	public class ProjectorUtils
	{
		public static GeometryProjector GenerateProjector(ProjectorConfig config)
		{
			GeometryProjector geometryProjector = new GeometryProjector(config);
			if (config.FrameSprite != null)
			{
				geometryProjector.Renderer = new SpriteProjectorRenderer(config.FrameSprite, geometryProjector.ProjectorIndex);
			}
			else
			{
				geometryProjector.Renderer = new SurfaceProjectorRenderer(geometryProjector.ProjectorIndex);
			}
			if (config.Metered)
			{
				geometryProjector.Renderer = new ProjectorMeterDecorator(geometryProjector.Renderer);
			}
			geometryProjector.Renderer = new ProjectorOutlineDecorator(geometryProjector.Renderer);
			if (!string.IsNullOrEmpty(config.TrackerName))
			{
				geometryProjector.Renderer = new ProjectorTurretDecorator(geometryProjector.Renderer);
			}
			if (!string.IsNullOrEmpty(config.buildingEquipmentShaderName) && ProjectorUtils.CanBeAnimated(config))
			{
				geometryProjector.Renderer = new ProjectorEquipmentBuildingDecorator(geometryProjector.Renderer);
			}
			else
			{
				geometryProjector.Renderer = new ProjectorShaderSwapDecorator(geometryProjector.Renderer);
			}
			geometryProjector.Renderer = new ProjectorAnimationDecorator(geometryProjector.Renderer);
			bool flag = config.AnimPreference == AnimationPreference.NoAnimation;
			bool flag2 = config.AnimPreference == AnimationPreference.AnimationPreferred && HardwareProfile.IsLowEndDevice();
			if (!flag && !flag2 && config.IconRotationSpeed > 0f)
			{
				geometryProjector.Renderer = new ProjectorRotationDecorator(geometryProjector.Renderer);
			}
			geometryProjector.AssetProcessor.LoadAllAssets(new Action<GeometryProjector>(ProjectorUtils.OnDefaultAssetSuccess), new Action<GeometryProjector>(ProjectorUtils.OnDefaultAssetFailure));
			return geometryProjector;
		}

		public static void ResetProjectorMembers(GeometryProjector projector)
		{
			projector.AssetProcessor.Reset();
			projector.Renderer.Render(projector.Config);
		}

		private static bool CanBeAnimated(ProjectorConfig config)
		{
			return config.AnimPreference == AnimationPreference.AnimationAlways || (config.AnimPreference == AnimationPreference.AnimationPreferred && !HardwareProfile.IsLowEndDevice());
		}

		public static void OnDefaultAssetSuccess(GeometryProjector projector)
		{
			ProjectorConfig config = projector.Config;
			projector.Renderer.Render(config);
			projector.Renderer.PostRender(config);
		}

		public static void OnDefaultAssetFailure(GeometryProjector projector)
		{
			projector.Destroy();
		}

		public static ProjectorConfig GenerateBuildingConfig(BuildingTypeVO vo, UXSprite frameSprite)
		{
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(vo, frameSprite, false);
			BuildingType type = vo.Type;
			if (type <= BuildingType.Turret)
			{
				if (type == BuildingType.Barracks)
				{
					projectorConfig.AnimationName = "IdleClosed";
					goto IL_5A;
				}
				if (type != BuildingType.Turret)
				{
					goto IL_4F;
				}
			}
			else if (type != BuildingType.Storage)
			{
				if (type != BuildingType.Armory)
				{
					goto IL_4F;
				}
				projectorConfig.AnimationName = "Idle";
				goto IL_5A;
			}
			projectorConfig.AnimationName = null;
			goto IL_5A;
			IL_4F:
			projectorConfig.AnimationName = "Active";
			IL_5A:
			type = vo.Type;
			if (type <= BuildingType.Starport)
			{
				if (type != BuildingType.ChampionPlatform)
				{
					if (type == BuildingType.Starport)
					{
						projectorConfig.Metered = true;
						projectorConfig.MeterValue = 1f;
					}
				}
				else
				{
					projectorConfig.AttachmentAssets = new string[]
					{
						vo.AssetName
					};
				}
			}
			else if (type != BuildingType.Turret)
			{
				if (type == BuildingType.Trap)
				{
					projectorConfig.SnapshotFrameDelay = 4;
				}
			}
			else if (!string.IsNullOrEmpty(vo.TurretUid))
			{
				TurretTypeVO turretTypeVO = Service.Get<IDataController>().Get<TurretTypeVO>(vo.TurretUid);
				projectorConfig.TrackerName = turretTypeVO.TrackerName;
			}
			return projectorConfig;
		}

		public static ProjectorConfig GenerateGeometryConfig(IGeometryVO vo, Action<RenderTexture, ProjectorConfig> callback, float width, float height)
		{
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(vo, null, false);
			projectorConfig.RenderCallback = callback;
			projectorConfig.RenderWidth = width;
			projectorConfig.RenderHeight = height;
			return projectorConfig;
		}

		public static ProjectorConfig GenerateGeometryConfig(IGeometryVO vo, UXSprite frameSprite)
		{
			return ProjectorUtils.GenerateGeometryConfig(vo, frameSprite, false);
		}

		public static ProjectorConfig GenerateGeometryConfig(IGeometryVO vo, UXSprite frameSprite, bool closeup)
		{
			ProjectorConfig projectorConfig = new ProjectorConfig();
			projectorConfig.IconRotationSpeed = vo.IconRotationSpeed;
			projectorConfig.AnimPreference = AnimationPreference.NoAnimation;
			projectorConfig.AssetName = vo.IconAssetName;
			projectorConfig.closeup = closeup;
			if (projectorConfig.closeup)
			{
				projectorConfig.CameraPosition = vo.IconCloseupCameraPosition;
				projectorConfig.CameraInterest = vo.IconCloseupLookatPosition;
			}
			else
			{
				projectorConfig.CameraPosition = vo.IconCameraPosition;
				projectorConfig.CameraInterest = vo.IconLookatPosition;
			}
			projectorConfig.AnimState = AnimState.Idle;
			projectorConfig.FrameSprite = frameSprite;
			return projectorConfig;
		}

		public static ProjectorConfig GenerateEquipmentConfig(EquipmentVO equipmentVO, Action<RenderTexture, ProjectorConfig> callback, float width, float height)
		{
			IGeometryVO vo = ProjectorUtils.DetermineVOForEquipment(equipmentVO);
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(vo, callback, width, height);
			projectorConfig.buildingEquipmentShaderName = ProjectorUtils.GetEquipmentBuildingShaderName(equipmentVO);
			return projectorConfig;
		}

		public static ProjectorConfig GenerateEquipmentConfig(EquipmentVO equipmentVO, UXSprite frameSprite, bool closeup)
		{
			IGeometryVO vo = ProjectorUtils.DetermineVOForEquipment(equipmentVO);
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(vo, frameSprite, closeup);
			projectorConfig.buildingEquipmentShaderName = ProjectorUtils.GetEquipmentBuildingShaderName(equipmentVO);
			return projectorConfig;
		}

		public static IGeometryVO DetermineVOForEquipment(EquipmentVO equipmentVO)
		{
			if (equipmentVO != null && !string.IsNullOrEmpty(equipmentVO.BuildingID))
			{
				BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
				UnlockController unlockController = Service.Get<UnlockController>();
				BuildingTypeVO buildingTypeVO = buildingUpgradeCatalog.GetMinLevel(equipmentVO.BuildingID);
				int lvl = buildingUpgradeCatalog.GetMaxLevel(equipmentVO.BuildingID).Lvl;
				for (int i = buildingTypeVO.Lvl + 1; i < lvl; i++)
				{
					BuildingTypeVO byLevel = buildingUpgradeCatalog.GetByLevel(equipmentVO.BuildingID, i);
					BuildingTypeVO buildingTypeVO2;
					if (!unlockController.IsUnlocked(byLevel, 1, out buildingTypeVO2))
					{
						break;
					}
					buildingTypeVO = byLevel;
				}
				return buildingTypeVO;
			}
			return equipmentVO;
		}

		private static string GetEquipmentBuildingShaderName(EquipmentVO equipmentVO)
		{
			IDataController dataController = Service.Get<IDataController>();
			string text = string.Empty;
			if (!string.IsNullOrEmpty(equipmentVO.BuildingID))
			{
				string uid = equipmentVO.EffectUids[0];
				EquipmentEffectVO equipmentEffectVO = dataController.Get<EquipmentEffectVO>(uid);
				string uid2 = equipmentEffectVO.BuffUids[0];
				BuffTypeVO buffTypeVO = dataController.Get<BuffTypeVO>(uid2);
				text = buffTypeVO.ShaderName;
				if (text.StartsWith("PL_2"))
				{
					text = text.Replace("PL_2", "");
				}
			}
			return text;
		}

		public ProjectorUtils()
		{
		}

		protected internal ProjectorUtils(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectorUtils.CanBeAnimated((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectorUtils.DetermineVOForEquipment((EquipmentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectorUtils.GenerateBuildingConfig((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (UXSprite)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectorUtils.GenerateEquipmentConfig((EquipmentVO)GCHandledObjects.GCHandleToObject(*args), (UXSprite)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectorUtils.GenerateEquipmentConfig((EquipmentVO)GCHandledObjects.GCHandleToObject(*args), (Action<RenderTexture, ProjectorConfig>)GCHandledObjects.GCHandleToObject(args[1]), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectorUtils.GenerateGeometryConfig((IGeometryVO)GCHandledObjects.GCHandleToObject(*args), (UXSprite)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectorUtils.GenerateGeometryConfig((IGeometryVO)GCHandledObjects.GCHandleToObject(*args), (UXSprite)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectorUtils.GenerateGeometryConfig((IGeometryVO)GCHandledObjects.GCHandleToObject(*args), (Action<RenderTexture, ProjectorConfig>)GCHandledObjects.GCHandleToObject(args[1]), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectorUtils.GenerateProjector((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectorUtils.GetEquipmentBuildingShaderName((EquipmentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			ProjectorUtils.OnDefaultAssetFailure((GeometryProjector)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			ProjectorUtils.OnDefaultAssetSuccess((GeometryProjector)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			ProjectorUtils.ResetProjectorMembers((GeometryProjector)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
