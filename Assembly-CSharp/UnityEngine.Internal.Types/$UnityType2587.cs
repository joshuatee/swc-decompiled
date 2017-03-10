using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2587 : $UnityType
	{
		public unsafe $UnityType2587()
		{
			*(UnityEngine.Internal.$Metadata.data + 998980) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IViewClockTimeObserver)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}
	}
}
