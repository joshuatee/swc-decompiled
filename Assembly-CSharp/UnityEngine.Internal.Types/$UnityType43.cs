using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType43 : $UnityType
	{
		public unsafe $UnityType43()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 353020) = ldftn($Invoke0);
			*(data + 353048) = ldftn($Invoke1);
			*(data + 353076) = ldftn($Invoke2);
			*(data + 353104) = ldftn($Invoke3);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IResourceFillable)GCHandledObjects.GCHandleToObject(instance)).CurrentFullnessPercentage);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IResourceFillable)GCHandledObjects.GCHandleToObject(instance)).PreviousFullnessPercentage);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IResourceFillable)GCHandledObjects.GCHandleToObject(instance)).CurrentFullnessPercentage = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((IResourceFillable)GCHandledObjects.GCHandleToObject(instance)).PreviousFullnessPercentage = *(float*)args;
			return -1L;
		}
	}
}
