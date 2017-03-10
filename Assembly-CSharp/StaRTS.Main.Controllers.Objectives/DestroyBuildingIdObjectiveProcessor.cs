using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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
			if (id == EventId.EntityKilled && base.IsEventValidForBattleObjective())
			{
				Entity entity = (Entity)cookie;
				BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
				if (buildingComponent != null && buildingComponent.BuildingType.BuildingID == this.buildingID)
				{
					this.parent.Progress(this, 1);
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.EntityKilled);
			base.Destroy();
		}

		protected internal DestroyBuildingIdObjectiveProcessor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DestroyBuildingIdObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DestroyBuildingIdObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
