using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.EnvironmentManager
{
	public class DefaultEnvironmentManager : IEnvironmentManager
	{
		public void Init()
		{
		}

		public string GetDeviceId()
		{
			return SystemInfo.deviceUniqueIdentifier;
		}

		public string GetDeviceIdForEvent2()
		{
			return this.GetDeviceId();
		}

		public bool IsDeviceIdValid()
		{
			return true;
		}

		public string GetDeviceIdType()
		{
			return "ud";
		}

		public string GetLocale()
		{
			return "en_US";
		}

		public string GetCurrencyCode()
		{
			return "NONE";
		}

		public string GetMachine()
		{
			return SystemInfo.deviceModel;
		}

		public string GetModel()
		{
			return SystemInfo.deviceModel;
		}

		public int GetAPILevel()
		{
			return 0;
		}

		public bool IsAutoRotationEnabled()
		{
			return true;
		}

		public bool IsMusicPlaying()
		{
			return false;
		}

		public bool IsRestrictedProfile()
		{
			return false;
		}

		public bool IsTablet()
		{
			return false;
		}

		public bool AreHeadphonesConnected()
		{
			return false;
		}

		public void ShowAlert(string titleText, string messageText, string yesButtonText, string noButtonText)
		{
		}

		public void GainAudioFocus()
		{
		}

		public string GetOS()
		{
			return "l";
		}

		public string GetPlatform()
		{
			return "mac";
		}

		public DefaultEnvironmentManager()
		{
		}

		protected internal DefaultEnvironmentManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).AreHeadphonesConnected());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GainAudioFocus();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetAPILevel());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetCurrencyCode());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetDeviceId());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetDeviceIdForEvent2());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetDeviceIdType());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetLocale());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetMachine());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetModel());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetOS());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).GetPlatform());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).IsAutoRotationEnabled());
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).IsDeviceIdValid());
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).IsMusicPlaying());
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).IsRestrictedProfile());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).IsTablet());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((DefaultEnvironmentManager)GCHandledObjects.GCHandleToObject(instance)).ShowAlert(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}
	}
}
