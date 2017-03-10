using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Objectives
{
	public class DeployTroopIdObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		private string troopId;

		public DeployTroopIdObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			this.troopId = vo.Item;
			Service.Get<EventManager>().RegisterObserver(this, EventId.TroopPlacedOnBoard);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.TroopPlacedOnBoard && base.IsEventValidForBattleObjective())
			{
				SmartEntity smartEntity = (SmartEntity)cookie;
				TroopComponent troopComponent = smartEntity.Get<TroopComponent>();
				TeamComponent teamComponent = smartEntity.Get<TeamComponent>();
				if (teamComponent != null && teamComponent.TeamType == TeamType.Attacker && troopComponent != null && troopComponent.TroopType.TroopID == this.troopId)
				{
					this.parent.Progress(this, 1);
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.TroopPlacedOnBoard);
			base.Destroy();
		}

		protected internal DeployTroopIdObjectiveProcessor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DeployTroopIdObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployTroopIdObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
