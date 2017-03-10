using System;
using WinRTBridge;

namespace StaRTS.Utils.Scheduling
{
	public class Timer
	{
		private bool isKilled;

		public uint Id
		{
			get;
			private set;
		}

		public uint Delay
		{
			get;
			private set;
		}

		public bool Repeat
		{
			get;
			private set;
		}

		public TimerDelegate Callback
		{
			get;
			private set;
		}

		public object Cookie
		{
			get;
			private set;
		}

		public uint TimeFire
		{
			get;
			private set;
		}

		public bool IsKilled
		{
			get
			{
				return this.isKilled;
			}
		}

		public Timer(uint id, uint delay, bool repeat, TimerDelegate callback, object cookie, uint now)
		{
			this.isKilled = false;
			this.Id = id;
			this.Delay = delay;
			this.Repeat = repeat;
			this.Callback = callback;
			this.Cookie = cookie;
			this.TimeFire = now + this.Delay;
		}

		public void DecTimeFire(uint delta)
		{
			this.TimeFire -= delta;
		}

		public uint IncTimeFireByDelay()
		{
			return this.TimeFire += this.Delay;
		}

		public void Kill()
		{
			this.Cookie = null;
			this.Callback = null;
			this.isKilled = true;
		}

		protected internal Timer(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Timer)GCHandledObjects.GCHandleToObject(instance)).Callback);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Timer)GCHandledObjects.GCHandleToObject(instance)).Cookie);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Timer)GCHandledObjects.GCHandleToObject(instance)).IsKilled);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Timer)GCHandledObjects.GCHandleToObject(instance)).Repeat);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Timer)GCHandledObjects.GCHandleToObject(instance)).Kill();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((Timer)GCHandledObjects.GCHandleToObject(instance)).Callback = (TimerDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((Timer)GCHandledObjects.GCHandleToObject(instance)).Cookie = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((Timer)GCHandledObjects.GCHandleToObject(instance)).Repeat = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
