using StaRTS.Main.Controllers.VictoryConditions;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType965 : $UnityType
	{
		public unsafe $UnityType965()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 600400) = ldftn($Invoke0);
			*(data + 600428) = ldftn($Invoke1);
			*(data + 600456) = ldftn($Invoke2);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IConditionParent)GCHandledObjects.GCHandleToObject(instance)).ChildFailed((AbstractCondition)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IConditionParent)GCHandledObjects.GCHandleToObject(instance)).ChildSatisfied((AbstractCondition)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IConditionParent)GCHandledObjects.GCHandleToObject(instance)).ChildUpdated((AbstractCondition)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}
	}
}
