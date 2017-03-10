using System;

namespace StaRTS.DataStructures.PriorityQueue
{
	public class PriorityQueueNode
	{
		public int Priority;

		public int InsertionIndex;

		public int QueueIndex;

		public PriorityQueueNode()
		{
		}

		protected internal PriorityQueueNode(UIntPtr dummy)
		{
		}
	}
}
