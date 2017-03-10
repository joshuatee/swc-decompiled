using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class WallNode : Node<WallNode>
	{
		public WallComponent WallComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public WallNode()
		{
		}

		protected internal WallNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WallNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WallNode)GCHandledObjects.GCHandleToObject(instance)).WallComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WallNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((WallNode)GCHandledObjects.GCHandleToObject(instance)).WallComp = (WallComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
