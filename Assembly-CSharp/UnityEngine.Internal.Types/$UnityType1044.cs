using StaRTS.Main.Models;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1044 : $UnityType
	{
		public unsafe $UnityType1044()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 625572) = ldftn($Invoke0);
			*(data + 625600) = ldftn($Invoke1);
			*(data + 625628) = ldftn($Invoke2);
			*(data + 625656) = ldftn($Invoke3);
			*(data + 625684) = ldftn($Invoke4);
			*(data + 625712) = ldftn($Invoke5);
			*(data + 625740) = ldftn($Invoke6);
			*(data + 625768) = ldftn($Invoke7);
			*(data + 625796) = ldftn($Invoke8);
			*(data + 625824) = ldftn($Invoke9);
			*(data + 625852) = ldftn($Invoke10);
			*(data + 625880) = ldftn($Invoke11);
			*(data + 625908) = ldftn($Invoke12);
			*(data + 625936) = ldftn($Invoke13);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Btn1);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Btn1Action);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Btn1Data);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Btn2);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Btn2Action);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Btn2Data);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Btn1 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Btn1Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Btn1Data = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Btn2 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Btn2Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Btn2Data = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ICallToAction)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
