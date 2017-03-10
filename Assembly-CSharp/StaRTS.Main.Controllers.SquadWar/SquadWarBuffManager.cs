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
using WinRTBridge;

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
			if (id <= EventId.BattleEndFullyProcessed)
			{
				if (id != EventId.BattleLoadStart)
				{
					if (id != EventId.BattleEndFullyProcessed)
					{
						return EatResponse.NotEaten;
					}
				}
				else
				{
					if (this.controller.WarManager.CurrentSquadWar != null)
					{
						CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
						SquadWarBuffManager.AddWarBuffs(currentBattle.Type, currentBattle.AttackerWarBuffs, currentBattle.DefenderWarBuffs);
						return EatResponse.NotEaten;
					}
					return EatResponse.NotEaten;
				}
			}
			else
			{
				if (id == EventId.BattleReplaySetup)
				{
					BattleRecord battleRecord = (BattleRecord)cookie;
					SquadWarBuffManager.AddWarBuffs(battleRecord.BattleType, battleRecord.AttackerWarBuffs, battleRecord.DefenderWarBuffs);
					return EatResponse.NotEaten;
				}
				if (id != EventId.BattleLeftBeforeStarting)
				{
					return EatResponse.NotEaten;
				}
			}
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

		protected internal SquadWarBuffManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			SquadWarBuffManager.AddWarBuffs((BattleType)(*(int*)args), (List<string>)GCHandledObjects.GCHandleToObject(args[1]), (List<string>)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarBuffManager)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
