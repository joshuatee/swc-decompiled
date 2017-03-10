using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Views.World;
using StaRTS.Utils.Core;
using StaRTS.Utils.MeshCombiner;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.CombineMesh
{
	public class HomeBaseCombineMeshHelper : AbstractCombineMeshHelper
	{
		private readonly HashSet<BuildingType> HOME_SUPPORTED_BUILDING_TYPES;

		public override HashSet<BuildingType> GetEligibleBuildingTypes()
		{
			return this.HOME_SUPPORTED_BUILDING_TYPES;
		}

		private bool IsHomeCombiningEligible()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			return !(currentState is EditBaseState) && !(currentState is BaseLayoutToolState);
		}

		public override void CombineAllMeshTypes(Dictionary<BuildingType, MeshCombiner> meshCombiners)
		{
			if (this.IsHomeCombiningEligible())
			{
				Entity selectedBuilding = null;
				Vector3 zero = Vector3.zero;
				BuildingMover buildingMoverForCombineMeshManager = Service.Get<BuildingController>().GetBuildingMoverForCombineMeshManager();
				buildingMoverForCombineMeshManager.DeselectBuildingBeforeCombiningMesh(out selectedBuilding, out zero);
				base.CombineAllMeshTypes(meshCombiners);
				buildingMoverForCombineMeshManager.SelectBuildingAfterCombiningMesh(selectedBuilding, zero);
			}
		}

		protected override void CombineMesh(BuildingType buildingType, MeshCombiner meshCombiner)
		{
			if (this.IsHomeCombiningEligible())
			{
				BuildingController buildingController = Service.Get<BuildingController>();
				Entity selectedBuilding = buildingController.SelectedBuilding;
				if (selectedBuilding != null)
				{
					BuildingType buildingTypeFromBuilding = base.GetBuildingTypeFromBuilding((SmartEntity)selectedBuilding);
					BuildingMover buildingMoverForCombineMeshManager = Service.Get<BuildingController>().GetBuildingMoverForCombineMeshManager();
					Vector3 zero = Vector3.zero;
					if (buildingTypeFromBuilding == buildingType)
					{
						buildingMoverForCombineMeshManager.DeselectBuildingBeforeCombiningMesh(out selectedBuilding, out zero);
					}
					base.CombineMesh(buildingType, meshCombiner);
					if (buildingTypeFromBuilding == buildingType)
					{
						buildingMoverForCombineMeshManager.SelectBuildingAfterCombiningMesh(selectedBuilding, zero);
						return;
					}
				}
				else
				{
					base.CombineMesh(buildingType, meshCombiner);
				}
			}
		}

		protected override void UncombineMesh(BuildingType buildingType, MeshCombiner meshCombiner)
		{
			if (this.IsHomeCombiningEligible())
			{
				base.UncombineMesh(buildingType, meshCombiner);
			}
		}

		protected override bool IsEntityEligibleForEligibleBuildingType(SmartEntity smartEntity)
		{
			bool result = true;
			BuildingType buildingTypeFromBuilding = base.GetBuildingTypeFromBuilding(smartEntity);
			if (buildingTypeFromBuilding == BuildingType.Storage)
			{
				result = this.IsStorageBuildingEligibleForHomeCombineMesh(smartEntity);
			}
			return result;
		}

		protected override List<Entity> GetBuildingEntityListByType(BuildingType buildingType)
		{
			return Service.Get<BuildingLookupController>().GetBuildingListByType(buildingType);
		}

		private bool IsStorageBuildingEligibleForHomeCombineMesh(SmartEntity entity)
		{
			return entity.BuildingComp.BuildingType.Currency != CurrencyType.Credits;
		}

		public HomeBaseCombineMeshHelper()
		{
			this.HOME_SUPPORTED_BUILDING_TYPES = new HashSet<BuildingType>(new BuildingType[]
			{
				BuildingType.Starport,
				BuildingType.Wall,
				BuildingType.Storage,
				BuildingType.ShieldGenerator,
				BuildingType.Clearable
			});
			base..ctor();
		}

		protected internal HomeBaseCombineMeshHelper(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HomeBaseCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).CombineAllMeshTypes((Dictionary<BuildingType, MeshCombiner>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((HomeBaseCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).CombineMesh((BuildingType)(*(int*)args), (MeshCombiner)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeBaseCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).GetBuildingEntityListByType((BuildingType)(*(int*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeBaseCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).GetEligibleBuildingTypes());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeBaseCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).IsEntityEligibleForEligibleBuildingType((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeBaseCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).IsHomeCombiningEligible());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HomeBaseCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).IsStorageBuildingEligibleForHomeCombineMesh((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((HomeBaseCombineMeshHelper)GCHandledObjects.GCHandleToObject(instance)).UncombineMesh((BuildingType)(*(int*)args), (MeshCombiner)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
