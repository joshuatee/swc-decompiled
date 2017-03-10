using StaRTS.Main.Story;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1999 : $UnityType
	{
		public unsafe $UnityType1999()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 836608) = ldftn($Invoke0);
			*(data + 836636) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IStoryReactor)GCHandledObjects.GCHandleToObject(instance)).ChildComplete((IStoryAction)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IStoryReactor)GCHandledObjects.GCHandleToObject(instance)).ChildPrepared((IStoryAction)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
