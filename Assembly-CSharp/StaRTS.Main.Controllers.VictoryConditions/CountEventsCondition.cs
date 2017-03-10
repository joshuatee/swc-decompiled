using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers.VictoryConditions
{
	public class CountEventsCondition : AbstractCondition, IEventObserver
	{
		private const int AMOUNT_ARG = 0;

		private const int EVENT_ARG = 1;

		private int eventsCounted;

		private int eventsThreshold;

		private EventId releventEvent;

		public CountEventsCondition(ConditionVO vo, IConditionParent parent, int startingValue) : base(vo, parent)
		{
			this.eventsCounted = startingValue;
			this.eventsThreshold = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
			this.releventEvent = StringUtils.ParseEnum<EventId>(this.prepareArgs[1]);
		}

		public CountEventsCondition(ConditionVO vo, IConditionParent parent, int startingValue, string overrideEvent) : base(vo, parent)
		{
			this.eventsCounted = startingValue;
			this.eventsThreshold = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
			this.releventEvent = StringUtils.ParseEnum<EventId>(overrideEvent);
		}

		public override void Start()
		{
			this.events.RegisterObserver(this, this.releventEvent, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == this.releventEvent)
			{
				this.parent.ChildUpdated(this, 1);
				this.IncrementEvent(1);
			}
			return EatResponse.NotEaten;
		}

		private void IncrementEvent(int delta)
		{
			this.eventsCounted += delta;
			if (this.eventsCounted >= this.eventsThreshold)
			{
				this.parent.ChildSatisfied(this);
			}
		}

		public override void Destroy()
		{
			this.events.UnregisterObserver(this, this.releventEvent);
		}

		public override bool IsConditionSatisfied()
		{
			return this.eventsCounted >= this.eventsThreshold;
		}

		public override void GetProgress(out int current, out int total)
		{
			current = this.eventsCounted;
			total = this.eventsThreshold;
		}

		protected internal CountEventsCondition(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CountEventsCondition)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CountEventsCondition)GCHandledObjects.GCHandleToObject(instance)).IncrementEvent(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CountEventsCondition)GCHandledObjects.GCHandleToObject(instance)).IsConditionSatisfied());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CountEventsCondition)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((CountEventsCondition)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
