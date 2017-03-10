using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.UX.Screens.Squads;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class CloseScreenStoryAction : AbstractStoryAction
	{
		private const string PLANET_DETAILS_SCREEN = "PlanetDetailsScreen";

		private const string SQUAD_SCREEN_OPEN = "";

		public CloseScreenStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			ScreenController screenController = Service.Get<ScreenController>();
			UXController uXController = Service.Get<UXController>();
			ScreenBase highestLevelScreen = screenController.GetHighestLevelScreen<ClosableScreen>();
			if (highestLevelScreen != null)
			{
				string name = highestLevelScreen.GetType().get_Name();
				if (name == "PlanetDetailsScreen")
				{
					if (!Service.Get<BuildingLookupController>().HasNavigationCenter())
					{
						Service.Get<GalaxyViewController>().GoToHome();
					}
					else
					{
						Service.Get<GalaxyViewController>().TranstionPlanetToGalaxy();
					}
					highestLevelScreen.Close(null);
				}
				else
				{
					highestLevelScreen.Close(null);
				}
			}
			if (uXController.HUD.IsSquadScreenOpenAndCloseable())
			{
				SquadController squadController = Service.Get<SquadController>();
				if (squadController.StateManager.SquadScreenState == SquadScreenState.Donation)
				{
					SquadSlidingScreen highestLevelScreen2 = screenController.GetHighestLevelScreen<SquadSlidingScreen>();
					highestLevelScreen2.CloseDonationView();
				}
				else
				{
					uXController.HUD.SlideSquadScreenClosed();
				}
			}
			this.parent.ChildComplete(this);
		}

		protected internal CloseScreenStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CloseScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CloseScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
