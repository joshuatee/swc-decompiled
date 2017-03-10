using Net.RichardLord.Ash.Core;
using StaRTS.Externals.Manimal;
using StaRTS.FX;
using StaRTS.GameBoard;
using StaRTS.GameBoard.Components;
using StaRTS.Main.Configs;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player.Building.Move;
using StaRTS.Main.Models.Commands.TransferObjects;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.World
{
	public class WorldController
	{
		public const int DEFAULT_SPAWN_PROTECTION_SIZE = 1;

		public const int DEFAULT_WALKABLE_INSET_SIZE = 1;

		private EntityController entityController;

		public Entity DroidHut
		{
			get;
			private set;
		}

		public WorldController()
		{
			Service.Set<WorldController>(this);
			this.entityController = Service.Get<EntityController>();
		}

		public Entity ProcessWorldDataBuilding(Building building, out bool validPlacement)
		{
			return this.ProcessWorldDataBuilding(building, true, false, out validPlacement);
		}

		public Entity ProcessWorldDataBuilding(Building building, bool createCollider, bool requestAsset, out bool validPlacement)
		{
			Entity entity = Service.Get<EntityFactory>().CreateBuildingEntity(building, createCollider, requestAsset, Service.Get<WorldTransitioner>().IsCurrentWorldHome());
			int x = Units.GridToBoardX(building.X);
			int z = Units.GridToBoardX(building.Z);
			BoardCell<Entity> boardCell = this.AddBuildingHelper(entity, x, z, false);
			validPlacement = (boardCell != null);
			return entity;
		}

		public BoardCell<Entity> AddBuildingHelper(Entity building, int x, int z, bool isUpgrade)
		{
			BoardCell<Entity> boardCell = this.AddBuildingToBoard(building, x, z, false);
			if (boardCell == null)
			{
				return null;
			}
			this.AddEntityToWorld(building);
			if (!isUpgrade)
			{
				Service.Get<EventManager>().SendEvent(EventId.BuildingPlacedOnBoard, building);
			}
			return boardCell;
		}

		public void AddEntityToWorld(Entity entity)
		{
			this.entityController.AddEntity(entity);
		}

		public BoardCell<Entity> AddBuildingToBoard(Entity building, int boardX, int boardZ, bool sendEvent)
		{
			BoardItemComponent boardItemComponent = building.Get<BoardItemComponent>();
			BoardItem<Entity> boardItem = boardItemComponent.BoardItem;
			SizeComponent size = boardItem.Size;
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			BuildingTypeVO buildingType = buildingComponent.BuildingType;
			bool flag = buildingType.Type == BuildingType.Clearable || buildingType.Type == BuildingType.Trap || buildingType.Type == BuildingType.ChampionPlatform;
			bool flag2 = buildingType.Type == BuildingType.Blocker;
			int walkableGap = flag2 ? 0 : this.CalculateWalkableGap(size);
			FlagStamp flagStamp = this.CreateFlagStamp(building, buildingType, size, walkableGap);
			if (!flag)
			{
				this.AddUnWalkableUnDestructibleFlags(flagStamp, size, walkableGap, flag2);
			}
			boardItem.FlagStamp = flagStamp;
			BoardController boardController = Service.Get<BoardController>();
			BoardCell<Entity> boardCell = boardController.Board.AddChild(boardItem, boardX, boardZ, building.Get<HealthComponent>(), !flag2);
			if (boardCell == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Failed to add building {0}:{1} at ({2},{3})", new object[]
				{
					buildingComponent.BuildingTO.Key,
					buildingComponent.BuildingTO.Uid,
					boardX,
					boardZ
				});
				return null;
			}
			TransformComponent transformComponent = building.Get<TransformComponent>();
			transformComponent.X = boardX;
			transformComponent.Z = boardZ;
			DamageableComponent damageableComponent = building.Get<DamageableComponent>();
			if (damageableComponent != null)
			{
				damageableComponent.Init();
			}
			buildingComponent.BuildingTO.SyncWithTransform(transformComponent);
			if (sendEvent)
			{
				Service.Get<EventManager>().SendEvent(EventId.BuildingPlacedOnBoard, building);
			}
			if (buildingType.Type == BuildingType.DroidHut)
			{
				this.DroidHut = building;
			}
			return boardCell;
		}

		public int CalculateWalkableGap(SizeComponent size)
		{
			if (Units.BoardToGridX(size.Width) > 1 && Units.BoardToGridZ(size.Depth) > 1)
			{
				return 1;
			}
			return 0;
		}

		public FlagStamp CreateFlagStamp(Entity building, BuildingTypeVO buildingVO, SizeComponent size, int walkableGap)
		{
			ShieldGeneratorComponent shieldGeneratorComponent = null;
			BuildingComponent buildingComponent = null;
			if (building != null)
			{
				shieldGeneratorComponent = building.Get<ShieldGeneratorComponent>();
				buildingComponent = building.Get<BuildingComponent>();
			}
			if (shieldGeneratorComponent != null)
			{
				return Service.Get<ShieldController>().CreateFlagStampForShield(shieldGeneratorComponent, size, walkableGap);
			}
			uint num;
			if (buildingVO != null && buildingVO.Type == BuildingType.Trap)
			{
				num = 0u;
			}
			else
			{
				num = 4u;
			}
			if (buildingVO != null && buildingVO.AllowDefensiveSpawn)
			{
				num |= 32u;
			}
			if (buildingComponent != null && buildingComponent.BuildingType.SpawnProtection > 0)
			{
				int num2 = buildingComponent.BuildingType.SpawnProtection;
				int num3 = num2 - (size.Width - walkableGap);
				if (num3 % 2 == 1)
				{
					num2++;
				}
				return new FlagStamp(num2, num2, num, false);
			}
			return new FlagStamp(size.Width - walkableGap + 2, size.Depth - walkableGap + 2, num, false);
		}

		public void AddUnWalkableUnDestructibleFlags(FlagStamp flagStamp, SizeComponent size, int walkableGap, bool blocker)
		{
			flagStamp.SetFlagsInRectCenter(size.Width - walkableGap, size.Depth - walkableGap, ((walkableGap > 0) ? 1u : 2u) | (blocker ? 64u : 0u));
		}

		public BoardCell<Entity> MoveBuildingWithinBoard(Entity building, int boardX, int boardZ)
		{
			BoardController boardController = Service.Get<BoardController>();
			BoardItemComponent boardItemComponent = building.Get<BoardItemComponent>();
			BoardItem<Entity> boardItem = boardItemComponent.BoardItem;
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			bool checkSkirt = buildingComponent.BuildingType.Type != BuildingType.Blocker;
			BoardCell<Entity> boardCell = boardController.Board.MoveChild(boardItem, boardX, boardZ, building.Get<HealthComponent>(), true, checkSkirt);
			if (boardCell != null)
			{
				TransformComponent transformComponent = building.Get<TransformComponent>();
				transformComponent.X = boardCell.X;
				transformComponent.Z = boardCell.Z;
				DamageableComponent damageableComponent = building.Get<DamageableComponent>();
				if (damageableComponent != null)
				{
					damageableComponent.Init();
				}
				Building buildingTO = building.Get<BuildingComponent>().BuildingTO;
				buildingTO.SyncWithTransform(transformComponent);
				Service.Get<EventManager>().SendEvent(EventId.BuildingMovedOnBoard, building);
			}
			else
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Failed to move building {0}:{1} to ({2},{3})", new object[]
				{
					buildingComponent.BuildingTO.Key,
					buildingComponent.BuildingTO.Uid,
					boardX,
					boardZ
				});
			}
			return boardCell;
		}

		public void FindValidPositionsAndAddBuildings(List<Entity> entities)
		{
			PositionMap positionMap = new PositionMap();
			for (int i = 0; i < entities.Count; i++)
			{
				Entity entity = entities[i];
				BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
				Building buildingTO = buildingComponent.BuildingTO;
				if (this.FindValidPositionAndUpdate(entity, buildingTO))
				{
					positionMap.AddPosition(buildingTO.Key, new Position
					{
						X = buildingTO.X,
						Z = buildingTO.Z
					});
				}
			}
			Service.Get<ServerAPI>().Enqueue(new BuildingMultiMoveCommand(new BuildingMultiMoveRequest
			{
				PositionMap = positionMap
			}));
		}

		private bool FindValidPositionAndUpdate(Entity entity, Building building)
		{
			int x = building.X;
			int z = building.Z;
			int num = 0;
			int num2 = 0;
			Service.Get<BuildingController>().FindStartingLocation(entity, out num, out num2, x, z, false);
			if (this.AddBuildingHelper(entity, num, num2, false) == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Attempted to fix position for building {0} at ({1},{2}) and no valid location available", new object[]
				{
					building.Key,
					x,
					z
				});
				return false;
			}
			Service.Get<StaRTSLogger>().ErrorFormat("Fixed invalid position for building {0} at ({1},{2}) to ({3},{4})", new object[]
			{
				building.Key,
				x,
				z,
				num,
				num2
			});
			building.X = num;
			building.Z = num2;
			return true;
		}

		public void ResetWorld()
		{
			EntityFactory entityFactory = Service.Get<EntityFactory>();
			EntityList allEntities = this.entityController.GetAllEntities();
			for (Entity entity = allEntities.Head; entity != null; entity = entity.Next)
			{
				entityFactory.DestroyEntity(entity, true, true);
			}
			this.DroidHut = null;
			Service.Get<SpatialIndexController>().Reset();
			Service.Get<BoardController>().ResetBoard();
			Service.Get<FXManager>().Reset();
			if (!HardwareProfile.IsLowEndDevice())
			{
				Service.Get<TerrainBlendController>().ResetTerrain();
			}
			Service.Get<EventManager>().SendEvent(EventId.WorldReset, null);
		}

		protected internal WorldController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldController)GCHandledObjects.GCHandleToObject(instance)).AddBuildingHelper((Entity)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), *(int*)(args + 2), *(sbyte*)(args + 3) != 0));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldController)GCHandledObjects.GCHandleToObject(instance)).AddBuildingToBoard((Entity)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), *(int*)(args + 2), *(sbyte*)(args + 3) != 0));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WorldController)GCHandledObjects.GCHandleToObject(instance)).AddEntityToWorld((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((WorldController)GCHandledObjects.GCHandleToObject(instance)).AddUnWalkableUnDestructibleFlags((FlagStamp)GCHandledObjects.GCHandleToObject(*args), (SizeComponent)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2), *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldController)GCHandledObjects.GCHandleToObject(instance)).CalculateWalkableGap((SizeComponent)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldController)GCHandledObjects.GCHandleToObject(instance)).CreateFlagStamp((Entity)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1]), (SizeComponent)GCHandledObjects.GCHandleToObject(args[2]), *(int*)(args + 3)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldController)GCHandledObjects.GCHandleToObject(instance)).FindValidPositionAndUpdate((Entity)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((WorldController)GCHandledObjects.GCHandleToObject(instance)).FindValidPositionsAndAddBuildings((List<Entity>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldController)GCHandledObjects.GCHandleToObject(instance)).DroidHut);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldController)GCHandledObjects.GCHandleToObject(instance)).MoveBuildingWithinBoard((Entity)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((WorldController)GCHandledObjects.GCHandleToObject(instance)).ResetWorld();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((WorldController)GCHandledObjects.GCHandleToObject(instance)).DroidHut = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
