using StaRTS.Externals.Manimal;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Story;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Notifications
{
	public class SocialPushNotificationController : IEventObserver
	{
		private const string SQUAD_JOIN_REPROMPT = "SocialPushNotifRepromptActionForSquadJoin";

		private const string TROOP_REQUEST_REPROMPT = "SocialPushNotifRepromptActionForTroopRequest";

		private const string WAR_TROOP_REQUEST_REPROMPT = "SocialPushNotifRepromptActionForWarTroopRequest";

		private const string RAID_NOTIFY_REPROMPT = "SocialPushNotifRepromptActionForRaids";

		public SocialPushNotificationController()
		{
			Service.Set<SocialPushNotificationController>(this);
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.SquadJoinedByCurrentPlayer);
			eventManager.RegisterObserver(this, EventId.SquadJoinInviteAcceptedByCurrentPlayer);
			eventManager.RegisterObserver(this, EventId.SquadTroopsRequestedByCurrentPlayer);
			eventManager.RegisterObserver(this, EventId.SquadWarTroopsRequestedByCurrentPlayer);
			eventManager.RegisterObserver(this, EventId.RaidNotifyRequest);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (Service.Get<NotificationController>().HasAgreedToNotifications())
			{
				return EatResponse.NotEaten;
			}
			uint time = ServerTime.Time;
			ServerPlayerPrefs serverPlayerPrefs = Service.Get<ServerPlayerPrefs>();
			int num = Convert.ToInt32(serverPlayerPrefs.GetPref(ServerPref.PushAuthPromptedCount), CultureInfo.InvariantCulture);
			bool flag = this.HasReachedPushNotificationLimit(num);
			if (id != EventId.RaidNotifyRequest)
			{
				switch (id)
				{
				case EventId.SquadJoinedByCurrentPlayer:
				case EventId.SquadJoinInviteAcceptedByCurrentPlayer:
					if (!flag && this.HasEnoughTimeElapsed(time, ServerPref.LastPushAuthPromptSquadJoinedTime, GameConstants.PUSH_NOTIFICATION_SQUAD_JOIN_COOLDOWN) && !this.IsHolonetOpen())
					{
						new ActionChain("SocialPushNotifRepromptActionForSquadJoin");
						serverPlayerPrefs.SetPref(ServerPref.LastPushAuthPromptSquadJoinedTime, time.ToString());
						serverPlayerPrefs.SetPref(ServerPref.PushAuthPromptedCount, (num + 1).ToString());
						Service.Get<ServerAPI>().Sync(new SetPrefsCommand(false));
					}
					break;
				case EventId.SquadTroopsRequestedByCurrentPlayer:
					if (!flag && this.HasEnoughTimeElapsed(time, ServerPref.LastPushAuthPromptTroopRequestTime, GameConstants.PUSH_NOTIFICATIONS_TROOP_REQUEST_COOLDOWN))
					{
						new ActionChain("SocialPushNotifRepromptActionForTroopRequest");
						serverPlayerPrefs.SetPref(ServerPref.LastPushAuthPromptTroopRequestTime, time.ToString());
						serverPlayerPrefs.SetPref(ServerPref.PushAuthPromptedCount, (num + 1).ToString());
						Service.Get<ServerAPI>().Sync(new SetPrefsCommand(false));
					}
					break;
				case EventId.SquadWarTroopsRequestedByCurrentPlayer:
					if (!flag && this.HasEnoughTimeElapsed(time, ServerPref.LastPushAuthPromptTroopRequestTime, GameConstants.PUSH_NOTIFICATIONS_TROOP_REQUEST_COOLDOWN))
					{
						new ActionChain("SocialPushNotifRepromptActionForWarTroopRequest");
						serverPlayerPrefs.SetPref(ServerPref.LastPushAuthPromptTroopRequestTime, time.ToString());
						serverPlayerPrefs.SetPref(ServerPref.PushAuthPromptedCount, (num + 1).ToString());
						Service.Get<ServerAPI>().Sync(new SetPrefsCommand(false));
					}
					break;
				}
			}
			else
			{
				new ActionChain("SocialPushNotifRepromptActionForRaids");
				serverPlayerPrefs.SetPref(ServerPref.LastPushAuthPromptTroopRequestTime, time.ToString());
				serverPlayerPrefs.SetPref(ServerPref.PushAuthPromptedCount, (num + 1).ToString());
				Service.Get<ServerAPI>().Sync(new SetPrefsCommand(false));
			}
			return EatResponse.NotEaten;
		}

		private bool HasEnoughTimeElapsed(uint nowSeconds, ServerPref pref, float cooldown)
		{
			ServerPlayerPrefs serverPlayerPrefs = Service.Get<ServerPlayerPrefs>();
			uint num = Convert.ToUInt32(serverPlayerPrefs.GetPref(pref), CultureInfo.InvariantCulture);
			uint num2 = nowSeconds - num;
			uint num3 = (uint)(cooldown * 3600f);
			return num2 > num3;
		}

		private bool HasReachedPushNotificationLimit(int timesAsked)
		{
			return timesAsked >= GameConstants.PUSH_NOTIFICATION_MAX_REACHED;
		}

		private bool IsHolonetOpen()
		{
			return Service.Get<ScreenController>().GetHighestLevelScreen<HolonetScreen>() != null;
		}

		protected internal SocialPushNotificationController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialPushNotificationController)GCHandledObjects.GCHandleToObject(instance)).HasReachedPushNotificationLimit(*(int*)args));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialPushNotificationController)GCHandledObjects.GCHandleToObject(instance)).IsHolonetOpen());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialPushNotificationController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
