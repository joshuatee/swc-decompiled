using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Notifications
{
	public class WindowsNotificationManager : INotificationManager
	{
		public delegate void ScheduleLocalNotificationCallBack(string id, string message, DateTime time);

		public delegate void ClearAllPendingLocalNotificationsCallBack();

		public delegate void ClearPendingLocalNotificationCallBack(string id);

		public static WindowsNotificationManager.ScheduleLocalNotificationCallBack scheduleLocalNotification;

		public static WindowsNotificationManager.ClearAllPendingLocalNotificationsCallBack clearAllPendingLocalNotifications;

		public static WindowsNotificationManager.ClearPendingLocalNotificationCallBack clearPendingLocalNotification;

		public WindowsNotificationManager()
		{
		}

		public void Init()
		{
		}

		public string GetDeviceToken()
		{
			return "";
		}

		public void ScheduleLocalNotification(string notificationUid, string inProgressMessage, string message, string soundName, DateTime time, string key, string objectId)
		{
			if (!Service.Get<NotificationController>().Enabled)
			{
				return;
			}
			if (WindowsNotificationManager.scheduleLocalNotification != null)
			{
				WindowsNotificationManager.scheduleLocalNotification(key + objectId, message, time);
			}
		}

		public void ClearReceivedLocalNotifications()
		{
		}

		public void ClearAllPendingLocalNotifications(bool clearCountdownTimers)
		{
			if (WindowsNotificationManager.clearAllPendingLocalNotifications != null)
			{
				WindowsNotificationManager.clearAllPendingLocalNotifications();
			}
		}

		public void ClearPendingLocalNotification(string key, string objectId)
		{
			if (WindowsNotificationManager.clearPendingLocalNotification != null)
			{
				WindowsNotificationManager.clearPendingLocalNotification(key + objectId);
			}
		}

		public void RegisterForRemoteNotifications()
		{
		}

		public void UnregisterForRemoteNotifications()
		{
		}

		public void BatchScheduleLocalNotification(string notificationUid, string inProgressMessage, string message, string soundName, List<DateTime> times, string key, string objectId)
		{
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

		public bool HasAuthorizedPushNotifications()
		{
			return true;
		}

		protected internal WindowsNotificationManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((WindowsNotificationManager)GCHandledObjects.GCHandleToObject(instance)).BatchScheduleLocalNotification(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), (List<DateTime>)GCHandledObjects.GCHandleToObject(args[4]), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), Marshal.PtrToStringUni(*(IntPtr*)(args + 6)));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WindowsNotificationManager)GCHandledObjects.GCHandleToObject(instance)).BatchScheduleLocalNotifications((List<NotificationObject>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WindowsNotificationManager)GCHandledObjects.GCHandleToObject(instance)).ClearAllPendingLocalNotifications(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((WindowsNotificationManager)GCHandledObjects.GCHandleToObject(instance)).ClearPendingLocalNotification(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((WindowsNotificationManager)GCHandledObjects.GCHandleToObject(instance)).ClearReceivedLocalNotifications();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WindowsNotificationManager)GCHandledObjects.GCHandleToObject(instance)).GetDeviceToken());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WindowsNotificationManager)GCHandledObjects.GCHandleToObject(instance)).HasAuthorizedPushNotifications());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((WindowsNotificationManager)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((WindowsNotificationManager)GCHandledObjects.GCHandleToObject(instance)).RegisterForRemoteNotifications();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((WindowsNotificationManager)GCHandledObjects.GCHandleToObject(instance)).ScheduleLocalNotification(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), *(*(IntPtr*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), Marshal.PtrToStringUni(*(IntPtr*)(args + 6)));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((WindowsNotificationManager)GCHandledObjects.GCHandleToObject(instance)).UnregisterForRemoteNotifications();
			return -1L;
		}
	}
}
