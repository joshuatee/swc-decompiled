using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class HousingNode : Node<HousingNode>
	{
		public HousingComponent FactoryComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public HousingNode()
		{
		}

		protected internal HousingNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HousingNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HousingNode)GCHandledObjects.GCHandleToObject(instance)).FactoryComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HousingNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HousingNode)GCHandledObjects.GCHandleToObject(instance)).FactoryComp = (HousingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
