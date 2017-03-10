using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class TrapNode : Node<TrapNode>
	{
		public TrapComponent TrapComp
		{
			get;
			set;
		}

		public TrapViewComponent TrapViewComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public TransformComponent TransformComp
		{
			get;
			set;
		}

		public HealthComponent HealthComp
		{
			get;
			set;
		}

		public TrapNode()
		{
		}

		protected internal TrapNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapNode)GCHandledObjects.GCHandleToObject(instance)).HealthComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapNode)GCHandledObjects.GCHandleToObject(instance)).TransformComp);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapNode)GCHandledObjects.GCHandleToObject(instance)).TrapComp);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapNode)GCHandledObjects.GCHandleToObject(instance)).TrapViewComp);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TrapNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TrapNode)GCHandledObjects.GCHandleToObject(instance)).HealthComp = (HealthComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TrapNode)GCHandledObjects.GCHandleToObject(instance)).TransformComp = (TransformComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TrapNode)GCHandledObjects.GCHandleToObject(instance)).TrapComp = (TrapComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TrapNode)GCHandledObjects.GCHandleToObject(instance)).TrapViewComp = (TrapViewComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
