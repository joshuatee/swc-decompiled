using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Objectives
{
	public class DeploySpecialAttackIdObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		private string specialAttackID;

		public DeploySpecialAttackIdObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			this.specialAttackID = vo.Item;
			Service.Get<EventManager>().RegisterObserver(this, EventId.SpecialAttackSpawned);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.SpecialAttackSpawned)
			{
				if (base.IsEventValidForBattleObjective())
				{
					SpecialAttack specialAttack = (SpecialAttack)cookie;
					if (specialAttack.VO.SpecialAttackID == this.specialAttackID)
					{
						this.parent.Progress(this, 1);
					}
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.SpecialAttackSpawned);
			base.Destroy();
		}
	}
}
