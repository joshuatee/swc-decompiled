using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public static class ArmoryUtils
	{
		public static List<string> GetValidEquipment(CurrentPlayer player, IDataController dc, string planetId)
		{
			List<string> list = null;
			List<string> equipment = player.ActiveArmory.Equipment;
			if (equipment != null)
			{
				int i = 0;
				int count = equipment.Count;
				while (i < count)
				{
					EquipmentVO equipmentVO = dc.Get<EquipmentVO>(equipment[i]);
					if (ArmoryUtils.IsEquipmentValidForPlanet(equipmentVO, planetId))
					{
						if (list == null)
						{
							list = new List<string>();
						}
						list.Add(equipmentVO.Uid);
					}
					i++;
				}
			}
			return list;
		}

		public static bool IsEquipmentValidForPlanet(EquipmentVO equipment, string planetId)
		{
			string[] planetIDs = equipment.PlanetIDs;
			int i = 0;
			int num = planetIDs.Length;
			while (i < num)
			{
				if (planetId == planetIDs[i])
				{
					return true;
				}
				i++;
			}
			return false;
		}

		public static EquipmentVO GetCurrentEquipmentDataByID(string equipmentId)
		{
			int level = Service.Get<CurrentPlayer>().UnlockedLevels.Equipment.GetLevel(equipmentId);
			return ArmoryUtils.GetEquipmentDataByID(equipmentId, level);
		}

		public static EquipmentVO GetEquipmentDataByID(string equipmentID, int level)
		{
			EquipmentUpgradeCatalog equipmentUpgradeCatalog = Service.Get<EquipmentUpgradeCatalog>();
			return equipmentUpgradeCatalog.GetByLevel(equipmentID, level);
		}

		public static int GetCurrentActiveEquipmentCapacity(ActiveArmory playerArmory)
		{
			IDataController dataController = Service.Get<IDataController>();
			int num = 0;
			int i = 0;
			int count = playerArmory.Equipment.Count;
			while (i < count)
			{
				EquipmentVO equipmentVO = dataController.Get<EquipmentVO>(playerArmory.Equipment[i]);
				num += equipmentVO.Size;
				i++;
			}
			return num;
		}

		public static bool HasEnoughCapacityToActivateEquipment(ActiveArmory armory, EquipmentVO equipment)
		{
			return ArmoryUtils.GetCurrentActiveEquipmentCapacity(armory) + equipment.Size <= armory.MaxCapacity;
		}

		public static bool IsEquipmentOnValidPlanet(CurrentPlayer player, EquipmentVO equipment)
		{
			if (equipment.PlanetIDs == null)
			{
				StringBuilder stringBuilder = new StringBuilder("CMS ERROR: ");
				stringBuilder.AppendFormat("{0} has no valid planets", new object[]
				{
					equipment.Uid
				});
				Service.Get<StaRTSLogger>().Error(stringBuilder.ToString());
				return false;
			}
			int i = 0;
			int num = equipment.PlanetIDs.Length;
			while (i < num)
			{
				if (player.PlanetId == equipment.PlanetIDs[i])
				{
					return true;
				}
				i++;
			}
			return false;
		}

		public static bool PlayerHasArmory()
		{
			NodeList<ArmoryNode> armoryNodeList = Service.Get<BuildingLookupController>().ArmoryNodeList;
			return armoryNodeList.CalculateCount() > 0;
		}

		public static bool IsBuildingRequirementMet(EquipmentVO equipment)
		{
			UnlockController unlockController = Service.Get<UnlockController>();
			return unlockController.IsUpgradeableUnlocked(equipment);
		}

		public static bool IsEquipmentActive(CurrentPlayer currentPlayer, EquipmentVO equipment)
		{
			return currentPlayer.ActiveArmory.Equipment.Contains(equipment.Uid);
		}

		public static bool CanAffordEquipment(CurrentPlayer currentPlayer, EquipmentVO equipment)
		{
			string equipmentID = equipment.EquipmentID;
			Dictionary<string, int> shards = currentPlayer.Shards;
			int num = shards.ContainsKey(equipmentID) ? shards[equipmentID] : 0;
			return num >= equipment.UpgradeShards;
		}

		public static bool IsAnyEquipmentActive(ActiveArmory armory)
		{
			return ArmoryUtils.GetCurrentActiveEquipmentCapacity(armory) > 0;
		}

		public static bool IsEquipmentOwned(CurrentPlayer currentPlayer, EquipmentVO equipmentVO)
		{
			return currentPlayer.UnlockedLevels.Equipment.Has(equipmentVO);
		}

		public static bool HasReachedMaxEquipmentShards(CurrentPlayer player, EquipmentUpgradeCatalog catalog, string equipmentID)
		{
			int level = player.UnlockedLevels.Equipment.GetLevel(equipmentID);
			EquipmentVO maxLevel = catalog.GetMaxLevel(equipmentID);
			int num = (maxLevel != null) ? maxLevel.Lvl : 0;
			if (level >= num)
			{
				return true;
			}
			int num2 = 0;
			for (int i = level + 1; i <= num; i++)
			{
				EquipmentVO byLevel = catalog.GetByLevel(equipmentID, i);
				num2 += byLevel.UpgradeShards;
			}
			return player.Shards.ContainsKey(equipmentID) && player.Shards[equipmentID] >= num2;
		}

		public static bool IsArmoryFull(CurrentPlayer currentPlayer)
		{
			int currentActiveEquipmentCapacity = ArmoryUtils.GetCurrentActiveEquipmentCapacity(currentPlayer.ActiveArmory);
			int maxCapacity = currentPlayer.ActiveArmory.MaxCapacity;
			return currentActiveEquipmentCapacity >= maxCapacity;
		}

		public static bool IsArmoryEmpty(CurrentPlayer currentPlayer)
		{
			int currentActiveEquipmentCapacity = ArmoryUtils.GetCurrentActiveEquipmentCapacity(currentPlayer.ActiveArmory);
			return currentActiveEquipmentCapacity == 0;
		}

		public static int GetShardsRequiredForNextUpgrade(CurrentPlayer currentPlayer, EquipmentUpgradeCatalog equipmentCatalog, EquipmentVO equipmentVO)
		{
			if (!ArmoryUtils.IsEquipmentOwned(currentPlayer, equipmentVO))
			{
				EquipmentVO minLevel = equipmentCatalog.GetMinLevel(equipmentVO.EquipmentID);
				return minLevel.UpgradeShards;
			}
			EquipmentVO nextLevel = equipmentCatalog.GetNextLevel(equipmentVO);
			if (nextLevel == null)
			{
				return -1;
			}
			return nextLevel.UpgradeShards;
		}

		public static bool IsAtMaxLevel(EquipmentUpgradeCatalog equipmentUpgradeCatalog, EquipmentVO equipmentVO)
		{
			EquipmentVO maxLevel = equipmentUpgradeCatalog.GetMaxLevel(equipmentVO.EquipmentID);
			return maxLevel.Lvl == equipmentVO.Lvl;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.CanAffordEquipment((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (EquipmentVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.GetCurrentActiveEquipmentCapacity((ActiveArmory)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.GetCurrentEquipmentDataByID(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.GetEquipmentDataByID(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.GetShardsRequiredForNextUpgrade((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (EquipmentUpgradeCatalog)GCHandledObjects.GCHandleToObject(args[1]), (EquipmentVO)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.GetValidEquipment((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (IDataController)GCHandledObjects.GCHandleToObject(args[1]), Marshal.PtrToStringUni(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.HasEnoughCapacityToActivateEquipment((ActiveArmory)GCHandledObjects.GCHandleToObject(*args), (EquipmentVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.HasReachedMaxEquipmentShards((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (EquipmentUpgradeCatalog)GCHandledObjects.GCHandleToObject(args[1]), Marshal.PtrToStringUni(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.IsAnyEquipmentActive((ActiveArmory)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.IsArmoryEmpty((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.IsArmoryFull((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.IsAtMaxLevel((EquipmentUpgradeCatalog)GCHandledObjects.GCHandleToObject(*args), (EquipmentVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.IsBuildingRequirementMet((EquipmentVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.IsEquipmentActive((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (EquipmentVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.IsEquipmentOnValidPlanet((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (EquipmentVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.IsEquipmentOwned((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (EquipmentVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.IsEquipmentValidForPlanet((EquipmentVO)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ArmoryUtils.PlayerHasArmory());
		}
	}
}
