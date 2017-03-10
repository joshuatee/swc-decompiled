using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class BuffNode : Node<BuffNode>
	{
		public BuffComponent BuffComp
		{
			get;
			set;
		}

		public BuffNode()
		{
		}

		protected internal BuffNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffNode)GCHandledObjects.GCHandleToObject(instance)).BuffComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BuffNode)GCHandledObjects.GCHandleToObject(instance)).BuffComp = (BuffComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
