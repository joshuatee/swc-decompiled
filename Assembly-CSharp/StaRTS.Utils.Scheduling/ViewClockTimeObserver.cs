using System;
using WinRTBridge;

namespace StaRTS.Utils.Scheduling
{
	public class ViewClockTimeObserver
	{
		public IViewClockTimeObserver Observer
		{
			get;
			private set;
		}

		public float TickSize
		{
			get;
			private set;
		}

		public float Accumulator
		{
			get;
			set;
		}

		public ViewClockTimeObserver(IViewClockTimeObserver observer, float tickSize, float accumulator)
		{
			this.Observer = observer;
			this.TickSize = tickSize;
			this.Accumulator = accumulator;
		}

		protected internal ViewClockTimeObserver(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ViewClockTimeObserver)GCHandledObjects.GCHandleToObject(instance)).Accumulator);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ViewClockTimeObserver)GCHandledObjects.GCHandleToObject(instance)).Observer);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ViewClockTimeObserver)GCHandledObjects.GCHandleToObject(instance)).TickSize);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ViewClockTimeObserver)GCHandledObjects.GCHandleToObject(instance)).Accumulator = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ViewClockTimeObserver)GCHandledObjects.GCHandleToObject(instance)).Observer = (IViewClockTimeObserver)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ViewClockTimeObserver)GCHandledObjects.GCHandleToObject(instance)).TickSize = *(float*)args;
			return -1L;
		}
	}
}
