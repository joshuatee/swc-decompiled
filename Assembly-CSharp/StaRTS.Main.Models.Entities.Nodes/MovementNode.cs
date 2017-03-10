using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class MovementNode : Node<MovementNode>
	{
		public BoardItemComponent BoardItemComp
		{
			get;
			set;
		}

		public PathingComponent Path
		{
			get;
			set;
		}

		public TransformComponent Transform
		{
			get;
			set;
		}

		public StateComponent State
		{
			get;
			set;
		}

		public MovementNode()
		{
		}

		protected internal MovementNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MovementNode)GCHandledObjects.GCHandleToObject(instance)).BoardItemComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MovementNode)GCHandledObjects.GCHandleToObject(instance)).Path);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MovementNode)GCHandledObjects.GCHandleToObject(instance)).State);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MovementNode)GCHandledObjects.GCHandleToObject(instance)).Transform);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((MovementNode)GCHandledObjects.GCHandleToObject(instance)).BoardItemComp = (BoardItemComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((MovementNode)GCHandledObjects.GCHandleToObject(instance)).Path = (PathingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((MovementNode)GCHandledObjects.GCHandleToObject(instance)).State = (StateComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((MovementNode)GCHandledObjects.GCHandleToObject(instance)).Transform = (TransformComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
