using StaRTS.Main.Models.Player.Misc;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle
{
	public class CurrentBattle : BattleEntry
	{
		public uint TimePassed
		{
			get;
			set;
		}

		public int InitialHealth
		{
			get;
			set;
		}

		public int CurrentHealth
		{
			get;
			set;
		}

		public bool Canceled
		{
			get;
			set;
		}

		public BattleDeploymentData AttackerDeployableData
		{
			get;
			set;
		}

		public BattleDeploymentData DefenderDeployableData
		{
			get;
			set;
		}

		public BattleDeploymentData SeededPlayerDeployableData
		{
			get;
			set;
		}

		public Dictionary<string, int> BuildingLootCreditsMap
		{
			get;
			set;
		}

		public Dictionary<string, int> BuildingLootMaterialsMap
		{
			get;
			set;
		}

		public Dictionary<string, int> BuildingLootContrabandMap
		{
			get;
			set;
		}

		public int LootCreditsDiscarded
		{
			get;
			set;
		}

		public int LootMaterialsDiscarded
		{
			get;
			set;
		}

		public int LootContrabandDiscarded
		{
			get;
			set;
		}

		public string BattleMusic
		{
			get;
			set;
		}

		public string AmbientMusic
		{
			get;
			set;
		}

		public Dictionary<string, string> DeadBuildingKeys
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

		public string BattleUid
		{
			get;
			set;
		}

		public BattleType Type
		{
			get;
			set;
		}

		public int TimeLeft
		{
			get;
			set;
		}

		public bool MultipleHeroDeploysAllowed
		{
			get;
			set;
		}

		public bool IsReplay
		{
			get;
			set;
		}

		public int DefenderBaseScore
		{
			get;
			set;
		}

		public bool PlayerDeployedGuildTroops
		{
			get;
			set;
		}

		public Dictionary<string, int> DefenderGuildTroopsAvailable
		{
			get;
			set;
		}

		public Dictionary<string, int> DefenderGuildTroopsDestroyed
		{
			get;
			set;
		}

		public Dictionary<string, int> AttackerGuildTroopsAvailable
		{
			get;
			set;
		}

		public Dictionary<string, int> AttackerGuildTroopsDeployed
		{
			get;
			set;
		}

		public Dictionary<string, int> DefenderChampionsAvailable
		{
			get;
			set;
		}

		public List<string> DisabledBuildings
		{
			get;
			set;
		}

		public List<string> UnarmedTraps
		{
			get;
			set;
		}

		public string PvpMissionUid
		{
			get;
			set;
		}

		public CurrentBattle()
		{
		}

		protected internal CurrentBattle(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).AmbientMusic);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).AttackerDeployableData);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).AttackerGuildTroopsAvailable);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).AttackerGuildTroopsDeployed);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).AttackingUnitsKilled);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).BattleMusic);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).BattleUid);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).BuildingLootContrabandMap);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).BuildingLootCreditsMap);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).BuildingLootMaterialsMap);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).Canceled);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).CurrentHealth);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DeadBuildingKeys);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DefenderBaseScore);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DefenderChampionsAvailable);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DefenderDeployableData);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DefenderGuildTroopsAvailable);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DefenderGuildTroopsDestroyed);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DefendingUnitsKilled);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DisabledBuildings);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).InitialHealth);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).IsReplay);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).LootContrabandDiscarded);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).LootCreditsDiscarded);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsDiscarded);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).MultipleHeroDeploysAllowed);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).PlayerDeployedGuildTroops);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).PvpMissionUid);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).SeededPlayerDeployableData);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).TimeLeft);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).UnarmedTraps);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).AmbientMusic = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).AttackerDeployableData = (BattleDeploymentData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).AttackerGuildTroopsAvailable = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).AttackerGuildTroopsDeployed = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).AttackingUnitsKilled = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).BattleMusic = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).BattleUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).BuildingLootContrabandMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).BuildingLootCreditsMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).BuildingLootMaterialsMap = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).Canceled = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).CurrentHealth = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DeadBuildingKeys = (Dictionary<string, string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DefenderBaseScore = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DefenderChampionsAvailable = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DefenderDeployableData = (BattleDeploymentData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DefenderGuildTroopsAvailable = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DefenderGuildTroopsDestroyed = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DefendingUnitsKilled = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).DisabledBuildings = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).InitialHealth = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).IsReplay = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).LootContrabandDiscarded = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).LootCreditsDiscarded = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).LootMaterialsDiscarded = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).MultipleHeroDeploysAllowed = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).PlayerDeployedGuildTroops = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).PvpMissionUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).SeededPlayerDeployableData = (BattleDeploymentData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).TimeLeft = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).Type = (BattleType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((CurrentBattle)GCHandledObjects.GCHandleToObject(instance)).UnarmedTraps = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
