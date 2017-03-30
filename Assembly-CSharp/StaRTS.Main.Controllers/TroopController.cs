using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.GameBoard;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.World.Deploying;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers
{
	public class TroopController : AbstractDeployableController
	{
		private const int DROPSHIP_SEARCH_RADIUS = 2;

		private BoardController boardController;

		public TroopController()
		{
			Service.Set<TroopController>(this);
			this.boardController = Service.Get<BoardController>();
		}

		public SmartEntity SpawnTroop(TroopTypeVO troopType, TeamType teamType, IntPosition boardPosition, TroopSpawnMode spawnMode, bool sendPlacedEvent)
		{
			return this.SpawnTroop(troopType, teamType, boardPosition, spawnMode, sendPlacedEvent, false);
		}

		public SmartEntity SpawnTroop(TroopTypeVO troopType, TeamType teamType, IntPosition boardPosition, TroopSpawnMode spawnMode, bool sendPlacedEvent, bool forceAllow)
		{
			Entity spawnBuilding = null;
			BoardCell<Entity> boardCell = null;
			if (!this.FinalizeSafeBoardPosition(troopType, ref spawnBuilding, ref boardPosition, ref boardCell, teamType, spawnMode, forceAllow))
			{
				return null;
			}
			SmartEntity smartEntity = Service.Get<EntityFactory>().CreateTroopEntity(troopType, teamType, boardPosition, spawnBuilding, spawnMode, true, true);
			if (smartEntity == null)
			{
				return null;
			}
			BoardItemComponent boardItemComp = smartEntity.BoardItemComp;
			BoardItem<Entity> boardItem = boardItemComp.BoardItem;
			if (Service.Get<BoardController>().Board.AddChild(boardItem, boardCell.X, boardCell.Z, null, false, !forceAllow && troopType.Type != TroopType.Champion) == null)
			{
				return null;
			}
			Service.Get<EntityController>().AddEntity(smartEntity);
			Service.Get<TroopAbilityController>().OnTroopSpawned(smartEntity);
			if (troopType.Type != TroopType.Champion || teamType == TeamType.Attacker)
			{
				base.EnsureBattlePlayState();
			}
			if (sendPlacedEvent)
			{
				Service.Get<EventManager>().SendEvent(EventId.TroopPlacedOnBoard, smartEntity);
			}
			return smartEntity;
		}

		public bool FinalizeSafeBoardPosition(TroopTypeVO troopType, ref Entity spawnBuilding, ref IntPosition boardPosition, ref BoardCell<Entity> targetCell, TeamType teamType, TroopSpawnMode spawnMode, bool forceAllow)
		{
			targetCell = this.boardController.Board.GetClampedDeployableCellAt(boardPosition.x, boardPosition.z, troopType.SizeX);
			boardPosition = new IntPosition(targetCell.X, targetCell.Z);
			BoardCell<Entity> boardCell = null;
			if (spawnMode == TroopSpawnMode.LeashedToBuilding)
			{
				if (targetCell.Children == null)
				{
					return false;
				}
				LinkedListNode<BoardItem<Entity>> linkedListNode = targetCell.Children.First;
				while (linkedListNode != null)
				{
					spawnBuilding = linkedListNode.Value.Data;
					BuildingComponent buildingComponent = spawnBuilding.Get<BuildingComponent>();
					DamageableComponent damageableComponent = spawnBuilding.Get<DamageableComponent>();
					if (buildingComponent != null && (forceAllow || buildingComponent.BuildingType.AllowDefensiveSpawn) && damageableComponent != null)
					{
						if (forceAllow && troopType.Type == TroopType.Champion)
						{
							boardPosition = new IntPosition(targetCell.X, targetCell.Z);
							break;
						}
						targetCell = damageableComponent.FindASafeSpawnSpot(troopType.SizeX, out boardCell);
						if (targetCell == null)
						{
							return false;
						}
						boardPosition = new IntPosition(targetCell.X, targetCell.Z);
						break;
					}
					else
					{
						linkedListNode = linkedListNode.Next;
					}
				}
				if (linkedListNode == null)
				{
					return false;
				}
			}
			else if (!this.ValidateTroopPlacement(boardPosition, teamType, troopType.SizeX, true, out boardCell, forceAllow))
			{
				return false;
			}
			return true;
		}

		public Entity DeployTroopWithOffset(TroopTypeVO troopVO, ref int currentOffsetIndex, IntPosition spawnPosition, bool forceAllow, TeamType teamType)
		{
			TroopController troopController = Service.Get<TroopController>();
			IntPosition ip = TroopDeployer.OFFSETS[currentOffsetIndex] * troopVO.AutoSpawnSpreadingScale;
			IntPosition boardPosition = spawnPosition + ip;
			if (!troopController.ValidateAttackerTroopPlacement(boardPosition, troopVO.SizeX, false))
			{
				boardPosition = spawnPosition;
			}
			if (++currentOffsetIndex == TroopDeployer.OFFSETS.Length)
			{
				currentOffsetIndex = 0;
			}
			return troopController.SpawnTroop(troopVO, teamType, boardPosition, TroopSpawnMode.Unleashed, true, forceAllow);
		}

		public SmartEntity SpawnChampion(TroopTypeVO troopType, TeamType teamType, IntPosition boardPosition)
		{
			TroopSpawnMode spawnMode = (teamType != TeamType.Defender) ? TroopSpawnMode.Unleashed : TroopSpawnMode.LeashedToBuilding;
			SmartEntity smartEntity = this.SpawnTroop(troopType, teamType, boardPosition, spawnMode, true, teamType == TeamType.Defender);
			if (smartEntity != null)
			{
				smartEntity.Add(new ChampionComponent(troopType));
			}
			return smartEntity;
		}

		public SmartEntity SpawnHero(TroopTypeVO troopType, TeamType teamType, IntPosition boardPosition)
		{
			return this.SpawnHero(troopType, teamType, boardPosition, teamType == TeamType.Defender);
		}

		public SmartEntity SpawnHero(TroopTypeVO troopType, TeamType teamType, IntPosition boardPosition, bool leashed)
		{
			return this.SpawnTroop(troopType, teamType, boardPosition, (!leashed) ? TroopSpawnMode.Unleashed : TroopSpawnMode.LeashedToBuilding, true);
		}

		public bool ValidateTroopPlacement(IntPosition boardPosition, TeamType teamType, int troopWidth, bool sendEventsForInvalidPlacement, out BoardCell<Entity> pathCell, bool forceAllow)
		{
			Board<Entity> board = this.boardController.Board;
			BoardCell<Entity> cellAt = board.GetCellAt(boardPosition.x, boardPosition.z);
			pathCell = null;
			if (!forceAllow && (cellAt == null || cellAt.CollidesWith(CollisionFilters.TROOP)))
			{
				if (sendEventsForInvalidPlacement)
				{
					Service.Get<EventManager>().SendEvent(EventId.TroopNotPlacedInvalidArea, boardPosition);
				}
				return false;
			}
			int num = boardPosition.x - troopWidth / 2;
			int num2 = boardPosition.z - troopWidth / 2;
			pathCell = board.GetCellAt(num, num2);
			if (!forceAllow && (num > 23 - troopWidth || num2 > 23 - troopWidth || pathCell == null || pathCell.Clearance < troopWidth))
			{
				if (sendEventsForInvalidPlacement)
				{
					Service.Get<EventManager>().SendEvent(EventId.TroopNotPlacedInvalidArea, new IntPosition(boardPosition.x - Units.GridToBoardX(troopWidth), boardPosition.z - Units.GridToBoardX(troopWidth)));
				}
				return false;
			}
			if (teamType == TeamType.Attacker)
			{
				uint num3 = cellAt.Flags & 20u;
				if (num3 != 0u && !forceAllow)
				{
					if (sendEventsForInvalidPlacement)
					{
						Service.Get<EventManager>().SendEvent(EventId.TroopNotPlacedInvalidArea, boardPosition);
					}
					return false;
				}
			}
			return true;
		}

		public bool FindValidDropShipTroopPlacementCell(IntPosition boardPosition, TeamType teamType, int troopWidth, out IntPosition newBoardPosition)
		{
			List<BoardCell<Entity>> list = GameUtils.TraverseSpiral(2, boardPosition.x, boardPosition.z);
			newBoardPosition = IntPosition.Zero;
			this.boardController.Board.RefreshClearanceMap();
			foreach (BoardCell<Entity> current in list)
			{
				if (current != null && current.Clearance >= troopWidth && !current.CollidesWith(CollisionFilters.TROOP))
				{
					newBoardPosition = new IntPosition(current.X, current.Z);
					return true;
				}
			}
			return false;
		}

		public bool ValidateAttackerTroopPlacement(IntPosition boardPosition, int troopWidth, bool sendEventsForInvalidPlacement)
		{
			BoardCell<Entity> boardCell;
			return this.ValidateTroopPlacement(boardPosition, TeamType.Attacker, troopWidth, sendEventsForInvalidPlacement, out boardCell, false);
		}

		public static bool IsEntityHealer(SmartEntity troop)
		{
			bool result = false;
			if (troop != null && troop.TroopComp != null)
			{
				result = troop.TroopComp.TroopType.IsHealer;
			}
			return result;
		}
	}
}
