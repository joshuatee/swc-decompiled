using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1934 : $UnityType
	{
		public unsafe $UnityType1934()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 755744) = ldftn($Invoke0);
			*(data + 755772) = ldftn($Invoke1);
			*(data + 755800) = ldftn($Invoke2);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IValueObject)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IValueObject)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IValueObject)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
