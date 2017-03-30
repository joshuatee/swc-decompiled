using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Story.Trigger
{
	public class WorldLoadStoryTrigger : AbstractStoryTrigger, IEventObserver
	{
		public WorldLoadStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public override void Activate()
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			this.parent.SatisfyTrigger(this);
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.WorldLoadComplete);
			base.Destroy();
		}
	}
}
