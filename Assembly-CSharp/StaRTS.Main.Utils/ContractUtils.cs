using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public static class ContractUtils
	{
		public static DeliveryType GetDeliveryTypeForBuildingContract(string buildingContractUid, BuildingTypeVO building)
		{
			if (building.Type == BuildingType.Clearable)
			{
				return DeliveryType.ClearClearable;
			}
			if (building.Type == BuildingType.TroopResearch)
			{
				TroopTypeVO optional = Service.Get<IDataController>().GetOptional<TroopTypeVO>(buildingContractUid);
				if (optional != null)
				{
					return DeliveryType.UpgradeTroop;
				}
				SpecialAttackTypeVO optional2 = Service.Get<IDataController>().GetOptional<SpecialAttackTypeVO>(buildingContractUid);
				if (optional2 != null)
				{
					return DeliveryType.UpgradeStarship;
				}
				EquipmentVO optional3 = Service.Get<IDataController>().GetOptional<EquipmentVO>(buildingContractUid);
				if (optional3 != null)
				{
					return DeliveryType.UpgradeEquipment;
				}
			}
			BuildingTypeVO optional4 = Service.Get<IDataController>().GetOptional<BuildingTypeVO>(buildingContractUid);
			if (optional4 == null || optional4.Type != building.Type)
			{
				return DeliveryType.Invalid;
			}
			if (optional4.BuildingID != building.BuildingID)
			{
				return DeliveryType.SwapBuilding;
			}
			if (optional4.Lvl == 1)
			{
				return DeliveryType.Building;
			}
			return DeliveryType.UpgradeBuilding;
		}

		public static DeliveryType GetDeliveryTypeForTroopContract(BuildingTypeVO building)
		{
			BuildingType type = building.Type;
			switch (type)
			{
			case BuildingType.Barracks:
				return DeliveryType.Infantry;
			case BuildingType.Factory:
				return DeliveryType.Vehicle;
			case BuildingType.FleetCommand:
				return DeliveryType.Starship;
			case BuildingType.HeroMobilizer:
				return DeliveryType.Hero;
			case BuildingType.ChampionPlatform:
				return DeliveryType.Champion;
			default:
				if (type != BuildingType.Cantina)
				{
					return DeliveryType.Invalid;
				}
				return DeliveryType.Mercenary;
			}
		}

		public static int GetBuildingContractTotalTime(string buildingContractUid, DeliveryType type)
		{
			if (type == DeliveryType.UpgradeTroop)
			{
				TroopTypeVO optional = Service.Get<IDataController>().GetOptional<TroopTypeVO>(buildingContractUid);
				return optional.UpgradeTime;
			}
			if (type == DeliveryType.UpgradeStarship)
			{
				SpecialAttackTypeVO optional2 = Service.Get<IDataController>().GetOptional<SpecialAttackTypeVO>(buildingContractUid);
				return optional2.UpgradeTime;
			}
			if (type == DeliveryType.UpgradeEquipment)
			{
				EquipmentVO optional3 = Service.Get<IDataController>().GetOptional<EquipmentVO>(buildingContractUid);
				return optional3.UpgradeTime;
			}
			BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(buildingContractUid);
			switch (type)
			{
			case DeliveryType.Building:
			case DeliveryType.UpgradeBuilding:
			case DeliveryType.ClearClearable:
				return buildingTypeVO.Time;
			case DeliveryType.SwapBuilding:
				return buildingTypeVO.SwapTime;
			}
			return 0;
		}

		public static int GetTroopContractTotalTime(string productUid, DeliveryType type, List<string> perkIds)
		{
			int num = 0;
			IDataController dataController = Service.Get<IDataController>();
			IUpgradeableVO upgradeableVO = null;
			switch (type)
			{
			case DeliveryType.Infantry:
			case DeliveryType.Vehicle:
			case DeliveryType.Hero:
			case DeliveryType.Champion:
			case DeliveryType.Mercenary:
			{
				TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(productUid);
				num = troopTypeVO.TrainingTime;
				upgradeableVO = troopTypeVO;
				break;
			}
			case DeliveryType.Starship:
			case DeliveryType.UpgradeStarship:
			{
				SpecialAttackTypeVO specialAttackTypeVO = dataController.Get<SpecialAttackTypeVO>(productUid);
				if (type == DeliveryType.UpgradeStarship)
				{
					num = specialAttackTypeVO.UpgradeTime;
				}
				else
				{
					num = specialAttackTypeVO.TrainingTime;
				}
				upgradeableVO = specialAttackTypeVO;
				break;
			}
			}
			ContractType contractType = ContractUtils.GetContractType(type);
			if (upgradeableVO != null && perkIds != null && perkIds.Count > 0 && ContractUtils.IsTroopType(contractType))
			{
				PerkManager perkManager = Service.Get<PerkManager>();
				float contractTimeReductionMultiplierForPerks = perkManager.GetContractTimeReductionMultiplierForPerks(upgradeableVO, perkIds);
				num = Mathf.FloorToInt((float)num * contractTimeReductionMultiplierForPerks);
			}
			return num;
		}

		public static DeliveryType GetTroopContractTypeByBuilding(BuildingTypeVO building)
		{
			BuildingType type = building.Type;
			switch (type)
			{
			case BuildingType.Barracks:
				return DeliveryType.Infantry;
			case BuildingType.Factory:
				return DeliveryType.Vehicle;
			case BuildingType.FleetCommand:
				return DeliveryType.Starship;
			case BuildingType.HeroMobilizer:
				return DeliveryType.Hero;
			case BuildingType.ChampionPlatform:
				return DeliveryType.Champion;
			default:
				if (type != BuildingType.Cantina)
				{
					return DeliveryType.Invalid;
				}
				return DeliveryType.Mercenary;
			}
		}

		public static ContractType GetContractType(DeliveryType deliveryType)
		{
			switch (deliveryType)
			{
			case DeliveryType.Invalid:
				return ContractType.Invalid;
			case DeliveryType.Infantry:
			case DeliveryType.Vehicle:
			case DeliveryType.Mercenary:
				return ContractType.Troop;
			case DeliveryType.Starship:
				return ContractType.SpecialAttack;
			case DeliveryType.Hero:
				return ContractType.Hero;
			case DeliveryType.Champion:
				return ContractType.Champion;
			case DeliveryType.Building:
				return ContractType.Build;
			case DeliveryType.UpgradeBuilding:
			case DeliveryType.SwapBuilding:
				return ContractType.Upgrade;
			case DeliveryType.UpgradeTroop:
			case DeliveryType.UpgradeStarship:
			case DeliveryType.UpgradeEquipment:
				return ContractType.Research;
			case DeliveryType.ClearClearable:
				return ContractType.Clear;
			default:
				Service.Get<StaRTSLogger>().ErrorFormat("Unhandled case for Contract Type mapping: {0}", new object[]
				{
					deliveryType
				});
				return ContractType.Invalid;
			}
		}

		public static bool IsTroopType(ContractType contractType)
		{
			return contractType == ContractType.Hero || contractType == ContractType.Champion || contractType == ContractType.SpecialAttack || contractType == ContractType.Troop;
		}

		public static bool IsBuildingType(ContractType contractType)
		{
			return !ContractUtils.IsTroopType(contractType);
		}

		public static bool ContractTypeConsumesDroid(ContractType contractType)
		{
			return contractType == ContractType.Champion || (contractType != ContractType.Research && ContractUtils.IsBuildingType(contractType));
		}

		public static int CalculateRemainingTimeOfAllTroopContracts(Entity entity)
		{
			int num = 0;
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			List<Contract> list = Service.Get<ISupportController>().FindAllTroopContractsForBuilding(buildingComponent.BuildingTO.Key);
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				if (i == 0)
				{
					num += list[i].GetRemainingTimeForSim();
				}
				else
				{
					num += list[i].TotalTime;
				}
				i++;
			}
			return num;
		}

		public static int CalculateNumTroopsQueued(Entity entity)
		{
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			List<Contract> list = Service.Get<ISupportController>().FindAllTroopContractsForBuilding(buildingComponent.BuildingTO.Key);
			return list.Count;
		}

		public static int CalculateSpaceOccupiedByQueuedTroops(Entity entity)
		{
			int num = 0;
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			List<Contract> list = Service.Get<ISupportController>().FindAllTroopContractsForBuilding(buildingComponent.BuildingTO.Key);
			bool flag = false;
			if (buildingComponent.BuildingType.Type == BuildingType.FleetCommand)
			{
				flag = true;
			}
			IDataController dataController = Service.Get<IDataController>();
			for (int i = 0; i < list.Count; i++)
			{
				string productUid = list[i].ProductUid;
				int size;
				if (flag)
				{
					size = dataController.Get<SpecialAttackTypeVO>(productUid).Size;
				}
				else
				{
					size = dataController.Get<TroopTypeVO>(productUid).Size;
				}
				num += size;
			}
			return num;
		}

		public static bool HasCapacityForTroop(Entity entity, int size)
		{
			int num = ContractUtils.CalculateSpaceOccupiedByQueuedTroops(entity);
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			return size <= buildingComponent.BuildingType.Storage - num;
		}

		public static void CalculateContractCost(string productUid, DeliveryType deliveryType, BuildingTypeVO buildingVO, List<string> contractPerkIds, out int credits, out int materials, out int contraband)
		{
			credits = 0;
			materials = 0;
			contraband = 0;
			switch (deliveryType)
			{
			case DeliveryType.Infantry:
			case DeliveryType.Vehicle:
			case DeliveryType.Hero:
			case DeliveryType.Champion:
			case DeliveryType.Mercenary:
			{
				TroopTypeVO troopTypeVO = Service.Get<IDataController>().Get<TroopTypeVO>(productUid);
				credits = troopTypeVO.Credits;
				materials = troopTypeVO.Materials;
				contraband = troopTypeVO.Contraband;
				goto IL_1A2;
			}
			case DeliveryType.Starship:
			{
				SpecialAttackTypeVO specialAttackTypeVO = Service.Get<IDataController>().Get<SpecialAttackTypeVO>(productUid);
				credits = specialAttackTypeVO.Credits;
				materials = specialAttackTypeVO.Materials;
				contraband = specialAttackTypeVO.Contraband;
				goto IL_1A2;
			}
			case DeliveryType.UpgradeBuilding:
			{
				BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(productUid);
				credits = buildingTypeVO.UpgradeCredits;
				materials = buildingTypeVO.UpgradeMaterials;
				contraband = buildingTypeVO.UpgradeContraband;
				goto IL_1A2;
			}
			case DeliveryType.SwapBuilding:
			{
				BuildingTypeVO buildingTypeVO2 = Service.Get<IDataController>().Get<BuildingTypeVO>(productUid);
				credits = buildingTypeVO2.SwapCredits;
				materials = buildingTypeVO2.SwapMaterials;
				contraband = buildingTypeVO2.SwapContraband;
				goto IL_1A2;
			}
			case DeliveryType.UpgradeTroop:
			{
				TroopTypeVO troopTypeVO2 = Service.Get<IDataController>().Get<TroopTypeVO>(productUid);
				credits = troopTypeVO2.UpgradeCredits;
				materials = troopTypeVO2.UpgradeMaterials;
				contraband = troopTypeVO2.UpgradeContraband;
				goto IL_1A2;
			}
			case DeliveryType.UpgradeStarship:
			{
				SpecialAttackTypeVO specialAttackTypeVO2 = Service.Get<IDataController>().Get<SpecialAttackTypeVO>(productUid);
				credits = specialAttackTypeVO2.UpgradeCredits;
				materials = specialAttackTypeVO2.UpgradeMaterials;
				contraband = specialAttackTypeVO2.UpgradeContraband;
				goto IL_1A2;
			}
			case DeliveryType.UpgradeEquipment:
				goto IL_1A2;
			case DeliveryType.ClearClearable:
			{
				BuildingTypeVO buildingTypeVO3 = Service.Get<IDataController>().Get<BuildingTypeVO>(productUid);
				credits = buildingTypeVO3.Credits;
				materials = buildingTypeVO3.Materials;
				contraband = buildingTypeVO3.Contraband;
				goto IL_1A2;
			}
			}
			Service.Get<StaRTSLogger>().Error("DeliveryType has no cost: " + deliveryType);
			IL_1A2:
			ContractType contractType = ContractUtils.GetContractType(deliveryType);
			if (ContractUtils.IsTroopType(contractType) && buildingVO != null && contractPerkIds != null && contractPerkIds.Count > 0)
			{
				PerkManager perkManager = Service.Get<PerkManager>();
				float contractCostMultiplierForPerks = perkManager.GetContractCostMultiplierForPerks(buildingVO, contractPerkIds);
				GameUtils.MultiplyCurrency(contractCostMultiplierForPerks, ref credits, ref materials, ref contraband);
			}
		}

		public static bool IsBuildingClearing(Entity selectedBuilding)
		{
			if (selectedBuilding == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsBuildingClearing: SelectedBuilding = null");
				return false;
			}
			if (selectedBuilding.Get<BuildingComponent>() == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsBuildingClearing: selectedBuilding.BuildingComponent = null");
				return false;
			}
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(buildingComponent.BuildingTO.Key);
			return contract != null && !contract.IsReadyToBeFinished() && contract.DeliveryType == DeliveryType.ClearClearable;
		}

		public static bool IsBuildingConstructing(Entity selectedBuilding)
		{
			if (selectedBuilding == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsBuildingConstructing: SelectedBuilding = null");
				return false;
			}
			if (selectedBuilding.Get<BuildingComponent>() == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsBuildingConstructing: selectedBuilding.BuildingComponent = null");
				return false;
			}
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			return ContractUtils.IsBuildingConstructing(buildingComponent.BuildingTO.Key);
		}

		public static bool IsBuildingConstructing(string buildingKey)
		{
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(buildingKey);
			return contract != null && !contract.IsReadyToBeFinished() && contract.DeliveryType == DeliveryType.Building;
		}

		public static bool IsChampionRepairing(Entity selectedBuilding)
		{
			if (selectedBuilding == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsChampionRepairing: SelectedBuilding = null");
				return false;
			}
			if (selectedBuilding.Get<BuildingComponent>() == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsChampionRepairing: selectedBuilding.BuildingComponent = null");
				return false;
			}
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			if (buildingComponent.BuildingType.Type != BuildingType.ChampionPlatform)
			{
				return false;
			}
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(buildingComponent.BuildingTO.Key);
			return contract != null && !contract.IsReadyToBeFinished() && contract.DeliveryType == DeliveryType.Champion;
		}

		public static bool IsBuildingUpgrading(Entity selectedBuilding)
		{
			if (selectedBuilding == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsBuildingUpgrading: SelectedBuilding = null");
				return false;
			}
			if (selectedBuilding.Get<BuildingComponent>() == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsBuildingUpgrading: selectedBuilding.BuildingComponent = null");
				return false;
			}
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(buildingComponent.BuildingTO.Key);
			return contract != null && !contract.IsReadyToBeFinished() && contract.DeliveryType == DeliveryType.UpgradeBuilding;
		}

		public static bool CanCancelDeployableContract(Entity selectedBuilding)
		{
			if (!ContractUtils.IsArmyUpgrading(selectedBuilding))
			{
				return false;
			}
			IDataController dataController = Service.Get<IDataController>();
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(buildingComponent.BuildingTO.Key);
			string productUid = contract.ProductUid;
			IDeployableVO optional = dataController.GetOptional<TroopTypeVO>(productUid);
			string text = null;
			if (optional != null)
			{
				text = optional.UpgradeShardUid;
			}
			else
			{
				optional = dataController.GetOptional<SpecialAttackTypeVO>(productUid);
				if (optional != null)
				{
					text = optional.UpgradeShardUid;
				}
				else
				{
					Service.Get<StaRTSLogger>().Error("CanCancelDeployableContract: Unsupported deployable type, not troop or special attack " + productUid);
				}
			}
			return string.IsNullOrEmpty(text);
		}

		public static bool IsArmyUpgrading(Entity selectedBuilding)
		{
			if (selectedBuilding == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsArmyUpgrading: SelectedBuilding = null");
				return false;
			}
			if (selectedBuilding.Get<BuildingComponent>() == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsArmyUpgrading: selectedBuilding.BuildingComponent = null");
				return false;
			}
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(buildingComponent.BuildingTO.Key);
			return contract != null && !contract.IsReadyToBeFinished() && (contract.DeliveryType == DeliveryType.UpgradeTroop || contract.DeliveryType == DeliveryType.UpgradeStarship);
		}

		public static bool IsEquipmentUpgrading(Entity selectedBuilding)
		{
			if (selectedBuilding == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsEquipmentUpgrading: SelectedBuilding = null");
				return false;
			}
			if (selectedBuilding.Get<BuildingComponent>() == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsEquipmentUpgrading: selectedBuilding.BuildingComponent = null");
				return false;
			}
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(buildingComponent.BuildingTO.Key);
			return contract != null && !contract.IsReadyToBeFinished() && contract.DeliveryType == DeliveryType.UpgradeEquipment;
		}

		public static bool IsBuildingSwapping(Entity selectedBuilding)
		{
			if (selectedBuilding == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsBuildingSwapping: SelectedBuilding = null");
				return false;
			}
			if (selectedBuilding.Get<BuildingComponent>() == null)
			{
				Service.Get<StaRTSLogger>().Error("ContractUtils.IsBuildingSwapping: selectedBuilding.BuildingComponent = null");
				return false;
			}
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(buildingComponent.BuildingTO.Key);
			return contract != null && !contract.IsReadyToBeFinished() && contract.DeliveryType == DeliveryType.SwapBuilding;
		}

		public static void GetArmyContractStorageAndSize(Contract contract, out InventoryStorage storage, out int size)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			IDataController dataController = Service.Get<IDataController>();
			switch (contract.DeliveryType)
			{
			case DeliveryType.Starship:
				storage = currentPlayer.Inventory.SpecialAttack;
				size = dataController.Get<SpecialAttackTypeVO>(contract.ProductUid).Size;
				return;
			case DeliveryType.Hero:
				storage = currentPlayer.Inventory.Hero;
				size = dataController.Get<TroopTypeVO>(contract.ProductUid).Size;
				return;
			case DeliveryType.Champion:
				storage = currentPlayer.Inventory.Champion;
				size = dataController.Get<TroopTypeVO>(contract.ProductUid).Size;
				return;
			default:
				storage = currentPlayer.Inventory.Troop;
				size = dataController.Get<TroopTypeVO>(contract.ProductUid).Size;
				return;
			}
		}

		public static bool IsArmyContractValid(Contract contract, Dictionary<string, int> inventorySizeOffsets)
		{
			InventoryStorage inventoryStorage;
			int num;
			ContractUtils.GetArmyContractStorageAndSize(contract, out inventoryStorage, out num);
			int num2 = 0;
			if (inventorySizeOffsets != null && inventorySizeOffsets.ContainsKey(inventoryStorage.Key))
			{
				num2 = inventorySizeOffsets[inventoryStorage.Key];
			}
			return inventoryStorage.GetTotalStorageCapacity() == -1 || inventoryStorage.GetTotalStorageAmount() + num + num2 <= inventoryStorage.GetTotalStorageCapacity();
		}

		public static bool IsArmyContractValid(Contract contract)
		{
			return ContractUtils.IsArmyContractValid(contract, null);
		}

		public static bool IsUpgradeTroopContractValid(Contract contract)
		{
			TroopTypeVO upgradeable = Service.Get<IDataController>().Get<TroopTypeVO>(contract.ProductUid);
			TroopUpgradeCatalog troopUpgradeCatalog = Service.Get<TroopUpgradeCatalog>();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			return troopUpgradeCatalog.CanUpgradeTo(currentPlayer.UnlockedLevels.Troops, upgradeable);
		}

		public static bool IsUpgradeStarshipContractValid(Contract contract)
		{
			SpecialAttackTypeVO upgradeable = Service.Get<IDataController>().Get<SpecialAttackTypeVO>(contract.ProductUid);
			StarshipUpgradeCatalog starshipUpgradeCatalog = Service.Get<StarshipUpgradeCatalog>();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			return starshipUpgradeCatalog.CanUpgradeTo(currentPlayer.UnlockedLevels.Starships, upgradeable);
		}

		public static bool IsUpgradeEquipmentContractValid(Contract contract)
		{
			EquipmentVO upgradeable = Service.Get<IDataController>().Get<EquipmentVO>(contract.ProductUid);
			EquipmentUpgradeCatalog equipmentUpgradeCatalog = Service.Get<EquipmentUpgradeCatalog>();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			return equipmentUpgradeCatalog.CanUpgradeTo(currentPlayer.UnlockedLevels.Equipment, upgradeable);
		}

		private static Contract FindBuildingToFinish()
		{
			Contract result = null;
			List<Contract> list = Service.Get<ISupportController>().FindAllContractsThatConsumeDroids();
			if (list.Count > 0)
			{
				result = list[0];
			}
			return result;
		}

		public static int MinimumCostToFinish()
		{
			Contract contract = ContractUtils.FindBuildingToFinish();
			return ContractUtils.GetCrystalCostToFinishContract(contract);
		}

		public static int GetCrystalCostToFinishContract(Contract contract)
		{
			int result = 0;
			if (contract != null)
			{
				int remainingTimeForSim = contract.GetRemainingTimeForSim();
				if (remainingTimeForSim > 0)
				{
					result = GameUtils.SecondsToCrystals(remainingTimeForSim);
				}
			}
			return result;
		}

		public static bool InstantFreeupDroid()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			int num = ContractUtils.CalculateDroidsInUse();
			if (num >= currentPlayer.CurrentDroidsAmount)
			{
				Contract contract = ContractUtils.FindBuildingToFinish();
				if (contract != null)
				{
					int crystalCostToFinishContract = ContractUtils.GetCrystalCostToFinishContract(contract);
					if (GameUtils.SpendCrystals(crystalCostToFinishContract))
					{
						NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
						for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
						{
							if (buildingNode.BuildingComp.BuildingTO.Key == contract.ContractTO.BuildingKey)
							{
								Service.Get<EventManager>().SendEvent(EventId.InitiatedBuyout, null);
								Service.Get<ISupportController>().BuyOutCurrentBuildingContract(buildingNode.Entity, true);
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public static int CalculateDroidsInUse()
		{
			List<Contract> list = Service.Get<ISupportController>().FindAllContractsThatConsumeDroids();
			return list.Count;
		}

		public static bool HasExistingHeroContract(string heroUpgradeGroup)
		{
			bool result = false;
			IDataController dataController = Service.Get<IDataController>();
			ISupportController supportController = Service.Get<ISupportController>();
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			BuildingComponent buildingComp = buildingLookupController.TacticalCommandNodeList.Head.BuildingComp;
			string key = buildingComp.BuildingTO.Key;
			List<Contract> list = supportController.FindAllTroopContractsForBuilding(key);
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(list[i].ProductUid);
				if (troopTypeVO.UpgradeGroup == heroUpgradeGroup)
				{
					result = true;
					break;
				}
				i++;
			}
			return result;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.CalculateDroidsInUse());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.CalculateNumTroopsQueued((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.CalculateRemainingTimeOfAllTroopContracts((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.CalculateSpaceOccupiedByQueuedTroops((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.CanCancelDeployableContract((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.ContractTypeConsumesDroid((ContractType)(*(int*)args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.FindBuildingToFinish());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.GetBuildingContractTotalTime(Marshal.PtrToStringUni(*(IntPtr*)args), (DeliveryType)(*(int*)(args + 1))));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.GetContractType((DeliveryType)(*(int*)args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.GetCrystalCostToFinishContract((Contract)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.GetDeliveryTypeForBuildingContract(Marshal.PtrToStringUni(*(IntPtr*)args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.GetDeliveryTypeForTroopContract((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.GetTroopContractTotalTime(Marshal.PtrToStringUni(*(IntPtr*)args), (DeliveryType)(*(int*)(args + 1)), (List<string>)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.GetTroopContractTypeByBuilding((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.HasCapacityForTroop((Entity)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.HasExistingHeroContract(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.InstantFreeupDroid());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsArmyContractValid((Contract)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsArmyContractValid((Contract)GCHandledObjects.GCHandleToObject(*args), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsArmyUpgrading((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsBuildingClearing((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsBuildingConstructing((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsBuildingConstructing(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsBuildingSwapping((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsBuildingType((ContractType)(*(int*)args)));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsBuildingUpgrading((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsChampionRepairing((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsEquipmentUpgrading((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsTroopType((ContractType)(*(int*)args)));
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsUpgradeEquipmentContractValid((Contract)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsUpgradeStarshipContractValid((Contract)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.IsUpgradeTroopContractValid((Contract)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractUtils.MinimumCostToFinish());
		}
	}
}
