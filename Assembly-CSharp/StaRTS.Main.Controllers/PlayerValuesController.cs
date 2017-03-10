using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class PlayerValuesController : IEventObserver
	{
		private UnlockController unlockController;

		public PlayerValuesController()
		{
			Service.Set<PlayerValuesController>(this);
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.BuildingConstructed, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingLevelUpgraded, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.TroopLevelUpgraded, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Default);
			this.unlockController = Service.Get<UnlockController>();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			switch (id)
			{
			case EventId.TroopLevelUpgraded:
				this.RecalculateXP();
				break;
			case EventId.StarshipLevelUpgraded:
			case EventId.BuildingSwapped:
				break;
			case EventId.BuildingLevelUpgraded:
			case EventId.BuildingConstructed:
				this.RecalculateAll();
				break;
			default:
				if (id == EventId.WorldLoadComplete)
				{
					if (currentState is ApplicationLoadState || currentState is HomeState || currentState is NeighborVisitState)
					{
						this.RecalculateAll();
					}
				}
				break;
			}
			return EatResponse.NotEaten;
		}

		public void RecalculateAll()
		{
			this.RecalcuateTroopHousingCapacity();
			this.RecalculateHeroHousingCapacity();
			this.RecalculateChampionHousingCapacity();
			this.RecalculateStarshipHousingCapacity();
			this.RecalculateCurrencyCapacities();
			this.RecalculateXP();
			this.RecalculateMaxEquipmentCapacity();
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			worldOwner.IsContrabandUnlocked = Service.Get<BuildingLookupController>().IsContrabandUnlocked();
			Service.Get<EventManager>().SendEvent(EventId.InventoryCapacityChanged, null);
		}

		private void RecalculateCurrencyCapacities()
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			NodeList<StorageNode> storageNodeList = Service.Get<BuildingLookupController>().StorageNodeList;
			for (StorageNode storageNode = storageNodeList.Head; storageNode != null; storageNode = storageNode.Next)
			{
				BuildingTypeVO buildingType = storageNode.BuildingComp.BuildingType;
				if (!ContractUtils.IsBuildingConstructing(storageNode.Entity))
				{
					switch (buildingType.Currency)
					{
					case CurrencyType.Credits:
						num += buildingType.Storage;
						break;
					case CurrencyType.Materials:
						num2 += buildingType.Storage;
						break;
					case CurrencyType.Contraband:
						num3 += buildingType.Storage;
						break;
					}
				}
			}
			NodeList<SquadBuildingNode> squadBuildingNodeList = Service.Get<BuildingLookupController>().SquadBuildingNodeList;
			SquadBuildingNode head = squadBuildingNodeList.Head;
			int squadCenterLevel = 0;
			if (head != null)
			{
				BuildingComponent buildingComp = head.BuildingComp;
				BuildingTypeVO buildingType2 = buildingComp.BuildingType;
				squadCenterLevel = buildingType2.Lvl;
			}
			int num4 = GameUtils.GetReputationCapacityForLevel(squadCenterLevel);
			if (num == 0)
			{
				num = GameConstants.NEW_PLAYER_CREDITS_CAPACITY;
			}
			if (num2 == 0)
			{
				num2 = GameConstants.NEW_PLAYER_MATERIALS_CAPACITY;
			}
			if (num3 == 0)
			{
				num3 = GameConstants.NEW_PLAYER_CONTRABAND_CAPACITY;
			}
			if (num4 == 0)
			{
				num4 = GameConstants.NEW_PLAYER_REPUTATION_CAPACITY;
			}
			worldOwner.Inventory.SetItemCapacity("credits", num);
			worldOwner.Inventory.SetItemCapacity("materials", num2);
			worldOwner.Inventory.SetItemCapacity("contraband", num3);
			worldOwner.Inventory.SetItemCapacity("reputation", num4);
		}

		private void RecalcuateTroopHousingCapacity()
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			int num = 0;
			NodeList<StarportNode> starportNodeList = Service.Get<BuildingLookupController>().StarportNodeList;
			for (StarportNode starportNode = starportNodeList.Head; starportNode != null; starportNode = starportNode.Next)
			{
				BuildingTypeVO buildingType = starportNode.BuildingComp.BuildingType;
				if (!ContractUtils.IsBuildingConstructing(starportNode.Entity))
				{
					num += buildingType.Storage;
				}
			}
			if (num == 0)
			{
				num = GameConstants.NEW_PLAYER_TROOP_CAPACITY;
			}
			worldOwner.Inventory.Troop.SetTotalStorageCapacity(num);
		}

		private void RecalculateHeroHousingCapacity()
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			int num = 0;
			NodeList<TacticalCommandNode> tacticalCommandNodeList = Service.Get<BuildingLookupController>().TacticalCommandNodeList;
			for (TacticalCommandNode tacticalCommandNode = tacticalCommandNodeList.Head; tacticalCommandNode != null; tacticalCommandNode = tacticalCommandNode.Next)
			{
				BuildingTypeVO buildingType = tacticalCommandNode.BuildingComp.BuildingType;
				if (!ContractUtils.IsBuildingConstructing(tacticalCommandNode.Entity))
				{
					num += buildingType.Storage;
				}
			}
			if (num == 0)
			{
				num = GameConstants.NEW_PLAYER_HERO_CAPACITY;
			}
			worldOwner.Inventory.Hero.SetTotalStorageCapacity(num);
		}

		private void RecalculateChampionHousingCapacity()
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			int num = 0;
			NodeList<ChampionPlatformNode> championPlatformNodeList = Service.Get<BuildingLookupController>().ChampionPlatformNodeList;
			for (ChampionPlatformNode championPlatformNode = championPlatformNodeList.Head; championPlatformNode != null; championPlatformNode = championPlatformNode.Next)
			{
				BuildingTypeVO buildingType = championPlatformNode.BuildingComp.BuildingType;
				if (!ContractUtils.IsBuildingConstructing(championPlatformNode.Entity))
				{
					num += buildingType.Storage;
				}
			}
			if (num == 0)
			{
				num = GameConstants.NEW_PLAYER_CHAMPION_CAPACITY;
			}
			worldOwner.Inventory.Champion.SetTotalStorageCapacity(num);
		}

		private void RecalculateStarshipHousingCapacity()
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			int num = 0;
			NodeList<FleetCommandNode> fleetCommandNodeList = Service.Get<BuildingLookupController>().FleetCommandNodeList;
			for (FleetCommandNode fleetCommandNode = fleetCommandNodeList.Head; fleetCommandNode != null; fleetCommandNode = fleetCommandNode.Next)
			{
				BuildingTypeVO buildingType = fleetCommandNode.BuildingComp.BuildingType;
				if (!ContractUtils.IsBuildingConstructing(fleetCommandNode.Entity))
				{
					num += buildingType.Storage;
				}
			}
			if (num == 0)
			{
				num = GameConstants.NEW_PLAYER_STARSHIP_CAPACITY;
			}
			worldOwner.Inventory.SpecialAttack.SetTotalStorageCapacity(num);
		}

		private void RecalculateXP()
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			Inventory inventory = worldOwner.Inventory;
			int num = -worldOwner.CurrentXPAmount;
			NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
			IDataController dataController = Service.Get<IDataController>();
			for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
			{
				BuildingTypeVO buildingType = buildingNode.BuildingComp.BuildingType;
				if (!ContractUtils.IsBuildingConstructing(buildingNode.Entity))
				{
					num += buildingType.Xp;
				}
			}
			foreach (TroopTypeVO current in dataController.GetAll<TroopTypeVO>())
			{
				if (current.PlayerFacing && worldOwner.UnlockedLevels.Troops.GetLevel(current.UpgradeGroup) == current.Lvl && worldOwner.Faction == current.Faction && this.unlockController.IsTroopUnlocked(worldOwner, current))
				{
					num += current.Xp;
				}
			}
			foreach (SpecialAttackTypeVO current2 in dataController.GetAll<SpecialAttackTypeVO>())
			{
				if (current2.PlayerFacing && worldOwner.UnlockedLevels.Starships.GetLevel(current2.UpgradeGroup) == current2.Lvl && worldOwner.Faction == current2.Faction && this.unlockController.IsSpecialAttackUnlocked(worldOwner, current2))
				{
					num += current2.Xp;
				}
			}
			inventory.ModifyXP(num);
		}

		private void RecalculateMaxEquipmentCapacity()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			NodeList<ArmoryNode> armoryNodeList = Service.Get<BuildingLookupController>().ArmoryNodeList;
			int num = 0;
			for (ArmoryNode armoryNode = armoryNodeList.Head; armoryNode != null; armoryNode = armoryNode.Next)
			{
				BuildingTypeVO buildingType = armoryNode.BuildingComp.BuildingType;
				if (!ContractUtils.IsBuildingConstructing(armoryNode.Entity))
				{
					num += buildingType.Storage;
				}
			}
			currentPlayer.ActiveArmory.SetMaxEquipmentCapacity(num);
		}

		protected internal PlayerValuesController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerValuesController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlayerValuesController)GCHandledObjects.GCHandleToObject(instance)).RecalcuateTroopHousingCapacity();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PlayerValuesController)GCHandledObjects.GCHandleToObject(instance)).RecalculateAll();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PlayerValuesController)GCHandledObjects.GCHandleToObject(instance)).RecalculateChampionHousingCapacity();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((PlayerValuesController)GCHandledObjects.GCHandleToObject(instance)).RecalculateCurrencyCapacities();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((PlayerValuesController)GCHandledObjects.GCHandleToObject(instance)).RecalculateHeroHousingCapacity();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((PlayerValuesController)GCHandledObjects.GCHandleToObject(instance)).RecalculateMaxEquipmentCapacity();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((PlayerValuesController)GCHandledObjects.GCHandleToObject(instance)).RecalculateStarshipHousingCapacity();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((PlayerValuesController)GCHandledObjects.GCHandleToObject(instance)).RecalculateXP();
			return -1L;
		}
	}
}
