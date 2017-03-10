using StaRTS.Externals.EnvironmentManager;
using StaRTS.Externals.Kochava;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.Notifications;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class PlayerLoginStartupTask : StartupTask
	{
		public PlayerLoginStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			LoginCommand loginCommand = new LoginCommand(new LoginRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId,
				LocalePreference = Service.Get<Lang>().Locale,
				DeviceToken = Service.Get<NotificationController>().GetDeviceToken(),
				TimeZoneOffset = Service.Get<EnvironmentController>().GetTimezoneOffset()
			});
			loginCommand.AddSuccessCallback(new AbstractCommand<LoginRequest, LoginResponse>.OnSuccessCallback(this.OnLoginComplete));
			Service.Get<ServerAPI>().Async(loginCommand);
		}

		private void OnLoginComplete(LoginResponse response, object cookie)
		{
			Service.Get<StaRTSLogger>().Debug("Player Logged In Successfully.");
			Service.Get<EventManager>().SendEvent(EventId.PlayerLoginSuccess, null);
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Service.Get<ServerAPI>().StartSession(currentPlayer.LoginTime);
			if (!Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				Service.Get<NotificationController>().TryEnableNotifications();
			}
			if (Service.Get<CurrentPlayer>().SessionCountToday == 1)
			{
				KochavaPlugin.FireEvent("dayPlayed", "1");
			}
			currentPlayer.Prizes.Crates.UpdateBadgingBasedOnAvailableCrates();
			base.Complete();
			Service.Get<IAccountSyncController>().UpdateExternalAccountInfo(new OnUpdateExternalAccountInfoResponseReceived(this.OnUpdateExternalAccountInfoResponseReceived));
		}

		private void OnUpdateExternalAccountInfoResponseReceived()
		{
			Service.Get<ISocialDataController>().PopulateFacebookData();
		}

		protected internal PlayerLoginStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PlayerLoginStartupTask)GCHandledObjects.GCHandleToObject(instance)).OnLoginComplete((LoginResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlayerLoginStartupTask)GCHandledObjects.GCHandleToObject(instance)).OnUpdateExternalAccountInfoResponseReceived();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PlayerLoginStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
