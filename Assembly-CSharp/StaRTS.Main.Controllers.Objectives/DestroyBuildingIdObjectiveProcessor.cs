using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Objectives
{
	public class DestroyBuildingIdObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		private string buildingID;

		public DestroyBuildingIdObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			this.buildingID = vo.Item;
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
					if (buildingComponent != null && buildingComponent.BuildingType.BuildingID == this.buildingID)
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
