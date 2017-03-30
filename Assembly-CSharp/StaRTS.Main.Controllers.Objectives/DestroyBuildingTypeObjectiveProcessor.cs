using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Objectives
{
	public class DestroyBuildingTypeObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		private BuildingType buildingType;

		public DestroyBuildingTypeObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			this.buildingType = StringUtils.ParseEnum<BuildingType>(vo.Item);
			Service.Get<EventManager>().RegisterObserver(this, EventId.EntityKilled);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.EntityKilled)
			{
				if (base.IsEventValidForBattleObjective())
				{
					Entity entity = (Entity)cookie;
					BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
					if (buildingComponent != null && buildingComponent.BuildingType.Type == this.buildingType)
					{
						this.parent.Progress(this, 1);
					}
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.EntityKilled);
			base.Destroy();
		}
	}
}
