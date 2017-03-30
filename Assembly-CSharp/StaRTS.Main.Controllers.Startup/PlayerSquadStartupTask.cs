using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Startup
{
	public class PlayerSquadStartupTask : StartupTask, IEventObserver
	{
		public PlayerSquadStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.SquadUpdateCompleted, EventPriority.Default);
			new PerkManager();
			new PerkViewController();
			Service.Get<SquadController>().Enable();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.SquadUpdateCompleted)
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.SquadUpdateCompleted);
				base.Complete();
			}
			return EatResponse.NotEaten;
		}
	}
}
