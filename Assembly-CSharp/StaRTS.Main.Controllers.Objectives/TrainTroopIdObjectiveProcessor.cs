using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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
			if (id == EventId.TroopRecruited || id == EventId.HeroMobilized)
			{
				base.CheckUnusedPveFlag();
				ContractEventData contractEventData = cookie as ContractEventData;
				IDataController dataController = Service.Get<IDataController>();
				TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(contractEventData.Contract.ProductUid);
				if (troopTypeVO.TroopID == this.troopId)
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

		protected internal TrainTroopIdObjectiveProcessor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TrainTroopIdObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrainTroopIdObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
