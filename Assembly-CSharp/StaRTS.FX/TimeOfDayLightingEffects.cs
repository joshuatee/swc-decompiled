using StaRTS.Assets;
using StaRTS.Externals.EnvironmentManager;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Battle.Replay;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.FX
{
	public class TimeOfDayLightingEffects : AbstractLightingEffects, IEventObserver
	{
		private const float DAY_LENGTH_IN_HOURS = 24f;

		private const float SECONDS_PER_HOUR = 3600f;

		private const float MIN_UPDATE_DELTA = 1E-07f;

		private const TimeOfDaySegments MID_DAY_SEGMENT = TimeOfDaySegments.Day;

		private double segmentPercentage;

		private TimeOfDaySegments timeOfDaySegment;

		private long lastLocalTimeSinceSunrise;

		private double localTimeDelta;

		private int cycleLengthSeconds;

		private bool pauseTimeOfDayUpdate;

		private bool planetColorGradientDataLoaded;

		private AssetHandle planetColorHandle;

		private PlanetaryLightingMonoBehaviour lightingData;

		private EventId applyLightingOverrideEvent;

		private EventId removeLightingOverrideEvent;

		private string overrideLightingDataAssetName;

		private readonly List<EventId> lightingEvents;

		public TimeOfDayLightingEffects()
		{
			this.lightingEvents = new List<EventId>
			{
				EventId.GameStateChanged,
				EventId.WorldOutTransitionComplete
			};
			base..ctor();
			this.applyLightingOverrideEvent = EventId.Nop;
			this.removeLightingOverrideEvent = EventId.Nop;
			this.pauseTimeOfDayUpdate = false;
			EventManager eventManager = Service.Get<EventManager>();
			int count = this.lightingEvents.Count;
			for (int i = 0; i < count; i++)
			{
				eventManager.RegisterObserver(this, this.lightingEvents[i], EventPriority.BeforeDefault);
			}
		}

		private bool IsTimePausedInDayPhase()
		{
			return this.pauseTimeOfDayUpdate || !GameConstants.TIME_OF_DAY_ENABLED;
		}

		public override void SetDefaultColors()
		{
			base.SetDefaultColors();
			string planetId = Service.Get<CurrentPlayer>().PlanetId;
			this.planetVO = Service.Get<IDataController>().Get<PlanetVO>(planetId);
			this.InitPlanetLightingData(DateUtils.GetNowSeconds());
		}

		private void LoadPlanetLightingData(string assetName)
		{
			AssetManager assetManager = Service.Get<AssetManager>();
			if (this.planetColorHandle != AssetHandle.Invalid)
			{
				assetManager.Unload(this.planetColorHandle);
				this.planetColorHandle = AssetHandle.Invalid;
				if (this.lightingData != null && this.lightingData.gameObject != null)
				{
					UnityEngine.Object.DestroyImmediate(this.lightingData.gameObject);
				}
				this.lightingData = null;
			}
			this.planetColorGradientDataLoaded = false;
			assetManager.Load(ref this.planetColorHandle, assetName, new AssetSuccessDelegate(this.OnPlanetColorGradientDataLoaded), new AssetFailureDelegate(this.OnPlanetColorFailedToLoad), null);
		}

		private void CalculateAndSetLocalTimeSinceSunrise(uint timeStamp)
		{
			uint num = timeStamp + (uint)Service.Get<EnvironmentController>().GetTimezoneOffsetSeconds();
			long sunriseTimestamp = this.planetVO.GetSunriseTimestamp((int)num);
			this.lastLocalTimeSinceSunrise = (long)((ulong)num - (ulong)sunriseTimestamp);
		}

		private void CalculateInitialPlanetLightingData(uint timeStamp)
		{
			this.localTimeDelta = 0.0;
			this.CalculateAndSetLocalTimeSinceSunrise(timeStamp);
			this.cycleLengthSeconds = (int)(24f / this.planetVO.CyclesPerDay * 3600f);
		}

		private void InitPlanetLightingData(uint timeStamp)
		{
			if (this.planetVO != null)
			{
				if (this.overrideLightingDataAssetName == null)
				{
					this.LoadPlanetLightingData(this.planetVO.TimeOfDayAsset);
					this.CalculateInitialPlanetLightingData(timeStamp);
					return;
				}
			}
			else
			{
				Service.Get<StaRTSLogger>().Warn("Init called without getting valid planet vo; current planet: " + Service.Get<CurrentPlayer>().PlanetId);
			}
		}

		public override void RefreshShaderColors()
		{
			base.RefreshShaderColors();
			this.UpdateShaderColors();
		}

		public override void UpdateEnvironmentLighting(float dt)
		{
			if (this.IsTimePausedInDayPhase())
			{
				this.SetTimeOfDayColorToTheDayPhase();
				return;
			}
			base.UpdateEnvironmentLighting(dt);
			if (this.IsColorTimeSegmentUpdateReady(dt))
			{
				this.UpdateShaderColors();
			}
		}

		private void RefreshPlanetData(uint timeStamp)
		{
			IMapDataLoader mapDataLoader = Service.Get<WorldTransitioner>().GetMapDataLoader();
			if (mapDataLoader != null)
			{
				PlanetVO planetData = mapDataLoader.GetPlanetData();
				if (planetData != null && this.planetVO.Uid != planetData.Uid)
				{
					this.planetVO = planetData;
					this.InitPlanetLightingData(timeStamp);
				}
				this.UpdateEnvironmentLighting(0f);
			}
		}

		private void SetTimeOfDayColorBySegmentAndPercent(TimeOfDaySegments segment, float percentIntoSegment)
		{
			this.segmentPercentage = (double)percentIntoSegment;
			this.timeOfDaySegment = segment;
			this.UpdateShaderColors();
		}

		private void SetTimeOfDayColorToTheDayPhase()
		{
			this.SetTimeOfDayColorBySegmentAndPercent(TimeOfDaySegments.Day, (float)GameConstants.TOD_MID_DAY_PERCENTAGE);
		}

		public void ForceSetAndLockTimeOfDay(TimeOfDaySegments segment, float percentIntoSegment)
		{
			this.pauseTimeOfDayUpdate = true;
			this.SetTimeOfDayColorBySegmentAndPercent(segment, percentIntoSegment);
		}

		public void UnlockAndRestoreTimeOfDay()
		{
			uint nowSeconds = DateUtils.GetNowSeconds();
			this.pauseTimeOfDayUpdate = false;
			this.CalculateInitialPlanetLightingData(nowSeconds);
		}

		public override void ApplyDelayedLightingDataOverride(EventId triggerEvent, string dataAssetName)
		{
			if (!this.lightingEvents.Contains(triggerEvent))
			{
				Service.Get<EventManager>().RegisterObserver(this, triggerEvent);
			}
			this.applyLightingOverrideEvent = triggerEvent;
			this.overrideLightingDataAssetName = dataAssetName;
		}

		public override void SetupDelayedLightingOverrideRemoval(EventId triggerEvent)
		{
			if (!this.lightingEvents.Contains(triggerEvent))
			{
				Service.Get<EventManager>().RegisterObserver(this, triggerEvent);
			}
			this.removeLightingOverrideEvent = triggerEvent;
		}

		public override void RemoveLightingDataOverride()
		{
			if (!string.IsNullOrEmpty(this.overrideLightingDataAssetName))
			{
				this.ClearLightingDataOverride();
				uint nowSeconds = DateUtils.GetNowSeconds();
				this.InitPlanetLightingData(nowSeconds);
			}
		}

		private void ClearLightingDataOverride()
		{
			this.applyLightingOverrideEvent = EventId.Nop;
			this.overrideLightingDataAssetName = null;
		}

		private void UnregisterOverrideEvent(EventId overrideEvent)
		{
			if (!this.lightingEvents.Contains(overrideEvent))
			{
				Service.Get<EventManager>().UnregisterObserver(this, overrideEvent);
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == this.applyLightingOverrideEvent)
			{
				this.UnregisterOverrideEvent(this.applyLightingOverrideEvent);
				this.applyLightingOverrideEvent = EventId.Nop;
				this.LoadPlanetLightingData(this.overrideLightingDataAssetName);
				return EatResponse.NotEaten;
			}
			if (id == this.removeLightingOverrideEvent)
			{
				this.UnregisterOverrideEvent(this.removeLightingOverrideEvent);
				this.removeLightingOverrideEvent = EventId.Nop;
				this.RemoveLightingDataOverride();
				return EatResponse.NotEaten;
			}
			if (id != EventId.WorldOutTransitionComplete)
			{
				if (id == EventId.GameStateChanged)
				{
					GameStateMachine gameStateMachine = Service.Get<GameStateMachine>();
					IState currentState = gameStateMachine.CurrentState;
					uint timeStamp = DateUtils.GetNowSeconds();
					if (currentState is BattlePlaybackState)
					{
						BattleEntry currentBattleEntry = Service.Get<BattlePlaybackController>().CurrentBattleEntry;
						BattleRecord currentBattleRecord = Service.Get<BattlePlaybackController>().CurrentBattleRecord;
						if (currentBattleEntry != null && currentBattleRecord != null)
						{
							uint num = (uint)(currentBattleRecord.BattleLength - currentBattleRecord.BattleAttributes.TimeLeft);
							timeStamp = currentBattleEntry.EndBattleServerTime - num;
						}
						else
						{
							timeStamp = (uint)UnityEngine.Random.Range(0, 86400000);
						}
						this.CalculateInitialPlanetLightingData(timeStamp);
					}
					else if (currentState is HomeState)
					{
						this.UnlockAndRestoreTimeOfDay();
					}
					else if (this.IsTimePausedInDayPhase())
					{
						this.SetTimeOfDayColorToTheDayPhase();
					}
					this.RefreshPlanetData(timeStamp);
				}
			}
			else
			{
				this.ClearLightingDataOverride();
				CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
				if (currentBattle != null && !currentBattle.IsPvP() && !currentBattle.IsDefense())
				{
					this.pauseTimeOfDayUpdate = true;
					this.SetTimeOfDayColorToTheDayPhase();
				}
			}
			return EatResponse.NotEaten;
		}

		public override Color GetCurrentLightingColor(LightingColorType type)
		{
			if (this.lightingData == null)
			{
				return this.defaultColor;
			}
			Gradient gradient = null;
			switch (type)
			{
			case LightingColorType.BuildingColorDark:
				gradient = this.lightingData.buildingsDark[(int)this.timeOfDaySegment];
				break;
			case LightingColorType.BuildingColorLight:
				gradient = this.lightingData.buildingsLight[(int)this.timeOfDaySegment];
				break;
			case LightingColorType.UnitColor:
				gradient = this.lightingData.units[(int)this.timeOfDaySegment];
				break;
			case LightingColorType.ShadowColor:
				gradient = this.lightingData.shadow[(int)this.timeOfDaySegment];
				break;
			case LightingColorType.GroundColor:
				gradient = this.lightingData.terrainDark[(int)this.timeOfDaySegment];
				break;
			case LightingColorType.GroundColorLight:
				gradient = this.lightingData.terrainLight[(int)this.timeOfDaySegment];
				break;
			case LightingColorType.GridColor:
				gradient = this.lightingData.gridColor[(int)this.timeOfDaySegment];
				break;
			case LightingColorType.BuildingGridColor:
				gradient = this.lightingData.buildingGridColor[(int)this.timeOfDaySegment];
				break;
			case LightingColorType.WallGridColor:
				gradient = this.lightingData.wallGridColor[(int)this.timeOfDaySegment];
				break;
			}
			if (gradient == null)
			{
				Service.Get<StaRTSLogger>().Error("Invalid Color Type; Type: " + type.ToString());
				return this.defaultColor;
			}
			return gradient.Evaluate((float)this.segmentPercentage);
		}

		private void UpdateShaderColors()
		{
			if (this.lightingData != null)
			{
				base.PLColorBuildingDark = this.GetCurrentLightingColor(LightingColorType.BuildingColorDark);
				Shader.SetGlobalColor("_PL_Buildings_Dark", base.PLColorBuildingDark);
				base.PLColorBuildingLight = this.GetCurrentLightingColor(LightingColorType.BuildingColorLight);
				Shader.SetGlobalColor("_PL_Buildings_Light", base.PLColorBuildingLight);
				base.PLColorUnits = this.GetCurrentLightingColor(LightingColorType.UnitColor);
				Shader.SetGlobalColor("_PL_Units", base.PLColorUnits);
				base.PLColorTerrainDark = this.GetCurrentLightingColor(LightingColorType.GroundColor);
				Shader.SetGlobalColor("_PL_Terrain_Dark", base.PLColorTerrainDark);
				base.PLColorTerrainLight = this.GetCurrentLightingColor(LightingColorType.GroundColorLight);
				Shader.SetGlobalColor("_PL_Terrain_Light", base.PLColorTerrainLight);
				base.PLColorShadow = this.GetCurrentLightingColor(LightingColorType.ShadowColor);
				Shader.SetGlobalColor("_PL_Shadow", base.PLColorShadow);
				base.PLColorWall = this.GetCurrentLightingColor(LightingColorType.WallGridColor);
				Shader.SetGlobalColor("_PL_Grid_Wall", base.PLColorWall);
				base.PLColorGrid = this.GetCurrentLightingColor(LightingColorType.GridColor);
				Shader.SetGlobalColor("_PL_Grid", base.PLColorGrid);
				base.PLColorGridBuildings = this.GetCurrentLightingColor(LightingColorType.BuildingGridColor);
				Shader.SetGlobalColor("_PL_Grid_Buildings", base.PLColorGridBuildings);
			}
		}

		private bool IsColorTimeSegmentUpdateReady(float dt)
		{
			if (this.planetColorGradientDataLoaded)
			{
				this.localTimeDelta += (double)dt;
				double num = this.localTimeDelta + (double)this.lastLocalTimeSinceSunrise;
				double secondsIntoCycle = num % (double)this.cycleLengthSeconds;
				double num2 = 0.0;
				TimeOfDaySegments segmentFromSecondsIntoCycle = this.GetSegmentFromSecondsIntoCycle(secondsIntoCycle, out num2);
				if (segmentFromSecondsIntoCycle != this.timeOfDaySegment || Math.Abs(num2 - this.segmentPercentage) > 1.0000000116860974E-07)
				{
					this.segmentPercentage = num2;
					this.timeOfDaySegment = segmentFromSecondsIntoCycle;
					return true;
				}
			}
			return false;
		}

		private TimeOfDaySegments GetSegmentFromSecondsIntoCycle(double secondsIntoCycle, out double percentTimeInSegment)
		{
			percentTimeInSegment = 0.0;
			if (this.planetVO != null)
			{
				double num = (double)(this.planetVO.SunriseDuration * (float)this.cycleLengthSeconds);
				double num2 = (double)(this.planetVO.MidDayDuration * (float)this.cycleLengthSeconds);
				double num3 = (double)(this.planetVO.SunsetDuration * (float)this.cycleLengthSeconds);
				double num4 = (double)(this.planetVO.NightDuration * (float)this.cycleLengthSeconds);
				if (secondsIntoCycle <= num && num > 0.0)
				{
					percentTimeInSegment = secondsIntoCycle / num;
					return TimeOfDaySegments.Sunrise;
				}
				secondsIntoCycle -= num;
				if (secondsIntoCycle <= num2 && num2 > 0.0)
				{
					percentTimeInSegment = secondsIntoCycle / num2;
					return TimeOfDaySegments.Day;
				}
				secondsIntoCycle -= num2;
				if (secondsIntoCycle <= num3 && num3 > 0.0)
				{
					percentTimeInSegment = secondsIntoCycle / num3;
					return TimeOfDaySegments.Sunset;
				}
				secondsIntoCycle -= num3;
				if (num4 > 0.0)
				{
					percentTimeInSegment = secondsIntoCycle / num4;
				}
			}
			return TimeOfDaySegments.Night;
		}

		private void OnPlanetColorGradientDataLoaded(object asset, object cookie)
		{
			this.planetColorGradientDataLoaded = true;
			GameObject gameObject = asset as GameObject;
			this.lightingData = gameObject.GetComponent<PlanetaryLightingMonoBehaviour>();
			if (this.lightingData == null)
			{
				Service.Get<StaRTSLogger>().Error("Failed find lighting data; asset name: " + gameObject.name);
				return;
			}
			if (this.IsTimePausedInDayPhase())
			{
				this.SetTimeOfDayColorToTheDayPhase();
			}
		}

		private void OnPlanetColorFailedToLoad(object cookie)
		{
			Service.Get<StaRTSLogger>().Error("Loading Planet Lighting Colors Failed; current planet: " + this.planetVO.Uid);
		}

		protected internal TimeOfDayLightingEffects(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).ApplyDelayedLightingDataOverride((EventId)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).ClearLightingDataOverride();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).ForceSetAndLockTimeOfDay((TimeOfDaySegments)(*(int*)args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).GetCurrentLightingColor((LightingColorType)(*(int*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).IsColorTimeSegmentUpdateReady(*(float*)args));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).IsTimePausedInDayPhase());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).LoadPlanetLightingData(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).OnPlanetColorFailedToLoad(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).OnPlanetColorGradientDataLoaded(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).RefreshShaderColors();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).RemoveLightingDataOverride();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).SetDefaultColors();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).SetTimeOfDayColorBySegmentAndPercent((TimeOfDaySegments)(*(int*)args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).SetTimeOfDayColorToTheDayPhase();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).SetupDelayedLightingOverrideRemoval((EventId)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).UnlockAndRestoreTimeOfDay();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).UnregisterOverrideEvent((EventId)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).UpdateEnvironmentLighting(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((TimeOfDayLightingEffects)GCHandledObjects.GCHandleToObject(instance)).UpdateShaderColors();
			return -1L;
		}
	}
}
