using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class StorageNode : Node<StorageNode>
	{
		public StorageComponent StorageComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public StorageNode()
		{
		}

		protected internal StorageNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StorageNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StorageNode)GCHandledObjects.GCHandleToObject(instance)).StorageComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((StorageNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((StorageNode)GCHandledObjects.GCHandleToObject(instance)).StorageComp = (StorageComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
