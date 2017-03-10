using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard.Components;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class DroidNode : Node<DroidNode>
	{
		public SizeComponent Size
		{
			get;
			set;
		}

		public StateComponent State
		{
			get;
			set;
		}

		public DroidComponent Droid
		{
			get;
			set;
		}

		public TransformComponent Transform
		{
			get;
			set;
		}

		public override bool IsValid()
		{
			return base.IsValid() && this.Size != null && this.State != null && this.Droid != null && this.Droid.Target != null && this.Transform != null;
		}

		public DroidNode()
		{
		}

		protected internal DroidNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidNode)GCHandledObjects.GCHandleToObject(instance)).Droid);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidNode)GCHandledObjects.GCHandleToObject(instance)).Size);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidNode)GCHandledObjects.GCHandleToObject(instance)).State);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidNode)GCHandledObjects.GCHandleToObject(instance)).Transform);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidNode)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DroidNode)GCHandledObjects.GCHandleToObject(instance)).Droid = (DroidComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((DroidNode)GCHandledObjects.GCHandleToObject(instance)).Size = (SizeComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((DroidNode)GCHandledObjects.GCHandleToObject(instance)).State = (StateComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((DroidNode)GCHandledObjects.GCHandleToObject(instance)).Transform = (TransformComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
