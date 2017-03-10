using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.EnvironmentManager
{
	public class EnvironmentController
	{
		public const string UNKNOWN_PLAYER_ID = "UnknownPlayerId";

		private IEnvironmentManager environmentManager;

		public EnvironmentController()
		{
			Service.Set<EnvironmentController>(this);
			this.environmentManager = new WindowsEnvironmentManager();
			this.environmentManager.Init();
		}

		public string GetLocale()
		{
			string text = this.environmentManager.GetLocale();
			if (string.IsNullOrEmpty(text))
			{
				text = "en_US";
			}
			return this.GetBIAppropriateLocale(text);
		}

		public string GetRawLocale()
		{
			return this.environmentManager.GetLocale();
		}

		public string GetCurrencyCode()
		{
			return this.environmentManager.GetCurrencyCode();
		}

		public bool IsTablet()
		{
			return this.environmentManager.IsTablet();
		}

		public string GetDeviceCountryCode()
		{
			string locale = this.environmentManager.GetLocale();
			string[] array = locale.Split(new char[]
			{
				'_'
			});
			string result = "US";
			if (array.Length > 1)
			{
				result = array[1];
			}
			return result;
		}

		private string GetBIAppropriateLocale(string deviceLocale)
		{
			string text = deviceLocale.Substring(0, 2);
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			string result;
			if (num <= 1194886160u)
			{
				if (num <= 1111292255u)
				{
					if (num != 1092248970u)
					{
						if (num == 1111292255u)
						{
							if (text == "ko")
							{
								result = "ko_KR";
								return result;
							}
						}
					}
					else if (text == "en")
					{
						result = "en_US";
						return result;
					}
				}
				else if (num != 1164435231u)
				{
					if (num != 1176137065u)
					{
						if (num == 1194886160u)
						{
							if (text == "it")
							{
								result = "it_IT";
								return result;
							}
						}
					}
					else if (text == "es")
					{
						result = "es_LA";
						return result;
					}
				}
				else if (text == "zh")
				{
					if (deviceLocale.Equals("zh_CN") || deviceLocale.Equals("zh_SG") || deviceLocale.Contains("Hans") || deviceLocale.Equals("zh-CN") || deviceLocale.Equals("zh-SG"))
					{
						result = "zh_CN";
						return result;
					}
					if (deviceLocale.Contains("Hant") || deviceLocale.Equals("zh-TW"))
					{
						result = "zh_TW";
						return result;
					}
					result = "en_US";
					return result;
				}
			}
			else if (num <= 1461901041u)
			{
				if (num != 1213488160u)
				{
					if (num == 1461901041u)
					{
						if (text == "fr")
						{
							result = "fr_FR";
							return result;
						}
					}
				}
				else if (text == "ru")
				{
					result = "ru_RU";
					return result;
				}
			}
			else if (num != 1545391778u)
			{
				if (num != 1565420801u)
				{
					if (num == 1816099348u)
					{
						if (text == "ja")
						{
							result = "ja_JP";
							return result;
						}
					}
				}
				else if (text == "pt")
				{
					result = "pt_BR";
					return result;
				}
			}
			else if (text == "de")
			{
				result = "de_DE";
				return result;
			}
			result = "en_US";
			return result;
		}

		public string GetDeviceId()
		{
			return this.environmentManager.GetDeviceId();
		}

		public string GetDeviceIDForEvent2()
		{
			return this.environmentManager.GetDeviceIdForEvent2();
		}

		public string GetDeviceIdType()
		{
			return this.environmentManager.GetDeviceIdType();
		}

		public string GetMachine()
		{
			return this.environmentManager.GetMachine();
		}

		public string GetModel()
		{
			return this.environmentManager.GetModel();
		}

		public int GetAPILevel()
		{
			Service.Get<StaRTSLogger>().Warn("GetAPILevel should only be used on Android");
			return this.environmentManager.GetAPILevel();
		}

		public string GetOSVersion()
		{
			string operatingSystem = SystemInfo.operatingSystem;
			return Regex.Replace(operatingSystem, "[^0-9.]", "");
		}

		public string GetOS()
		{
			return this.environmentManager.GetOS();
		}

		public string GetPlatform()
		{
			return this.environmentManager.GetPlatform();
		}

		public bool IsMusicPlaying()
		{
			return this.environmentManager.IsMusicPlaying();
		}

		public bool AreHeadphonesConnected()
		{
			return this.environmentManager.AreHeadphonesConnected();
		}

		public bool IsRestrictedProfile()
		{
			return this.environmentManager.IsRestrictedProfile();
		}

		public double GetTimezoneOffset()
		{
			return DateTimeOffset.get_Now().get_Offset().get_TotalHours();
		}

		public int GetTimezoneOffsetSeconds()
		{
			return (int)(this.GetTimezoneOffset() * 3600.0);
		}

		public void GainAudioFocus()
		{
			this.environmentManager.GainAudioFocus();
		}

		public void SetupAutoRotation()
		{
			if (this.environmentManager.IsAutoRotationEnabled())
			{
				Screen.orientation = ScreenOrientation.AutoRotation;
				return;
			}
			if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
			{
				Screen.orientation = ScreenOrientation.LandscapeRight;
				return;
			}
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}

		public void ShowAlert(string titleText, string messageText, string yesButtonText)
		{
			this.ShowAlert(titleText, messageText, yesButtonText, "");
		}

		public void ShowAlert(string titleText, string messageText, string yesButtonText, string noButtonText)
		{
			if (Service.IsSet<GameIdleController>())
			{
				Service.Get<GameIdleController>().Enabled = false;
			}
			this.environmentManager.ShowAlert(titleText, messageText, yesButtonText, noButtonText);
		}

		protected internal EnvironmentController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).AreHeadphonesConnected());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GainAudioFocus();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetAPILevel());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetBIAppropriateLocale(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetCurrencyCode());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetDeviceCountryCode());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetDeviceId());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetDeviceIDForEvent2());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetDeviceIdType());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetLocale());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetMachine());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetModel());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetOS());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetOSVersion());
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetPlatform());
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetRawLocale());
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetTimezoneOffsetSeconds());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).IsMusicPlaying());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).IsRestrictedProfile());
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).IsTablet());
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).SetupAutoRotation();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).ShowAlert(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((EnvironmentController)GCHandledObjects.GCHandleToObject(instance)).ShowAlert(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}
	}
}
