using StaRTS.Main.Models;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1045 : $UnityType
	{
		public unsafe $UnityType1045()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 625964) = ldftn($Invoke0);
			*(data + 625992) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITimestamped)GCHandledObjects.GCHandleToObject(instance)).StartTime);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ITimestamped)GCHandledObjects.GCHandleToObject(instance)).StartTime = *(int*)args;
			return -1L;
		}
	}
}
