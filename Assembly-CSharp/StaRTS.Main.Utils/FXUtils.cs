using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard.Components;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public static class FXUtils
	{
		public static readonly Color SELECTION_OUTLINE_COLOR = new Color(0.482f, 0.831f, 0.996f, 1f);

		public const float SELECTION_OUTLINE_WIDTH = 0.00125f;

		public const string ATTACHMENT_RUBBLE = "rubble";

		public const string ATTACHMENT_FILL_STATE = "fillState";

		private const string FX_DESTRUCT_DEFAULT_UID = "fx_debris_{0}x{1}";

		private const string FX_VEHICLE_DESTRUCTION_UID = "fx_vehdebris_{0}x{1}";

		private const string FX_WALL_DESTRUCTION_UID = "effect176";

		private const string FX_RUBBLE_MODEL_UID = "rebelRubble{0}";

		private const string FX_WALL_RUBBLE_MODEL_UID = "rebelRubbleWall1";

		private const string FX_SHIELD_DESTRUCTION_UID = "fx_debris_6x6";

		private const int FX_MAX_SIZE = 6;

		private const string KEY_DELIMITER = "|";

		public static string MakeAssetKey(string attachmentKey, Entity entity)
		{
			return attachmentKey + "|" + entity.ID;
		}

		public static string MakeAssetKey(string assetName, Vector3 worldPos)
		{
			return assetName + "|" + worldPos.ToString();
		}

		public static string GetRubbleAssetNameForEntity(SmartEntity entity)
		{
			SizeComponent sizeComp = entity.SizeComp;
			int num = Units.BoardToGridX(sizeComp.Width);
			int num2 = Units.BoardToGridZ(sizeComp.Depth);
			string uid;
			if (entity.BuildingComp.BuildingType.Type == BuildingType.Wall)
			{
				uid = string.Format("rebelRubbleWall1", new object[]
				{
					num,
					num2
				});
			}
			else
			{
				uid = string.Format("rebelRubble{0}", new object[]
				{
					num,
					num2
				});
			}
			BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(uid);
			return buildingTypeVO.AssetName;
		}

		public static string GetDebrisAssetNameForEntity(SmartEntity entity, bool isBuilding)
		{
			SizeComponent sizeComp = entity.SizeComp;
			int val = Units.BoardToGridX(sizeComp.Width);
			int val2 = Units.BoardToGridZ(sizeComp.Depth);
			int num = Math.Min(val, val2);
			string uid;
			if (isBuilding)
			{
				BuildingTypeVO buildingType = entity.BuildingComp.BuildingType;
				string text = string.IsNullOrEmpty(buildingType.DestructFX) ? "fx_debris_{0}x{1}" : buildingType.DestructFX;
				BuildingType type = buildingType.Type;
				if (type != BuildingType.Wall)
				{
					if (type != BuildingType.Turret)
					{
						if (type != BuildingType.ShieldGenerator)
						{
							uid = string.Format(text, new object[]
							{
								num,
								num
							});
						}
						else
						{
							uid = "fx_debris_6x6";
						}
					}
					else
					{
						uid = ((buildingType.Faction == FactionType.Tusken) ? "effect176" : string.Format(text, new object[]
						{
							num,
							num
						}));
					}
				}
				else
				{
					uid = "effect176";
				}
			}
			else
			{
				uid = string.Format("fx_vehdebris_{0}x{1}", new object[]
				{
					num,
					num
				});
			}
			EffectsTypeVO effectsTypeVO = Service.Get<IDataController>().Get<EffectsTypeVO>(uid);
			return effectsTypeVO.AssetName;
		}

		public static List<IAssetVO> GetEffectAssetTypes()
		{
			List<IAssetVO> list = new List<IAssetVO>();
			list.Add(FXUtils.GetAssetType<BuildingTypeVO>("rebelRubbleWall1"));
			list.Add(FXUtils.GetAssetType<EffectsTypeVO>("effect176"));
			for (int i = 1; i < 6; i++)
			{
				string uid = string.Format("fx_debris_{0}x{1}", new object[]
				{
					i,
					i
				});
				list.Add(FXUtils.GetAssetType<EffectsTypeVO>(uid));
				string uid2 = string.Format("rebelRubble{0}", new object[]
				{
					i
				});
				list.Add(FXUtils.GetAssetType<BuildingTypeVO>(uid2));
			}
			return list;
		}

		private static IAssetVO GetAssetType<T>(string uid) where T : IValueObject
		{
			return Service.Get<IDataController>().Get<T>(uid) as IAssetVO;
		}

		public static Color ConvertHexStringToColorObject(string hexColor)
		{
			Color grey = Color.grey;
			grey.a = 1f;
			if (!string.IsNullOrEmpty(hexColor) && hexColor.get_Length() == 6)
			{
				grey.r = (float)int.Parse(hexColor.Substring(0, 2), 512) / 255f;
				grey.g = (float)int.Parse(hexColor.Substring(2, 2), 512) / 255f;
				grey.b = (float)int.Parse(hexColor.Substring(4, 2), 512) / 255f;
			}
			else
			{
				Service.Get<StaRTSLogger>().Warn("FXUtils: Invalid hex color: " + hexColor);
			}
			return grey;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FXUtils.ConvertHexStringToColorObject(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FXUtils.GetDebrisAssetNameForEntity((SmartEntity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FXUtils.GetEffectAssetTypes());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FXUtils.GetRubbleAssetNameForEntity((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FXUtils.MakeAssetKey(Marshal.PtrToStringUni(*(IntPtr*)args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FXUtils.MakeAssetKey(Marshal.PtrToStringUni(*(IntPtr*)args), *(*(IntPtr*)(args + 1))));
		}
	}
}
