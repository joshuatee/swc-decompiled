using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Main.Views.World;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class ShuttleController : IEventObserver
	{
		private const string FACTORY_VEHICLE_LOCATOR = "locator_vehicle";

		private const string LANDING_EFFECT = "fx_landing_lrg";

		private const string TAKEOFF_EFFECT = "fx_takeoff_lrg";

		private const string REBEL_SHUTTLE_ASSET = "e9explor_rbl-mod";

		private const string EMPIRE_SHUTTLE_ASSET = "lamdaclassshuttle_emp-mod";

		private const string CONTRABAND_SHUTTLE_ASSET = "contrabandship_con-mod";

		private const string ARMORY_SHUTTLE_ASSET = "supplyship_smg-ani";

		private const int REGULAR_SHUTTLE = 0;

		private const int CONTRABAND_SHUTTLE = 1;

		private const int ARMORY_SHUTTLE = 2;

		private const float DEFAULT_ANIM_PERCENTAGE = 0.5f;

		private Dictionary<Entity, ShuttleAnim> shuttles;

		private Dictionary<int, string> rebelShuttles;

		private Dictionary<int, string> empireShuttles;

		public ShuttleController()
		{
			Service.Set<ShuttleController>(this);
			this.shuttles = new Dictionary<Entity, ShuttleAnim>();
			this.rebelShuttles = new Dictionary<int, string>();
			this.empireShuttles = new Dictionary<int, string>();
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingViewReady, EventPriority.AfterDefault);
			Service.Get<EventManager>().RegisterObserver(this, EventId.CurrencyCollected, EventPriority.AfterDefault);
			Service.Get<EventManager>().RegisterObserver(this, EventId.StarportMeterUpdated, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.EntityDestroyed, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.UserLiftedBuilding, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingReplaced, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.EquipmentDeactivated);
			this.SortShuttleAssetDataByFactionAndLevel();
		}

		private void SortShuttleAssetDataByFactionAndLevel()
		{
			IDataController dataController = Service.Get<IDataController>();
			Dictionary<string, TransportTypeVO>.ValueCollection all = dataController.GetAll<TransportTypeVO>();
			foreach (TransportTypeVO current in all)
			{
				if (current.TransportType == TransportType.Troop)
				{
					if (current.Faction == FactionType.Rebel)
					{
						this.rebelShuttles.Add(current.Level, current.AssetName);
					}
					if (current.Faction == FactionType.Empire)
					{
						this.empireShuttles.Add(current.Level, current.AssetName);
					}
				}
			}
		}

		public void UpdateContrabandShuttle(SmartEntity generator)
		{
			float percent = (float)generator.BuildingComp.BuildingTO.AccruedCurrency / (float)generator.BuildingComp.BuildingType.Storage;
			this.UpdateShuttle(generator, percent, 1);
		}

		public void UpdateArmoryShuttle(SmartEntity starport)
		{
			this.UpdateShuttle(starport, 0.5f, 2);
		}

		private void UpdateShuttle(Entity starport, float percent, int shuttleType)
		{
			if (percent <= 0f && shuttleType != 1)
			{
				this.RemoveStarportShuttle(starport);
				return;
			}
			if (!this.shuttles.ContainsKey(starport))
			{
				BuildingComponent buildingComponent = starport.Get<BuildingComponent>();
				BuildingTypeVO buildingType = buildingComponent.BuildingType;
				string text = null;
				if (shuttleType == 1)
				{
					text = "contrabandship_con-mod";
				}
				else if (shuttleType == 2)
				{
					text = "supplyship_smg-ani";
				}
				else
				{
					FactionType faction = buildingType.Faction;
					if (faction != FactionType.Empire)
					{
						if (faction == FactionType.Rebel)
						{
							if (this.rebelShuttles.ContainsKey(buildingType.Lvl))
							{
								text = this.rebelShuttles[buildingType.Lvl];
							}
							else
							{
								text = "e9explor_rbl-mod";
							}
						}
					}
					else if (this.empireShuttles.ContainsKey(buildingType.Lvl))
					{
						text = this.empireShuttles[buildingType.Lvl];
					}
					else
					{
						text = "lamdaclassshuttle_emp-mod";
					}
				}
				if (text != null)
				{
					ShuttleAnim shuttleAnim = new ShuttleAnim(starport);
					shuttleAnim.Percentage = percent;
					shuttleAnim.IsContrabandShuttle = (shuttleType == 1);
					shuttleAnim.IsArmoryShuttle = (shuttleType == 2);
					AssetManager assetManager = Service.Get<AssetManager>();
					assetManager.Load(ref shuttleAnim.LandingHandle, "fx_landing_lrg", new AssetSuccessDelegate(this.OnLandingFxSuccess), null, shuttleAnim);
					assetManager.Load(ref shuttleAnim.TakeoffHandle, "fx_takeoff_lrg", new AssetSuccessDelegate(this.OnTakeOffFxSuccess), null, shuttleAnim);
					assetManager.Load(ref shuttleAnim.MainHandle, text, new AssetSuccessDelegate(this.OnAssetSuccess), new AssetFailureDelegate(this.OnAssetFailure), shuttleAnim);
					this.shuttles.Add(starport, shuttleAnim);
				}
				return;
			}
			if (shuttleType == 1)
			{
				this.AnimateContrabandShuttle(this.shuttles[starport], percent);
				return;
			}
			if (shuttleType == 2)
			{
				this.AnimateArmoryShuttle(this.shuttles[starport]);
				return;
			}
			this.AnimateShuttle(this.shuttles[starport], percent);
		}

		private void OnLandingFxSuccess(object asset, object cookie)
		{
			GameObject gameObject = (GameObject)asset;
			gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			ShuttleAnim shuttleAnim = (ShuttleAnim)cookie;
			shuttleAnim.LandingEffect = gameObject;
			shuttleAnim.LandingEffect.GetComponent<ParticleSystem>().Stop(true);
		}

		private void OnTakeOffFxSuccess(object asset, object cookie)
		{
			GameObject gameObject = (GameObject)asset;
			gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			ShuttleAnim shuttleAnim = (ShuttleAnim)cookie;
			shuttleAnim.TakeOffEffect = gameObject;
			shuttleAnim.TakeOffEffect.GetComponent<ParticleSystem>().Stop(true);
		}

		private void OnAssetSuccess(object asset, object cookie)
		{
			GameObject gameObject = (GameObject)asset;
			ShuttleAnim shuttleAnim = (ShuttleAnim)cookie;
			shuttleAnim.GameObj = gameObject;
			SmartEntity smartEntity = (SmartEntity)shuttleAnim.Starport;
			if (this.shuttles.ContainsKey(smartEntity))
			{
				shuttleAnim.Anim = gameObject.GetComponent<Animation>();
				GameObjectViewComponent gameObjectViewComponent = smartEntity.Get<GameObjectViewComponent>();
				Transform transform = gameObject.transform;
				AssetMeshDataMonoBehaviour component;
				if (shuttleAnim.IsContrabandShuttle)
				{
					bool flag = shuttleAnim.Percentage < 1f || shuttleAnim.State == ShuttleState.Landing || shuttleAnim.State == ShuttleState.Idle;
					if (smartEntity.GameObjectViewComp == null || smartEntity.GameObjectViewComp.MainGameObject == null)
					{
						this.UnloadShuttle(shuttleAnim);
						StorageSpreadUtils.UpdateAllStarportFullnessMeters();
						return;
					}
					component = smartEntity.GameObjectViewComp.MainGameObject.GetComponent<AssetMeshDataMonoBehaviour>();
					for (int i = 0; i < component.OtherGameObjects.Count; i++)
					{
						if (component.OtherGameObjects[i].name.Contains("locator_vehicle"))
						{
							transform.position = component.OtherGameObjects[i].transform.position;
							break;
						}
					}
					if (flag)
					{
						this.AnimateContrabandShuttle(shuttleAnim, shuttleAnim.Percentage);
					}
				}
				else if (shuttleAnim.IsArmoryShuttle)
				{
					if (smartEntity.GameObjectViewComp == null || smartEntity.GameObjectViewComp.MainGameObject == null)
					{
						this.UnloadShuttle(shuttleAnim);
						return;
					}
					transform.position = gameObjectViewComponent.MainTransform.position;
					this.AnimateArmoryShuttle(shuttleAnim);
				}
				else
				{
					transform.position = gameObjectViewComponent.MainTransform.position;
					this.AnimateShuttle(shuttleAnim, shuttleAnim.Percentage);
				}
				transform.rotation = Quaternion.AngleAxis(-90f, Vector3.up);
				component = gameObject.GetComponent<AssetMeshDataMonoBehaviour>();
				if (component != null && component.OtherGameObjects != null)
				{
					shuttleAnim.ShadowGameObject = component.ShadowGameObject;
					for (int j = 0; j < component.OtherGameObjects.Count; j++)
					{
						if (component.OtherGameObjects[j].name.Contains("center_of_mass"))
						{
							shuttleAnim.CenterOfMass = component.OtherGameObjects[j];
							break;
						}
					}
				}
				if (shuttleAnim.ShadowGameObject != null)
				{
					shuttleAnim.ShadowMaterial = UnityUtils.EnsureMaterialCopy(shuttleAnim.ShadowGameObject.GetComponent<Renderer>());
					shuttleAnim.ShadowMaterial.shader = Service.Get<AssetManager>().Shaders.GetShader("TransportShadow");
				}
			}
			else
			{
				this.UnloadShuttle(shuttleAnim);
			}
			StorageSpreadUtils.UpdateAllStarportFullnessMeters();
		}

		private void OnAssetFailure(object cookie)
		{
			this.UnloadShuttle((ShuttleAnim)cookie);
		}

		private void AnimateContrabandShuttle(ShuttleAnim anim, float percent)
		{
			if (anim.Anim == null)
			{
				return;
			}
			if (percent < 1f)
			{
				ShuttleState state = anim.State;
				if (state != ShuttleState.Landing && state != ShuttleState.Idle)
				{
					anim.EnqueueState(ShuttleState.Landing);
					anim.EnqueueState(ShuttleState.Idle);
					return;
				}
			}
			else
			{
				ShuttleState state = anim.State;
				if (state == ShuttleState.Landing || state == ShuttleState.Idle)
				{
					anim.EnqueueState(ShuttleState.LiftOff);
					return;
				}
				this.RemoveStarportShuttle(anim.Starport);
			}
		}

		public void DestroyArmoryShuttle(Entity entity)
		{
			if (!this.shuttles.ContainsKey(entity))
			{
				return;
			}
			ShuttleAnim shuttleAnim = this.shuttles[entity];
			if (shuttleAnim.State == ShuttleState.Idle || !ArmoryUtils.IsAnyEquipmentActive(Service.Get<CurrentPlayer>().ActiveArmory))
			{
				this.RemoveStarportShuttle(entity);
			}
		}

		private void AnimateArmoryShuttle(ShuttleAnim anim)
		{
			if (anim.Anim == null || !ArmoryUtils.IsAnyEquipmentActive(Service.Get<CurrentPlayer>().ActiveArmory))
			{
				return;
			}
			anim.EnqueueState(ShuttleState.DropOff);
			anim.EnqueueState(ShuttleState.Idle);
		}

		private void AnimateShuttle(ShuttleAnim anim, float percent)
		{
			if (percent <= 0f)
			{
				this.RemoveStarportShuttle(anim.Starport);
				return;
			}
			if (anim.Anim == null)
			{
				return;
			}
			if (percent < 1f)
			{
				switch (anim.State)
				{
				case ShuttleState.None:
					anim.EnqueueState(ShuttleState.Landing);
					anim.EnqueueState(ShuttleState.Idle);
					return;
				case ShuttleState.Landing:
				case ShuttleState.Idle:
					break;
				default:
					return;
				}
			}
			else
			{
				switch (anim.State)
				{
				case ShuttleState.None:
					anim.EnqueueState(ShuttleState.Landing);
					anim.EnqueueState(ShuttleState.Idle);
					anim.EnqueueState(ShuttleState.LiftOff);
					anim.EnqueueState(ShuttleState.Hover);
					return;
				case ShuttleState.Landing:
				case ShuttleState.LiftOff:
				case ShuttleState.Hover:
					break;
				case ShuttleState.Idle:
					anim.EnqueueState(ShuttleState.LiftOff);
					anim.EnqueueState(ShuttleState.Hover);
					break;
				default:
					return;
				}
			}
		}

		public ShuttleAnim GetShuttleForStarport(Entity starport)
		{
			ShuttleAnim result = null;
			if (this.shuttles.ContainsKey(starport))
			{
				result = this.shuttles[starport];
			}
			return result;
		}

		public void RemoveStarportShuttle(Entity starport)
		{
			if (this.shuttles.ContainsKey(starport))
			{
				this.UnloadShuttle(this.shuttles[starport]);
				this.shuttles.Remove(starport);
			}
		}

		private void UnloadShuttle(ShuttleAnim anim)
		{
			anim.Stop();
			if (anim.ShadowMaterial != null)
			{
				UnityUtils.DestroyMaterial(anim.ShadowMaterial);
				anim.ShadowMaterial = null;
			}
			AssetManager assetManager = Service.Get<AssetManager>();
			if (anim.GameObj != null)
			{
				UnityEngine.Object.Destroy(anim.GameObj);
				anim.GameObj = null;
			}
			if (anim.MainHandle != AssetHandle.Invalid)
			{
				assetManager.Unload(anim.MainHandle);
				anim.MainHandle = AssetHandle.Invalid;
			}
			if (anim.LandingEffect != null)
			{
				UnityEngine.Object.Destroy(anim.LandingEffect);
				anim.LandingEffect = null;
			}
			if (anim.LandingHandle != AssetHandle.Invalid)
			{
				assetManager.Unload(anim.LandingHandle);
				anim.LandingHandle = AssetHandle.Invalid;
			}
			if (anim.TakeOffEffect != null)
			{
				UnityEngine.Object.Destroy(anim.TakeOffEffect);
				anim.TakeOffEffect = null;
			}
			if (anim.TakeoffHandle != AssetHandle.Invalid)
			{
				assetManager.Unload(anim.TakeoffHandle);
				anim.TakeoffHandle = AssetHandle.Invalid;
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.EntityDestroyed)
			{
				if (id != EventId.BuildingViewReady)
				{
					if (id != EventId.CurrencyCollected)
					{
						if (id != EventId.EntityDestroyed)
						{
							return EatResponse.NotEaten;
						}
						uint num = (uint)cookie;
						using (Dictionary<Entity, ShuttleAnim>.KeyCollection.Enumerator enumerator = this.shuttles.Keys.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Entity current = enumerator.Current;
								if (current.ID == num)
								{
									this.RemoveStarportShuttle(current);
									break;
								}
							}
							return EatResponse.NotEaten;
						}
					}
					else
					{
						CurrencyCollectionTag currencyCollectionTag = cookie as CurrencyCollectionTag;
						if (currencyCollectionTag.Type == CurrencyType.Contraband)
						{
							this.UpdateContrabandShuttle((SmartEntity)currencyCollectionTag.Building);
							return EatResponse.NotEaten;
						}
						return EatResponse.NotEaten;
					}
				}
				EntityViewParams entityViewParams = (EntityViewParams)cookie;
				SmartEntity entity = entityViewParams.Entity;
				if (!this.shuttles.ContainsKey(entity) && Service.Get<GameStateMachine>().CurrentState is HomeState && entity.BuildingComp.BuildingType.Type == BuildingType.Resource && entity.BuildingComp.BuildingType.Currency == CurrencyType.Contraband)
				{
					this.UpdateContrabandShuttle(entity);
				}
			}
			else if (id <= EventId.BuildingReplaced)
			{
				if (id != EventId.StarportMeterUpdated)
				{
					if (id == EventId.BuildingReplaced)
					{
						Entity entity2 = (Entity)cookie;
						BuildingTypeVO buildingType = entity2.Get<BuildingComponent>().BuildingType;
						if (buildingType.Type == BuildingType.Starport || (buildingType.Type == BuildingType.Resource && buildingType.Currency == CurrencyType.Contraband))
						{
							if (buildingType.Currency == CurrencyType.Contraband)
							{
								this.UpdateShuttle(entity2, 0.5f, 1);
							}
							else if (buildingType.Type == BuildingType.Armory)
							{
								this.UpdateShuttle(entity2, 0.5f, 2);
							}
							else
							{
								this.UpdateShuttle(entity2, 0.5f, 0);
							}
						}
					}
				}
				else
				{
					MeterShaderComponent meterShaderComponent = (MeterShaderComponent)cookie;
					this.UpdateShuttle(meterShaderComponent.Entity, meterShaderComponent.Percentage, 0);
				}
			}
			else if (id != EventId.UserLiftedBuilding)
			{
				if (id == EventId.EquipmentDeactivated)
				{
					NodeList<ArmoryNode> armoryNodeList = Service.Get<BuildingLookupController>().ArmoryNodeList;
					for (ArmoryNode armoryNode = armoryNodeList.Head; armoryNode != null; armoryNode = armoryNode.Next)
					{
						this.DestroyArmoryShuttle(armoryNode.Entity);
					}
				}
			}
			else
			{
				Entity entity3 = (Entity)cookie;
				BuildingTypeVO buildingType2 = entity3.Get<BuildingComponent>().BuildingType;
				if (buildingType2.Type == BuildingType.Starport || (buildingType2.Type == BuildingType.Resource && buildingType2.Currency == CurrencyType.Contraband) || buildingType2.Type == BuildingType.Armory)
				{
					this.RemoveStarportShuttle(entity3);
				}
			}
			return EatResponse.NotEaten;
		}

		protected internal ShuttleController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).AnimateArmoryShuttle((ShuttleAnim)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).AnimateContrabandShuttle((ShuttleAnim)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).AnimateShuttle((ShuttleAnim)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).DestroyArmoryShuttle((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).GetShuttleForStarport((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).OnAssetFailure(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).OnAssetSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).OnLandingFxSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).OnTakeOffFxSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).RemoveStarportShuttle((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).SortShuttleAssetDataByFactionAndLevel();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).UnloadShuttle((ShuttleAnim)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).UpdateArmoryShuttle((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).UpdateContrabandShuttle((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ShuttleController)GCHandledObjects.GCHandleToObject(instance)).UpdateShuttle((Entity)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(int*)(args + 2));
			return -1L;
		}
	}
}
