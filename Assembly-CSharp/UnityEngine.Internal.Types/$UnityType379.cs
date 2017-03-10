using StaRTS.Externals.BI;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType379 : $UnityType
	{
		public unsafe $UnityType379()
		{
			*(UnityEngine.Internal.$Metadata.data + 436068) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ILogCreator)GCHandledObjects.GCHandleToObject(instance)).CreateWWWDataFromBILog((BILog)GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
