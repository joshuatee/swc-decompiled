using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures.PriorityQueue;
using StaRTS.GameBoard.Pathfinding.InternalClasses;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;

namespace StaRTS.GameBoard.Pathfinding
{
	public class Path
	{
		public const int DIST_STRAIGHT = 1000;

		public const int DIST_DIAGONAL = 1414;

		public const int MAX_SPEED_NORMALIZER = 10;

		private const int DIR_LEN = 8;

		private List<BoardCell<Entity>> pathCells;

		private List<BoardCell<Entity>> turns;

		private List<int> turnDistances;

		private BoardCell<Entity> startCell;

		private BoardCell<Entity> destCell;

		private BoardCell<Entity> targetCell;

		private PathfindingCellInfo curPathingCell;

		private Board<Entity> board;

		private PathingManager pathingManager;

		private int damagePerSecond;

		private uint maxShooterRange;

		private uint minShooterRange;

		private int maxSpeed;

		private int heristicMultiplier;

		private bool melee;

		private bool overWalls;

		private ProjectileTypeVO projectileType;

		private int maxPathLength;

		private bool isHealer;

		private uint targetInRangeModifier;

		private static readonly int[] dirX;

		private static readonly int[] dirY;

		private int priorityQueueSize;

		private HeapPriorityQueue<PathfindingCellInfo> openCells;

		private bool destructible;

		public int TroopWidth;

		public bool NoWall;

		private bool crushesWalls;

		private bool isTargetShield;

		public int CellCount
		{
			get
			{
				return this.pathCells.Count;
			}
		}

		public int TurnCount
		{
			get
			{
				return this.turns.Count;
			}
		}

		public Path(BoardCell<Entity> fromCell, BoardCell<Entity> toCell, BoardCell<Entity> targetAt, int maxLength, PathTroopParams troopParams, PathBoardParams boardParams)
		{
			this.pathCells = new List<BoardCell<Entity>>();
			this.turns = new List<BoardCell<Entity>>();
			this.turnDistances = new List<int>();
			BoardController boardController = Service.Get<BoardController>();
			this.board = boardController.Board;
			this.pathingManager = Service.Get<PathingManager>();
			this.startCell = fromCell;
			this.destCell = toCell;
			this.targetCell = targetAt;
			this.NoWall = (boardParams.IgnoreWall || troopParams.CrushesWalls);
			this.crushesWalls = troopParams.CrushesWalls;
			this.destructible = boardParams.Destructible;
			this.isHealer = troopParams.IsHealer;
			this.TroopWidth = troopParams.TroopWidth;
			this.damagePerSecond = troopParams.DPS;
			this.maxShooterRange = troopParams.MaxRange;
			this.targetInRangeModifier = troopParams.TargetInRangeModifier;
			if (this.isHealer && this.maxShooterRange > troopParams.SupportRange)
			{
				this.maxShooterRange = troopParams.SupportRange;
			}
			this.minShooterRange = troopParams.MinRange;
			this.maxSpeed = troopParams.MaxSpeed;
			this.heristicMultiplier = (int)troopParams.PathSearchWidth;
			this.maxPathLength = ((!this.isHealer) ? maxLength : -1);
			this.melee = troopParams.IsMelee;
			this.overWalls = troopParams.IsOverWall;
			this.projectileType = troopParams.ProjectileType;
			this.isTargetShield = troopParams.IsTargetShield;
			this.openCells = new HeapPriorityQueue<PathfindingCellInfo>(boardController.GetPriorityQueueSize());
			this.curPathingCell = this.pathingManager.GetPathingCell();
			this.curPathingCell.Cell = this.startCell;
			this.startCell.PathInfo = this.curPathingCell;
			this.curPathingCell.InRange = this.InRangeOfTarget(this.startCell);
			this.curPathingCell.RemainingCost = this.HeuristicDiagonal(this.startCell, this.destCell);
			this.curPathingCell.PathLength = 0;
			this.curPathingCell.PastCost = 0;
			this.curPathingCell.InClosedSet = true;
		}

		static Path()
		{
			Path.dirX = new int[]
			{
				-1,
				-1,
				-1,
				0,
				0,
				1,
				1,
				1
			};
			Path.dirY = new int[]
			{
				-1,
				0,
				1,
				-1,
				1,
				-1,
				0,
				1
			};
			for (int i = 0; i < 8; i++)
			{
				Path.dirX[i] += 23;
				Path.dirY[i] += 23;
			}
		}

		public BoardCell<Entity> GetDestCell()
		{
			return this.destCell;
		}

		public BoardCell<Entity> GetCell(int index)
		{
			if (index < this.pathCells.Count)
			{
				return this.pathCells[index];
			}
			return null;
		}

		private bool InRangeOfTarget(BoardCell<Entity> curCell)
		{
			if (curCell == this.destCell && this.minShooterRange == 0u)
			{
				return true;
			}
			int x;
			int z;
			if (this.melee && this.destCell != null)
			{
				x = this.destCell.X;
				z = this.destCell.Z;
			}
			else
			{
				if (this.targetCell == null)
				{
					return false;
				}
				x = this.targetCell.X;
				z = this.targetCell.Z;
			}
			int halfWidthForOffset = BoardUtils.GetHalfWidthForOffset(this.TroopWidth);
			int num = curCell.X + halfWidthForOffset;
			int num2 = curCell.Z + halfWidthForOffset;
			int num3 = num - x;
			int num4 = num3 + ((num3 <= 0) ? -1 : 1);
			int num5 = num2 - z;
			int num6 = num5 + ((num5 <= 0) ? -1 : 1);
			int num7 = num4 * num4 + num6 * num6;
			num7 *= 1;
			int num8 = num3 * num3 + num5 * num5;
			num8 *= 1;
			uint num9 = this.maxShooterRange * this.maxShooterRange;
			uint num10 = this.minShooterRange * this.minShooterRange;
			if (this.isHealer)
			{
				num7 += (int)(num9 / 2u);
			}
			else if (this.targetInRangeModifier > 0u)
			{
				num9 /= this.targetInRangeModifier;
			}
			if ((long)num7 < (long)((ulong)num9) && (long)num8 >= (long)((ulong)num10))
			{
				BoardCell<Entity> boardCell = BoardUtils.WhereDoesLineCrossFlag(Service.Get<BoardController>().Board, num, num2, x, z, 64u);
				return boardCell == null;
			}
			return false;
		}

		private List<BoardCell<Entity>> FindTheTurns(List<BoardCell<Entity>> path)
		{
			List<BoardCell<Entity>> list = new List<BoardCell<Entity>>();
			int count = path.Count;
			list.Add(path[count - 1]);
			for (int i = count - 2; i > 0; i--)
			{
				BoardCell<Entity> boardCell = path[i];
				BoardCell<Entity> boardCell2 = path[i + 1];
				BoardCell<Entity> boardCell3 = path[i - 1];
				if (boardCell.X - boardCell2.X != boardCell3.X - boardCell.X || boardCell.Z - boardCell2.Z != boardCell3.Z - boardCell.Z)
				{
					list.Add(boardCell);
				}
			}
			if (count >= 2)
			{
				list.Add(path[0]);
			}
			return list;
		}

		private void AddTurn(BoardCell<Entity> legStart, BoardCell<Entity> legEnd, List<BoardCell<Entity>> pathCells, List<BoardCell<Entity>> turns, List<int> turnDistances)
		{
			turns.Add(legEnd);
			int num = 0;
			int num2 = 0;
			this.RasterLine(legStart.X, legStart.Z, legEnd.X, legEnd.Z, pathCells, out num, out num2);
			turnDistances.Add(num * 1000 + num2 * 1414);
		}

		public void EndCurrentPath(PathingComponent pathing)
		{
			int nextTileIndex = pathing.NextTileIndex;
			if (nextTileIndex < this.pathCells.Count)
			{
				this.pathCells.RemoveRange(nextTileIndex, this.pathCells.Count - nextTileIndex);
			}
		}

		private void SmoothThePath(List<BoardCell<Entity>> rawTurns, List<BoardCell<Entity>> pathCells, List<BoardCell<Entity>> turns, List<int> turnDistances)
		{
			if (rawTurns.Count == 0)
			{
				Service.Get<Logger>().Error("SmoothThePath: Not expecting empty path!");
				return;
			}
			pathCells.Add(rawTurns[0]);
			turns.Add(rawTurns[0]);
			turnDistances.Add(0);
			if (rawTurns.Count == 1)
			{
				return;
			}
			if (rawTurns.Count == 2)
			{
				this.AddTurn(rawTurns[0], rawTurns[1], pathCells, turns, turnDistances);
				return;
			}
			BoardCell<Entity> boardCell = rawTurns[0];
			BoardCell<Entity> boardCell2 = rawTurns[1];
			for (int i = 2; i < rawTurns.Count; i++)
			{
				BoardCell<Entity> boardCell3 = rawTurns[i];
				if (!BoardUtils.HasLineOfClearance(this.board, boardCell.X, boardCell.Z, boardCell3.X, boardCell3.Z, this.TroopWidth))
				{
					this.AddTurn(boardCell, boardCell2, pathCells, turns, turnDistances);
					boardCell = boardCell2;
				}
				boardCell2 = boardCell3;
			}
			this.AddTurn(boardCell, boardCell2, pathCells, turns, turnDistances);
		}

		public void CalculatePath(out bool found)
		{
			List<BoardCell<Entity>> list = new List<BoardCell<Entity>>();
			int num = 46 - this.TroopWidth;
			while (!this.curPathingCell.InRange)
			{
				if (this.maxPathLength < 0 || this.curPathingCell.PathLength < this.maxPathLength)
				{
					BoardCell<Entity> cell = this.curPathingCell.Cell;
					int x = cell.X;
					int z = cell.Z;
					for (int i = 0; i < 8; i++)
					{
						int num2 = x + Path.dirX[i];
						if (num2 >= 0 && num2 <= num)
						{
							int num3 = z + Path.dirY[i];
							if (num3 >= 0 && num3 <= num)
							{
								BoardCell<Entity> boardCell = this.board.Cells[num2, num3];
								if ((boardCell.Flags & 64u) == 0u)
								{
									if (!this.destructible)
									{
										int num4 = (!this.NoWall) ? boardCell.Clearance : boardCell.ClearanceNoWall;
										if (this.TroopWidth > num4)
										{
											goto IL_2A2;
										}
									}
									int num5 = this.CostToNeighbor(cell, boardCell, list);
									if (num5 != 2147483647)
									{
										int num6 = this.curPathingCell.PastCost + num5;
										int pathLength = this.curPathingCell.PathLength + 1;
										PathfindingCellInfo pathfindingCellInfo = boardCell.PathInfo;
										if (pathfindingCellInfo != null && pathfindingCellInfo.PoolIndex < this.pathingManager.FreeCellIndex)
										{
											if (!pathfindingCellInfo.InClosedSet)
											{
												if (!this.openCells.Contains(pathfindingCellInfo))
												{
													Service.Get<Logger>().ErrorFormat("Allocated cell not in close/open sets,PoolIndex:{0}, FreeIndex:{1}", new object[]
													{
														pathfindingCellInfo.PoolIndex,
														this.pathingManager.FreeCellIndex
													});
												}
												else if (num6 < pathfindingCellInfo.PastCost)
												{
													pathfindingCellInfo.PastCost = num6;
													pathfindingCellInfo.PathLength = pathLength;
													this.openCells.UpdatePriority(pathfindingCellInfo, pathfindingCellInfo.PastCost + pathfindingCellInfo.RemainingCost);
												}
											}
										}
										else
										{
											pathfindingCellInfo = this.pathingManager.GetPathingCell();
											pathfindingCellInfo.PrevCell = this.curPathingCell;
											if (boardCell.PathInfo != null)
											{
												PathfindingCellInfo pathInfo = boardCell.PathInfo;
												pathInfo.Cell = null;
											}
											pathfindingCellInfo.Cell = boardCell;
											boardCell.PathInfo = pathfindingCellInfo;
											pathfindingCellInfo.InRange = this.InRangeOfTarget(boardCell);
											pathfindingCellInfo.RemainingCost = this.HeuristicDiagonal(boardCell, this.destCell);
											pathfindingCellInfo.PastCost = num6;
											pathfindingCellInfo.PathLength = pathLength;
											this.openCells.Enqueue(pathfindingCellInfo, pathfindingCellInfo.PastCost + pathfindingCellInfo.RemainingCost);
										}
									}
								}
							}
						}
						IL_2A2:;
					}
				}
				if (this.openCells.Count == 0)
				{
					this.pathingManager.RecycleAllPathingCells();
					this.openCells = null;
					found = false;
					return;
				}
				this.curPathingCell = this.openCells.Dequeue();
				this.curPathingCell.InClosedSet = true;
			}
			if (!this.curPathingCell.InRange)
			{
				this.pathingManager.RecycleAllPathingCells();
				this.openCells = null;
				found = false;
				return;
			}
			do
			{
				list.Add(this.curPathingCell.Cell);
				this.curPathingCell = this.curPathingCell.PrevCell;
			}
			while (this.curPathingCell != null);
			int count = list.Count;
			if (count == 0)
			{
				Service.Get<Logger>().ErrorFormat("Empth Path from {0} to {1} within range {2}", new object[]
				{
					this.startCell,
					this.destCell,
					this.maxShooterRange
				});
			}
			if (list[count - 1] != this.startCell)
			{
				Service.Get<Logger>().ErrorFormat("First cell doesn't match: {0} and {1}", new object[]
				{
					this.startCell,
					list[count - 1]
				});
			}
			List<BoardCell<Entity>> rawTurns = this.FindTheTurns(list);
			this.SmoothThePath(rawTurns, this.pathCells, this.turns, this.turnDistances);
			this.pathingManager.RecycleAllPathingCells();
			this.openCells = null;
			found = true;
		}

		private int CostToNeighbor(BoardCell<Entity> fromCell, BoardCell<Entity> toCell, List<BoardCell<Entity>> cells)
		{
			int num;
			if (fromCell.X != toCell.X && fromCell.Z != toCell.Z)
			{
				num = 1414;
			}
			else
			{
				num = 1000;
			}
			int num2 = num * 10 / this.maxSpeed;
			int num3 = this.TroopWidth - ((!this.NoWall) ? toCell.Clearance : toCell.ClearanceNoWall);
			if (num3 > 0)
			{
				int num4 = this.RasterCrossSection(fromCell.X, fromCell.Z, toCell.X, toCell.Z, this.TroopWidth, cells);
				for (int i = 0; i < num4; i++)
				{
					BoardCell<Entity> boardCell = cells[i];
					uint flags = boardCell.Flags;
					if ((flags & 3u) != 0u)
					{
						if (!this.NoWall || (flags & 1u) != 0u)
						{
							if ((flags & 64u) != 0u || this.isHealer)
							{
								num2 = 2147483647;
								break;
							}
							HealthComponent buildingHealth = boardCell.BuildingHealth;
							if (buildingHealth != null && this.damagePerSecond != 0)
							{
								ArmorType armorType = buildingHealth.ArmorType;
								int num5 = 100;
								if (this.projectileType != null)
								{
									int num6 = this.projectileType.DamageMultipliers[(int)armorType];
									if (num6 >= 0)
									{
										num5 = num6;
									}
									else
									{
										Service.Get<Logger>().ErrorFormat("ArmorType {0} not found in ProjectileType {1}", new object[]
										{
											armorType,
											this.projectileType.Uid
										});
									}
								}
								int num7 = this.damagePerSecond * num5 / 100;
								if (num7 <= 0)
								{
									num2 = 2147483647;
									break;
								}
								num2 += buildingHealth.Health * 1000 / num7;
							}
						}
					}
				}
				cells.Clear();
			}
			return num2;
		}

		private int HeuristicDiagonal(BoardCell<Entity> fromCell, BoardCell<Entity> toCell)
		{
			uint num = (uint)(IntMath.FastDist(fromCell.X, fromCell.Z, toCell.X, toCell.Z) / 1024);
			uint num2 = 0u;
			if (num > this.maxShooterRange)
			{
				num2 = num - this.maxShooterRange;
			}
			else if (num < this.minShooterRange)
			{
				num2 = this.minShooterRange - num;
			}
			return this.heristicMultiplier * (int)num2 * 10 / this.maxSpeed;
		}

		public BoardCell<Entity> GetTurn(int turnIndex)
		{
			if (turnIndex >= 0 && turnIndex < this.turns.Count)
			{
				return this.turns[turnIndex];
			}
			return null;
		}

		public int GetTurnDistance(int turnIndex)
		{
			return this.turnDistances[turnIndex];
		}

		private void AddEntitiesOnCellToBlockingList(BoardCell<Entity> cell, uint targetId, HashSet<uint> entityIds, LinkedList<Entity> list, LinkedList<Entity> wallList, bool isPathingCell)
		{
			if (cell.Children == null)
			{
				return;
			}
			if (!cell.IsWalkableNoWall() || ((!this.NoWall || (this.crushesWalls && isPathingCell)) && (isPathingCell || !this.overWalls) && !cell.IsWalkable()))
			{
				foreach (BoardItem<Entity> current in cell.Children)
				{
					if (current.Data.Has(typeof(HealthComponent)))
					{
						if (entityIds.Add(current.Data.ID))
						{
							if (current.Data.ID != targetId)
							{
								SmartEntity smartEntity = (SmartEntity)current.Data;
								if (smartEntity.BuildingComp != null && smartEntity.BuildingComp.BuildingType.Type == BuildingType.Wall && this.crushesWalls)
								{
									wallList.AddLast(current.Data);
								}
								else
								{
									list.AddLast(current.Data);
								}
							}
						}
					}
				}
			}
		}

		private void AddCell(int x, int y, List<BoardCell<Entity>> cells)
		{
			x += 23;
			if (x >= 0 && x < 46)
			{
				y += 23;
				if (y >= 0 && y < 46)
				{
					BoardCell<Entity> item = this.board.Cells[x, y];
					cells.Add(item);
				}
			}
		}

		private int RasterCrossSection(int x0, int y0, int x1, int y1, int size, List<BoardCell<Entity>> cells)
		{
			if (size == 1)
			{
				this.AddCell(x1, y1, cells);
				return 1;
			}
			int num = x1 - x0;
			int num2 = y1 - y0;
			if (num > 0 && num2 == 0)
			{
				int i = y1;
				int num3 = y1 + size;
				while (i < num3)
				{
					this.AddCell(x1 + 1, i, cells);
					i++;
				}
				return size;
			}
			if (num < 0 && num2 == 0)
			{
				int j = y1;
				int num4 = y1 + size;
				while (j < num4)
				{
					this.AddCell(x1, j, cells);
					j++;
				}
				return size;
			}
			if (num == 0 && num2 > 0)
			{
				int k = x1;
				int num5 = x1 + size;
				while (k < num5)
				{
					this.AddCell(k, y1 + 1, cells);
					k++;
				}
				return size;
			}
			if (num == 0 && num2 < 0)
			{
				int l = x1;
				int num6 = x1 + size;
				while (l < num6)
				{
					this.AddCell(l, y1, cells);
					l++;
				}
				return size;
			}
			if (num > 0 && num2 < 0)
			{
				int num7 = 1;
				int num8 = x1;
				int num9 = y1 + size - 1;
				int num10 = x1 + size - 1;
				while (num8 < num10 && num9 > y1)
				{
					this.AddCell(num8, y1, cells);
					this.AddCell(x1 + 1, num9, cells);
					num7 += 2;
					num8++;
					num9--;
				}
				this.AddCell(x1 + 1, y1, cells);
				return num7;
			}
			if (num < 0 && num2 < 0)
			{
				int num7 = 1;
				int num11 = x1 + size - 1;
				int num12 = y1 + size - 1;
				while (num11 > x1 && num12 > y1)
				{
					this.AddCell(num11, y1, cells);
					this.AddCell(x1, num12, cells);
					num7 += 2;
					num11--;
					num12--;
				}
				this.AddCell(x1, y1, cells);
				return num7;
			}
			if (num < 0 && num2 > 0)
			{
				int num7 = 1;
				int num13 = x1 + size - 1;
				int num14 = y1;
				int num15 = y1 + size - 1;
				while (num13 > x1 && num14 < num15)
				{
					this.AddCell(num13, y1 + 1, cells);
					this.AddCell(x1, num14, cells);
					num7 += 2;
					num13--;
					num14++;
				}
				this.AddCell(x1, y1 + 1, cells);
				return num7;
			}
			if (num > 0 && num2 > 0)
			{
				int num7 = 1;
				int num16 = x1;
				int num17 = y1;
				int num18 = y1 + size - 1;
				while (num16 < x1 + size - 1 && num17 < num18)
				{
					this.AddCell(num16, y1 + 1, cells);
					this.AddCell(x1 + 1, num17, cells);
					num7 += 2;
					num16++;
					num17++;
				}
				this.AddCell(x1 + 1, y1 + 1, cells);
				return num7;
			}
			return 0;
		}

		public LinkedList<Entity> GetBlockingEntities(uint targetId, out LinkedList<Entity> wallListForCrushing)
		{
			HashSet<uint> entityIds = new HashSet<uint>();
			LinkedList<Entity> linkedList = new LinkedList<Entity>();
			LinkedList<Entity> linkedList2 = new LinkedList<Entity>();
			List<BoardCell<Entity>> list = new List<BoardCell<Entity>>();
			for (int i = 0; i < this.pathCells.Count; i++)
			{
				BoardCell<Entity> boardCell = this.pathCells[i];
				int num = (!this.NoWall || this.crushesWalls) ? boardCell.Clearance : boardCell.ClearanceNoWall;
				if (num < this.TroopWidth)
				{
					int x;
					int y;
					if (i == 0)
					{
						if (this.pathCells.Count <= 1)
						{
							break;
						}
						x = boardCell.X * 2 - this.pathCells[1].X;
						y = boardCell.Z * 2 - this.pathCells[1].Z;
					}
					else
					{
						x = this.pathCells[i - 1].X;
						y = this.pathCells[i - 1].Z;
					}
					this.RasterCrossSection(x, y, boardCell.X, boardCell.Z, this.TroopWidth, list);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				this.AddEntitiesOnCellToBlockingList(list[j], targetId, entityIds, linkedList, linkedList2, true);
			}
			if (!this.melee)
			{
				list.Clear();
				int num2 = 0;
				int num3 = 0;
				int halfWidthForOffset = BoardUtils.GetHalfWidthForOffset(this.TroopWidth);
				int num4 = this.pathCells[this.pathCells.Count - 1].X + halfWidthForOffset;
				int num5 = this.pathCells[this.pathCells.Count - 1].Z + halfWidthForOffset;
				SmartEntity smartEntity = null;
				bool flag = true;
				if (this.isTargetShield)
				{
					foreach (BoardItem<Entity> current in this.targetCell.Children)
					{
						if (GameUtils.IsEntityShieldGenerator((SmartEntity)current.Data))
						{
							smartEntity = (SmartEntity)current.Data;
							break;
						}
					}
					if (smartEntity == null)
					{
						Service.Get<Logger>().Error("Pathing believes target is shield generator, however targetCell does not have shield generator entity.");
					}
					else
					{
						flag = Service.Get<ShieldController>().IsPositionUnderShield(num4, num5, smartEntity);
					}
				}
				this.RasterLine(num4, num5, this.targetCell.X, this.targetCell.Z, list, out num2, out num3);
				for (int k = 0; k < list.Count - 1; k++)
				{
					if (this.isTargetShield && !flag && Service.Get<ShieldController>().IsPositionUnderShield(list[k].X, list[k].Z, smartEntity))
					{
						break;
					}
					this.AddEntitiesOnCellToBlockingList(list[k], targetId, entityIds, linkedList, linkedList2, false);
				}
			}
			wallListForCrushing = linkedList2;
			return linkedList;
		}

		private void RasterLine(int x0, int y0, int x1, int y1, List<BoardCell<Entity>> cells, out int flatDist, out int diagDist)
		{
			int num = (x1 <= x0) ? -1 : 1;
			int num2 = (x1 - x0) * num;
			int num3 = (y1 <= y0) ? -1 : 1;
			int num4 = (y1 - y0) * num3;
			flatDist = 0;
			diagDist = 0;
			if (num2 > num4)
			{
				int num5 = num4 * 2 - num2;
				int num6 = num4 * 2;
				int num7 = (num4 - num2) * 2;
				int num8 = x0;
				int num9 = y0;
				while (num8 != x1)
				{
					if (num5 <= 0)
					{
						num5 += num6;
						num8 += num;
						flatDist++;
					}
					else
					{
						num5 += num7;
						num8 += num;
						num9 += num3;
						diagDist++;
					}
					this.AddCell(num8, num9, cells);
				}
			}
			else
			{
				int num10 = num2 * 2 - num4;
				int num11 = num2 * 2;
				int num12 = (num2 - num4) * 2;
				int num13 = x0;
				int num14 = y0;
				while (num14 != y1)
				{
					if (num10 <= 0)
					{
						num10 += num11;
						num14 += num3;
						flatDist++;
					}
					else
					{
						num10 += num12;
						num13 += num;
						num14 += num3;
						diagDist++;
					}
					this.AddCell(num13, num14, cells);
				}
			}
		}
	}
}
