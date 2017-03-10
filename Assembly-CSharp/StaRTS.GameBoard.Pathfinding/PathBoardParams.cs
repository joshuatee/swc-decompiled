using System;

namespace StaRTS.GameBoard.Pathfinding
{
	public class PathBoardParams
	{
		public bool IgnoreWall;

		public bool Destructible;

		public PathBoardParams()
		{
		}

		protected internal PathBoardParams(UIntPtr dummy)
		{
		}
	}
}
