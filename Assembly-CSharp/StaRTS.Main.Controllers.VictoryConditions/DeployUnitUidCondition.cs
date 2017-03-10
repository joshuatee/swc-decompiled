using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers.VictoryConditions
{
	public class DeployUnitUidCondition : AbstractCondition, IEventObserver
	{
		private const int AMOUNT_ARG = 0;

		private const int UNIT_UID_ARG = 1;

		private string unitMatchUid;

		protected int unitsToDeploy;

		protected int unitsDeployed;

		public DeployUnitUidCondition(ConditionVO vo, IConditionParent parent) : base(vo, parent)
		{
			this.unitMatchUid = this.prepareArgs[1];
			this.unitsToDeploy = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
			this.unitsDeployed = 0;
		}

		public override void Start()
		{
			this.events.RegisterObserver(this, EventId.TroopDeployed, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.TroopDeployed)
			{
				Entity entity = (Entity)cookie;
				TroopComponent troopComponent = entity.Get<TroopComponent>();
				if (troopComponent != null && this.IsTroopValid(troopComponent))
				{
					this.unitsDeployed++;
					this.EvaluateAmount();
				}
			}
			return EatResponse.NotEaten;
		}

		private bool IsTroopValid(TroopComponent component)
		{
			return component.TroopType.Uid == this.unitMatchUid;
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
			this.events.UnregisterObserver(this, EventId.TroopDeployed);
		}

		public override bool IsConditionSatisfied()
		{
			return this.unitsDeployed >= this.unitsToDeploy;
		}

		public override void GetProgress(out int current, out int total)
		{
			current = this.unitsDeployed;
			total = this.unitsToDeploy;
		}

		protected internal DeployUnitUidCondition(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DeployUnitUidCondition)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DeployUnitUidCondition)GCHandledObjects.GCHandleToObject(instance)).EvaluateAmount();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployUnitUidCondition)GCHandledObjects.GCHandleToObject(instance)).IsConditionSatisfied());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployUnitUidCondition)GCHandledObjects.GCHandleToObject(instance)).IsTroopValid((TroopComponent)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployUnitUidCondition)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DeployUnitUidCondition)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
