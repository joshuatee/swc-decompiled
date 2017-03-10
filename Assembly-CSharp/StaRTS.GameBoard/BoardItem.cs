using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard.Components;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.GameBoard
{
	public class BoardItem<T>
	{
		private T data;

		private Board<T> currentBoard;

		private BoardCell<T> currentCell;

		private SizeComponent size;

		private FilterComponent filter;

		private FlagStamp flagStamp;

		private LinkedListNode<BoardItem<T>> linkedListNode;

		public T Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}

		public Board<T> CurrentBoard
		{
			get
			{
				return this.currentBoard;
			}
		}

		public BoardCell<T> CurrentCell
		{
			get
			{
				return this.currentCell;
			}
		}

		public SizeComponent Size
		{
			get
			{
				return this.size;
			}
		}

		public FilterComponent Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				this.filter = value;
			}
		}

		public FlagStamp FlagStamp
		{
			get
			{
				return this.flagStamp;
			}
			set
			{
				this.flagStamp = value;
			}
		}

		public int Width
		{
			get
			{
				return this.size.Width;
			}
			set
			{
				this.size.Width = value;
			}
		}

		public int Depth
		{
			get
			{
				return this.size.Depth;
			}
			set
			{
				this.size.Depth = value;
			}
		}

		public int BoardX
		{
			get
			{
				return this.currentCell.X;
			}
		}

		public int BoardZ
		{
			get
			{
				return this.currentCell.Z;
			}
		}

		public LinkedListNode<BoardItem<T>> LinkedListNode
		{
			get
			{
				return this.linkedListNode;
			}
			set
			{
				this.linkedListNode = value;
			}
		}

		public BoardItem(SizeComponent size, T data, FilterComponent filter)
		{
			this.size = size;
			this.data = data;
			this.filter = filter;
		}

		internal void Internal_InformAddedToBoard(Board<T> board, int x, int y)
		{
			this.currentBoard = board;
			this.currentCell = board.GetCellAt(x, y);
		}

		internal void Internal_InformRemovedFromBoard()
		{
			this.currentBoard = null;
			this.currentCell = null;
		}

		public override string ToString()
		{
			string text = "< [";
			if (this.currentCell != null)
			{
				text = text + this.BoardX.ToString() + ", " + this.BoardZ.ToString();
			}
			else
			{
				text += "-, -";
			}
			return text + "] " + this.data.ToString() + ">";
		}

		protected internal BoardItem(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).BoardX);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).BoardZ);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).Depth);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).Filter);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).FlagStamp);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).Size);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).Width);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).Internal_InformRemovedFromBoard();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).Depth = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).Filter = (FilterComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).FlagStamp = (FlagStamp)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).Width = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardItem<Entity>)GCHandledObjects.GCHandleToObject(instance)).ToString());
		}
	}
}
