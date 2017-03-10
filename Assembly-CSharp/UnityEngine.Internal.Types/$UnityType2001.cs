using StaRTS.Main.Story;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2001 : $UnityType
	{
		public unsafe $UnityType2001()
		{
			*(UnityEngine.Internal.$Metadata.data + 836804) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ITriggerReactor)GCHandledObjects.GCHandleToObject(instance)).SatisfyTrigger((IStoryTrigger)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
