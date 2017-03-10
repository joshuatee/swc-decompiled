using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard;
using StaRTS.GameBoard.Pathfinding;
using StaRTS.Main.Views.Entities;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class PathingComponent : ComponentBase
	{
		private const uint KILOTILES_PER_MINUTE = 3u;

		private const uint MS_PER_MINUTE = 60000u;

		private int nextTileIndex;

		private int maxSpeed;

		public BoardCell<Entity> EndCell;

		public BoardCell<Entity> TargetCell;

		public Path CurrentPath;

		public SmartEntity Target;

		public PathView PathView;

		public int TimePerBoardCellMs
		{
			get;
			private set;
		}

		public int TimeToMove
		{
			get;
			set;
		}

		public uint TimeOnSegment
		{
			get;
			set;
		}

		public int NextTileIndex
		{
			get
			{
				return this.nextTileIndex;
			}
		}

		public bool StateDirty
		{
			get;
			set;
		}

		public int MaxSpeed
		{
			get
			{
				return this.maxSpeed;
			}
			set
			{
				this.maxSpeed = value;
				this.TimePerBoardCellMs = (int)(60000L / (3L * (long)this.maxSpeed));
			}
		}

		public BoardCell<Entity> GetNextTile()
		{
			return this.CurrentPath.GetCell(this.nextTileIndex);
		}

		public BoardCell<Entity> AdvanceNextTile()
		{
			Path arg_17_0 = this.CurrentPath;
			int index = this.nextTileIndex + 1;
			this.nextTileIndex = index;
			return arg_17_0.GetCell(index);
		}

		public void InitializePathView()
		{
			if (this.PathView == null)
			{
				this.PathView = new PathView(this);
			}
			else
			{
				this.PathView.Reset(this);
			}
			this.PathView.AdvanceNextTurn();
		}

		public void Reset()
		{
			this.TimeOnSegment = 0u;
			this.TimeToMove = 0;
			this.Target = null;
			this.CurrentPath = null;
			this.nextTileIndex = 0;
			this.EndCell = null;
			this.TargetCell = null;
		}

		public PathingComponent()
		{
			this.Reset();
		}

		public PathingComponent(int maxSpeed, SmartEntity target)
		{
			this.Reset();
			this.MaxSpeed = maxSpeed;
			this.TimePerBoardCellMs = (int)(60000L / (3L * (long)maxSpeed));
			this.Target = target;
		}

		protected internal PathingComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).AdvanceNextTile());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).NextTileIndex);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).StateDirty);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).TimePerBoardCellMs);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).TimeToMove);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).GetNextTile());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).InitializePathView();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).StateDirty = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).TimePerBoardCellMs = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((PathingComponent)GCHandledObjects.GCHandleToObject(instance)).TimeToMove = *(int*)args;
			return -1L;
		}
	}
}
