using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers.Objectives
{
	public class DonateTroopIdObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		private string troopId;

		public DonateTroopIdObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			this.troopId = vo.Item;
			Service.Get<EventManager>().RegisterObserver(this, EventId.SquadTroopsDonatedByCurrentPlayer);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.SquadTroopsDonatedByCurrentPlayer)
			{
				Dictionary<string, int> dictionary = (Dictionary<string, int>)cookie;
				IDataController dataController = Service.Get<IDataController>();
				foreach (KeyValuePair<string, int> current in dictionary)
				{
					TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(current.Key);
					if (troopTypeVO.TroopID == this.troopId)
					{
						this.parent.Progress(this, current.Value * troopTypeVO.Size);
					}
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.SquadTroopsDonatedByCurrentPlayer);
			base.Destroy();
		}
	}
}
