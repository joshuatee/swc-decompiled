using StaRTS.Externals.EnvironmentManager;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Notifications;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

namespace StaRTS.Externals.DMOAnalytics
{
	public class DMOAnalyticsController : IEventObserver
	{
		private const string IN_APP_CURRENCY_JSON_FORMAT = "{{\"item_id\":\"{0}|{1}\",\"item_count\":\"{2}\"}}";

		private const string IN_APP_CURRENCY_JSON_FORMAT_WITHOUT_TYPE = "{{\"item_id\":\"{0}\",\"item_count\":\"{1}\"}}";

		private const string PAYMENT_ACTION_JSON_FORMAT = "{{\"item_id\":\"{0}\",\"item_count\":\"{1}\"}}";

		private IDMOAnalyticsManager analytics;

		public DMOAnalyticsController()
		{
			Service.Set<DMOAnalyticsController>(this);
			this.analytics = new DefaultDMOAnalyticsManager();
			this.LogAppStart();
		}

		public void Init()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.ApplicationPauseToggled, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ApplicationQuit, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.PlayerLoginSuccess, EventPriority.Default);
		}

		public void Destroy()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.ApplicationPauseToggled);
			eventManager.UnregisterObserver(this, EventId.ApplicationQuit);
			eventManager.UnregisterObserver(this, EventId.PlayerLoginSuccess);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.ApplicationPauseToggled)
			{
				if (id != EventId.ApplicationQuit)
				{
					if (id == EventId.PlayerLoginSuccess)
					{
						CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
						this.LogUserInfo("guid", currentPlayer.PlayerId);
						if (Service.Get<NotificationController>().Enabled)
						{
							string deviceToken = Service.Get<NotificationController>().GetDeviceToken();
							if (!string.IsNullOrEmpty(deviceToken))
							{
								this.LogUserInfo("ur", deviceToken);
							}
						}
					}
				}
				else
				{
					this.LogAppEnd();
				}
			}
			else
			{
				this.HandleApplicationPause((bool)cookie);
			}
			return EatResponse.NotEaten;
		}

		public void LogAge(int age)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Serializer serializer = Serializer.Start();
			serializer.AddString("player_id", currentPlayer.PlayerId);
			serializer.AddString("action", "age_gate");
			serializer.AddString("context", "age_gate");
			serializer.AddString("type", age.ToString());
			serializer.Add<int>("level", this.GetHQLevel());
			string parameters = serializer.End().ToString();
			this.LogGameAction(parameters);
		}

		public void LogNotificationImpression(string notifID, bool isLocalNotif, string desc, string message)
		{
			string placement = "Push_Notification";
			if (isLocalNotif)
			{
				placement = "Local_Notification";
			}
			this.LogAdAction(placement, notifID, "Impression", desc, message);
		}

		public void LogNotificationReengage(string notifID, bool isLocalNotif, string desc, string message)
		{
			string placement = "Push_Notification";
			if (isLocalNotif)
			{
				placement = "Local_Notification";
			}
			this.LogAdAction(placement, notifID, "Reengage", desc, message);
		}

		public void LogAdAction(string placement, string creative, string type, string desc, string message)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Serializer serializer = Serializer.Start();
			serializer.AddString("player_id", currentPlayer.PlayerId);
			serializer.AddString("creative", creative);
			serializer.AddString("placement", placement);
			serializer.AddString("offer", desc);
			serializer.AddString("type", type);
			serializer.AddString("locale", Service.Get<EnvironmentController>().GetLocale());
			serializer.AddString("message", message);
			serializer.AddString("currency", "USD");
			serializer.Add<double>("gross_revenue", 0.0);
			serializer.End();
			Service.Get<StaRTSLogger>().Debug("LogAdAction: " + serializer.ToString());
			this.analytics.LogEventWithContext("ad_action", serializer.ToString());
		}

		public void LogUserInfo(string userIdDomain, string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				Service.Get<StaRTSLogger>().Error("LogUserInfo: " + userIdDomain + " : userId is null");
				return;
			}
			if (!Service.IsSet<CurrentPlayer>())
			{
				Service.Get<StaRTSLogger>().Error("LogUserInfo: CurrentPlayer is not set.");
				return;
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (string.IsNullOrEmpty(currentPlayer.PlayerId))
			{
				Service.Get<StaRTSLogger>().Error("LogUserInfo: Player.PlayerId is NullOrEmpty");
				return;
			}
			Serializer serializer = Serializer.Start();
			serializer.AddString("player_id", currentPlayer.PlayerId);
			serializer.AddString("user_id", userId);
			serializer.AddString("user_id_domain", userIdDomain);
			string parameters = serializer.End().ToString();
			this.analytics.LogEventWithContext("user_info", parameters);
		}

		public void LogInAppCurrencyAction(int currencyAmount, string itemType, string itemId, int itemCount, string type, string subType)
		{
			this.LogInAppCurrencyAction(currencyAmount, itemType, itemId, itemCount, type, subType, "");
		}

		public void LogInAppCurrencyAction(int currencyAmount, string itemType, string itemId, int itemCount, string type, string subType, string context)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			StringBuilder stringBuilder = new StringBuilder();
			if (itemType.get_Length() == 0)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{{\"item_id\":\"{0}\",\"item_count\":\"{1}\"}}", new object[]
				{
					itemId,
					itemCount
				});
			}
			else
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{{\"item_id\":\"{0}|{1}\",\"item_count\":\"{2}\"}}", new object[]
				{
					itemType,
					itemId,
					itemCount
				});
			}
			string val = stringBuilder.ToString();
			int itemAmount = currentPlayer.Inventory.GetItemAmount("crystals");
			Serializer serializer = Serializer.Start();
			serializer.AddString("player_id", currentPlayer.PlayerId);
			serializer.Add<int>("amount", currencyAmount);
			serializer.AddString("currency", "crystals");
			serializer.Add<int>("balance", itemAmount);
			serializer.Add<string>("item", val);
			serializer.AddString("type", type);
			serializer.AddString("subtype", subType);
			if (context.get_Length() > 0)
			{
				serializer.AddString("context", context);
			}
			serializer.Add<int>("level", this.GetHQLevel());
			string parameters = serializer.End().ToString();
			this.analytics.LogEventWithContext("in_app_currency_action", parameters);
		}

		public void LogPaymentAction(string currency, double amountPaid, string productId, int amount, string type)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Serializer serializer = Serializer.Start();
			serializer.AddString("player_id", currentPlayer.PlayerId);
			serializer.AddString("currency", currency);
			serializer.AddString("locale", Service.Get<EnvironmentController>().GetLocale());
			serializer.Add<double>("amount_paid", -amountPaid);
			serializer.AddString("type", type);
			serializer.Add<int>("level", this.GetHQLevel());
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{{\"item_id\":\"{0}\",\"item_count\":\"{1}\"}}", new object[]
			{
				productId,
				amount
			});
			serializer.Add<string>("item", stringBuilder.ToString());
			serializer.End();
			this.analytics.LogEventWithContext("payment_action", serializer.ToString());
		}

		private int GetHQLevel()
		{
			if (Service.IsSet<BuildingLookupController>())
			{
				return Service.Get<BuildingLookupController>().GetHighestLevelHQ();
			}
			return 0;
		}

		public void LogEvent(string appEvent)
		{
			this.analytics.LogEvent(appEvent);
		}

		public void LogAppStart()
		{
			this.analytics.LogAppStart();
		}

		public void LogAppEnd()
		{
			this.analytics.LogAppEnd();
		}

		public void LogAppForeground()
		{
			this.analytics.LogAppForeground();
		}

		public void LogAppBackground()
		{
			this.analytics.LogAppBackground();
		}

		public void LogEventWithContext(string eventName, string parameters)
		{
			this.analytics.LogEventWithContext(eventName, parameters);
		}

		public void LogGameAction(string parameters)
		{
			this.analytics.LogEventWithContext("game_action", parameters);
		}

		public void FlushAnalyticsQueue()
		{
			this.analytics.FlushAnalyticsQueue();
		}

		public void SetDebugLogging(bool isEnable)
		{
			this.analytics.SetDebugLogging(isEnable);
		}

		public void SetCanUseNetwork(bool isEnable)
		{
			this.analytics.SetCanUseNetwork(isEnable);
		}

		private void HandleApplicationPause(bool paused)
		{
			if (paused)
			{
				this.LogAppBackground();
				return;
			}
			this.LogAppForeground();
		}

		protected internal DMOAnalyticsController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).FlushAnalyticsQueue();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).GetHQLevel());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).HandleApplicationPause(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogAdAction(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogAge(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogAppBackground();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogAppEnd();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogAppForeground();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogAppStart();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogEvent(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogEventWithContext(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogGameAction(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogInAppCurrencyAction(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), *(int*)(args + 3), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogInAppCurrencyAction(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), *(int*)(args + 3), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), Marshal.PtrToStringUni(*(IntPtr*)(args + 6)));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogNotificationImpression(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogNotificationReengage(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogPaymentAction(Marshal.PtrToStringUni(*(IntPtr*)args), *(double*)(args + 1), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), *(int*)(args + 3), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).LogUserInfo(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).SetCanUseNetwork(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((DMOAnalyticsController)GCHandledObjects.GCHandleToObject(instance)).SetDebugLogging(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
