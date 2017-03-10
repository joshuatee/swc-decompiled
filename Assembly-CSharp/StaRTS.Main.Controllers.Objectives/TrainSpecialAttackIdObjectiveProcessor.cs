using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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
			if (id == EventId.StarshipMobilized && base.IsEventValidForBattleObjective())
			{
				ContractEventData contractEventData = (ContractEventData)cookie;
				IDataController dataController = Service.Get<IDataController>();
				SpecialAttackTypeVO specialAttackTypeVO = dataController.Get<SpecialAttackTypeVO>(contractEventData.Contract.ProductUid);
				if (specialAttackTypeVO.SpecialAttackID == this.specialAttackID)
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

		protected internal TrainSpecialAttackIdObjectiveProcessor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TrainSpecialAttackIdObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrainSpecialAttackIdObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
