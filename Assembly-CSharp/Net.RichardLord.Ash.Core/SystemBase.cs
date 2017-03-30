using System;

namespace Net.RichardLord.Ash.Core
{
	public abstract class SystemBase<T>
	{
		internal T Next
		{
			get;
			set;
		}

		internal T Previous
		{
			get;
			set;
		}

		internal int Priority
		{
			get;
			set;
		}

		internal ushort SchedulingPattern
		{
			get;
			set;
		}

		public abstract void AddToGame(IGame game);

		public abstract void RemoveFromGame(IGame game);

		public abstract void Update();
	}
}
