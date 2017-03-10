using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class FactionChangedStoryTrigger : AbstractStoryTrigger, IEventObserver
	{
		private const int FACTION_ARG = 0;

		private FactionType faction;

		public FactionChangedStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public override void Activate()
		{
			base.Activate();
			this.faction = StringUtils.ParseEnum<FactionType>(this.prepareArgs[0]);
			Service.Get<EventManager>().RegisterObserver(this, EventId.ScreenClosing, EventPriority.Default);
		}

		private void RemoveListeners()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ScreenClosing);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ScreenClosing)
			{
				ChooseFactionScreen chooseFactionScreen = cookie as ChooseFactionScreen;
				if (chooseFactionScreen != null)
				{
					FactionType factionType = Service.Get<CurrentPlayer>().Faction;
					if (factionType == this.faction)
					{
						this.RemoveListeners();
						this.parent.SatisfyTrigger(this);
					}
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			this.RemoveListeners();
			base.Destroy();
		}

		protected internal FactionChangedStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((FactionChangedStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((FactionChangedStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionChangedStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((FactionChangedStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).RemoveListeners();
			return -1L;
		}
	}
}
