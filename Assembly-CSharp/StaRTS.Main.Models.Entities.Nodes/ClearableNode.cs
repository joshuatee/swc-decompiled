using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class ClearableNode : Node<ClearableNode>
	{
		public ClearableComponent ClearableComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public ClearableNode()
		{
		}

		protected internal ClearableNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ClearableNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ClearableNode)GCHandledObjects.GCHandleToObject(instance)).ClearableComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ClearableNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ClearableNode)GCHandledObjects.GCHandleToObject(instance)).ClearableComp = (ClearableComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
