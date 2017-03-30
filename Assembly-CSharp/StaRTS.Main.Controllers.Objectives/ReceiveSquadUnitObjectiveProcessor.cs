using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers.Objectives
{
	public class ReceiveSquadUnitObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		public ReceiveSquadUnitObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.SquadTroopsReceived);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.SquadTroopsReceived)
			{
				KeyValuePair<string, int> keyValuePair = (KeyValuePair<string, int>)cookie;
				string key = keyValuePair.Key;
				int value = keyValuePair.Value;
				base.CheckUnusedPveFlag();
				IDataController dataController = Service.Get<IDataController>();
				TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(key);
				this.parent.Progress(this, troopTypeVO.Size * value);
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.SquadTroopsReceived);
			base.Destroy();
		}
	}
}
