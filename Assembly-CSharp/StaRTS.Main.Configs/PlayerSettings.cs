using StaRTS.Main.Controllers;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Configs
{
	public static class PlayerSettings
	{
		private const string PREF_MUSIC_VOLUME = "prefMusicVolume";

		private const string PREF_SFX_VOLUME = "prefSfxVolume";

		private const string PREF_NOTIFICATIONS_LEVEL = "prefNotificationsLevel2";

		private const string PREF_LOCALE_COPY = "prefLocaleCopy";

		private const string PREF_HARDWARE_PROFILE = "prefHardwareProfile";

		private const string PREF_SKIP_RAID_WAIT = "prefSkipRaidWait";

		private const string PREF_SKIP_RAID_DEFEND = "prefSkipRaidDefend";

		public const float DEFAULT_AUDIO_VOLUME = 1f;

		public const int DEFAULT_NOTIFICATIONS_LEVEL = 100;

		public const int HARDWARE_PROFILE_DEFAULT = 0;

		public const int HARDWARE_PROFILE_FORCE_HIGH_END = 1;

		public const int HARDWARE_PROFILE_FORCE_LOW_END = 2;

		public const int HARDWARE_PROFILE_COUNT = 3;

		public static float GetMusicVolume()
		{
			return UserPlayerPrefsController.GetFloat("prefMusicVolume", 1f);
		}

		public static void SetMusicVolume(float volume)
		{
			UserPlayerPrefsController.SetFloat("prefMusicVolume", volume);
			UserPlayerPrefsController.Save();
		}

		public static float GetSfxVolume()
		{
			return UserPlayerPrefsController.GetFloat("prefSfxVolume", 1f);
		}

		public static void SetSfxVolume(float volume)
		{
			UserPlayerPrefsController.SetFloat("prefSfxVolume", volume);
			UserPlayerPrefsController.Save();
		}

		public static int GetNotificationsLevel()
		{
			return UserPlayerPrefsController.GetInt("prefNotificationsLevel2", 0);
		}

		public static void SetNotificationsLevel(int level)
		{
			UserPlayerPrefsController.SetInt("prefNotificationsLevel2", level);
			UserPlayerPrefsController.Save();
		}

		public static void SetSkipRaidWaitConfirmation(bool skip)
		{
			UserPlayerPrefsController.SetInt("prefSkipRaidWait", skip ? 1 : 0);
			UserPlayerPrefsController.Save();
		}

		public static bool GetSkipRaidWaitConfirmation()
		{
			return UserPlayerPrefsController.GetInt("prefSkipRaidWait", 0) == 1;
		}

		public static void SetSkipRaidDefendConfirmation(bool skip)
		{
			UserPlayerPrefsController.SetInt("prefSkipRaidDefend", skip ? 1 : 0);
			UserPlayerPrefsController.Save();
		}

		public static bool GetSkipRaidDefendConfirmation()
		{
			return UserPlayerPrefsController.GetInt("prefSkipRaidDefend", 0) == 1;
		}

		public static string GetLocaleCopy()
		{
			return UserPlayerPrefsController.GetString("prefLocaleCopy", null);
		}

		public static void SetLocaleCopy(string locale)
		{
			UserPlayerPrefsController.SetString("prefLocaleCopy", locale);
			UserPlayerPrefsController.Save();
		}

		public static int GetHardwareProfile()
		{
			return UserPlayerPrefsController.GetInt("prefHardwareProfile", 0);
		}

		public static void SetHardwareProfile(int hardwareProfile)
		{
			UserPlayerPrefsController.SetInt("prefHardwareProfile", hardwareProfile);
			UserPlayerPrefsController.Save();
		}

		public static void ResetAllSettingsToDefault()
		{
			UserPlayerPrefsController.SetInt("prefHardwareProfile", 0);
			UserPlayerPrefsController.SetString("prefLocaleCopy", null);
			UserPlayerPrefsController.SetInt("prefSkipRaidDefend", 0);
			UserPlayerPrefsController.SetInt("prefSkipRaidWait", 0);
			UserPlayerPrefsController.SetInt("prefNotificationsLevel2", 0);
			UserPlayerPrefsController.SetFloat("prefSfxVolume", 1f);
			UserPlayerPrefsController.SetFloat("prefMusicVolume", 1f);
			UserPlayerPrefsController.Save();
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlayerSettings.GetHardwareProfile());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlayerSettings.GetLocaleCopy());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlayerSettings.GetMusicVolume());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlayerSettings.GetNotificationsLevel());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlayerSettings.GetSfxVolume());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlayerSettings.GetSkipRaidDefendConfirmation());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlayerSettings.GetSkipRaidWaitConfirmation());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			PlayerSettings.ResetAllSettingsToDefault();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			PlayerSettings.SetHardwareProfile(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			PlayerSettings.SetLocaleCopy(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			PlayerSettings.SetMusicVolume(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			PlayerSettings.SetNotificationsLevel(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			PlayerSettings.SetSfxVolume(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			PlayerSettings.SetSkipRaidDefendConfirmation(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			PlayerSettings.SetSkipRaidWaitConfirmation(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
