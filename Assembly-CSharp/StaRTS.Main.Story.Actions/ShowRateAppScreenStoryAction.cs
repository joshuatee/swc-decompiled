using StaRTS.Externals.BI;
using StaRTS.Externals.EnvironmentManager;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Story.Actions
{
	public class ShowRateAppScreenStoryAction : AbstractStoryAction, IEventObserver
	{
		private const string RATE_APP_TRUE = "1";

		private int numTimesViewed;

		public ShowRateAppScreenStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			if (!GameConstants.RATE_MY_APP_ENABLED)
			{
				this.parent.ChildComplete(this);
				return;
			}
			if (Service.Get<GalaxyViewController>().IsPlanetDetailsScreenOpeningOrOpen())
			{
				this.parent.ChildComplete(this);
				return;
			}
			if (Service.Get<EnvironmentController>().IsRestrictedProfile())
			{
				this.parent.ChildComplete(this);
				return;
			}
			if (Service.Get<ServerPlayerPrefs>().GetPref(ServerPref.RatedApp) == "1")
			{
				this.parent.ChildComplete(this);
				return;
			}
			if (Service.Get<ScreenController>().GetHighestLevelScreen<MissionCompleteScreen>() == null)
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.ScreenClosing, EventPriority.Default);
				Service.Get<ScreenController>().AddScreen(new RateAppScreen());
			}
			else
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.MissionCompleteScreenDisplayed, EventPriority.Default);
			}
			this.numTimesViewed = Convert.ToInt32(Service.Get<ServerPlayerPrefs>().GetPref(ServerPref.NumRateAppViewed));
			this.numTimesViewed++;
			Service.Get<ServerPlayerPrefs>().SetPref(ServerPref.NumRateAppViewed, this.numTimesViewed.ToString());
			SetPrefsCommand command = new SetPrefsCommand(false);
			Service.Get<ServerAPI>().Enqueue(command);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			EatResponse result = EatResponse.NotEaten;
			if (id != EventId.ScreenClosing)
			{
				if (id == EventId.MissionCompleteScreenDisplayed)
				{
					Service.Get<EventManager>().UnregisterObserver(this, EventId.MissionCompleteScreenDisplayed);
					Service.Get<EventManager>().RegisterObserver(this, EventId.ScreenClosing, EventPriority.Default);
					Service.Get<ScreenController>().AddScreen(new RateAppScreen());
				}
			}
			else
			{
				RateAppScreen rateAppScreen = cookie as RateAppScreen;
				if (rateAppScreen != null)
				{
					Service.Get<EventManager>().UnregisterObserver(this, EventId.ScreenClosing);
					this.OnNotificationScreenClosed(rateAppScreen.ClosedWithConfirmation);
				}
			}
			return result;
		}

		private void OnNotificationScreenClosed(bool gotoStoreFront)
		{
			if (gotoStoreFront)
			{
				Service.Get<BILoggingController>().TrackGameAction("rateapp", "yes", this.numTimesViewed.ToString(), null, 1);
				Service.Get<ServerPlayerPrefs>().SetPref(ServerPref.RatedApp, "1");
				SetPrefsCommand command = new SetPrefsCommand(false);
				Service.Get<ServerAPI>().Sync(command);
				GameUtils.TryAndOpenAppropriateStorePage();
			}
			else
			{
				Service.Get<BILoggingController>().TrackGameAction("rateapp", "no", this.numTimesViewed.ToString(), null, 1);
			}
			this.parent.ChildComplete(this);
		}
	}
}
