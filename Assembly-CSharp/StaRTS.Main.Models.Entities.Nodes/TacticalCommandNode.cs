using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class TacticalCommandNode : Node<TacticalCommandNode>
	{
		public TacticalCommandComponent TacticalCommandComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public TacticalCommandNode()
		{
		}

		protected internal TacticalCommandNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TacticalCommandNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TacticalCommandNode)GCHandledObjects.GCHandleToObject(instance)).TacticalCommandComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TacticalCommandNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TacticalCommandNode)GCHandledObjects.GCHandleToObject(instance)).TacticalCommandComp = (TacticalCommandComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
