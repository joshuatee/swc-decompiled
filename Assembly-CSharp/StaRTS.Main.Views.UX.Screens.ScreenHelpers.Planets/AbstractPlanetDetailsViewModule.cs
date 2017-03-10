using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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

		protected internal AbstractPlanetDetailsViewModule(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractPlanetDetailsViewModule)GCHandledObjects.GCHandleToObject(instance)).CampController);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractPlanetDetailsViewModule)GCHandledObjects.GCHandleToObject(instance)).EvtManager);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractPlanetDetailsViewModule)GCHandledObjects.GCHandleToObject(instance)).LangController);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractPlanetDetailsViewModule)GCHandledObjects.GCHandleToObject(instance)).Player);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractPlanetDetailsViewModule)GCHandledObjects.GCHandleToObject(instance)).RManager);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractPlanetDetailsViewModule)GCHandledObjects.GCHandleToObject(instance)).ScrController);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractPlanetDetailsViewModule)GCHandledObjects.GCHandleToObject(instance)).Sdc);
		}
	}
}
