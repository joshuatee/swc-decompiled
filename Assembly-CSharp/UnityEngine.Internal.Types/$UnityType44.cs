using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType44 : $UnityType
	{
		public unsafe $UnityType44()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 353132) = ldftn($Invoke0);
			*(data + 353160) = ldftn($Invoke1);
			*(data + 353188) = ldftn($Invoke2);
			*(data + 353216) = ldftn($Invoke3);
			*(data + 353244) = ldftn($Invoke4);
			*(data + 353272) = ldftn($Invoke5);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ITapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).ContentDidAppear((TapjoyEvent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ITapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).ContentDidDisappear((TapjoyEvent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ITapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).ContentIsReady((TapjoyEvent)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ITapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).DidRequestAction((TapjoyEvent)GCHandledObjects.GCHandleToObject(*args), (TapjoyEventRequest)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ITapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).SendEventFailed((TapjoyEvent)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ITapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).SendEventSucceeded((TapjoyEvent)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}
	}
}
