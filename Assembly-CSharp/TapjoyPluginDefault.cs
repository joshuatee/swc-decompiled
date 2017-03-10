using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

public class TapjoyPluginDefault
{
	public static void SetCallbackHandler(string handlerName)
	{
	}

	public static void RequestTapjoyConnect(string appID, string secretKey)
	{
	}

	public static void RequestTapjoyConnect(string appID, string secretKey, Dictionary<string, object> flags)
	{
	}

	public static void EnableLogging(bool enable)
	{
	}

	public static void SendSegmentationParams(Dictionary<string, object> segmentationParams)
	{
	}

	public static void AppPause()
	{
	}

	public static void AppResume()
	{
	}

	public static void ActionComplete(string actionID)
	{
	}

	public static void SetUserID(string userID)
	{
	}

	public static void ShowOffers()
	{
	}

	public static void GetTapPoints()
	{
	}

	public static void SpendTapPoints(int points)
	{
	}

	public static void AwardTapPoints(int points)
	{
	}

	public static int QueryTapPoints()
	{
		return 0;
	}

	public static void ShowDefaultEarnedCurrencyAlert()
	{
	}

	public static void GetDisplayAd()
	{
	}

	public static void ShowDisplayAd()
	{
	}

	public static void HideDisplayAd()
	{
	}

	public static void SetDisplayAdSize(string size)
	{
	}

	public static void EnableDisplayAdAutoRefresh(bool enable)
	{
	}

	public static void MoveDisplayAd(int x, int y)
	{
	}

	public static void SetTransitionEffect(int transition)
	{
	}

	public static void GetFullScreenAd()
	{
	}

	public static void ShowFullScreenAd()
	{
	}

	public static void SendShutDownEvent()
	{
	}

	public static void SendIAPEvent(string name, float price, int quantity, string currencyCode)
	{
	}

	public static void ShowOffersWithCurrencyID(string currencyID, bool selector)
	{
	}

	public static void GetDisplayAdWithCurrencyID(string currencyID)
	{
	}

	public static void GetFullScreenAdWithCurrencyID(string currencyID)
	{
	}

	public static void SetCurrencyMultiplier(float multiplier)
	{
	}

	public static void CreateEvent(string eventGuid, string eventName, string eventParameter)
	{
	}

	public static void SendEvent(string eventGuid)
	{
	}

	public static void ShowEvent(string eventGuid)
	{
	}

	public static void EnableEventAutoPresent(string eventGuid, bool autoPresent)
	{
	}

	public static void EnableEventPreload(string eventGuid, bool preload)
	{
	}

	public static void IsContentReady(string guid)
	{
	}

	public static void EventRequestCompleted(string guid)
	{
	}

	public static void EventRequestCancelled(string guid)
	{
	}

	public TapjoyPluginDefault()
	{
	}

	protected internal TapjoyPluginDefault(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		TapjoyPluginDefault.ActionComplete(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		TapjoyPluginDefault.AppPause();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		TapjoyPluginDefault.AppResume();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		TapjoyPluginDefault.AwardTapPoints(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		TapjoyPluginDefault.CreateEvent(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		TapjoyPluginDefault.EnableDisplayAdAutoRefresh(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		TapjoyPluginDefault.EnableEventAutoPresent(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		TapjoyPluginDefault.EnableEventPreload(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		TapjoyPluginDefault.EnableLogging(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		TapjoyPluginDefault.EventRequestCancelled(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		TapjoyPluginDefault.EventRequestCompleted(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		TapjoyPluginDefault.GetDisplayAd();
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		TapjoyPluginDefault.GetDisplayAdWithCurrencyID(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		TapjoyPluginDefault.GetFullScreenAd();
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		TapjoyPluginDefault.GetFullScreenAdWithCurrencyID(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		TapjoyPluginDefault.GetTapPoints();
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		TapjoyPluginDefault.HideDisplayAd();
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		TapjoyPluginDefault.IsContentReady(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		TapjoyPluginDefault.MoveDisplayAd(*(int*)args, *(int*)(args + 1));
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(TapjoyPluginDefault.QueryTapPoints());
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		TapjoyPluginDefault.RequestTapjoyConnect(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		TapjoyPluginDefault.RequestTapjoyConnect(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (Dictionary<string, object>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		TapjoyPluginDefault.SendEvent(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		TapjoyPluginDefault.SendIAPEvent(Marshal.PtrToStringUni(*(IntPtr*)args), *(float*)(args + 1), *(int*)(args + 2), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		TapjoyPluginDefault.SendSegmentationParams((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		TapjoyPluginDefault.SendShutDownEvent();
		return -1L;
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		TapjoyPluginDefault.SetCallbackHandler(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		TapjoyPluginDefault.SetCurrencyMultiplier(*(float*)args);
		return -1L;
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		TapjoyPluginDefault.SetDisplayAdSize(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		TapjoyPluginDefault.SetTransitionEffect(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		TapjoyPluginDefault.SetUserID(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke31(long instance, long* args)
	{
		TapjoyPluginDefault.ShowDefaultEarnedCurrencyAlert();
		return -1L;
	}

	public unsafe static long $Invoke32(long instance, long* args)
	{
		TapjoyPluginDefault.ShowDisplayAd();
		return -1L;
	}

	public unsafe static long $Invoke33(long instance, long* args)
	{
		TapjoyPluginDefault.ShowEvent(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke34(long instance, long* args)
	{
		TapjoyPluginDefault.ShowFullScreenAd();
		return -1L;
	}

	public unsafe static long $Invoke35(long instance, long* args)
	{
		TapjoyPluginDefault.ShowOffers();
		return -1L;
	}

	public unsafe static long $Invoke36(long instance, long* args)
	{
		TapjoyPluginDefault.ShowOffersWithCurrencyID(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke37(long instance, long* args)
	{
		TapjoyPluginDefault.SpendTapPoints(*(int*)args);
		return -1L;
	}
}
