using StaRTS.Externals.FacebookApi;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType401 : $UnityType
	{
		public unsafe $UnityType401()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 441976) = ldftn($Invoke0);
			*(data + 442004) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IResult)GCHandledObjects.GCHandleToObject(instance)).Error);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IResult)GCHandledObjects.GCHandleToObject(instance)).RawResult);
		}
	}
}
