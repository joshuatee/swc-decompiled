using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class BoardItemComponent : ComponentBase
	{
		public BoardItem<Entity> BoardItem
		{
			get;
			set;
		}

		public BoardItemComponent(BoardItem<Entity> boardItem)
		{
			this.BoardItem = boardItem;
		}

		protected internal BoardItemComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BoardItemComponent)GCHandledObjects.GCHandleToObject(instance)).BoardItem);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BoardItemComponent)GCHandledObjects.GCHandleToObject(instance)).BoardItem = (BoardItem<Entity>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
