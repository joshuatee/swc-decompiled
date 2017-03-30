using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

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
			}
			else
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.SquadJoinedByCurrentPlayer);
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.SquadJoinedByCurrentPlayer)
			{
				if (this.IsSatisfied())
				{
					Service.Get<EventManager>().UnregisterObserver(this, EventId.SquadJoinedByCurrentPlayer);
					this.parent.SatisfyTrigger(this);
				}
			}
			return EatResponse.NotEaten;
		}

		private bool IsSatisfied()
		{
			int num = Convert.ToInt32(this.prepareArgs[0]);
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
	}
}
