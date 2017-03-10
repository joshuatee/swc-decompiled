using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class DroidHutNode : Node<DroidHutNode>
	{
		public DroidHutComponent DroidHutComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public DroidHutNode()
		{
		}

		protected internal DroidHutNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidHutNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidHutNode)GCHandledObjects.GCHandleToObject(instance)).DroidHutComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DroidHutNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DroidHutNode)GCHandledObjects.GCHandleToObject(instance)).DroidHutComp = (DroidHutComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
