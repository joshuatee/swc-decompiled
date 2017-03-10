using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class LootNode : Node<LootNode>
	{
		public LootComponent LootComp
		{
			get;
			set;
		}

		public HealthComponent HealthComp
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

		public LootNode()
		{
		}

		protected internal LootNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootNode)GCHandledObjects.GCHandleToObject(instance)).HealthComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootNode)GCHandledObjects.GCHandleToObject(instance)).LootComp);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootNode)GCHandledObjects.GCHandleToObject(instance)).View);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LootNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((LootNode)GCHandledObjects.GCHandleToObject(instance)).HealthComp = (HealthComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((LootNode)GCHandledObjects.GCHandleToObject(instance)).LootComp = (LootComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((LootNode)GCHandledObjects.GCHandleToObject(instance)).View = (GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
