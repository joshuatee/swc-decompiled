using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class CantinaNode : Node<CantinaNode>
	{
		public CantinaComponent CantinaComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public CantinaNode()
		{
		}

		protected internal CantinaNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CantinaNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CantinaNode)GCHandledObjects.GCHandleToObject(instance)).CantinaComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((CantinaNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((CantinaNode)GCHandledObjects.GCHandleToObject(instance)).CantinaComp = (CantinaComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
