using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

public class TapjoyPlugin : MonoBehaviour, IUnitySerializable
{
	public const string MacAddressOptionOn = "0";

	public const string MacAddressOptionOffWithVersionCheck = "1";

	public const string MacAddressOptionOff = "2";

	private static Dictionary<string, TapjoyEvent> eventDictionary = new Dictionary<string, TapjoyEvent>();

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action connectCallSucceeded;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action connectCallFailed;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action<int> getTapPointsSucceeded;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action getTapPointsFailed;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action<int> spendTapPointsSucceeded;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action spendTapPointsFailed;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action awardTapPointsSucceeded;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action awardTapPointsFailed;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action<int> tapPointsEarned;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action getFullScreenAdSucceeded;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action getFullScreenAdFailed;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action getDisplayAdSucceeded;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action getDisplayAdFailed;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action videoAdStarted;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action videoAdFailed;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action videoAdCompleted;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action showOffersFailed;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action<TapjoyViewType> viewOpened;

	[method: CompilerGenerated]
	[CompilerGenerated]
	public static event Action<TapjoyViewType> viewClosed;

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
			TapjoyPlugin.connectCallSucceeded.Invoke();
		}
	}

	public void TapjoyConnectFail(string message)
	{
		if (TapjoyPlugin.connectCallFailed != null)
		{
			TapjoyPlugin.connectCallFailed.Invoke();
		}
	}

	public void TapPointsLoaded(string message)
	{
		if (TapjoyPlugin.getTapPointsSucceeded != null)
		{
			TapjoyPlugin.getTapPointsSucceeded.Invoke(int.Parse(message));
		}
	}

	public void TapPointsLoadedError(string message)
	{
		if (TapjoyPlugin.getTapPointsFailed != null)
		{
			TapjoyPlugin.getTapPointsFailed.Invoke();
		}
	}

	public void TapPointsSpent(string message)
	{
		if (TapjoyPlugin.spendTapPointsSucceeded != null)
		{
			TapjoyPlugin.spendTapPointsSucceeded.Invoke(int.Parse(message));
		}
	}

	public void TapPointsSpendError(string message)
	{
		if (TapjoyPlugin.spendTapPointsFailed != null)
		{
			TapjoyPlugin.spendTapPointsFailed.Invoke();
		}
	}

	public void TapPointsAwarded(string message)
	{
		if (TapjoyPlugin.awardTapPointsSucceeded != null)
		{
			TapjoyPlugin.awardTapPointsSucceeded.Invoke();
		}
	}

	public void TapPointsAwardError(string message)
	{
		if (TapjoyPlugin.awardTapPointsFailed != null)
		{
			TapjoyPlugin.awardTapPointsFailed.Invoke();
		}
	}

	public void CurrencyEarned(string message)
	{
		if (TapjoyPlugin.tapPointsEarned != null)
		{
			TapjoyPlugin.tapPointsEarned.Invoke(int.Parse(message));
		}
	}

	public void FullScreenAdLoaded(string message)
	{
		if (TapjoyPlugin.getFullScreenAdSucceeded != null)
		{
			TapjoyPlugin.getFullScreenAdSucceeded.Invoke();
		}
	}

	public void FullScreenAdError(string message)
	{
		if (TapjoyPlugin.getFullScreenAdFailed != null)
		{
			TapjoyPlugin.getFullScreenAdFailed.Invoke();
		}
	}

	public void DisplayAdLoaded(string message)
	{
		if (TapjoyPlugin.getDisplayAdSucceeded != null)
		{
			TapjoyPlugin.getDisplayAdSucceeded.Invoke();
		}
	}

	public void DisplayAdError(string message)
	{
		if (TapjoyPlugin.getDisplayAdFailed != null)
		{
			TapjoyPlugin.getDisplayAdFailed.Invoke();
		}
	}

	public void VideoAdStart(string message)
	{
		if (TapjoyPlugin.videoAdStarted != null)
		{
			TapjoyPlugin.videoAdStarted.Invoke();
		}
	}

	public void VideoAdError(string message)
	{
		if (TapjoyPlugin.videoAdFailed != null)
		{
			TapjoyPlugin.videoAdFailed.Invoke();
		}
	}

	public void VideoAdComplete(string message)
	{
		if (TapjoyPlugin.videoAdCompleted != null)
		{
			TapjoyPlugin.videoAdCompleted.Invoke();
		}
	}

	public void ShowOffersError(string message)
	{
		if (TapjoyPlugin.showOffersFailed != null)
		{
			TapjoyPlugin.showOffersFailed.Invoke();
		}
	}

	public void ViewOpened(string message)
	{
		if (TapjoyPlugin.viewOpened != null)
		{
			int num = int.Parse(message);
			TapjoyPlugin.viewOpened.Invoke((TapjoyViewType)num);
		}
	}

	public void ViewClosed(string message)
	{
		if (TapjoyPlugin.viewClosed != null)
		{
			int num = int.Parse(message);
			TapjoyPlugin.viewClosed.Invoke((TapjoyViewType)num);
		}
	}

	public static void SetCallbackHandler(string handlerName)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.SetCallbackHandler(handlerName);
	}

	public static void RequestTapjoyConnect(string appID, string secretKey)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.RequestTapjoyConnect(appID, secretKey);
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
			dictionary.Add(current.get_Key(), current.get_Value());
		}
		TapjoyPluginDefault.RequestTapjoyConnect(appID, secretKey, dictionary);
	}

	public static void RequestTapjoyConnect(string appID, string secretKey, Dictionary<string, object> flags)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.RequestTapjoyConnect(appID, secretKey, flags);
	}

	public static void EnableLogging(bool enable)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.EnableLogging(enable);
	}

	public static void SendSegmentationParams(Dictionary<string, object> segmentationParams)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.SendSegmentationParams(segmentationParams);
	}

	public static void AppPause()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.AppPause();
	}

	public static void AppResume()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.AppResume();
	}

	public static void ActionComplete(string actionID)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.ActionComplete(actionID);
	}

	public static void SetUserID(string userID)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.SetUserID(userID);
	}

	public static void ShowOffers()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.ShowOffers();
	}

	public static void GetTapPoints()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.GetTapPoints();
	}

	public static void SpendTapPoints(int points)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.SpendTapPoints(points);
	}

	public static void AwardTapPoints(int points)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.AwardTapPoints(points);
	}

	public static int QueryTapPoints()
	{
		if (Application.isEditor)
		{
			return 0;
		}
		return TapjoyPluginDefault.QueryTapPoints();
	}

	public static void ShowDefaultEarnedCurrencyAlert()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.ShowDefaultEarnedCurrencyAlert();
	}

	public static void GetDisplayAd()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.GetDisplayAd();
	}

	public static void ShowDisplayAd()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.ShowDisplayAd();
	}

	public static void HideDisplayAd()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.HideDisplayAd();
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
		TapjoyPluginDefault.SetDisplayAdSize(displayAdSize);
	}

	public static void EnableDisplayAdAutoRefresh(bool enable)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.EnableDisplayAdAutoRefresh(enable);
	}

	public static void MoveDisplayAd(int x, int y)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.MoveDisplayAd(x, y);
	}

	[Obsolete("SetTransitionEffect is deprecated since 10.0.0")]
	public static void SetTransitionEffect(int transition)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.SetTransitionEffect(transition);
	}

	[Obsolete("GetFullScreenAd is deprecated since 10.0.0")]
	public static void GetFullScreenAd()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.GetFullScreenAd();
	}

	[Obsolete("ShowFullScreenAd is deprecated since 10.0.0. Tapjoy ad units now use TJEvent")]
	public static void ShowFullScreenAd()
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.ShowFullScreenAd();
	}

	[Obsolete("SetVideoCacheCount is deprecated, video is now controlled via your Tapjoy Dashboard.")]
	public static void SetVideoCacheCount(int cacheCount)
	{
	}

	public static void SendShutDownEvent()
	{
		TapjoyPluginDefault.SendShutDownEvent();
	}

	public static void SendIAPEvent(string name, float price, int quantity, string currencyCode)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.SendIAPEvent(name, price, quantity, currencyCode);
	}

	public static void ShowOffersWithCurrencyID(string currencyID, bool selector)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.ShowOffersWithCurrencyID(currencyID, selector);
	}

	public static void GetDisplayAdWithCurrencyID(string currencyID)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.GetDisplayAdWithCurrencyID(currencyID);
	}

	public static void GetFullScreenAdWithCurrencyID(string currencyID)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.GetFullScreenAdWithCurrencyID(currencyID);
	}

	public static void SetCurrencyMultiplier(float multiplier)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.SetCurrencyMultiplier(multiplier);
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
			int status = Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
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
		int.TryParse(array[1], ref type);
		string identifier = array[2];
		int quantity;
		int.TryParse(array[3], ref quantity);
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
		TapjoyPluginDefault.CreateEvent(text, eventName, eventParameter);
		return text;
	}

	public static void SendEvent(string guid)
	{
		TapjoyPluginDefault.SendEvent(guid);
	}

	public static void ShowEvent(string guid)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.ShowEvent(guid);
	}

	public static void EnableEventAutoPresent(string guid, bool autoPresent)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.EnableEventAutoPresent(guid, autoPresent);
	}

	public static void EnableEventPreload(string guid, bool preload)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.EnableEventPreload(guid, preload);
	}

	public static void EventRequestCompleted(string guid)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.EventRequestCompleted(guid);
	}

	public static void EventRequestCancelled(string guid)
	{
		if (Application.isEditor)
		{
			return;
		}
		TapjoyPluginDefault.EventRequestCancelled(guid);
	}

	public TapjoyPlugin()
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

	protected internal TapjoyPlugin(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		TapjoyPlugin.ActionComplete(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		TapjoyPlugin.awardTapPointsFailed += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		TapjoyPlugin.awardTapPointsSucceeded += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		TapjoyPlugin.connectCallFailed += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		TapjoyPlugin.connectCallSucceeded += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		TapjoyPlugin.getDisplayAdFailed += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		TapjoyPlugin.getDisplayAdSucceeded += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		TapjoyPlugin.getFullScreenAdFailed += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		TapjoyPlugin.getFullScreenAdSucceeded += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		TapjoyPlugin.getTapPointsFailed += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		TapjoyPlugin.getTapPointsSucceeded += (Action<int>)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		TapjoyPlugin.showOffersFailed += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		TapjoyPlugin.spendTapPointsFailed += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		TapjoyPlugin.spendTapPointsSucceeded += (Action<int>)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		TapjoyPlugin.tapPointsEarned += (Action<int>)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		TapjoyPlugin.videoAdCompleted += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		TapjoyPlugin.videoAdFailed += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		TapjoyPlugin.videoAdStarted += (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		TapjoyPlugin.viewClosed += (Action<TapjoyViewType>)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		TapjoyPlugin.viewOpened += (Action<TapjoyViewType>)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		TapjoyPlugin.AppPause();
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		TapjoyPlugin.AppResume();
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		TapjoyPlugin.AwardTapPoints(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).ContentDidAppear(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).ContentDidDisappear(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).ContentIsReady(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(TapjoyPlugin.CreateEvent((TapjoyEvent)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2))));
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).CurrencyEarned(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).DidRequestAction(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).DisplayAdError(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke31(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).DisplayAdLoaded(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke32(long instance, long* args)
	{
		TapjoyPlugin.EnableDisplayAdAutoRefresh(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke33(long instance, long* args)
	{
		TapjoyPlugin.EnableEventAutoPresent(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke34(long instance, long* args)
	{
		TapjoyPlugin.EnableEventPreload(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke35(long instance, long* args)
	{
		TapjoyPlugin.EnableLogging(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke36(long instance, long* args)
	{
		TapjoyPlugin.EventRequestCancelled(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke37(long instance, long* args)
	{
		TapjoyPlugin.EventRequestCompleted(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke38(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).FullScreenAdError(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke39(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).FullScreenAdLoaded(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke40(long instance, long* args)
	{
		TapjoyPlugin.GetDisplayAd();
		return -1L;
	}

	public unsafe static long $Invoke41(long instance, long* args)
	{
		TapjoyPlugin.GetDisplayAdWithCurrencyID(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke42(long instance, long* args)
	{
		TapjoyPlugin.GetFullScreenAd();
		return -1L;
	}

	public unsafe static long $Invoke43(long instance, long* args)
	{
		TapjoyPlugin.GetFullScreenAdWithCurrencyID(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke44(long instance, long* args)
	{
		TapjoyPlugin.GetTapPoints();
		return -1L;
	}

	public unsafe static long $Invoke45(long instance, long* args)
	{
		TapjoyPlugin.HideDisplayAd();
		return -1L;
	}

	public unsafe static long $Invoke46(long instance, long* args)
	{
		TapjoyPlugin.MoveDisplayAd(*(int*)args, *(int*)(args + 1));
		return -1L;
	}

	public unsafe static long $Invoke47(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(TapjoyPlugin.QueryTapPoints());
	}

	public unsafe static long $Invoke48(long instance, long* args)
	{
		TapjoyPlugin.awardTapPointsFailed -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke49(long instance, long* args)
	{
		TapjoyPlugin.awardTapPointsSucceeded -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke50(long instance, long* args)
	{
		TapjoyPlugin.connectCallFailed -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke51(long instance, long* args)
	{
		TapjoyPlugin.connectCallSucceeded -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke52(long instance, long* args)
	{
		TapjoyPlugin.getDisplayAdFailed -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke53(long instance, long* args)
	{
		TapjoyPlugin.getDisplayAdSucceeded -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke54(long instance, long* args)
	{
		TapjoyPlugin.getFullScreenAdFailed -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke55(long instance, long* args)
	{
		TapjoyPlugin.getFullScreenAdSucceeded -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke56(long instance, long* args)
	{
		TapjoyPlugin.getTapPointsFailed -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke57(long instance, long* args)
	{
		TapjoyPlugin.getTapPointsSucceeded -= (Action<int>)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke58(long instance, long* args)
	{
		TapjoyPlugin.showOffersFailed -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke59(long instance, long* args)
	{
		TapjoyPlugin.spendTapPointsFailed -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke60(long instance, long* args)
	{
		TapjoyPlugin.spendTapPointsSucceeded -= (Action<int>)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke61(long instance, long* args)
	{
		TapjoyPlugin.tapPointsEarned -= (Action<int>)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke62(long instance, long* args)
	{
		TapjoyPlugin.videoAdCompleted -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke63(long instance, long* args)
	{
		TapjoyPlugin.videoAdFailed -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke64(long instance, long* args)
	{
		TapjoyPlugin.videoAdStarted -= (Action)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke65(long instance, long* args)
	{
		TapjoyPlugin.viewClosed -= (Action<TapjoyViewType>)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke66(long instance, long* args)
	{
		TapjoyPlugin.viewOpened -= (Action<TapjoyViewType>)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke67(long instance, long* args)
	{
		TapjoyPlugin.RequestTapjoyConnect(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke68(long instance, long* args)
	{
		TapjoyPlugin.RequestTapjoyConnect(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (Dictionary<string, object>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke69(long instance, long* args)
	{
		TapjoyPlugin.RequestTapjoyConnect(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (Dictionary<string, string>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke70(long instance, long* args)
	{
		TapjoyPlugin.SendEvent(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke71(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).SendEventComplete(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke72(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).SendEventCompleteWithContent(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke73(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).SendEventFail(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke74(long instance, long* args)
	{
		TapjoyPlugin.SendIAPEvent(Marshal.PtrToStringUni(*(IntPtr*)args), *(float*)(args + 1), *(int*)(args + 2), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
		return -1L;
	}

	public unsafe static long $Invoke75(long instance, long* args)
	{
		TapjoyPlugin.SendSegmentationParams((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke76(long instance, long* args)
	{
		TapjoyPlugin.SendShutDownEvent();
		return -1L;
	}

	public unsafe static long $Invoke77(long instance, long* args)
	{
		TapjoyPlugin.SetCallbackHandler(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke78(long instance, long* args)
	{
		TapjoyPlugin.SetCurrencyMultiplier(*(float*)args);
		return -1L;
	}

	public unsafe static long $Invoke79(long instance, long* args)
	{
		TapjoyPlugin.SetDisplayAdContentSize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke80(long instance, long* args)
	{
		TapjoyPlugin.SetDisplayAdSize((TapjoyDisplayAdSize)(*(int*)args));
		return -1L;
	}

	public unsafe static long $Invoke81(long instance, long* args)
	{
		TapjoyPlugin.SetTransitionEffect(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke82(long instance, long* args)
	{
		TapjoyPlugin.SetUserID(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke83(long instance, long* args)
	{
		TapjoyPlugin.SetVideoCacheCount(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke84(long instance, long* args)
	{
		TapjoyPlugin.ShowDefaultEarnedCurrencyAlert();
		return -1L;
	}

	public unsafe static long $Invoke85(long instance, long* args)
	{
		TapjoyPlugin.ShowDisplayAd();
		return -1L;
	}

	public unsafe static long $Invoke86(long instance, long* args)
	{
		TapjoyPlugin.ShowEvent(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke87(long instance, long* args)
	{
		TapjoyPlugin.ShowFullScreenAd();
		return -1L;
	}

	public unsafe static long $Invoke88(long instance, long* args)
	{
		TapjoyPlugin.ShowOffers();
		return -1L;
	}

	public unsafe static long $Invoke89(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).ShowOffersError(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke90(long instance, long* args)
	{
		TapjoyPlugin.ShowOffersWithCurrencyID(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke91(long instance, long* args)
	{
		TapjoyPlugin.SpendTapPoints(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke92(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).TapjoyConnectFail(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke93(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).TapjoyConnectSuccess(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke94(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).TapPointsAwarded(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke95(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).TapPointsAwardError(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke96(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).TapPointsLoaded(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke97(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).TapPointsLoadedError(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke98(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).TapPointsSpendError(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke99(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).TapPointsSpent(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke100(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke101(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke102(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke103(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke104(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke105(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).VideoAdComplete(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke106(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).VideoAdError(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke107(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).VideoAdStart(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke108(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).ViewClosed(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke109(long instance, long* args)
	{
		((TapjoyPlugin)GCHandledObjects.GCHandleToObject(instance)).ViewOpened(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}
}
