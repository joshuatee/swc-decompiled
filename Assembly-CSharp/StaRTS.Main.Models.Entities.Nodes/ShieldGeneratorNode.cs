using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class ShieldGeneratorNode : Node<ShieldGeneratorNode>
	{
		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public ShieldGeneratorComponent ShieldGenComp
		{
			get;
			set;
		}

		public BoardItemComponent BoardItem
		{
			get;
			set;
		}

		public GameObjectViewComponent View
		{
			get;
			set;
		}

		public ShieldGeneratorNode()
		{
		}

		protected internal ShieldGeneratorNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldGeneratorNode)GCHandledObjects.GCHandleToObject(instance)).BoardItem);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldGeneratorNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldGeneratorNode)GCHandledObjects.GCHandleToObject(instance)).ShieldGenComp);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldGeneratorNode)GCHandledObjects.GCHandleToObject(instance)).View);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ShieldGeneratorNode)GCHandledObjects.GCHandleToObject(instance)).BoardItem = (BoardItemComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ShieldGeneratorNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ShieldGeneratorNode)GCHandledObjects.GCHandleToObject(instance)).ShieldGenComp = (ShieldGeneratorComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ShieldGeneratorNode)GCHandledObjects.GCHandleToObject(instance)).View = (GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
