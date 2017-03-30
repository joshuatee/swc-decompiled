using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Objectives
{
	public class TrainSpecialAttackIdObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		private string specialAttackID;

		public TrainSpecialAttackIdObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			this.specialAttackID = vo.Item;
			Service.Get<EventManager>().RegisterObserver(this, EventId.StarshipMobilized);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.StarshipMobilized)
			{
				if (base.IsEventValidForBattleObjective())
				{
					ContractEventData contractEventData = (ContractEventData)cookie;
					IDataController dataController = Service.Get<IDataController>();
					SpecialAttackTypeVO specialAttackTypeVO = dataController.Get<SpecialAttackTypeVO>(contractEventData.Contract.ProductUid);
					if (specialAttackTypeVO.SpecialAttackID == this.specialAttackID)
					{
						this.parent.Progress(this, 1);
					}
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
