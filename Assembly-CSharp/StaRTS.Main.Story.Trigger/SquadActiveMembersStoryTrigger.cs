using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class SquadActiveMembersStoryTrigger : AbstractStoryTrigger, IEventObserver
	{
		private const int ARG_MIN_SQUAD_ACTIVE_MEMBERS = 0;

		private const int ARG_MAX_SQUAD_ACTIVE_MEMBERS = 1;

		public SquadActiveMembersStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
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
			Service.Get<EventManager>().RegisterObserver(this, EventId.SquadJoinedByCurrentPlayer);
			Service.Get<EventManager>().RegisterObserver(this, EventId.SquadUpdated);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if ((id == EventId.SquadUpdated || id == EventId.SquadJoinedByCurrentPlayer) && this.IsSatisfied())
			{
				this.UnregisterObservers();
				this.parent.SatisfyTrigger(this);
			}
			return EatResponse.NotEaten;
		}

		private bool IsSatisfied()
		{
			bool result = false;
			int num = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
			int num2 = Convert.ToInt32(this.prepareArgs[1], CultureInfo.InvariantCulture);
			SquadController squadController = Service.Get<SquadController>();
			Squad currentSquad = squadController.StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				result = false;
			}
			else if (currentSquad.ActiveMemberCount >= num && currentSquad.ActiveMemberCount <= num2)
			{
				result = true;
			}
			return result;
		}

		public override void Destroy()
		{
			this.UnregisterObservers();
			base.Destroy();
		}

		private void UnregisterObservers()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.SquadJoinedByCurrentPlayer);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.SquadUpdated);
		}

		protected internal SquadActiveMembersStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadActiveMembersStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadActiveMembersStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadActiveMembersStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).IsSatisfied());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadActiveMembersStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadActiveMembersStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).UnregisterObservers();
			return -1L;
		}
	}
}
