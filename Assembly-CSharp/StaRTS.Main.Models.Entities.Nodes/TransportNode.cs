using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class TransportNode : Node<TransportNode>
	{
		public StateComponent State
		{
			get;
			set;
		}

		public TransportComponent Transport
		{
			get;
			set;
		}

		public TransportNode()
		{
		}

		protected internal TransportNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportNode)GCHandledObjects.GCHandleToObject(instance)).State);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransportNode)GCHandledObjects.GCHandleToObject(instance)).Transport);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TransportNode)GCHandledObjects.GCHandleToObject(instance)).State = (StateComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TransportNode)GCHandledObjects.GCHandleToObject(instance)).Transport = (TransportComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
