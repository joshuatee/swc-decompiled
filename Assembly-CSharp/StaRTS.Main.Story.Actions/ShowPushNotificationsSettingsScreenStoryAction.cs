using StaRTS.Externals.BI;
using StaRTS.Main.Configs;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Notifications;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Story.Actions
{
	public class ShowPushNotificationsSettingsScreenStoryAction : AbstractStoryAction
	{
		public const string DEFAULT_PUSH_NOTIF_AUTH_MSG = "notif_auth_alert_message";

		public ShowPushNotificationsSettingsScreenStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			this.ShowPushNotifPrompt();
		}

		private void ShowPushNotifPrompt()
		{
			Service.Get<BILoggingController>().TrackGameAction("push_notification", "01_custom_ask", Service.Get<BuildingLookupController>().GetHighestLevelHQ().ToString(), string.Empty, 1);
			string descriptionText = string.Empty;
			if (this.prepareArgs.Length < 1)
			{
				descriptionText = "notif_auth_alert_message";
			}
			else
			{
				descriptionText = this.prepareArgs[0];
			}
			Service.Get<BILoggingController>().TrackGameAction("push_notification", "03_standard_ask", Service.Get<BuildingLookupController>().GetHighestLevelHQ().ToString(), string.Empty, 1);
			bool showIncent = GameConstants.PUSH_NOTIFICATION_ENABLE_INCENTIVE && !Service.Get<CurrentPlayer>().IsPushIncentivized;
			Service.Get<ScreenController>().AddScreen(new TwoButtonFueScreen(false, new OnScreenModalResult(this.OnConfirmationScreenClosed), null, descriptionText, showIncent));
		}

		private void OnConfirmationScreenClosed(object result, object cookie)
		{
			bool flag = result != null;
			this.UpdatePushNotificationsSetting(flag);
			string action = "02_custom_allow";
			if (!flag)
			{
				action = "02_custom_deny";
			}
			Service.Get<BILoggingController>().TrackGameAction("push_notification", action, Service.Get<BuildingLookupController>().GetHighestLevelHQ().ToString(), string.Empty, 1);
		}

		private void UpdatePushNotificationsSetting(bool enable)
		{
			Service.Get<UserInputInhibitor>().AllowAll();
			int notificationsLevel = (!enable) ? 0 : 100;
			PlayerSettings.SetNotificationsLevel(notificationsLevel);
			Service.Get<NotificationController>().Enabled = enable;
			string deviceToken = Service.Get<NotificationController>().GetDeviceToken();
			string action = "04_standard_deny";
			if (!string.IsNullOrEmpty(deviceToken))
			{
				action = "04_standard_allow";
			}
			Service.Get<BILoggingController>().TrackGameAction("push_notification", action, Service.Get<BuildingLookupController>().GetHighestLevelHQ().ToString(), string.Empty);
			this.parent.ChildComplete(this);
		}
	}
}
