using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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

		protected internal PlayerSquadStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerSquadStartupTask)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlayerSquadStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
