using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models;
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
	public class OwnBuildingCondition : AbstractCondition, IEventObserver
	{
		private const string ANY_STRING = "any";

		private const int COUNT_ARG = 0;

		private const int BUILDING_ID_ARG = 1;

		private const int MIN_LEVEL_ARG = 2;

		private int threshold;

		private string buildingId;

		private int level;

		private int count;

		private bool any;

		private ConditionMatchType matchType;

		private bool observingEvents;

		public OwnBuildingCondition(ConditionVO vo, IConditionParent parent, ConditionMatchType matchType) : base(vo, parent)
		{
			this.threshold = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
			this.buildingId = this.prepareArgs[1];
			this.matchType = matchType;
			this.any = (this.buildingId == "any");
			if (matchType == ConditionMatchType.Uid)
			{
				this.level = Service.Get<IDataController>().Get<BuildingTypeVO>(this.buildingId).Lvl;
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
			if (this.IsConditionSatisfied())
			{
				this.parent.ChildSatisfied(this);
				return;
			}
			this.events.RegisterObserver(this, EventId.BuildingConstructed, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.BuildingLevelUpgraded, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.BuildingSwapped, EventPriority.Default);
			this.observingEvents = true;
		}

		public override void Destroy()
		{
			if (this.observingEvents)
			{
				this.events.UnregisterObserver(this, EventId.BuildingConstructed);
				this.events.UnregisterObserver(this, EventId.BuildingLevelUpgraded);
				this.events.UnregisterObserver(this, EventId.BuildingSwapped);
			}
		}

		public override void GetProgress(out int current, out int total)
		{
			current = 0;
			total = this.threshold;
			EntityController entityController = Service.Get<EntityController>();
			IDataController dataController = Service.Get<IDataController>();
			NodeList<BuildingNode> nodeList = entityController.GetNodeList<BuildingNode>();
			for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
			{
				if (!ContractUtils.IsBuildingConstructing(buildingNode.Entity))
				{
					BuildingTypeVO vo = dataController.Get<BuildingTypeVO>(buildingNode.BuildingComp.BuildingTO.Uid);
					if (this.IsBuildingValid(vo))
					{
						current++;
					}
				}
			}
		}

		public override bool IsConditionSatisfied()
		{
			int num;
			int num2;
			this.GetProgress(out num, out num2);
			return num >= num2;
		}

		private bool IsBuildingValid(BuildingTypeVO vo)
		{
			if (vo.Type == BuildingType.Clearable)
			{
				return false;
			}
			if (this.any)
			{
				return true;
			}
			if (vo.Lvl >= this.level)
			{
				switch (this.matchType)
				{
				case ConditionMatchType.Uid:
					return vo.Uid == this.buildingId;
				case ConditionMatchType.Id:
					return vo.UpgradeGroup == this.buildingId;
				case ConditionMatchType.Type:
					return vo.Type == StringUtils.ParseEnum<BuildingType>(this.buildingId);
				}
			}
			return false;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			ContractEventData contractEventData = cookie as ContractEventData;
			BuildingTypeVO vo = Service.Get<IDataController>().Get<BuildingTypeVO>(contractEventData.Contract.ProductUid);
			if (this.IsBuildingValid(vo) && this.IsConditionSatisfied())
			{
				this.parent.ChildSatisfied(this);
			}
			return EatResponse.NotEaten;
		}

		protected internal OwnBuildingCondition(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((OwnBuildingCondition)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OwnBuildingCondition)GCHandledObjects.GCHandleToObject(instance)).IsBuildingValid((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OwnBuildingCondition)GCHandledObjects.GCHandleToObject(instance)).IsConditionSatisfied());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OwnBuildingCondition)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((OwnBuildingCondition)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
