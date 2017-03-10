using StaRTS.GameBoard.Components;
using StaRTS.Main.Utils;
using System;
using WinRTBridge;

namespace StaRTS.GameBoard
{
	public class ConstraintRegion
	{
		public int Left
		{
			get;
			set;
		}

		public int Right
		{
			get;
			set;
		}

		public int Top
		{
			get;
			set;
		}

		public int Bottom
		{
			get;
			set;
		}

		public FilterComponent FilterComponent
		{
			get;
			set;
		}

		public ConstraintRegion(int left, int right, int top, int bottom, FilterComponent filterComponent)
		{
			this.Left = left;
			this.Right = right;
			this.Top = top;
			this.Bottom = bottom;
			this.FilterComponent = filterComponent;
		}

		public bool Blocks<T>(BoardItem<T> item, int x, int z, int width, int depth)
		{
			return this.FilterComponent.CollidesWith(item.Filter) && !GameUtils.RectContainsRect(this.Left, this.Right, this.Top, this.Bottom, x, x + width, z, z + depth);
		}

		protected internal ConstraintRegion(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ConstraintRegion)GCHandledObjects.GCHandleToObject(instance)).Bottom);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ConstraintRegion)GCHandledObjects.GCHandleToObject(instance)).FilterComponent);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ConstraintRegion)GCHandledObjects.GCHandleToObject(instance)).Left);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ConstraintRegion)GCHandledObjects.GCHandleToObject(instance)).Right);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ConstraintRegion)GCHandledObjects.GCHandleToObject(instance)).Top);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ConstraintRegion)GCHandledObjects.GCHandleToObject(instance)).Bottom = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ConstraintRegion)GCHandledObjects.GCHandleToObject(instance)).FilterComponent = (FilterComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ConstraintRegion)GCHandledObjects.GCHandleToObject(instance)).Left = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ConstraintRegion)GCHandledObjects.GCHandleToObject(instance)).Right = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ConstraintRegion)GCHandledObjects.GCHandleToObject(instance)).Top = *(int*)args;
			return -1L;
		}
	}
}
