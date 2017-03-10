using StaRTS.DataStructures;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Utils.Events
{
	public class EventManager
	{
		private EventObservers[] eventIdToObservers;

		public EventManager()
		{
			Service.Set<EventManager>(this);
			int num = 518;
			this.eventIdToObservers = new EventObservers[num];
			for (int i = 0; i < num; i++)
			{
				this.eventIdToObservers[i] = null;
			}
		}

		public void RegisterObserver(IEventObserver observer, EventId id)
		{
			this.RegisterObserver(observer, id, EventPriority.Default);
		}

		public void RegisterObserver(IEventObserver observer, EventId id, EventPriority priority)
		{
			if (observer == null)
			{
				return;
			}
			EventObservers eventObservers = this.eventIdToObservers[(int)id];
			if (eventObservers == null)
			{
				eventObservers = new EventObservers();
				this.eventIdToObservers[(int)id] = eventObservers;
			}
			PriorityList<IEventObserver> list = eventObservers.List;
			if (list.IndexOf(observer) < 0)
			{
				list.Add(observer, (int)priority);
			}
		}

		public void UnregisterObserver(IEventObserver observer, EventId id)
		{
			EventObservers eventObservers = this.eventIdToObservers[(int)id];
			if (eventObservers != null)
			{
				PriorityList<IEventObserver> list = eventObservers.List;
				MutableIterator iter = eventObservers.Iter;
				int num = list.IndexOf(observer);
				if (num >= 0)
				{
					list.RemoveAt(num);
					iter.OnRemove(num);
					if (list.Count == 0)
					{
						this.eventIdToObservers[(int)id] = null;
					}
				}
			}
		}

		public bool IsEventListenerRegistered(IEventObserver observer, EventId id)
		{
			EventObservers eventObservers = this.eventIdToObservers[(int)id];
			if (eventObservers == null)
			{
				return false;
			}
			PriorityList<IEventObserver> list = eventObservers.List;
			return list.IndexOf(observer) >= 0;
		}

		public void SendEvent(EventId id, object cookie)
		{
			EventObservers eventObservers = this.eventIdToObservers[(int)id];
			if (eventObservers != null)
			{
				PriorityList<IEventObserver> list = eventObservers.List;
				MutableIterator iter = eventObservers.Iter;
				iter.Init(list.Count);
				while (iter.Active())
				{
					IEventObserver element = list.GetElement(iter.Index);
					if (element.OnEvent(id, cookie) == EatResponse.Eaten)
					{
						break;
					}
					iter.Next();
				}
				iter.Reset();
			}
		}

		protected internal EventManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventManager)GCHandledObjects.GCHandleToObject(instance)).IsEventListenerRegistered((IEventObserver)GCHandledObjects.GCHandleToObject(*args), (EventId)(*(int*)(args + 1))));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EventManager)GCHandledObjects.GCHandleToObject(instance)).RegisterObserver((IEventObserver)GCHandledObjects.GCHandleToObject(*args), (EventId)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EventManager)GCHandledObjects.GCHandleToObject(instance)).RegisterObserver((IEventObserver)GCHandledObjects.GCHandleToObject(*args), (EventId)(*(int*)(args + 1)), (EventPriority)(*(int*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EventManager)GCHandledObjects.GCHandleToObject(instance)).SendEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((EventManager)GCHandledObjects.GCHandleToObject(instance)).UnregisterObserver((IEventObserver)GCHandledObjects.GCHandleToObject(*args), (EventId)(*(int*)(args + 1)));
			return -1L;
		}
	}
}
