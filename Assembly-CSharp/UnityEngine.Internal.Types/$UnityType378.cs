using StaRTS.Externals.BI;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType378 : $UnityType
	{
		public unsafe $UnityType378()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 436012) = ldftn($Invoke0);
			*(data + 436040) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IDeviceInfoController)GCHandledObjects.GCHandleToObject(instance)).AddDeviceSpecificInfo((BILog)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IDeviceInfoController)GCHandledObjects.GCHandleToObject(instance)).GetDeviceId());
		}
	}
}
