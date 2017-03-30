using StaRTS.Externals.BI;
using StaRTS.Externals.DMOAnalytics;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using UnityEngine;

namespace StaRTS.Externals.GameServices
{
	public class GameServicesHelperAndroid : IGameServicesHelper
	{
		private const string GAME_SERVICE_USER_DOMAIN = "gp";

		private const string GAME_SERVICE_EXTERNAL_NETWORK_CODE = "googleplus";

		private AndroidJavaClass mGameServicesWrapper;

		private string userId;

		private string authToken;

		public GameServicesHelperAndroid()
		{
			this.mGameServicesWrapper = new AndroidJavaClass("com.disney.starts.GameServicesWrapper");
		}

		public void OnReady()
		{
			string accountProviderId = Service.Get<IAccountSyncController>().GetAccountProviderId(AccountProvider.GOOGLEPLAY);
			if (!string.IsNullOrEmpty(accountProviderId))
			{
				this.SignIn();
			}
		}

		public bool HasBeenPromptedForSignIn()
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			int pref = sharedPlayerPrefs.GetPref<int>("promptedForGoogleSignin");
			return pref > 0;
		}

		public bool IsUserAuthenticated()
		{
			bool result = false;
			if (this.mGameServicesWrapper != null)
			{
				result = this.mGameServicesWrapper.CallStatic<bool>("isConnected", new object[0]);
			}
			return result;
		}

		public string GetUserId()
		{
			return this.userId;
		}

		public string GetAuthToken()
		{
			return this.authToken;
		}

		public string GetUserDomain()
		{
			return "gp";
		}

		public string GetExternalNetworkCode()
		{
			return "googleplus";
		}

		public void SignIn()
		{
			if (this.mGameServicesWrapper != null)
			{
				SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
				sharedPlayerPrefs.SetPref("promptedForGoogleSignin", "1");
				Service.Get<ServerAPI>().Sync();
				this.mGameServicesWrapper.CallStatic("signIn", new object[0]);
			}
		}

		public void SignOut()
		{
			if (this.mGameServicesWrapper != null)
			{
				this.mGameServicesWrapper.CallStatic("signOut", new object[0]);
			}
			this.userId = null;
			this.authToken = null;
			Service.Get<EventManager>().SendEvent(EventId.GameServicesSignedOut, AccountProvider.GOOGLEPLAY);
		}

		public void UnlockAchievement(string achievementId)
		{
			if (this.IsUserAuthenticated() && this.mGameServicesWrapper != null)
			{
				this.mGameServicesWrapper.CallStatic("unlockAchievement", new object[]
				{
					achievementId
				});
			}
		}

		public void AddScoreToLeaderboard(int score, string leaderboardId)
		{
			if (this.IsUserAuthenticated() && this.mGameServicesWrapper != null)
			{
				this.mGameServicesWrapper.CallStatic("addScoreToLeaderboard", new object[]
				{
					score,
					leaderboardId
				});
			}
		}

		public void ShowAchievements()
		{
			if (this.IsUserAuthenticated() && this.mGameServicesWrapper != null)
			{
				this.mGameServicesWrapper.CallStatic("showAchievements", new object[0]);
			}
		}

		public void ShowLeaderboard(string leaderboardId)
		{
			if (this.IsUserAuthenticated() && this.mGameServicesWrapper != null)
			{
				this.mGameServicesWrapper.CallStatic("showLeaderboard", new object[]
				{
					leaderboardId
				});
			}
		}

		public void Share(string text, string contentURL, string thumbnailURL)
		{
			if (this.IsUserAuthenticated() && this.mGameServicesWrapper != null)
			{
				this.mGameServicesWrapper.CallStatic("share", new object[]
				{
					text,
					contentURL,
					thumbnailURL
				});
			}
		}

		public void HandleUserIdCallback(string userId)
		{
			if (Service.IsSet<BILoggingController>() && Service.IsSet<DMOAnalyticsController>())
			{
				this.userId = userId;
				userId = this.GetUserId();
				Service.Get<BILoggingController>().TrackNetworkMappingInfo("gp", userId);
				Service.Get<DMOAnalyticsController>().LogUserInfo("gp", userId);
			}
		}

		public void HandleAuthTokenCallback(string authToken)
		{
			if (Service.IsSet<EventManager>())
			{
				this.authToken = authToken;
				Service.Get<EventManager>().SendEvent(EventId.GameServicesSignedIn, AccountProvider.GOOGLEPLAY);
			}
		}
	}
}
