using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.FacebookApi
{
	public class FacebookManager : MonoBehaviour, IUnitySerializable
	{
		public const string FACEBOOK_APP_ID = "518856528223038";

		public const string FACEBOOK_APP_SECRET = "74e418b06a955513cf4cb2b71dd8b199";

		private static FacebookNativeController _FBController = null;

		private static FacebookManager _instance = null;

		private static FacebookInviteCompleteDelegate OnInviteCallback;

		private static FacebookDelegate OnPostCallback;

		private static bool mFacebookIsInitialized = false;

		public static List<Dictionary<string, object>> m_cachedFriends = new List<Dictionary<string, object>>();

		public static List<Dictionary<string, object>> m_cachedInvitableFriends = new List<Dictionary<string, object>>();

		public static FacebookDelegate OnFacebookLoggedIn
		{
			get;
			set;
		}

		public static FacebookDelegate OnFacebookLogInFailed
		{
			get;
			set;
		}

		public static bool IsLoggedIn
		{
			get
			{
				return FacebookManager.FBController != null && FacebookManager.FBController.IsLoggedIn;
			}
		}

		public static FacebookManager Instance
		{
			get
			{
				if (FacebookManager._instance == null)
				{
					GameObject gameObject = GameObject.Find("Facebook Manager");
					FacebookManager._instance = gameObject.GetComponent<FacebookManager>();
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				return FacebookManager._instance;
			}
		}

		public static FacebookNativeController FBController
		{
			get
			{
				if (FacebookManager._FBController == null)
				{
					FacebookManager._FBController = new FacebookNativeController();
					FacebookManager._FBController.Init();
				}
				return FacebookManager._FBController;
			}
		}

		public void StaticReset()
		{
			FacebookManager.OnFacebookLoggedIn = null;
			FacebookManager.OnFacebookLogInFailed = null;
			FacebookManager.mFacebookIsInitialized = false;
			FacebookManager.FBController.StaticReset();
		}

		public void Login()
		{
			Service.Get<GameIdleController>().Enabled = false;
			FacebookManager.FBController.Login(FacebookManager.OnFacebookLoggedIn, FacebookManager.OnFacebookLogInFailed);
		}

		public void LoginSilent()
		{
			Service.Get<GameIdleController>().Enabled = false;
			if (FacebookManager.IsLoggedIn)
			{
				FacebookManager.OnFacebookLogin();
			}
			Service.Get<GameIdleController>().Enabled = true;
		}

		public void Logout()
		{
			FacebookManager.FBController.Logout();
		}

		public static void GetFriends(FacebookGetFriendsCompleteDelegate callback)
		{
			FacebookManager.FBController.GetFriends(callback);
		}

		public static void GetSelfData(FacebookGetSelfDataCompleteDelegate callback)
		{
			FacebookManager.FBController.GetSelfData(callback);
		}

		public void InviteFriends(string message, string title, FacebookInviteCompleteDelegate callback)
		{
			Service.Get<GameIdleController>().Enabled = false;
			FacebookManager.OnInviteCallback = callback;
			FacebookManager.FBController.InviteFriends(message, title, new FacebookInviteCompleteDelegate(this.OnFriendsInvited));
		}

		private void OnFriendsInvited(List<string> recipientIDs)
		{
			if (FacebookManager.OnInviteCallback != null)
			{
				FacebookManager.OnInviteCallback(recipientIDs);
			}
			Service.Get<GameIdleController>().Enabled = true;
		}

		private void Start()
		{
			FacebookManager._instance = this;
		}

		public void OnFacebookInit()
		{
			FacebookManager.mFacebookIsInitialized = true;
			FacebookManager.FBController.OnFacebookInit();
		}

		private void OnHideUnity(bool isGameShown)
		{
			if (isGameShown)
			{
				Service.Get<GameIdleController>().Enabled = true;
				return;
			}
			Service.Get<GameIdleController>().Enabled = false;
		}

		public static void OnFacebookLogin()
		{
			if (FacebookManager.FBController.IsLoggedIn || !Service.Get<GameIdleController>().Enabled)
			{
				Service.Get<GameIdleController>().Enabled = true;
				if (FacebookManager.FBController.IsLoggedIn)
				{
					if (FacebookManager.OnFacebookLoggedIn != null)
					{
						FacebookManager.OnFacebookLoggedIn();
						return;
					}
				}
				else if (FacebookManager.OnFacebookLogInFailed != null)
				{
					FacebookManager.OnFacebookLogInFailed();
				}
			}
		}

		public void PostVideoToWall(string link, string linkName, string linkCaption, string pictureURL)
		{
			FacebookManager.FBController.PostVideoToWall(link, linkName, linkCaption, pictureURL);
		}

		private static void OnPost()
		{
			Service.Get<GameIdleController>().Enabled = true;
			if (FacebookManager.OnPostCallback != null)
			{
				FacebookManager.OnPostCallback();
				FacebookManager.OnPostCallback = null;
			}
		}

		public string getUserId()
		{
			return FacebookManager.FBController.getUserId();
		}

		public string getAccessToken()
		{
			return FacebookManager.FBController.getAccessToken();
		}

		public FacebookManager()
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

		protected internal FacebookManager(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FacebookManager.FBController);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FacebookManager.Instance);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FacebookManager.IsLoggedIn);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FacebookManager.OnFacebookLoggedIn);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FacebookManager.OnFacebookLogInFailed);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).getAccessToken());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			FacebookManager.GetFriends((FacebookGetFriendsCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			FacebookManager.GetSelfData((FacebookGetSelfDataCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).getUserId());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).InviteFriends(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (FacebookInviteCompleteDelegate)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).Login();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).LoginSilent();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).Logout();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).OnFacebookInit();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			FacebookManager.OnFacebookLogin();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).OnFriendsInvited((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).OnHideUnity(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			FacebookManager.OnPost();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).PostVideoToWall(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			FacebookManager.OnFacebookLoggedIn = (FacebookDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			FacebookManager.OnFacebookLogInFailed = (FacebookDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).StaticReset();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((FacebookManager)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
