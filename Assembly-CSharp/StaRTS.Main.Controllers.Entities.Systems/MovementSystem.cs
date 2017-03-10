using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.Systems
{
	public class MovementSystem : SimSystemBase
	{
		private EntityController entityController;

		private NodeList<MovementNode> nodeList;

		public MovementSystem()
		{
		}

		public override void AddToGame(IGame game)
		{
			this.entityController = Service.Get<EntityController>();
			this.nodeList = this.entityController.GetNodeList<MovementNode>();
		}

		public override void RemoveFromGame(IGame game)
		{
		}

		protected override void Update(uint dt)
		{
			Board<Entity> board = Service.Get<BoardController>().Board;
			for (MovementNode movementNode = this.nodeList.Head; movementNode != null; movementNode = movementNode.Next)
			{
				SmartEntity smartEntity = (SmartEntity)movementNode.Entity;
				if (smartEntity.StateComp.CurState == EntityState.Moving && smartEntity.PathingComp.CurrentPath != null)
				{
					smartEntity.PathingComp.TimeOnSegment += dt;
					if ((ulong)smartEntity.PathingComp.TimeOnSegment > (ulong)((long)smartEntity.PathingComp.TimeToMove))
					{
						BoardCell<Entity> boardCell = smartEntity.PathingComp.GetNextTile();
						if (boardCell == null)
						{
							if (smartEntity.DroidComp == null)
							{
								Service.Get<ShooterController>().StopMoving(smartEntity.StateComp);
							}
							Service.Get<EventManager>().SendEvent(EventId.TroopReachedPathEnd, smartEntity);
							smartEntity.PathingComp.CurrentPath = null;
						}
						else
						{
							smartEntity.TransformComp.X = boardCell.X;
							smartEntity.TransformComp.Z = boardCell.Z;
							board.MoveChild(smartEntity.BoardItemComp.BoardItem, smartEntity.TransformComp.CenterGridX(), smartEntity.TransformComp.CenterGridZ(), null, false, false);
							PathView pathView = smartEntity.PathingComp.PathView;
							BoardCell<Entity> nextTurn = pathView.GetNextTurn();
							if (nextTurn.X == boardCell.X && nextTurn.Z == boardCell.Z)
							{
								pathView.AdvanceNextTurn();
							}
							boardCell = smartEntity.PathingComp.AdvanceNextTile();
							if (boardCell != null)
							{
								bool flag = smartEntity.TransformComp.X != boardCell.X && smartEntity.TransformComp.Z != boardCell.Z;
								smartEntity.PathingComp.TimeToMove += (flag ? 1414 : 1000) * smartEntity.PathingComp.TimePerBoardCellMs / 1000;
							}
							else
							{
								DefenderComponent defenderComp = smartEntity.DefenderComp;
								if (defenderComp != null)
								{
									defenderComp.Patrolling = false;
								}
							}
						}
					}
				}
			}
		}

		protected internal MovementSystem(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((MovementSystem)GCHandledObjects.GCHandleToObject(instance)).AddToGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((MovementSystem)GCHandledObjects.GCHandleToObject(instance)).RemoveFromGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
