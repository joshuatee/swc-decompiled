using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Battle.Replay;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers.SquadWar
{
	public class SquadWarBuffManager : IEventObserver
	{
		private SquadController controller;

		public SquadWarBuffManager(SquadController controller)
		{
			this.controller = controller;
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.BattleLoadStart, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BattleReplaySetup, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BattleEndFullyProcessed, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BattleLeftBeforeStarting, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			switch (id)
			{
			case EventId.BattleReplaySetup:
			{
				BattleRecord battleRecord = (BattleRecord)cookie;
				SquadWarBuffManager.AddWarBuffs(battleRecord.BattleType, battleRecord.AttackerWarBuffs, battleRecord.DefenderWarBuffs);
				return EatResponse.NotEaten;
			}
			case EventId.BattleRecordRetrieved:
				IL_1A:
				if (id == EventId.BattleLoadStart)
				{
					if (this.controller.WarManager.CurrentSquadWar != null)
					{
						CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
						SquadWarBuffManager.AddWarBuffs(currentBattle.Type, currentBattle.AttackerWarBuffs, currentBattle.DefenderWarBuffs);
					}
					return EatResponse.NotEaten;
				}
				if (id != EventId.BattleEndFullyProcessed)
				{
					return EatResponse.NotEaten;
				}
				goto IL_94;
			case EventId.BattleLeftBeforeStarting:
				goto IL_94;
			}
			goto IL_1A;
			IL_94:
			Service.Get<BuffController>().ClearWarBuffs();
			return EatResponse.NotEaten;
		}

		public static void AddWarBuffs(BattleType battleType, List<string> attackerBuffs, List<string> defenderBuffs)
		{
			BuffController buffController = Service.Get<BuffController>();
			buffController.ClearWarBuffs();
			if (battleType != BattleType.PvpAttackSquadWar && battleType != BattleType.PveBuffBase)
			{
				return;
			}
			IDataController dataController = Service.Get<IDataController>();
			if (attackerBuffs != null)
			{
				int i = 0;
				int count = attackerBuffs.Count;
				while (i < count)
				{
					WarBuffVO warBuff = dataController.Get<WarBuffVO>(attackerBuffs[i]);
					buffController.AddAttackerWarBuff(warBuff);
					i++;
				}
			}
			if (battleType != BattleType.PveBuffBase && defenderBuffs != null)
			{
				int j = 0;
				int count2 = defenderBuffs.Count;
				while (j < count2)
				{
					WarBuffVO warBuff2 = dataController.Get<WarBuffVO>(defenderBuffs[j]);
					buffController.AddDefenderWarBuff(warBuff2);
					j++;
				}
			}
		}
	}
}
