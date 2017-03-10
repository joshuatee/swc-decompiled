using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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
			if (id == EventId.TroopRecruited || id == EventId.HeroMobilized)
			{
				base.CheckUnusedPveFlag();
				ContractEventData contractEventData = cookie as ContractEventData;
				if (this.deliveryType == contractEventData.Contract.DeliveryType)
				{
					this.parent.Progress(this, 1);
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

		protected internal TrainTroopTypeObjectiveProcessor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TrainTroopTypeObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrainTroopTypeObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
