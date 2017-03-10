using StaRTS.Externals.BI;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Configs;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Notifications
{
	public class NotificationController
	{
		public const string SOUND_DEFAULT = "sfx_ui_notification";

		private INotificationManager notificationManager;

		private NotificationEventManager notificationEventManager;

		private bool isEnabled;

		public bool Enabled
		{
			get
			{
				return this.isEnabled;
			}
			set
			{
				this.isEnabled = value;
				if (this.isEnabled)
				{
					this.RegisterForRemoteNotifications();
					return;
				}
				this.UnregisterForRemoteNotifications();
			}
		}

		public NotificationController()
		{
			Service.Set<NotificationController>(this);
			this.notificationManager = new WindowsNotificationManager();
			this.notificationEventManager = new NotificationEventManager();
		}

		public bool HasAgreedToNotifications()
		{
			return PlayerSettings.GetNotificationsLevel() > 0;
		}

		public void TryEnableNotifications()
		{
			this.Enabled = (PlayerSettings.GetNotificationsLevel() > 0);
		}

		public bool HasAuthorizedPushNotifications()
		{
			return this.notificationManager.HasAuthorizedPushNotifications();
		}

		public void Init()
		{
			if (Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				return;
			}
			this.notificationManager.Init();
			this.notificationEventManager.Init();
		}

		public string GetDeviceToken()
		{
			return this.notificationManager.GetDeviceToken();
		}

		public void ScheduleLocalNotification(string message, string soundName, DateTime time)
		{
			this.ScheduleLocalNotification("", "", message, soundName, time, "", "");
		}

		public void ScheduleLocalNotification(string notificationUID, string inProgressMessage, string message, string soundName, DateTime time, string key, string objectId)
		{
			this.notificationManager.ScheduleLocalNotification(notificationUID, inProgressMessage, message, soundName, time, key, objectId);
		}

		public void BatchScheduleLocalNotifications(List<NotificationObject> notifs)
		{
			this.ClearAllPendingLocalNotifications();
			this.notificationManager.BatchScheduleLocalNotifications(notifs);
		}

		public void ClearReceivedLocalNotifications()
		{
			this.notificationManager.ClearReceivedLocalNotifications();
		}

		public void ClearAllPendingLocalNotifications()
		{
			this.notificationManager.ClearAllPendingLocalNotifications(true);
		}

		public void ClearPendingLocalNotification(string key, string objectId)
		{
			this.notificationManager.ClearPendingLocalNotification(key, objectId);
		}

		public void RegisterForRemoteNotifications()
		{
			if (this.notificationManager == null)
			{
				return;
			}
			this.notificationManager.RegisterForRemoteNotifications();
			string deviceToken = this.notificationManager.GetDeviceToken();
			if (!string.IsNullOrEmpty(deviceToken))
			{
				RegisterDeviceRequest registerDeviceRequest = new RegisterDeviceRequest();
				registerDeviceRequest.DeviceToken = deviceToken;
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				if (currentPlayer == null)
				{
					Service.Get<StaRTSLogger>().Warn("Trying to register for remote notification before CurrentPlayer is available");
					return;
				}
				registerDeviceRequest.PlayerId = currentPlayer.PlayerId;
				RegisterDeviceCommand registerDeviceCommand = new RegisterDeviceCommand(registerDeviceRequest);
				registerDeviceCommand.AddSuccessCallback(new AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>.OnSuccessCallback(this.OnRegisterSuccess));
				if (!Service.Get<ServerAPI>().Enabled && Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
				{
					Service.Get<ServerAPI>().Enabled = true;
				}
				Service.Get<ServerAPI>().Sync(registerDeviceCommand);
				Service.Get<BILoggingController>().TrackNetworkMappingInfo("ur", deviceToken);
			}
		}

		private void OnRegisterSuccess(RegisterDeviceResponse response, object cookie)
		{
			if (Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				Service.Get<ServerAPI>().Enabled = false;
			}
		}

		public void UnregisterForRemoteNotifications()
		{
			if (this.notificationManager == null)
			{
				return;
			}
			this.notificationManager.UnregisterForRemoteNotifications();
			string deviceToken = this.notificationManager.GetDeviceToken();
			if (!string.IsNullOrEmpty(deviceToken))
			{
				DeregisterDeviceRequest deregisterDeviceRequest = new DeregisterDeviceRequest();
				deregisterDeviceRequest.DeviceToken = deviceToken;
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				if (currentPlayer == null)
				{
					Service.Get<StaRTSLogger>().Warn("Trying to unregister for remote notification before CurrentPlayer is available");
					return;
				}
				deregisterDeviceRequest.PlayerId = currentPlayer.PlayerId;
				DeregisterDeviceCommand command = new DeregisterDeviceCommand(deregisterDeviceRequest);
				Service.Get<ServerAPI>().Async(command);
			}
			if (Service.IsSet<BuildingLookupController>())
			{
				Service.Get<BILoggingController>().TrackGameAction("push_notification", "04_standard_deny", Service.Get<BuildingLookupController>().GetHighestLevelHQ().ToString(), "");
			}
		}

		protected internal NotificationController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((NotificationController)GCHandledObjects.GCHandleToObject(instance)).BatchScheduleLocalNotifications((List<NotificationObject>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((NotificationController)GCHandledObjects.GCHandleToObject(instance)).ClearAllPendingLocalNotifications();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((NotificationController)GCHandledObjects.GCHandleToObject(instance)).ClearPendingLocalNotification(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((NotificationController)GCHandledObjects.GCHandleToObject(instance)).ClearReceivedLocalNotifications();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationController)GCHandledObjects.GCHandleToObject(instance)).Enabled);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationController)GCHandledObjects.GCHandleToObject(instance)).GetDeviceToken());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationController)GCHandledObjects.GCHandleToObject(instance)).HasAgreedToNotifications());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationController)GCHandledObjects.GCHandleToObject(instance)).HasAuthorizedPushNotifications());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((NotificationController)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((NotificationController)GCHandledObjects.GCHandleToObject(instance)).OnRegisterSuccess((RegisterDeviceResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((NotificationController)GCHandledObjects.GCHandleToObject(instance)).RegisterForRemoteNotifications();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((NotificationController)GCHandledObjects.GCHandleToObject(instance)).ScheduleLocalNotification(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((NotificationController)GCHandledObjects.GCHandleToObject(instance)).ScheduleLocalNotification(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), *(*(IntPtr*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), Marshal.PtrToStringUni(*(IntPtr*)(args + 6)));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((NotificationController)GCHandledObjects.GCHandleToObject(instance)).Enabled = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((NotificationController)GCHandledObjects.GCHandleToObject(instance)).TryEnableNotifications();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((NotificationController)GCHandledObjects.GCHandleToObject(instance)).UnregisterForRemoteNotifications();
			return -1L;
		}
	}
}
