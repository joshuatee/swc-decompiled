using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.World;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class BuildingAnimationController : IEventObserver
	{
		private NodeList<BuildingRenderNode> nodeList;

		private const string IDLE_ANIMATION_TRACK = "Idle";

		private const string ACTIVE_ANIM = "Active";

		private const string FULL_ANIM = "Full";

		private const string INTRO_ANIM = "Intro";

		private const float FACTORY_SPARK_DELAY1 = 0.6f;

		private const float FACTORY_SPARK_DELAY2 = 1f;

		private const string TRAP_IDLE_LOOP = "InactiveIdle";

		private const string TRAP_ACTIVATE_TRACK = "Activate";

		private const string TRAP_ACTIVATED_LOOP = "ActivatedIdle";

		private const string TRAP_DEACTIVATE_TRACK = "Deactivate";

		private const string TRAP_DEACTIVATED_LOOP = "DeactivatedIdle";

		private string[] barracksOpenCloseAnims;

		private string[] storageOpenCloseAnims;

		public BuildingAnimationController()
		{
			Service.Set<BuildingAnimationController>(this);
			this.nodeList = Service.Get<EntityController>().GetNodeList<BuildingRenderNode>();
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.ShuttleAnimStateChanged);
			eventManager.RegisterObserver(this, EventId.GameStateChanged);
			eventManager.RegisterObserver(this, EventId.WorldLoadComplete);
			eventManager.RegisterObserver(this, EventId.ContractStarted);
			eventManager.RegisterObserver(this, EventId.ContractContinued);
			eventManager.RegisterObserver(this, EventId.ContractStopped);
			eventManager.RegisterObserver(this, EventId.GeneratorJustFilled);
			eventManager.RegisterObserver(this, EventId.CurrencyCollected);
			eventManager.RegisterObserver(this, EventId.BuildingViewReady);
			eventManager.RegisterObserver(this, EventId.EntityPostBattleRepairStarted);
			eventManager.RegisterObserver(this, EventId.EntityPostBattleRepairFinished);
			eventManager.RegisterObserver(this, EventId.TroopRecruited);
			eventManager.RegisterObserver(this, EventId.StorageDoorEvent);
			eventManager.RegisterObserver(this, EventId.ScreenClosing);
			eventManager.RegisterObserver(this, EventId.EquipmentDeactivated);
			this.barracksOpenCloseAnims = new string[]
			{
				"OpenDoors",
				"CloseDoors",
				"IdleClosed"
			};
			this.storageOpenCloseAnims = new string[]
			{
				"OpenDoor",
				"CloseDoor"
			};
		}

		private bool BuildingEligibleForActiveAnimation(Entity entity, IState gameState, BuildingAnimationComponent animComp)
		{
			if (entity == null)
			{
				return false;
			}
			if (gameState is EditBaseState)
			{
				return false;
			}
			if (animComp.BuildingUpgrading)
			{
				return false;
			}
			if (Service.Get<PostBattleRepairController>().IsEntityInRepair(entity))
			{
				return false;
			}
			if (animComp.Manufacturing)
			{
				return true;
			}
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			if (buildingComponent == null)
			{
				return false;
			}
			BuildingTypeVO buildingType = buildingComponent.BuildingType;
			if (buildingType.Type == BuildingType.HQ)
			{
				return true;
			}
			if (buildingType.Type == BuildingType.ShieldGenerator && buildingType.SubType == BuildingSubType.OutpostDefenseGenerator)
			{
				return true;
			}
			if (!(gameState is HomeState))
			{
				return false;
			}
			if (buildingComponent.BuildingType.Type == BuildingType.Barracks || buildingComponent.BuildingType.Type == BuildingType.Cantina)
			{
				return true;
			}
			if (buildingComponent.BuildingType.Type == BuildingType.DroidHut)
			{
				return true;
			}
			if (buildingComponent.BuildingType.Type == BuildingType.ScoutTower)
			{
				return Service.Get<RaidDefenseController>().IsRaidAvailable();
			}
			if (buildingComponent.BuildingType.Type == BuildingType.Resource && buildingComponent.BuildingTO.AccruedCurrency < buildingType.Storage)
			{
				return true;
			}
			if (buildingComponent.BuildingType.Type == BuildingType.Storage && buildingComponent.BuildingTO.CurrentStorage < buildingType.Storage)
			{
				return true;
			}
			if (buildingComponent.BuildingType.Type == BuildingType.Armory)
			{
				ActiveArmory activeArmory = Service.Get<CurrentPlayer>().ActiveArmory;
				return ArmoryUtils.IsAnyEquipmentActive(activeArmory);
			}
			return false;
		}

		private bool BuildingEligibleForIdleAnimation(Entity entity, IState gameState, BuildingAnimationComponent animComp)
		{
			if (entity == null)
			{
				return false;
			}
			if (gameState is EditBaseState)
			{
				return false;
			}
			if (animComp.BuildingUpgrading)
			{
				return false;
			}
			if (Service.Get<PostBattleRepairController>().IsEntityInRepair(entity))
			{
				return false;
			}
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			if (buildingComponent == null)
			{
				return false;
			}
			if (!(gameState is HomeState))
			{
				return false;
			}
			if (buildingComponent.BuildingType.Type == BuildingType.ScoutTower)
			{
				return true;
			}
			if (buildingComponent.BuildingType.Type == BuildingType.Armory)
			{
				ActiveArmory activeArmory = Service.Get<CurrentPlayer>().ActiveArmory;
				return !ArmoryUtils.IsAnyEquipmentActive(activeArmory);
			}
			return false;
		}

		public void PlayAnimation(Animation anim)
		{
			this.PlayAnimation(anim, "Active");
		}

		public void PlayAnimation(Animation anim, string animName)
		{
			anim.Stop();
			if (anim.GetClip(animName) != null)
			{
				anim[animName].time = Service.Get<Rand>().ViewRangeFloat(0f, anim[animName].length);
				anim.Play(animName);
				return;
			}
			anim.Play();
		}

		private void StopAnimation(Animation anim)
		{
			anim.Stop();
			if (anim.GetClip("Idle") != null)
			{
				anim["Idle"].normalizedTime = 1f;
				anim.Play("Idle");
				anim.Sample();
				anim.Stop();
			}
		}

		private void PlayFXs(List<ParticleSystem> fx)
		{
			if (fx == null)
			{
				return;
			}
			for (int i = 0; i < fx.Count; i++)
			{
				fx[i].Play();
			}
		}

		private void StopFXs(List<ParticleSystem> fx)
		{
			if (fx == null)
			{
				return;
			}
			for (int i = 0; i < fx.Count; i++)
			{
				fx[i].Stop();
			}
		}

		private void UpdateAnimation(Entity entity, IState gameMode, BuildingAnimationComponent animComp, bool updateContraband)
		{
			TrapComponent trapComp = ((SmartEntity)entity).TrapComp;
			if (trapComp != null)
			{
				return;
			}
			if (this.BuildingEligibleForActiveAnimation(entity, gameMode, animComp))
			{
				this.PlayAnimation(animComp.Anim, "Active");
				this.PlayFXs(animComp.ListOfParticleSystems);
			}
			else if (this.BuildingEligibleForIdleAnimation(entity, gameMode, animComp))
			{
				this.PlayAnimation(animComp.Anim, "Idle");
				this.StopFXs(animComp.ListOfParticleSystems);
			}
			else
			{
				this.StopAnimation(animComp.Anim);
				this.StopFXs(animComp.ListOfParticleSystems);
			}
			this.UpdateArmoryAnimation((SmartEntity)entity);
			if (updateContraband)
			{
				this.UpdateContraBandShipAnimation((SmartEntity)entity);
			}
		}

		private void EnqueueAnimation(BuildingAnimationComponent animComp, string animationID)
		{
			animComp.Anim.PlayQueued(animationID);
		}

		private void UpdateAnimations(IState gameMode)
		{
			for (BuildingRenderNode buildingRenderNode = this.nodeList.Head; buildingRenderNode != null; buildingRenderNode = buildingRenderNode.Next)
			{
				this.UpdateAnimation(buildingRenderNode.Entity, gameMode, buildingRenderNode.AnimComp, true);
				BuildingComponent buildingComp = buildingRenderNode.BuildingComp;
				BuildingTypeVO buildingType = buildingComp.BuildingType;
				if (buildingType.Type == BuildingType.Resource && buildingComp.BuildingTO.CurrentStorage >= buildingType.Storage)
				{
					this.UpdateAnimationOnGeneratorFull((SmartEntity)buildingRenderNode.Entity, gameMode);
				}
			}
			if (gameMode is HomeState)
			{
				StorageSpreadUtils.UpdateAllStarportFullnessMeters();
			}
		}

		private void StartAnimationOnContractStarted(Entity entity, Contract contract, IState currentState)
		{
			if (Service.Get<ISupportController>().IsBuildingFrozen(contract.ContractTO.BuildingKey))
			{
				return;
			}
			BuildingAnimationComponent buildingAnimationComponent = entity.Get<BuildingAnimationComponent>();
			if (buildingAnimationComponent == null)
			{
				return;
			}
			bool flag = contract.DeliveryType == DeliveryType.UpgradeBuilding || contract.DeliveryType == DeliveryType.Building;
			if (flag)
			{
				buildingAnimationComponent.BuildingUpgrading = true;
			}
			else
			{
				buildingAnimationComponent.BuildingUpgrading = false;
				buildingAnimationComponent.Manufacturing = true;
			}
			this.UpdateAnimation(entity, currentState, buildingAnimationComponent, true);
		}

		public void FactorySpark(int message, GameObjectViewComponent gameObjViewComp)
		{
			if (gameObjViewComp != null)
			{
				this.PrepareFactorySparkWithEvent(message, gameObjViewComp);
			}
		}

		public void DepotSpark(int message, GameObjectViewComponent gameObjViewComp)
		{
			if (gameObjViewComp != null)
			{
				this.PrepareFactorySparkWithEvent(message, gameObjViewComp);
			}
		}

		private void PrepareFactorySparkWithEvent(int message, GameObjectViewComponent viewComp)
		{
			if (viewComp.EffectGameObjects.Count > message)
			{
				GameObject gameObject = viewComp.EffectGameObjects[message];
				if (gameObject != null)
				{
					Transform transform = gameObject.transform;
					Transform transform2 = null;
					if (transform.childCount > 0)
					{
						transform2 = transform.GetChild(0);
					}
					if (transform2 != null)
					{
						ParticleSystem component = transform2.GetComponent<ParticleSystem>();
						component.simulationSpace = ParticleSystemSimulationSpace.World;
						if (component != null)
						{
							this.PlayStopParticle(0u, component);
						}
					}
				}
			}
		}

		private void UpdateAnimationOnContractStopped(Entity entity, IState currentState)
		{
			BuildingAnimationComponent buildingAnimationComponent = entity.Get<BuildingAnimationComponent>();
			if (buildingAnimationComponent == null)
			{
				return;
			}
			if (buildingAnimationComponent.BuildingUpgrading)
			{
				buildingAnimationComponent.BuildingUpgrading = false;
			}
			else if (buildingAnimationComponent.Manufacturing)
			{
				buildingAnimationComponent.Manufacturing = false;
			}
			this.UpdateAnimation(entity, currentState, buildingAnimationComponent, false);
		}

		private void UpdateContraBandGeneratorAnimation(SmartEntity entity, ShuttleAnim currentState)
		{
			if (entity.BuildingComp.BuildingType.Type != BuildingType.Resource || entity.BuildingComp.BuildingType.Currency != CurrencyType.Contraband)
			{
				return;
			}
			if (currentState != null)
			{
				BuildingAnimationComponent buildingAnimationComp = entity.BuildingAnimationComp;
				Animation anim = buildingAnimationComp.Anim;
				switch (currentState.State)
				{
				case ShuttleState.Landing:
				case ShuttleState.LiftOff:
					if (anim.GetClip("Full") != null)
					{
						anim.Stop();
						anim.Play("Full");
						return;
					}
					break;
				case ShuttleState.Idle:
					if (anim.GetClip("Active") != null && anim.GetClip("Intro") != null)
					{
						anim.Stop();
						anim.Play("Intro");
						this.EnqueueAnimation(buildingAnimationComp, "Active");
					}
					break;
				default:
					return;
				}
			}
		}

		private void UpdateArmoryAnimation(SmartEntity entity)
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			BuildingComponent buildingComp = entity.BuildingComp;
			if (!(currentState is HomeState) || buildingComp == null || buildingComp.BuildingType.Type != BuildingType.Armory)
			{
				return;
			}
			BuildingAnimationComponent buildingAnimationComp = entity.BuildingAnimationComp;
			if (buildingAnimationComp == null)
			{
				return;
			}
			Animation anim = buildingAnimationComp.Anim;
			if (!ArmoryUtils.IsAnyEquipmentActive(currentPlayer.ActiveArmory) && anim.GetClip("Idle") != null)
			{
				anim.Stop();
				anim.Play("Idle");
				Service.Get<ShuttleController>().DestroyArmoryShuttle(entity);
				return;
			}
			if (entity.StateComp.CurState == EntityState.Idle && anim.IsPlaying("Idle") && anim.GetClip("Active") != null && anim.GetClip("Intro") != null)
			{
				anim.Stop();
				anim.Play("Intro");
				this.EnqueueAnimation(buildingAnimationComp, "Active");
				Service.Get<ShuttleController>().UpdateArmoryShuttle(entity);
			}
		}

		private void UpdateContraBandShipAnimation(SmartEntity entity)
		{
			if (entity == null || entity.BuildingComp.BuildingType.Type != BuildingType.Resource || entity.BuildingComp.BuildingType.Currency != CurrencyType.Contraband)
			{
				return;
			}
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (!(currentState is HomeState) || ContractUtils.IsBuildingUpgrading(entity) || ContractUtils.IsBuildingConstructing(entity))
			{
				Service.Get<ShuttleController>().RemoveStarportShuttle(entity);
			}
			else
			{
				Service.Get<ShuttleController>().UpdateContrabandShuttle(entity);
			}
			ShuttleAnim shuttleForStarport = Service.Get<ShuttleController>().GetShuttleForStarport(entity);
			if (shuttleForStarport != null)
			{
				this.UpdateContraBandGeneratorAnimation(entity, shuttleForStarport);
			}
		}

		private void UpdateAnimationOnGeneratorFull(SmartEntity entity, IState currentState)
		{
			if (!(currentState is HomeState))
			{
				return;
			}
			BuildingAnimationComponent buildingAnimationComp = entity.BuildingAnimationComp;
			Animation anim = buildingAnimationComp.Anim;
			if (anim.GetClip("Full") != null)
			{
				anim.Stop();
				anim.Play("Full");
			}
			this.UpdateContraBandShipAnimation(entity);
		}

		public void PlayStopParticle(uint id, ParticleSystem particle)
		{
			if (particle == null)
			{
				return;
			}
			if (particle.isPlaying)
			{
				particle.Stop();
			}
			particle.Play();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (id <= EventId.WorldLoadComplete)
			{
				if (id <= EventId.StorageDoorEvent)
				{
					if (id != EventId.BuildingViewReady)
					{
						switch (id)
						{
						case EventId.CurrencyCollected:
						{
							CurrencyCollectionTag currencyCollectionTag = cookie as CurrencyCollectionTag;
							SmartEntity smartEntity = (SmartEntity)currencyCollectionTag.Building;
							if (smartEntity != null)
							{
								BuildingAnimationComponent buildingAnimationComp = smartEntity.BuildingAnimationComp;
								if (buildingAnimationComp != null)
								{
									this.UpdateAnimation(smartEntity, currentState, buildingAnimationComp, true);
								}
							}
							break;
						}
						case EventId.GeneratorJustFilled:
							this.UpdateAnimationOnGeneratorFull((SmartEntity)cookie, currentState);
							break;
						case EventId.StorageDoorEvent:
						{
							SmartEntity smartEntity = (SmartEntity)cookie;
							if (smartEntity != null && smartEntity.BuildingAnimationComp != null && smartEntity.StorageComp != null && smartEntity.StorageComp.CurrentFullnessPercentage < 1f)
							{
								BuildingAnimationComponent buildingAnimationComp = smartEntity.BuildingAnimationComp;
								if (this.BuildingEligibleForActiveAnimation(smartEntity, currentState, buildingAnimationComp))
								{
									buildingAnimationComp.Anim.Stop();
									int num = this.storageOpenCloseAnims.Length;
									for (int i = 0; i < num; i++)
									{
										this.EnqueueAnimation(buildingAnimationComp, this.storageOpenCloseAnims[i]);
									}
								}
							}
							break;
						}
						}
					}
					else
					{
						EntityViewParams entityViewParams = cookie as EntityViewParams;
						SmartEntity smartEntity = entityViewParams.Entity;
						GameObject mainGameObject = smartEntity.GameObjectViewComp.MainGameObject;
						Animation component = mainGameObject.GetComponent<Animation>();
						if (!(component == null))
						{
							AssetMeshDataMonoBehaviour component2 = mainGameObject.GetComponent<AssetMeshDataMonoBehaviour>();
							this.UpdateAnimation(smartEntity, currentState, new BuildingAnimationComponent(component, component2 ? component2.ListOfParticleSystems : null), true);
						}
					}
				}
				else if (id != EventId.TroopRecruited)
				{
					if (id == EventId.WorldLoadComplete)
					{
						this.UpdateAnimations(currentState);
					}
				}
				else
				{
					ContractEventData contractEventData = cookie as ContractEventData;
					if (contractEventData.Contract.DeliveryType == DeliveryType.Infantry)
					{
						SmartEntity smartEntity = (SmartEntity)contractEventData.Entity;
						if (smartEntity != null)
						{
							BuildingAnimationComponent buildingAnimationComp = smartEntity.BuildingAnimationComp;
							if (buildingAnimationComp != null && this.BuildingEligibleForActiveAnimation(smartEntity, currentState, buildingAnimationComp))
							{
								buildingAnimationComp.Anim.Stop();
								for (int j = 0; j < this.barracksOpenCloseAnims.Length; j++)
								{
									this.EnqueueAnimation(buildingAnimationComp, this.barracksOpenCloseAnims[j]);
								}
							}
						}
					}
				}
			}
			else if (id <= EventId.ContractStopped)
			{
				if (id != EventId.GameStateChanged)
				{
					switch (id)
					{
					case EventId.ContractStarted:
					case EventId.ContractContinued:
					{
						ContractEventData contractEventData2 = (ContractEventData)cookie;
						this.StartAnimationOnContractStarted(contractEventData2.Entity, contractEventData2.Contract, currentState);
						break;
					}
					case EventId.ContractStopped:
						this.UpdateAnimationOnContractStopped((Entity)cookie, currentState);
						break;
					}
				}
				else
				{
					this.UpdateAnimations(currentState);
				}
			}
			else if (id != EventId.ScreenClosing)
			{
				switch (id)
				{
				case EventId.EntityPostBattleRepairStarted:
				case EventId.EntityPostBattleRepairFinished:
				{
					SmartEntity smartEntity = (SmartEntity)cookie;
					if (smartEntity != null)
					{
						BuildingAnimationComponent buildingAnimationComp = smartEntity.BuildingAnimationComp;
						if (buildingAnimationComp != null)
						{
							this.UpdateAnimation(smartEntity, currentState, buildingAnimationComp, true);
						}
					}
					break;
				}
				case EventId.AllPostBattleRepairFinished:
					break;
				case EventId.ShuttleAnimStateChanged:
				{
					ShuttleAnim shuttleAnim = (ShuttleAnim)cookie;
					SmartEntity smartEntity = (SmartEntity)shuttleAnim.Starport;
					if (smartEntity.BuildingComp.BuildingType.Type == BuildingType.Armory)
					{
						Service.Get<ShuttleController>().DestroyArmoryShuttle(smartEntity);
					}
					else
					{
						this.UpdateContraBandGeneratorAnimation(smartEntity, shuttleAnim);
					}
					break;
				}
				default:
					if (id == EventId.EquipmentDeactivated)
					{
						NodeList<ArmoryNode> armoryNodeList = Service.Get<BuildingLookupController>().ArmoryNodeList;
						for (ArmoryNode armoryNode = armoryNodeList.Head; armoryNode != null; armoryNode = armoryNode.Next)
						{
							this.UpdateArmoryAnimation((SmartEntity)armoryNode.Entity);
						}
					}
					break;
				}
			}
			else if (cookie is ArmoryScreen)
			{
				NodeList<ArmoryNode> armoryNodeList2 = Service.Get<BuildingLookupController>().ArmoryNodeList;
				for (ArmoryNode armoryNode2 = armoryNodeList2.Head; armoryNode2 != null; armoryNode2 = armoryNode2.Next)
				{
					this.UpdateArmoryAnimation((SmartEntity)armoryNode2.Entity);
				}
			}
			return EatResponse.NotEaten;
		}

		protected internal BuildingAnimationController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).BuildingEligibleForActiveAnimation((Entity)GCHandledObjects.GCHandleToObject(*args), (IState)GCHandledObjects.GCHandleToObject(args[1]), (BuildingAnimationComponent)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).BuildingEligibleForIdleAnimation((Entity)GCHandledObjects.GCHandleToObject(*args), (IState)GCHandledObjects.GCHandleToObject(args[1]), (BuildingAnimationComponent)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).DepotSpark(*(int*)args, (GameObjectViewComponent)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).EnqueueAnimation((BuildingAnimationComponent)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).FactorySpark(*(int*)args, (GameObjectViewComponent)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).PlayAnimation((Animation)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).PlayAnimation((Animation)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).PlayFXs((List<ParticleSystem>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).PrepareFactorySparkWithEvent(*(int*)args, (GameObjectViewComponent)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).StartAnimationOnContractStarted((Entity)GCHandledObjects.GCHandleToObject(*args), (Contract)GCHandledObjects.GCHandleToObject(args[1]), (IState)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).StopAnimation((Animation)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).StopFXs((List<ParticleSystem>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).UpdateAnimation((Entity)GCHandledObjects.GCHandleToObject(*args), (IState)GCHandledObjects.GCHandleToObject(args[1]), (BuildingAnimationComponent)GCHandledObjects.GCHandleToObject(args[2]), *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).UpdateAnimationOnContractStopped((Entity)GCHandledObjects.GCHandleToObject(*args), (IState)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).UpdateAnimationOnGeneratorFull((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (IState)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).UpdateAnimations((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).UpdateArmoryAnimation((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).UpdateContraBandGeneratorAnimation((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (ShuttleAnim)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((BuildingAnimationController)GCHandledObjects.GCHandleToObject(instance)).UpdateContraBandShipAnimation((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
