using StaRTS.Externals.Manimal;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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

		protected internal ServerController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ServerController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
