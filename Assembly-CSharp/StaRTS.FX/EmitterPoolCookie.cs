using System;
using WinRTBridge;

namespace StaRTS.FX
{
	public class EmitterPoolCookie
	{
		public object Cookie
		{
			get;
			private set;
		}

		public float DelayPostEmitterStop
		{
			get;
			private set;
		}

		public EmitterStopDelegate PostEmitterStop
		{
			get;
			private set;
		}

		public EmitterStopDelegate EmitterStop
		{
			get;
			private set;
		}

		public EmitterPoolCookie(object cookie, EmitterStopDelegate emitterStop, float delayPostEmitterStop, EmitterStopDelegate postEmitterStop)
		{
			this.Cookie = cookie;
			this.EmitterStop = emitterStop;
			this.DelayPostEmitterStop = delayPostEmitterStop;
			this.PostEmitterStop = postEmitterStop;
		}

		protected internal EmitterPoolCookie(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EmitterPoolCookie)GCHandledObjects.GCHandleToObject(instance)).Cookie);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EmitterPoolCookie)GCHandledObjects.GCHandleToObject(instance)).DelayPostEmitterStop);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EmitterPoolCookie)GCHandledObjects.GCHandleToObject(instance)).EmitterStop);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EmitterPoolCookie)GCHandledObjects.GCHandleToObject(instance)).PostEmitterStop);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((EmitterPoolCookie)GCHandledObjects.GCHandleToObject(instance)).Cookie = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((EmitterPoolCookie)GCHandledObjects.GCHandleToObject(instance)).DelayPostEmitterStop = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((EmitterPoolCookie)GCHandledObjects.GCHandleToObject(instance)).EmitterStop = (EmitterStopDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((EmitterPoolCookie)GCHandledObjects.GCHandleToObject(instance)).PostEmitterStop = (EmitterStopDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
