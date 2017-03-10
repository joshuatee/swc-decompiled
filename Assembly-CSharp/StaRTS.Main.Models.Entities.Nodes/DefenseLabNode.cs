using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class DefenseLabNode : Node<DefenseLabNode>
	{
		public DefenseLabComponent DefenseLabComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public DefenseLabNode()
		{
		}

		protected internal DefenseLabNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenseLabNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenseLabNode)GCHandledObjects.GCHandleToObject(instance)).DefenseLabComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DefenseLabNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DefenseLabNode)GCHandledObjects.GCHandleToObject(instance)).DefenseLabComp = (DefenseLabComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
