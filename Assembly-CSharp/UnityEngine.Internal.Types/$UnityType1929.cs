using StaRTS.Main.Models.ValueObjects;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1929 : $UnityType
	{
		public unsafe $UnityType1929()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 754540) = ldftn($Invoke0);
			*(data + 754568) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ITrackerVO)GCHandledObjects.GCHandleToObject(instance)).TrackerName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ITrackerVO)GCHandledObjects.GCHandleToObject(instance)).TrackerName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
