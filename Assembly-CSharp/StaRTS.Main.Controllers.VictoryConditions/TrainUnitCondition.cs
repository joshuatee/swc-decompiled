using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers.VictoryConditions
{
	public class TrainUnitCondition : AbstractCondition, IEventObserver
	{
		private const string ANY_STRING = "any";

		private const int AMOUNT_ARG = 0;

		private const int UNIT_MATCH_ARG = 1;

		private const int MIN_LEVEL_ARG = 2;

		private string unitMatch;

		private ConditionMatchType matchType;

		private int level;

		protected int unitsToTrain;

		protected int unitsTrained;

		private bool any;

		public TrainUnitCondition(ConditionVO vo, IConditionParent parent, int startingValue, ConditionMatchType matchType) : base(vo, parent)
		{
			this.matchType = matchType;
			this.unitMatch = this.prepareArgs[1];
			this.unitsToTrain = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
			this.unitsTrained = startingValue;
			if (this.unitMatch == "any")
			{
				this.any = true;
			}
			if (matchType == ConditionMatchType.Uid)
			{
				this.level = Service.Get<IDataController>().Get<TroopTypeVO>(this.unitMatch).Lvl;
				return;
			}
			if (!this.any)
			{
				this.level = Convert.ToInt32(this.prepareArgs[2], CultureInfo.InvariantCulture);
			}
		}

		public override void Start()
		{
			this.events.RegisterObserver(this, EventId.ContractCompleted, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ContractCompleted)
			{
				IDataController dataController = Service.Get<IDataController>();
				ContractEventData contractEventData = (ContractEventData)cookie;
				Contract contract = contractEventData.Contract;
				DeliveryType deliveryType = contract.DeliveryType;
				if (deliveryType == DeliveryType.Infantry || deliveryType == DeliveryType.Vehicle || deliveryType == DeliveryType.Mercenary)
				{
					TroopTypeVO troop = dataController.Get<TroopTypeVO>(contract.ProductUid);
					if (this.IsTroopValid(troop))
					{
						this.unitsTrained++;
						this.parent.ChildUpdated(this, 1);
						this.EvaluateAmount();
					}
				}
			}
			return EatResponse.NotEaten;
		}

		private bool IsTroopValid(TroopTypeVO troop)
		{
			if (this.any)
			{
				return true;
			}
			if (troop.Lvl >= this.level)
			{
				switch (this.matchType)
				{
				case ConditionMatchType.Uid:
					return troop.Uid == this.unitMatch;
				case ConditionMatchType.Id:
					return troop.TroopID == this.unitMatch;
				case ConditionMatchType.Type:
					return troop.Type == StringUtils.ParseEnum<TroopType>(this.unitMatch);
				}
			}
			return false;
		}

		protected virtual void EvaluateAmount()
		{
			if (this.IsConditionSatisfied())
			{
				this.parent.ChildSatisfied(this);
			}
		}

		public override void Destroy()
		{
			this.events.UnregisterObserver(this, EventId.ContractCompleted);
		}

		public override bool IsConditionSatisfied()
		{
			return this.unitsTrained >= this.unitsToTrain;
		}

		public override void GetProgress(out int current, out int total)
		{
			current = this.unitsTrained;
			total = this.unitsToTrain;
		}

		protected internal TrainUnitCondition(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TrainUnitCondition)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TrainUnitCondition)GCHandledObjects.GCHandleToObject(instance)).EvaluateAmount();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrainUnitCondition)GCHandledObjects.GCHandleToObject(instance)).IsConditionSatisfied());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrainUnitCondition)GCHandledObjects.GCHandleToObject(instance)).IsTroopValid((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrainUnitCondition)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TrainUnitCondition)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
