using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers.VictoryConditions
{
	public class RetainBuildingCondition : AbstractCondition, IEventObserver
	{
		private const int AMOUNT_ARG = 0;

		private const int BUILDING_TYPE_ARG = 1;

		private const int MIN_LEVEL_ARG = 2;

		private const string ANY_STRING = "any";

		private bool any;

		private bool byPercent;

		private string buildingId;

		private int level;

		protected int buildingsToDestroy;

		protected int buildingsDestroyed;

		private ConditionMatchType matchType;

		public RetainBuildingCondition(ConditionVO vo, IConditionParent parent, ConditionMatchType matchType) : base(vo, parent)
		{
			this.matchType = matchType;
			this.buildingId = this.prepareArgs[1];
			this.any = (this.buildingId == "any");
			if (matchType == ConditionMatchType.Uid)
			{
				this.level = Service.Get<IDataController>().Get<BuildingTypeVO>(this.buildingId).Lvl;
			}
			else if (!this.any && this.prepareArgs.Length > 2)
			{
				this.level = Convert.ToInt32(this.prepareArgs[2], CultureInfo.InvariantCulture);
			}
			else
			{
				this.level = 0;
			}
			this.buildingsDestroyed = 0;
			this.buildingsToDestroy = 0;
			EntityController entityController = Service.Get<EntityController>();
			NodeList<BuildingNode> nodeList = entityController.GetNodeList<BuildingNode>();
			BuildingNode buildingNode = nodeList.Head;
			int num = 0;
			while (buildingNode != null)
			{
				if (this.IsBuildingValid(buildingNode.BuildingComp))
				{
					num++;
				}
				buildingNode = buildingNode.Next;
			}
			if (!this.prepareArgs[0].Contains("%"))
			{
				this.buildingsToDestroy = num - Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
				return;
			}
			this.byPercent = true;
			string text = this.prepareArgs[0];
			text = text.Substring(0, text.get_Length() - 1);
			int percent = 100 - Convert.ToInt32(text, CultureInfo.InvariantCulture);
			if (this.any && this.byPercent)
			{
				this.buildingsToDestroy = percent;
				return;
			}
			this.buildingsToDestroy = IntMath.GetPercent(percent, num);
		}

		public override void Start()
		{
			if (this.any && this.byPercent)
			{
				this.events.RegisterObserver(this, EventId.DamagePercentUpdated, EventPriority.Default);
				return;
			}
			this.events.RegisterObserver(this, EventId.EntityKilled, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.EntityKilled)
			{
				if (id == EventId.DamagePercentUpdated)
				{
					this.buildingsDestroyed = (int)cookie;
					this.EvaluateAmount();
				}
			}
			else
			{
				Entity entity = (Entity)cookie;
				BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
				if (buildingComponent != null && this.IsBuildingValid(buildingComponent))
				{
					this.buildingsDestroyed++;
					this.EvaluateAmount();
				}
			}
			return EatResponse.NotEaten;
		}

		private bool IsBuildingValid(BuildingComponent component)
		{
			BuildingTypeVO buildingType = component.BuildingType;
			if (this.any && GameUtils.IsBuildingTypeValidForBattleConditions(component.BuildingType.Type))
			{
				return true;
			}
			if (buildingType.Lvl >= this.level)
			{
				switch (this.matchType)
				{
				case ConditionMatchType.Uid:
					return buildingType.Uid == this.buildingId;
				case ConditionMatchType.Id:
					return buildingType.UpgradeGroup == this.buildingId;
				case ConditionMatchType.Type:
					return buildingType.Type == StringUtils.ParseEnum<BuildingType>(this.buildingId);
				}
			}
			return false;
		}

		protected virtual void EvaluateAmount()
		{
			if (this.buildingsDestroyed > this.buildingsToDestroy)
			{
				this.parent.ChildFailed(this);
			}
		}

		public override void Destroy()
		{
			this.events.UnregisterObserver(this, EventId.EntityKilled);
			this.events.UnregisterObserver(this, EventId.DamagePercentUpdated);
		}

		public override bool IsConditionSatisfied()
		{
			return this.buildingsDestroyed <= this.buildingsToDestroy;
		}

		public override void GetProgress(out int current, out int total)
		{
			current = this.buildingsDestroyed;
			total = this.buildingsToDestroy;
		}

		protected internal RetainBuildingCondition(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((RetainBuildingCondition)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((RetainBuildingCondition)GCHandledObjects.GCHandleToObject(instance)).EvaluateAmount();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RetainBuildingCondition)GCHandledObjects.GCHandleToObject(instance)).IsBuildingValid((BuildingComponent)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RetainBuildingCondition)GCHandledObjects.GCHandleToObject(instance)).IsConditionSatisfied());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RetainBuildingCondition)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((RetainBuildingCondition)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
