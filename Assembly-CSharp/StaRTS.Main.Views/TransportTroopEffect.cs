using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Main.Views.World;
using StaRTS.Utils;
using StaRTS.Utils.Animation;
using StaRTS.Utils.Animation.Anims;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views
{
	public class TransportTroopEffect : IEventObserver
	{
		public delegate void OnEffectFinished(Entity troopEntity, Entity starportEntity);

		private const string EFFECT_ASSET = "troop_card_spawn";

		private const string TROOP_CARD_PARTICLE_SYSTEM = "CardFX";

		private const string SHUTTLE_GLOW_PARTICLE_SYSTEM = "UnderGlow";

		private const string TROOP_ASSET = "unittransport_event_troop_";

		private const float EFFECT_POS_Y = 1.2f;

		private const float POSITION_TWEEN_TIME = 0.8f;

		private const float FADE_OUT_TIME = 0.3f;

		private const float EFFECT_DURATION = 2f;

		private TroopTypeVO troopVO;

		private Entity troopEntity;

		private Entity starportEntity;

		private ViewFader entityFader;

		private TransportTroopEffect.OnEffectFinished onFinished;

		private ShuttleAnim shuttle;

		private GameObject effectObj;

		private AssetHandle troopHandle;

		private AssetHandle effectHandle;

		private ParticleSystem troopCardPS;

		private ParticleSystem shuttleGlowPS;

		private Texture2D troopCardTexture;

		private Material troopCardMaterial;

		private AnimPosition animPosition;

		private bool loadedAssets;

		private bool showFullEffect;

		private bool pathReached;

		private uint timerId;

		public TransportTroopEffect(Entity troopEntity, TroopTypeVO troopVO, Entity starportEntity, ViewFader entityFader, TransportTroopEffect.OnEffectFinished onFinished, bool showFullEffect)
		{
			this.troopVO = troopVO;
			this.troopEntity = troopEntity;
			this.starportEntity = starportEntity;
			this.entityFader = entityFader;
			this.onFinished = onFinished;
			this.showFullEffect = showFullEffect;
			if (showFullEffect)
			{
				TextureVO optional = Service.Get<IDataController>().GetOptional<TextureVO>("unittransport_event_troop_" + troopVO.TroopID);
				if (optional != null)
				{
					GeometryTag geometryTag = new GeometryTag(troopVO, optional.AssetName);
					Service.Get<EventManager>().SendEvent(EventId.TextureCreated, geometryTag);
					AssetManager assetManager = Service.Get<AssetManager>();
					assetManager.Load(ref this.troopHandle, geometryTag.assetName, new AssetSuccessDelegate(this.OnTroopCardLoaded), new AssetFailureDelegate(this.OnTroopCardLoadFailed), null);
					assetManager.Load(ref this.effectHandle, "troop_card_spawn", new AssetSuccessDelegate(this.OnEffectLoaded), new AssetFailureDelegate(this.OnEffectLoadFailed), null);
					this.loadedAssets = true;
				}
				else
				{
					Service.Get<StaRTSLogger>().WarnFormat("Transport troop effect error: {0} not found in TextureData", new object[]
					{
						"unittransport_event_troop_" + troopVO.TroopID
					});
					this.showFullEffect = false;
				}
			}
			ShuttleAnim shuttleForStarport = Service.Get<ShuttleController>().GetShuttleForStarport(starportEntity);
			if (shuttleForStarport != null && shuttleForStarport.State == ShuttleState.Idle)
			{
				this.shuttle = shuttleForStarport;
			}
			else
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.ShuttleAnimStateChanged, EventPriority.Default);
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingMovedOnBoard, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingReplaced, EventPriority.Default);
			this.timerId = 0u;
			this.pathReached = false;
		}

		private void OnTroopCardLoaded(object asset, object cookie)
		{
			this.troopCardTexture = (Texture2D)asset;
			this.TryShowEffect();
		}

		private void OnTroopCardLoadFailed(object cookie)
		{
			this.showFullEffect = false;
			this.TryShowEffect();
		}

		private void OnEffectLoaded(object asset, object cookie)
		{
			this.effectObj = UnityEngine.Object.Instantiate<GameObject>(asset as GameObject);
			this.troopCardPS = this.effectObj.transform.FindChild("CardFX").GetComponent<ParticleSystem>();
			this.shuttleGlowPS = this.effectObj.transform.FindChild("UnderGlow").GetComponent<ParticleSystem>();
			this.effectObj.SetActive(false);
			if (this.troopCardPS == null || !UnityUtils.HasRendererMaterial(this.troopCardPS.GetComponent<Renderer>()))
			{
				Service.Get<StaRTSLogger>().WarnFormat("Transport troop effect error: Particle system {0} not found in {1}", new object[]
				{
					"CardFX",
					"troop_card_spawn"
				});
				this.showFullEffect = false;
			}
			if (this.shuttleGlowPS == null)
			{
				Service.Get<StaRTSLogger>().WarnFormat("Transport troop effect error: Particle system {0} not found in {1}", new object[]
				{
					"UnderGlow",
					"troop_card_spawn"
				});
				this.showFullEffect = false;
			}
			this.TryShowEffect();
		}

		private void OnEffectLoadFailed(object cookie)
		{
			this.showFullEffect = false;
			this.TryShowEffect();
		}

		public void OnTroopReachedPathEnd()
		{
			this.pathReached = true;
			StorageSpreadUtils.AddTroopToStarportVisually(this.starportEntity, this.troopVO);
			this.TryShowEffect();
		}

		private bool ShuttleReadyForShowEffect()
		{
			return this.shuttle != null && this.shuttle.CenterOfMass != null && this.shuttle.CenterOfMass.transform != null;
		}

		private void TryShowEffect()
		{
			if (this.troopEntity == null)
			{
				Service.Get<StaRTSLogger>().Error("TryShowEffect with Null troopEntity");
				return;
			}
			bool flag = !this.showFullEffect || (this.troopCardTexture != null && this.effectObj != null && this.effectObj.transform != null);
			if (!this.ShuttleReadyForShowEffect())
			{
				this.shuttle = Service.Get<ShuttleController>().GetShuttleForStarport(this.starportEntity);
			}
			if (!flag || !this.pathReached || !this.ShuttleReadyForShowEffect())
			{
				if (!this.ShuttleReadyForShowEffect())
				{
					Service.Get<EventManager>().RegisterObserver(this, EventId.ShuttleAnimStateChanged, EventPriority.Default);
				}
				return;
			}
			GameObjectViewComponent gameObjectViewComponent = this.troopEntity.Get<GameObjectViewComponent>();
			if (gameObjectViewComponent == null)
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.TroopViewReady, EventPriority.Default);
				return;
			}
			Service.Get<EventManager>().SendEvent(EventId.TroopLoadingIntoStarport, this.troopEntity);
			if (this.entityFader == null || gameObjectViewComponent.MainGameObject == null)
			{
				this.Finish();
				return;
			}
			Vector3 position = this.shuttle.CenterOfMass.transform.position;
			this.animPosition = new AnimPosition(gameObjectViewComponent.MainGameObject, 0.8f, position);
			Service.Get<AnimController>().Animate(this.animPosition);
			this.entityFader.FadeOut(this.troopEntity, 0f, 0.3f, null, new FadingDelegate(this.OnFadeOutComplete));
			AssetMeshDataMonoBehaviour component = gameObjectViewComponent.MainGameObject.GetComponent<AssetMeshDataMonoBehaviour>();
			if (component != null && component.ShadowGameObject != null)
			{
				component.ShadowGameObject.SetActive(false);
			}
			this.timerId = Service.Get<ViewTimerManager>().CreateViewTimer(2f, false, new TimerDelegate(this.OnEffectTimer), null);
			if (this.showFullEffect)
			{
				this.effectObj.SetActive(true);
				position.y = 1.2f;
				this.effectObj.transform.position = position;
				this.troopCardMaterial = UnityUtils.EnsureMaterialCopy(this.troopCardPS.GetComponent<Renderer>());
				this.troopCardMaterial.mainTexture = this.troopCardTexture;
				this.troopCardPS.Play();
				this.shuttleGlowPS.Play();
				this.shuttle.PulseOutline();
			}
		}

		private void OnFadeOutComplete(object fadedObject)
		{
			Entity entity = (Entity)fadedObject;
			if (this.animPosition != null)
			{
				Service.Get<AnimController>().CompleteAndRemoveAnim(this.animPosition);
				this.animPosition = null;
			}
			Service.Get<EntityFactory>().DestroyEntity(entity, true, false);
		}

		private void OnEffectTimer(uint timerId, object cookie)
		{
			this.Finish();
		}

		private void Finish()
		{
			if (this.onFinished != null)
			{
				this.onFinished(this.troopEntity, this.starportEntity);
			}
			this.Cleanup();
		}

		public void Cleanup()
		{
			if (this.loadedAssets)
			{
				if (this.effectObj != null)
				{
					UnityEngine.Object.Destroy(this.effectObj);
				}
				UnityUtils.DestroyMaterial(this.troopCardMaterial);
				AssetManager assetManager = Service.Get<AssetManager>();
				if (this.effectHandle != AssetHandle.Invalid)
				{
					assetManager.Unload(this.effectHandle);
					this.effectHandle = AssetHandle.Invalid;
				}
				if (this.troopHandle != AssetHandle.Invalid)
				{
					assetManager.Unload(this.troopHandle);
					this.troopHandle = AssetHandle.Invalid;
				}
			}
			if (this.shuttle == null)
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.ShuttleAnimStateChanged);
			}
			else
			{
				this.shuttle = null;
			}
			if (this.timerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.timerId);
			}
			else
			{
				this.timerId = 0u;
			}
			if (this.animPosition != null)
			{
				Service.Get<AnimController>().CompleteAndRemoveAnim(this.animPosition);
			}
			this.troopVO = null;
			this.troopEntity = null;
			this.starportEntity = null;
			this.entityFader = null;
			this.onFinished = null;
			this.effectObj = null;
			this.troopCardPS = null;
			this.shuttleGlowPS = null;
			this.troopCardTexture = null;
			this.troopCardMaterial = null;
			this.animPosition = null;
			this.showFullEffect = false;
			this.pathReached = false;
			Service.Get<EventManager>().UnregisterObserver(this, EventId.TroopViewReady);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.BuildingMovedOnBoard);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.BuildingReplaced);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.TroopViewReady)
			{
				if (id != EventId.BuildingMovedOnBoard)
				{
					if (id == EventId.TroopViewReady)
					{
						EntityViewParams entityViewParams = (EntityViewParams)cookie;
						if (entityViewParams.Entity == this.troopEntity)
						{
							Service.Get<EventManager>().UnregisterObserver(this, EventId.TroopViewReady);
							this.TryShowEffect();
						}
					}
				}
				else if (this.starportEntity == cookie)
				{
					this.shuttle = null;
					Service.Get<EventManager>().RegisterObserver(this, EventId.ShuttleAnimStateChanged, EventPriority.Default);
				}
			}
			else if (id != EventId.BuildingReplaced)
			{
				if (id == EventId.ShuttleAnimStateChanged)
				{
					ShuttleAnim shuttleAnim = (ShuttleAnim)cookie;
					if (shuttleAnim.State == ShuttleState.Idle && shuttleAnim.Starport == this.starportEntity)
					{
						this.shuttle = shuttleAnim;
						Service.Get<EventManager>().UnregisterObserver(this, EventId.ShuttleAnimStateChanged);
						this.TryShowEffect();
					}
				}
			}
			else
			{
				Entity entity = (Entity)cookie;
				if (this.starportEntity.Get<StarportComponent>() == null)
				{
					StarportComponent starportComponent = entity.Get<StarportComponent>();
					if (starportComponent != null)
					{
						this.starportEntity = entity;
						this.shuttle = Service.Get<ShuttleController>().GetShuttleForStarport(this.starportEntity);
					}
				}
			}
			return EatResponse.NotEaten;
		}

		protected internal TransportTroopEffect(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TransportTroopEffect)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TransportTroopEffect)GCHandledObjects.GCHandleToObject(instance)).Finish();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TransportTroopEffect)GCHandledObjects.GCHandleToObject(instance)).OnEffectLoaded(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TransportTroopEffect)GCHandledObjects.GCHandleToObject(instance)).OnEffectLoadFailed(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportTroopEffect)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TransportTroopEffect)GCHandledObjects.GCHandleToObject(instance)).OnFadeOutComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TransportTroopEffect)GCHandledObjects.GCHandleToObject(instance)).OnTroopCardLoaded(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TransportTroopEffect)GCHandledObjects.GCHandleToObject(instance)).OnTroopCardLoadFailed(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TransportTroopEffect)GCHandledObjects.GCHandleToObject(instance)).OnTroopReachedPathEnd();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportTroopEffect)GCHandledObjects.GCHandleToObject(instance)).ShuttleReadyForShowEffect());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((TransportTroopEffect)GCHandledObjects.GCHandleToObject(instance)).TryShowEffect();
			return -1L;
		}
	}
}
