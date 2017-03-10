using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1928 : $UnityType
	{
		public unsafe $UnityType1928()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 754344) = ldftn($Invoke0);
			*(data + 754372) = ldftn($Invoke1);
			*(data + 754400) = ldftn($Invoke2);
			*(data + 754428) = ldftn($Invoke3);
			*(data + 754456) = ldftn($Invoke4);
			*(data + 754484) = ldftn($Invoke5);
			*(data + 754512) = ldftn($Invoke6);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITimedEventVO)GCHandledObjects.GCHandleToObject(instance)).EndTimestamp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITimedEventVO)GCHandledObjects.GCHandleToObject(instance)).StartTimestamp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITimedEventVO)GCHandledObjects.GCHandleToObject(instance)).UseTimeZoneOffset);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITimedEventVO)GCHandledObjects.GCHandleToObject(instance)).GetClosingDurationSeconds());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITimedEventVO)GCHandledObjects.GCHandleToObject(instance)).GetUpcomingDurationSeconds());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ITimedEventVO)GCHandledObjects.GCHandleToObject(instance)).EndTimestamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ITimedEventVO)GCHandledObjects.GCHandleToObject(instance)).StartTimestamp = *(int*)args;
			return -1L;
		}
	}
}
