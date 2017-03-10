using System;

namespace Net.RichardLord.Ash.Core
{
	internal class NodePool<TNode> where TNode : Node<TNode>, new()
	{
		private TNode _tail;

		private TNode _cacheTail;

		internal TNode Get()
		{
			if (this._tail != null)
			{
				TNode tail = this._tail;
				this._tail = this._tail.Previous;
				tail.Previous = default(TNode);
				return tail;
			}
			return Activator.CreateInstance<TNode>();
		}

		internal void Dispose(TNode node)
		{
			node.Next = default(TNode);
			node.Previous = this._tail;
			this._tail = node;
		}

		internal void Cache(TNode node)
		{
			node.Previous = this._cacheTail;
			this._cacheTail = node;
		}

		internal void ReleaseCache()
		{
			while (this._cacheTail != null)
			{
				TNode cacheTail = this._cacheTail;
				this._cacheTail = cacheTail.Previous;
				cacheTail.Next = default(TNode);
				cacheTail.Previous = this._tail;
				this._tail = cacheTail;
			}
		}
	}
}
