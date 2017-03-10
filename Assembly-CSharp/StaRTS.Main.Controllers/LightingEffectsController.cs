using StaRTS.FX;
using StaRTS.Main.Models;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class LightingEffectsController : IViewFrameTimeObserver
	{
		public AbstractLightingEffects LightingEffects
		{
			get;
			private set;
		}

		public LightingEffectsController()
		{
			Service.Set<LightingEffectsController>(this);
			if (GameConstants.TIME_OF_DAY_ENABLED)
			{
				this.RegisterForFrameUpdates();
			}
			this.LightingEffects = new TimeOfDayLightingEffects();
		}

		public void OnViewFrameTime(float dt)
		{
			this.UpdateLightingEffects(dt);
		}

		public Color GetCurrentLightingColor(LightingColorType type)
		{
			return this.LightingEffects.GetCurrentLightingColor(type);
		}

		public void RegisterForFrameUpdates()
		{
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
		}

		public void UnregisterForFrameUpdates()
		{
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		public void UpdateLightingEffects(float dt)
		{
			this.LightingEffects.UpdateEnvironmentLighting(dt);
		}

		protected internal LightingEffectsController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LightingEffectsController)GCHandledObjects.GCHandleToObject(instance)).LightingEffects);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LightingEffectsController)GCHandledObjects.GCHandleToObject(instance)).GetCurrentLightingColor((LightingColorType)(*(int*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((LightingEffectsController)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LightingEffectsController)GCHandledObjects.GCHandleToObject(instance)).RegisterForFrameUpdates();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LightingEffectsController)GCHandledObjects.GCHandleToObject(instance)).LightingEffects = (AbstractLightingEffects)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((LightingEffectsController)GCHandledObjects.GCHandleToObject(instance)).UnregisterForFrameUpdates();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((LightingEffectsController)GCHandledObjects.GCHandleToObject(instance)).UpdateLightingEffects(*(float*)args);
			return -1L;
		}
	}
}
