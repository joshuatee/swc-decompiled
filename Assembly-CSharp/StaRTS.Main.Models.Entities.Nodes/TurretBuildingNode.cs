using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class TurretBuildingNode : Node<TurretBuildingNode>
	{
		public TurretBuildingComponent TurretBuildingComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public TurretBuildingNode()
		{
		}

		protected internal TurretBuildingNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretBuildingNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretBuildingNode)GCHandledObjects.GCHandleToObject(instance)).TurretBuildingComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TurretBuildingNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TurretBuildingNode)GCHandledObjects.GCHandleToObject(instance)).TurretBuildingComp = (TurretBuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
