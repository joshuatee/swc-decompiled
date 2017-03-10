using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2589 : $UnityType
	{
		public unsafe $UnityType2589()
		{
			*(UnityEngine.Internal.$Metadata.data + 999036) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IViewPhysicsTimeObserver)GCHandledObjects.GCHandleToObject(instance)).OnViewPhysicsTime(*(float*)args);
			return -1L;
		}
	}
}
