using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using StaRTS.GameBoard.Components;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.World;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class EntityViewManager
	{
		public const float BUILDING_COLLIDER_BASE_HEIGHT = 0.25f;

		private const float HEALTH_FADE_TIME = 4f;

		public WallConnector WallConnector
		{
			get;
			private set;
		}

		public EntityViewManager()
		{
			Service.Set<EntityViewManager>(this);
			this.WallConnector = new WallConnector();
		}

		public void UnloadEntityAsset(Entity entity)
		{
			AssetComponent assetComponent = entity.Get<AssetComponent>();
			AssetManager assetManager = Service.Get<AssetManager>();
			if (assetComponent != null)
			{
				if (assetComponent.RequestedAssetHandle != AssetHandle.Invalid)
				{
					assetManager.Unload(assetComponent.RequestedAssetHandle);
					assetComponent.RequestedAssetHandle = AssetHandle.Invalid;
					assetComponent.RequestedAssetName = null;
				}
				int i = 0;
				int count = assetComponent.AddOnsAssetHandles.Count;
				while (i < count)
				{
					assetManager.Unload(assetComponent.AddOnsAssetHandles[i]);
					i++;
				}
				assetComponent.AddOnsAssetHandles.Clear();
			}
		}

		private void PrepareEntityView(SmartEntity entity, GameObject gameObject)
		{
			EntityRef entityRef = gameObject.AddComponent<EntityRef>();
			entityRef.Entity = entity;
			IDataController dataController = Service.Get<IDataController>();
			GameObjectViewComponent gameObjectViewComponent;
			if (entity.GameObjectViewComp != null)
			{
				gameObjectViewComponent = entity.GameObjectViewComp;
				UnityEngine.Object.Destroy(gameObjectViewComponent.MainGameObject);
				gameObjectViewComponent.MainGameObject = gameObject;
			}
			else
			{
				float tooltipHeightOffset = 0f;
				if (entity.BuildingComp != null)
				{
					tooltipHeightOffset = entity.BuildingComp.BuildingType.TooltipHeightOffset;
					EffectsTypeVO effectsTypeVO = dataController.Get<EffectsTypeVO>("effect203");
					AssetManager assetManager = Service.Get<AssetManager>();
					assetManager.RegisterPreloadableAsset(effectsTypeVO.AssetName);
					AssetHandle assetHandle = AssetHandle.Invalid;
					assetManager.Load(ref assetHandle, effectsTypeVO.AssetName, new AssetSuccessDelegate(this.SparkFxSuccess), null, entity);
				}
				else if (entity.TroopComp != null)
				{
					TroopTypeVO troopTypeVO = Service.Get<IDataController>().Get<TroopTypeVO>(entity.TroopComp.TroopType.Uid);
					tooltipHeightOffset = troopTypeVO.TooltipHeightOffset;
				}
				gameObjectViewComponent = new GameObjectViewComponent(gameObject, tooltipHeightOffset);
				entity.Add(gameObjectViewComponent);
			}
			SupportComponent supportComp = entity.SupportComp;
			SupportViewComponent supportViewComp = entity.SupportViewComp;
			if (supportComp != null && supportViewComp == null)
			{
				SupportViewComponent component = new SupportViewComponent();
				entity.Add<SupportViewComponent>(component);
				if (Service.Get<BuildingController>().PurchasingBuilding != entity)
				{
					Service.Get<BuildingTooltipController>().EnsureBuildingTooltip(entity);
				}
			}
			GeneratorComponent generatorComp = entity.GeneratorComp;
			GeneratorViewComponent generatorViewComponent = entity.GeneratorViewComp;
			if (generatorComp != null && generatorViewComponent == null)
			{
				generatorViewComponent = new GeneratorViewComponent(entity);
				entity.Add<GeneratorViewComponent>(generatorViewComponent);
			}
			TrapComponent trapComp = entity.TrapComp;
			TrapViewComponent trapViewComponent = entity.TrapViewComp;
			if (trapComp != null && trapViewComponent == null)
			{
				Animator component2 = gameObjectViewComponent.MainGameObject.GetComponent<Animator>();
				Transform transform = gameObjectViewComponent.MainGameObject.transform.FindChild("Contents");
				if (component2 == null)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("A trap has been added that does not have a MecAnim controller. Building Uid: {0}, AssetName: {1}", new object[]
					{
						entity.BuildingComp.BuildingType.Uid,
						entity.BuildingComp.BuildingType.AssetName
					});
				}
				else if (transform == null)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("A trap has been added that does not have a Contents transform. Building Uid: {0}, AssetName: {1}", new object[]
					{
						entity.BuildingComp.BuildingType.Uid,
						entity.BuildingComp.BuildingType.AssetName
					});
				}
				else
				{
					GameObject gameObject2 = transform.gameObject;
					if (gameObject2 == null)
					{
						Service.Get<StaRTSLogger>().ErrorFormat("A trap has been added that does not have a Contents GameObject. Building Uid: {0}, AssetName: {1}", new object[]
						{
							entity.BuildingComp.BuildingType.Uid,
							entity.BuildingComp.BuildingType.AssetName
						});
					}
					trapViewComponent = new TrapViewComponent(component2);
					if (trapComp.Type.EventType == TrapEventType.Turret)
					{
						Transform transform2 = gameObjectViewComponent.MainGameObject.transform;
						Transform transform3 = transform2.FindChild(trapComp.Type.TurretTED.TurretAnimatorName);
						if (transform3 == null)
						{
							Service.Get<StaRTSLogger>().ErrorFormat("Trap {0}: Cannot find a gameobject in path {1}", new object[]
							{
								entity.BuildingComp.BuildingType.Uid,
								trapComp.Type.TurretTED.TurretAnimatorName
							});
						}
						else
						{
							Animator component3 = transform3.gameObject.GetComponent<Animator>();
							if (component3 == null)
							{
								Service.Get<StaRTSLogger>().ErrorFormat("Trap {0}: Cannot find an animator on gameobject in path {1}", new object[]
								{
									entity.BuildingComp.BuildingType.Uid,
									trapComp.Type.TurretTED.TurretAnimatorName
								});
							}
							else
							{
								trapViewComponent.TurretAnim = component3;
							}
						}
					}
					entity.Add<TrapViewComponent>(trapViewComponent);
				}
			}
			AssetMeshDataMonoBehaviour component4 = gameObjectViewComponent.MainGameObject.GetComponent<AssetMeshDataMonoBehaviour>();
			if (component4 != null && component4.OtherGameObjects != null)
			{
				for (int i = 0; i < component4.OtherGameObjects.Count; i++)
				{
					if (component4.OtherGameObjects[i].name.Contains("center_of_mass"))
					{
						gameObjectViewComponent.CenterOfMass = component4.OtherGameObjects[i];
					}
					else if (component4.OtherGameObjects[i].name.Contains("locator_hit"))
					{
						gameObjectViewComponent.HitLocators.Add(component4.OtherGameObjects[i]);
					}
					else if (component4.OtherGameObjects[i].name.Contains("locator_vehicle"))
					{
						gameObjectViewComponent.VehicleLocator = component4.OtherGameObjects[i];
					}
					else if (component4.OtherGameObjects[i].name.Contains("locator_effect"))
					{
						gameObjectViewComponent.EffectLocators.Add(component4.OtherGameObjects[i]);
					}
				}
			}
			FactoryComponent factoryComp = entity.FactoryComp;
			if (factoryComp != null)
			{
				EffectsTypeVO effectsTypeVO2 = dataController.Get<EffectsTypeVO>("effect203");
				AssetManager assetManager2 = Service.Get<AssetManager>();
				assetManager2.RegisterPreloadableAsset(effectsTypeVO2.AssetName);
				AssetHandle assetHandle2 = AssetHandle.Invalid;
				assetManager2.Load(ref assetHandle2, effectsTypeVO2.AssetName, new AssetSuccessDelegate(this.SparkFxSuccess), null, entity);
			}
			this.SetupGunLocators(entity, component4);
			if (entity.TroopShieldHealthComp != null && entity.TroopShieldViewComp == null)
			{
				TroopTypeVO troop = Service.Get<IDataController>().Get<TroopTypeVO>(entity.TroopComp.TroopType.Uid);
				entity.Add<TroopShieldViewComponent>(new TroopShieldViewComponent(troop));
			}
			gameObject.SetActive(false);
			this.CheckHealthView(entity);
			this.CheckMeterShaderView(entity);
			Service.Get<EntityRenderController>().UpdateNewEntityView(entity);
		}

		private void SetupGunLocators(SmartEntity entity, AssetMeshDataMonoBehaviour meta)
		{
			ShooterComponent shooterComp = entity.ShooterComp;
			if (shooterComp == null)
			{
				return;
			}
			if (meta == null || meta.GunLocatorGameObjects == null || meta.GunLocatorGameObjects.Count == 0)
			{
				return;
			}
			List<GameObject> list = null;
			if (entity.TroopComp != null)
			{
				TroopAbilityVO abilityVO = entity.TroopComp.AbilityVO;
				if (abilityVO != null)
				{
					int[] altGunLocators = abilityVO.AltGunLocators;
					if (altGunLocators != null && altGunLocators.Length != 0)
					{
						int count = meta.GunLocatorGameObjects.Count;
						List<GameObject> list2 = new List<GameObject>();
						int i = 0;
						int num = altGunLocators.Length;
						while (i < num)
						{
							int num2 = altGunLocators[i];
							if (num2 >= 1 && num2 < count)
							{
								GameObject item = meta.GunLocatorGameObjects[num2 - 1];
								list2.Add(item);
							}
							i++;
						}
						if (list2.Count > 0)
						{
							list = new List<GameObject>();
							for (int j = 0; j < count; j++)
							{
								GameObject item2 = meta.GunLocatorGameObjects[j];
								if (!list2.Contains(item2))
								{
									list.Add(item2);
								}
							}
							this.SetupGunLocatorsBySequence(entity, list2, abilityVO.GunSequence, true);
						}
					}
				}
			}
			if (list == null)
			{
				list = meta.GunLocatorGameObjects;
			}
			this.SetupGunLocatorsBySequence(entity, list, shooterComp.ShooterVO.GunSequence, false);
		}

		private void SetupGunLocatorsBySequence(SmartEntity entity, List<GameObject> gunLocatorGameObjects, int[] fireSequence, bool isAlternateSequence)
		{
			GameObjectViewComponent gameObjectViewComp = entity.GameObjectViewComp;
			List<List<GameObject>> list = isAlternateSequence ? gameObjectViewComp.SetupAlternateGunLocators() : gameObjectViewComp.GunLocators;
			int count = list.Count;
			if (count > 0)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("SetupGunLocatorsBySequence : numOrderedGunLocators = {0}on model {1}", new object[]
				{
					count,
					this.GetEntityUid(entity)
				});
				return;
			}
			int count2 = gunLocatorGameObjects.Count;
			int num = fireSequence.Length;
			if (num == count2)
			{
				Dictionary<int, List<GameObject>> dictionary = new Dictionary<int, List<GameObject>>();
				for (int i = 0; i < count2; i++)
				{
					GameObject gameObject = gunLocatorGameObjects[i];
					if (gameObject != null)
					{
						if (!dictionary.ContainsKey(fireSequence[i]))
						{
							dictionary.Add(fireSequence[i], new List<GameObject>());
						}
						dictionary[fireSequence[i]].Add(gameObject);
					}
					else
					{
						Service.Get<StaRTSLogger>().WarnFormat("Cannot find gun locator {0} for model {1}.", new object[]
						{
							i,
							this.GetEntityUid(entity)
						});
					}
				}
				int j = 1;
				int count3 = dictionary.Count;
				while (j <= count3)
				{
					list.Add(dictionary[j]);
					j++;
				}
				return;
			}
			Service.Get<StaRTSLogger>().ErrorFormat("GunSequence length ({0}) doesn't matchnumber of gun locators ({1}) for {2}", new object[]
			{
				fireSequence.Length,
				gunLocatorGameObjects.Count,
				this.GetEntityUid(entity)
			});
			List<GameObject> list2 = new List<GameObject>();
			for (int k = 0; k < count2; k++)
			{
				list2.Add(gunLocatorGameObjects[k]);
			}
			list.Add(list2);
		}

		private string GetEntityUid(SmartEntity entity)
		{
			if (entity.BuildingComp != null)
			{
				return entity.BuildingComp.BuildingType.Uid;
			}
			if (entity.TroopComp != null)
			{
				return entity.TroopComp.TroopType.Uid;
			}
			return string.Format("<entity {0}>", new object[]
			{
				entity.ID
			});
		}

		public void CheckHealthView(SmartEntity entity)
		{
			HealthComponent healthComp = entity.HealthComp;
			ShieldBorderComponent shieldBorderComp = entity.ShieldBorderComp;
			if (shieldBorderComp != null)
			{
				SmartEntity smartEntity = (SmartEntity)shieldBorderComp.ShieldGeneratorEntity;
				GameObjectViewComponent gameObjectViewComp = smartEntity.GameObjectViewComp;
				bool showAtFullHealth = this.ShowHealthView(entity, healthComp, gameObjectViewComp, false, true, false, false);
				this.ShowHealthView(smartEntity, smartEntity.HealthComp, gameObjectViewComp, true, false, showAtFullHealth, true);
				return;
			}
			ShieldGeneratorComponent shieldGeneratorComp = entity.ShieldGeneratorComp;
			if (shieldGeneratorComp != null)
			{
				SmartEntity smartEntity2 = (SmartEntity)shieldGeneratorComp.ShieldBorderEntity;
				GameObjectViewComponent gameObjectViewComp = entity.GameObjectViewComp;
				bool showAtFullHealth2 = this.ShowHealthView(entity, healthComp, gameObjectViewComp, true, false, false, true);
				this.ShowHealthView(smartEntity2, smartEntity2.HealthComp, gameObjectViewComp, false, true, showAtFullHealth2, false);
				return;
			}
			TroopShieldHealthComponent troopShieldHealthComp = entity.TroopShieldHealthComp;
			bool hasSecondary = false;
			bool showAtFullHealth3 = false;
			if (troopShieldHealthComp != null)
			{
				showAtFullHealth3 = this.ShowHealthView(entity, troopShieldHealthComp, entity.GameObjectViewComp, true, true, true, false);
				hasSecondary = true;
			}
			this.ShowHealthView(entity, healthComp, entity.GameObjectViewComp, true, hasSecondary, showAtFullHealth3, true);
		}

		private bool ShowHealthView(Entity entity, IHealthComponent health, GameObjectViewComponent viewToAttachTo, bool hasPrimary, bool hasSecondary, bool showAtFullHealth, bool isUpdatingPrimary)
		{
			SmartEntity smartEntity = (SmartEntity)entity;
			if (smartEntity.TrapComp != null)
			{
				return false;
			}
			bool flag = health != null && health.Health >= health.MaxHealth;
			if (health == null || (!showAtFullHealth & flag))
			{
				if (smartEntity.HealthViewComp != null)
				{
					smartEntity.HealthViewComp.TeardownElements();
				}
				return false;
			}
			if (viewToAttachTo == null)
			{
				if (smartEntity.HealthViewComp != null)
				{
					smartEntity.HealthViewComp.TeardownElements();
				}
				return false;
			}
			HealthViewComponent healthViewComponent = smartEntity.HealthViewComp;
			if (healthViewComponent == null)
			{
				healthViewComponent = new HealthViewComponent();
				entity.Add(healthViewComponent);
			}
			if (!healthViewComponent.IsInitialized)
			{
				healthViewComponent.SetupElements(viewToAttachTo, hasPrimary, hasSecondary);
			}
			healthViewComponent.UpdateHealth(health.Health, health.MaxHealth, !isUpdatingPrimary);
			if (!flag || !healthViewComponent.WillFadeOnTimer())
			{
				healthViewComponent.GoAwayIn(4f);
			}
			return true;
		}

		private void CheckMeterShaderView(Entity entity)
		{
			if (!entity.Has<GameObjectViewComponent>())
			{
				return;
			}
			GameObject mainGameObject = entity.Get<GameObjectViewComponent>().MainGameObject;
			if (entity.Get<MeterShaderComponent>() == null)
			{
				MeterShaderComponent meterShaderComponent = this.CreateMeterShaderComponentIfApplicable(mainGameObject);
				if (meterShaderComponent != null)
				{
					entity.Add(meterShaderComponent);
				}
			}
		}

		public MeterShaderComponent CreateMeterShaderComponentIfApplicable(GameObject gameObject)
		{
			if (gameObject != null)
			{
				AssetMeshDataMonoBehaviour component = gameObject.GetComponent<AssetMeshDataMonoBehaviour>();
				if (component != null && component.MeterGameObject != null)
				{
					return new MeterShaderComponent(component.MeterGameObject);
				}
			}
			return null;
		}

		private void PrepareGameObject(SmartEntity entity, GameObject gameObject, bool createCollider)
		{
			SizeComponent sizeComp = entity.SizeComp;
			if (sizeComp != null && createCollider)
			{
				if (gameObject.GetComponent<BoxCollider>() == null)
				{
					gameObject.AddComponent<BoxCollider>();
				}
				this.SetColliderHelper(gameObject, sizeComp, true);
			}
			this.PrepareEntityView(entity, gameObject);
		}

		public void SetCollider(Entity entity, bool flat)
		{
			GameObjectViewComponent gameObjectViewComponent = entity.Get<GameObjectViewComponent>();
			if (gameObjectViewComponent == null)
			{
				return;
			}
			SizeComponent sizeComponent = entity.Get<SizeComponent>();
			if (sizeComponent == null)
			{
				return;
			}
			this.SetColliderHelper(gameObjectViewComponent.MainGameObject, sizeComponent, flat);
		}

		private void SetColliderHelper(GameObject gameObject, SizeComponent size, bool flat)
		{
			BoxCollider component = gameObject.GetComponent<BoxCollider>();
			if (component == null)
			{
				return;
			}
			float y = gameObject.transform.position.y;
			float num = Units.BoardToWorldX(size.Depth);
			float num2 = Units.BoardToWorldX(size.Width);
			float num3 = flat ? 0.25f : ((num + num2) * 0.5f + y);
			component.size = new Vector3(num, num3, num2);
			component.center = new Vector3(0f, num3 * 0.5f - y, 0f);
		}

		public void LoadEntityAsset(Entity entity)
		{
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			bool flag = buildingComponent != null;
			string text = null;
			if (flag && buildingComponent.BuildingType.Connectors != null)
			{
				text = this.WallConnector.GetConnectorAssetName(entity, false, false);
			}
			if (text == null)
			{
				text = entity.Get<AssetComponent>().AssetName;
			}
			if (flag && buildingComponent.BuildingType.Type == BuildingType.Clearable)
			{
				PlanetVO planet = Service.Get<CurrentPlayer>().Map.Planet;
				text = GameUtils.GetClearableAssetName(buildingComponent.BuildingType, planet);
			}
			this.LoadEntityAsset(entity, text, flag);
		}

		public void LoadEntityAsset(Entity e, string assetName, bool createCollider)
		{
			SmartEntity smartEntity = (SmartEntity)e;
			EntityViewParams entityViewParams = new EntityViewParams(smartEntity, createCollider);
			AssetComponent assetComp = smartEntity.AssetComp;
			if (assetComp == null)
			{
				Service.Get<StaRTSLogger>().Error("Entity is missing asset component: " + assetName);
				this.AssetFailure(entityViewParams);
				return;
			}
			if (assetComp.RequestedAssetName == assetName)
			{
				bool isBuilding = smartEntity.BuildingComp != null;
				bool flag = assetName == null;
				bool isDroid = smartEntity.DroidComp != null;
				if (!flag)
				{
					GameObjectViewComponent gameObjectViewComp = smartEntity.GameObjectViewComp;
					if (gameObjectViewComp == null || gameObjectViewComp.MainGameObject == null)
					{
						return;
					}
				}
				this.SendAssetReadyEvent(isBuilding, flag, isDroid, entityViewParams);
				return;
			}
			if (assetComp.RequestedAssetHandle != AssetHandle.Invalid)
			{
				this.UnloadEntityAsset(smartEntity);
			}
			assetComp.RequestedAssetName = assetName;
			if (assetName == null)
			{
				this.AssetFailure(entityViewParams);
				return;
			}
			AssetManager assetManager = Service.Get<AssetManager>();
			assetManager.RegisterPreloadableAsset(assetName);
			WorldPreloadAsset preloadedAsset = Service.Get<WorldPreloader>().GetPreloadedAsset(assetName);
			if (preloadedAsset == null)
			{
				AssetHandle requestedAssetHandle = AssetHandle.Invalid;
				assetManager.Load(ref requestedAssetHandle, assetName, new AssetSuccessDelegate(this.AssetSuccess), new AssetFailureDelegate(this.AssetFailure), entityViewParams);
				assetComp.RequestedAssetHandle = requestedAssetHandle;
				return;
			}
			assetComp.RequestedAssetHandle = preloadedAsset.Handle;
			if (preloadedAsset.GameObj == null)
			{
				this.AssetFailure(entityViewParams);
				return;
			}
			this.AssetSuccess(preloadedAsset.GameObj, entityViewParams);
		}

		private void AssetSuccess(object asset, object cookie)
		{
			this.ProcessGameObject(asset, cookie, false);
		}

		private void ProcessGameObject(object asset, object cookie, bool isMissingAsset)
		{
			GameObject gameObject = asset as GameObject;
			EntityViewParams entityViewParams = cookie as EntityViewParams;
			SmartEntity entity = entityViewParams.Entity;
			BuildingComponent buildingComp = entity.BuildingComp;
			TroopComponent troopComp = entity.TroopComp;
			TransportComponent transportComp = entity.TransportComp;
			DroidComponent droidComp = entity.DroidComp;
			bool flag = buildingComp != null;
			bool flag2 = troopComp != null;
			bool flag3 = transportComp != null;
			bool isDroid = droidComp != null;
			AssetMeshDataMonoBehaviour component = gameObject.GetComponent<AssetMeshDataMonoBehaviour>();
			if (component != null && component.ShadowGameObject != null)
			{
				Vector3 position = component.ShadowGameObject.transform.position;
				position.y = 0.0225f;
				component.ShadowGameObject.transform.position = position;
			}
			Transform transform = gameObject.transform.FindChild("MASTER_MOVER");
			if (transform != null)
			{
				Transform transform2 = transform.FindChild("shadowMesh");
				if (transform2 != null)
				{
					Vector3 position2 = transform2.position;
					position2.y = 0.0225f;
					transform2.position = position2;
				}
			}
			if (flag2)
			{
				gameObject.name = "Troop " + troopComp.TroopType.Uid + " #" + entity.ID.ToString();
			}
			else if (flag)
			{
				gameObject.name = "Building " + buildingComp.BuildingType.Uid + " #" + entity.ID.ToString();
			}
			else if (flag3)
			{
				gameObject.name = "Transport " + transportComp.TransportType.Uid + " #" + entity.ID.ToString();
				transportComp.GameObj = gameObject;
				transportComp.ShadowGameObject = gameObject.transform.Find("shadowMesh").gameObject;
				transportComp.ShadowMaterial = UnityUtils.EnsureMaterialCopy(transportComp.ShadowGameObject.GetComponent<Renderer>());
				transportComp.ShadowMaterial.shader = Service.Get<AssetManager>().Shaders.GetShader("TransportShadow");
				return;
			}
			if (isMissingAsset)
			{
				gameObject.name += " MISSING";
			}
			bool createCollider = entityViewParams.CreateCollider;
			this.PrepareGameObject(entity, gameObject, createCollider);
			if (isMissingAsset || !this.LoadAddOns(entityViewParams))
			{
				this.SendAssetReadyEvent(flag, isMissingAsset, isDroid, entityViewParams);
			}
		}

		private bool LoadAddOns(EntityViewParams viewParams)
		{
			TrapComponent trapComp = viewParams.Entity.TrapComp;
			if (trapComp == null)
			{
				return false;
			}
			if (trapComp.Type.AddOns == null || trapComp.Type.AddOns.Count == 0)
			{
				return false;
			}
			AssetManager assetManager = Service.Get<AssetManager>();
			List<string> list = new List<string>();
			List<object> list2 = new List<object>();
			int i = 0;
			int count = trapComp.Type.AddOns.Count;
			while (i < count)
			{
				AssetHandle item = AssetHandle.Invalid;
				viewParams.Entity.AssetComp.AddOnsAssetHandles.Add(item);
				AddOnMapping addOnMapping = trapComp.Type.AddOns[i];
				string model = addOnMapping.Model;
				AddOnViewParams item2 = new AddOnViewParams(viewParams.Entity, addOnMapping.Parent);
				list.Add(model);
				list2.Add(item2);
				assetManager.RegisterPreloadableAsset(model);
				i++;
			}
			assetManager.MultiLoad(viewParams.Entity.AssetComp.AddOnsAssetHandles, list, new AssetSuccessDelegate(this.AddOnSuccess), null, list2, new AssetsCompleteDelegate(this.OnAllAddonsLoaded), viewParams);
			return true;
		}

		private void SparkFxSuccess(object asset, object cookie)
		{
			SmartEntity smartEntity = (SmartEntity)cookie;
			if (smartEntity == null || smartEntity.GameObjectViewComp == null)
			{
				return;
			}
			GameObjectViewComponent gameObjectViewComp = smartEntity.GameObjectViewComp;
			int i = 0;
			int count = gameObjectViewComp.EffectLocators.Count;
			while (i < count)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(asset as GameObject);
				GameObject gameObject2 = gameObjectViewComp.EffectLocators[i];
				if (gameObject2 != null)
				{
					gameObjectViewComp.EffectGameObjects.Add(gameObject);
					gameObject.transform.SetParent(gameObject2.transform);
					gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
				}
				i++;
			}
		}

		private void AddOnSuccess(object asset, object cookie)
		{
			AddOnViewParams addOnViewParams = (AddOnViewParams)cookie;
			if (addOnViewParams.Entity == null)
			{
				return;
			}
			GameObjectViewComponent gameObjectViewComp = addOnViewParams.Entity.GameObjectViewComp;
			if (gameObjectViewComp == null || gameObjectViewComp.MainGameObject == null)
			{
				return;
			}
			string parentName = addOnViewParams.ParentName;
			Transform transform = gameObjectViewComp.MainGameObject.transform.FindChild(parentName);
			if (transform == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Add On Parent not found!  There's a mismatch between the CMS AddOn parent name ({0}) and the loaded model ({1})", new object[]
				{
					parentName,
					gameObjectViewComp.MainGameObject.name
				});
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(asset as GameObject);
			gameObject.transform.parent = transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localRotation = Quaternion.identity;
		}

		private void OnAllAddonsLoaded(object cookie)
		{
			EntityViewParams entityViewParams = (EntityViewParams)cookie;
			bool isBuilding = entityViewParams.Entity.BuildingComp != null;
			bool isDroid = entityViewParams.Entity.DroidComp != null;
			this.SendAssetReadyEvent(isBuilding, false, isDroid, entityViewParams);
		}

		private void SendAssetReadyEvent(bool isBuilding, bool isMissingAsset, bool isDroid, EntityViewParams viewParams)
		{
			EventId id;
			if (isBuilding)
			{
				id = (isMissingAsset ? EventId.BuildingViewFailed : EventId.BuildingViewReady);
			}
			else
			{
				id = (isDroid ? EventId.DroidViewReady : EventId.TroopViewReady);
			}
			Service.Get<EventManager>().SendEvent(id, viewParams);
		}

		private void AssetFailure(object cookie)
		{
			EntityViewParams entityViewParams = cookie as EntityViewParams;
			BuildingComponent buildingComp = entityViewParams.Entity.BuildingComp;
			if (buildingComp != null)
			{
				GameObject asset = new GameObject();
				this.ProcessGameObject(asset, cookie, true);
			}
		}

		public void DestroyView(Entity entity)
		{
			entity.Remove<GameObjectViewComponent>();
			entity.Remove<HealthViewComponent>();
			entity.Remove<SupportViewComponent>();
		}

		protected internal EntityViewManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).AddOnSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).AssetFailure(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).AssetSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).CheckHealthView((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).CheckMeterShaderView((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).CreateMeterShaderComponentIfApplicable((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).DestroyView((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).WallConnector);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).GetEntityUid((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).LoadAddOns((EntityViewParams)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).LoadEntityAsset((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).LoadEntityAsset((Entity)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).OnAllAddonsLoaded(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).PrepareEntityView((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (GameObject)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).PrepareGameObject((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).ProcessGameObject(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).SendAssetReadyEvent(*(sbyte*)args != 0, *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0, (EntityViewParams)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).WallConnector = (WallConnector)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).SetCollider((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).SetColliderHelper((GameObject)GCHandledObjects.GCHandleToObject(*args), (SizeComponent)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).SetupGunLocators((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (AssetMeshDataMonoBehaviour)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).SetupGunLocatorsBySequence((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (List<GameObject>)GCHandledObjects.GCHandleToObject(args[1]), (int[])GCHandledObjects.GCHandleToPinnedArrayObject(args[2]), *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).ShowHealthView((Entity)GCHandledObjects.GCHandleToObject(*args), (IHealthComponent)GCHandledObjects.GCHandleToObject(args[1]), (GameObjectViewComponent)GCHandledObjects.GCHandleToObject(args[2]), *(sbyte*)(args + 3) != 0, *(sbyte*)(args + 4) != 0, *(sbyte*)(args + 5) != 0, *(sbyte*)(args + 6) != 0));
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).SparkFxSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((EntityViewManager)GCHandledObjects.GCHandleToObject(instance)).UnloadEntityAsset((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
