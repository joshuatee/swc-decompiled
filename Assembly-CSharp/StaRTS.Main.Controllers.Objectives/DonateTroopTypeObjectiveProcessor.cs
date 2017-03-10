using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Objectives
{
	public class DonateTroopTypeObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		private TroopType troopType;

		public DonateTroopTypeObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			this.troopType = StringUtils.ParseEnum<TroopType>(vo.Item);
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
					TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(current.get_Key());
					if (troopTypeVO.Type == this.troopType)
					{
						this.parent.Progress(this, current.get_Value() * troopTypeVO.Size);
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

		protected internal DonateTroopTypeObjectiveProcessor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DonateTroopTypeObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DonateTroopTypeObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
