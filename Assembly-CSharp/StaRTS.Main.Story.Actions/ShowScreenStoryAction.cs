using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class ShowScreenStoryAction : AbstractStoryAction, IEventObserver
	{
		private const string PLANET_DETAILS_SCREEN = "PlanetDetails";

		private const string GALAXY_MAP = "GalaxyMap";

		private string screenType;

		private string param;

		private EventManager eventManager;

		public ShowScreenStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
			this.eventManager = Service.Get<EventManager>();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ScreenLoaded)
			{
				ScreenBase screen = (ScreenBase)cookie;
				this.OnScreenReady(screen);
			}
			return EatResponse.NotEaten;
		}

		public override void Prepare()
		{
			if (string.IsNullOrEmpty(this.vo.PrepareString))
			{
				Service.Get<StaRTSLogger>().Error("ShowScreenStoryAction: " + this.vo.Uid + " : lacks a prepare string");
				return;
			}
			this.screenType = this.prepareArgs[0];
			if (this.prepareArgs.Length > 1)
			{
				this.param = this.prepareArgs[1];
			}
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (this.screenType == "PlanetDetails")
			{
				this.eventManager.RegisterObserver(this, EventId.ScreenLoaded, EventPriority.Default);
				IState currentState = Service.Get<GameStateMachine>().CurrentState;
				if (currentState is GalaxyState)
				{
					if (string.IsNullOrEmpty(this.param))
					{
						Service.Get<GalaxyViewController>().OpenPlanetDetailsForPlanet(currentPlayer.Planet.Uid);
					}
					else
					{
						Service.Get<GalaxyViewController>().OpenPlanetDetailsForPlanet(this.param);
					}
				}
				else
				{
					Service.Get<GalaxyViewController>().GoToPlanetView(currentPlayer.Planet.Uid, CampaignScreenSection.Main);
				}
				this.screenType = "PlanetDetailsScreen";
				return;
			}
			if (this.screenType == "GalaxyMap")
			{
				Service.Get<GalaxyViewController>().GoToGalaxyView();
				this.OnScreenReady(null);
				return;
			}
			Service.Get<StaRTSLogger>().Error("ShowScreenStoryAction: " + this.vo.Uid + " : invalid screen type provided: " + this.screenType);
		}

		private void OnScreenReady(ScreenBase screen)
		{
			if (screen != null)
			{
				string name = screen.GetType().get_Name();
				if (name != this.screenType)
				{
					return;
				}
			}
			this.eventManager.UnregisterObserver(this, EventId.ScreenLoaded);
			this.parent.ChildComplete(this);
		}

		protected internal ShowScreenStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ShowScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShowScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ShowScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnScreenReady((ScreenBase)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ShowScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
