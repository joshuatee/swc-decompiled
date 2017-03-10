using System;
using System.Collections;
using WinRTBridge;

namespace StaRTS.Utils.Core
{
	public class MutableIterator
	{
		private int index;

		private int count;

		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				if (this.index == 0)
				{
					this.index = value;
				}
			}
		}

		public int Count
		{
			get
			{
				return this.count;
			}
		}

		public MutableIterator()
		{
			this.Reset();
		}

		public void Reset()
		{
			this.index = 0;
			this.count = 0;
		}

		public void Init(int count)
		{
			this.index = 0;
			this.count = count;
		}

		public void Init(ICollection list)
		{
			this.index = 0;
			this.count = list.get_Count();
		}

		public bool Active()
		{
			return this.index < this.count;
		}

		public void Next()
		{
			this.index++;
		}

		public void OnRemove(int i)
		{
			if (this.count > 0)
			{
				this.count--;
				if (i <= this.index)
				{
					this.index--;
				}
			}
		}

		protected internal MutableIterator(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MutableIterator)GCHandledObjects.GCHandleToObject(instance)).Active());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MutableIterator)GCHandledObjects.GCHandleToObject(instance)).Count);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MutableIterator)GCHandledObjects.GCHandleToObject(instance)).Index);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((MutableIterator)GCHandledObjects.GCHandleToObject(instance)).Init((ICollection)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((MutableIterator)GCHandledObjects.GCHandleToObject(instance)).Init(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((MutableIterator)GCHandledObjects.GCHandleToObject(instance)).Next();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((MutableIterator)GCHandledObjects.GCHandleToObject(instance)).OnRemove(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((MutableIterator)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((MutableIterator)GCHandledObjects.GCHandleToObject(instance)).Index = *(int*)args;
			return -1L;
		}
	}
}
