using StaRTS.Externals.Manimal;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers
{
	public class ServerController : IEventObserver
	{
		public ServerController()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.ApplicationPauseToggled, EventPriority.ServerAfterOthers);
			eventManager.RegisterObserver(this, EventId.ApplicationQuit, EventPriority.ServerAfterOthers);
			Service.Set<ServerController>(this);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.ApplicationPauseToggled)
			{
				if (id == EventId.ApplicationQuit)
				{
					Service.Get<ServerAPI>().Sync();
				}
			}
			else
			{
				bool flag = (bool)cookie;
				if (flag)
				{
					Service.Get<ServerAPI>().Sync();
				}
			}
			return EatResponse.NotEaten;
		}
	}
}
