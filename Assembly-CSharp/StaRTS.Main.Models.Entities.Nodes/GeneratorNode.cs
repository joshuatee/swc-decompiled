using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class GeneratorNode : Node<GeneratorNode>
	{
		public GeneratorComponent GeneratorComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public GeneratorNode()
		{
		}

		protected internal GeneratorNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratorNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratorNode)GCHandledObjects.GCHandleToObject(instance)).GeneratorComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GeneratorNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((GeneratorNode)GCHandledObjects.GCHandleToObject(instance)).GeneratorComp = (GeneratorComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
