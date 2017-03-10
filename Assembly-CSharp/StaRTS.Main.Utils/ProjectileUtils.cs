using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Battle.Replay;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public static class ProjectileUtils
	{
		public static List<IAssetVO> GetBattleProjectileAssets(Map map, BattleTypeVO battle, BattleDeploymentData defensiveWaveData, List<string> attackerWarBuffs, List<string> defenderWarBuffs, Dictionary<string, int> defenderSquadTroops, Dictionary<string, int> defenderChampions, List<string> attackerEquipment, List<string> defenderEquipment)
		{
			IDataController dc = Service.Get<IDataController>();
			List<IAssetVO> list = new List<IAssetVO>();
			if (battle == null || !battle.OverridePlayerUnits)
			{
				Inventory inventory = Service.Get<CurrentPlayer>().Inventory;
				ProjectileUtils.AddTroopProjectileAssets(inventory.Troop, list, dc);
				ProjectileUtils.AddSpecialAttackProjectileAssets(inventory.SpecialAttack, list, dc);
				ProjectileUtils.AddTroopProjectileAssets(inventory.Hero, list, dc);
				ProjectileUtils.AddTroopProjectileAssets(inventory.Champion, list, dc);
			}
			List<SquadDonatedTroop> troops = Service.Get<SquadController>().StateManager.Troops;
			if (troops != null)
			{
				int i = 0;
				int count = troops.Count;
				while (i < count)
				{
					ProjectileUtils.AddTroopProjectileAssets(troops[i].TroopUid, list, dc);
					i++;
				}
			}
			ProjectileUtils.AddTroopProjectileAssets(defenderSquadTroops, list, dc);
			ProjectileUtils.AddTroopProjectileAssets(defenderChampions, list, dc);
			ProjectileUtils.AddBattleProjectileAssets(map, battle, defensiveWaveData, attackerWarBuffs, defenderWarBuffs, attackerEquipment, defenderEquipment, list, dc);
			return list;
		}

		public static List<IAssetVO> GetBattleRecordProjectileAssets(Map map, BattleRecord battleRecord, List<string> attackerWarBuffs, List<string> defenderWarBuffs, List<string> attackerEquipment, List<string> defenderEquipment)
		{
			IDataController dc = Service.Get<IDataController>();
			List<IAssetVO> list = new List<IAssetVO>();
			BattleDeploymentData attackerDeploymentData = battleRecord.AttackerDeploymentData;
			if (attackerDeploymentData != null)
			{
				ProjectileUtils.AddTroopProjectileAssets(attackerDeploymentData.TroopData, list, dc);
				ProjectileUtils.AddSpecialAttackProjectileAssets(attackerDeploymentData.SpecialAttackData, list, dc);
				ProjectileUtils.AddTroopProjectileAssets(attackerDeploymentData.HeroData, list, dc);
				ProjectileUtils.AddTroopProjectileAssets(attackerDeploymentData.ChampionData, list, dc);
			}
			BattleDeploymentData defenderDeploymentData = battleRecord.DefenderDeploymentData;
			if (defenderDeploymentData != null)
			{
				ProjectileUtils.AddTroopProjectileAssets(defenderDeploymentData.TroopData, list, dc);
				ProjectileUtils.AddSpecialAttackProjectileAssets(defenderDeploymentData.SpecialAttackData, list, dc);
				ProjectileUtils.AddTroopProjectileAssets(defenderDeploymentData.HeroData, list, dc);
				ProjectileUtils.AddTroopProjectileAssets(defenderDeploymentData.ChampionData, list, dc);
			}
			Dictionary<string, int> attackerGuildTroops = battleRecord.AttackerGuildTroops;
			ProjectileUtils.AddTroopProjectileAssets(attackerGuildTroops, list, dc);
			Dictionary<string, int> defenderGuildTroops = battleRecord.DefenderGuildTroops;
			ProjectileUtils.AddTroopProjectileAssets(defenderGuildTroops, list, dc);
			Dictionary<string, int> defenderChampions = battleRecord.DefenderChampions;
			ProjectileUtils.AddTroopProjectileAssets(defenderChampions, list, dc);
			ProjectileUtils.AddBattleProjectileAssets(map, null, null, attackerWarBuffs, defenderWarBuffs, attackerEquipment, defenderEquipment, list, dc);
			return list;
		}

		private static void AddBattleProjectileAssets(Map map, BattleTypeVO battle, BattleDeploymentData defensiveWaveData, List<string> attackerWarBuffs, List<string> defenderWarBuffs, List<string> attackerEquipment, List<string> defenderEquipment, List<IAssetVO> assets, IDataController dc)
		{
			if (battle != null)
			{
				ProjectileUtils.AddTroopProjectileAssets(battle.TroopData, assets, dc);
				ProjectileUtils.AddSpecialAttackProjectileAssets(battle.SpecialAttackData, assets, dc);
				ProjectileUtils.AddTroopProjectileAssets(battle.HeroData, assets, dc);
				ProjectileUtils.AddTroopProjectileAssets(battle.ChampionData, assets, dc);
				if (!string.IsNullOrEmpty(battle.EncounterProfile))
				{
					EncounterProfileVO optional = dc.GetOptional<EncounterProfileVO>(battle.EncounterProfile);
					if (optional != null && !string.IsNullOrEmpty(optional.GroupString))
					{
						string[] array = optional.GroupString.Split(new char[]
						{
							'|'
						});
						int i = 0;
						int num = array.Length;
						while (i < num)
						{
							if (!string.IsNullOrEmpty(array[i]))
							{
								string[] array2 = array[i].Split(new char[]
								{
									','
								});
								if (array2.Length > 2)
								{
									ProjectileUtils.AddTroopProjectileAssets(array2[2], assets, dc);
								}
							}
							i++;
						}
					}
				}
			}
			if (defensiveWaveData != null)
			{
				ProjectileUtils.AddTroopProjectileAssets(defensiveWaveData.TroopData, assets, dc);
				ProjectileUtils.AddSpecialAttackProjectileAssets(defensiveWaveData.SpecialAttackData, assets, dc);
				ProjectileUtils.AddTroopProjectileAssets(defensiveWaveData.HeroData, assets, dc);
				ProjectileUtils.AddTroopProjectileAssets(defensiveWaveData.ChampionData, assets, dc);
				ProjectileUtils.AddTroopProjectileAssets(defensiveWaveData.SquadData, assets, dc);
			}
			ProjectileUtils.AddWarBuffAssets(attackerWarBuffs, assets, dc);
			ProjectileUtils.AddWarBuffAssets(defenderWarBuffs, assets, dc);
			ProjectileUtils.AddEquipmentAssets(attackerEquipment, assets, dc);
			ProjectileUtils.AddEquipmentAssets(defenderEquipment, assets, dc);
			ProjectileUtils.AddBuildingProjectileAssets(map, assets, dc);
		}

		private static void AddWarBuffAssets(List<string> warBuffs, List<IAssetVO> assets, IDataController dc)
		{
			if (warBuffs != null)
			{
				int i = 0;
				int count = warBuffs.Count;
				while (i < count)
				{
					WarBuffVO warBuffVO = dc.Get<WarBuffVO>(warBuffs[i]);
					ProjectileUtils.AddBuffProjectileAssets(warBuffVO.TroopBuffUid, assets, dc);
					ProjectileUtils.AddBuffProjectileAssets(warBuffVO.BuildingBuffUid, assets, dc);
					i++;
				}
			}
		}

		private static void AddEquipmentAssets(List<string> equipment, List<IAssetVO> assets, IDataController dc)
		{
			if (equipment != null)
			{
				int i = 0;
				int count = equipment.Count;
				while (i < count)
				{
					EquipmentVO equipmentVO = dc.Get<EquipmentVO>(equipment[i]);
					string[] effectUids = equipmentVO.EffectUids;
					if (effectUids != null)
					{
						int j = 0;
						int num = effectUids.Length;
						while (j < num)
						{
							EquipmentEffectVO equipmentEffectVO = dc.Get<EquipmentEffectVO>(effectUids[j]);
							string[] buffUids = equipmentEffectVO.BuffUids;
							if (buffUids != null)
							{
								int k = 0;
								int num2 = buffUids.Length;
								while (k < num2)
								{
									ProjectileUtils.AddBuffProjectileAssets(buffUids[k], assets, dc);
									k++;
								}
							}
							j++;
						}
					}
					i++;
				}
			}
		}

		private static void AddTroopProjectileAssets(InventoryStorage storage, List<IAssetVO> assets, IDataController dc)
		{
			Dictionary<string, InventoryEntry> internalStorage = storage.GetInternalStorage();
			foreach (KeyValuePair<string, InventoryEntry> current in internalStorage)
			{
				ProjectileUtils.AddTroopProjectileAssets(current.get_Key(), assets, dc);
			}
		}

		private static void AddTroopProjectileAssets(Dictionary<string, int> troops, List<IAssetVO> assets, IDataController dc)
		{
			if (troops != null)
			{
				foreach (KeyValuePair<string, int> current in troops)
				{
					ProjectileUtils.AddTroopProjectileAssets(current.get_Key(), assets, dc);
				}
			}
		}

		private static void AddSpecialAttackProjectileAssets(InventoryStorage storage, List<IAssetVO> assets, IDataController dc)
		{
			Dictionary<string, InventoryEntry> internalStorage = storage.GetInternalStorage();
			foreach (KeyValuePair<string, InventoryEntry> current in internalStorage)
			{
				ProjectileUtils.AddSpecialAttackProjectileAssets(current.get_Key(), assets, dc);
			}
		}

		private static void AddSpecialAttackProjectileAssets(Dictionary<string, int> specialAttacks, List<IAssetVO> assets, IDataController dc)
		{
			if (specialAttacks != null)
			{
				foreach (KeyValuePair<string, int> current in specialAttacks)
				{
					ProjectileUtils.AddSpecialAttackProjectileAssets(current.get_Key(), assets, dc);
				}
			}
		}

		public static void AddTroopProjectileAssets(string troopUid, List<IAssetVO> assets, IDataController dc)
		{
			TroopTypeVO troopTypeVO = dc.Get<TroopTypeVO>(troopUid);
			ProjectileTypeVO projectileType = troopTypeVO.ProjectileType;
			ActiveArmory activeArmory = Service.Get<CurrentPlayer>().ActiveArmory;
			if (activeArmory != null)
			{
				SkinTypeVO applicableSkin = Service.Get<SkinController>().GetApplicableSkin(troopTypeVO, activeArmory.Equipment);
				if (applicableSkin != null && applicableSkin.Override != null && applicableSkin.Override.ProjectileType != null)
				{
					projectileType = applicableSkin.Override.ProjectileType;
				}
			}
			ProjectileUtils.AddProjectileAssets(projectileType, assets, dc);
			ProjectileUtils.AddProjectileAssets(troopTypeVO.DeathProjectileType, assets, dc);
			ProjectileUtils.AddBuffProjectileAssets(troopTypeVO.SpawnApplyBuffs, assets, dc);
			if (!string.IsNullOrEmpty(troopTypeVO.Ability))
			{
				TroopAbilityVO troopAbilityVO = dc.Get<TroopAbilityVO>(troopTypeVO.Ability);
				ProjectileUtils.AddProjectileAssets(troopAbilityVO.ProjectileType, assets, dc);
				ProjectileUtils.AddBuffProjectileAssets(troopAbilityVO.SelfBuff, assets, dc);
			}
		}

		private static void AddSpecialAttackProjectileAssets(string specialAttackUid, List<IAssetVO> assets, IDataController dc)
		{
			SpecialAttackTypeVO specialAttackTypeVO = dc.Get<SpecialAttackTypeVO>(specialAttackUid);
			ProjectileUtils.AddProjectileAssets(specialAttackTypeVO.ProjectileType, assets, dc);
			if (!string.IsNullOrEmpty(specialAttackTypeVO.LinkedUnit))
			{
				ProjectileUtils.AddTroopProjectileAssets(specialAttackTypeVO.LinkedUnit, assets, dc);
			}
		}

		private static void AddBuildingProjectileAssets(Map map, List<IAssetVO> assets, IDataController dc)
		{
			int i = 0;
			int count = map.Buildings.Count;
			while (i < count)
			{
				BuildingTypeVO buildingTypeVO = dc.Get<BuildingTypeVO>(map.Buildings[i].Uid);
				if (!string.IsNullOrEmpty(buildingTypeVO.TurretUid))
				{
					TurretTypeVO turretTypeVO = dc.Get<TurretTypeVO>(buildingTypeVO.TurretUid);
					ProjectileUtils.AddProjectileAssets(turretTypeVO.ProjectileType, assets, dc);
				}
				if (buildingTypeVO.TrapUid != null)
				{
					TrapTypeVO trapTypeVO = dc.Get<TrapTypeVO>(buildingTypeVO.TrapUid);
					if (trapTypeVO.TurretTED != null && !string.IsNullOrEmpty(trapTypeVO.TurretTED.TurretUid))
					{
						TurretTypeVO turretTypeVO2 = dc.Get<TurretTypeVO>(trapTypeVO.TurretTED.TurretUid);
						ProjectileUtils.AddProjectileAssets(turretTypeVO2.ProjectileType, assets, dc);
					}
					if (trapTypeVO.ShipTED != null && !string.IsNullOrEmpty(trapTypeVO.ShipTED.SpecialAttackName))
					{
						ProjectileUtils.AddSpecialAttackProjectileAssets(trapTypeVO.ShipTED.SpecialAttackName, assets, dc);
					}
				}
				i++;
			}
		}

		public static void AddProjectileAssets(ProjectileTypeVO ptVO, List<IAssetVO> assets, IDataController dc)
		{
			if (ptVO != null)
			{
				assets.Add(ptVO);
				ProjectileUtils.AddBuffProjectileAssets(ptVO.ApplyBuffs, assets, dc);
			}
		}

		private static void AddBuffProjectileAssets(string[] buffUids, List<IAssetVO> assets, IDataController dc)
		{
			if (buffUids != null)
			{
				int i = 0;
				int num = buffUids.Length;
				while (i < num)
				{
					ProjectileUtils.AddBuffProjectileAssets(buffUids[i], assets, dc);
					i++;
				}
			}
		}

		private static void AddBuffProjectileAssets(string buffUid, List<IAssetVO> assets, IDataController dc)
		{
			if (!string.IsNullOrEmpty(buffUid))
			{
				BuffTypeVO item = dc.Get<BuffTypeVO>(buffUid);
				assets.Add(item);
			}
		}

		public static List<string> GetAssetNames(IAssetVO asset)
		{
			List<string> list = new List<string>();
			if (asset is ProjectileTypeVO)
			{
				ProjectileUtils.AddProjectileAssetNames((ProjectileTypeVO)asset, list);
			}
			else if (asset is BuffTypeVO)
			{
				ProjectileUtils.AddBuffProjectileAssetNames((BuffTypeVO)asset, list);
			}
			return list;
		}

		private static void AddProjectileAssetNames(ProjectileTypeVO ptVO, List<string> assetNames)
		{
			if (!string.IsNullOrEmpty(ptVO.BulletAssetName))
			{
				assetNames.Add(ptVO.BulletAssetName);
			}
			if (!string.IsNullOrEmpty(ptVO.GroundBulletAssetName))
			{
				assetNames.Add(ptVO.GroundBulletAssetName);
			}
			if (!string.IsNullOrEmpty(ptVO.ChargeAssetName))
			{
				assetNames.Add(ptVO.ChargeAssetName);
			}
			if (!string.IsNullOrEmpty(ptVO.MuzzleFlashAssetName))
			{
				assetNames.Add(ptVO.MuzzleFlashAssetName);
			}
			if (!string.IsNullOrEmpty(ptVO.HitSparkAssetName))
			{
				assetNames.Add(ptVO.HitSparkAssetName);
			}
		}

		private static void AddBuffProjectileAssetNames(BuffTypeVO buffTypeVO, List<string> assetNames)
		{
			if (!string.IsNullOrEmpty(buffTypeVO.RebelMuzzleAssetName))
			{
				assetNames.Add(buffTypeVO.RebelMuzzleAssetName);
			}
			if (!string.IsNullOrEmpty(buffTypeVO.EmpireMuzzleAssetName))
			{
				assetNames.Add(buffTypeVO.EmpireMuzzleAssetName);
			}
			if (!string.IsNullOrEmpty(buffTypeVO.RebelImpactAssetName))
			{
				assetNames.Add(buffTypeVO.RebelImpactAssetName);
			}
			if (!string.IsNullOrEmpty(buffTypeVO.EmpireImpactAssetName))
			{
				assetNames.Add(buffTypeVO.EmpireImpactAssetName);
			}
		}

		public static bool AreAllBulletAssetsLoaded(Bullet bullet, HashSet<IAssetVO> loadedAssets)
		{
			ProjectileTypeVO projectileType = bullet.ProjectileType;
			if (projectileType != null && !loadedAssets.Contains(projectileType))
			{
				return false;
			}
			if (bullet.AppliedBuffs != null)
			{
				int i = 0;
				int count = bullet.AppliedBuffs.Count;
				while (i < count)
				{
					BuffTypeVO buffType = bullet.AppliedBuffs[i].BuffType;
					if (buffType != null && !loadedAssets.Contains(buffType))
					{
						return false;
					}
					i++;
				}
			}
			return true;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			ProjectileUtils.AddBattleProjectileAssets((Map)GCHandledObjects.GCHandleToObject(*args), (BattleTypeVO)GCHandledObjects.GCHandleToObject(args[1]), (BattleDeploymentData)GCHandledObjects.GCHandleToObject(args[2]), (List<string>)GCHandledObjects.GCHandleToObject(args[3]), (List<string>)GCHandledObjects.GCHandleToObject(args[4]), (List<string>)GCHandledObjects.GCHandleToObject(args[5]), (List<string>)GCHandledObjects.GCHandleToObject(args[6]), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[7]), (IDataController)GCHandledObjects.GCHandleToObject(args[8]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			ProjectileUtils.AddBuffProjectileAssetNames((BuffTypeVO)GCHandledObjects.GCHandleToObject(*args), (List<string>)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			ProjectileUtils.AddBuffProjectileAssets(Marshal.PtrToStringUni(*(IntPtr*)args), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1]), (IDataController)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			ProjectileUtils.AddBuffProjectileAssets((string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1]), (IDataController)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			ProjectileUtils.AddBuildingProjectileAssets((Map)GCHandledObjects.GCHandleToObject(*args), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1]), (IDataController)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			ProjectileUtils.AddEquipmentAssets((List<string>)GCHandledObjects.GCHandleToObject(*args), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1]), (IDataController)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			ProjectileUtils.AddProjectileAssetNames((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args), (List<string>)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			ProjectileUtils.AddProjectileAssets((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1]), (IDataController)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			ProjectileUtils.AddSpecialAttackProjectileAssets((InventoryStorage)GCHandledObjects.GCHandleToObject(*args), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1]), (IDataController)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			ProjectileUtils.AddSpecialAttackProjectileAssets((Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1]), (IDataController)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			ProjectileUtils.AddSpecialAttackProjectileAssets(Marshal.PtrToStringUni(*(IntPtr*)args), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1]), (IDataController)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			ProjectileUtils.AddTroopProjectileAssets(Marshal.PtrToStringUni(*(IntPtr*)args), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1]), (IDataController)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			ProjectileUtils.AddTroopProjectileAssets((InventoryStorage)GCHandledObjects.GCHandleToObject(*args), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1]), (IDataController)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			ProjectileUtils.AddTroopProjectileAssets((Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1]), (IDataController)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			ProjectileUtils.AddWarBuffAssets((List<string>)GCHandledObjects.GCHandleToObject(*args), (List<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1]), (IDataController)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileUtils.AreAllBulletAssetsLoaded((Bullet)GCHandledObjects.GCHandleToObject(*args), (HashSet<IAssetVO>)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileUtils.GetAssetNames((IAssetVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileUtils.GetBattleProjectileAssets((Map)GCHandledObjects.GCHandleToObject(*args), (BattleTypeVO)GCHandledObjects.GCHandleToObject(args[1]), (BattleDeploymentData)GCHandledObjects.GCHandleToObject(args[2]), (List<string>)GCHandledObjects.GCHandleToObject(args[3]), (List<string>)GCHandledObjects.GCHandleToObject(args[4]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[5]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[6]), (List<string>)GCHandledObjects.GCHandleToObject(args[7]), (List<string>)GCHandledObjects.GCHandleToObject(args[8])));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProjectileUtils.GetBattleRecordProjectileAssets((Map)GCHandledObjects.GCHandleToObject(*args), (BattleRecord)GCHandledObjects.GCHandleToObject(args[1]), (List<string>)GCHandledObjects.GCHandleToObject(args[2]), (List<string>)GCHandledObjects.GCHandleToObject(args[3]), (List<string>)GCHandledObjects.GCHandleToObject(args[4]), (List<string>)GCHandledObjects.GCHandleToObject(args[5])));
		}
	}
}
