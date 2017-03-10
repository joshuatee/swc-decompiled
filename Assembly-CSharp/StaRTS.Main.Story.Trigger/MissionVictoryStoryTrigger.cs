using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class MissionVictoryStoryTrigger : AbstractStoryTrigger, IEventObserver
	{
		private const int MISSION_UID_ARG = 0;

		public MissionVictoryStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public override void Activate()
		{
			base.Activate();
			Service.Get<EventManager>().RegisterObserver(this, EventId.VictoryConditionSuccess, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.VictoryConditionSuccess)
			{
				ConditionVO conditionVO = (ConditionVO)cookie;
				if (conditionVO.Uid.ToLower() == this.prepareArgs[0].ToLower())
				{
					this.parent.SatisfyTrigger(this);
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.VictoryConditionSuccess);
			base.Destroy();
		}

		protected internal MissionVictoryStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((MissionVictoryStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((MissionVictoryStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MissionVictoryStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
