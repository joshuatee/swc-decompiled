using StaRTS.Main.Controllers.Notifications;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType858 : $UnityType
	{
		public unsafe $UnityType858()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 579288) = ldftn($Invoke0);
			*(data + 579316) = ldftn($Invoke1);
			*(data + 579344) = ldftn($Invoke2);
			*(data + 579372) = ldftn($Invoke3);
			*(data + 579400) = ldftn($Invoke4);
			*(data + 579428) = ldftn($Invoke5);
			*(data + 579456) = ldftn($Invoke6);
			*(data + 579484) = ldftn($Invoke7);
			*(data + 579512) = ldftn($Invoke8);
			*(data + 579540) = ldftn($Invoke9);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((INotificationManager)GCHandledObjects.GCHandleToObject(instance)).BatchScheduleLocalNotifications((List<NotificationObject>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((INotificationManager)GCHandledObjects.GCHandleToObject(instance)).ClearAllPendingLocalNotifications(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((INotificationManager)GCHandledObjects.GCHandleToObject(instance)).ClearPendingLocalNotification(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((INotificationManager)GCHandledObjects.GCHandleToObject(instance)).ClearReceivedLocalNotifications();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((INotificationManager)GCHandledObjects.GCHandleToObject(instance)).GetDeviceToken());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((INotificationManager)GCHandledObjects.GCHandleToObject(instance)).HasAuthorizedPushNotifications());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((INotificationManager)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((INotificationManager)GCHandledObjects.GCHandleToObject(instance)).RegisterForRemoteNotifications();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((INotificationManager)GCHandledObjects.GCHandleToObject(instance)).ScheduleLocalNotification(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), *(*(IntPtr*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), Marshal.PtrToStringUni(*(IntPtr*)(args + 6)));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((INotificationManager)GCHandledObjects.GCHandleToObject(instance)).UnregisterForRemoteNotifications();
			return -1L;
		}
	}
}
