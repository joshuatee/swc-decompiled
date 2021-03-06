using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace StaRTS.DataStructures
{
	public class OrderedSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
	{
		private readonly IDictionary<T, LinkedListNode<T>> dictionary;

		private readonly LinkedList<T> linkedList;

		public int Count
		{
			get
			{
				return this.dictionary.get_Count();
			}
		}

		public virtual bool IsReadOnly
		{
			get
			{
				return this.dictionary.get_IsReadOnly();
			}
		}

		public LinkedListNode<T> First
		{
			get
			{
				return this.linkedList.First;
			}
		}

		public LinkedListNode<T> Last
		{
			get
			{
				return this.linkedList.Last;
			}
		}

		public OrderedSet() : this(EqualityComparer<T>.Default)
		{
		}

		public OrderedSet(IEqualityComparer<T> comparer)
		{
			this.dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
			this.linkedList = new LinkedList<T>();
		}

		void ICollection<T>.Add(T item)
		{
			this.Add(item);
		}

		public void Clear()
		{
			this.linkedList.Clear();
			this.dictionary.Clear();
		}

		public bool Remove(T item)
		{
			if (item != null)
			{
				LinkedListNode<T> linkedListNode;
				if (!this.dictionary.TryGetValue(item, ref linkedListNode))
				{
					return false;
				}
				this.dictionary.Remove(item);
				if (linkedListNode == null)
				{
					Service.Get<StaRTSLogger>().Error("OrderedSet Node is NULL");
				}
				else if (!this.linkedList.Contains(item))
				{
					Service.Get<StaRTSLogger>().Error("OrderedSet list does not contain item");
				}
				else if (linkedListNode.List != this.linkedList)
				{
					Service.Get<StaRTSLogger>().Error("OrderedSet node list does not match");
					linkedListNode = this.linkedList.Find(item);
					if (linkedListNode != null)
					{
						this.linkedList.Remove(linkedListNode);
					}
				}
				else
				{
					this.linkedList.Remove(linkedListNode);
				}
			}
			else
			{
				Service.Get<StaRTSLogger>().Error("OrderedSet Item is NULL");
			}
			return true;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.linkedList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public bool Contains(T item)
		{
			return this.dictionary.ContainsKey(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.linkedList.CopyTo(array, arrayIndex);
		}

		public bool Add(T item)
		{
			if (this.dictionary.ContainsKey(item))
			{
				return false;
			}
			LinkedListNode<T> linkedListNode = this.linkedList.AddLast(item);
			this.dictionary.Add(item, linkedListNode);
			if (linkedListNode == null)
			{
				Service.Get<StaRTSLogger>().Error("OrderedSet Added NULL node " + item);
			}
			return true;
		}
	}
}
