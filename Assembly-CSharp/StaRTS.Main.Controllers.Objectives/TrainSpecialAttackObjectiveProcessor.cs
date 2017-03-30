using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Objectives
{
	public class TrainSpecialAttackObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		public TrainSpecialAttackObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.StarshipMobilized);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.StarshipMobilized)
			{
				if (base.IsEventValidForBattleObjective())
				{
					this.parent.Progress(this, 1);
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.StarshipMobilized);
			base.Destroy();
		}
	}
}
