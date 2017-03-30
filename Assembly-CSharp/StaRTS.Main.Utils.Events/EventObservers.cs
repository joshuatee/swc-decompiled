using StaRTS.DataStructures;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Utils.Events
{
	public class EventObservers
	{
		public PriorityList<IEventObserver> List
		{
			get;
			private set;
		}

		public MutableIterator Iter
		{
			get;
			private set;
		}

		public EventObservers()
		{
			this.List = new PriorityList<IEventObserver>();
			this.Iter = new MutableIterator();
		}
	}
}
