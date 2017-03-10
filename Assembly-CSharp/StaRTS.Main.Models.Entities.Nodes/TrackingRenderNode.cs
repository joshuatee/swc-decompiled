using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class TrackingRenderNode : Node<TrackingRenderNode>
	{
		public GameObjectViewComponent View
		{
			get;
			set;
		}

		public TrackingGameObjectViewComponent TrackingView
		{
			get;
			set;
		}

		public TrackingComponent TrackingComp
		{
			get;
			set;
		}

		public TrackingRenderNode()
		{
		}

		protected internal TrackingRenderNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingRenderNode)GCHandledObjects.GCHandleToObject(instance)).TrackingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingRenderNode)GCHandledObjects.GCHandleToObject(instance)).TrackingView);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingRenderNode)GCHandledObjects.GCHandleToObject(instance)).View);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TrackingRenderNode)GCHandledObjects.GCHandleToObject(instance)).TrackingComp = (TrackingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TrackingRenderNode)GCHandledObjects.GCHandleToObject(instance)).TrackingView = (TrackingGameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TrackingRenderNode)GCHandledObjects.GCHandleToObject(instance)).View = (GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
