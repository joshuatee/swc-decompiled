using System;

namespace StaRTS.DataStructures.PriorityQueue
{
	public sealed class HeapPriorityQueue<T> where T : PriorityQueueNode
	{
		private int _numNodes;

		private readonly T[] _nodes;

		private int _numNodesEverEnqueued;

		public int Count
		{
			get
			{
				return this._numNodes;
			}
		}

		public int MaxSize
		{
			get
			{
				return this._nodes.Length - 1;
			}
		}

		public T First
		{
			get
			{
				return this._nodes[1];
			}
		}

		public HeapPriorityQueue(int maxNodes)
		{
			this._numNodes = 0;
			this._nodes = new T[maxNodes + 1];
			this._numNodesEverEnqueued = 0;
		}

		public void Clear()
		{
			for (int i = 1; i < this._nodes.Length; i++)
			{
				this._nodes[i] = (T)((object)null);
			}
			this._numNodes = 0;
		}

		public bool Contains(T node)
		{
			return this._nodes[node.QueueIndex] == node;
		}

		public void Enqueue(T node, int priority)
		{
			node.Priority = priority;
			this._numNodes++;
			this._nodes[this._numNodes] = node;
			node.QueueIndex = this._numNodes;
			node.InsertionIndex = this._numNodesEverEnqueued++;
			this.CascadeUp(this._nodes[this._numNodes]);
		}

		private void Swap(T node1, T node2)
		{
			this._nodes[node1.QueueIndex] = node2;
			this._nodes[node2.QueueIndex] = node1;
			int queueIndex = node1.QueueIndex;
			node1.QueueIndex = node2.QueueIndex;
			node2.QueueIndex = queueIndex;
		}

		private void CascadeUp(T node)
		{
			for (int i = node.QueueIndex / 2; i >= 1; i = node.QueueIndex / 2)
			{
				T t = this._nodes[i];
				if (t.Priority < node.Priority || (t.Priority == node.Priority && t.InsertionIndex < node.InsertionIndex))
				{
					break;
				}
				this.Swap(node, t);
			}
		}

		private void CascadeDown(T node)
		{
			int num = node.QueueIndex;
			while (true)
			{
				T t = node;
				int num2 = 2 * num;
				if (num2 > this._numNodes)
				{
					break;
				}
				T t2 = this._nodes[num2];
				if (t2.Priority < t.Priority || (t2.Priority == t.Priority && t2.InsertionIndex < t.InsertionIndex))
				{
					t = t2;
				}
				int num3 = num2 + 1;
				if (num3 <= this._numNodes)
				{
					T t3 = this._nodes[num3];
					if (t3.Priority < t.Priority || (t3.Priority == t.Priority && t3.InsertionIndex < t.InsertionIndex))
					{
						t = t3;
					}
				}
				if (t == node)
				{
					goto IL_153;
				}
				this._nodes[num] = t;
				int queueIndex = t.QueueIndex;
				t.QueueIndex = num;
				num = queueIndex;
			}
			node.QueueIndex = num;
			this._nodes[num] = node;
			return;
			IL_153:
			node.QueueIndex = num;
			this._nodes[num] = node;
		}

		private bool HasHigherPriority(T higher, T lower)
		{
			return higher.Priority < lower.Priority || (higher.Priority == lower.Priority && higher.InsertionIndex < lower.InsertionIndex);
		}

		public T Dequeue()
		{
			T t = this._nodes[1];
			this.Remove(t);
			return t;
		}

		public void UpdatePriority(T node, int priority)
		{
			node.Priority = priority;
			this.OnNodeUpdated(node);
		}

		private void OnNodeUpdated(T node)
		{
			int num = node.QueueIndex / 2;
			T lower = this._nodes[num];
			if (num > 0 && this.HasHigherPriority(node, lower))
			{
				this.CascadeUp(node);
			}
			else
			{
				this.CascadeDown(node);
			}
		}

		public void Remove(T node)
		{
			if (this._numNodes <= 1)
			{
				this._nodes[1] = (T)((object)null);
				this._numNodes = 0;
				return;
			}
			bool flag = false;
			T t = this._nodes[this._numNodes];
			if (node.QueueIndex != this._numNodes)
			{
				this.Swap(node, t);
				flag = true;
			}
			this._numNodes--;
			this._nodes[node.QueueIndex] = (T)((object)null);
			if (flag)
			{
				this.OnNodeUpdated(t);
			}
		}

		public T Get(int i)
		{
			return this._nodes[i];
		}

		public bool IsValidQueue()
		{
			for (int i = 1; i < this._nodes.Length; i++)
			{
				if (this._nodes[i] != null)
				{
					int num = 2 * i;
					if (num < this._nodes.Length && this._nodes[num] != null && this.HasHigherPriority(this._nodes[num], this._nodes[i]))
					{
						return false;
					}
					int num2 = num + 1;
					if (num2 < this._nodes.Length && this._nodes[num2] != null && this.HasHigherPriority(this._nodes[num2], this._nodes[i]))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
