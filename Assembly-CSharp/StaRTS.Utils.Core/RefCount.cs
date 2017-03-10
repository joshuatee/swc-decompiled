using System;
using WinRTBridge;

namespace StaRTS.Utils.Core
{
	public class RefCount
	{
		private int count;

		public int Count
		{
			get
			{
				return this.count;
			}
		}

		public RefCount()
		{
			this.count = 0;
		}

		public RefCount(int count)
		{
			this.count = count;
		}

		public int AddRef()
		{
			int result = this.count + 1;
			this.count = result;
			return result;
		}

		public int Release()
		{
			if (this.count <= 0)
			{
				return 0;
			}
			int result = this.count - 1;
			this.count = result;
			return result;
		}

		protected internal RefCount(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RefCount)GCHandledObjects.GCHandleToObject(instance)).AddRef());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RefCount)GCHandledObjects.GCHandleToObject(instance)).Count);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RefCount)GCHandledObjects.GCHandleToObject(instance)).Release());
		}
	}
}
