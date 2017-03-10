using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class SupportNode : Node<SupportNode>
	{
		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public SupportComponent SupportComp
		{
			get;
			set;
		}

		public SupportNode()
		{
		}

		protected internal SupportNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportNode)GCHandledObjects.GCHandleToObject(instance)).SupportComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SupportNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SupportNode)GCHandledObjects.GCHandleToObject(instance)).SupportComp = (SupportComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
