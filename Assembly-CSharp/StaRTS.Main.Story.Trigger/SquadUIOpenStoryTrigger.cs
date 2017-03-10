using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class SquadUIOpenStoryTrigger : AbstractStoryTrigger, IEventObserver
	{
		public SquadUIOpenStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public override void Activate()
		{
			base.Activate();
			if (this.IsSatisfied())
			{
				this.parent.SatisfyTrigger(this);
				return;
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.SquadScreenOpenedOrClosed, EventPriority.BeforeDefault);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.SquadScreenOpenedOrClosed)
			{
				bool flag = (bool)cookie;
				if (this.IsSatisfied() | flag)
				{
					this.UnregisterObservers();
					this.parent.SatisfyTrigger(this);
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			this.UnregisterObservers();
			base.Destroy();
		}

		private void UnregisterObservers()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.SquadScreenOpenedOrClosed);
		}

		private bool IsSatisfied()
		{
			bool result = false;
			if (Service.IsSet<UXController>())
			{
				HUD hUD = Service.Get<UXController>().HUD;
				if (hUD != null)
				{
					result = hUD.IsSquadScreenOpenAndCloseable();
				}
			}
			return result;
		}

		protected internal SquadUIOpenStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadUIOpenStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadUIOpenStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadUIOpenStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).IsSatisfied());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadUIOpenStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadUIOpenStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).UnregisterObservers();
			return -1L;
		}
	}
}
