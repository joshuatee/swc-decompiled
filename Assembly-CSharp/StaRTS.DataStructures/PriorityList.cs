using StaRTS.Main.Models.Entities;
using StaRTS.Main.Utils.Events;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.DataStructures
{
	public class PriorityList<T>
	{
		private List<ElementPriorityPair<T>> list;

		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		public PriorityList()
		{
			this.list = new List<ElementPriorityPair<T>>();
		}

		public virtual int Add(T element, int priority)
		{
			if (element == null)
			{
				return -1;
			}
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				ElementPriorityPair<T> elementPriorityPair = this.list[i];
				if (elementPriorityPair.Element == element)
				{
					return -1;
				}
				if (priority > elementPriorityPair.Priority)
				{
					this.list.Insert(i, new ElementPriorityPair<T>(element, priority));
					return i;
				}
				i++;
			}
			this.list.Add(new ElementPriorityPair<T>(element, priority));
			return this.list.Count - 1;
		}

		public ElementPriorityPair<T> Get(int i)
		{
			return this.list[i];
		}

		public T GetElement(int i)
		{
			return this.list[i].Element;
		}

		public int GetPriority(int i)
		{
			return this.list[i].Priority;
		}

		public void GetElementPriority(int i, out T element, out int priority)
		{
			ElementPriorityPair<T> elementPriorityPair = this.list[i];
			element = elementPriorityPair.Element;
			priority = elementPriorityPair.Priority;
		}

		public int IndexOf(T element)
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				if (this.list[i].Element == element)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		public void RemoveAt(int i)
		{
			this.list.RemoveAt(i);
		}

		protected internal PriorityList(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PriorityList<SmartEntity>)GCHandledObjects.GCHandleToObject(instance)).Count);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PriorityList<SmartEntity>)GCHandledObjects.GCHandleToObject(instance)).GetPriority(*(int*)args));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PriorityList<SmartEntity>)GCHandledObjects.GCHandleToObject(instance)).RemoveAt(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PriorityList<IEventObserver>)GCHandledObjects.GCHandleToObject(instance)).Count);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PriorityList<IEventObserver>)GCHandledObjects.GCHandleToObject(instance)).GetPriority(*(int*)args));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((PriorityList<IEventObserver>)GCHandledObjects.GCHandleToObject(instance)).RemoveAt(*(int*)args);
			return -1L;
		}
	}
}
