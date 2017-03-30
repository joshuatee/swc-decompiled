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
			switch (id)
			{
			case EventId.WorldLoadComplete:
				if ((!this.IsCurrentWorldHome() || this.isStartupTasksCompleted) && !this.IsCurrentWorldUserWarBase())
				{
					this.CombineAllMeshTypes();
				}
				return EatResponse.NotEaten;
			case EventId.WorldInTransitionComplete:
			case EventId.WorldOutTransitionComplete:
				IL_1D:
				switch (id)
				{
				case EventId.ClearableCleared:
					if (!this.IsFueInProgress())
					{
						this.GetCurrentCombineMeshHelper().BuildingObjectDestroyed(BuildingType.Clearable, this.meshCombiners, true);
					}
					return EatResponse.NotEaten;
				case EventId.ClearableStarted:
					IL_33:
					switch (id)
					{
					case EventId.BuildingLevelUpgraded:
					case EventId.BuildingSwapped:
						if (!this.IsFueInProgress() && this.IsCurrentWorldHome())
						{
							SmartEntity buildingEntity = (SmartEntity)((ContractEventData)cookie).Entity;
							this.GetCurrentCombineMeshHelper().BuildingObjectDestroyed(buildingEntity, this.meshCombiners, false);
						}
						return EatResponse.NotEaten;
					case EventId.BuildingConstructed:
						return EatResponse.NotEaten;
					default:
						if (id == EventId.BuildingViewReady || id == EventId.BuildingViewFailed)
						{
							SmartEntity entity = ((EntityViewParams)cookie).Entity;
							if (!this.IsFueInProgress() && this.isStartupTasksCompleted && Service.Get<WorldTransitioner>().IsEverythingLoaded() && this.IsCurrentWorldHome())
							{
								this.GetCurrentCombineMeshHelper().BuildingObjectAdded(entity, this.meshCombiners);
							}
							return EatResponse.NotEaten;
						}
						if (id == EventId.PostBuildingEntityKilled)
						{
							BuildingType buildingType = (BuildingType)((int)cookie);
							if (!this.IsFueInProgress())
							{
								this.GetCurrentCombineMeshHelper().BuildingObjectDestroyed(buildingType, this.meshCombiners, true);
							}
							return EatResponse.NotEaten;
						}
						if (id == EventId.GameStateChanged)
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
							return EatResponse.NotEaten;
						}
						if (id != EventId.ShaderResetOnEntity)
						{
							return EatResponse.NotEaten;
						}
						if ((!this.IsCurrentWorldHome() || this.isStartupTasksCompleted) && !this.IsCurrentWorldUserWarBase())
						{
							this.CombineAllMeshTypes();
						}
						return EatResponse.NotEaten;
					}
					break;
				case EventId.StartupTasksCompleted:
					this.isStartupTasksCompleted = true;
					return EatResponse.NotEaten;
				}
				goto IL_33;
			case EventId.WorldReset:
				if (this.isSomeMeshesCombined)
				{
					this.UncombineAllMeshTypes(true);
				}
				return EatResponse.NotEaten;
			}
			goto IL_1D;
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
	}
}
