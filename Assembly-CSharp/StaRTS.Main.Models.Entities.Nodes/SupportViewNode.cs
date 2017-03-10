using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class SupportViewNode : Node<SupportViewNode>
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

		public SupportViewComponent SupportView
		{
			get;
			set;
		}

		public GameObjectViewComponent View
		{
			get;
			set;
		}

		public SupportViewNode()
		{
		}

		protected internal SupportViewNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportViewNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportViewNode)GCHandledObjects.GCHandleToObject(instance)).SupportComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportViewNode)GCHandledObjects.GCHandleToObject(instance)).SupportView);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportViewNode)GCHandledObjects.GCHandleToObject(instance)).View);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SupportViewNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SupportViewNode)GCHandledObjects.GCHandleToObject(instance)).SupportComp = (SupportComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SupportViewNode)GCHandledObjects.GCHandleToObject(instance)).SupportView = (SupportViewComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SupportViewNode)GCHandledObjects.GCHandleToObject(instance)).View = (GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
