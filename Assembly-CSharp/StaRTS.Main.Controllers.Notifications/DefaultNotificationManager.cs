using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Notifications
{
	public class DefaultNotificationManager : INotificationManager
	{
		public DefaultNotificationManager()
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
		}

		public void BatchScheduleLocalNotifications(List<NotificationObject> list)
		{
		}

		public void ClearReceivedLocalNotifications()
		{
		}

		public void ClearAllPendingLocalNotifications(bool clearCountdownTimers)
		{
		}

		public void ClearPendingLocalNotification(string key, string objectId)
		{
		}

		public void RegisterForRemoteNotifications()
		{
		}

		public void UnregisterForRemoteNotifications()
		{
		}

		public bool HasAuthorizedPushNotifications()
		{
			return false;
		}

		protected internal DefaultNotificationManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DefaultNotificationManager)GCHandledObjects.GCHandleToObject(instance)).BatchScheduleLocalNotifications((List<NotificationObject>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DefaultNotificationManager)GCHandledObjects.GCHandleToObject(instance)).ClearAllPendingLocalNotifications(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DefaultNotificationManager)GCHandledObjects.GCHandleToObject(instance)).ClearPendingLocalNotification(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DefaultNotificationManager)GCHandledObjects.GCHandleToObject(instance)).ClearReceivedLocalNotifications();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultNotificationManager)GCHandledObjects.GCHandleToObject(instance)).GetDeviceToken());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefaultNotificationManager)GCHandledObjects.GCHandleToObject(instance)).HasAuthorizedPushNotifications());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((DefaultNotificationManager)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((DefaultNotificationManager)GCHandledObjects.GCHandleToObject(instance)).RegisterForRemoteNotifications();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((DefaultNotificationManager)GCHandledObjects.GCHandleToObject(instance)).ScheduleLocalNotification(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), *(*(IntPtr*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), Marshal.PtrToStringUni(*(IntPtr*)(args + 6)));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((DefaultNotificationManager)GCHandledObjects.GCHandleToObject(instance)).UnregisterForRemoteNotifications();
			return -1L;
		}
	}
}
