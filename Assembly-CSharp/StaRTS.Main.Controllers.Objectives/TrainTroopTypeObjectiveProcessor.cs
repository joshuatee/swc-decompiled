using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Objectives
{
	public class TrainTroopTypeObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		private DeliveryType deliveryType;

		public TrainTroopTypeObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			TroopType troopType = StringUtils.ParseEnum<TroopType>(vo.Item);
			if (troopType == TroopType.Champion)
			{
				this.deliveryType = DeliveryType.Champion;
			}
			else if (troopType == TroopType.Hero)
			{
				this.deliveryType = DeliveryType.Hero;
			}
			else if (troopType == TroopType.Infantry)
			{
				this.deliveryType = DeliveryType.Infantry;
			}
			else if (troopType == TroopType.Mercenary)
			{
				this.deliveryType = DeliveryType.Mercenary;
			}
			else if (troopType == TroopType.Vehicle)
			{
				this.deliveryType = DeliveryType.Vehicle;
			}
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
				if (this.deliveryType == contractEventData.Contract.DeliveryType)
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
