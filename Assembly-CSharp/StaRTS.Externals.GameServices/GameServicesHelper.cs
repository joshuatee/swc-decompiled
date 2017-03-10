using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.GameServices
{
	public class GameServicesHelper : IGameServicesHelper
	{
		private const string USER_DOMAIN = "gp";

		private const string EXTERNAL_NETWORK_CODE = "googleplus";

		private const string USER_ID = "1234567890";

		private bool mUserIsAuthenticated;

		public void OnReady()
		{
			this.SignIn();
		}

		public void SignIn()
		{
			this.mUserIsAuthenticated = true;
		}

		public void SignOut()
		{
			this.mUserIsAuthenticated = false;
		}

		public bool IsUserAuthenticated()
		{
			return this.mUserIsAuthenticated;
		}

		public bool HasBeenPromptedForSignIn()
		{
			return true;
		}

		public string GetUserId()
		{
			return "1234567890";
		}

		public string GetAuthToken()
		{
			return null;
		}

		public string GetUserDomain()
		{
			return "gp";
		}

		public string GetExternalNetworkCode()
		{
			return "googleplus";
		}

		public void UnlockAchievement(string achievementId)
		{
			this.IsUserAuthenticated();
		}

		public void AddScoreToLeaderboard(int score, string leaderboardId)
		{
			this.IsUserAuthenticated();
		}

		public void ShowAchievements()
		{
			this.IsUserAuthenticated();
		}

		public void ShowLeaderboard(string leaderboardId)
		{
			this.IsUserAuthenticated();
		}

		public void Share(string text, string contentURL, string thumbnailURL)
		{
			this.IsUserAuthenticated();
		}

		public void HandleUserIdCallback(string userId)
		{
		}

		public void HandleAuthTokenCallback(string authToken)
		{
		}

		public GameServicesHelper()
		{
		}

		protected internal GameServicesHelper(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).AddScoreToLeaderboard(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).GetAuthToken());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).GetExternalNetworkCode());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).GetUserDomain());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).GetUserId());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).HandleAuthTokenCallback(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).HandleUserIdCallback(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).HasBeenPromptedForSignIn());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).IsUserAuthenticated());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).OnReady();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).Share(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).ShowAchievements();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).ShowLeaderboard(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).SignIn();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).SignOut();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((GameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).UnlockAchievement(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
