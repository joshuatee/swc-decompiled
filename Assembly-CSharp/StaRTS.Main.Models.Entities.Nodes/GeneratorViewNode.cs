using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class GeneratorViewNode : Node<GeneratorViewNode>
	{
		public GeneratorComponent GeneratorComp
		{
			get;
			set;
		}

		public GeneratorViewComponent GeneratorView
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public GameObjectViewComponent View
		{
			get;
			set;
		}

		public GeneratorViewNode()
		{
		}

		protected internal GeneratorViewNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratorViewNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratorViewNode)GCHandledObjects.GCHandleToObject(instance)).GeneratorComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratorViewNode)GCHandledObjects.GCHandleToObject(instance)).GeneratorView);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratorViewNode)GCHandledObjects.GCHandleToObject(instance)).View);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((GeneratorViewNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((GeneratorViewNode)GCHandledObjects.GCHandleToObject(instance)).GeneratorComp = (GeneratorComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((GeneratorViewNode)GCHandledObjects.GCHandleToObject(instance)).GeneratorView = (GeneratorViewComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((GeneratorViewNode)GCHandledObjects.GCHandleToObject(instance)).View = (GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
