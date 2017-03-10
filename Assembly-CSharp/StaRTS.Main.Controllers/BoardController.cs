using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard;
using StaRTS.GameBoard.Components;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class BoardController
	{
		public const int DEPLOYABLE_BOARD_RADIUS = 23;

		private Board<Entity> board;

		public Board<Entity> Board
		{
			get
			{
				return this.board;
			}
		}

		public BoardController()
		{
			Service.Set<BoardController>(this);
			this.Initialize();
		}

		private void Initialize()
		{
			this.board = new Board<Entity>(46, 42, 0u);
			this.board.AddConstraintRegion(new ConstraintRegion(-21, 21, -21, 21, CollisionFilters.BUILDABLE_AREA));
		}

		public int GetPriorityQueueSize()
		{
			return this.Board.BoardSize * this.Board.BoardSize;
		}

		public void RemoveEntity(Entity entity, bool removeSpawnProtection)
		{
			BoardItemComponent boardItemComponent = entity.Get<BoardItemComponent>();
			if (boardItemComponent == null)
			{
				return;
			}
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			this.board.RemoveChild(boardItemComponent.BoardItem, buildingComponent != null && buildingComponent.BuildingType.Type != BuildingType.Blocker, buildingComponent != null);
			if (buildingComponent != null)
			{
				FlagStamp flagStamp = boardItemComponent.BoardItem.FlagStamp;
				if (flagStamp == null)
				{
					return;
				}
				flagStamp.Clear();
				if (!removeSpawnProtection)
				{
					uint num = 4u;
					if (buildingComponent.BuildingType.AllowDefensiveSpawn)
					{
						num |= 32u;
					}
					flagStamp.Fill(num);
				}
				this.board.AddFlagStamp(flagStamp);
			}
			Service.Get<EventManager>().SendEvent(EventId.BuildingRemovedFromBoard, entity);
		}

		public void ResetBoard()
		{
			this.Initialize();
		}

		protected internal BoardController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardController)GCHandledObjects.GCHandleToObject(instance)).Board);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardController)GCHandledObjects.GCHandleToObject(instance)).GetPriorityQueueSize());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BoardController)GCHandledObjects.GCHandleToObject(instance)).Initialize();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BoardController)GCHandledObjects.GCHandleToObject(instance)).RemoveEntity((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((BoardController)GCHandledObjects.GCHandleToObject(instance)).ResetBoard();
			return -1L;
		}
	}
}
