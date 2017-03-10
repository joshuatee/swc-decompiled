using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class HealthViewNode : Node<HealthViewNode>
	{
		public HealthViewComponent HealthView
		{
			get;
			set;
		}

		public HealthViewNode()
		{
		}

		protected internal HealthViewNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthViewNode)GCHandledObjects.GCHandleToObject(instance)).HealthView);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((HealthViewNode)GCHandledObjects.GCHandleToObject(instance)).HealthView = (HealthViewComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
