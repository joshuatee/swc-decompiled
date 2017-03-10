using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class TrapBuildingNode : Node<TrapBuildingNode>
	{
		public TrapComponent TrapComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public TrapBuildingNode()
		{
		}

		protected internal TrapBuildingNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapBuildingNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapBuildingNode)GCHandledObjects.GCHandleToObject(instance)).TrapComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TrapBuildingNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TrapBuildingNode)GCHandledObjects.GCHandleToObject(instance)).TrapComp = (TrapComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
