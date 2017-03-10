using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class BattleCompleteStoryTrigger : AbstractStoryTrigger, IEventObserver
	{
		public BattleCompleteStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public override void Activate()
		{
			base.Activate();
			Service.Get<EventManager>().RegisterObserver(this, EventId.BattleEndFullyProcessed, EventPriority.Default);
		}

		private void RemoveListeners()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.BattleEndFullyProcessed);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.BattleEndFullyProcessed)
			{
				this.RemoveListeners();
				this.parent.SatisfyTrigger(this);
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			this.RemoveListeners();
			base.Destroy();
		}

		protected internal BattleCompleteStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BattleCompleteStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BattleCompleteStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleCompleteStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BattleCompleteStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).RemoveListeners();
			return -1L;
		}
	}
}
