using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class ScreenAppearStoryTrigger : AbstractStoryTrigger, IEventObserver
	{
		private const string PLANET_DETAIL_SCREEN = "Planet_Detail_Screen";

		private const string PLANETARY_COMMAND_UPGRADE_SCREEN = "Planetary_Command_Upgrade_Screen";

		private const string PLANET_CONFIRM_SELECTION_SCREEN = "Confirm_Selected_Planet_Screen";

		private const string PLANET_CONFIRM_SMALL_BOX = "Planetary_Command_Confirm_Small_Box";

		public ScreenAppearStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public override void Activate()
		{
			base.Activate();
			Service.Get<EventManager>().RegisterObserver(this, EventId.ScreenLoaded, EventPriority.Default);
		}

		private void RemoveListeners()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ScreenLoaded);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ScreenLoaded)
			{
				AlertScreen alertScreen = cookie as AlertScreen;
				if (this.vo.PrepareString.Equals("Planetary_Command_Confirm_Small_Box") && alertScreen != null && "Tutorial".Equals(alertScreen.Tag))
				{
					this.RemoveListeners();
					this.parent.SatisfyTrigger(this);
				}
				else if (cookie is PlanetDetailsScreen && this.vo.PrepareString.Equals("Planet_Detail_Screen"))
				{
					this.RemoveListeners();
					this.parent.SatisfyTrigger(this);
				}
				else if (cookie is NavigationCenterUpgradeScreen && this.vo.PrepareString.Equals("Planetary_Command_Upgrade_Screen"))
				{
					this.RemoveListeners();
					this.parent.SatisfyTrigger(this);
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			this.RemoveListeners();
			base.Destroy();
		}

		protected internal ScreenAppearStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ScreenAppearStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ScreenAppearStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenAppearStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ScreenAppearStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).RemoveListeners();
			return -1L;
		}
	}
}
