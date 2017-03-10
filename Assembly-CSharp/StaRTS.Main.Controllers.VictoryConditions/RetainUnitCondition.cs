using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers.VictoryConditions
{
	public class RetainUnitCondition : AbstractCondition, IEventObserver
	{
		private const int AMOUNT_ARG = 0;

		private const int UNIT_ID_ARG = 1;

		private const int MIN_LEVEL_ARG = 2;

		private const string ANY_STRING = "any";

		private bool any;

		private string unitMatchId;

		private int level;

		private TeamType teamMatchType;

		protected int unitsToKill;

		protected int unitsKilled;

		private ConditionMatchType matchType;

		public RetainUnitCondition(ConditionVO vo, IConditionParent parent, ConditionMatchType matchType) : base(vo, parent)
		{
			this.matchType = matchType;
			this.unitMatchId = this.prepareArgs[1];
			this.unitsToKill = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
			this.unitsKilled = 0;
			this.any = (this.unitMatchId == "any");
			if (matchType == ConditionMatchType.Uid)
			{
				this.level = Service.Get<IDataController>().Get<TroopTypeVO>(this.unitMatchId).Lvl;
				return;
			}
			if (!this.any && this.prepareArgs.Length > 2)
			{
				this.level = Convert.ToInt32(this.prepareArgs[2], CultureInfo.InvariantCulture);
				return;
			}
			this.level = 0;
		}

		public override void Start()
		{
			this.teamMatchType = Service.Get<BattleController>().CurrentPlayerTeamType;
			this.events.RegisterObserver(this, EventId.EntityKilled, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.EntityKilled)
			{
				Entity entity = (Entity)cookie;
				TroopComponent troopComponent = entity.Get<TroopComponent>();
				if (troopComponent != null && this.IsTroopValid(troopComponent))
				{
					this.unitsKilled++;
					this.EvaluateAmount();
				}
			}
			return EatResponse.NotEaten;
		}

		private bool IsTroopValid(TroopComponent component)
		{
			if (component.Entity.Get<TeamComponent>().TeamType != this.teamMatchType)
			{
				return false;
			}
			if (this.any)
			{
				return true;
			}
			ITroopDeployableVO troopType = component.TroopType;
			if (troopType.Lvl >= this.level)
			{
				switch (this.matchType)
				{
				case ConditionMatchType.Uid:
					return troopType.Uid == this.unitMatchId;
				case ConditionMatchType.Id:
					return troopType.UpgradeGroup == this.unitMatchId;
				case ConditionMatchType.Type:
					return troopType.Type == StringUtils.ParseEnum<TroopType>(this.unitMatchId);
				}
			}
			return false;
		}

		protected virtual void EvaluateAmount()
		{
			if (this.IsConditionSatisfied())
			{
				this.parent.ChildFailed(this);
			}
		}

		public override void Destroy()
		{
			this.events.UnregisterObserver(this, EventId.EntityKilled);
		}

		public override bool IsConditionSatisfied()
		{
			return this.unitsKilled <= this.unitsToKill;
		}

		public override void GetProgress(out int current, out int total)
		{
			current = this.unitsKilled;
			total = this.unitsToKill;
		}

		protected internal RetainUnitCondition(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((RetainUnitCondition)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((RetainUnitCondition)GCHandledObjects.GCHandleToObject(instance)).EvaluateAmount();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RetainUnitCondition)GCHandledObjects.GCHandleToObject(instance)).IsConditionSatisfied());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RetainUnitCondition)GCHandledObjects.GCHandleToObject(instance)).IsTroopValid((TroopComponent)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RetainUnitCondition)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((RetainUnitCondition)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
