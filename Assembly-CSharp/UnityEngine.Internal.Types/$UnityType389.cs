using StaRTS.Externals.EnvironmentManager;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType389 : $UnityType
	{
		public unsafe $UnityType389()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 438896) = ldftn($Invoke0);
			*(data + 438924) = ldftn($Invoke1);
			*(data + 438952) = ldftn($Invoke2);
			*(data + 438980) = ldftn($Invoke3);
			*(data + 439008) = ldftn($Invoke4);
			*(data + 439036) = ldftn($Invoke5);
			*(data + 439064) = ldftn($Invoke6);
			*(data + 439092) = ldftn($Invoke7);
			*(data + 439120) = ldftn($Invoke8);
			*(data + 439148) = ldftn($Invoke9);
			*(data + 439176) = ldftn($Invoke10);
			*(data + 439204) = ldftn($Invoke11);
			*(data + 439232) = ldftn($Invoke12);
			*(data + 439260) = ldftn($Invoke13);
			*(data + 439288) = ldftn($Invoke14);
			*(data + 439316) = ldftn($Invoke15);
			*(data + 439344) = ldftn($Invoke16);
			*(data + 439372) = ldftn($Invoke17);
			*(data + 439400) = ldftn($Invoke18);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).AreHeadphonesConnected());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GainAudioFocus();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetAPILevel());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrencyCode());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetDeviceId());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetDeviceIdForEvent2());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetDeviceIdType());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetLocale());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetMachine());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetModel());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetOS());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetPlatform());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).IsAutoRotationEnabled());
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).IsDeviceIdValid());
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).IsMusicPlaying());
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).IsRestrictedProfile());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).IsTablet());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((IEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).ShowAlert(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}
	}
}
