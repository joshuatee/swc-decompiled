using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using StaRTS.GameBoard.Components;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Pooling;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.World.Targeting
{
	public class TargetReticle : IViewFrameTimeObserver
	{
		public const string VIEW_NAME = "TargetReticle {0}";

		public const string ASSET_NAME = "tac_reticle_quad";

		public const int SCALE_PERCENT = 140;

		public const float LOCKON_SCALE_FACTOR = 3f;

		public const float LOCKON_ROTATION = 180f;

		public const float LOCKON_SPIN_DURATION = 0.5f;

		public const float LOCKON_SCALE_DURATION = 0.3f;

		private GameObject gameObjectView;

		private IObjectPool<TargetReticle> targetIdentifier;

		private Entity pendingTarget;

		private bool assetReady;

		private float reticleScale;

		private string name;

		private float lockonMaxDuration;

		private float ageMS;

		public GameObject View
		{
			get
			{
				return this.gameObjectView;
			}
			set
			{
				this.gameObjectView = value;
				this.ViewTransform = ((this.gameObjectView == null) ? null : this.gameObjectView.transform);
			}
		}

		public Transform ViewTransform
		{
			get;
			private set;
		}

		public TargetReticle()
		{
			this.reticleScale = 1f;
			this.name = "";
			base..ctor();
			AssetHandle assetHandle = AssetHandle.Invalid;
			Service.Get<AssetManager>().Load(ref assetHandle, "tac_reticle_quad", new AssetSuccessDelegate(this.OnLoad), new AssetFailureDelegate(this.OnFail), null);
		}

		public static TargetReticle CreateTargetReticlePoolObject(IObjectPool<TargetReticle> objectPool)
		{
			TargetReticle targetReticle = new TargetReticle();
			targetReticle.Construct(objectPool);
			return targetReticle;
		}

		public static void DeactivateTargetReticlePoolObject(TargetReticle targetReticle)
		{
			targetReticle.Deactivate();
		}

		public void Construct(IObjectPool<TargetReticle> objectPool)
		{
			this.targetIdentifier = objectPool;
			if (string.IsNullOrEmpty(this.name))
			{
				this.name = string.Format("TargetReticle {0}", new object[]
				{
					this.targetIdentifier.Capacity
				});
			}
		}

		private void OnLoad(object asset, object cookie)
		{
			if (asset is GameObject)
			{
				this.assetReady = true;
				this.CreateView((GameObject)asset, this.targetIdentifier);
			}
		}

		private void OnFail(object cookie)
		{
			Service.Get<StaRTSLogger>().Error("Failed to load target reticle");
		}

		private void CreateView(GameObject asset, IObjectPool<TargetReticle> objectPool)
		{
			this.View = asset;
			this.View.name = this.name;
			this.Deactivate();
			if (this.pendingTarget != null)
			{
				this.SetTarget(this.pendingTarget);
				this.pendingTarget = null;
			}
		}

		public void SetTarget(Entity target)
		{
			if (!this.assetReady)
			{
				this.pendingTarget = target;
				return;
			}
			TransformComponent transformComponent = target.Get<TransformComponent>();
			if (transformComponent == null)
			{
				return;
			}
			SizeComponent sizeComponent = target.Get<SizeComponent>();
			this.SetWorldTarget(transformComponent.CenterX(), transformComponent.CenterZ(), (float)sizeComponent.Width, (float)sizeComponent.Depth);
			this.StartLockon();
		}

		public void SetWorldTarget(float boardX, float boardZ, float width, float depth)
		{
			if (this.View == null)
			{
				return;
			}
			this.View.name = this.name;
			this.ViewTransform.position = new Vector3(Units.BoardToWorldX(boardX), 0.09f, Units.BoardToWorldZ(boardZ));
			this.reticleScale = 1.4f * Mathf.Max(Units.BoardToWorldX(width), Units.BoardToWorldZ(depth));
			this.ViewTransform.localScale = new Vector3(3f * this.reticleScale, 3f * this.reticleScale, 3f * this.reticleScale);
			this.ViewTransform.localEulerAngles = Vector3.zero;
			this.View.SetActive(true);
			this.StartLockon();
		}

		public void Deactivate()
		{
			if (this.View == null)
			{
				return;
			}
			this.View.SetActive(false);
		}

		private void StartLockon()
		{
			this.ageMS = 0f;
			this.lockonMaxDuration = Mathf.Max(0.5f, 0.3f);
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
		}

		private void FinishLockon()
		{
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		public void OnViewFrameTime(float dt)
		{
			this.ageMS += dt;
			float num = Mathf.Max(0f, (0.5f - this.ageMS) / 0.5f);
			float num2 = Mathf.Max(0f, (0.3f - this.ageMS) / 0.3f);
			if (this.ageMS >= this.lockonMaxDuration)
			{
				this.FinishLockon();
			}
			float num3 = 1f + num2 * 2f;
			float y = num * 180f;
			this.ViewTransform.localScale = new Vector3(num3 * this.reticleScale, num3 * this.reticleScale, num3 * this.reticleScale);
			this.ViewTransform.localEulerAngles = new Vector3(0f, y, 0f);
		}

		protected internal TargetReticle(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).Construct((IObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetReticle.CreateTargetReticlePoolObject((IObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).CreateView((GameObject)GCHandledObjects.GCHandleToObject(*args), (IObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).Deactivate();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			TargetReticle.DeactivateTargetReticlePoolObject((TargetReticle)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).FinishLockon();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).View);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).ViewTransform);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).OnFail(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).OnLoad(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).View = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).ViewTransform = (Transform)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).SetTarget((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).SetWorldTarget(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((TargetReticle)GCHandledObjects.GCHandleToObject(instance)).StartLockon();
			return -1L;
		}
	}
}
