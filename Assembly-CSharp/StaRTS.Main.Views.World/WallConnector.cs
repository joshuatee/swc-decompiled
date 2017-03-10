using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.World
{
	public class WallConnector : IEventObserver
	{
		public WallConnector()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.BuildingPurchaseSuccess);
			eventManager.RegisterObserver(this, EventId.BuildingReplaced);
			eventManager.RegisterObserver(this, EventId.PreEntityKilled);
			eventManager.RegisterObserver(this, EventId.UserLiftedBuilding);
			eventManager.RegisterObserver(this, EventId.UserLoweredBuilding);
			eventManager.RegisterObserver(this, EventId.UserStashedBuilding);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.BuildingReplaced)
			{
				if (id == EventId.BuildingPurchaseSuccess)
				{
					goto IL_4C;
				}
				if (id != EventId.PreEntityKilled)
				{
					if (id != EventId.BuildingReplaced)
					{
						return EatResponse.NotEaten;
					}
					goto IL_4C;
				}
			}
			else
			{
				if (id == EventId.UserLiftedBuilding)
				{
					this.DisconnectAllNeighbors(cookie as Entity, false);
					return EatResponse.NotEaten;
				}
				if (id == EventId.UserLoweredBuilding)
				{
					goto IL_4C;
				}
				if (id != EventId.UserStashedBuilding)
				{
					return EatResponse.NotEaten;
				}
			}
			this.DisconnectAllNeighbors(cookie as Entity, true);
			return EatResponse.NotEaten;
			IL_4C:
			this.ConnectAllNeighbors(cookie as Entity);
			return EatResponse.NotEaten;
		}

		private BoardCell<Entity> CanConnect(Entity wall)
		{
			if (wall == null)
			{
				return null;
			}
			BuildingComponent buildingComponent = wall.Get<BuildingComponent>();
			if (buildingComponent == null)
			{
				return null;
			}
			BuildingTypeVO buildingType = buildingComponent.BuildingType;
			if (buildingType == null)
			{
				return null;
			}
			if (buildingType.Connectors == null)
			{
				return null;
			}
			BoardItemComponent boardItemComponent = wall.Get<BoardItemComponent>();
			if (boardItemComponent == null)
			{
				return null;
			}
			return boardItemComponent.BoardItem.CurrentCell;
		}

		public string GetConnectorAssetName(Entity wall, bool ignoreNE, bool ignoreNW)
		{
			BoardCell<Entity> boardCell = this.CanConnect(wall);
			if (boardCell == null)
			{
				return null;
			}
			Entity entity = ignoreNE ? null : this.GetWallGridNeighbor(boardCell, 1, 0);
			Entity entity2 = ignoreNW ? null : this.GetWallGridNeighbor(boardCell, 0, 1);
			if (entity != null && entity2 != null)
			{
				return wall.Get<BuildingComponent>().BuildingType.Connectors.AssetNameBoth;
			}
			if (entity != null)
			{
				return wall.Get<BuildingComponent>().BuildingType.Connectors.AssetNameNE;
			}
			if (entity2 != null)
			{
				return wall.Get<BuildingComponent>().BuildingType.Connectors.AssetNameNW;
			}
			return wall.Get<BuildingComponent>().BuildingType.AssetName;
		}

		private void LoadWallAssetExplicit(Entity wall, bool ignoreNE, bool ignoreNW)
		{
			string text;
			if (!ignoreNE && !ignoreNW)
			{
				text = wall.Get<BuildingComponent>().BuildingType.Connectors.AssetNameBoth;
			}
			else if (!ignoreNE)
			{
				text = wall.Get<BuildingComponent>().BuildingType.Connectors.AssetNameNE;
			}
			else if (!ignoreNW)
			{
				text = wall.Get<BuildingComponent>().BuildingType.Connectors.AssetNameNW;
			}
			else
			{
				text = wall.Get<BuildingComponent>().BuildingType.AssetName;
			}
			if (text != null)
			{
				Service.Get<EntityViewManager>().LoadEntityAsset(wall, text, true);
			}
		}

		private void LoadWallAsset(Entity wall, bool ignoreNE, bool ignoreNW)
		{
			string connectorAssetName = this.GetConnectorAssetName(wall, ignoreNE, ignoreNW);
			if (connectorAssetName != null)
			{
				Service.Get<EntityViewManager>().LoadEntityAsset(wall, connectorAssetName, true);
			}
		}

		private Entity GetNeighborFromTransform(Entity wall, List<Entity> wallList, int x, int z)
		{
			int x2 = wall.Get<TransformComponent>().X;
			int z2 = wall.Get<TransformComponent>().Z;
			for (int i = 0; i < wallList.Count; i++)
			{
				int x3 = wallList[i].Get<TransformComponent>().X;
				int z3 = wallList[i].Get<TransformComponent>().Z;
				if (x2 + x == x3 && z2 + z == z3)
				{
					return wallList[i];
				}
			}
			return null;
		}

		public void ConnectWallsInExclusiveSet(List<Entity> wallList, bool connectWithSetOnly)
		{
			for (int i = 0; i < wallList.Count; i++)
			{
				Entity wall = wallList[i];
				BoardCell<Entity> boardCell = this.CanConnect(wall);
				if (boardCell != null)
				{
					if (connectWithSetOnly)
					{
						bool ignoreNE = !wallList.Contains(this.GetNeighborFromTransform(wall, wallList, 1, 0));
						bool ignoreNW = !wallList.Contains(this.GetNeighborFromTransform(wall, wallList, 0, 1));
						this.LoadWallAssetExplicit(wall, ignoreNE, ignoreNW);
					}
					else
					{
						bool ignoreNE2 = !wallList.Contains(this.GetWallGridNeighbor(boardCell, 1, 0));
						bool ignoreNW2 = !wallList.Contains(this.GetWallGridNeighbor(boardCell, 0, 1));
						this.LoadWallAsset(wall, ignoreNE2, ignoreNW2);
						Entity wallGridNeighbor = this.GetWallGridNeighbor(boardCell, -1, 0);
						Entity wallGridNeighbor2 = this.GetWallGridNeighbor(boardCell, 0, -1);
						bool flag = wallGridNeighbor == null || wallList.Contains(wallGridNeighbor);
						bool flag2 = wallGridNeighbor2 == null || wallList.Contains(wallGridNeighbor2);
						if (!flag)
						{
							this.LoadWallAsset(wallGridNeighbor, true, false);
						}
						if (!flag2)
						{
							this.LoadWallAsset(wallGridNeighbor2, false, true);
						}
					}
				}
			}
		}

		private void ConnectAllNeighbors(Entity wall)
		{
			BoardCell<Entity> boardCell = this.CanConnect(wall);
			if (boardCell == null)
			{
				return;
			}
			this.LoadWallAsset(wall, false, false);
			Entity wallGridNeighbor = this.GetWallGridNeighbor(boardCell, -1, 0);
			Entity wallGridNeighbor2 = this.GetWallGridNeighbor(boardCell, 0, -1);
			this.LoadWallAsset(wallGridNeighbor, false, false);
			this.LoadWallAsset(wallGridNeighbor2, false, false);
		}

		private void DisconnectAllNeighbors(Entity wall, bool targetDestroyed)
		{
			BoardCell<Entity> boardCell = this.CanConnect(wall);
			if (boardCell == null)
			{
				return;
			}
			if (!targetDestroyed)
			{
				this.LoadWallAsset(wall, true, true);
			}
			Entity wallGridNeighbor = this.GetWallGridNeighbor(boardCell, -1, 0);
			Entity wallGridNeighbor2 = this.GetWallGridNeighbor(boardCell, 0, -1);
			this.LoadWallAsset(wallGridNeighbor, true, false);
			this.LoadWallAsset(wallGridNeighbor2, false, true);
		}

		private Entity GetWallGridNeighbor(BoardCell<Entity> cell, int gridDeltaX, int gridDeltaZ)
		{
			int num = Units.GridToBoardX(gridDeltaX);
			int num2 = Units.GridToBoardZ(gridDeltaZ);
			BoardCell<Entity> cellAt = cell.ParentBoard.GetCellAt(cell.X + num, cell.Z + num2);
			if (cellAt != null && cellAt.Children != null)
			{
				using (IEnumerator<BoardItem<Entity>> enumerator = cellAt.Children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BoardItem<Entity> current = enumerator.get_Current();
						Entity data = current.Data;
						BuildingComponent buildingComponent = data.Get<BuildingComponent>();
						if (buildingComponent != null && buildingComponent.BuildingType.Connectors != null)
						{
							return current.Data;
						}
					}
				}
			}
			return null;
		}

		public List<Entity> GetWallChains(Entity rootWall, int xDir, int zDir)
		{
			List<Entity> list = new List<Entity>();
			BoardCell<Entity> boardCell = this.CanConnect(rootWall);
			BoardCell<Entity> cell = boardCell;
			Entity entity = rootWall;
			while (cell != null)
			{
				if (entity != rootWall)
				{
					list.Add(entity);
				}
				entity = this.GetWallGridNeighbor(cell, xDir, zDir);
				cell = this.CanConnect(entity);
			}
			return list;
		}

		protected internal WallConnector(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WallConnector)GCHandledObjects.GCHandleToObject(instance)).CanConnect((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WallConnector)GCHandledObjects.GCHandleToObject(instance)).ConnectAllNeighbors((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WallConnector)GCHandledObjects.GCHandleToObject(instance)).ConnectWallsInExclusiveSet((List<Entity>)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((WallConnector)GCHandledObjects.GCHandleToObject(instance)).DisconnectAllNeighbors((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WallConnector)GCHandledObjects.GCHandleToObject(instance)).GetConnectorAssetName((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WallConnector)GCHandledObjects.GCHandleToObject(instance)).GetNeighborFromTransform((Entity)GCHandledObjects.GCHandleToObject(*args), (List<Entity>)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2), *(int*)(args + 3)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WallConnector)GCHandledObjects.GCHandleToObject(instance)).GetWallChains((Entity)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WallConnector)GCHandledObjects.GCHandleToObject(instance)).GetWallGridNeighbor((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((WallConnector)GCHandledObjects.GCHandleToObject(instance)).LoadWallAsset((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((WallConnector)GCHandledObjects.GCHandleToObject(instance)).LoadWallAssetExplicit((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WallConnector)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
