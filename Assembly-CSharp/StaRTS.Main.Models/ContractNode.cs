using StaRTS.DataStructures.PriorityQueue;
using StaRTS.Main.Models.Player.World;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	internal class ContractNode : PriorityQueueNode
	{
		public Contract Contract
		{
			get;
			private set;
		}

		public uint StartServerTime
		{
			get;
			private set;
		}

		public Building BuildingTO
		{
			get;
			private set;
		}

		public LinkedListNode<ContractNode> NextNode
		{
			get;
			set;
		}

		public ContractNode(Contract contract, uint startServerTime, Building buildingTO)
		{
			this.Contract = contract;
			this.StartServerTime = startServerTime;
			this.BuildingTO = buildingTO;
		}

		protected internal ContractNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractNode)GCHandledObjects.GCHandleToObject(instance)).BuildingTO);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractNode)GCHandledObjects.GCHandleToObject(instance)).Contract);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractNode)GCHandledObjects.GCHandleToObject(instance)).NextNode);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ContractNode)GCHandledObjects.GCHandleToObject(instance)).BuildingTO = (Building)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ContractNode)GCHandledObjects.GCHandleToObject(instance)).Contract = (Contract)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ContractNode)GCHandledObjects.GCHandleToObject(instance)).NextNode = (LinkedListNode<ContractNode>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
