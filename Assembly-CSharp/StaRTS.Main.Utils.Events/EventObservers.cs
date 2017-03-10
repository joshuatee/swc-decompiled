using StaRTS.DataStructures;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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

		protected internal EventObservers(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventObservers)GCHandledObjects.GCHandleToObject(instance)).Iter);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventObservers)GCHandledObjects.GCHandleToObject(instance)).List);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EventObservers)GCHandledObjects.GCHandleToObject(instance)).Iter = (MutableIterator)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EventObservers)GCHandledObjects.GCHandleToObject(instance)).List = (PriorityList<IEventObserver>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
