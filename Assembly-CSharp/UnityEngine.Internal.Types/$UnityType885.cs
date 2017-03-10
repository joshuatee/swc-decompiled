using StaRTS.Main.Controllers.Performance;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType885 : $UnityType
	{
		public unsafe $UnityType885()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 583236) = ldftn($Invoke0);
			*(data + 583264) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IPerformanceObserver)GCHandledObjects.GCHandleToObject(instance)).OnPerformanceDeviceMemUsage(*args);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IPerformanceObserver)GCHandledObjects.GCHandleToObject(instance)).OnPerformanceFPS(*(float*)args);
			return -1L;
		}
	}
}
