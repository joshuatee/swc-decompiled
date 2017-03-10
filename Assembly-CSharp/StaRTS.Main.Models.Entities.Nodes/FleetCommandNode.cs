using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class FleetCommandNode : Node<FleetCommandNode>
	{
		public FleetCommandComponent FleetCommandComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public FleetCommandNode()
		{
		}

		protected internal FleetCommandNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FleetCommandNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FleetCommandNode)GCHandledObjects.GCHandleToObject(instance)).FleetCommandComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((FleetCommandNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((FleetCommandNode)GCHandledObjects.GCHandleToObject(instance)).FleetCommandComp = (FleetCommandComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
