using StaRTS.Externals.BI;
using StaRTS.Externals.DMOAnalytics;
using StaRTS.Externals.FacebookApi;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Leaderboard;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class SocialDataController : ISocialDataController
	{
		private const string KEY_FRIEND_DATA_LIST = "data";

		private const string KEY_GENDER = "gender";

		private const string KEY_FULL_NAME = "name";

		private const string KEY_FIRST_NAME = "first_name";

		private const string KEY_LAST_NAME = "last_name";

		private const string KEY_LOCALE = "locale";

		private const string KEY_ID = "id";

		private const string KEY_RECIPIENTS = "to";

		private List<OnAllDataFetchedDelegate> allDataFetchedCallbacks;

		public OnRequestDelegate InviteFriendsCB
		{
			get;
			set;
		}

		public OnFBFriendsDelegate FriendsDetailsCB
		{
			get;
			set;
		}

		public bool HaveAllData
		{
			get
			{
				return this.HaveFriendData && this.HaveSelfData;
			}
		}

		public bool HaveFriendData
		{
			get;
			private set;
		}

		public bool HaveSelfData
		{
			get;
			private set;
		}

		public string Gender
		{
			get;
			private set;
		}

		public string FullName
		{
			get;
			private set;
		}

		public string FirstName
		{
			get;
			private set;
		}

		public string LastName
		{
			get;
			private set;
		}

		public string FacebookLocale
		{
			get;
			private set;
		}

		public string FacebookId
		{
			get;
			private set;
		}

		public List<SocialFriendData> Friends
		{
			get;
			private set;
		}

		public Dictionary<string, SocialFriendData> PlayerIdToFriendData
		{
			get;
			private set;
		}

		public List<string> InstalledFBIDs
		{
			get;
			private set;
		}

		public bool IsLoggedIn
		{
			get
			{
				return FacebookManager.IsLoggedIn;
			}
		}

		public bool IsLoggingIn
		{
			get;
			private set;
		}

		public SocialDataController()
		{
			this.allDataFetchedCallbacks = new List<OnAllDataFetchedDelegate>();
			this.HaveFriendData = false;
			this.HaveSelfData = false;
			this.IsLoggingIn = false;
			Service.Set<ISocialDataController>(this);
		}

		public void StaticReset()
		{
			FacebookManager.Instance.StaticReset();
		}

		public void PopulateFacebookData()
		{
			if (FacebookManager.IsLoggedIn)
			{
				FacebookManager.GetFriends(new FacebookGetFriendsCompleteDelegate(this.OnGetFriendsData));
				FacebookManager.GetSelfData(new FacebookGetSelfDataCompleteDelegate(this.OnGetSelfData));
			}
		}

		public void UpdateFriends()
		{
			string friendIds = "";
			if (FacebookManager.IsLoggedIn && this.InstalledFBIDs != null && this.InstalledFBIDs.Count > 0)
			{
				friendIds = string.Join(",", this.InstalledFBIDs.ToArray());
			}
			Service.Get<LeaderboardController>().UpdateFriends(friendIds, new LeaderboardController.OnUpdateData(this.OnGetFriendsData));
		}

		private void OnGetFriendsData(List<Dictionary<string, object>> friends)
		{
			this.Friends = new List<SocialFriendData>();
			this.PlayerIdToFriendData = new Dictionary<string, SocialFriendData>();
			this.InstalledFBIDs = new List<string>();
			if (friends != null)
			{
				for (int i = 0; i < friends.Count; i++)
				{
					SocialFriendData socialFriendData = (SocialFriendData)new SocialFriendData().FromFriendObject(friends[i]);
					this.Friends.Add(socialFriendData);
					if (socialFriendData.Installed)
					{
						this.InstalledFBIDs.Add(socialFriendData.Id);
					}
				}
				this.CommonFriendDataActions();
				return;
			}
			this.OnGetFriendsData(false);
			Service.Get<StaRTSLogger>().ErrorFormat("Error fetching FB friends data", new object[0]);
		}

		private void CallFriendsDetailsCB()
		{
			OnFBFriendsDelegate friendsDetailsCB = this.FriendsDetailsCB;
			this.FriendsDetailsCB = null;
			if (friendsDetailsCB != null)
			{
				friendsDetailsCB();
			}
		}

		private void OnGetFriendsData(bool success)
		{
			if (!success)
			{
				this.CommonFriendDataActions();
				return;
			}
			if (this.Friends == null)
			{
				this.CallFriendsDetailsCB();
				return;
			}
			List<PlayerLBEntity> list = Service.Get<LeaderboardController>().Friends.List;
			Dictionary<string, PlayerLBEntity> dictionary = new Dictionary<string, PlayerLBEntity>();
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				PlayerLBEntity playerLBEntity = list[i];
				if (!string.IsNullOrEmpty(playerLBEntity.SocialID) && !dictionary.ContainsKey(playerLBEntity.SocialID))
				{
					dictionary.Add(playerLBEntity.SocialID, playerLBEntity);
				}
				i++;
			}
			int j = 0;
			int count2 = this.Friends.Count;
			while (j < count2)
			{
				SocialFriendData socialFriendData = this.Friends[j];
				if (dictionary.ContainsKey(socialFriendData.Id))
				{
					socialFriendData.PlayerData = dictionary[socialFriendData.Id];
					if (!this.PlayerIdToFriendData.ContainsKey(socialFriendData.PlayerData.PlayerID))
					{
						this.PlayerIdToFriendData.Add(socialFriendData.PlayerData.PlayerID, socialFriendData);
					}
				}
				j++;
			}
			dictionary.Clear();
			this.CommonFriendDataActions();
		}

		private void CommonFriendDataActions()
		{
			this.CallFriendsDetailsCB();
			this.HaveFriendData = true;
			this.DoCallbacksIfHaveAllData();
		}

		private void OnGetSelfData(Dictionary<string, object> responseData)
		{
			this.HaveSelfData = true;
			if (responseData.ContainsKey("gender"))
			{
				this.Gender = (string)responseData["gender"];
			}
			if (responseData.ContainsKey("name"))
			{
				this.FullName = (string)responseData["name"];
			}
			if (responseData.ContainsKey("locale"))
			{
				this.FacebookLocale = (string)responseData["locale"];
			}
			if (responseData.ContainsKey("first_name"))
			{
				this.FirstName = (string)responseData["first_name"];
			}
			if (responseData.ContainsKey("last_name"))
			{
				this.LastName = (string)responseData["last_name"];
			}
			if (responseData.ContainsKey("id"))
			{
				this.FacebookId = (string)responseData["id"];
				if (!string.IsNullOrEmpty(this.FacebookId))
				{
					Service.Get<BILoggingController>().TrackNetworkMappingInfo("f", this.FacebookId);
					Service.Get<DMOAnalyticsController>().LogUserInfo("fb", this.FacebookId);
				}
			}
			this.DoCallbacksIfHaveAllData();
			this.FacebookLoginLogicAfterGetSelfDataResponse();
		}

		private void DoCallbacksIfHaveAllData()
		{
			if (this.HaveAllData)
			{
				ProcessingScreen.Hide();
				for (int i = 0; i < this.allDataFetchedCallbacks.Count; i++)
				{
					this.allDataFetchedCallbacks[i]();
				}
				this.allDataFetchedCallbacks.Clear();
			}
		}

		public void InviteFriends(OnRequestDelegate callback)
		{
			if (callback != null)
			{
				this.InviteFriendsCB = callback;
			}
			Lang lang = Service.Get<Lang>();
			string message = lang.Get("FB_INVITE_MESSAGE", new object[0]);
			string title = lang.Get("FB_INVITE_TITLE", new object[0]);
			FacebookManager.Instance.InviteFriends(message, title, new FacebookInviteCompleteDelegate(this.OnFacebookInviteFriends));
		}

		private void OnFacebookInviteFriends(List<string> recipientIDs)
		{
			if (this.InviteFriendsCB != null)
			{
				this.InviteFriendsCB();
			}
			string trackingCode = "InviteRequest";
			if (recipientIDs != null && recipientIDs.Count > 0)
			{
				int count = recipientIDs.Count;
				string text = "";
				for (int i = 0; i < count; i++)
				{
					text += recipientIDs[i];
					if (i < count - 1)
					{
						text += ",";
					}
				}
				Service.Get<BILoggingController>().TrackSendMessage(trackingCode, text, count);
			}
		}

		public void GetSelfPicture(OnGetProfilePicture callback, object cookie)
		{
			Service.Get<Engine>().StartCoroutine(this.DownloadProfileImageCoroutine("https://graph.facebook.com/" + this.FacebookId + "/picture?type=square", callback, cookie));
		}

		public void GetFriendPicture(SocialFriendData friend, OnGetProfilePicture callback, object cookie)
		{
			Service.Get<Engine>().StartCoroutine(this.DownloadProfileImageCoroutine(friend.PictureURL, callback, cookie));
		}

		[IteratorStateMachine(typeof(SocialDataController.<DownloadProfileImageCoroutine>d__83))]
		private IEnumerator DownloadProfileImageCoroutine(string url, OnGetProfilePicture callback, object cookie)
		{
			WWW wWW = new WWW(url);
			WWWManager.Add(wWW);
			yield return wWW;
			if (!WWWManager.Remove(wWW))
			{
				yield break;
			}
			string error = wWW.error;
			if (string.IsNullOrEmpty(error))
			{
				callback(wWW.texture, cookie);
			}
			else
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Error fetching picture at {0}", new object[]
				{
					url
				});
			}
			wWW.Dispose();
			yield break;
		}

		public void DestroyFriendPicture(Texture2D texture)
		{
			UnityEngine.Object.Destroy(texture);
		}

		public void Logout()
		{
			this.HaveFriendData = false;
			this.HaveSelfData = false;
			this.Friends = null;
			this.PlayerIdToFriendData = null;
			Service.Get<LeaderboardController>().Friends.List.Clear();
			FacebookManager.Instance.Logout();
			Service.Get<IAccountSyncController>().UnregisterFacebookAccount();
		}

		public void CheckFacebookLoginOnStartup()
		{
			FacebookManager.Instance.LoginSilent();
		}

		public void Login(OnAllDataFetchedDelegate callback)
		{
			if (this.HaveAllData)
			{
				callback();
				return;
			}
			if (this.IsLoggedIn)
			{
				return;
			}
			this.allDataFetchedCallbacks.Add(callback);
			ProcessingScreen.Show();
			Service.Get<Engine>().ForceGarbageCollection(null);
			FacebookManager.OnFacebookLoggedIn = new FacebookDelegate(this.OnFacebookLogin);
			FacebookManager.OnFacebookLogInFailed = new FacebookDelegate(this.OnFacebookLoginFailed);
			FacebookManager.Instance.Login();
			Service.Get<BILoggingController>().TrackAuthorization("allow", "f");
		}

		private void OnFacebookLogin()
		{
			this.IsLoggingIn = false;
			if (FacebookManager.IsLoggedIn)
			{
				FacebookManager.GetSelfData(new FacebookGetSelfDataCompleteDelegate(this.OnGetSelfData));
			}
		}

		private void FacebookLoginLogicAfterGetSelfDataResponse()
		{
			string @string = PlayerPrefs.GetString("FB_ID", null);
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			string text = null;
			if (sharedPlayerPrefs != null)
			{
				text = sharedPlayerPrefs.GetPref<string>("FB_ID");
			}
			if ((!string.IsNullOrEmpty(text) && text.CompareTo(FacebookManager.Instance.getUserId()) != 0) || (!string.IsNullOrEmpty(@string) && @string.CompareTo(FacebookManager.Instance.getUserId()) != 0))
			{
				ProcessingScreen.Hide();
				Lang lang = Service.Get<Lang>();
				string title = lang.Get("IAP_DISABLED_ANDROID_TITLE", new object[0]);
				string message = lang.Get("FACEBOOK_YOU_ARE_USING_ANOTHER_ACCOUNT", new object[0]);
				AlertScreen.ShowModal(false, title, message, new OnScreenModalResult(this.OnFacebookOverrideWarningClosed), null);
				return;
			}
			this.FinishFacebookLogin();
		}

		private void FinishFacebookLogin()
		{
			if (FacebookManager.IsLoggedIn)
			{
				PlayerPrefs.SetString("FB_ID", FacebookManager.Instance.getUserId());
				PlayerPrefs.Save();
				FacebookManager.GetFriends(new FacebookGetFriendsCompleteDelegate(this.OnGetFriendsData));
			}
			Service.Get<IAccountSyncController>().OnFacebookSignIn();
		}

		private void OnFacebookOverrideWarningClosed(object result, object cookie)
		{
			if (result == null || !(bool)result)
			{
				FacebookManager.Instance.Logout();
				Service.Get<EventManager>().SendEvent(EventId.FacebookLoggedOut, null);
				return;
			}
			this.FinishFacebookLogin();
		}

		private void OnFacebookLoginFailed()
		{
			this.IsLoggingIn = false;
			ProcessingScreen.Hide();
			this.allDataFetchedCallbacks.Clear();
		}

		protected internal SocialDataController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).CallFriendsDetailsCB();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).CheckFacebookLoginOnStartup();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).CommonFriendDataActions();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).DestroyFriendPicture((Texture2D)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).DoCallbacksIfHaveAllData();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).DownloadProfileImageCoroutine(Marshal.PtrToStringUni(*(IntPtr*)args), (OnGetProfilePicture)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).FacebookLoginLogicAfterGetSelfDataResponse();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).FinishFacebookLogin();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).FacebookId);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).FacebookLocale);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).FirstName);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).Friends);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).FriendsDetailsCB);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).FullName);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).Gender);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).HaveAllData);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).HaveFriendData);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).HaveSelfData);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).InstalledFBIDs);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).InviteFriendsCB);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).IsLoggedIn);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).IsLoggingIn);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).LastName);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).PlayerIdToFriendData);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).GetFriendPicture((SocialFriendData)GCHandledObjects.GCHandleToObject(*args), (OnGetProfilePicture)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).GetSelfPicture((OnGetProfilePicture)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).InviteFriends((OnRequestDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).Login((OnAllDataFetchedDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).Logout();
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).OnFacebookInviteFriends((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).OnFacebookLogin();
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).OnFacebookLoginFailed();
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).OnFacebookOverrideWarningClosed(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).OnGetFriendsData(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).OnGetFriendsData((List<Dictionary<string, object>>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).OnGetSelfData((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).PopulateFacebookData();
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).FacebookId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).FacebookLocale = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).FirstName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).Friends = (List<SocialFriendData>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).FriendsDetailsCB = (OnFBFriendsDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).FullName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).Gender = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).HaveFriendData = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).HaveSelfData = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).InstalledFBIDs = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).InviteFriendsCB = (OnRequestDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).IsLoggingIn = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).LastName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).PlayerIdToFriendData = (Dictionary<string, SocialFriendData>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).StaticReset();
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((SocialDataController)GCHandledObjects.GCHandleToObject(instance)).UpdateFriends();
			return -1L;
		}
	}
}
