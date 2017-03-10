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
	public class SquadMemberStoryTrigger : AbstractStoryTrigger, IEventObserver
	{
		private const int SITUATION_UID_ARG = 0;

		private const int SITUATION_IS_OWNER = 3;

		private const int SITUATION_IS_BRASS = 2;

		private const int SITUATION_IS_IN_SQUAD = 1;

		private const int SITUATION_NOT_IN_SQUAD = 0;

		public SquadMemberStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
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
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.SquadJoinedByCurrentPlayer && this.IsSatisfied())
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.SquadJoinedByCurrentPlayer);
				this.parent.SatisfyTrigger(this);
			}
			return EatResponse.NotEaten;
		}

		private bool IsSatisfied()
		{
			int num = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
			bool result = false;
			SquadController squadController = Service.Get<SquadController>();
			if (squadController.StateManager.GetCurrentSquad() == null)
			{
				if (num == 0)
				{
					result = true;
				}
			}
			else
			{
				SquadRole role = squadController.StateManager.Role;
				switch (num)
				{
				case 1:
					result = true;
					break;
				case 2:
					if (role == SquadRole.Owner || role == SquadRole.Officer)
					{
						result = true;
					}
					break;
				case 3:
					if (role == SquadRole.Owner)
					{
						result = true;
					}
					break;
				}
			}
			return result;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.SquadJoinedByCurrentPlayer);
			base.Destroy();
		}

		protected internal SquadMemberStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadMemberStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadMemberStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMemberStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).IsSatisfied());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMemberStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
