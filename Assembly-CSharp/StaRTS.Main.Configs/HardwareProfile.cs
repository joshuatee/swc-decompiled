using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Configs
{
	public static class HardwareProfile
	{
		private const string DEVICE_IPOD5 = "iPod5,1";

		private const string DEVICE_IPHONE4 = "iPhone3,1";

		private const string DEVICE_IPHONE4S = "iPhone4,1";

		private const string DEVICE_IPAD2 = "iPad2,2";

		private const string DEVICE_IPADMINI = "iPad2,5";

		private const string DEVICE_IPAD3 = "iPad3,1";

		private static readonly string[] LOW_END_DEVICES = new string[]
		{
			"iPod5,1",
			"iPhone3,1",
			"iPhone4,1",
			"iPad2,2",
			"iPad2,5",
			"iPad3,1"
		};

		public static bool IsLowEndDevice()
		{
			int hardwareProfile = PlayerSettings.GetHardwareProfile();
			if (hardwareProfile == 1)
			{
				return false;
			}
			if (hardwareProfile != 2)
			{
				string deviceModel = HardwareProfile.GetDeviceModel();
				int i = 0;
				int num = HardwareProfile.LOW_END_DEVICES.Length;
				while (i < num)
				{
					if (deviceModel == HardwareProfile.LOW_END_DEVICES[i])
					{
						return true;
					}
					i++;
				}
				return false;
			}
			return true;
		}

		public static string GetDeviceModel()
		{
			return SystemInfo.deviceModel;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(HardwareProfile.GetDeviceModel());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(HardwareProfile.IsLowEndDevice());
		}
	}
}
