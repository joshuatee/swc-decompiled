using StaRTS.Externals.FileManagement;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType408 : $UnityType
	{
		public unsafe $UnityType408()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 442620) = ldftn($Invoke0);
			*(data + 442648) = ldftn($Invoke1);
			*(data + 442676) = ldftn($Invoke2);
			*(data + 442704) = ldftn($Invoke3);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IFileManifest)GCHandledObjects.GCHandleToObject(instance)).GetManifestVersion());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IFileManifest)GCHandledObjects.GCHandleToObject(instance)).GetVersionFromFileUrl(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IFileManifest)GCHandledObjects.GCHandleToObject(instance)).Prepare((FmsOptions)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IFileManifest)GCHandledObjects.GCHandleToObject(instance)).TranslateFileUrl(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}
	}
}
