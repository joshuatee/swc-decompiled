using StaRTS.Externals.DMOAnalytics;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace StaRTS.Main.Controllers.Notifications
{
	public class AndroidNotificationManager : INotificationManager
	{
		private DateTime epochDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private AndroidJavaObject notificationHandler;

		private AndroidJavaObject pluginActivity;

		private AndroidJavaClass mUrbanAirshipWrapper;

		public AndroidNotificationManager()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.disney.starts.PluginActivity");
			this.pluginActivity = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			this.notificationHandler = this.pluginActivity.Get<AndroidJavaObject>("notificationHandler");
			this.mUrbanAirshipWrapper = new AndroidJavaClass("com.disney.UrbanAirship.UrbanAirshipWrapper");
		}

		public void Init()
		{
			this.LogReceivedLocalNotifications();
			this.ClearAllPendingLocalNotifications(true);
			this.ClearReceivedLocalNotifications();
		}

		public string GetDeviceToken()
		{
			return this.GetUrbanAirshipAPID();
		}

		private string GetUrbanAirshipAPID()
		{
			string result = string.Empty;
			if (this.mUrbanAirshipWrapper != null)
			{
				result = this.mUrbanAirshipWrapper.CallStatic<string>("getAPID", new object[0]);
			}
			return result;
		}

		private void LogReceivedLocalNotifications()
		{
			string text = this.notificationHandler.Call<string>("GetReceivedLocalNotifications", new object[0]);
			Service.Get<Logger>().Debug("Received Notification Data: " + text);
			Dictionary<string, object> dictionary = new JsonParser(text).Parse() as Dictionary<string, object>;
			if (!dictionary.ContainsKey("receivedLocalNotifs"))
			{
				return;
			}
			IDataController dataController = Service.Get<IDataController>();
			DateTime now = DateTime.Now;
			now.AddMinutes(-5.0);
			List<object> list = dictionary["receivedLocalNotifs"] as List<object>;
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				IDictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
				string text2 = dictionary2["notifId"] as string;
				long num = Convert.ToInt64(dictionary2["date"] as string);
				Service.Get<Logger>().Debug(string.Concat(new object[]
				{
					"notifId: ",
					text2,
					" date: ",
					num
				}));
				if (!string.IsNullOrEmpty(text2))
				{
					NotificationTypeVO optional = dataController.GetOptional<NotificationTypeVO>(text2);
					if (optional != null)
					{
						DateTime t = DateUtils.DateFromMillis(num);
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append(text2);
						stringBuilder.Append("|");
						stringBuilder.Append("none");
						stringBuilder.Append("|");
						stringBuilder.Append(optional.SoundName);
						int num2 = DateTime.Compare(t, now);
						if (num2 >= 0)
						{
							Service.Get<DMOAnalyticsController>().LogNotificationReengage(text2, true, optional.Desc, stringBuilder.ToString());
						}
						else
						{
							Service.Get<DMOAnalyticsController>().LogNotificationImpression(text2, true, optional.Desc, stringBuilder.ToString());
						}
					}
				}
			}
		}

		public void ScheduleLocalNotification(string notificationUid, string inProgressMessage, string message, string soundName, DateTime time, string key, string objectId)
		{
			string text = this.GetEpochTime(time.ToUniversalTime()).ToString();
			this.notificationHandler.Call("ScheduleLocalNotification", new object[]
			{
				notificationUid,
				message,
				soundName,
				text,
				key,
				objectId
			});
		}

		public void BatchScheduleLocalNotifications(List<NotificationObject> list)
		{
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				NotificationObject notificationObject = list[i];
				this.ScheduleLocalNotification(notificationObject.NotificationUid, notificationObject.InProgressMessage, notificationObject.Message, notificationObject.SoundName, notificationObject.Time, notificationObject.Key, notificationObject.ObjectId);
			}
		}

		public void ClearReceivedLocalNotifications()
		{
			this.notificationHandler.Call("ClearReceivedLocalNotifications", new object[0]);
		}

		public void ClearAllPendingLocalNotifications(bool clearCountdownTimers)
		{
			this.notificationHandler.Call("ClearAllNotifications", new object[0]);
		}

		public void ClearPendingLocalNotification(string key, string objectId)
		{
			this.notificationHandler.Call("ClearPendingLocalNotification", new object[]
			{
				key,
				objectId
			});
		}

		public void RegisterForRemoteNotifications()
		{
			if (this.mUrbanAirshipWrapper != null)
			{
				this.mUrbanAirshipWrapper.CallStatic("enablePush", new object[0]);
			}
		}

		public void UnregisterForRemoteNotifications()
		{
			if (this.mUrbanAirshipWrapper != null)
			{
				this.mUrbanAirshipWrapper.CallStatic("disablePush", new object[0]);
			}
		}

		private long GetEpochTime(DateTime time)
		{
			return (long)(time - this.epochDate).TotalMilliseconds;
		}

		public bool HasAuthorizedPushNotifications()
		{
			return this.mUrbanAirshipWrapper != null && this.mUrbanAirshipWrapper.CallStatic<bool>("isPushEnabled", new object[0]);
		}
	}
}
