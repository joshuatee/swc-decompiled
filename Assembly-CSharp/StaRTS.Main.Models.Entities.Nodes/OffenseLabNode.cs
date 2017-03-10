using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class OffenseLabNode : Node<OffenseLabNode>
	{
		public OffenseLabComponent OffenseLabComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public OffenseLabNode()
		{
		}

		protected internal OffenseLabNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OffenseLabNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OffenseLabNode)GCHandledObjects.GCHandleToObject(instance)).OffenseLabComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((OffenseLabNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((OffenseLabNode)GCHandledObjects.GCHandleToObject(instance)).OffenseLabComp = (OffenseLabComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
