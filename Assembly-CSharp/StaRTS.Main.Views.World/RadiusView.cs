using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.World
{
	public class RadiusView
	{
		private const string ASSET_BUNDLE_INCORRECT_LOAD = "The incorrect assetbundle has been loaded for 'fx_radius_ring'";

		private const string RADIUS_KEY = "radius";

		private const string ASSET_NAME = "fx_radius_ring";

		private const string WHITE_RING = "fx_radius_white";

		private const string RED_RING = "fx_radius_red";

		private const string TOP_RING = "fx_radius_top";

		private const float PARTICLE_SCALE = 8.7f;

		protected const float PREWARM_DURATION = 5f;

		private const float PREWARM_DURATION_TOP = 6f;

		private GameObject radiusGameObject;

		private ParticleSystem whiteRingParticles;

		private ParticleSystem redRingParticles;

		private ParticleSystem topRingParticles;

		private GameObjectViewComponent govc;

		private AssetHandle handle;

		private bool ready;

		private Entity pendingEntity;

		public RadiusView() : this("fx_radius_ring")
		{
		}

		protected RadiusView(string assetName)
		{
			this.handle = AssetHandle.Invalid;
			Service.Get<AssetManager>().Load(ref this.handle, assetName, new AssetSuccessDelegate(this.OnRingLoaded), null, null);
		}

		private void OnRingLoaded(object asset, object cookie)
		{
			this.ready = true;
			GameObject original = (GameObject)asset;
			this.radiusGameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			this.SetupAssetOnLoad();
			this.radiusGameObject.SetActive(false);
			if (this.pendingEntity != null)
			{
				this.ShowHighlight(this.pendingEntity);
				this.pendingEntity = null;
			}
		}

		protected virtual void SetupAssetOnLoad()
		{
			this.TryFindParticleSystem(ref this.whiteRingParticles, "fx_radius_white");
			this.TryFindParticleSystem(ref this.redRingParticles, "fx_radius_red");
			this.TryFindParticleSystem(ref this.topRingParticles, "fx_radius_top");
		}

		protected void TryFindParticleSystem(ref ParticleSystem particlesToSet, string name)
		{
			Transform transform = this.radiusGameObject.transform.FindChild(name);
			if (transform != null)
			{
				particlesToSet = transform.gameObject.GetComponent<ParticleSystem>();
				if (particlesToSet == null)
				{
					this.ready = false;
					Service.Get<StaRTSLogger>().Error("The incorrect assetbundle has been loaded for 'fx_radius_ring'");
				}
			}
		}

		public void ShowHighlight(Entity entity)
		{
			if (!this.ready)
			{
				this.pendingEntity = entity;
				return;
			}
			if (entity == null)
			{
				Service.Get<StaRTSLogger>().Error("RadiusView.ShowHighlight: Entity = null");
				return;
			}
			this.govc = entity.Get<GameObjectViewComponent>();
			if (this.govc == null)
			{
				return;
			}
			if (this.radiusGameObject == null)
			{
				Service.Get<StaRTSLogger>().Error("RadiusView.ShowHighlight: radiusGameObject = null");
				return;
			}
			this.radiusGameObject.SetActive(true);
			if (!this.SetupParticlesOnShow(entity))
			{
				this.radiusGameObject.SetActive(false);
				return;
			}
			this.govc.AttachGameObject("radius", this.radiusGameObject, new Vector3(0f, 0.06f, 0f), true, false);
		}

		protected virtual bool SetupParticlesOnShow(Entity entity)
		{
			SmartEntity smartEntity = (SmartEntity)entity;
			uint minRange;
			if (smartEntity.ShooterComp != null)
			{
				minRange = smartEntity.ShooterComp.ShooterVO.MaxAttackRange;
			}
			else
			{
				if (smartEntity.TrapComp == null)
				{
					return false;
				}
				minRange = TrapUtils.GetTrapAttackRadius(smartEntity.TrapComp.Type);
			}
			this.SetupParticleSystemWithRange(this.whiteRingParticles, 5f, minRange);
			return true;
		}

		protected void SetupParticleSystemWithRange(ParticleSystem particles, float prewarm, uint minRange)
		{
			if (particles != null)
			{
				float num = minRange * 8.7f * 1f;
				if (num > 0f)
				{
					if (!particles.gameObject.activeSelf)
					{
						particles.gameObject.SetActive(true);
					}
					particles.startSize = num;
					particles.Simulate(prewarm);
					particles.Play();
					return;
				}
				if (particles.gameObject.activeSelf)
				{
					particles.gameObject.SetActive(false);
				}
			}
		}

		public void HideHighlight()
		{
			if (this.govc == null)
			{
				return;
			}
			this.govc.DetachGameObject("radius");
			this.govc = null;
			this.radiusGameObject.SetActive(false);
		}

		public void Destroy()
		{
			if (this.handle != AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Unload(this.handle);
				this.handle = AssetHandle.Invalid;
			}
		}

		protected internal RadiusView(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((RadiusView)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((RadiusView)GCHandledObjects.GCHandleToObject(instance)).HideHighlight();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((RadiusView)GCHandledObjects.GCHandleToObject(instance)).OnRingLoaded(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((RadiusView)GCHandledObjects.GCHandleToObject(instance)).SetupAssetOnLoad();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RadiusView)GCHandledObjects.GCHandleToObject(instance)).SetupParticlesOnShow((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((RadiusView)GCHandledObjects.GCHandleToObject(instance)).ShowHighlight((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
