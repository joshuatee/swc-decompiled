using StaRTS.Audio;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.GameServices
{
	public class GameServicesManager : MonoBehaviour, IEventObserver, IUnitySerializable
	{
		private static IGameServicesHelper mGameServicesHelper;

		private static bool isWindowOpen;

		public static void StaticReset()
		{
			GameServicesManager.mGameServicesHelper = null;
			GameServicesManager.isWindowOpen = false;
		}

		public static void Startup()
		{
			GameObject gameObject = GameObject.Find("Game Services Manager");
			if (gameObject == null)
			{
				Service.Get<StaRTSLogger>().Error("Unable to find Game Services Manager object");
				return;
			}
			GameServicesManager component = gameObject.GetComponent<GameServicesManager>();
			if (component == null)
			{
				Service.Get<StaRTSLogger>().Error("Missing GameServicesManager component");
				return;
			}
			component.enabled = true;
			GameServicesManager.Init();
			Service.Get<EventManager>().RegisterObserver(component, EventId.ApplicationPauseToggled, EventPriority.AfterDefault);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ApplicationPauseToggled)
			{
				this.HandleApplicationPause((bool)cookie);
			}
			return EatResponse.NotEaten;
		}

		public static void AttemptAutomaticSignInPrompt()
		{
			string accountProviderId = Service.Get<IAccountSyncController>().GetAccountProviderId(AccountProvider.GOOGLEPLAY);
			bool flag = GameServicesManager.mGameServicesHelper.HasBeenPromptedForSignIn();
			if (string.IsNullOrEmpty(accountProviderId) && !flag)
			{
				GameServicesManager.mGameServicesHelper.SignIn();
			}
		}

		private void HandleApplicationPause(bool paused)
		{
			if (paused)
			{
				return;
			}
			if (GameServicesManager.isWindowOpen)
			{
				GameServicesManager.isWindowOpen = false;
				GameServicesManager.ToggleIdleTimerAndSounds(true);
			}
		}

		private static void Init()
		{
			GameServicesManager.mGameServicesHelper = new GameServicesHelper();
		}

		private static void ToggleIdleTimerAndSounds(bool enabled)
		{
			if (Service.IsSet<GameIdleController>())
			{
				Service.Get<GameIdleController>().Enabled = enabled;
			}
			if (Service.IsSet<AudioManager>())
			{
				Service.Get<AudioManager>().ToggleAllSounds(enabled);
			}
		}

		public static void OnReady()
		{
			GameServicesManager.mGameServicesHelper.OnReady();
		}

		public static void SignIn()
		{
			GameServicesManager.ToggleIdleTimerAndSounds(false);
			GameServicesManager.isWindowOpen = true;
			GameServicesManager.mGameServicesHelper.SignIn();
		}

		public static void SignOut()
		{
			GameServicesManager.mGameServicesHelper.SignOut();
		}

		public static void UnlockAchievement(string achievementId)
		{
			if (GameServicesManager.mGameServicesHelper != null)
			{
				GameServicesManager.mGameServicesHelper.UnlockAchievement(achievementId);
			}
		}

		public static void AddScoreToLeaderboard(int score, string leaderboardId)
		{
			GameServicesManager.mGameServicesHelper.AddScoreToLeaderboard(score, leaderboardId);
		}

		public static void ShowAchievements()
		{
			GameServicesManager.ToggleIdleTimerAndSounds(false);
			GameServicesManager.isWindowOpen = true;
			GameServicesManager.mGameServicesHelper.ShowAchievements();
		}

		public static void ShowLeaderboard(string leaderboardId)
		{
			GameServicesManager.ToggleIdleTimerAndSounds(false);
			GameServicesManager.isWindowOpen = true;
			GameServicesManager.mGameServicesHelper.ShowLeaderboard(leaderboardId);
		}

		public static void Share(string text, string contentURL, string thumbnailURL)
		{
			GameServicesManager.ToggleIdleTimerAndSounds(false);
			GameServicesManager.isWindowOpen = true;
			GameServicesManager.mGameServicesHelper.Share(text, contentURL, thumbnailURL);
		}

		public static bool IsUserAuthenticated()
		{
			return GameServicesManager.mGameServicesHelper.IsUserAuthenticated();
		}

		public static string GetUserId()
		{
			return GameServicesManager.mGameServicesHelper.GetUserId();
		}

		public static string GetAuthToken()
		{
			return GameServicesManager.mGameServicesHelper.GetAuthToken();
		}

		public static string GetUserDomain()
		{
			return GameServicesManager.mGameServicesHelper.GetUserDomain();
		}

		public static string GetExternalNetworkCode()
		{
			return GameServicesManager.mGameServicesHelper.GetExternalNetworkCode();
		}

		public void GameServicesUserIdCallback(string userId)
		{
			GameServicesManager.ToggleIdleTimerAndSounds(true);
			GameServicesManager.isWindowOpen = false;
			if (GameServicesManager.mGameServicesHelper != null)
			{
				GameServicesManager.mGameServicesHelper.HandleUserIdCallback(userId);
			}
		}

		public void GameServicesSignInFailedCallback(string errorCode)
		{
			if (Service.IsSet<EventManager>())
			{
				Service.Get<EventManager>().SendEvent(EventId.GameServicesSignedOut, AccountProvider.GOOGLEPLAY);
			}
		}

		public void GameServicesAuthTokenCallback(string authToken)
		{
			if (GameServicesManager.mGameServicesHelper != null)
			{
				GameServicesManager.mGameServicesHelper.HandleAuthTokenCallback(authToken);
			}
		}

		public GameServicesManager()
		{
		}

		public override void Unity_Serialize(int depth)
		{
		}

		public override void Unity_Deserialize(int depth)
		{
		}

		public override void Unity_RemapPPtrs(int depth)
		{
		}

		public override void Unity_NamedSerialize(int depth)
		{
		}

		public override void Unity_NamedDeserialize(int depth)
		{
		}

		protected internal GameServicesManager(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			GameServicesManager.AddScoreToLeaderboard(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			GameServicesManager.AttemptAutomaticSignInPrompt();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GameServicesManager)GCHandledObjects.GCHandleToObject(instance)).GameServicesAuthTokenCallback(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((GameServicesManager)GCHandledObjects.GCHandleToObject(instance)).GameServicesSignInFailedCallback(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((GameServicesManager)GCHandledObjects.GCHandleToObject(instance)).GameServicesUserIdCallback(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameServicesManager.GetAuthToken());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameServicesManager.GetExternalNetworkCode());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameServicesManager.GetUserDomain());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameServicesManager.GetUserId());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((GameServicesManager)GCHandledObjects.GCHandleToObject(instance)).HandleApplicationPause(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			GameServicesManager.Init();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameServicesManager.IsUserAuthenticated());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameServicesManager)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			GameServicesManager.OnReady();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			GameServicesManager.Share(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			GameServicesManager.ShowAchievements();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			GameServicesManager.ShowLeaderboard(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			GameServicesManager.SignIn();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			GameServicesManager.SignOut();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			GameServicesManager.Startup();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			GameServicesManager.StaticReset();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			GameServicesManager.ToggleIdleTimerAndSounds(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((GameServicesManager)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((GameServicesManager)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((GameServicesManager)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((GameServicesManager)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((GameServicesManager)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			GameServicesManager.UnlockAchievement(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
