using StaRTS.Externals.FileManagement;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType409 : $UnityType
	{
		public unsafe $UnityType409()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 442732) = ldftn($Invoke0);
			*(data + 442760) = ldftn($Invoke1);
			*(data + 442788) = ldftn($Invoke2);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IManifestLoader)GCHandledObjects.GCHandleToObject(instance)).GetManifest());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IManifestLoader)GCHandledObjects.GCHandleToObject(instance)).IsLoaded());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IManifestLoader)GCHandledObjects.GCHandleToObject(instance)).Load((FmsOptions)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (FmsCallback)GCHandledObjects.GCHandleToObject(args[2]), (FmsCallback)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}
	}
}
