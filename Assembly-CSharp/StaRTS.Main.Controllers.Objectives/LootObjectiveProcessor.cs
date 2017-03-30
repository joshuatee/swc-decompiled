using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Objectives
{
	public class LootObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		private const int CREDITS = 0;

		private const int MATERIAL = 1;

		private const int CONTRABAND = 2;

		private int itemType;

		public LootObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			string item = vo.Item;
			switch (item)
			{
			case "credits":
				this.itemType = 0;
				break;
			case "materials":
				this.itemType = 1;
				break;
			case "contraband":
				this.itemType = 2;
				break;
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.BattleEndProcessing);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.BattleEndProcessing)
			{
				if (base.IsEventValidForBattleObjective())
				{
					CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
					int num = 0;
					if (this.itemType == 0)
					{
						num = currentBattle.LootCreditsEarned;
					}
					else if (this.itemType == 1)
					{
						num = currentBattle.LootMaterialsEarned;
					}
					else if (this.itemType == 2)
					{
						num = currentBattle.LootContrabandEarned;
					}
					if (num > 0)
					{
						this.parent.Progress(this, num);
					}
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.BattleEndProcessing);
			base.Destroy();
		}
	}
}
