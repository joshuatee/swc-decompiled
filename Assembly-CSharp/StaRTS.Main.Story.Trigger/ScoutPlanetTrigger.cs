using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class ScoutPlanetTrigger : AbstractStoryTrigger, IEventObserver
	{
		public ScoutPlanetTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.PlanetScoutingStart && (string.IsNullOrEmpty(this.vo.PrepareString) || this.vo.PrepareString.Equals((string)cookie)))
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.PlanetScoutingStart);
				this.parent.SatisfyTrigger(this);
			}
			return EatResponse.NotEaten;
		}

		public override void Activate()
		{
			base.Activate();
			Service.Get<EventManager>().RegisterObserver(this, EventId.PlanetScoutingStart, EventPriority.Default);
		}

		protected internal ScoutPlanetTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ScoutPlanetTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScoutPlanetTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
