using System;
using System.Collections.Generic;
using UnityEngine;

public class TapjoyPluginAndroid : MonoBehaviour
{
	private static string CONNECT_FLAG_DICTIONARY_NAME = "connectFlags";

	private static string SEGMENTS_DICTIONARY_NAME = "segmentationParams";

	private static AndroidJavaObject currentActivity;

	private static AndroidJavaClass tapjoyConnect;

	private static AndroidJavaObject tapjoyConnectInstance;

	private static AndroidJavaClass TapjoyConnect
	{
		get
		{
			if (TapjoyPluginAndroid.tapjoyConnect == null)
			{
				Debug.Log("C#: Loading TapjoyPlugin");
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				TapjoyPluginAndroid.currentActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				TapjoyPluginAndroid.tapjoyConnect = new AndroidJavaClass("com.tapjoy.TapjoyConnectUnity");
			}
			return TapjoyPluginAndroid.tapjoyConnect;
		}
	}

	private static AndroidJavaObject TapjoyConnectInstance
	{
		get
		{
			if (TapjoyPluginAndroid.tapjoyConnectInstance == null)
			{
				TapjoyPluginAndroid.tapjoyConnectInstance = TapjoyPluginAndroid.TapjoyConnect.CallStatic<AndroidJavaObject>("getTapjoyConnectInstance", new object[0]);
			}
			return TapjoyPluginAndroid.tapjoyConnectInstance;
		}
	}

	public static void SetCallbackHandler(string handlerName)
	{
		TapjoyPluginAndroid.TapjoyConnect.CallStatic("setHandlerClass", new object[]
		{
			handlerName
		});
	}

	public static void RequestTapjoyConnect(string appID, string secretKey)
	{
		TapjoyPluginAndroid.RequestTapjoyConnect(appID, secretKey, null);
	}

	public static void RequestTapjoyConnect(string appID, string secretKey, Dictionary<string, object> flags)
	{
		if (flags != null)
		{
			foreach (KeyValuePair<string, object> current in flags)
			{
				if (current.Value.GetType().IsGenericType)
				{
					Dictionary<string, object> dictionary = (Dictionary<string, object>)current.Value;
					string key = current.Key;
					TapjoyPluginAndroid.transferDictionaryToJavaWithName(dictionary, key);
					TapjoyPluginAndroid.TapjoyConnect.CallStatic("setDictionaryInDictionary", new object[]
					{
						current.Key,
						key,
						TapjoyPluginAndroid.CONNECT_FLAG_DICTIONARY_NAME
					});
				}
				else
				{
					TapjoyPluginAndroid.TapjoyConnect.CallStatic("setKeyValueInDictionary", new object[]
					{
						current.Key,
						current.Value,
						TapjoyPluginAndroid.CONNECT_FLAG_DICTIONARY_NAME
					});
				}
			}
		}
		TapjoyPluginAndroid.TapjoyConnect.CallStatic("requestTapjoyConnect", new object[]
		{
			TapjoyPluginAndroid.currentActivity,
			appID,
			secretKey
		});
	}

	public static void transferDictionaryToJavaWithName(Dictionary<string, object> dictionary, string dictionaryName)
	{
		foreach (KeyValuePair<string, object> current in dictionary)
		{
			TapjoyPluginAndroid.TapjoyConnect.CallStatic("setKeyValueInDictionary", new object[]
			{
				current.Key,
				current.Value,
				dictionaryName
			});
		}
	}

	public static void EnableLogging(bool enable)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.tapjoy.TapjoyLog");
		androidJavaClass.CallStatic("enableLogging", new object[]
		{
			enable
		});
	}

	public static void SendSegmentationParams(Dictionary<string, object> segmentationParams)
	{
		TapjoyPluginAndroid.transferDictionaryToJavaWithName(segmentationParams, TapjoyPluginAndroid.SEGMENTS_DICTIONARY_NAME);
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("sendSegmentationParams", new object[]
		{
			TapjoyPluginAndroid.SEGMENTS_DICTIONARY_NAME
		});
	}

	public static void AppPause()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("appPause", new object[0]);
	}

	public static void AppResume()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("appResume", new object[0]);
	}

	public static void ActionComplete(string actionID)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("actionComplete", new object[]
		{
			actionID
		});
	}

	public static void SetUserID(string userID)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("setUserID", new object[]
		{
			userID
		});
	}

	public static void ShowOffers()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("showOffers", new object[0]);
	}

	public static void GetTapPoints()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("getTapPoints", new object[0]);
	}

	public static void SpendTapPoints(int points)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("spendTapPoints", new object[]
		{
			points
		});
	}

	public static void AwardTapPoints(int points)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("awardTapPoints", new object[]
		{
			points
		});
	}

	public static int QueryTapPoints()
	{
		return TapjoyPluginAndroid.TapjoyConnectInstance.Call<int>("getTapPointsTotal", new object[0]);
	}

	public static void SetEarnedPointsNotifier()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("setEarnedPointsNotifier", new object[0]);
	}

	public static void ShowDefaultEarnedCurrencyAlert()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("showDefaultEarnedCurrencyAlert", new object[0]);
	}

	public static void GetDisplayAd()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("getDisplayAd", new object[0]);
	}

	public static void ShowDisplayAd()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("showDisplayAd", new object[0]);
	}

	public static void HideDisplayAd()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("hideDisplayAd", new object[0]);
	}

	public static void SetDisplayAdSize(string size)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("setDisplayAdSize", new object[]
		{
			size
		});
	}

	public static void EnableDisplayAdAutoRefresh(bool enable)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("enableDisplayAdAutoRefresh", new object[]
		{
			enable
		});
	}

	public static void RefreshDisplayAd()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("getDisplayAd", new object[0]);
	}

	public static void MoveDisplayAd(int x, int y)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("setDisplayAdPosition", new object[]
		{
			x,
			y
		});
	}

	public static void SetTransitionEffect(int transition)
	{
	}

	public static void GetFullScreenAd()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("getFullScreenAd", new object[0]);
	}

	public static void ShowFullScreenAd()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("showFullScreenAd", new object[0]);
	}

	public static void SendShutDownEvent()
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("sendShutDownEvent", new object[0]);
	}

	public static void SendIAPEvent(string name, float price, int quantity, string currencyCode)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("sendIAPEvent", new object[]
		{
			name,
			price,
			quantity,
			currencyCode
		});
	}

	public static void ShowOffersWithCurrencyID(string currencyID, bool selector)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("showOffersWithCurrencyID", new object[]
		{
			currencyID,
			selector
		});
	}

	public static void GetDisplayAdWithCurrencyID(string currencyID)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("getDisplayAdWithCurrencyID", new object[]
		{
			currencyID
		});
	}

	public static void GetFullScreenAdWithCurrencyID(string currencyID)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("getFullScreenAdWithCurrencyID", new object[]
		{
			currencyID
		});
	}

	public static void SetCurrencyMultiplier(float multiplier)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("setCurrencyMultiplier", new object[]
		{
			multiplier
		});
	}

	public static void CreateEvent(string eventGuid, string eventName, string eventParameter)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("createEventWithGuid", new object[]
		{
			eventGuid,
			eventName,
			eventParameter
		});
	}

	public static void SendEvent(string eventGuid)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("sendEventWithGuid", new object[]
		{
			eventGuid
		});
	}

	public static void ShowEvent(string eventGuid)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("showEventWithGuid", new object[]
		{
			eventGuid
		});
	}

	public static void EnableEventAutoPresent(string eventGuid, bool autoPresent)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("enableEventAutoPresent", new object[]
		{
			eventGuid,
			autoPresent
		});
	}

	public static void EnableEventPreload(string eventGuid, bool preload)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("enableEventPreload", new object[]
		{
			eventGuid,
			preload
		});
	}

	public static void EventRequestCompleted(string eventGuid)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("eventRequestCompleted", new object[]
		{
			eventGuid
		});
	}

	public static void EventRequestCancelled(string eventGuid)
	{
		TapjoyPluginAndroid.TapjoyConnectInstance.Call("eventRequestCancelled", new object[]
		{
			eventGuid
		});
	}
}
