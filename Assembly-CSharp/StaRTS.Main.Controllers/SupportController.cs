using Net.RichardLord.Ash.Core;
using StaRTS.Externals.DMOAnalytics;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Equipment;
using StaRTS.Main.Models.Commands.Player.Building.Clear;
using StaRTS.Main.Models.Commands.Player.Building.Construct;
using StaRTS.Main.Models.Commands.Player.Building.Contracts;
using StaRTS.Main.Models.Commands.Player.Building.Contracts.Buyout;
using StaRTS.Main.Models.Commands.Player.Building.Contracts.Cancel;
using StaRTS.Main.Models.Commands.Player.Building.Swap;
using StaRTS.Main.Models.Commands.Player.Building.Upgrade;
using StaRTS.Main.Models.Commands.Player.Deployable;
using StaRTS.Main.Models.Commands.Player.Deployable.Upgrade.Start;
using StaRTS.Main.Models.Commands.TransferObjects;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class SupportController : IEventObserver, ISupportController, IViewFrameTimeObserver
	{
		private delegate int ProductDeliveryDelegate(Contract contract, Building buildingTO, BuildingTypeVO buildingVO);

		private delegate bool ContractValidationDelegate(Contract contract);

		private delegate void FinishContractOnServerDelegate(string buildingKey, string productUid, int amount);

		private Dictionary<DeliveryType, SupportController.ProductDeliveryDelegate> deliveryMethods;

		private Dictionary<DeliveryType, SupportController.ContractValidationDelegate> validationMethods;

		private Dictionary<DeliveryType, EventId> events;

		private EventManager eventManager;

		private ServerAPI serverAPI;

		private List<ContractTO> contractDataFromServer;

		private List<string> frozenBuildingsFromServer;

		private List<ContractTO> offlineCompletedContracts;

		private List<Contract> currentContracts;

		private MutableIterator mutableIterator;

		private Dictionary<string, SmartEntity> buildingKeyToEntities;

		private HashSet<string> frozenBuildings;

		private HashSet<string> pausedBuildings;

		private bool isIterating;

		private bool needSortCurrentContracts;

		private Dictionary<string, int> temporaryInventorySizeServerDeltas;

		private float accumulatedUpdateDt;

		public const float UPDATE_TIME_THRESHOLD = 0.1f;

		public SupportController()
		{
			Service.Set<ISupportController>(this);
			this.currentContracts = new List<Contract>();
			this.mutableIterator = new MutableIterator();
			this.frozenBuildings = new HashSet<string>();
			this.pausedBuildings = new HashSet<string>();
			this.buildingKeyToEntities = new Dictionary<string, SmartEntity>();
			this.temporaryInventorySizeServerDeltas = new Dictionary<string, int>();
			this.accumulatedUpdateDt = 0f;
			this.eventManager = Service.Get<EventManager>();
			this.serverAPI = Service.Get<ServerAPI>();
			this.events = new Dictionary<DeliveryType, EventId>
			{
				{
					DeliveryType.Infantry,
					EventId.TroopRecruited
				},
				{
					DeliveryType.Vehicle,
					EventId.TroopRecruited
				},
				{
					DeliveryType.Mercenary,
					EventId.TroopRecruited
				},
				{
					DeliveryType.Starship,
					EventId.StarshipMobilized
				},
				{
					DeliveryType.Hero,
					EventId.HeroMobilized
				},
				{
					DeliveryType.Champion,
					EventId.ChampionRepaired
				},
				{
					DeliveryType.Building,
					EventId.BuildingConstructed
				},
				{
					DeliveryType.UpgradeBuilding,
					EventId.BuildingLevelUpgraded
				},
				{
					DeliveryType.SwapBuilding,
					EventId.BuildingSwapped
				},
				{
					DeliveryType.UpgradeTroop,
					EventId.TroopLevelUpgraded
				},
				{
					DeliveryType.UpgradeStarship,
					EventId.StarshipLevelUpgraded
				},
				{
					DeliveryType.UpgradeEquipment,
					EventId.EquipmentUpgraded
				},
				{
					DeliveryType.ClearClearable,
					EventId.ClearableCleared
				}
			};
			this.deliveryMethods = new Dictionary<DeliveryType, SupportController.ProductDeliveryDelegate>
			{
				{
					DeliveryType.Infantry,
					new SupportController.ProductDeliveryDelegate(this.DeliverTroop)
				},
				{
					DeliveryType.Vehicle,
					new SupportController.ProductDeliveryDelegate(this.DeliverTroop)
				},
				{
					DeliveryType.Mercenary,
					new SupportController.ProductDeliveryDelegate(this.DeliverTroop)
				},
				{
					DeliveryType.Starship,
					new SupportController.ProductDeliveryDelegate(this.DeliverStarship)
				},
				{
					DeliveryType.Hero,
					new SupportController.ProductDeliveryDelegate(this.DeliverHero)
				},
				{
					DeliveryType.Champion,
					new SupportController.ProductDeliveryDelegate(this.DeliverChampion)
				},
				{
					DeliveryType.Building,
					new SupportController.ProductDeliveryDelegate(this.DeliverBuilding)
				},
				{
					DeliveryType.UpgradeBuilding,
					new SupportController.ProductDeliveryDelegate(this.DeliverUpgradeBuilding)
				},
				{
					DeliveryType.SwapBuilding,
					new SupportController.ProductDeliveryDelegate(this.DeliverSwapBuilding)
				},
				{
					DeliveryType.UpgradeTroop,
					new SupportController.ProductDeliveryDelegate(this.DeliverUpgradeTroopOrStarship)
				},
				{
					DeliveryType.UpgradeStarship,
					new SupportController.ProductDeliveryDelegate(this.DeliverUpgradeTroopOrStarship)
				},
				{
					DeliveryType.UpgradeEquipment,
					new SupportController.ProductDeliveryDelegate(this.DeliverUpgradeEquipment)
				},
				{
					DeliveryType.ClearClearable,
					new SupportController.ProductDeliveryDelegate(this.ClearClearable)
				}
			};
			this.validationMethods = new Dictionary<DeliveryType, SupportController.ContractValidationDelegate>
			{
				{
					DeliveryType.Infantry,
					new SupportController.ContractValidationDelegate(ContractUtils.IsArmyContractValid)
				},
				{
					DeliveryType.Vehicle,
					new SupportController.ContractValidationDelegate(ContractUtils.IsArmyContractValid)
				},
				{
					DeliveryType.Mercenary,
					new SupportController.ContractValidationDelegate(ContractUtils.IsArmyContractValid)
				},
				{
					DeliveryType.Starship,
					new SupportController.ContractValidationDelegate(ContractUtils.IsArmyContractValid)
				},
				{
					DeliveryType.Hero,
					new SupportController.ContractValidationDelegate(ContractUtils.IsArmyContractValid)
				},
				{
					DeliveryType.Champion,
					new SupportController.ContractValidationDelegate(ContractUtils.IsArmyContractValid)
				},
				{
					DeliveryType.UpgradeTroop,
					new SupportController.ContractValidationDelegate(ContractUtils.IsUpgradeTroopContractValid)
				},
				{
					DeliveryType.UpgradeStarship,
					new SupportController.ContractValidationDelegate(ContractUtils.IsUpgradeStarshipContractValid)
				},
				{
					DeliveryType.UpgradeEquipment,
					new SupportController.ContractValidationDelegate(ContractUtils.IsUpgradeEquipmentContractValid)
				}
			};
			this.eventManager.RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Notification);
			this.eventManager.RegisterObserver(this, EventId.GameStateChanged, EventPriority.Default);
			this.eventManager.RegisterObserver(this, EventId.ContractsCompletedWhileOffline, EventPriority.Default);
		}

		public void UpdateCurrentContractsListFromServer(object serverData)
		{
			this.contractDataFromServer = new List<ContractTO>();
			List<object> list = serverData as List<object>;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				ContractTO item = new ContractTO().FromObject(list[i]) as ContractTO;
				this.contractDataFromServer.Add(item);
				i++;
			}
		}

		public void UpdateFrozenBuildingsListFromServer(object serverData)
		{
			this.frozenBuildingsFromServer = new List<string>();
			List<object> list = serverData as List<object>;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				this.frozenBuildingsFromServer.Add(list[i] as string);
				i++;
			}
		}

		private void InitializeContracts()
		{
			if (this.contractDataFromServer == null || this.currentContracts.Count > 0)
			{
				return;
			}
			NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
			int i = 0;
			int count = this.contractDataFromServer.Count;
			while (i < count)
			{
				ContractTO contractTO = this.contractDataFromServer[i];
				BuildingTypeVO building = null;
				for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
				{
					if (buildingNode.BuildingComp.BuildingTO.Key == contractTO.BuildingKey)
					{
						building = buildingNode.BuildingComp.BuildingType;
						break;
					}
				}
				DeliveryType deliveryType;
				int totalTime;
				if (ContractUtils.IsBuildingType(contractTO.ContractType))
				{
					deliveryType = ContractUtils.GetDeliveryTypeForBuildingContract(contractTO.Uid, building);
					totalTime = ContractUtils.GetBuildingContractTotalTime(contractTO.Uid, deliveryType);
				}
				else
				{
					deliveryType = ContractUtils.GetDeliveryTypeForTroopContract(building);
					totalTime = ContractUtils.GetTroopContractTotalTime(contractTO.Uid, deliveryType, contractTO.PerkIds);
				}
				Contract contract = new Contract(contractTO.Uid, deliveryType, totalTime, 0.0, contractTO.Tag);
				contract.ContractTO = contractTO;
				contract.UpdateRemainingTime();
				this.currentContracts.Add(contract);
				i++;
			}
			if (this.frozenBuildingsFromServer != null)
			{
				for (BuildingNode buildingNode2 = nodeList.Head; buildingNode2 != null; buildingNode2 = buildingNode2.Next)
				{
					string key = buildingNode2.BuildingComp.BuildingTO.Key;
					int num = this.frozenBuildingsFromServer.IndexOf(key);
					if (num != -1)
					{
						if (this.FindCurrentContract(key) != null)
						{
							this.FreezeBuilding(key);
						}
						this.frozenBuildingsFromServer.RemoveAt(num);
					}
				}
			}
			this.frozenBuildingsFromServer = null;
			this.contractDataFromServer = null;
			this.SortCurrentContracts();
		}

		public List<ContractTO> GetContractEventsThatHappenedOffline()
		{
			return this.offlineCompletedContracts;
		}

		public List<ContractTO> GetUninitializedContractData()
		{
			return this.contractDataFromServer;
		}

		public void ReleaseContractEventsThatHappnedOffline()
		{
			if (this.offlineCompletedContracts != null)
			{
				int i = 0;
				int count = this.offlineCompletedContracts.Count;
				while (i < count)
				{
					ContractTO cookie = this.offlineCompletedContracts[i];
					Service.Get<EventManager>().SendEvent(EventId.ContractCompletedForStoryAction, cookie);
					i++;
				}
			}
			this.offlineCompletedContracts = null;
		}

		private void SendContractContinuedEvents()
		{
			NodeList<SupportNode> nodeList = Service.Get<EntityController>().GetNodeList<SupportNode>();
			for (SupportNode supportNode = nodeList.Head; supportNode != null; supportNode = supportNode.Next)
			{
				Contract contract = this.FindCurrentContract(supportNode.BuildingComp.BuildingTO.Key);
				if (contract != null)
				{
					switch (contract.DeliveryType)
					{
					case DeliveryType.Champion:
					case DeliveryType.Building:
					case DeliveryType.UpgradeBuilding:
					case DeliveryType.SwapBuilding:
						this.DisableBuilding(supportNode.Entity);
						break;
					}
					ContractEventData cookie = new ContractEventData(contract, supportNode.Entity, true);
					this.eventManager.SendEvent(EventId.ContractContinued, cookie);
				}
			}
		}

		public void DisableBuilding(Entity building)
		{
			Service.Get<EventManager>().SendEvent(EventId.ShowScaffolding, building);
			building.Remove<ShooterComponent>();
			building.Remove<TurretShooterComponent>();
			if (building.Has<ShieldGeneratorComponent>())
			{
				Service.Get<ShieldController>().StopAllEffects();
				Service.Get<ShieldController>().RecalculateFlagStampsForShieldBorder(building, false);
				Service.Get<ShieldController>().RemoveShieldEffect(building);
				Service.Get<EntityFactory>().DestroyEntity(building.Get<ShieldGeneratorComponent>().ShieldBorderEntity, true, true);
				building.Remove<ShieldGeneratorComponent>();
				Service.Get<EventManager>().SendEvent(EventId.ShieldDisabled, null);
			}
		}

		private void EnableBuilding(Entity building)
		{
			BuildingTypeVO buildingType = building.Get<BuildingComponent>().BuildingType;
			if (buildingType.Type == BuildingType.Turret)
			{
				TurretTypeVO turretType = Service.Get<IDataController>().Get<TurretTypeVO>(buildingType.TurretUid);
				Service.Get<EntityFactory>().AddTurretComponentsToEntity((SmartEntity)building, turretType);
			}
			if (buildingType.Type == BuildingType.ShieldGenerator)
			{
				Service.Get<EntityFactory>().AddShieldComponentsToEntity(building, buildingType);
				Service.Get<ShieldController>().RecalculateFlagStampsForShieldBorder(building, true);
				Service.Get<ShieldController>().InitializeEffects(building);
			}
		}

		public int SortContractTOByEndTime(ContractTO a, ContractTO b)
		{
			if (a == b)
			{
				return 0;
			}
			uint endTime = a.EndTime;
			uint endTime2 = b.EndTime;
			if (endTime > endTime2)
			{
				return 1;
			}
			if (endTime != endTime2)
			{
				return -1;
			}
			long num = GameUtils.StringHash(a.BuildingKey);
			long num2 = GameUtils.StringHash(b.BuildingKey);
			if (num > num2)
			{
				return 1;
			}
			if (num < num2)
			{
				return -1;
			}
			bool flag = ContractUtils.IsTroopType(a.ContractType);
			bool flag2 = ContractUtils.IsTroopType(b.ContractType);
			if (flag && !flag2)
			{
				return 1;
			}
			if (!flag & flag2)
			{
				return -1;
			}
			num = GameUtils.StringHash(a.Uid);
			num2 = GameUtils.StringHash(b.Uid);
			if (num > num2)
			{
				return 1;
			}
			if (num != num2)
			{
				return -1;
			}
			return 0;
		}

		public int SortByEndTime(Contract a, Contract b)
		{
			if (a == b)
			{
				return 0;
			}
			return this.SortContractTOByEndTime(a.ContractTO, b.ContractTO);
		}

		public Contract FindBuildingContract(string buildingKey)
		{
			int i = 0;
			int count = this.currentContracts.Count;
			while (i < count)
			{
				if (this.currentContracts[i].ContractTO.BuildingKey == buildingKey && ContractUtils.IsBuildingType(this.currentContracts[i].ContractTO.ContractType))
				{
					return this.currentContracts[i];
				}
				i++;
			}
			return null;
		}

		public bool HasTroopContractForBuilding(string buildingKey)
		{
			int i = 0;
			int count = this.currentContracts.Count;
			while (i < count)
			{
				if (this.currentContracts[i].ContractTO.BuildingKey == buildingKey && ContractUtils.IsTroopType(this.currentContracts[i].ContractTO.ContractType))
				{
					return true;
				}
				i++;
			}
			return false;
		}

		public List<Contract> FindAllTroopContractsForBuilding(string buildingKey)
		{
			List<Contract> list = new List<Contract>();
			int i = 0;
			int count = this.currentContracts.Count;
			while (i < count)
			{
				if (this.currentContracts[i].ContractTO.BuildingKey == buildingKey && ContractUtils.IsTroopType(this.currentContracts[i].ContractTO.ContractType))
				{
					list.Add(this.currentContracts[i]);
				}
				i++;
			}
			return list;
		}

		public Contract FindCurrentContract(string buildingKey)
		{
			List<Contract> list = null;
			int i = 0;
			int count = this.currentContracts.Count;
			while (i < count)
			{
				if (this.currentContracts[i].ContractTO.BuildingKey == buildingKey)
				{
					if (list == null)
					{
						list = new List<Contract>();
					}
					list.Add(this.currentContracts[i]);
				}
				i++;
			}
			Contract result = null;
			if (list != null && list.Count > 0)
			{
				result = list[0];
				int j = 1;
				int count2 = list.Count;
				while (j < count2)
				{
					Contract contract = list[j];
					if (ContractUtils.IsBuildingType(contract.ContractTO.ContractType))
					{
						result = contract;
						break;
					}
					j++;
				}
			}
			return result;
		}

		public Contract FindFirstContractWithProductUid(string productUid)
		{
			if (string.IsNullOrEmpty(productUid))
			{
				return null;
			}
			for (int i = 0; i < this.currentContracts.Count; i++)
			{
				if (this.currentContracts[i].ProductUid.Equals(productUid))
				{
					return this.currentContracts[i];
				}
			}
			return null;
		}

		private Contract FindLastContractById(string buildingKey, string productUid)
		{
			for (int i = this.currentContracts.Count - 1; i >= 0; i--)
			{
				if (this.currentContracts[i].ContractTO.BuildingKey == buildingKey && this.currentContracts[i].ContractTO.Uid == productUid)
				{
					return this.currentContracts[i];
				}
			}
			return null;
		}

		private Contract FindLastTroopContractForBuilding(string buildingKey, string productUid)
		{
			Contract contract = null;
			Contract contract2 = null;
			int i = 0;
			int count = this.currentContracts.Count;
			while (i < count)
			{
				if (this.currentContracts[i].ContractTO.BuildingKey == buildingKey)
				{
					contract2 = this.currentContracts[i];
					if (this.currentContracts[i].ContractTO.Uid == productUid && (contract == null || this.currentContracts[i].ContractTO.EndTime > contract.ContractTO.EndTime))
					{
						contract = this.currentContracts[i];
					}
				}
				i++;
			}
			if (contract == null)
			{
				contract = contract2;
			}
			return contract;
		}

		private Contract FindFirstTroopContractForBuilding(string buildingKey)
		{
			Contract result = null;
			int i = 0;
			int count = this.currentContracts.Count;
			while (i < count)
			{
				Contract contract = this.currentContracts[i];
				if (contract.ContractTO.BuildingKey == buildingKey && ContractUtils.IsTroopType(contract.ContractTO.ContractType))
				{
					result = contract;
					break;
				}
				i++;
			}
			return result;
		}

		public List<Contract> FindAllContractsThatConsumeDroids()
		{
			List<Contract> list = new List<Contract>();
			int i = 0;
			int count = this.currentContracts.Count;
			while (i < count)
			{
				if (ContractUtils.ContractTypeConsumesDroid(this.currentContracts[i].ContractTO.ContractType))
				{
					list.Add(this.currentContracts[i]);
				}
				i++;
			}
			return list;
		}

		public bool IsContractValidForStorage(Contract contract)
		{
			return !this.validationMethods.ContainsKey(contract.DeliveryType) || this.validationMethods[contract.DeliveryType](contract);
		}

		private int DeliverProducts(Contract contract, Building buildingTO, BuildingTypeVO buildingVO)
		{
			if (this.deliveryMethods.ContainsKey(contract.DeliveryType))
			{
				return this.deliveryMethods[contract.DeliveryType](contract, buildingTO, buildingVO);
			}
			return 0;
		}

		private int DeliverTroop(Contract contract, Building buildingTO, BuildingTypeVO buildingVO)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			TroopTypeVO troopTypeVO = Service.Get<IDataController>().Get<TroopTypeVO>(contract.ProductUid);
			int level = currentPlayer.UnlockedLevels.Troops.GetLevel(troopTypeVO.UpgradeGroup);
			TroopTypeVO byLevel = Service.Get<TroopUpgradeCatalog>().GetByLevel(troopTypeVO, level);
			this.DeliverUnit(currentPlayer.Inventory.Troop, byLevel);
			return 0;
		}

		private int DeliverStarship(Contract contract, Building buildingTO, BuildingTypeVO buildingVO)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			SpecialAttackTypeVO specialAttackTypeVO = Service.Get<IDataController>().Get<SpecialAttackTypeVO>(contract.ProductUid);
			int level = currentPlayer.UnlockedLevels.Starships.GetLevel(specialAttackTypeVO.UpgradeGroup);
			SpecialAttackTypeVO byLevel = Service.Get<StarshipUpgradeCatalog>().GetByLevel(specialAttackTypeVO, level);
			this.DeliverUnit(currentPlayer.Inventory.SpecialAttack, byLevel);
			return 0;
		}

		private int DeliverHero(Contract contract, Building buildingTO, BuildingTypeVO buildingVO)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			TroopTypeVO troopTypeVO = Service.Get<IDataController>().Get<TroopTypeVO>(contract.ProductUid);
			int level = currentPlayer.UnlockedLevels.Troops.GetLevel(troopTypeVO.UpgradeGroup);
			TroopTypeVO byLevel = Service.Get<TroopUpgradeCatalog>().GetByLevel(troopTypeVO, level);
			this.DeliverUnit(currentPlayer.Inventory.Hero, byLevel);
			return 0;
		}

		private int DeliverChampion(Contract contract, Building buildingTO, BuildingTypeVO buildingVO)
		{
			Service.Get<CurrentPlayer>().OnChampionRepaired(contract.ProductUid);
			return 0;
		}

		private void DeliverUnit(InventoryStorage inventoryStorage, IDeployableVO deployableVO)
		{
			if (this.temporaryInventorySizeServerDeltas.ContainsKey(inventoryStorage.Key) && this.temporaryInventorySizeServerDeltas[inventoryStorage.Key] < 0)
			{
				Dictionary<string, int> dictionary = this.temporaryInventorySizeServerDeltas;
				string key = inventoryStorage.Key;
				dictionary[key] += deployableVO.Size;
				return;
			}
			inventoryStorage.ModifyItemAmount(deployableVO.Uid, 1);
		}

		private int DeliverBuilding(Contract contract, Building buildingTO, BuildingTypeVO buildingVO)
		{
			this.OnBuildingContractDelivered(contract, buildingTO, buildingVO);
			return 0;
		}

		private int DeliverUpgradeBuilding(Contract contract, Building buildingTO, BuildingTypeVO buildingVO)
		{
			buildingTO.Uid = contract.ProductUid;
			this.OnBuildingContractDelivered(contract, buildingTO, buildingVO);
			return 0;
		}

		private int DeliverSwapBuilding(Contract contract, Building buildingTO, BuildingTypeVO buildingVO)
		{
			buildingTO.Uid = contract.ProductUid;
			this.OnBuildingContractDelivered(contract, buildingTO, buildingVO);
			return 0;
		}

		private void OnBuildingContractDelivered(Contract contract, Building buildingTO, BuildingTypeVO buildingVO)
		{
			BuildingType type = buildingVO.Type;
			switch (type)
			{
			case BuildingType.FleetCommand:
			case BuildingType.HeroMobilizer:
			case BuildingType.ChampionPlatform:
			case BuildingType.Starport:
				this.UnfreezeAllBuildings(ServerTime.Time);
				return;
			case BuildingType.Housing:
			case BuildingType.Squad:
				break;
			default:
			{
				if (type != BuildingType.Resource)
				{
					return;
				}
				bool isConstructionContract = contract.DeliveryType == DeliveryType.Building;
				Service.Get<ICurrencyController>().UpdateGeneratorAfterFinishedContract(buildingVO, buildingTO, contract.ContractTO.EndTime, isConstructionContract);
				break;
			}
			}
		}

		private int DeliverUpgradeTroopOrStarship(Contract contract, Building buildingTO, BuildingTypeVO buildingVO)
		{
			Service.Get<CurrentPlayer>().UnlockedLevels.UpgradeTroopsOrStarships(contract);
			return 1;
		}

		private int DeliverUpgradeEquipment(Contract contract, Building buildingTO, BuildingTypeVO buildingVO)
		{
			Service.Get<CurrentPlayer>().UnlockedLevels.UpgradeEquipmentLevel(contract);
			return 1;
		}

		private int ClearClearable(Contract contract, Building buildingTO, BuildingTypeVO buildingVO)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			int currentStorage = buildingTO.CurrentStorage;
			if (currentStorage > 0)
			{
				currentPlayer.Inventory.ModifyCrystals(currentStorage);
				BuildingTypeVO buildingInfo = Service.Get<IDataController>().Get<BuildingTypeVO>(contract.ProductUid);
				Lang lang = Service.Get<Lang>();
				string instructions;
				if (currentStorage == 1)
				{
					instructions = lang.Get("CLEARABLE_FOUND", new object[]
					{
						currentStorage,
						lang.Get("CRYSTAL", new object[0]),
						LangUtils.GetClearableDisplayName(buildingInfo)
					});
				}
				else
				{
					instructions = lang.Get("CLEARABLE_FOUND", new object[]
					{
						currentStorage,
						lang.Get("CRYSTALS", new object[0]),
						LangUtils.GetClearableDisplayName(buildingInfo)
					});
				}
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(instructions);
			}
			currentPlayer.Map.Buildings.Remove(buildingTO);
			return 0;
		}

		public Contract StartHeroMobilization(TroopTypeVO hero, Entity building)
		{
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			Contract result = this.StartTroopContract(hero.Uid, DeliveryType.Hero, hero.Size, building);
			this.SpendCurrencyForDeployableContract(hero, building);
			DeployableContractRequest request = new DeployableContractRequest(buildingComponent.BuildingTO.Key, hero.Uid, 1);
			this.serverAPI.Enqueue(new DeployableStartContractCommand(request));
			return result;
		}

		public Contract StartChampionRepair(TroopTypeVO champion, Entity building)
		{
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			Contract result = this.StartTroopContract(champion.Uid, DeliveryType.Champion, champion.Size, building);
			this.SpendCurrencyForDeployableContract(champion, building);
			DeployableContractRequest request = new DeployableContractRequest(buildingComponent.BuildingTO.Key, champion.Uid, 1);
			this.serverAPI.Enqueue(new DeployableStartContractCommand(request));
			this.eventManager.SendEvent(EventId.ChampionStartedRepairing, building);
			return result;
		}

		public Contract StartStarshipMobilization(SpecialAttackTypeVO starship, Entity building)
		{
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			Contract result = this.StartTroopContract(starship.Uid, DeliveryType.Starship, starship.Size, building);
			this.SpendCurrencyForDeployableContract(starship, building);
			DeployableContractRequest request = new DeployableContractRequest(buildingComponent.BuildingTO.Key, starship.Uid, 1);
			this.serverAPI.Enqueue(new DeployableStartContractCommand(request));
			return result;
		}

		public void StartClearingBuilding(Entity building)
		{
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			BuildingTypeVO buildingType = buildingComponent.BuildingType;
			if (!this.StartBuildingContract(buildingType.Uid, DeliveryType.ClearClearable, buildingType.Time, building))
			{
				return;
			}
			GameUtils.SpendCurrency(buildingType.Credits, buildingType.Materials, buildingType.Contraband, true);
			this.eventManager.SendEvent(EventId.ClearableStarted, building);
			BuildingClearCommand command = new BuildingClearCommand(new BuildingClearRequest
			{
				InstanceId = buildingComponent.BuildingTO.Key,
				PayWithHardCurrency = false
			});
			this.serverAPI.Enqueue(command);
		}

		public void StartTurretCrossgrade(BuildingTypeVO swapBuildingInfo, Entity turret)
		{
			if (!this.StartBuildingContract(swapBuildingInfo.Uid, DeliveryType.SwapBuilding, swapBuildingInfo.SwapTime, turret))
			{
				return;
			}
			int swapCredits = swapBuildingInfo.SwapCredits;
			int swapMaterials = swapBuildingInfo.SwapMaterials;
			int swapContraband = swapBuildingInfo.SwapContraband;
			GameUtils.SpendCurrency(swapCredits, swapMaterials, swapContraband, true);
			BuildingSwapCommand command = new BuildingSwapCommand(new BuildingSwapRequest
			{
				goingToBuildingUid = swapBuildingInfo.Uid,
				InstanceId = turret.Get<BuildingComponent>().BuildingTO.Key
			});
			this.serverAPI.Enqueue(command);
			this.eventManager.SendEvent(EventId.BuildingStartedUpgrading, turret);
		}

		public void InstantBuildingConstruct(BuildingTypeVO buildingType, Entity selectedBuilding, int x, int z, string tag)
		{
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			this.serverAPI.Enqueue(new BuildingConstructCommand(new BuildingConstructRequest(buildingComponent.BuildingTO.Key, buildingComponent.BuildingTO.Uid, new Position
			{
				X = x,
				Z = z
			}, false, true, tag)));
			Contract contract = new Contract(buildingType.Uid, DeliveryType.Building, 0, 0.0, tag);
			ContractEventData cookie = new ContractEventData(contract, selectedBuilding, false);
			Service.Get<EventManager>().SendEvent(EventId.BuildingConstructed, cookie);
		}

		public void StartBuildingConstruct(BuildingTypeVO buildingType, Entity selectedBuilding, int x, int z, string tag)
		{
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			if (buildingType.Type == BuildingType.Resource)
			{
				buildingComponent.BuildingTO.LastCollectTime = ServerTime.Time;
			}
			this.StartBuildingContract(buildingType.Uid, DeliveryType.Building, buildingType.Time, selectedBuilding);
			this.serverAPI.Enqueue(new BuildingConstructCommand(new BuildingConstructRequest(buildingComponent.BuildingTO.Key, buildingComponent.BuildingTO.Uid, new Position
			{
				X = x,
				Z = z
			}, false, buildingComponent.BuildingType.Time == 0, tag)));
		}

		public void StartBuildingUpgrade(BuildingTypeVO nextUpgradeType, Entity selectedBuilding, bool isInstant)
		{
			this.StartBuildingUpgrade(nextUpgradeType, selectedBuilding, isInstant, "");
		}

		public void StartBuildingUpgrade(BuildingTypeVO nextUpgradeType, Entity selectedBuilding, bool isInstant, string tag)
		{
			int totalTime = isInstant ? 0 : nextUpgradeType.Time;
			if (!this.StartBuildingContract(nextUpgradeType.Uid, DeliveryType.UpgradeBuilding, totalTime, selectedBuilding, true, tag))
			{
				return;
			}
			Service.Get<ICurrencyController>().ForceCollectAccruedCurrencyForUpgrade(selectedBuilding);
			if (!isInstant)
			{
				int credits = nextUpgradeType.Credits;
				int materials = nextUpgradeType.Materials;
				int contraband = nextUpgradeType.Contraband;
				GameUtils.SpendCurrency(credits, materials, contraband, true);
			}
			this.eventManager.SendEvent(EventId.BuildingStartedUpgrading, selectedBuilding);
			if (!isInstant)
			{
				BuildingComponent buildingComp = ((SmartEntity)selectedBuilding).BuildingComp;
				this.serverAPI.Enqueue(new BuildingUpgradeCommand(new BuildingContractRequest(buildingComp.BuildingTO.Key, false, tag)));
			}
		}

		public void StartAllWallPartBuildingUpgrade(BuildingTypeVO nextUpgradeType, Entity selectedBuilding, bool sendBackendCommand, bool sendBILog)
		{
			if (!this.StartBuildingContract(nextUpgradeType.Uid, DeliveryType.UpgradeBuilding, nextUpgradeType.Time, selectedBuilding, sendBILog))
			{
				return;
			}
			if (sendBackendCommand)
			{
				int credits = nextUpgradeType.Credits;
				int materials = nextUpgradeType.Materials;
				int contraband = nextUpgradeType.Contraband;
				GameUtils.SpendCurrency(credits, materials, contraband, false);
			}
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			if (sendBackendCommand)
			{
				this.serverAPI.Enqueue(new BuildingUpgradeCommand(new BuildingContractRequest(buildingComponent.BuildingTO.Key, false, "")));
			}
		}

		public bool StartTroopUpgrade(TroopTypeVO troop, Entity building)
		{
			this.StartBuildingContract(troop.Uid, DeliveryType.UpgradeTroop, troop.UpgradeTime, building);
			int upgradeCredits = troop.UpgradeCredits;
			int upgradeMaterials = troop.UpgradeMaterials;
			int upgradeContraband = troop.UpgradeContraband;
			GameUtils.SpendCurrency(upgradeCredits, upgradeMaterials, upgradeContraband, true);
			if (!string.IsNullOrEmpty(troop.UpgradeShardUid))
			{
				Service.Get<DeployableShardUnlockController>().SpendDeployableShard(troop.UpgradeShardUid, troop.UpgradeShardCount);
			}
			this.StartDeployableUpgradeOnServer(troop.Uid, building.Get<BuildingComponent>().BuildingTO.Key);
			return true;
		}

		public bool StartStarshipUpgrade(SpecialAttackTypeVO starship, Entity building)
		{
			this.StartBuildingContract(starship.Uid, DeliveryType.UpgradeStarship, starship.UpgradeTime, building);
			int upgradeCredits = starship.UpgradeCredits;
			int upgradeMaterials = starship.UpgradeMaterials;
			int upgradeContraband = starship.UpgradeContraband;
			GameUtils.SpendCurrency(upgradeCredits, upgradeMaterials, upgradeContraband, true);
			if (!string.IsNullOrEmpty(starship.UpgradeShardUid))
			{
				Service.Get<DeployableShardUnlockController>().SpendDeployableShard(starship.UpgradeShardUid, starship.UpgradeShardCount);
			}
			this.StartDeployableUpgradeOnServer(starship.Uid, building.Get<BuildingComponent>().BuildingTO.Key);
			return true;
		}

		private void StartDeployableUpgradeOnServer(string deployableUid, string buildingInstanceId)
		{
			this.serverAPI.Enqueue(new DeployableUpgradeStartCommand(new DeployableUpgradeStartRequest
			{
				BuildingId = buildingInstanceId,
				TroopUid = deployableUid
			}));
		}

		public bool StartEquipmentUpgrade(EquipmentVO equipment, Entity building)
		{
			this.StartBuildingContract(equipment.Uid, DeliveryType.UpgradeEquipment, equipment.UpgradeTime, building);
			Service.Get<ArmoryController>().ChargeEquipmentUpgradeCost(equipment);
			EquipmentUpgradeStartRequest request = new EquipmentUpgradeStartRequest(building.Get<BuildingComponent>().BuildingTO.Key, equipment.Uid);
			EquipmentUpgradeStartCommand equipmentUpgradeStartCommand = new EquipmentUpgradeStartCommand(request);
			equipmentUpgradeStartCommand.Context = equipment.EquipmentID;
			equipmentUpgradeStartCommand.AddFailureCallback(new AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>.OnFailureCallback(this.OnEquipmentUpgradeFailure));
			this.serverAPI.Enqueue(equipmentUpgradeStartCommand);
			return true;
		}

		private void OnEquipmentUpgradeFailure(uint status, object cookie)
		{
			Service.Get<StaRTSLogger>().ErrorFormat("StartEquipmentUpgrade equipmentID '{0}' failed!", new object[]
			{
				cookie as string
			});
		}

		public Contract StartTroopTrainContract(TroopTypeVO troop, Entity building)
		{
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			DeliveryType deliveryType = DeliveryType.Invalid;
			switch (troop.Type)
			{
			case TroopType.Infantry:
				deliveryType = DeliveryType.Infantry;
				break;
			case TroopType.Vehicle:
				deliveryType = DeliveryType.Vehicle;
				break;
			case TroopType.Mercenary:
				deliveryType = DeliveryType.Mercenary;
				break;
			}
			if (deliveryType == DeliveryType.Invalid)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Error adding bad troop train contract: {0}", new object[]
				{
					troop.Uid
				});
				return null;
			}
			Contract contract = this.StartTroopContract(troop.Uid, deliveryType, troop.Size, building);
			if (contract == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Error Adding contract {0} at {1}", new object[]
				{
					troop.Uid,
					buildingComponent.BuildingTO.Key
				});
				return null;
			}
			this.SpendCurrencyForDeployableContract(troop, building);
			DeployableContractRequest request = new DeployableContractRequest(buildingComponent.BuildingTO.Key, troop.Uid, 1);
			this.serverAPI.Enqueue(new DeployableStartContractCommand(request));
			return contract;
		}

		public void CancelTroopTrainContract(string productUid, Entity building)
		{
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			string key = buildingComponent.BuildingTO.Key;
			Contract contract = this.FindLastContractById(key, productUid);
			if (contract == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("No contract for {0} at {1}", new object[]
				{
					productUid,
					key
				});
				return;
			}
			this.SimulateCheckAllContractsWithCurrentTime();
			this.CancelContract(building, contract);
			uint time = ServerTime.Time;
			int num = 0;
			if (!this.frozenBuildings.Contains(key))
			{
				uint endTime = contract.ContractTO.EndTime;
				if (endTime - (uint)contract.TotalTime <= time)
				{
					num = GameUtils.GetTimeDifferenceSafe(endTime, time);
				}
				else
				{
					num = contract.TotalTime;
				}
				if (num > 0)
				{
					this.ShiftTroopContractTimesAfterThis(key, -num, endTime);
				}
			}
			if (num > 0 || this.IsBuildingFrozen(key))
			{
				this.RefundContractCost(contract);
				this.UnfreezeAllBuildings(time);
				DeployableContractRequest request = new DeployableContractRequest(key, productUid, 1);
				this.serverAPI.Enqueue(new DeployableCancelContractCommand(request));
			}
			else
			{
				this.UnfreezeAllBuildings(time);
			}
			ContractEventData cookie = new ContractEventData(contract, building, false, false);
			this.eventManager.SendEvent(EventId.TroopCancelled, cookie);
		}

		public void BuyoutAllTroopTrainContracts(Entity entity)
		{
			this.BuyoutAllTroopTrainContracts(entity, false);
		}

		public void BuyoutAllTroopTrainContracts(Entity entity, bool alreadySpentCrystals)
		{
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			List<Contract> list = new List<Contract>(this.FindAllTroopContractsForBuilding(buildingComponent.BuildingTO.Key));
			int num = 0;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				Contract contract = null;
				int num2 = 0;
				bool flag = false;
				if (i == list.Count - 1 || list[i].ProductUid != list[i + 1].ProductUid)
				{
					contract = list[num];
					num2 = i + 1 - num;
					int num3;
					if (num == 0)
					{
						num3 = contract.GetRemainingTimeForSim();
					}
					else
					{
						num3 = contract.TotalTime;
					}
					int num4 = contract.TotalTime * (num2 - 1) + num3;
					if (num4 > 0)
					{
						int num5 = GameUtils.SecondsToCrystals(num4);
						if (!alreadySpentCrystals && !GameUtils.SpendCrystals(num5))
						{
							return;
						}
						int currencyAmount = -num5;
						string itemType = "unit";
						string productUid = contract.ProductUid;
						int itemCount = num2;
						string type = Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress ? "FUE_speed_up_unit" : "speed_up_unit";
						string subType = "consumable";
						Service.Get<DMOAnalyticsController>().LogInAppCurrencyAction(currencyAmount, itemType, productUid, itemCount, type, subType);
						flag = true;
					}
					num = i + 1;
				}
				bool silent = i != 0;
				if (flag)
				{
					this.FinishCurrentContract(entity, silent, 0, new SupportController.FinishContractOnServerDelegate(this.BuyoutDeployableContractOnServer), buildingComponent.BuildingTO.Key, contract.ProductUid, num2);
				}
				else
				{
					this.FinishCurrentContract(entity, silent);
				}
				this.UpdateTooltips(entity);
				i++;
			}
		}

		private void BuyoutDeployableContractOnServer(string buildingKey, string productUid, int amount)
		{
			DeployableContractRequest request = new DeployableContractRequest(buildingKey, productUid, amount);
			this.serverAPI.Enqueue(new DeployableBuyoutContractCommand(request));
		}

		private bool StartBuildingContract(string productUid, DeliveryType contractType, int totalTime, Entity building)
		{
			return this.StartBuildingContract(productUid, contractType, totalTime, building, true);
		}

		private bool StartBuildingContract(string productUid, DeliveryType contractType, int totalTime, Entity building, bool sendBILog)
		{
			return this.StartBuildingContract(productUid, contractType, totalTime, building, sendBILog, "");
		}

		private bool StartBuildingContract(string productUid, DeliveryType contractType, int totalTime, Entity building, bool sendBILog, string tag)
		{
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			string key = buildingComponent.BuildingTO.Key;
			Contract contract = this.FindBuildingContract(key);
			uint time = ServerTime.Time;
			if (contract != null)
			{
				return false;
			}
			contract = new Contract(productUid, contractType, totalTime, this.serverAPI.ServerTimePrecise, tag);
			contract.ContractTO = new ContractTO();
			contract.ContractTO.BuildingKey = key;
			contract.ContractTO.Uid = productUid;
			contract.ContractTO.ContractType = ContractUtils.GetContractType(contractType);
			contract.ContractTO.EndTime = time + (uint)totalTime;
			contract.ContractTO.Tag = tag;
			int num = totalTime;
			if (this.IsBuildingFrozen(key))
			{
				Contract contract2 = this.FindFirstTroopContractForBuilding(key);
				if (contract2 != null)
				{
					uint endTime = contract2.ContractTO.EndTime;
					if (time > endTime)
					{
						uint num2 = time - endTime;
						num += (int)num2;
					}
				}
			}
			this.ShiftTroopContractTimes(key, num);
			this.AddContract(contract, building);
			ContractEventData cookie = new ContractEventData(contract, building, false, sendBILog);
			this.eventManager.SendEvent(EventId.ContractAdded, cookie);
			this.eventManager.SendEvent(EventId.ContractStarted, cookie);
			this.eventManager.SendEvent(EventId.ContractBacklogUpdated, cookie);
			this.UpdateTooltips(building);
			switch (contract.DeliveryType)
			{
			case DeliveryType.Building:
			case DeliveryType.UpgradeBuilding:
			case DeliveryType.SwapBuilding:
				this.DisableBuilding(building);
				break;
			}
			return true;
		}

		private Contract StartTroopContract(string productUid, DeliveryType contractType, int productSize, Entity building)
		{
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			Contract contract = this.FindBuildingContract(buildingComponent.BuildingTO.Key);
			if (contract != null)
			{
				return null;
			}
			if (productSize > 0 && !ContractUtils.HasCapacityForTroop(building, productSize))
			{
				return null;
			}
			List<string> playerActivePerkIds = Service.Get<PerkManager>().GetPlayerActivePerkIds();
			int troopContractTotalTime = ContractUtils.GetTroopContractTotalTime(productUid, contractType, playerActivePerkIds);
			Contract contract2 = new Contract(productUid, contractType, troopContractTotalTime, this.serverAPI.ServerTimePrecise);
			contract2.ContractTO = new ContractTO();
			contract2.ContractTO.BuildingKey = buildingComponent.BuildingTO.Key;
			contract2.ContractTO.Uid = productUid;
			contract2.ContractTO.ContractType = ContractUtils.GetContractType(contractType);
			contract2.ContractTO.PerkIds = playerActivePerkIds;
			uint time = ServerTime.Time;
			Contract contract3 = this.FindLastTroopContractForBuilding(buildingComponent.BuildingTO.Key, productUid);
			if (contract3 != null)
			{
				this.ShiftTroopContractTimesAfterThis(buildingComponent.BuildingTO.Key, contract2.TotalTime, contract3.ContractTO.EndTime);
				contract2.ContractTO.EndTime = contract3.ContractTO.EndTime + (uint)contract2.TotalTime;
			}
			else
			{
				contract2.ContractTO.EndTime = time + (uint)contract2.TotalTime;
			}
			this.AddContract(contract2, building);
			ContractEventData cookie = new ContractEventData(contract2, building, false);
			this.eventManager.SendEvent(EventId.ContractAdded, cookie);
			this.eventManager.SendEvent(EventId.ContractBacklogUpdated, cookie);
			this.eventManager.SendEvent(EventId.ContractStarted, cookie);
			this.UpdateTooltips(building);
			return contract2;
		}

		private void UpdateTooltips(Entity building)
		{
			Service.Get<BuildingTooltipController>().EnsureBuildingTooltip((SmartEntity)building);
			Service.Get<UXController>().HUD.UpdateDroidCount();
		}

		public void CancelCurrentBuildingContract(Contract contract, Entity building)
		{
			if (contract.GetRemainingTimeForSim() <= 0)
			{
				return;
			}
			this.RefundContractCost(contract);
			this.CancelContract(building, contract);
			uint serverTime = this.serverAPI.ServerTime;
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			string key = buildingComponent.BuildingTO.Key;
			int timeDifferenceSafe = GameUtils.GetTimeDifferenceSafe(serverTime, contract.ContractTO.EndTime);
			this.ShiftTroopContractTimes(key, timeDifferenceSafe);
			contract.ContractTO.EndTime = serverTime;
			this.SortCurrentContracts();
			if (buildingComponent.BuildingType.Type == BuildingType.Resource)
			{
				buildingComponent.BuildingTO.LastCollectTime = serverTime;
			}
			this.serverAPI.Enqueue(new BuildingContractCancelCommand(new BuildingContractRequest(key, true, "")));
			ContractEventData cookie = new ContractEventData(contract, building, false, false);
			this.eventManager.SendEvent(EventId.BuildingCancelled, cookie);
		}

		private bool IsRefundableContractTypeForBuilding(ContractType contractType)
		{
			return contractType == ContractType.Upgrade || contractType == ContractType.Clear || contractType == ContractType.Research;
		}

		private void RefundContractCost(Contract contract)
		{
			string uid = contract.ContractTO.Uid;
			string buildingKey = contract.ContractTO.BuildingKey;
			List<string> perkIds = contract.ContractTO.PerkIds;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			SmartEntity smartEntity = null;
			ContractType contractType = ContractUtils.GetContractType(contract.DeliveryType);
			float multiplier;
			if (this.IsRefundableContractTypeForBuilding(contractType))
			{
				multiplier = (float)GameConstants.CONTRACT_REFUND_PERCENTAGE_BUILDINGS / 100f;
			}
			else
			{
				if (!ContractUtils.IsTroopType(contractType))
				{
					Service.Get<StaRTSLogger>().Error("Attempted to refund unsupported contract type: " + contractType.ToString());
					return;
				}
				multiplier = (float)GameConstants.CONTRACT_REFUND_PERCENTAGE_TROOPS / 100f;
			}
			this.buildingKeyToEntities.TryGetValue(buildingKey, out smartEntity);
			BuildingTypeVO buildingVO = null;
			if (smartEntity != null)
			{
				buildingVO = smartEntity.Get<BuildingComponent>().BuildingType;
			}
			ContractUtils.CalculateContractCost(uid, contract.DeliveryType, buildingVO, perkIds, out num, out num2, out num3);
			GameUtils.MultiplyCurrency(multiplier, ref num, ref num2, ref num3);
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (num > 0)
			{
				currentPlayer.Inventory.ModifyCredits(num);
			}
			if (num2 > 0)
			{
				currentPlayer.Inventory.ModifyMaterials(num2);
			}
			if (num3 > 0)
			{
				currentPlayer.Inventory.ModifyContraband(num3);
			}
		}

		private Contract CancelContract(Entity entity, Contract contract)
		{
			ContractEventData cookie = new ContractEventData(contract, entity, false);
			this.eventManager.SendEvent(EventId.ContractCanceled, cookie);
			this.eventManager.SendEvent(EventId.ContractBacklogUpdated, cookie);
			this.RemoveContract(entity, contract, true);
			this.UpdateTooltips(entity);
			return contract;
		}

		public bool FinishCurrentContract(Entity entity, bool silent, bool sendBILog)
		{
			return this.FinishCurrentContract(entity, silent, 0, null, null, null, 0, sendBILog);
		}

		public bool FinishCurrentContract(Entity entity, bool silent)
		{
			return this.FinishCurrentContract(entity, silent, 0, null, null, null, 0, true);
		}

		private bool FinishCurrentContract(Entity entity, bool silent, int troopContractShiftTime, SupportController.FinishContractOnServerDelegate serverDelegate, string buildingKey, string productUid, int amount)
		{
			return this.FinishCurrentContract(entity, silent, troopContractShiftTime, serverDelegate, buildingKey, productUid, amount, true);
		}

		private bool FinishCurrentContract(Entity entity, bool silent, int troopContractShiftTime, SupportController.FinishContractOnServerDelegate serverDelegate, string buildingKey, string productUid, int amount, bool sendBILog)
		{
			if (entity == null)
			{
				Service.Get<StaRTSLogger>().Error("entity is null in FinishCurrentContract");
				return false;
			}
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			if (buildingComponent == null)
			{
				GameUtils.LogComponentsAsError("buildingComp is null in FinishCurrentContract", entity);
				return false;
			}
			Building buildingTO = buildingComponent.BuildingTO;
			if (buildingTO == null)
			{
				Service.Get<StaRTSLogger>().Error("buildingTO is null in FinishCurrentContract");
				return false;
			}
			string key = buildingTO.Key;
			if (key == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("buildingTOKey is null in FinishCurrentContract", new object[0]);
				return false;
			}
			Contract contract = this.FindCurrentContract(key);
			ContractEventData cookie = new ContractEventData(contract, entity, silent, sendBILog);
			if (!this.IsContractValidForStorage(contract))
			{
				this.eventManager.SendEvent(EventId.ContractInvalidForStorage, cookie);
				this.eventManager.SendEvent(EventId.ContractStopped, entity);
				this.FreezeBuilding(key);
				return false;
			}
			this.ShiftTroopContractTimes(key, troopContractShiftTime);
			this.RemoveContract(entity, contract, false);
			this.DeliverProducts(contract, buildingTO, buildingComponent.BuildingType);
			this.UpdateTooltips(entity);
			if (serverDelegate != null)
			{
				serverDelegate(buildingKey, productUid, amount);
			}
			if (this.events.ContainsKey(contract.DeliveryType))
			{
				this.eventManager.SendEvent(this.events[contract.DeliveryType], cookie);
			}
			EventId eventId = EventId.ContractCompletedForStoryAction;
			if (eventId != EventId.Nop)
			{
				this.eventManager.SendEvent(eventId, contract.ContractTO);
			}
			this.eventManager.SendEvent(EventId.ContractCompleted, cookie);
			this.eventManager.SendEvent(EventId.ContractBacklogUpdated, cookie);
			string productUid2 = contract.ProductUid;
			if (this.IsDeployableUpgradeContract(contract) && Service.Get<DeployableShardUnlockController>().IsUIDForAShardUpgradableDeployable(productUid2))
			{
				IDataController dataController = Service.Get<IDataController>();
				IDeployableVO optional = dataController.GetOptional<TroopTypeVO>(productUid2);
				if (optional == null)
				{
					optional = dataController.GetOptional<SpecialAttackTypeVO>(productUid2);
				}
				Service.Get<EventManager>().SendEvent(EventId.ShardUnitUpgraded, optional);
			}
			return true;
		}

		private bool IsDeployableUpgradeContract(Contract contract)
		{
			return contract.DeliveryType == DeliveryType.UpgradeStarship || contract.DeliveryType == DeliveryType.UpgradeTroop;
		}

		public void BuyOutCurrentBuildingContract(Entity entity, bool sendBackendCommand)
		{
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			Contract contract = this.FindCurrentContract(buildingComponent.BuildingTO.Key);
			if (contract == null)
			{
				return;
			}
			if (contract.GetRemainingTimeForSim() <= 0)
			{
				return;
			}
			bool flag = contract.DeliveryType == DeliveryType.Champion;
			uint time = ServerTime.Time;
			int troopContractShiftTime = flag ? 0 : GameUtils.GetTimeDifferenceSafe(time, contract.ContractTO.EndTime);
			contract.ContractTO.EndTime = time;
			SupportController.FinishContractOnServerDelegate serverDelegate = null;
			string productUid = flag ? contract.ProductUid : buildingComponent.BuildingTO.Uid;
			if (sendBackendCommand)
			{
				if (contract.DeliveryType == DeliveryType.Champion)
				{
					serverDelegate = new SupportController.FinishContractOnServerDelegate(this.BuyoutDeployableContractOnServer);
				}
				else
				{
					serverDelegate = new SupportController.FinishContractOnServerDelegate(this.BuyoutBuildingOnServer);
				}
			}
			if ((contract.DeliveryType == DeliveryType.Building || contract.DeliveryType == DeliveryType.UpgradeBuilding) && buildingComponent.BuildingType.Type == BuildingType.NavigationCenter)
			{
				Service.Get<CurrentPlayer>().AddUnlockedPlanet(contract.Tag);
			}
			this.FinishCurrentContract(entity, false, troopContractShiftTime, serverDelegate, buildingComponent.BuildingTO.Key, productUid, 1);
		}

		private void BuyoutBuildingOnServer(string buildingKey, string productUid, int amount)
		{
			this.serverAPI.Enqueue(new BuildingContractBuyoutCommand(new BuildingContractRequest(buildingKey, true, "")));
		}

		private void SortCurrentContracts()
		{
			if (!this.isIterating)
			{
				this.currentContracts.Sort(new Comparison<Contract>(this.SortByEndTime));
				this.needSortCurrentContracts = false;
				return;
			}
			this.needSortCurrentContracts = true;
		}

		private void IterationBegin()
		{
			this.mutableIterator.Init(this.currentContracts);
			this.isIterating = true;
		}

		private void IterationEnd()
		{
			this.mutableIterator.Reset();
			this.isIterating = false;
			if (this.needSortCurrentContracts)
			{
				this.SortCurrentContracts();
			}
		}

		private void RemoveContract(Entity buildingEntity, Contract contract, bool isCanceling)
		{
			string buildingKey = contract.ContractTO.BuildingKey;
			int num = this.currentContracts.IndexOf(contract);
			if (num >= 0)
			{
				this.currentContracts.RemoveAt(num);
				this.mutableIterator.OnRemove(num);
			}
			this.SortCurrentContracts();
			Contract contract2 = this.FindCurrentContract(buildingKey);
			if (contract2 != null)
			{
				this.eventManager.SendEvent(EventId.ContractStarted, new ContractEventData(contract2, buildingEntity, false));
			}
			else
			{
				SupportViewComponent supportViewComponent = buildingEntity.Get<SupportViewComponent>();
				if (supportViewComponent != null)
				{
					supportViewComponent.TeardownElements();
				}
				this.eventManager.SendEvent(EventId.ContractStopped, buildingEntity);
			}
			DeliveryType deliveryType = contract.DeliveryType;
			if (deliveryType == DeliveryType.Building)
			{
				this.EnableBuilding(buildingEntity);
				return;
			}
			if (deliveryType != DeliveryType.UpgradeBuilding)
			{
				return;
			}
			if (isCanceling)
			{
				this.EnableBuilding(buildingEntity);
			}
		}

		private void ShiftTroopContractTimes(string buildingKey, int shiftTime)
		{
			List<Contract> list = this.FindAllTroopContractsForBuilding(buildingKey);
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				list[i].ContractTO.EndTime = GameUtils.GetModifiedTimeSafe(list[i].ContractTO.EndTime, shiftTime);
				i++;
			}
		}

		private void ShiftTroopContractTimesAfterThis(string buildingKey, int shiftTime, uint leastEndTime)
		{
			List<Contract> list = this.FindAllTroopContractsForBuilding(buildingKey);
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				if (list[i].ContractTO.EndTime > leastEndTime)
				{
					list[i].ContractTO.EndTime = GameUtils.GetModifiedTimeSafe(list[i].ContractTO.EndTime, shiftTime);
				}
				i++;
			}
		}

		public void OnViewFrameTime(float dt)
		{
			this.accumulatedUpdateDt += dt;
			if (this.accumulatedUpdateDt >= 0.1f)
			{
				this.UpdateAllContracts(this.serverAPI.ServerTime, this.serverAPI.ServerTimePrecise);
				this.accumulatedUpdateDt = 0f;
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			IGameState gameState = Service.Get<GameStateMachine>().CurrentState as IGameState;
			if (id != EventId.WorldLoadComplete)
			{
				if (id != EventId.GameStateChanged)
				{
					if (id == EventId.ContractsCompletedWhileOffline)
					{
						this.offlineCompletedContracts = (cookie as List<ContractTO>);
					}
				}
				else if (!gameState.CanUpdateHomeContracts())
				{
					Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
				}
			}
			else if (gameState is ApplicationLoadState || gameState is HomeState || gameState is WarBoardState)
			{
				this.InitializeContracts();
				this.SendContractContinuedEvents();
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			}
			return EatResponse.NotEaten;
		}

		private void AddContract(Contract contract, Entity entity)
		{
			this.currentContracts.Add(contract);
			this.SortCurrentContracts();
		}

		private void UpdatingBuildingKeyToEntity()
		{
			this.buildingKeyToEntities.Clear();
			NodeList<SupportNode> nodeList = Service.Get<EntityController>().GetNodeList<SupportNode>();
			for (SupportNode supportNode = nodeList.Head; supportNode != null; supportNode = supportNode.Next)
			{
				string key = supportNode.BuildingComp.BuildingTO.Key;
				if (!this.buildingKeyToEntities.ContainsKey(key))
				{
					this.buildingKeyToEntities.Add(key, (SmartEntity)supportNode.Entity);
				}
				else
				{
					Service.Get<StaRTSLogger>().Error("UpdatingBuildingKeyToEntity has duplicates");
				}
			}
		}

		private void UpdateAllContracts(uint now, double nowPrecise)
		{
			this.UpdatingBuildingKeyToEntity();
			this.IterationBegin();
			while (this.mutableIterator.Active())
			{
				Contract contract = this.currentContracts[this.mutableIterator.Index];
				ContractTO contractTO = contract.ContractTO;
				if (contractTO == null)
				{
					Service.Get<StaRTSLogger>().Error("UpdateAllContracts: Null ContractTo found.");
				}
				else
				{
					string buildingKey = contractTO.BuildingKey;
					if (buildingKey == null)
					{
						Service.Get<StaRTSLogger>().Error("UpdateAllContracts: Null buildingKey found.");
					}
					else if (nowPrecise - contract.LastUpdateTime >= 1.0)
					{
						Contract expr_78 = contract;
						double lastUpdateTime = expr_78.LastUpdateTime;
						expr_78.LastUpdateTime = lastUpdateTime + 1.0;
						if (ContractUtils.IsBuildingType(contractTO.ContractType) || !this.IsBuildingFrozen(buildingKey))
						{
							SmartEntity smartEntity;
							this.buildingKeyToEntities.TryGetValue(buildingKey, out smartEntity);
							if (smartEntity != null)
							{
								contract.UpdateRemainingTime();
								if (!this.pausedBuildings.Contains(buildingKey) && contract.IsReadyToBeFinished())
								{
									this.FinishCurrentContract(smartEntity, false);
								}
							}
							else
							{
								Service.Get<StaRTSLogger>().Error("UpdateAllContracts: Null entity found.");
							}
						}
					}
				}
				this.mutableIterator.Next();
			}
			this.IterationEnd();
		}

		public void PauseBuilding(string buildingKey)
		{
			if (!this.pausedBuildings.Contains(buildingKey))
			{
				this.pausedBuildings.Add(buildingKey);
			}
		}

		public void UnpauseAllBuildings()
		{
			this.pausedBuildings.Clear();
			this.SortCurrentContracts();
		}

		public bool IsBuildingFrozen(string buildingKey)
		{
			return this.frozenBuildings.Contains(buildingKey);
		}

		private void FreezeBuilding(string buildingKey)
		{
			if (!this.frozenBuildings.Contains(buildingKey))
			{
				this.frozenBuildings.Add(buildingKey);
			}
		}

		public void UnfreezeAllBuildings(uint time)
		{
			Dictionary<string, uint> dictionary = null;
			HashSet<string> hashSet = null;
			int i = 0;
			int count = this.currentContracts.Count;
			while (i < count)
			{
				Contract contract = this.currentContracts[i];
				string buildingKey = contract.ContractTO.BuildingKey;
				if ((hashSet == null || !hashSet.Contains(buildingKey)) && this.frozenBuildings.Contains(buildingKey) && !contract.DoNotShiftTimesForUnfreeze)
				{
					if (ContractUtils.IsBuildingType(contract.ContractTO.ContractType))
					{
						if (dictionary == null)
						{
							dictionary = new Dictionary<string, uint>();
						}
						dictionary.Add(buildingKey, contract.ContractTO.EndTime);
					}
					else
					{
						if (hashSet == null)
						{
							hashSet = new HashSet<string>();
						}
						hashSet.Add(buildingKey);
						int num = 0;
						uint endTime = contract.ContractTO.EndTime;
						if (dictionary != null && dictionary.ContainsKey(buildingKey))
						{
							uint timeB = dictionary[buildingKey];
							num = GameUtils.GetTimeDifferenceSafe(endTime, timeB);
						}
						else if (time > contract.ContractTO.EndTime)
						{
							num = GameUtils.GetTimeDifferenceSafe(time, endTime);
						}
						if (num != 0)
						{
							this.ShiftTroopContractTimes(buildingKey, num);
						}
					}
				}
				i++;
			}
			this.frozenBuildings.Clear();
			this.SortCurrentContracts();
		}

		private bool ShouldBeFrozen(Contract contract, Dictionary<string, int> inventorySizeOffsets, bool isTroopContract)
		{
			bool result = false;
			if (isTroopContract)
			{
				if (!ContractUtils.IsArmyContractValid(contract, inventorySizeOffsets))
				{
					result = true;
				}
			}
			else if (!this.IsContractValidForStorage(contract))
			{
				result = true;
			}
			return result;
		}

		public void SimulateCheckAllContractsWithCurrentTime()
		{
			List<Contract> list = null;
			this.SimulateCheckAllContractsWithCurrentTime(true, true, out list);
		}

		private void SimulateCheckAllContractsWithCurrentTime(bool updateFreezeState, bool simulateTroopContractUpdate, out List<Contract> finishedContracts)
		{
			finishedContracts = null;
			uint time = ServerTime.Time;
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			HashSet<string> hashSet = null;
			foreach (KeyValuePair<string, int> current in this.temporaryInventorySizeServerDeltas)
			{
				dictionary.Add(current.get_Key(), current.get_Value());
			}
			int i = 0;
			int count = this.currentContracts.Count;
			while (i < count)
			{
				Contract contract = this.currentContracts[i];
				string buildingKey = contract.ContractTO.BuildingKey;
				bool flag = ContractUtils.IsTroopType(contract.ContractTO.ContractType);
				if ((!flag || simulateTroopContractUpdate) && !((hashSet != null && hashSet.Contains(buildingKey)) & flag) && time >= contract.ContractTO.EndTime)
				{
					if (this.ShouldBeFrozen(contract, dictionary, flag))
					{
						if (updateFreezeState)
						{
							this.FreezeBuilding(buildingKey);
						}
						if (hashSet == null)
						{
							hashSet = new HashSet<string>();
						}
						hashSet.Add(buildingKey);
					}
					else
					{
						if (updateFreezeState)
						{
							contract.DoNotShiftTimesForUnfreeze = true;
						}
						if (flag)
						{
							InventoryStorage inventoryStorage;
							int num;
							ContractUtils.GetArmyContractStorageAndSize(contract, out inventoryStorage, out num);
							if (dictionary.ContainsKey(inventoryStorage.Key))
							{
								Dictionary<string, int> dictionary2 = dictionary;
								string key = inventoryStorage.Key;
								dictionary2[key] += num;
							}
							else
							{
								dictionary.Add(inventoryStorage.Key, num);
							}
						}
						if (contract.ContractTO.ContractType == ContractType.Upgrade && contract.TotalTime == 0)
						{
							BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(contract.ProductUid);
							if (buildingTypeVO.Type == BuildingType.Wall)
							{
								goto IL_1A0;
							}
						}
						if (finishedContracts == null)
						{
							finishedContracts = new List<Contract>();
						}
						finishedContracts.Add(contract);
					}
				}
				IL_1A0:
				i++;
			}
		}

		public void GetEstimatedUpdatedContractListsForChecksum(bool simulateTroopContractUpdate, out List<Contract> remainingContracts, out List<Contract> finishedContracts)
		{
			remainingContracts = null;
			finishedContracts = null;
			if (this.currentContracts != null)
			{
				this.SimulateCheckAllContractsWithCurrentTime(false, simulateTroopContractUpdate, out finishedContracts);
				remainingContracts = new List<Contract>(this.currentContracts);
				if (finishedContracts != null)
				{
					int i = 0;
					int count = finishedContracts.Count;
					while (i < count)
					{
						remainingContracts.Remove(finishedContracts[i]);
						i++;
					}
				}
			}
		}

		public void SyncCurrentPlayerInventoryWithServer(Dictionary<string, object> deployables)
		{
			if (deployables != null)
			{
				Inventory inventory = Service.Get<CurrentPlayer>().Inventory;
				this.SyncInventory(inventory.Troop, "troop", deployables);
				this.SyncInventory(inventory.SpecialAttack, "specialAttack", deployables);
				this.SyncInventory(inventory.Hero, "hero", deployables);
				this.SyncInventory(inventory.Champion, "champion", deployables);
			}
		}

		private void SyncInventory(InventoryStorage storage, string storageKey, Dictionary<string, object> deployables)
		{
			if (storage == null)
			{
				return;
			}
			if (deployables.ContainsKey(storageKey))
			{
				Dictionary<string, object> dictionary = deployables[storageKey] as Dictionary<string, object>;
				if (dictionary == null)
				{
					return;
				}
				int totalStorageAmount = storage.GetTotalStorageAmount();
				foreach (string current in storage.GetInternalStorage().Keys)
				{
					storage.ClearItemAmount(current);
				}
				foreach (string current2 in dictionary.Keys)
				{
					if (!storage.HasItem(current2))
					{
						InventoryEntry inventoryEntry = new InventoryEntry();
						inventoryEntry.FromObject(dictionary[current2]);
						storage.CreateInventoryItem(current2, inventoryEntry);
					}
					else
					{
						Dictionary<string, object> dictionary2 = dictionary[current2] as Dictionary<string, object>;
						if (dictionary2 != null)
						{
							int delta = Convert.ToInt32(dictionary2["amount"], CultureInfo.InvariantCulture);
							storage.ModifyItemAmount(current2, delta);
						}
					}
				}
				int totalStorageAmount2 = storage.GetTotalStorageAmount();
				int num = totalStorageAmount - totalStorageAmount2;
				if (this.temporaryInventorySizeServerDeltas.ContainsKey(storage.Key))
				{
					Dictionary<string, int> dictionary3 = this.temporaryInventorySizeServerDeltas;
					string key = storage.Key;
					dictionary3[key] += num;
					return;
				}
				this.temporaryInventorySizeServerDeltas.Add(storage.Key, num);
			}
		}

		public void CheatForceUpdateAllContracts()
		{
			this.UpdateAllContracts(this.serverAPI.ServerTime, this.serverAPI.ServerTimePrecise);
		}

		private void SpendCurrencyForDeployableContract(IDeployableVO deployableVO, Entity building)
		{
			int credits = deployableVO.Credits;
			int materials = deployableVO.Materials;
			int contraband = deployableVO.Contraband;
			BuildingTypeVO buildingType = building.Get<BuildingComponent>().BuildingType;
			float contractCostMultiplier = Service.Get<PerkManager>().GetContractCostMultiplier(buildingType);
			GameUtils.SpendCurrencyWithMultiplier(credits, materials, contraband, contractCostMultiplier, false);
		}

		protected internal SupportController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).AddContract((Contract)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).BuyoutAllTroopTrainContracts((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).BuyoutAllTroopTrainContracts((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).BuyoutBuildingOnServer(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).BuyOutCurrentBuildingContract((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).BuyoutDeployableContractOnServer(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).CancelContract((Entity)GCHandledObjects.GCHandleToObject(*args), (Contract)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).CancelCurrentBuildingContract((Contract)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).CancelTroopTrainContract(Marshal.PtrToStringUni(*(IntPtr*)args), (Entity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).CheatForceUpdateAllContracts();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).ClearClearable((Contract)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).DeliverBuilding((Contract)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).DeliverChampion((Contract)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).DeliverHero((Contract)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).DeliverProducts((Contract)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).DeliverStarship((Contract)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).DeliverSwapBuilding((Contract)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).DeliverTroop((Contract)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).DeliverUnit((InventoryStorage)GCHandledObjects.GCHandleToObject(*args), (IDeployableVO)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).DeliverUpgradeBuilding((Contract)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).DeliverUpgradeEquipment((Contract)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).DeliverUpgradeTroopOrStarship((Contract)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).DisableBuilding((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).EnableBuilding((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).FindAllContractsThatConsumeDroids());
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).FindAllTroopContractsForBuilding(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).FindBuildingContract(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).FindCurrentContract(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).FindFirstContractWithProductUid(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).FindFirstTroopContractForBuilding(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).FindLastContractById(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).FindLastTroopContractForBuilding(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).FinishCurrentContract((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).FinishCurrentContract((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).FinishCurrentContract((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(int*)(args + 2), (SupportController.FinishContractOnServerDelegate)GCHandledObjects.GCHandleToObject(args[3]), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), *(int*)(args + 6)));
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).FinishCurrentContract((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(int*)(args + 2), (SupportController.FinishContractOnServerDelegate)GCHandledObjects.GCHandleToObject(args[3]), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), *(int*)(args + 6), *(sbyte*)(args + 7) != 0));
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).FreezeBuilding(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).GetContractEventsThatHappenedOffline());
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).GetUninitializedContractData());
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).HasTroopContractForBuilding(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).InitializeContracts();
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).InstantBuildingConstruct((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2), *(int*)(args + 3), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)));
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).IsBuildingFrozen(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).IsContractValidForStorage((Contract)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).IsDeployableUpgradeContract((Contract)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).IsRefundableContractTypeForBuilding((ContractType)(*(int*)args)));
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).IterationBegin();
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).IterationEnd();
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).OnBuildingContractDelivered((Contract)GCHandledObjects.GCHandleToObject(*args), (Building)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).PauseBuilding(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).RefundContractCost((Contract)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).ReleaseContractEventsThatHappnedOffline();
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).RemoveContract((Entity)GCHandledObjects.GCHandleToObject(*args), (Contract)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).SendContractContinuedEvents();
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).ShiftTroopContractTimes(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).ShouldBeFrozen((Contract)GCHandledObjects.GCHandleToObject(*args), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).SimulateCheckAllContractsWithCurrentTime();
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).SortByEndTime((Contract)GCHandledObjects.GCHandleToObject(*args), (Contract)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).SortContractTOByEndTime((ContractTO)GCHandledObjects.GCHandleToObject(*args), (ContractTO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).SortCurrentContracts();
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).SpendCurrencyForDeployableContract((IDeployableVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartAllWallPartBuildingUpgrade((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartBuildingConstruct((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2), *(int*)(args + 3), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)));
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartBuildingContract(Marshal.PtrToStringUni(*(IntPtr*)args), (DeliveryType)(*(int*)(args + 1)), *(int*)(args + 2), (Entity)GCHandledObjects.GCHandleToObject(args[3])));
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartBuildingContract(Marshal.PtrToStringUni(*(IntPtr*)args), (DeliveryType)(*(int*)(args + 1)), *(int*)(args + 2), (Entity)GCHandledObjects.GCHandleToObject(args[3]), *(sbyte*)(args + 4) != 0));
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartBuildingContract(Marshal.PtrToStringUni(*(IntPtr*)args), (DeliveryType)(*(int*)(args + 1)), *(int*)(args + 2), (Entity)GCHandledObjects.GCHandleToObject(args[3]), *(sbyte*)(args + 4) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 5))));
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartBuildingUpgrade((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartBuildingUpgrade((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartChampionRepair((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartClearingBuilding((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartDeployableUpgradeOnServer(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartEquipmentUpgrade((EquipmentVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartHeroMobilization((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartStarshipMobilization((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartStarshipUpgrade((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartTroopContract(Marshal.PtrToStringUni(*(IntPtr*)args), (DeliveryType)(*(int*)(args + 1)), *(int*)(args + 2), (Entity)GCHandledObjects.GCHandleToObject(args[3])));
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartTroopTrainContract((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartTroopUpgrade((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).StartTurretCrossgrade((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (Entity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).SyncCurrentPlayerInventoryWithServer((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).SyncInventory((InventoryStorage)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (Dictionary<string, object>)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).UnpauseAllBuildings();
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).UpdateCurrentContractsListFromServer(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).UpdateFrozenBuildingsListFromServer(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).UpdateTooltips((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((SupportController)GCHandledObjects.GCHandleToObject(instance)).UpdatingBuildingKeyToEntity();
			return -1L;
		}
	}
}
