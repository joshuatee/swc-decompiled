using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TapjoyPlugin : MonoBehaviour
{
	public const string MacAddressOptionOn = "0";

	public const string MacAddressOptionOffWithVersionCheck = "1";

	public const string MacAddressOptionOff = "2";

	private static Dictionary<string, TapjoyEvent> eventDictionary = new Dictionary<string, TapjoyEvent>();

	public static event Action connectCallSucceeded
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.connectCallSucceeded = (Action)Delegate.Combine(TapjoyPlugin.connectCallSucceeded, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.connectCallSucceeded = (Action)Delegate.Remove(TapjoyPlugin.connectCallSucceeded, value);
		}
	}

	public static event Action connectCallFailed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.connectCallFailed = (Action)Delegate.Combine(TapjoyPlugin.connectCallFailed, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.connectCallFailed = (Action)Delegate.Remove(TapjoyPlugin.connectCallFailed, value);
		}
	}

	public static event Action<int> getTapPointsSucceeded
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.getTapPointsSucceeded = (Action<int>)Delegate.Combine(TapjoyPlugin.getTapPointsSucceeded, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.getTapPointsSucceeded = (Action<int>)Delegate.Remove(TapjoyPlugin.getTapPointsSucceeded, value);
		}
	}

	public static event Action getTapPointsFailed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.getTapPointsFailed = (Action)Delegate.Combine(TapjoyPlugin.getTapPointsFailed, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.getTapPointsFailed = (Action)Delegate.Remove(TapjoyPlugin.getTapPointsFailed, value);
		}
	}

	public static event Action<int> spendTapPointsSucceeded
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.spendTapPointsSucceeded = (Action<int>)Delegate.Combine(TapjoyPlugin.spendTapPointsSucceeded, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.spendTapPointsSucceeded = (Action<int>)Delegate.Remove(TapjoyPlugin.spendTapPointsSucceeded, value);
		}
	}

	public static event Action spendTapPointsFailed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.spendTapPointsFailed = (Action)Delegate.Combine(TapjoyPlugin.spendTapPointsFailed, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.spendTapPointsFailed = (Action)Delegate.Remove(TapjoyPlugin.spendTapPointsFailed, value);
		}
	}

	public static event Action awardTapPointsSucceeded
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.awardTapPointsSucceeded = (Action)Delegate.Combine(TapjoyPlugin.awardTapPointsSucceeded, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.awardTapPointsSucceeded = (Action)Delegate.Remove(TapjoyPlugin.awardTapPointsSucceeded, value);
		}
	}

	public static event Action awardTapPointsFailed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.awardTapPointsFailed = (Action)Delegate.Combine(TapjoyPlugin.awardTapPointsFailed, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.awardTapPointsFailed = (Action)Delegate.Remove(TapjoyPlugin.awardTapPointsFailed, value);
		}
	}

	public static event Action<int> tapPointsEarned
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.tapPointsEarned = (Action<int>)Delegate.Combine(TapjoyPlugin.tapPointsEarned, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.tapPointsEarned = (Action<int>)Delegate.Remove(TapjoyPlugin.tapPointsEarned, value);
		}
	}

	public static event Action getFullScreenAdSucceeded
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.getFullScreenAdSucceeded = (Action)Delegate.Combine(TapjoyPlugin.getFullScreenAdSucceeded, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.getFullScreenAdSucceeded = (Action)Delegate.Remove(TapjoyPlugin.getFullScreenAdSucceeded, value);
		}
	}

	public static event Action getFullScreenAdFailed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.getFullScreenAdFailed = (Action)Delegate.Combine(TapjoyPlugin.getFullScreenAdFailed, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.getFullScreenAdFailed = (Action)Delegate.Remove(TapjoyPlugin.getFullScreenAdFailed, value);
		}
	}

	public static event Action getDisplayAdSucceeded
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.getDisplayAdSucceeded = (Action)Delegate.Combine(TapjoyPlugin.getDisplayAdSucceeded, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.getDisplayAdSucceeded = (Action)Delegate.Remove(TapjoyPlugin.getDisplayAdSucceeded, value);
		}
	}

	public static event Action getDisplayAdFailed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.getDisplayAdFailed = (Action)Delegate.Combine(TapjoyPlugin.getDisplayAdFailed, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.getDisplayAdFailed = (Action)Delegate.Remove(TapjoyPlugin.getDisplayAdFailed, value);
		}
	}

	public static event Action videoAdStarted
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.videoAdStarted = (Action)Delegate.Combine(TapjoyPlugin.videoAdStarted, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.videoAdStarted = (Action)Delegate.Remove(TapjoyPlugin.videoAdStarted, value);
		}
	}

	public static event Action videoAdFailed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.videoAdFailed = (Action)Delegate.Combine(TapjoyPlugin.videoAdFailed, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.videoAdFailed = (Action)Delegate.Remove(TapjoyPlugin.videoAdFailed, value);
		}
	}

	public static event Action videoAdCompleted
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.videoAdCompleted = (Action)Delegate.Combine(TapjoyPlugin.videoAdCompleted, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.videoAdCompleted = (Action)Delegate.Remove(TapjoyPlugin.videoAdCompleted, value);
		}
	}

	public static event Action showOffersFailed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.showOffersFailed = (Action)Delegate.Combine(TapjoyPlugin.showOffersFailed, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.showOffersFailed = (Action)Delegate.Remove(TapjoyPlugin.showOffersFailed, value);
		}
	}

	public static event Action<TapjoyViewType> viewOpened
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.viewOpened = (Action<TapjoyViewType>)Delegate.Combine(TapjoyPlugin.viewOpened, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.viewOpened = (Action<TapjoyViewType>)Delegate.Remove(TapjoyPlugin.viewOpened, value);
		}
	}

	public static event Action<TapjoyViewType> viewClosed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			TapjoyPlugin.viewClosed = (Action<TapjoyViewType>)Delegate.Combine(TapjoyPlugin.viewClosed, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			TapjoyPlugin.viewClosed = (Action<TapjoyViewType>)Delegate.Remove(TapjoyPlugin.viewClosed, value);
		}
	}

	private void Awake()
	{
		base.gameObject.name = base.GetType().ToString();
		TapjoyPlugin.SetCallbackHandler(base.gameObject.name);
		Debug.Log("C#: UnitySendMessage directs to " + base.gameObject.name);
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void TapjoyConnectSuccess(string message)
	{
		if (TapjoyPlugin.connectCallSucceeded != null)
		{
			TapjoyPlugin.connectCallSucceeded();
		}
	}

	public void TapjoyConnectFail(string message)
	{
		if (TapjoyPlugin.connectCallFailed != null)
		{
			TapjoyPlugin.connectCallFailed();
		}
	}

	public void TapPointsLoaded(string message)
	{
		if (TapjoyPlugin.getTapPointsSucceeded != null)
		{
			TapjoyPlugin.getTapPointsSucceeded(int.Parse(message));
		}
	}

	public void TapPointsLoadedError(string message)
	{
		if (TapjoyPlugin.getTapPointsFailed != null)
		{
			TapjoyPlugin.getTapPointsFailed();
		}
	}

	public void TapPointsSpent(string message)
	{
		if (TapjoyPlugin.spendTapPointsSucceeded != null)
		{
			TapjoyPlugin.spendTapPointsSucceeded(int.Parse(message));
		}
	}

	public void TapPointsSpendError(string message)
	{
		if (TapjoyPlugin.spendTapPointsFailed != null)
		{
			TapjoyPlugin.spendTapPointsFailed();
		}
	}

	public void TapPointsAwarded(string message)
	{
		if (TapjoyPlugin.awardTapPointsSucceeded != null)
		{
			TapjoyPlugin.awardTapPointsSucceeded();
		}
	}

	public void TapPointsAwardError(string message)
	{
		if (TapjoyPlugin.awardTapPointsFailed != null)
		{
			TapjoyPlugin.awardTapPointsFailed();
		}
	}

	public void CurrencyEarned(string message)
	{
		if (TapjoyPlugin.tapPointsEarned != null)
		{
			TapjoyPlugin.tapPointsEarned(int.Parse(message));
		}
	}

	public void FullScreenAdLoaded(string message)
	{
		if (TapjoyPlugin.getFullScreenAdSucceeded != null)
		{
			TapjoyPlugin.getFullScreenAdSucceeded();
		}
	}

	public void FullScreenAdError(string message)
	{
		if (TapjoyPlugin.getFullScreenAdFailed != null)
		{
			TapjoyPlugin.getFullScreenAdFailed();
		}
	}

	public void DisplayAdLoaded(string message)
	{
		if (TapjoyPlugin.getDisplayAdSucceeded != null)
		{
			TapjoyPlugin.getDisplayAdSucceeded();
		}
	}

	public void DisplayAdError(string message)
	{
		if (TapjoyPlugin.getDisplayAdFailed != null)
		{
			TapjoyPlugin.getDisplayAdFailed();
		}
	}

	public void VideoAdStart(string message)
	{
		if (TapjoyPlugin.videoAdStarted != null)
		{
			TapjoyPlugin.videoAdStarted();
		}
	}

	public void VideoAdError(string message)
	{
		if (TapjoyPlugin.videoAdFailed != null)
		{
			TapjoyPlugin.videoAdFailed();
		}
	}

	public void VideoAdComplete(string message)
	{
		if (TapjoyPlugin.videoAdCompleted != null)
		{
			TapjoyPlugin.videoAdCompleted();
		}
	}

	public void ShowOffersError(string message)
	{
		if (TapjoyPlugin.showOffersFailed != null)
		{
			TapjoyPlugin.showOffersFailed();
		}
	}

	public void ViewOpened(string message)
	{
		if (TapjoyPlugin.viewOpened != null)
		{
			int obj = int.Parse(message);
			TapjoyPlugin.viewOpened((TapjoyViewType)obj);
		}
	}

	public void ViewClosed(string message)
	{
		if (TapjoyPlugin.viewClosed != null)
		{
			int obj = int.Parse(message);
			TapjoyPlugin.viewClosed((TapjoyViewType)obj);
		}
	}

	public static void SetCallbackHandler(string handlerName)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.SetCallbackHandler(handlerName);
	}

	public static void RequestTapjoyConnect(string appID, string secretKey)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.RequestTapjoyConnect(appID, secretKey);
	}

	public static void RequestTapjoyConnect(string appID, string secretKey, Dictionary<string, string> flags)
	{
		if (Application.isEditor)
		{
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (KeyValuePair<string, string> current in flags)
		{
			dictionary.Add(current.Key, current.Value);
		}
		TapjoyPluginAndroid.RequestTapjoyConnect(appID, secretKey, dictionary);
	}

	public static void RequestTapjoyConnect(string appID, string secretKey, Dictionary<string, object> flags)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.RequestTapjoyConnect(appID, secretKey, flags);
	}

	public static void EnableLogging(bool enable)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.EnableLogging(enable);
	}

	public static void SendSegmentationParams(Dictionary<string, object> segmentationParams)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.SendSegmentationParams(segmentationParams);
	}

	public static void AppPause()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.AppPause();
	}

	public static void AppResume()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.AppResume();
	}

	public static void ActionComplete(string actionID)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.ActionComplete(actionID);
	}

	public static void SetUserID(string userID)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.SetUserID(userID);
	}

	public static void ShowOffers()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.ShowOffers();
	}

	public static void GetTapPoints()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.GetTapPoints();
	}

	public static void SpendTapPoints(int points)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.SpendTapPoints(points);
	}

	public static void AwardTapPoints(int points)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.AwardTapPoints(points);
	}

	public static int QueryTapPoints()
	{
		if (Application.isEditor)
		{
			return 0;
		}
		return TapjoyPluginAndroid.QueryTapPoints();
	}

	public static void ShowDefaultEarnedCurrencyAlert()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.ShowDefaultEarnedCurrencyAlert();
	}

	public static void GetDisplayAd()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.GetDisplayAd();
	}

	public static void ShowDisplayAd()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.ShowDisplayAd();
	}

	public static void HideDisplayAd()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.HideDisplayAd();
	}

	[Obsolete("SetDisplayAdContentSize is deprecated. Please use SetDisplayAdSize.")]
	public static void SetDisplayAdContentSize(int size)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPlugin.SetDisplayAdSize((TapjoyDisplayAdSize)size);
	}

	public static void SetDisplayAdSize(TapjoyDisplayAdSize size)
	{
		if (Application.isEditor)
		{
			return;
		}
		string displayAdSize = "320x50";
		if (size == TapjoyDisplayAdSize.SIZE_640X100)
		{
			displayAdSize = "640x100";
		}
		if (size == TapjoyDisplayAdSize.SIZE_768X90)
		{
			displayAdSize = "768x90";
		}
		TapjoyPluginAndroid.SetDisplayAdSize(displayAdSize);
	}

	public static void EnableDisplayAdAutoRefresh(bool enable)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.EnableDisplayAdAutoRefresh(enable);
	}

	public static void MoveDisplayAd(int x, int y)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.MoveDisplayAd(x, y);
	}

	[Obsolete("SetTransitionEffect is deprecated since 10.0.0")]
	public static void SetTransitionEffect(int transition)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.SetTransitionEffect(transition);
	}

	[Obsolete("GetFullScreenAd is deprecated since 10.0.0")]
	public static void GetFullScreenAd()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.GetFullScreenAd();
	}

	[Obsolete("ShowFullScreenAd is deprecated since 10.0.0. Tapjoy ad units now use TJEvent")]
	public static void ShowFullScreenAd()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.ShowFullScreenAd();
	}

	[Obsolete("SetVideoCacheCount is deprecated, video is now controlled via your Tapjoy Dashboard.")]
	public static void SetVideoCacheCount(int cacheCount)
	{
	}

	public static void SendShutDownEvent()
	{
		TapjoyPluginAndroid.SendShutDownEvent();
	}

	public static void SendIAPEvent(string name, float price, int quantity, string currencyCode)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.SendIAPEvent(name, price, quantity, currencyCode);
	}

	public static void ShowOffersWithCurrencyID(string currencyID, bool selector)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.ShowOffersWithCurrencyID(currencyID, selector);
	}

	public static void GetDisplayAdWithCurrencyID(string currencyID)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.GetDisplayAdWithCurrencyID(currencyID);
	}

	public static void GetFullScreenAdWithCurrencyID(string currencyID)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.GetFullScreenAdWithCurrencyID(currencyID);
	}

	public static void SetCurrencyMultiplier(float multiplier)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.SetCurrencyMultiplier(multiplier);
	}

	public void SendEventCompleteWithContent(string guid)
	{
		if (TapjoyPlugin.eventDictionary != null && TapjoyPlugin.eventDictionary.ContainsKey(guid))
		{
			TapjoyPlugin.eventDictionary[guid].TriggerSendEventSucceeded(true);
		}
	}

	public void SendEventComplete(string guid)
	{
		if (TapjoyPlugin.eventDictionary != null && TapjoyPlugin.eventDictionary.ContainsKey(guid))
		{
			TapjoyPlugin.eventDictionary[guid].TriggerSendEventSucceeded(false);
		}
	}

	public void ContentIsReady(string parametersAsString)
	{
		string[] array = parametersAsString.Split(new char[]
		{
			','
		});
		if (array.Length == 2)
		{
			string key = array[0];
			int status = Convert.ToInt32(array[1]);
			if (TapjoyPlugin.eventDictionary != null && TapjoyPlugin.eventDictionary.ContainsKey(key))
			{
				TapjoyPlugin.eventDictionary[key].TriggerContentIsReady(status);
			}
		}
	}

	public void SendEventFail(string guid)
	{
		if (TapjoyPlugin.eventDictionary != null && TapjoyPlugin.eventDictionary.ContainsKey(guid))
		{
			TapjoyPlugin.eventDictionary[guid].TriggerSendEventFailed(null);
		}
	}

	public void ContentDidAppear(string guid)
	{
		if (TapjoyPlugin.eventDictionary != null && TapjoyPlugin.eventDictionary.ContainsKey(guid))
		{
			TapjoyPlugin.eventDictionary[guid].TriggerContentDidAppear();
		}
	}

	public void ContentDidDisappear(string guid)
	{
		if (TapjoyPlugin.eventDictionary != null && TapjoyPlugin.eventDictionary.ContainsKey(guid))
		{
			TapjoyPlugin.eventDictionary[guid].TriggerContentDidDisappear();
		}
	}

	public void DidRequestAction(string message)
	{
		string[] array = message.Split(new char[]
		{
			','
		});
		string key = array[0];
		int type;
		int.TryParse(array[1], out type);
		string identifier = array[2];
		int quantity;
		int.TryParse(array[3], out quantity);
		if (TapjoyPlugin.eventDictionary != null && TapjoyPlugin.eventDictionary.ContainsKey(key))
		{
			TapjoyPlugin.eventDictionary[key].TriggerDidRequestAction(type, identifier, quantity);
		}
	}

	public static string CreateEvent(TapjoyEvent eventRef, string eventName, string eventParameter)
	{
		if (Application.isEditor)
		{
			return null;
		}
		string text = Guid.NewGuid().ToString();
		while (TapjoyPlugin.eventDictionary.ContainsKey(text))
		{
			text = Guid.NewGuid().ToString();
		}
		TapjoyPlugin.eventDictionary.Add(text, eventRef);
		TapjoyPluginAndroid.CreateEvent(text, eventName, eventParameter);
		return text;
	}

	public static void SendEvent(string guid)
	{
		TapjoyPluginAndroid.SendEvent(guid);
	}

	public static void ShowEvent(string guid)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.ShowEvent(guid);
	}

	public static void EnableEventAutoPresent(string guid, bool autoPresent)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.EnableEventAutoPresent(guid, autoPresent);
	}

	public static void EnableEventPreload(string guid, bool preload)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.EnableEventPreload(guid, preload);
	}

	public static void EventRequestCompleted(string guid)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.EventRequestCompleted(guid);
	}

	public static void EventRequestCancelled(string guid)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginAndroid.EventRequestCancelled(guid);
	}
}
