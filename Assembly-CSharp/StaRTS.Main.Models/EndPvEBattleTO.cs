using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Battle.Replay;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class EndPvEBattleTO
	{
		public CurrentBattle Battle
		{
			get;
			set;
		}

		public Dictionary<string, int> SeededTroopsDeployed
		{
			get;
			set;
		}

		public Dictionary<string, int> DefendingUnitsKilled
		{
			get;
			set;
		}

		public Dictionary<string, int> AttackingUnitsKilled
		{
			get;
			set;
		}

		public Dictionary<string, int> DefenderGuildUnitsSpent
		{
			get;
			set;
		}

		public Dictionary<string, int> AttackerGuildUnitsSpent
		{
			get;
			set;
		}

		public Dictionary<string, int> LootEarned
		{
			get;
			set;
		}

		public Dictionary<string, int> BuildingHealthMap
		{
			get;
			set;
		}

		public Dictionary<string, string> BuildingUids
		{
			get;
			set;
		}

		public List<string> UnarmedTraps
		{
			get;
			set;
		}

		public BattleRecord ReplayData
		{
			get;
			set;
		}

		public EndPvEBattleTO()
		{
		}

		protected internal EndPvEBattleTO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).AttackerGuildUnitsSpent);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).AttackingUnitsKilled);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).Battle);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).BuildingHealthMap);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).BuildingUids);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).DefenderGuildUnitsSpent);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).DefendingUnitsKilled);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).LootEarned);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).ReplayData);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).SeededTroopsDeployed);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).UnarmedTraps);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).AttackerGuildUnitsSpent = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).AttackingUnitsKilled = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).Battle = (CurrentBattle)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).BuildingHealthMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).BuildingUids = (Dictionary<string, string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).DefenderGuildUnitsSpent = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).DefendingUnitsKilled = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).LootEarned = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).ReplayData = (BattleRecord)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).SeededTroopsDeployed = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((EndPvEBattleTO)GCHandledObjects.GCHandleToObject(instance)).UnarmedTraps = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
