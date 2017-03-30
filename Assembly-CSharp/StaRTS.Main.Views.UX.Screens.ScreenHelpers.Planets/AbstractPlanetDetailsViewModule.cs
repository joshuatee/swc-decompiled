using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers.Planets
{
	public class AbstractPlanetDetailsViewModule
	{
		protected PlanetDetailsScreen screen;

		protected UXElement eventInfoGroup;

		protected CurrentPlayer Player
		{
			get
			{
				return Service.Get<CurrentPlayer>();
			}
		}

		protected CampaignController CampController
		{
			get
			{
				return Service.Get<CampaignController>();
			}
		}

		protected Lang LangController
		{
			get
			{
				return Service.Get<Lang>();
			}
		}

		protected EventManager EvtManager
		{
			get
			{
				return Service.Get<EventManager>();
			}
		}

		protected IDataController Sdc
		{
			get
			{
				return Service.Get<IDataController>();
			}
		}

		protected RewardManager RManager
		{
			get
			{
				return Service.Get<RewardManager>();
			}
		}

		protected ScreenController ScrController
		{
			get
			{
				return Service.Get<ScreenController>();
			}
		}

		public AbstractPlanetDetailsViewModule(PlanetDetailsScreen screen)
		{
			this.screen = screen;
		}
	}
}
