using Facebook.MiniJSON;
using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.FacebookApi
{
	public class FacebookNativeController
	{
		public delegate void NativeLogin(FacebookDelegate onSuccess, FacebookDelegate onFailure);

		public delegate void NativeLogout();

		public delegate bool NativeIsLoggedIn();

		public delegate string NativeGetString();

		public delegate Dictionary<string, object> NativeGetSelfData();

		public delegate void NativeInviteFriends(string message, string title, FacebookInviteCompleteDelegate callback);

		public delegate void NativePostToWall(string link, string linkName, string linkCaption, string pictureURL);

		public delegate void APIDelegate(string url, HttpMethod method, Action<GraphResult> callback, Dictionary<string, string> formData);

		public static FacebookNativeController.NativeLogin m_NativeLogin;

		public static FacebookNativeController.NativeLogout m_NativeLogout;

		public static FacebookNativeController.NativeIsLoggedIn m_NativeIsLoggedIn;

		public static FacebookNativeController.NativeGetString m_NativeGetUserID;

		public static FacebookNativeController.NativeGetString m_NativeGetAccessToken;

		public static FacebookNativeController.NativeGetSelfData m_NativeGetSelfData;

		public static FacebookNativeController.NativeInviteFriends m_NativeInviteFriends;

		public static FacebookNativeController.NativePostToWall m_NativePostToWall;

		public static FacebookNativeController.APIDelegate m_APINativeCall;

		private bool m_isUserFriendsGetRequestPending;

		private bool m_isLoggingIn;

		protected FacebookGetFriendsCompleteDelegate m_getFriendsCompleteCallback;

		protected FacebookGetSelfDataCompleteDelegate m_getSelfDataCompleteCallback;

		public bool IsLoggedIn
		{
			get
			{
				bool result = false;
				if (FacebookNativeController.m_NativeIsLoggedIn != null)
				{
					result = FacebookNativeController.m_NativeIsLoggedIn();
				}
				return result;
			}
			set
			{
				this.IsLoggedIn = value;
			}
		}

		public FacebookNativeController()
		{
		}

		public void StaticReset()
		{
		}

		public void Login(FacebookDelegate loginSuccessCallback, FacebookDelegate loginFailCallback)
		{
			if (FacebookNativeController.m_NativeLogin != null)
			{
				FacebookNativeController.m_NativeLogin(loginSuccessCallback, loginFailCallback);
				return;
			}
			if (loginFailCallback != null)
			{
				loginFailCallback();
			}
		}

		public void Logout()
		{
			if (FacebookNativeController.m_NativeLogout != null)
			{
				FacebookNativeController.m_NativeLogout();
			}
		}

		public void GetFriends(FacebookGetFriendsCompleteDelegate callback)
		{
			if (this.m_isUserFriendsGetRequestPending)
			{
				Debug.LogWarning(string.Format("[FacebookManager] - RequestUserFriend: Request already queue, aborting...", new object[0]));
				return;
			}
			this.m_isUserFriendsGetRequestPending = true;
			this.RequestUserFriendsByUrl("/me/friends", delegate(List<Dictionary<string, object>> userList)
			{
				this.m_isUserFriendsGetRequestPending = false;
				callback(userList);
			});
		}

		private List<Dictionary<string, object>> RequestUserFriendsByUrl(string url, Action<List<Dictionary<string, object>>> callback)
		{
			List<Dictionary<string, object>> auxList = new List<Dictionary<string, object>>();
			Dictionary<string, string> formData = new Dictionary<string, string>
			{
				{
					"fields",
					"name,id,picture.type(square),installed"
				}
			};
			this.APIcallback(url, HttpMethod.GET, delegate(Dictionary<string, object> responseDict, string error)
			{
				if (string.IsNullOrEmpty(error))
				{
					if (responseDict != null)
					{
						bool flag = "/me/friends".Equals(url);
						if ((responseDict["data"] as List<object>).Count > 0)
						{
							using (List<object>.Enumerator enumerator = (responseDict["data"] as List<object>).GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									Dictionary<string, object> dictionary = (Dictionary<string, object>)enumerator.Current;
									string value = string.Empty;
									if (dictionary.ContainsKey("picture"))
									{
										Dictionary<string, object> dictionary2 = (dictionary["picture"] as Dictionary<string, object>)["data"] as Dictionary<string, object>;
										value = (dictionary2["url"] as string);
									}
									Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
									dictionary3["id"] = dictionary["id"];
									dictionary3["name"] = dictionary["name"];
									dictionary3["installed"] = dictionary["installed"];
									dictionary3["picture"] = value;
									auxList.Add(dictionary3);
								}
								goto IL_137;
							}
						}
						Debug.LogWarning("[FacebookManager] - RequestUserFriendsByUrl: no playing friends");
						IL_137:
						if (flag)
						{
							FacebookManager.m_cachedFriends.Clear();
							FacebookManager.m_cachedFriends = auxList;
						}
						else if ("/me/invitable_friends".Equals(url))
						{
							FacebookManager.m_cachedInvitableFriends.Clear();
							FacebookManager.m_cachedInvitableFriends = auxList;
						}
						else
						{
							Debug.LogError("[FacebookManager] - RequestUserFriendsByUrl: unsupported url");
							if (callback != null)
							{
								auxList.Clear();
							}
						}
					}
					if (callback != null)
					{
						callback.Invoke(auxList);
						return;
					}
				}
				else
				{
					Debug.LogError(string.Format("[FacebookManager] - RequestUserFriendsByUrl: error {0}", new object[]
					{
						error
					}));
					if (callback != null)
					{
						auxList.Clear();
						callback.Invoke(auxList);
					}
				}
			}, formData);
			return auxList;
		}

		public void GetSelfData(FacebookGetSelfDataCompleteDelegate callback)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			if (FacebookNativeController.m_NativeGetSelfData != null)
			{
				data = FacebookNativeController.m_NativeGetSelfData();
			}
			callback(data);
		}

		public void InviteFriends(string message, string title, FacebookInviteCompleteDelegate callback)
		{
			if (FacebookNativeController.m_NativeInviteFriends != null)
			{
				FacebookNativeController.m_NativeInviteFriends(message, title, callback);
			}
		}

		public void Init()
		{
		}

		public void OnFacebookInit()
		{
		}

		protected void OnHideUnity(bool isGameShown)
		{
			if (isGameShown)
			{
				Service.Get<GameIdleController>().Enabled = true;
				return;
			}
			Service.Get<GameIdleController>().Enabled = false;
		}

		public string getUserId()
		{
			if (FacebookNativeController.m_NativeGetUserID != null)
			{
				return FacebookNativeController.m_NativeGetUserID();
			}
			return "";
		}

		public string getAccessToken()
		{
			if (FacebookNativeController.m_NativeGetAccessToken != null)
			{
				return FacebookNativeController.m_NativeGetAccessToken();
			}
			return "";
		}

		public void PostVideoToWall(string link, string linkName, string linkCaption, string pictureURL)
		{
			if (FacebookNativeController.m_NativePostToWall != null)
			{
				FacebookNativeController.m_NativePostToWall(link, linkName, linkCaption, pictureURL);
			}
		}

		public static void APICall(string url, HttpMethod method, Action<IGraphResult> callback, Dictionary<string, string> formData = null)
		{
			if (FacebookNativeController.m_APINativeCall != null)
			{
				FacebookNativeController.m_APINativeCall(url, method, delegate(GraphResult response)
				{
					response.DownloadTexture(callback);
				}, formData);
				return;
			}
			callback.Invoke(new GraphResult("Not available", null));
		}

		protected bool APIcallback(string url, HttpMethod method, Action<Dictionary<string, object>, string> callback, Dictionary<string, string> formData = null)
		{
			if (!FacebookManager.IsLoggedIn)
			{
				callback.Invoke(null, "User is not Logged in");
				return false;
			}
			bool result;
			try
			{
				FacebookNativeController.APICall(url, method, delegate(IGraphResult response)
				{
					Dictionary<string, object> dictionary = null;
					string text2 = string.Empty;
					if (response != null && string.IsNullOrEmpty(response.Error))
					{
						try
						{
							dictionary = (Json.Deserialize(response.RawResult) as Dictionary<string, object>);
							if (dictionary == null)
							{
								text2 = string.Format("[FacebookManager] - GetGenericAPIcall: No reponse received?!?! for url: {0} - {1}", new object[]
								{
									url,
									response.RawResult
								});
								Debug.LogWarning(text2);
							}
							goto IL_AC;
						}
						catch (Exception ex2)
						{
							text2 = string.Format("[FacebookManager] - GetGenericAPIcall: raw response {0}, error: {1}", new object[]
							{
								response.RawResult,
								ex2.get_Message()
							});
							Debug.LogWarning(text2);
							goto IL_AC;
						}
					}
					text2 = string.Format("[FacebookManager] - GetGenericAPIcall: returns url: {0} with error: {1}", new object[]
					{
						url,
						response.Error
					});
					Debug.LogWarning(text2);
					IL_AC:
					callback.Invoke(dictionary, text2);
				}, formData);
				result = true;
			}
			catch (Exception ex)
			{
				string text = string.Format("[FacebookManager] - GetGenericAPIcall: exception caught: {0}", new object[]
				{
					ex.get_Message()
				});
				Debug.LogError(text);
				if (callback != null)
				{
					callback.Invoke(null, text);
				}
				result = false;
			}
			return result;
		}

		protected internal FacebookNativeController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			FacebookNativeController.APICall(Marshal.PtrToStringUni(*(IntPtr*)args), (HttpMethod)(*(int*)(args + 1)), (Action<IGraphResult>)GCHandledObjects.GCHandleToObject(args[2]), (Dictionary<string, string>)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).APIcallback(Marshal.PtrToStringUni(*(IntPtr*)args), (HttpMethod)(*(int*)(args + 1)), (Action<Dictionary<string, object>, string>)GCHandledObjects.GCHandleToObject(args[2]), (Dictionary<string, string>)GCHandledObjects.GCHandleToObject(args[3])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).IsLoggedIn);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).getAccessToken());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).GetFriends((FacebookGetFriendsCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).GetSelfData((FacebookGetSelfDataCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).getUserId());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).InviteFriends(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (FacebookInviteCompleteDelegate)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).Login((FacebookDelegate)GCHandledObjects.GCHandleToObject(*args), (FacebookDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).Logout();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).OnFacebookInit();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).OnHideUnity(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).PostVideoToWall(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).RequestUserFriendsByUrl(Marshal.PtrToStringUni(*(IntPtr*)args), (Action<List<Dictionary<string, object>>>)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).IsLoggedIn = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((FacebookNativeController)GCHandledObjects.GCHandleToObject(instance)).StaticReset();
			return -1L;
		}
	}
}
