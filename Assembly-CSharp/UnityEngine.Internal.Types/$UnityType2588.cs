using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2588 : $UnityType
	{
		public unsafe $UnityType2588()
		{
			*(UnityEngine.Internal.$Metadata.data + 999008) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IViewFrameTimeObserver)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}
	}
}
