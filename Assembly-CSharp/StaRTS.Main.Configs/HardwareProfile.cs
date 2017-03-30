using System;
using UnityEngine;

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

		public static bool IsLowEndDevice()
		{
			int hardwareProfile = PlayerSettings.GetHardwareProfile();
			return hardwareProfile != 1 && (hardwareProfile == 2 || true);
		}

		public static string GetDeviceModel()
		{
			return SystemInfo.deviceModel;
		}
	}
}
