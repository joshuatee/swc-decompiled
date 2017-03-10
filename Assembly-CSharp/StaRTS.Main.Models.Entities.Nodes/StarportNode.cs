using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class StarportNode : Node<StarportNode>
	{
		public StarportComponent StarportComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public StarportNode()
		{
		}

		protected internal StarportNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StarportNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StarportNode)GCHandledObjects.GCHandleToObject(instance)).StarportComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((StarportNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((StarportNode)GCHandledObjects.GCHandleToObject(instance)).StarportComp = (StarportComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
