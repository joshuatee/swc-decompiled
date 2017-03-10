using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard.Components;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class EntityRenderNode : Node<EntityRenderNode>
	{
		public TransformComponent Transform
		{
			get;
			set;
		}

		public SizeComponent Size
		{
			get;
			set;
		}

		public PathingComponent Path
		{
			get;
			set;
		}

		public GameObjectViewComponent View
		{
			get;
			set;
		}

		public StateComponent State
		{
			get;
			set;
		}

		public EntityRenderNode()
		{
		}

		protected internal EntityRenderNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityRenderNode)GCHandledObjects.GCHandleToObject(instance)).Path);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityRenderNode)GCHandledObjects.GCHandleToObject(instance)).Size);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityRenderNode)GCHandledObjects.GCHandleToObject(instance)).State);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityRenderNode)GCHandledObjects.GCHandleToObject(instance)).Transform);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityRenderNode)GCHandledObjects.GCHandleToObject(instance)).View);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((EntityRenderNode)GCHandledObjects.GCHandleToObject(instance)).Path = (PathingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((EntityRenderNode)GCHandledObjects.GCHandleToObject(instance)).Size = (SizeComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((EntityRenderNode)GCHandledObjects.GCHandleToObject(instance)).State = (StateComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((EntityRenderNode)GCHandledObjects.GCHandleToObject(instance)).Transform = (TransformComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((EntityRenderNode)GCHandledObjects.GCHandleToObject(instance)).View = (GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
