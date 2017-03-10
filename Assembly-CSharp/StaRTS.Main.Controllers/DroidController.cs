using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.GameBoard;
using StaRTS.GameBoard.Pathfinding;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class DroidController : IEventObserver
	{
		private string DROID_UID;

		private EntityController entityController;

		public DroidController()
		{
			this.DROID_UID = "civilianWorkerDroid01";
			base..ctor();
			this.entityController = Service.Get<EntityController>();
			Service.Set<DroidController>(this);
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.DroidViewReady, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.InventoryResourceUpdated, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ClearableStarted, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ClearableCleared, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ChampionRepaired, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingPurchaseSuccess, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingConstructed, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingLevelUpgraded, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingStartedUpgrading, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ChampionStartedRepairing, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingSwapped, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingCancelled, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.TroopCancelled, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BattleLoadStart, EventPriority.Default);
		}

		public Entity GetDroidHut()
		{
			NodeList<DroidHutNode> nodeList = this.entityController.GetNodeList<DroidHutNode>();
			DroidHutNode head = nodeList.Head;
			if (head == null)
			{
				return null;
			}
			return head.Entity;
		}

		private Entity CreateDroid(CivilianTypeVO droidType)
		{
			Entity droidHut = this.GetDroidHut();
			if (droidHut == null)
			{
				return null;
			}
			TransformComponent transformComponent = droidHut.Get<TransformComponent>();
			IntPosition intPosition = new IntPosition(transformComponent.X - 1, transformComponent.Z - 1);
			Entity entity = Service.Get<EntityFactory>().CreateDroidEntity(droidType, intPosition);
			BoardItemComponent boardItemComponent = entity.Get<BoardItemComponent>();
			Service.Get<BoardController>().Board.AddChild(boardItemComponent.BoardItem, intPosition.x, intPosition.z, null, false);
			Service.Get<EntityController>().AddEntity(entity);
			return entity;
		}

		public void DestroyDroid(DroidNode droid)
		{
			Service.Get<EntityFactory>().DestroyEntity(droid.Entity, true, false);
		}

		private void DestroyAllDroids()
		{
			List<DroidNode> list = new List<DroidNode>();
			NodeList<DroidNode> nodeList = Service.Get<EntityController>().GetNodeList<DroidNode>();
			for (DroidNode droidNode = nodeList.Head; droidNode != null; droidNode = droidNode.Next)
			{
				list.Add(droidNode);
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.DestroyDroid(list[i]);
			}
		}

		public void HideAllNonClearableDroids()
		{
			List<DroidNode> list = new List<DroidNode>();
			NodeList<DroidNode> nodeList = Service.Get<EntityController>().GetNodeList<DroidNode>();
			for (DroidNode droidNode = nodeList.Head; droidNode != null; droidNode = droidNode.Next)
			{
				BuildingType type = droidNode.Droid.Target.Get<BuildingComponent>().BuildingType.Type;
				if (type != BuildingType.Clearable)
				{
					list.Add(droidNode);
				}
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].Entity.Get<GameObjectViewComponent>().MainGameObject.SetActive(false);
			}
		}

		public void ShowAllDroids()
		{
			NodeList<DroidNode> nodeList = Service.Get<EntityController>().GetNodeList<DroidNode>();
			for (DroidNode droidNode = nodeList.Head; droidNode != null; droidNode = droidNode.Next)
			{
				droidNode.Entity.Get<GameObjectViewComponent>().MainGameObject.SetActive(true);
				this.AssignDroidPath(droidNode);
			}
		}

		private Entity FindIdleDroid(CivilianTypeVO droidType)
		{
			Entity droidHut = this.GetDroidHut();
			NodeList<DroidNode> nodeList = Service.Get<EntityController>().GetNodeList<DroidNode>();
			for (DroidNode droidNode = nodeList.Head; droidNode != null; droidNode = droidNode.Next)
			{
				if (droidNode.Droid.Target == droidHut || droidNode.Droid.Target == null)
				{
					return droidNode.Entity;
				}
			}
			return this.CreateDroid(droidType);
		}

		public bool UpdateDroidTransform(DroidNode droid, float dt)
		{
			if (droid != null && droid.IsValid() && droid.Entity != null)
			{
				PathingComponent pathingComponent = droid.Entity.Get<PathingComponent>();
				if (pathingComponent != null)
				{
					if ((ulong)pathingComponent.TimeOnSegment > (ulong)((long)pathingComponent.TimeToMove) && this.ShouldCalculatePath(droid))
					{
						droid.State.CurState = EntityState.Moving;
						this.AssignDroidPath(droid);
						return false;
					}
					return pathingComponent.CurrentPath == null;
				}
			}
			return false;
		}

		private bool ShouldCalculatePath(DroidNode node)
		{
			TransformComponent transformComponent = node.Droid.Target.Get<TransformComponent>();
			if (transformComponent == null)
			{
				return false;
			}
			PathingComponent pathingComponent = node.Entity.Get<PathingComponent>();
			if (pathingComponent == null)
			{
				return false;
			}
			Path currentPath = pathingComponent.CurrentPath;
			if (currentPath == null)
			{
				return false;
			}
			if (currentPath.TurnCount == 0)
			{
				return false;
			}
			BoardCell<Entity> turn = currentPath.GetTurn(currentPath.TurnCount - 1);
			int num = turn.X - transformComponent.X;
			int num2 = turn.Z - transformComponent.Z;
			return num < -1 || num > transformComponent.BoardWidth || num2 < -1 || num2 > transformComponent.BoardDepth;
		}

		public void AssignDroidPath(DroidNode droid)
		{
			if (droid != null && droid.IsValid())
			{
				int x = droid.Transform.X;
				int z = droid.Transform.Z;
				BoardController boardController = Service.Get<BoardController>();
				BoardCell<Entity> startCell = boardController.Board.GetCellAt(x, z);
				SmartEntity smartEntity = (SmartEntity)droid.Droid.Target;
				TransformComponent transformComp = smartEntity.TransformComp;
				if (transformComp == null)
				{
					return;
				}
				int num = transformComp.X - 1;
				int num2 = transformComp.Z - 1;
				int num3 = Service.Get<Rand>().ViewRangeInt(0, transformComp.BoardWidth + transformComp.BoardDepth + 1);
				if (num3 <= transformComp.BoardWidth)
				{
					num += num3;
				}
				else
				{
					num2 += num3 - transformComp.BoardWidth;
				}
				BoardCell<Entity> cellAt = boardController.Board.GetCellAt(num, num2);
				if (!droid.Droid.AnimateTravel)
				{
					startCell = cellAt;
				}
				if (cellAt != null)
				{
					Service.Get<PathingManager>().StartPathingWorkerOrPatrol((SmartEntity)droid.Entity, smartEntity, startCell, cellAt, droid.Size.Width, true);
				}
				droid.Droid.AnimateTravel = true;
			}
		}

		private void AssignWorkToDroid(Entity building, bool hasScaffolding, bool forceCreate, bool animateTravel)
		{
			CivilianTypeVO droidType = Service.Get<IDataController>().Get<CivilianTypeVO>(this.DROID_UID);
			Entity entity = forceCreate ? this.CreateDroid(droidType) : this.FindIdleDroid(droidType);
			if (entity == null || building == null)
			{
				return;
			}
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			if (buildingComponent == null || buildingComponent.BuildingType.Type == BuildingType.Wall)
			{
				return;
			}
			entity.Get<DroidComponent>().AnimateTravel = animateTravel;
			entity.Get<DroidComponent>().Target = building;
		}

		private void RemoveWorkFromDroid(Entity building, bool hasScaffolding)
		{
			NodeList<DroidNode> nodeList = Service.Get<EntityController>().GetNodeList<DroidNode>();
			for (DroidNode droidNode = nodeList.Head; droidNode != null; droidNode = droidNode.Next)
			{
				if (droidNode.Droid.Target == building)
				{
					droidNode.Droid.AnimateTravel = true;
					droidNode.Droid.Target = this.GetDroidHut();
					droidNode.State.CurState = EntityState.Moving;
					this.AssignDroidPath(droidNode);
					return;
				}
			}
		}

		private void InitializeDroids()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			int num = currentPlayer.CurrentDroidsAmount;
			NodeList<SupportViewNode> nodeList = Service.Get<EntityController>().GetNodeList<SupportViewNode>();
			ISupportController supportController = Service.Get<ISupportController>();
			for (SupportViewNode supportViewNode = nodeList.Head; supportViewNode != null; supportViewNode = supportViewNode.Next)
			{
				Contract contract = supportController.FindCurrentContract(supportViewNode.Entity.Get<BuildingComponent>().BuildingTO.Key);
				if (contract != null)
				{
					DeliveryType deliveryType = contract.DeliveryType;
					if (deliveryType == DeliveryType.SwapBuilding || deliveryType == DeliveryType.UpgradeBuilding || deliveryType == DeliveryType.Building)
					{
						this.AssignWorkToDroid(supportViewNode.Entity, true, true, false);
						num--;
					}
					else if (deliveryType == DeliveryType.ClearClearable || deliveryType == DeliveryType.Champion)
					{
						this.AssignWorkToDroid(supportViewNode.Entity, false, true, false);
						num--;
					}
				}
			}
		}

		private void PrepareDroid(Entity droidEntity)
		{
			Entity droidHut = this.GetDroidHut();
			if (droidHut == null)
			{
				Service.Get<EntityFactory>().DestroyEntity(droidEntity, true, false);
				return;
			}
			DroidComponent droidComponent = droidEntity.Get<DroidComponent>();
			if (droidComponent.Target == null)
			{
				droidComponent.Target = droidHut;
			}
		}

		public virtual EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.ClearableStarted)
			{
				if (id <= EventId.BuildingPurchaseSuccess)
				{
					switch (id)
					{
					case EventId.DroidViewReady:
					{
						EntityViewParams entityViewParams = cookie as EntityViewParams;
						this.PrepareDroid(entityViewParams.Entity);
						return EatResponse.NotEaten;
					}
					case EventId.BuildingViewReady:
					case EventId.BuildingViewFailed:
						return EatResponse.NotEaten;
					case EventId.BuildingCancelled:
					case EventId.TroopCancelled:
					{
						ContractEventData contractEventData = (ContractEventData)cookie;
						ContractType contractType = ContractUtils.GetContractType(contractEventData.Contract.DeliveryType);
						if (ContractUtils.ContractTypeConsumesDroid(contractType))
						{
							SmartEntity smartEntity = (SmartEntity)contractEventData.Entity;
							BuildingComponent buildingComp = smartEntity.BuildingComp;
							this.RemoveWorkFromDroid(smartEntity, buildingComp.BuildingType.Type != BuildingType.Clearable);
							return EatResponse.NotEaten;
						}
						return EatResponse.NotEaten;
					}
					default:
						if (id != EventId.BuildingPurchaseSuccess)
						{
							return EatResponse.NotEaten;
						}
						break;
					}
				}
				else if (id != EventId.BuildingStartedUpgrading)
				{
					if (id == EventId.ClearableCleared)
					{
						goto IL_148;
					}
					if (id != EventId.ClearableStarted)
					{
						return EatResponse.NotEaten;
					}
				}
			}
			else if (id <= EventId.BuildingConstructed)
			{
				if (id != EventId.ChampionStartedRepairing)
				{
					if (id == EventId.ChampionRepaired)
					{
						goto IL_148;
					}
					switch (id)
					{
					case EventId.BuildingLevelUpgraded:
					case EventId.BuildingSwapped:
					case EventId.BuildingConstructed:
						goto IL_148;
					default:
						return EatResponse.NotEaten;
					}
				}
			}
			else
			{
				if (id == EventId.WorldLoadComplete)
				{
					this.InitializeDroids();
					return EatResponse.NotEaten;
				}
				if (id == EventId.BattleLoadStart)
				{
					this.DestroyAllDroids();
					return EatResponse.NotEaten;
				}
				if (id != EventId.InventoryResourceUpdated)
				{
					return EatResponse.NotEaten;
				}
				if (object.Equals(cookie, "droids"))
				{
					this.AssignWorkToDroid(this.GetDroidHut(), false, true, true);
					return EatResponse.NotEaten;
				}
				return EatResponse.NotEaten;
			}
			Entity building = cookie as Entity;
			this.AssignWorkToDroid(building, false, false, true);
			return EatResponse.NotEaten;
			IL_148:
			ContractEventData contractEventData2 = cookie as ContractEventData;
			this.RemoveWorkFromDroid(contractEventData2.Entity, id != EventId.ClearableCleared);
			return EatResponse.NotEaten;
		}

		protected internal DroidController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DroidController)GCHandledObjects.GCHandleToObject(instance)).AssignDroidPath((DroidNode)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DroidController)GCHandledObjects.GCHandleToObject(instance)).AssignWorkToDroid((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidController)GCHandledObjects.GCHandleToObject(instance)).CreateDroid((CivilianTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DroidController)GCHandledObjects.GCHandleToObject(instance)).DestroyAllDroids();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((DroidController)GCHandledObjects.GCHandleToObject(instance)).DestroyDroid((DroidNode)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidController)GCHandledObjects.GCHandleToObject(instance)).FindIdleDroid((CivilianTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidController)GCHandledObjects.GCHandleToObject(instance)).GetDroidHut());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((DroidController)GCHandledObjects.GCHandleToObject(instance)).HideAllNonClearableDroids();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((DroidController)GCHandledObjects.GCHandleToObject(instance)).InitializeDroids();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((DroidController)GCHandledObjects.GCHandleToObject(instance)).PrepareDroid((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((DroidController)GCHandledObjects.GCHandleToObject(instance)).RemoveWorkFromDroid((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidController)GCHandledObjects.GCHandleToObject(instance)).ShouldCalculatePath((DroidNode)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((DroidController)GCHandledObjects.GCHandleToObject(instance)).ShowAllDroids();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidController)GCHandledObjects.GCHandleToObject(instance)).UpdateDroidTransform((DroidNode)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1)));
		}
	}
}
