using System;
using WinRTBridge;

namespace StaRTS.FX
{
	public class ShieldReason
	{
		public ShieldLoadReason Reason
		{
			get;
			private set;
		}

		public object Cookie
		{
			get;
			private set;
		}

		public ShieldReason(ShieldLoadReason reason, object cookie)
		{
			this.Reason = reason;
			this.Cookie = cookie;
		}

		protected internal ShieldReason(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldReason)GCHandledObjects.GCHandleToObject(instance)).Cookie);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldReason)GCHandledObjects.GCHandleToObject(instance)).Reason);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ShieldReason)GCHandledObjects.GCHandleToObject(instance)).Cookie = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ShieldReason)GCHandledObjects.GCHandleToObject(instance)).Reason = (ShieldLoadReason)(*(int*)args);
			return -1L;
		}
	}
}
