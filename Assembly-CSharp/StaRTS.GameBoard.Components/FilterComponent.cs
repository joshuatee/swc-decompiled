using System;
using WinRTBridge;

namespace StaRTS.GameBoard.Components
{
	public class FilterComponent
	{
		public int categoryBits;

		public int maskBits;

		public int Category
		{
			get
			{
				return this.categoryBits;
			}
			set
			{
				this.categoryBits = value;
			}
		}

		public int Mask
		{
			get
			{
				return this.maskBits;
			}
			set
			{
				this.maskBits = value;
			}
		}

		public FilterComponent(int categoryBits, int maskBits)
		{
			this.categoryBits = categoryBits;
			this.maskBits = maskBits;
		}

		public FilterComponent(FilterComponent otherFilter)
		{
			this.Set(otherFilter);
		}

		public bool CollidesWith(FilterComponent otherFilter)
		{
			return otherFilter != null && ((this.categoryBits & otherFilter.maskBits) != 0 || (this.maskBits & otherFilter.categoryBits) != 0);
		}

		public void Merge(FilterComponent otherFilter)
		{
			if (otherFilter == null)
			{
				return;
			}
			this.categoryBits |= otherFilter.Category;
			this.maskBits |= otherFilter.Mask;
		}

		public void Set(FilterComponent otherFilter)
		{
			if (otherFilter == null)
			{
				this.categoryBits = 0;
				this.maskBits = 0;
				return;
			}
			this.categoryBits = otherFilter.categoryBits;
			this.maskBits = otherFilter.maskBits;
		}

		protected internal FilterComponent(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FilterComponent)GCHandledObjects.GCHandleToObject(instance)).CollidesWith((FilterComponent)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FilterComponent)GCHandledObjects.GCHandleToObject(instance)).Category);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FilterComponent)GCHandledObjects.GCHandleToObject(instance)).Mask);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((FilterComponent)GCHandledObjects.GCHandleToObject(instance)).Merge((FilterComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((FilterComponent)GCHandledObjects.GCHandleToObject(instance)).Set((FilterComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((FilterComponent)GCHandledObjects.GCHandleToObject(instance)).Category = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((FilterComponent)GCHandledObjects.GCHandleToObject(instance)).Mask = *(int*)args;
			return -1L;
		}
	}
}
