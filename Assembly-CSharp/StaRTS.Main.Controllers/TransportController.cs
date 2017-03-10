using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using StaRTS.DataStructures;
using StaRTS.FX;
using StaRTS.GameBoard;
using StaRTS.GameBoard.Pathfinding;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class TransportController : IEventObserver
	{
		private static readonly Vector3 SPAWN_POSITION = new Vector3(60f, 12f, 60f);

		private static readonly Vector3 FACTORY_ORIENTATION = new Vector3(-1f, 0f, 0f);

		private static readonly Vector3 STARPORT_ORIENTATION = new Vector3(-1f, 0f, 0f);

		private static readonly Vector3 DOCK_OFFSET = new Vector3(0f, 1f, 3.8f);

		private static readonly float FACTORY_WALL_HEIGHT = 1.7f;

		private string TRANSPORT_SHIP_EMPIRE;

		private string TRANSPORT_SHIP_REBEL;

		private const string FACTORY_PRODUCT = "FactoryProduct";

		private const float VEHICLE_SCALE = 0.8f;

		private const float FADE_OUT_TIME = 1f;

		private const int TARGET_PROXIMITY = 2;

		private const string FACTORY_VEHICLE_LOCATOR = "locator_vehicle";

		private const string LANDING_EFFECT = "fx_landing_lrg";

		private const string TAKEOFF_EFFECT = "fx_takeoff_lrg";

		private const float LANDING_EFFECT_DELAY = 0.2f;

		private const float TAKEOFF_EFFECT_DELAY = 5f;

		private const float VEHICLE_SPAWN_DELAY = 3f;

		private Dictionary<int, GameObject> landingFxObjects;

		private Dictionary<int, GameObject> takeOffFxObjects;

		private Dictionary<int, AssetHandle> landingFxHandles;

		private Dictionary<int, AssetHandle> takeOffFxHandles;

		private int activeEffectsCount;

		private const int MAX_TROOP_EFFECTS_PER_STARPORT = 10;

		private EntityController entityController;

		private IDataController sdc;

		private FXManager fxManager;

		private ViewFader entityFader;

		private Vector3 factoryOffset;

		private Dictionary<Entity, TroopTypeVO> busyEntities;

		private Dictionary<Entity, TransportTroopEffect> troopEffectsByEntity;

		private Dictionary<Entity, int> numTroopEffectsByStarport;

		private List<Entity> busyTransportByFactory;

		public TransportController()
		{
			this.TRANSPORT_SHIP_EMPIRE = "ThetaClassBarge1";
			this.TRANSPORT_SHIP_REBEL = "Cr25transport1";
			base..ctor();
			this.entityController = Service.Get<EntityController>();
			this.sdc = Service.Get<IDataController>();
			this.fxManager = Service.Get<FXManager>();
			this.entityFader = new ViewFader();
			Service.Set<TransportController>(this);
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.TroopRecruited, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ContractStarted, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ContractContinued, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ContractCanceled, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.TroopReachedPathEnd, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.WorldReset, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingReplaced, EventPriority.Default);
			this.landingFxHandles = new Dictionary<int, AssetHandle>();
			this.takeOffFxHandles = new Dictionary<int, AssetHandle>();
			this.landingFxObjects = new Dictionary<int, GameObject>();
			this.takeOffFxObjects = new Dictionary<int, GameObject>();
			this.busyEntities = new Dictionary<Entity, TroopTypeVO>();
		}

		private void FactoryReached(object cookie)
		{
			ContractEventData contractEventData = cookie as ContractEventData;
			Entity entity = contractEventData.Entity;
			this.DespawnVehicle(entity);
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(entity.Get<BuildingComponent>().BuildingTO.Key);
			if (contract != null)
			{
				KeyValuePair<Contract, Entity> keyValuePair = new KeyValuePair<Contract, Entity>(contract, entity);
				Service.Get<ViewTimerManager>().CreateViewTimer(3f, false, new TimerDelegate(this.CallbackSpawnVehicle), keyValuePair);
			}
			Service.Get<EventManager>().SendEvent(EventId.TransportDeparted, null);
		}

		private void CallbackSpawnVehicle(uint id, object cookie)
		{
			KeyValuePair<Contract, Entity> keyValuePair = (KeyValuePair<Contract, Entity>)cookie;
			Contract key = keyValuePair.get_Key();
			Entity value = keyValuePair.get_Value();
			this.SpawnVehicle(key, value);
		}

		private void StarportReached(object cookie)
		{
			KeyValuePair<Entity, ContractEventData> keyValuePair = (KeyValuePair<Entity, ContractEventData>)cookie;
			Entity key = keyValuePair.get_Key();
			ContractEventData value = keyValuePair.get_Value();
			this.RemoveTransportRequest(value);
			TroopTypeVO troop = this.sdc.Get<TroopTypeVO>(value.Contract.ProductUid);
			StorageSpreadUtils.AddTroopToStarportVisually(key, troop);
			Service.Get<EventManager>().SendEvent(EventId.TransportDeparted, null);
		}

		private void ArrivingAtBuilding(object cookie)
		{
			Service.Get<EventManager>().SendEvent(EventId.TransportArrived, null);
			if (cookie != null)
			{
				this.LoadAndPlayEffects(cookie);
			}
		}

		private void LoadAndPlayEffects(object cookie)
		{
			int key = ((KeyValuePair<int, Vector3>)cookie).get_Key();
			if (!this.landingFxHandles.ContainsKey(key))
			{
				AssetHandle value = AssetHandle.Invalid;
				Service.Get<AssetManager>().Load(ref value, "fx_landing_lrg", new AssetSuccessDelegate(this.OnLandingFxSuccess), null, cookie);
				this.landingFxHandles.Add(key, value);
			}
			if (!this.takeOffFxHandles.ContainsKey(key))
			{
				AssetHandle value2 = AssetHandle.Invalid;
				Service.Get<AssetManager>().Load(ref value2, "fx_takeoff_lrg", new AssetSuccessDelegate(this.OnTakeOffFxSuccess), null, cookie);
				this.takeOffFxHandles.Add(key, value2);
			}
		}

		private void OnLandingFxSuccess(object asset, object cookie)
		{
			KeyValuePair<int, Vector3> keyValuePair = (KeyValuePair<int, Vector3>)cookie;
			int key = keyValuePair.get_Key();
			GameObject gameObject = (GameObject)asset;
			gameObject = Service.Get<AssetManager>().CloneGameObject(gameObject);
			gameObject.transform.position = keyValuePair.get_Value();
			Service.Get<ViewTimerManager>().CreateViewTimer(0.2f, false, new TimerDelegate(this.CallbackPlayParticle), gameObject);
			if (!this.landingFxObjects.ContainsKey(key))
			{
				this.landingFxObjects.Add(key, gameObject);
			}
		}

		private void OnTakeOffFxSuccess(object asset, object cookie)
		{
			KeyValuePair<int, Vector3> keyValuePair = (KeyValuePair<int, Vector3>)cookie;
			int key = keyValuePair.get_Key();
			GameObject gameObject = (GameObject)asset;
			gameObject = Service.Get<AssetManager>().CloneGameObject(gameObject);
			gameObject.transform.position = keyValuePair.get_Value();
			Service.Get<ViewTimerManager>().CreateViewTimer(5f, false, new TimerDelegate(this.CallbackPlayParticle), gameObject);
			if (!this.takeOffFxObjects.ContainsKey(key))
			{
				this.takeOffFxObjects.Add(key, gameObject);
			}
		}

		private void CallbackPlayParticle(uint id, object cookie)
		{
			GameObject effectObject = (GameObject)cookie;
			this.PlayParticle(effectObject);
		}

		private void PlayParticle(GameObject effectObject)
		{
			if (effectObject != null)
			{
				ParticleSystem component = effectObject.GetComponent<ParticleSystem>();
				if (component != null)
				{
					component.Play();
				}
			}
		}

		private void UnloadEffects(object cookie)
		{
			int key = ((KeyValuePair<int, Vector3>)cookie).get_Key();
			if (this.landingFxObjects.ContainsKey(key))
			{
				UnityEngine.Object.Destroy(this.landingFxObjects[key]);
				this.landingFxObjects.Remove(key);
			}
			if (this.takeOffFxObjects.ContainsKey(key))
			{
				UnityEngine.Object.Destroy(this.takeOffFxObjects[key]);
				this.takeOffFxObjects.Remove(key);
			}
			if (this.landingFxHandles.ContainsKey(key))
			{
				AssetHandle assetHandle = this.landingFxHandles[key];
				if (assetHandle != AssetHandle.Invalid)
				{
					Service.Get<AssetManager>().Unload(assetHandle);
					this.landingFxHandles.Remove(key);
				}
			}
			if (this.takeOffFxHandles.ContainsKey(key))
			{
				AssetHandle assetHandle2 = this.takeOffFxHandles[key];
				if (assetHandle2 != AssetHandle.Invalid)
				{
					Service.Get<AssetManager>().Unload(assetHandle2);
					this.takeOffFxHandles.Remove(key);
				}
			}
			this.activeEffectsCount--;
		}

		private void BuildSpline(LinearSpline spline, Vector3 startPosition, Vector3 pickupPosition, Vector3 dropoffPosition, Entity starportEntity, ContractEventData contractData)
		{
			spline.Start();
			KeyValuePair<int, Vector3> keyValuePair = new KeyValuePair<int, Vector3>(this.activeEffectsCount, dropoffPosition);
			this.activeEffectsCount++;
			Vector3 vector = pickupPosition + new Vector3(0f, 12f, 0f);
			Quaternion rotation = Quaternion.LookRotation(vector - startPosition);
			spline.AddWayPoint(startPosition, rotation);
			Vector3 position = Vector3.Lerp(startPosition, vector, 0.6f);
			spline.AddWayPoint(position, rotation, 0.9f, 0f);
			position = Vector3.Lerp(startPosition, vector, 0.7f);
			spline.AddWayPoint(position, rotation, 0.8f, 0f);
			position = Vector3.Lerp(startPosition, vector, 0.8f);
			spline.AddWayPoint(position, rotation, 0.7f, 0f);
			position = Vector3.Lerp(startPosition, vector, 0.9f);
			spline.AddWayPoint(position, rotation, 0.6f, 0f);
			Quaternion rotation2 = Quaternion.LookRotation(TransportController.FACTORY_ORIENTATION);
			spline.AddWayPoint(vector, rotation2, 0.3f, 0f, new WaypointReached(this.ArrivingAtBuilding), null);
			spline.AddWayPoint(pickupPosition, rotation2, 0.3f, 2f, new WaypointReached(this.FactoryReached), contractData);
			Quaternion rotation3 = Quaternion.LookRotation(TransportController.FACTORY_ORIENTATION + new Vector3(0f, -0.2f, 0f));
			spline.AddWayPoint(vector + new Vector3(0f, -5f, 0f), rotation3, 0.25f, 0f);
			Vector3 vector2 = dropoffPosition + new Vector3(0f, 12f, 0f);
			Vector3 vector3 = vector2 - vector;
			vector3.Normalize();
			Quaternion rotation4 = Quaternion.LookRotation(vector3 + new Vector3(0f, -0.2f, 0f));
			spline.AddWayPoint(vector, rotation4, 0.7f, 0f);
			Vector3 position2 = Vector3.Lerp(vector, vector2, 0.2f);
			Quaternion rotation5 = Quaternion.LookRotation(vector3);
			spline.AddWayPoint(position2, rotation5, 0.8f, 0f);
			Vector3 position3 = Vector3.Lerp(vector, vector2, 0.3f);
			spline.AddWayPoint(position3, rotation5);
			position = Vector3.Lerp(vector, vector2, 0.6f);
			spline.AddWayPoint(position, rotation5, 0.8f, 0f);
			position = Vector3.Lerp(vector, vector2, 0.7f);
			spline.AddWayPoint(position, rotation5, 0.6f, 0f);
			position = Vector3.Lerp(vector, vector2, 0.8f);
			spline.AddWayPoint(position, rotation5, 0.5f, 0f);
			position = Vector3.Lerp(vector, vector2, 0.9f);
			spline.AddWayPoint(position, rotation5, 0.4f, 0f);
			rotation2 = Quaternion.LookRotation(TransportController.STARPORT_ORIENTATION);
			spline.AddWayPoint(vector2, rotation2, 0.3f, 0f, new WaypointReached(this.ArrivingAtBuilding), keyValuePair);
			spline.AddWayPoint(dropoffPosition, rotation2, 0.3f, 2f, new WaypointReached(this.StarportReached), new KeyValuePair<Entity, ContractEventData>(starportEntity, contractData));
			rotation3 = Quaternion.LookRotation(TransportController.STARPORT_ORIENTATION + new Vector3(0f, -0.2f, 0f));
			spline.AddWayPoint(vector2 + new Vector3(0f, -5f, 0f), rotation3, 0.25f, 0f);
			Vector3 vector4 = startPosition - vector2;
			vector4.Normalize();
			Quaternion rotation6 = Quaternion.LookRotation(vector4 + new Vector3(0f, -0.2f, 0f));
			spline.AddWayPoint(vector2, rotation6, 0.7f, 0f, new WaypointReached(this.UnloadEffects), keyValuePair);
			position2 = Vector3.Lerp(vector2, startPosition, 0.2f);
			Quaternion rotation7 = Quaternion.LookRotation(vector4);
			spline.AddWayPoint(position2, rotation7, 0.8f, 0f);
			position3 = Vector3.Lerp(vector2, startPosition, 0.3f);
			spline.AddWayPoint(position3, rotation7);
			spline.AddWayPoint(startPosition, rotation7);
		}

		private Entity FindIdleTransport(TransportTypeVO transportType)
		{
			NodeList<TransportNode> nodeList = this.entityController.GetNodeList<TransportNode>();
			for (TransportNode transportNode = nodeList.Head; transportNode != null; transportNode = transportNode.Next)
			{
				if (transportNode.State.CurState == EntityState.Idle)
				{
					transportNode.Transport.GameObj.SetActive(true);
					return transportNode.Entity;
				}
			}
			Entity entity = Service.Get<EntityFactory>().CreateTransportEntity(transportType);
			Service.Get<EntityController>().AddEntity(entity);
			return entity;
		}

		private Entity FindIdleStarport(ContractEventData contractData)
		{
			Entity entity = StorageSpreadUtils.FindLeastFullStarport();
			if (entity != null)
			{
				TroopTypeVO troop = this.sdc.Get<TroopTypeVO>(contractData.Contract.ProductUid);
				StorageSpreadUtils.AddTroopToStarportReserve(entity, troop);
			}
			return entity;
		}

		private bool DespawnVehicle(Entity factoryEntity)
		{
			if (this.busyEntities.ContainsKey(factoryEntity))
			{
				this.fxManager.RemoveAttachedFXFromEntity(factoryEntity, "FactoryProduct");
				this.busyEntities.Remove(factoryEntity);
				return true;
			}
			return false;
		}

		private void OnVehicleAssetLoadSuccess(GameObject instance, Entity parentEntity, float value)
		{
			Animator component = instance.GetComponent<Animator>();
			if (component != null)
			{
				component.SetInteger("Motivation", 4);
			}
			if (this.busyEntities.ContainsKey(parentEntity))
			{
				float factoryRotation = this.busyEntities[parentEntity].FactoryRotation;
				Transform transform = instance.transform;
				transform.rotation = Quaternion.Euler(0f, -90f + factoryRotation, 0f);
				transform.localScale *= 0.8f * this.busyEntities[parentEntity].FactoryScaleFactor;
			}
		}

		private bool SpawnVehicle(Contract contract, Entity contractEntity)
		{
			if (this.busyEntities.ContainsKey(contractEntity))
			{
				return true;
			}
			TroopTypeVO optional = this.sdc.GetOptional<TroopTypeVO>(contract.ProductUid);
			if (optional == null)
			{
				Service.Get<StaRTSLogger>().Error("Could not find troop with uid " + contract.ProductUid);
				return true;
			}
			bool flag = this.DespawnVehicle(contractEntity);
			string troopID = optional.TroopID;
			string assetName;
			if (troopID == "ATAT")
			{
				assetName = "atatfactory_emp-mod";
			}
			else if (troopID == "MHC")
			{
				assetName = "umhcfactory_emp-mod";
			}
			else
			{
				assetName = optional.AssetName;
			}
			GameObjectViewComponent gameObjectViewComponent = contractEntity.Get<GameObjectViewComponent>();
			if (gameObjectViewComponent == null)
			{
				return true;
			}
			GameObject mainGameObject = gameObjectViewComponent.MainGameObject;
			if (mainGameObject == null)
			{
				return true;
			}
			Transform transform = mainGameObject.transform.Find("locator_vehicle");
			if (transform == null)
			{
				return true;
			}
			Vector3 offset = Vector3.zero;
			offset = transform.position + new Vector3(0f, 0.2f, 0f) - mainGameObject.transform.position;
			this.fxManager.CreateAndAttachFXToEntity(contractEntity, assetName, "FactoryProduct", new FXManager.AttachedFXLoadedCallback(this.OnVehicleAssetLoadSuccess), false, offset, true);
			this.busyEntities.Add(contractEntity, optional);
			return !flag;
		}

		private void SpawnInfantry(ContractEventData contractData)
		{
			Entity entity = this.FindIdleStarport(contractData);
			if (entity == null)
			{
				return;
			}
			Entity entity2 = contractData.Entity;
			TransformComponent transformComponent = entity2.Get<TransformComponent>();
			BoardCell<Entity> boardCell = null;
			IntPosition boardPosition = new IntPosition(transformComponent.X, transformComponent.Z);
			TroopTypeVO troopTypeVO = this.sdc.Get<TroopTypeVO>(contractData.Contract.ProductUid);
			Service.Get<TroopController>().FinalizeSafeBoardPosition(troopTypeVO, ref entity2, ref boardPosition, ref boardCell, TeamType.Defender, TroopSpawnMode.Unleashed, true);
			SmartEntity smartEntity = Service.Get<EntityFactory>().CreateTroopEntity(troopTypeVO, TeamType.Defender, boardPosition, entity2, TroopSpawnMode.Unleashed, false, true);
			BoardItemComponent boardItemComp = smartEntity.BoardItemComp;
			Service.Get<BoardController>().Board.AddChild(boardItemComp.BoardItem, boardCell.X, boardCell.Z, null, false);
			Service.Get<EntityController>().AddEntity(smartEntity);
			TroopComponent troopComp = smartEntity.TroopComp;
			TeamComponent teamComp = smartEntity.TeamComp;
			bool flag = false;
			PathingManager pathingManager = Service.Get<PathingManager>();
			pathingManager.StartPathing(smartEntity, (SmartEntity)entity, smartEntity.TransformComp, false, out flag, 0, new PathTroopParams
			{
				TroopWidth = smartEntity.SizeComp.Width,
				DPS = 0,
				MinRange = 0u,
				MaxRange = 2u,
				MaxSpeed = troopComp.SpeedVO.MaxSpeed,
				PathSearchWidth = troopComp.TroopType.PathSearchWidth,
				IsMelee = true,
				IsOverWall = false,
				IsHealer = false,
				CrushesWalls = false,
				IsTargetShield = false,
				TargetInRangeModifier = troopComp.TroopType.TargetInRangeModifier
			}, new PathBoardParams
			{
				IgnoreWall = (teamComp != null && teamComp.IsDefender()),
				Destructible = false
			}, false, true);
			if (!flag)
			{
				pathingManager.StartPathing(smartEntity, (SmartEntity)entity, smartEntity.TransformComp, false, out flag, 0, new PathTroopParams
				{
					TroopWidth = smartEntity.SizeComp.Width,
					DPS = 0,
					MinRange = 0u,
					MaxRange = 2u,
					MaxSpeed = troopComp.SpeedVO.MaxSpeed,
					PathSearchWidth = troopComp.TroopType.PathSearchWidth,
					IsMelee = true,
					IsOverWall = false,
					IsHealer = false,
					CrushesWalls = false,
					IsTargetShield = false,
					TargetInRangeModifier = troopComp.TroopType.TargetInRangeModifier
				}, new PathBoardParams
				{
					IgnoreWall = true,
					Destructible = false
				}, false, true);
			}
			smartEntity.StateComp.CurState = EntityState.Moving;
			bool showFullEffect = true;
			if (this.numTroopEffectsByStarport == null)
			{
				this.numTroopEffectsByStarport = new Dictionary<Entity, int>();
			}
			if (this.numTroopEffectsByStarport.ContainsKey(entity))
			{
				Dictionary<Entity, int> arg_283_0 = this.numTroopEffectsByStarport;
				Entity key = entity;
				int num = arg_283_0[key];
				arg_283_0[key] = num + 1;
				if (num >= 10)
				{
					showFullEffect = false;
				}
			}
			else
			{
				this.numTroopEffectsByStarport.Add(entity, 1);
			}
			if (this.troopEffectsByEntity == null)
			{
				this.troopEffectsByEntity = new Dictionary<Entity, TransportTroopEffect>();
			}
			this.troopEffectsByEntity.Add(smartEntity, new TransportTroopEffect(smartEntity, troopTypeVO, entity, this.entityFader, new TransportTroopEffect.OnEffectFinished(this.OnTroopEffectFinished), showFullEffect));
		}

		private void OnTroopEffectFinished(Entity troopEntity, Entity starportEntity)
		{
			this.troopEffectsByEntity.Remove(troopEntity);
			Dictionary<Entity, int> expr_15 = this.numTroopEffectsByStarport;
			int num = expr_15[starportEntity];
			expr_15[starportEntity] = num - 1;
		}

		private bool CountTransportRequest(ContractEventData contractData)
		{
			if (this.busyTransportByFactory == null)
			{
				this.busyTransportByFactory = new List<Entity>();
			}
			if (this.busyTransportByFactory.Contains(contractData.Entity))
			{
				return false;
			}
			this.busyTransportByFactory.Add(contractData.Entity);
			return true;
		}

		private void RemoveTransportRequest(ContractEventData contractData)
		{
			if (this.busyTransportByFactory != null && this.busyTransportByFactory.Contains(contractData.Entity))
			{
				this.busyTransportByFactory.Remove(contractData.Entity);
			}
		}

		private void SpawnTransport(ContractEventData contractData)
		{
			if (!this.CountTransportRequest(contractData))
			{
				return;
			}
			Entity entity = contractData.Entity;
			BuildingTypeVO buildingVO = contractData.BuildingVO;
			string uid;
			if (buildingVO.Faction == FactionType.Empire)
			{
				uid = this.TRANSPORT_SHIP_EMPIRE;
			}
			else
			{
				if (buildingVO.Faction != FactionType.Rebel)
				{
					return;
				}
				uid = this.TRANSPORT_SHIP_REBEL;
			}
			TransportTypeVO transportType = Service.Get<IDataController>().Get<TransportTypeVO>(uid);
			Entity entity2 = this.FindIdleTransport(transportType);
			if (entity2 == null)
			{
				return;
			}
			Entity entity3 = this.FindIdleStarport(contractData);
			if (entity3 == null)
			{
				return;
			}
			TransformComponent transformComponent = entity.Get<TransformComponent>();
			Vector3 vector = new Vector3(Units.BoardToWorldX(transformComponent.CenterX()), 0f, Units.BoardToWorldZ(transformComponent.CenterZ()));
			GameObject vehicleLocator = entity.Get<GameObjectViewComponent>().VehicleLocator;
			if (vehicleLocator != null)
			{
				this.factoryOffset = new Vector3(0f, vehicleLocator.transform.position.y + TransportController.FACTORY_WALL_HEIGHT, 0f);
			}
			else
			{
				this.factoryOffset = new Vector3(0f, TransportController.FACTORY_WALL_HEIGHT, 0f);
			}
			vector += this.factoryOffset;
			transformComponent = entity3.Get<TransformComponent>();
			Vector3 vector2 = new Vector3(Units.BoardToWorldX(transformComponent.CenterX()), 0f, Units.BoardToWorldZ(transformComponent.CenterZ()));
			vector2 += TransportController.DOCK_OFFSET;
			TransportComponent transportComponent = entity2.Get<TransportComponent>();
			this.BuildSpline(transportComponent.Spline, TransportController.SPAWN_POSITION, vector, vector2, entity3, contractData);
			entity2.Get<StateComponent>().CurState = EntityState.Moving;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.WorldReset)
			{
				if (id != EventId.TroopRecruited)
				{
					if (id != EventId.BuildingReplaced)
					{
						if (id == EventId.WorldReset)
						{
							if (this.troopEffectsByEntity != null)
							{
								foreach (TransportTroopEffect current in this.troopEffectsByEntity.Values)
								{
									current.Cleanup();
								}
								this.troopEffectsByEntity.Clear();
							}
							if (this.busyTransportByFactory != null)
							{
								this.busyTransportByFactory.Clear();
							}
							if (this.numTroopEffectsByStarport != null)
							{
								this.numTroopEffectsByStarport.Clear();
							}
						}
					}
					else
					{
						Entity entity = (Entity)cookie;
						StarportComponent starportComponent = entity.Get<StarportComponent>();
						if (starportComponent != null && this.numTroopEffectsByStarport != null)
						{
							foreach (KeyValuePair<Entity, int> current2 in this.numTroopEffectsByStarport)
							{
								if (current2.get_Key().Get<StarportComponent>() == null)
								{
									this.numTroopEffectsByStarport[entity] = current2.get_Value();
									this.numTroopEffectsByStarport.Remove(current2.get_Key());
									break;
								}
							}
						}
					}
				}
				else
				{
					ContractEventData contractEventData = cookie as ContractEventData;
					DeliveryType deliveryType = contractEventData.Contract.DeliveryType;
					if (deliveryType != DeliveryType.Infantry)
					{
						if (deliveryType == DeliveryType.Vehicle)
						{
							this.SpawnTransport(contractEventData);
							return EatResponse.NotEaten;
						}
						if (deliveryType != DeliveryType.Mercenary)
						{
							return EatResponse.NotEaten;
						}
					}
					this.SpawnInfantry(contractEventData);
				}
			}
			else
			{
				if (id <= EventId.ContractStarted)
				{
					if (id != EventId.TroopReachedPathEnd)
					{
						if (id != EventId.ContractStarted)
						{
							return EatResponse.NotEaten;
						}
					}
					else
					{
						Entity entity2 = cookie as Entity;
						if (entity2.Get<ShooterComponent>() == null && entity2.Get<DroidComponent>() == null && this.troopEffectsByEntity.ContainsKey(entity2))
						{
							TransportTroopEffect transportTroopEffect = this.troopEffectsByEntity[entity2];
							transportTroopEffect.OnTroopReachedPathEnd();
							return EatResponse.NotEaten;
						}
						return EatResponse.NotEaten;
					}
				}
				else if (id != EventId.ContractContinued)
				{
					if (id != EventId.ContractCanceled)
					{
						return EatResponse.NotEaten;
					}
					ContractEventData contractEventData2 = cookie as ContractEventData;
					DeliveryType deliveryType2 = contractEventData2.Contract.DeliveryType;
					if (deliveryType2 == DeliveryType.Vehicle)
					{
						this.DespawnVehicle(contractEventData2.Entity);
						return EatResponse.NotEaten;
					}
					return EatResponse.NotEaten;
				}
				ContractEventData contractEventData3 = cookie as ContractEventData;
				if (contractEventData3.Contract.DeliveryType == DeliveryType.Vehicle)
				{
					this.SpawnVehicle(contractEventData3.Contract, contractEventData3.Entity);
				}
			}
			return EatResponse.NotEaten;
		}

		protected internal TransportController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).ArrivingAtBuilding(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).BuildSpline((LinearSpline)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3)), (Entity)GCHandledObjects.GCHandleToObject(args[4]), (ContractEventData)GCHandledObjects.GCHandleToObject(args[5]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportController)GCHandledObjects.GCHandleToObject(instance)).CountTransportRequest((ContractEventData)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportController)GCHandledObjects.GCHandleToObject(instance)).DespawnVehicle((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).FactoryReached(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportController)GCHandledObjects.GCHandleToObject(instance)).FindIdleStarport((ContractEventData)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportController)GCHandledObjects.GCHandleToObject(instance)).FindIdleTransport((TransportTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).LoadAndPlayEffects(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).OnLandingFxSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).OnTakeOffFxSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).OnTroopEffectFinished((Entity)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).OnVehicleAssetLoadSuccess((GameObject)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]), *(float*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).PlayParticle((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).RemoveTransportRequest((ContractEventData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).SpawnInfantry((ContractEventData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).SpawnTransport((ContractEventData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportController)GCHandledObjects.GCHandleToObject(instance)).SpawnVehicle((Contract)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).StarportReached(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((TransportController)GCHandledObjects.GCHandleToObject(instance)).UnloadEffects(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
