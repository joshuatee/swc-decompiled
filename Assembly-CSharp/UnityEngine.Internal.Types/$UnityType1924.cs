using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1924 : $UnityType
	{
		public unsafe $UnityType1924()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 753728) = ldftn($Invoke0);
			*(data + 753756) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IHealthVO)GCHandledObjects.GCHandleToObject(instance)).Health);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IHealthVO)GCHandledObjects.GCHandleToObject(instance)).Health = *(int*)args;
			return -1L;
		}
	}
}
