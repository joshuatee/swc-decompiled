using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.FX
{
	public class AbstractLightingEffects
	{
		protected const string PL_COLOR_BUILDINGS_LIGHT = "_PL_Buildings_Light";

		protected const string PL_COLOR_BUILDINGS_DARK = "_PL_Buildings_Dark";

		protected const string PL_COLOR_TERRAIN_LIGHT = "_PL_Terrain_Light";

		protected const string PL_COLOR_TERRAIN_DARK = "_PL_Terrain_Dark";

		protected const string PL_COLOR_SHADOW = "_PL_Shadow";

		protected const string PL_COLOR_GRID_WALL = "_PL_Grid_Wall";

		protected const string PL_COLOR_GRID = "_PL_Grid";

		protected const string PL_COLOR_GRID_BUILDINGS = "_PL_Grid_Buildings";

		protected const string PL_COLOR_UNITS = "_PL_Units";

		protected const string PLANETARY_OUTER_SHADOW_MATERIAL = "_outerShadow";

		protected PlanetVO planetVO;

		protected string planetLightingUid;

		protected Color defaultColor;

		public Color PLColorBuildingLight
		{
			get;
			set;
		}

		public Color PLColorBuildingDark
		{
			get;
			set;
		}

		public Color PLColorTerrainLight
		{
			get;
			set;
		}

		public Color PLColorTerrainDark
		{
			get;
			set;
		}

		public Color PLColorShadow
		{
			get;
			set;
		}

		public Color PLColorUnits
		{
			get;
			set;
		}

		public Color PLColorWall
		{
			get;
			set;
		}

		public Color PLColorGrid
		{
			get;
			set;
		}

		public Color PLColorGridBuildings
		{
			get;
			set;
		}

		public AbstractLightingEffects()
		{
			this.defaultColor = new Color(0.5f, 0.5f, 0.5f, 1f);
			base..ctor();
			this.SetDefaultColors();
			this.RefreshShaderColors();
		}

		public virtual void SetDefaultColors()
		{
		}

		public virtual void RefreshShaderColors()
		{
		}

		public virtual Color GetCurrentLightingColor(LightingColorType type)
		{
			return this.defaultColor;
		}

		public virtual void UpdateEnvironmentLighting(float dt)
		{
		}

		public virtual void ApplyDelayedLightingDataOverride(EventId triggerEvent, string dataAssetName)
		{
		}

		public virtual void RemoveLightingDataOverride()
		{
		}

		public virtual void SetupDelayedLightingOverrideRemoval(EventId triggerEvent)
		{
		}

		protected internal AbstractLightingEffects(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).ApplyDelayedLightingDataOverride((EventId)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorBuildingDark);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorBuildingLight);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorGrid);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorGridBuildings);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorShadow);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorTerrainDark);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorTerrainLight);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorUnits);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorWall);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).GetCurrentLightingColor((LightingColorType)(*(int*)args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).RefreshShaderColors();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).RemoveLightingDataOverride();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorBuildingDark = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorBuildingLight = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorGrid = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorGridBuildings = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorShadow = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorTerrainDark = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorTerrainLight = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorUnits = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).PLColorWall = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).SetDefaultColors();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).SetupDelayedLightingOverrideRemoval((EventId)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((AbstractLightingEffects)GCHandledObjects.GCHandleToObject(instance)).UpdateEnvironmentLighting(*(float*)args);
			return -1L;
		}
	}
}
