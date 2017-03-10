using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class BlockingWarStoryTrigger : AbstractStoryTrigger, IEventObserver
	{
		public BlockingWarStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public override void Activate()
		{
			base.Activate();
			Service.Get<EventManager>().RegisterObserver(this, EventId.WarLaunchFlow, EventPriority.BeforeDefault);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.WarLaunchFlow)
			{
				this.parent.SatisfyTrigger(this);
				return EatResponse.Eaten;
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.WarLaunchFlow);
			base.Destroy();
		}

		protected internal BlockingWarStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BlockingWarStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BlockingWarStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BlockingWarStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
