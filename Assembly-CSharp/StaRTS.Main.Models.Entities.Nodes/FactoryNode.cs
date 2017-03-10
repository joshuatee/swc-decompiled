using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class FactoryNode : Node<FactoryNode>
	{
		public FactoryComponent FactoryComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public FactoryNode()
		{
		}

		protected internal FactoryNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactoryNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactoryNode)GCHandledObjects.GCHandleToObject(instance)).FactoryComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((FactoryNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((FactoryNode)GCHandledObjects.GCHandleToObject(instance)).FactoryComp = (FactoryComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
