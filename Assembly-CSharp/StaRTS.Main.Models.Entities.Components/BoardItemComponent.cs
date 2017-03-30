using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard;
using System;

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
	}
}
