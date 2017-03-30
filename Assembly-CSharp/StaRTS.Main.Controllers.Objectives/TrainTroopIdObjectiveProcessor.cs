using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Objectives
{
	public class TrainTroopIdObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		private string troopId;

		public TrainTroopIdObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			this.troopId = vo.Item;
			Service.Get<EventManager>().RegisterObserver(this, EventId.TroopRecruited);
			Service.Get<EventManager>().RegisterObserver(this, EventId.HeroMobilized);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			switch (id)
			{
			case EventId.TroopRecruited:
			case EventId.HeroMobilized:
			{
				base.CheckUnusedPveFlag();
				ContractEventData contractEventData = cookie as ContractEventData;
				IDataController dataController = Service.Get<IDataController>();
				TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(contractEventData.Contract.ProductUid);
				if (troopTypeVO.TroopID == this.troopId)
				{
					this.parent.Progress(this, 1);
				}
				break;
			}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.TroopRecruited);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.HeroMobilized);
			base.Destroy();
		}
	}
}
