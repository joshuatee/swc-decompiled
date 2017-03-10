using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class TrackingNode : Node<TrackingNode>
	{
		public TransformComponent Transform
		{
			get;
			set;
		}

		public TrackingComponent TrackingComp
		{
			get;
			set;
		}

		public ShooterComponent ShooterComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public TrackingNode()
		{
		}

		protected internal TrackingNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingNode)GCHandledObjects.GCHandleToObject(instance)).ShooterComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingNode)GCHandledObjects.GCHandleToObject(instance)).TrackingComp);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrackingNode)GCHandledObjects.GCHandleToObject(instance)).Transform);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TrackingNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TrackingNode)GCHandledObjects.GCHandleToObject(instance)).ShooterComp = (ShooterComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TrackingNode)GCHandledObjects.GCHandleToObject(instance)).TrackingComp = (TrackingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TrackingNode)GCHandledObjects.GCHandleToObject(instance)).Transform = (TransformComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
