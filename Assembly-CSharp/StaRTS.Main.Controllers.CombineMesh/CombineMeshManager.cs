using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.MeshCombiner;
using StaRTS.Utils.Pooling;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.CombineMesh
{
	public class CombineMeshManager : IEventObserver
	{
		private bool isSomeMeshesCombined;

		private bool isStartupTasksCompleted;

		private GameObject combinedMeshesGameObject;

		private ObjectPool<GameObject> meshCombinerGameObjectPool;

		private Dictionary<BuildingType, MeshCombiner> meshCombiners;

		private AbstractCombineMeshHelper homeBaseCobmbineMeshHelper;

		private AbstractCombineMeshHelper battleBaseCombineMeshHelper;

		public CombineMeshManager()
		{
			Service.Set<CombineMeshManager>(this);
			EventManager eventManager = Service.Get<EventManager>();
			this.meshCombinerGameObjectPool = MeshCombiner.CreateMeshCombinerObjectPool();
			this.combinedMeshesGameObject = new GameObject("CombinedMeshes");
			this.homeBaseCobmbineMeshHelper = new HomeBaseCombineMeshHelper();
			this.battleBaseCombineMeshHelper = new BattleBaseCombineMeshHelper();
			HashSet<BuildingType> hashSet = new HashSet<BuildingType>();
			hashSet.UnionWith(this.homeBaseCobmbineMeshHelper.GetEligibleBuildingTypes());
			hashSet.UnionWith(this.battleBaseCombineMeshHelper.GetEligibleBuildingTypes());
			this.meshCombiners = new Dictionary<BuildingType, MeshCombiner>();
			foreach (BuildingType current in hashSet)
			{
				this.meshCombiners.Add(current, new MeshCombiner(this.meshCombinerGameObjectPool, this.combinedMeshesGameObject, current.ToString()));
			}
			eventManager.RegisterObserver(this, EventId.GameStateChanged, EventPriority.MeshCombineAfterOthers);
			eventManager.RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.MeshCombineAfterOthers);
			eventManager.RegisterObserver(this, EventId.ShaderResetOnEntity, EventPriority.MeshCombineAfterOthers);
			eventManager.RegisterObserver(this, EventId.StartupTasksCompleted, EventPriority.MeshCombineAfterOthers);
			eventManager.RegisterObserver(this, EventId.BuildingConstructed, EventPriority.BeforeDefault);
			eventManager.RegisterObserver(this, EventId.BuildingLevelUpgraded, EventPriority.BeforeDefault);
			eventManager.RegisterObserver(this, EventId.BuildingSwapped, EventPriority.BeforeDefault);
			eventManager.RegisterObserver(this, EventId.ClearableCleared, EventPriority.AfterDefault);
			eventManager.RegisterObserver(this, EventId.WorldReset, EventPriority.BeforeDefault);
			eventManager.RegisterObserver(this, EventId.PostBuildingEntityKilled, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingViewReady, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingViewFailed, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.StartupTasksCompleted)
			{
				if (id <= EventId.BuildingViewFailed)
				{
					if (id == EventId.BuildingViewReady || id == EventId.BuildingViewFailed)
					{
						SmartEntity entity = ((EntityViewParams)cookie).Entity;
						if (!this.IsFueInProgress() && this.isStartupTasksCompleted && Service.Get<WorldTransitioner>().IsEverythingLoaded() && this.IsCurrentWorldHome())
						{
							this.GetCurrentCombineMeshHelper().BuildingObjectAdded(entity, this.meshCombiners);
						}
					}
				}
				else if (id != EventId.PostBuildingEntityKilled)
				{
					if (id != EventId.ClearableCleared)
					{
						if (id == EventId.StartupTasksCompleted)
						{
							this.isStartupTasksCompleted = true;
						}
					}
					else if (!this.IsFueInProgress())
					{
						this.GetCurrentCombineMeshHelper().BuildingObjectDestroyed(BuildingType.Clearable, this.meshCombiners, true);
					}
				}
				else
				{
					BuildingType buildingType = (BuildingType)cookie;
					if (!this.IsFueInProgress())
					{
						this.GetCurrentCombineMeshHelper().BuildingObjectDestroyed(buildingType, this.meshCombiners, true);
					}
				}
			}
			else if (id <= EventId.WorldLoadComplete)
			{
				switch (id)
				{
				case EventId.BuildingLevelUpgraded:
				case EventId.BuildingSwapped:
					if (!this.IsFueInProgress() && this.IsCurrentWorldHome())
					{
						SmartEntity buildingEntity = (SmartEntity)((ContractEventData)cookie).Entity;
						this.GetCurrentCombineMeshHelper().BuildingObjectDestroyed(buildingEntity, this.meshCombiners, false);
					}
					break;
				case EventId.BuildingConstructed:
					break;
				default:
					if (id == EventId.WorldLoadComplete)
					{
						if ((!this.IsCurrentWorldHome() || this.isStartupTasksCompleted) && !this.IsCurrentWorldUserWarBase())
						{
							this.CombineAllMeshTypes();
						}
					}
					break;
				}
			}
			else if (id != EventId.WorldReset)
			{
				if (id != EventId.GameStateChanged)
				{
					if (id == EventId.ShaderResetOnEntity)
					{
						if ((!this.IsCurrentWorldHome() || this.isStartupTasksCompleted) && !this.IsCurrentWorldUserWarBase())
						{
							this.CombineAllMeshTypes();
						}
					}
				}
				else
				{
					IState currentState = Service.Get<GameStateMachine>().CurrentState;
					Type previousStateType = (Type)cookie;
					if (currentState is HomeState)
					{
						if (!this.isStartupTasksCompleted)
						{
							this.CombineAllMeshTypes();
						}
						else if (this.IsPreviousStateEditMode(previousStateType))
						{
							this.CombineAllMeshTypes();
						}
					}
					else if (this.DidJustTransitionFromHomeToEditState(previousStateType, currentState))
					{
						this.UncombineAllMeshTypes(false);
					}
				}
			}
			else if (this.isSomeMeshesCombined)
			{
				this.UncombineAllMeshTypes(true);
			}
			return EatResponse.NotEaten;
		}

		private bool IsPreviousStateEditMode(Type previousStateType)
		{
			return previousStateType == typeof(EditBaseState);
		}

		private bool DidJustTransitionFromHomeToEditState(Type previousStateType, IState currentState)
		{
			return previousStateType == typeof(HomeState) && currentState is EditBaseState;
		}

		public bool IsRendererCombined(SmartEntity entity, Renderer renderer)
		{
			if (entity.BuildingComp != null)
			{
				BuildingType buildingTypeFromBuilding = this.GetBuildingTypeFromBuilding(entity);
				MeshCombiner meshCombiner;
				if (this.meshCombiners.TryGetValue(buildingTypeFromBuilding, out meshCombiner))
				{
					return meshCombiner.IsRendererCombined(renderer);
				}
			}
			return false;
		}

		private void CombineAllMeshTypes()
		{
			if (!this.IsFueInProgress())
			{
				this.isSomeMeshesCombined = true;
				this.GetCurrentCombineMeshHelper().CombineAllMeshTypes(this.meshCombiners);
			}
		}

		private void UncombineAllMeshTypes(bool resetPool)
		{
			if (!this.IsFueInProgress())
			{
				this.isSomeMeshesCombined = false;
				this.GetCurrentCombineMeshHelper().UncombineAllMeshTypes(this.meshCombiners);
				if (resetPool)
				{
					this.meshCombinerGameObjectPool.ClearOutPool();
				}
			}
		}

		private bool IsFueInProgress()
		{
			return Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress;
		}

		private bool IsCurrentWorldHome()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			return Service.Get<WorldTransitioner>().IsCurrentWorldHome();
		}

		private bool IsCurrentWorldUserWarBase()
		{
			return Service.Get<WorldTransitioner>().IsCurrentWorldUserWarBase();
		}

		private AbstractCombineMeshHelper GetCurrentCombineMeshHelper()
		{
			if (this.IsCurrentWorldHome())
			{
				return this.homeBaseCobmbineMeshHelper;
			}
			return this.battleBaseCombineMeshHelper;
		}

		private BuildingType GetBuildingTypeFromBuilding(SmartEntity smartEntity)
		{
			return smartEntity.BuildingComp.BuildingType.Type;
		}

		protected internal CombineMeshManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CombineMeshManager)GCHandledObjects.GCHandleToObject(instance)).CombineAllMeshTypes();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombineMeshManager)GCHandledObjects.GCHandleToObject(instance)).DidJustTransitionFromHomeToEditState((Type)GCHandledObjects.GCHandleToObject(*args), (IState)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombineMeshManager)GCHandledObjects.GCHandleToObject(instance)).GetBuildingTypeFromBuilding((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombineMeshManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrentCombineMeshHelper());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombineMeshManager)GCHandledObjects.GCHandleToObject(instance)).IsCurrentWorldHome());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombineMeshManager)GCHandledObjects.GCHandleToObject(instance)).IsCurrentWorldUserWarBase());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombineMeshManager)GCHandledObjects.GCHandleToObject(instance)).IsFueInProgress());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombineMeshManager)GCHandledObjects.GCHandleToObject(instance)).IsPreviousStateEditMode((Type)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombineMeshManager)GCHandledObjects.GCHandleToObject(instance)).IsRendererCombined((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (Renderer)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CombineMeshManager)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((CombineMeshManager)GCHandledObjects.GCHandleToObject(instance)).UncombineAllMeshTypes(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
