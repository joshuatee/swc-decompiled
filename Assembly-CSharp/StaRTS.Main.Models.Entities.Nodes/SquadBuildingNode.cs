using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class SquadBuildingNode : Node<SquadBuildingNode>
	{
		public SquadBuildingComponent SquadBuildingComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public SquadBuildingNode()
		{
		}

		protected internal SquadBuildingNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadBuildingNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadBuildingNode)GCHandledObjects.GCHandleToObject(instance)).SquadBuildingComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadBuildingNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadBuildingNode)GCHandledObjects.GCHandleToObject(instance)).SquadBuildingComp = (SquadBuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
