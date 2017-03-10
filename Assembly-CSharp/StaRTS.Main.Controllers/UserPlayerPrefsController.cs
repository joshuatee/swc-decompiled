using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class UserPlayerPrefsController
	{
		public static void SetString(string key, string value)
		{
			UserPlayerPrefsController.ClearOriginalKey(key);
			string prefix = UserPlayerPrefsController.GetPrefix();
			PlayerPrefs.SetString(prefix + key, value);
		}

		public static void SetFloat(string key, float value)
		{
			UserPlayerPrefsController.ClearOriginalKey(key);
			string prefix = UserPlayerPrefsController.GetPrefix();
			PlayerPrefs.SetFloat(prefix + key, value);
		}

		public static void SetInt(string key, int value)
		{
			UserPlayerPrefsController.ClearOriginalKey(key);
			string prefix = UserPlayerPrefsController.GetPrefix();
			PlayerPrefs.SetInt(prefix + key, value);
		}

		public static string GetString(string key, string fallback)
		{
			string prefix = UserPlayerPrefsController.GetPrefix();
			if (PlayerPrefs.HasKey(prefix + key))
			{
				return PlayerPrefs.GetString(prefix + key);
			}
			if (PlayerPrefs.HasKey(key))
			{
				return PlayerPrefs.GetString(key);
			}
			return fallback;
		}

		public static float GetFloat(string key, float fallback)
		{
			string prefix = UserPlayerPrefsController.GetPrefix();
			if (PlayerPrefs.HasKey(prefix + key))
			{
				return PlayerPrefs.GetFloat(prefix + key);
			}
			if (PlayerPrefs.HasKey(key))
			{
				return PlayerPrefs.GetFloat(key);
			}
			return fallback;
		}

		public static int GetInt(string key, int fallback)
		{
			string prefix = UserPlayerPrefsController.GetPrefix();
			if (PlayerPrefs.HasKey(prefix + key))
			{
				return PlayerPrefs.GetInt(prefix + key);
			}
			if (PlayerPrefs.HasKey(key))
			{
				return PlayerPrefs.GetInt(key);
			}
			return fallback;
		}

		private static string GetPrefix()
		{
			if (Service.IsSet<CurrentPlayer>())
			{
				return Service.Get<CurrentPlayer>().PlayerId;
			}
			if (PlayerPrefs.HasKey("prefPlayerId"))
			{
				return PlayerPrefs.GetString("prefPlayerId");
			}
			return string.Empty;
		}

		private static void ClearOriginalKey(string key)
		{
			if (PlayerPrefs.HasKey(key))
			{
				PlayerPrefs.DeleteKey(key);
			}
		}

		public static void Save()
		{
			PlayerPrefs.Save();
		}

		public UserPlayerPrefsController()
		{
		}

		protected internal UserPlayerPrefsController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			UserPlayerPrefsController.ClearOriginalKey(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UserPlayerPrefsController.GetFloat(Marshal.PtrToStringUni(*(IntPtr*)args), *(float*)(args + 1)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UserPlayerPrefsController.GetInt(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UserPlayerPrefsController.GetPrefix());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UserPlayerPrefsController.GetString(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			UserPlayerPrefsController.Save();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			UserPlayerPrefsController.SetFloat(Marshal.PtrToStringUni(*(IntPtr*)args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			UserPlayerPrefsController.SetInt(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			UserPlayerPrefsController.SetString(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}
	}
}
