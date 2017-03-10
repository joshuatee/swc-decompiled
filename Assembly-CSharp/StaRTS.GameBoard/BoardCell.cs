using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.GameBoard.Components;
using StaRTS.GameBoard.Pathfinding.InternalClasses;
using StaRTS.Main.Models.Entities.Components;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.GameBoard
{
	public class BoardCell<T>
	{
		private FilterComponent defaultFilter;

		private OrderedSet<BoardItem<T>> children;

		private HashSet<FlagStamp> flagStamps;

		public int X;

		public int Z;

		public uint Flags;

		public int Clearance;

		public int ClearanceNoWall;

		public PathfindingCellInfo PathInfo;

		public HealthComponent BuildingHealth;

		private List<T> obstacles;

		public List<T> Obstacles
		{
			get
			{
				return this.obstacles;
			}
		}

		public FilterComponent DefaultFilter
		{
			get
			{
				return this.defaultFilter;
			}
			set
			{
				this.defaultFilter = value;
			}
		}

		public OrderedSet<BoardItem<T>> Children
		{
			get
			{
				return this.children;
			}
			set
			{
				this.children = value;
			}
		}

		public Board<T> ParentBoard
		{
			get;
			protected set;
		}

		public bool IsNotSpawnProtected()
		{
			return (this.Flags & 20u) == 0u;
		}

		public bool IsWalkable()
		{
			return (this.Flags & 3u) == 0u;
		}

		public bool IsDestructible()
		{
			return (this.Flags & 64u) == 0u;
		}

		public bool CanShootThrough()
		{
			return this.IsWalkable() && this.IsDestructible();
		}

		public bool IsWalkableNoWall()
		{
			return (this.Flags & 1u) == 0u;
		}

		public BoardCell(Board<T> parentBoard, int x, int z, FilterComponent defaultFilter, uint defaultFlags)
		{
			this.ParentBoard = parentBoard;
			this.X = x;
			this.Z = z;
			this.children = null;
			this.flagStamps = null;
			this.obstacles = null;
			this.BuildingHealth = null;
			this.defaultFilter = defaultFilter;
			this.Flags = defaultFlags;
			this.Clearance = 0;
			this.PathInfo = null;
		}

		public bool CollidesWithItem(BoardItem<T> item)
		{
			FilterComponent filter = item.Filter;
			if (this.defaultFilter != null && this.defaultFilter.CollidesWith(filter))
			{
				return true;
			}
			if (this.children != null)
			{
				using (IEnumerator<BoardItem<T>> enumerator = this.children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BoardItem<T> current = enumerator.get_Current();
						if (current != item && current.Filter != null && current.Filter.CollidesWith(filter))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		public bool CollidesWith(FilterComponent filter)
		{
			if (this.defaultFilter != null && this.defaultFilter.CollidesWith(filter))
			{
				return true;
			}
			if (this.children != null)
			{
				using (IEnumerator<BoardItem<T>> enumerator = this.children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BoardItem<T> current = enumerator.get_Current();
						if (current.Filter != null && current.Filter.CollidesWith(filter))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		public bool AddChild(BoardItem<T> item, int startX, int startZ)
		{
			return this.AddChild(item, startX, startZ, false);
		}

		public bool AddChild(BoardItem<T> item, int startX, int startZ, bool ignoreCollisions)
		{
			if (this.children != null && this.children.Contains(item))
			{
				return true;
			}
			if (!ignoreCollisions && this.CollidesWithItem(item))
			{
				return false;
			}
			if (this.children == null)
			{
				this.children = new OrderedSet<BoardItem<T>>();
			}
			this.children.Add(item);
			return true;
		}

		public void AddFlagStamp(FlagStamp flagStamp)
		{
			bool flag = this.IsWalkable();
			bool flag2 = this.IsWalkableNoWall();
			if (this.flagStamps == null)
			{
				this.flagStamps = new HashSet<FlagStamp>();
			}
			this.flagStamps.Add(flagStamp);
			this.Flags |= flagStamp.GetFlagsForCell(this.X, this.Z);
			if (this.ParentBoard.IsCellOutsideFlaggableArea(this.X, this.Z))
			{
				this.Flags &= 4294967275u;
			}
			bool flag3 = this.IsWalkable();
			bool flag4 = this.IsWalkableNoWall();
			if (flag != flag3)
			{
				this.ParentBoard.DirtyClearanceMap(this.X, this.Z);
			}
			if (flag2 != flag4)
			{
				this.ParentBoard.DirtyClearanceMapNoWall(this.X, this.Z);
			}
		}

		public void RemoveChild(BoardItem<T> child)
		{
			if (this.children != null && this.children.Remove(child) && this.children.Count == 0)
			{
				this.children = null;
			}
		}

		public void RemoveFlagStamp(FlagStamp flagStamp)
		{
			if (this.flagStamps != null && this.flagStamps.Remove(flagStamp))
			{
				if (this.flagStamps.Count == 0)
				{
					this.flagStamps = null;
				}
				this.RefreshFlags();
			}
		}

		private void RefreshFlags()
		{
			bool flag = this.IsWalkable();
			bool flag2 = this.IsWalkableNoWall();
			this.Flags = 0u;
			if (this.flagStamps != null)
			{
				foreach (FlagStamp current in this.flagStamps)
				{
					this.Flags |= current.GetFlagsForCell(this.X, this.Z);
				}
			}
			bool flag3 = this.IsWalkable();
			bool flag4 = this.IsWalkableNoWall();
			if (flag != flag3)
			{
				this.ParentBoard.DirtyClearanceMap(this.X, this.Z);
			}
			if (flag2 != flag4)
			{
				this.ParentBoard.DirtyClearanceMapNoWall(this.X, this.Z);
			}
		}

		public void AddObstacle(T obstacle)
		{
			if (this.obstacles == null)
			{
				this.obstacles = new List<T>();
			}
			this.obstacles.Add(obstacle);
		}

		public void ClearObstacles()
		{
			this.obstacles = null;
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"[",
				this.X,
				", ",
				this.Z,
				"]"
			});
		}

		protected internal BoardCell(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).AddFlagStamp((FlagStamp)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).CanShootThrough());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).ClearObstacles();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).CollidesWith((FilterComponent)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).DefaultFilter);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).IsDestructible());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).IsNotSpawnProtected());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).IsWalkable());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).IsWalkableNoWall());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).RefreshFlags();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).RemoveFlagStamp((FlagStamp)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).DefaultFilter = (FilterComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardCell<Entity>)GCHandledObjects.GCHandleToObject(instance)).ToString());
		}
	}
}
