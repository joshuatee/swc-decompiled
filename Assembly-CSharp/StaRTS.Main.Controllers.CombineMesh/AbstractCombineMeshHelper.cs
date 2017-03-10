using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Utils.Core;
using StaRTS.Utils.MeshCombiner;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.CombineMesh
{
	public abstract class AbstractCombineMeshHelper
	{
		private const string FLAG_MESH = "flagMesh";

		public abstract HashSet<BuildingType> GetEligibleBuildingTypes();

		protected abstract List<Entity> GetBuildingEntityListByType(BuildingType buidlingType);

		private bool IsEntityEligible(SmartEntity smartEntity)
		{
			if (smartEntity.BuildingComp != null)
			{
				BuildingType buildingTypeFromBuilding = this.GetBuildingTypeFromBuilding(smartEntity);
				return this.IsBuildingTypeEligible(buildingTypeFromBuilding) && this.IsEntityEligibleForEligibleBuildingType(smartEntity);
			}
			return false;
		}

		protected virtual bool IsEntityEligibleForEligibleBuildingType(SmartEntity smartEntity)
		{
			return true;
		}

		private bool IsBuildingTypeEligible(BuildingType buildingType)
		{
			return this.GetEligibleBuildingTypes().Contains(buildingType);
		}

		protected BuildingType GetBuildingTypeFromBuilding(SmartEntity smartEntity)
		{
			return smartEntity.BuildingComp.BuildingType.Type;
		}

		private void InternalBuildingObjectDestroyed(BuildingType buildingType, Dictionary<BuildingType, MeshCombiner> meshCombiners, bool noFutureAddCallExpected)
		{
			if (noFutureAddCallExpected)
			{
				this.CombineMesh(buildingType, meshCombiners[buildingType]);
				return;
			}
			this.UncombineMesh(buildingType, meshCombiners[buildingType]);
		}

		public void BuildingObjectDestroyed(BuildingType buildingType, Dictionary<BuildingType, MeshCombiner> meshCombiners, bool noFutureAddCallExpected)
		{
			if (this.IsBuildingTypeEligible(buildingType))
			{
				this.InternalBuildingObjectDestroyed(buildingType, meshCombiners, noFutureAddCallExpected);
			}
		}

		public void BuildingObjectDestroyed(SmartEntity buildingEntity, Dictionary<BuildingType, MeshCombiner> meshCombiners, bool noFutureAddCallExpected)
		{
			if (this.IsEntityEligible(buildingEntity))
			{
				BuildingType buildingTypeFromBuilding = this.GetBuildingTypeFromBuilding(buildingEntity);
				this.InternalBuildingObjectDestroyed(buildingTypeFromBuilding, meshCombiners, noFutureAddCallExpected);
			}
		}

		public void BuildingObjectAdded(SmartEntity buildingEntity, Dictionary<BuildingType, MeshCombiner> meshCombiners)
		{
			if (this.IsEntityEligible(buildingEntity))
			{
				BuildingType buildingTypeFromBuilding = this.GetBuildingTypeFromBuilding(buildingEntity);
				this.CombineMesh(buildingTypeFromBuilding, meshCombiners[buildingTypeFromBuilding]);
			}
		}

		public virtual void CombineAllMeshTypes(Dictionary<BuildingType, MeshCombiner> meshCombiners)
		{
			Dictionary<BuildingType, HashSet<Renderer>> allRenderersToCombine = this.GetAllRenderersToCombine();
			foreach (KeyValuePair<BuildingType, HashSet<Renderer>> current in allRenderersToCombine)
			{
				BuildingType key = current.get_Key();
				meshCombiners[key].CombineMeshes(current.get_Value());
			}
		}

		public virtual void UncombineAllMeshTypes(Dictionary<BuildingType, MeshCombiner> meshCombiners)
		{
			foreach (KeyValuePair<BuildingType, MeshCombiner> current in meshCombiners)
			{
				if (current.get_Value() != null)
				{
					current.get_Value().UncombineMesh();
				}
			}
		}

		protected virtual void CombineMesh(BuildingType buildingType, MeshCombiner meshCombiner)
		{
			meshCombiner.CombineMeshes(this.GetRenderersToCombine(buildingType));
		}

		protected virtual void UncombineMesh(BuildingType buildingType, MeshCombiner meshCombiner)
		{
			meshCombiner.UncombineMesh();
		}

		private Dictionary<BuildingType, HashSet<Renderer>> GetAllRenderersToCombine()
		{
			EntityController entityController = Service.Get<EntityController>();
			Dictionary<BuildingType, HashSet<Renderer>> dictionary = new Dictionary<BuildingType, HashSet<Renderer>>();
			NodeList<BuildingNode> nodeList = entityController.GetNodeList<BuildingNode>();
			for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
			{
				BuildingType type = buildingNode.BuildingComp.BuildingType.Type;
				SmartEntity smartEntity = (SmartEntity)buildingNode.Entity;
				if (this.IsBuildingTypeEligible(type) && this.IsEntityEligibleForEligibleBuildingType(smartEntity))
				{
					HashSet<Renderer> hashSet;
					if (!dictionary.TryGetValue(type, out hashSet))
					{
						hashSet = new HashSet<Renderer>();
						dictionary[type] = hashSet;
					}
					this.AddRenderersFromEntity(hashSet, smartEntity);
				}
			}
			return dictionary;
		}

		private HashSet<Renderer> GetRenderersToCombine(BuildingType buildingType)
		{
			HashSet<Renderer> hashSet = new HashSet<Renderer>();
			List<Entity> buildingEntityListByType = this.GetBuildingEntityListByType(buildingType);
			int i = 0;
			int count = buildingEntityListByType.Count;
			while (i < count)
			{
				SmartEntity smartEntity = (SmartEntity)buildingEntityListByType[i];
				if (this.IsEntityEligibleForEligibleBuildingType(smartEntity))
				{
					this.AddRenderersFromEntity(hashSet, smartEntity);
				}
				i++;
			}
			return hashSet;
		}

		private void AddRenderFromGameObject(HashSet<Renderer> list, GameObject gameObject)
		{
			Renderer component = gameObject.GetComponent<Renderer>();
			list.Add(component);
		}

		protected void AddRenderersFromEntity(HashSet<Renderer> renderers, SmartEntity entity)
		{
			if (entity.GameObjectViewComp != null)
			{
				GameObject mainGameObject = entity.GameObjectViewComp.MainGameObject;
				AssetMeshDataMonoBehaviour component = mainGameObject.GetComponent<AssetMeshDataMonoBehaviour>();
				if (component != null)
				{
					List<GameObject> selectableGameObjects = component.SelectableGameObjects;
					int i = 0;
					int count = selectableGameObjects.Count;
					while (i < count)
					{
						GameObject gameObject = selectableGameObjects[i];
						if (!gameObject.name.StartsWith("flagMesh"))
						{
							this.AddRenderFromGameObject(renderers, gameObject);
						}
						i++;
					}
					this.AddRenderFromGameObject(renderers, component.ShadowGameObject);
				}
			}
		}

		protected AbstractCombineMeshHelper()
		{
		}

		protected internal AbstractCombineMeshHelper(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).AddRenderersFromEntity((HashSet<Renderer>)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).AddRenderFromGameObject((HashSet<Renderer>)GCHandledObjects.GCHandleToObject(*args), (GameObject)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).BuildingObjectAdded((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (Dictionary<BuildingType, MeshCombiner>)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).BuildingObjectDestroyed((BuildingType)(*(int*)args), (Dictionary<BuildingType, MeshCombiner>)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).BuildingObjectDestroyed((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (Dictionary<BuildingType, MeshCombiner>)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).CombineAllMeshTypes((Dictionary<BuildingType, MeshCombiner>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).CombineMesh((BuildingType)(*(int*)args), (MeshCombiner)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).GetAllRenderersToCombine());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).GetBuildingEntityListByType((BuildingType)(*(int*)args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).GetBuildingTypeFromBuilding((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).GetEligibleBuildingTypes());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).GetRenderersToCombine((BuildingType)(*(int*)args)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).InternalBuildingObjectDestroyed((BuildingType)(*(int*)args), (Dictionary<BuildingType, MeshCombiner>)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).IsBuildingTypeEligible((BuildingType)(*(int*)args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).IsEntityEligible((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).IsEntityEligibleForEligibleBuildingType((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).UncombineAllMeshTypes((Dictionary<BuildingType, MeshCombiner>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((AbstractCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).UncombineMesh((BuildingType)(*(int*)args), (MeshCombiner)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
