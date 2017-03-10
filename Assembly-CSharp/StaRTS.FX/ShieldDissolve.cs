using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.FX
{
	public class ShieldDissolve : IViewFrameTimeObserver
	{
		private const float DISSOLVE_TIME = 0.75f;

		private const float DISSOLVE_DELAY = 0.25f;

		private bool turnOn;

		private bool done;

		private bool delayActive;

		private float delayTime;

		private float dissolveTime;

		private float shieldDissolve;

		private DissolveCompleteDelegate onComplete;

		private GameObject shield;

		private Material shieldMaterial;

		private ShieldBuildingInfo info;

		public ShieldDissolve()
		{
			this.done = true;
			base..ctor();
			this.done = true;
			this.delayActive = false;
			this.dissolveTime = 0f;
			this.delayTime = 0.25f;
		}

		public void Play(GameObject shield, Material material, bool turnOn, DissolveCompleteDelegate OnCompleteCallback, ShieldBuildingInfo info)
		{
			if (this.done && this.turnOn == turnOn)
			{
				return;
			}
			this.shield = shield;
			this.shieldMaterial = material;
			this.turnOn = turnOn;
			this.info = info;
			this.onComplete = OnCompleteCallback;
			if (this.done && this.shieldMaterial != null)
			{
				this.done = false;
				if (turnOn)
				{
					this.dissolveTime = 0f;
					this.delayActive = true;
					this.delayTime = 0.25f;
					this.shieldMaterial.SetFloat("_Dissolve", 1f);
				}
				else
				{
					this.dissolveTime = 0.75f;
					this.shieldMaterial.SetFloat("_Dissolve", 0f);
				}
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			}
		}

		public void OnViewFrameTime(float dt)
		{
			if (this.delayActive)
			{
				this.delayTime -= dt;
				if (this.delayTime > 0f)
				{
					return;
				}
				this.delayActive = false;
			}
			if (this.turnOn)
			{
				this.dissolveTime += dt;
				if (this.dissolveTime >= 0.75f)
				{
					this.done = true;
					this.dissolveTime = 0.75f;
				}
			}
			else
			{
				this.dissolveTime -= dt;
				if (this.dissolveTime <= 0f)
				{
					this.done = true;
					this.dissolveTime = 0f;
				}
			}
			if (this.shieldMaterial != null)
			{
				this.shieldDissolve = (0.75f - this.dissolveTime) / 0.75f;
				this.shieldMaterial.SetFloat("_Dissolve", this.shieldDissolve);
			}
			if (this.done)
			{
				if (this.onComplete != null)
				{
					this.onComplete(this.shield, !this.turnOn, this.info);
				}
				this.Cleanup();
			}
		}

		public void Cleanup()
		{
			this.shield = null;
			this.shieldMaterial = null;
			this.onComplete = null;
			this.info = null;
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		protected internal ShieldDissolve(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ShieldDissolve)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ShieldDissolve)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ShieldDissolve)GCHandledObjects.GCHandleToObject(instance)).Play((GameObject)GCHandledObjects.GCHandleToObject(*args), (Material)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, (DissolveCompleteDelegate)GCHandledObjects.GCHandleToObject(args[3]), (ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(args[4]));
			return -1L;
		}
	}
}
