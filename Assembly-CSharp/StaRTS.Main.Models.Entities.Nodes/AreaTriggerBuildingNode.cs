using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class AreaTriggerBuildingNode : Node<AreaTriggerBuildingNode>
	{
		public TransformComponent TransformComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public AreaTriggerComponent AreaTriggerComp
		{
			get;
			set;
		}

		public AreaTriggerBuildingNode()
		{
		}

		protected internal AreaTriggerBuildingNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AreaTriggerBuildingNode)GCHandledObjects.GCHandleToObject(instance)).AreaTriggerComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AreaTriggerBuildingNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AreaTriggerBuildingNode)GCHandledObjects.GCHandleToObject(instance)).TransformComp);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AreaTriggerBuildingNode)GCHandledObjects.GCHandleToObject(instance)).AreaTriggerComp = (AreaTriggerComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AreaTriggerBuildingNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AreaTriggerBuildingNode)GCHandledObjects.GCHandleToObject(instance)).TransformComp = (TransformComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
