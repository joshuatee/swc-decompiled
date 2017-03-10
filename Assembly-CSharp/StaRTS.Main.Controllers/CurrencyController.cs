using Net.RichardLord.Ash.Core;
using StaRTS.Externals.Manimal;
using StaRTS.FX;
using StaRTS.Main.Controllers.Entities.Systems;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player.Building.Collect;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class CurrencyController : IEventObserver, ICurrencyController
	{
		private const int MIN_FOR_COLLECTION = 1;

		private const int INITIAL_CONTRABAND_SEED = 1;

		private EntityController entityController;

		public const string BUILD_MORE_CREDIT_STORAGE = "BUILD_MORE_CREDIT_STORAGE";

		public const string FULL_CREDIT_STORAGE = "FULL_CREDIT_STORAGE";

		public const string UPGRADE_CREDIT_STORAGE = "UPGRADE_CREDIT_STORAGE";

		public const string BUILD_MORE_MATERIAL_STORAGE = "BUILD_MORE_MATERIAL_STORAGE";

		public const string FULL_MATERIAL_STORAGE = "FULL_MATERIAL_STORAGE";

		public const string UPGRADE_MATERIAL_STORAGE = "UPGRADE_MATERIAL_STORAGE";

		public const string BUILD_MORE_CONTRABAND_STORAGE = "BUILD_MORE_CONTRABAND_STORAGE";

		public const string FULL_CONTRABAND_STORAGE = "FULL_CONTRABAND_STORAGE";

		public const string UPGRADE_CONTRABAND_STORAGE = "UPGRADE_CONTRABAND_STORAGE";

		public CurrencyController()
		{
			Service.Set<ICurrencyController>(this);
			this.entityController = Service.Get<EntityController>();
			Service.Get<EventManager>().RegisterObserver(this, EventId.GameStateChanged, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingConstructed, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.InventoryResourceUpdated, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.PlanetRelocateStarted, EventPriority.Default);
		}

		public bool TryCollectCurrencyOnSelection(Entity entity)
		{
			if (!(Service.Get<GameStateMachine>().CurrentState is HomeState))
			{
				return false;
			}
			if (Service.Get<PostBattleRepairController>().IsEntityInRepair(entity))
			{
				return false;
			}
			if (ContractUtils.IsBuildingConstructing(entity) || ContractUtils.IsBuildingUpgrading(entity))
			{
				return false;
			}
			if (!this.IsGeneratorThresholdMet(entity))
			{
				return false;
			}
			if (!this.CanStoreCollectionAmountFromGenerator(entity))
			{
				BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
				CurrencyType currency = buildingComponent.BuildingType.Currency;
				this.HandleUnableToCollect(currency);
				return false;
			}
			this.CollectCurrency(entity);
			return true;
		}

		public void CollectCurrency(Entity buildingEntity)
		{
			if (buildingEntity == null)
			{
				return;
			}
			GeneratorComponent generatorComponent = buildingEntity.Get<GeneratorComponent>();
			BuildingComponent buildingComponent = buildingEntity.Get<BuildingComponent>();
			GeneratorViewComponent generatorViewComponent = buildingEntity.Get<GeneratorViewComponent>();
			if (buildingComponent == null || generatorComponent == null)
			{
				return;
			}
			Building buildingTO = buildingComponent.BuildingTO;
			int num = this.CollectCurrencyFromGenerator(buildingEntity, true);
			string contextId = "";
			CurrencyType currency = buildingComponent.BuildingType.Currency;
			switch (currency)
			{
			case CurrencyType.Credits:
				contextId = "Credits";
				break;
			case CurrencyType.Materials:
				contextId = "Materials";
				break;
			case CurrencyType.Contraband:
				contextId = "Contraband";
				break;
			}
			if (buildingTO.AccruedCurrency < 1)
			{
				Service.Get<UXController>().HUD.ToggleContextButton(contextId, false);
			}
			if (num > 0)
			{
				this.OnCollectCurrency(buildingEntity, buildingComponent, num);
			}
			else if (num == 0)
			{
				this.HandleUnableToCollect(currency);
			}
			if (buildingTO.CurrentStorage == 0)
			{
				generatorViewComponent.ShowCollectButton(false);
			}
		}

		public void HandleUnableToCollect(CurrencyType currencyType)
		{
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			IDataController dataController = Service.Get<IDataController>();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			BuildingTypeVO buildingTypeVO = null;
			foreach (BuildingTypeVO current in dataController.GetAll<BuildingTypeVO>())
			{
				if (current.Faction == currentPlayer.Faction && current.Type == BuildingType.Storage && current.Currency == currencyType)
				{
					buildingTypeVO = current;
					break;
				}
			}
			int num = 0;
			if (buildingTypeVO != null)
			{
				num = buildingLookupController.GetBuildingMaxPurchaseQuantity(buildingTypeVO, 0);
			}
			int num2 = 0;
			int num3 = 0;
			int highestLevelHQ = buildingLookupController.GetHighestLevelHQ();
			for (StorageNode storageNode = buildingLookupController.StorageNodeList.Head; storageNode != null; storageNode = storageNode.Next)
			{
				if (storageNode.BuildingComp.BuildingType.Currency == currencyType)
				{
					num2++;
					if (storageNode.BuildingComp.BuildingType.Lvl == highestLevelHQ)
					{
						num3++;
					}
				}
			}
			bool flag = num2 < num;
			string instructions = string.Empty;
			switch (currencyType)
			{
			case CurrencyType.Credits:
				if (flag)
				{
					instructions = Service.Get<Lang>().Get("BUILD_MORE_CREDIT_STORAGE", new object[0]);
				}
				else if (num3 == num2)
				{
					instructions = Service.Get<Lang>().Get("FULL_CREDIT_STORAGE", new object[0]);
				}
				else
				{
					instructions = Service.Get<Lang>().Get("UPGRADE_CREDIT_STORAGE", new object[0]);
				}
				break;
			case CurrencyType.Materials:
				if (flag)
				{
					instructions = Service.Get<Lang>().Get("BUILD_MORE_MATERIAL_STORAGE", new object[0]);
				}
				else if (num3 == num2)
				{
					instructions = Service.Get<Lang>().Get("FULL_MATERIAL_STORAGE", new object[0]);
				}
				else
				{
					instructions = Service.Get<Lang>().Get("UPGRADE_MATERIAL_STORAGE", new object[0]);
				}
				break;
			case CurrencyType.Contraband:
				if (flag)
				{
					instructions = Service.Get<Lang>().Get("BUILD_MORE_CONTRABAND_STORAGE", new object[0]);
				}
				else if (num3 == num2)
				{
					instructions = Service.Get<Lang>().Get("FULL_CONTRABAND_STORAGE", new object[0]);
				}
				else
				{
					instructions = Service.Get<Lang>().Get("UPGRADE_CONTRABAND_STORAGE", new object[0]);
				}
				break;
			}
			Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(instructions);
		}

		public void ForceCollectAccruedCurrencyForUpgrade(Entity buildingEntity)
		{
			if (buildingEntity == null)
			{
				return;
			}
			GeneratorComponent generatorComponent = buildingEntity.Get<GeneratorComponent>();
			BuildingComponent buildingComponent = buildingEntity.Get<BuildingComponent>();
			if (buildingComponent == null || generatorComponent == null)
			{
				return;
			}
			int num = this.CollectCurrencyFromGenerator(buildingEntity, false);
			if (num > 0)
			{
				this.OnCollectCurrency(buildingEntity, buildingComponent, num);
			}
		}

		private void OnCollectCurrency(Entity buildingEntity, BuildingComponent buildingComp, int amountCollected)
		{
			CurrencyCollectionTag currencyCollectionTag = new CurrencyCollectionTag();
			currencyCollectionTag.Building = buildingEntity;
			currencyCollectionTag.Type = buildingComp.BuildingType.Currency;
			currencyCollectionTag.Delta = amountCollected;
			Service.Get<CurrencyEffects>().PlayEffect(buildingEntity, currencyCollectionTag.Type, amountCollected);
			this.UpdateStorageEffectsAfterCollection(buildingEntity, buildingComp.BuildingType);
			GeneratorViewComponent generatorViewComp = ((SmartEntity)buildingEntity).GeneratorViewComp;
			if (generatorViewComp != null)
			{
				generatorViewComp.ShowAmountCollectedText(amountCollected, currencyCollectionTag.Type);
			}
			Service.Get<EventManager>().SendEvent(EventId.CurrencyCollected, currencyCollectionTag);
		}

		private bool GetTimePassed(Building buildingTO, out uint deltaTime, bool logErrors)
		{
			uint time = ServerTime.Time;
			if (time < buildingTO.LastCollectTime)
			{
				deltaTime = 0u;
				if (logErrors)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Cannot collect from {0}, cur time {1} is less than last collect time {2}", new object[]
					{
						buildingTO.Key,
						time,
						buildingTO.LastCollectTime
					});
				}
				return false;
			}
			deltaTime = time - buildingTO.LastCollectTime;
			if (deltaTime > 2147483647u)
			{
				if (logErrors)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Cannot collect from {0}, delta time {1} is too large", new object[]
					{
						buildingTO.Key,
						deltaTime
					});
				}
				return false;
			}
			return true;
		}

		private int CollectCurrencyFromGenerator(Entity buildingEntity, bool sendServerCommand)
		{
			BuildingComponent buildingComponent = buildingEntity.Get<BuildingComponent>();
			Building buildingTO = buildingComponent.BuildingTO;
			Inventory inventory = Service.Get<CurrentPlayer>().Inventory;
			int num = -1;
			int num2 = 0;
			uint secondsPassed;
			if (this.GetTimePassed(buildingTO, out secondsPassed, true))
			{
				num = this.CalculateAccruedCurrency(buildingTO.CurrentStorage, buildingComponent.BuildingType, secondsPassed);
				switch (buildingComponent.BuildingType.Currency)
				{
				case CurrencyType.Credits:
					num2 = inventory.ModifyCredits(num);
					break;
				case CurrencyType.Materials:
					num2 = inventory.ModifyMaterials(num);
					break;
				case CurrencyType.Contraband:
					num2 = inventory.ModifyContraband(num);
					break;
				}
			}
			ServerAPI serverAPI = Service.Get<ServerAPI>();
			buildingTO.LastCollectTime = serverAPI.ServerTime;
			buildingTO.CurrentStorage = num2;
			buildingTO.AccruedCurrency = num2;
			if (sendServerCommand)
			{
				serverAPI.Enqueue(new BuildingCollectCommand(new BuildingCollectRequest
				{
					BuildingId = buildingTO.Key
				}));
			}
			return num - num2;
		}

		public bool CanStoreCollectionAmountFromGenerator(Entity buildingEntity)
		{
			BuildingComponent buildingComponent = buildingEntity.Get<BuildingComponent>();
			Inventory inventory = Service.Get<CurrentPlayer>().Inventory;
			int num = 0;
			switch (buildingComponent.BuildingType.Currency)
			{
			case CurrencyType.Credits:
				num = inventory.GetItemCapacity("credits") - inventory.GetItemAmount("credits");
				break;
			case CurrencyType.Materials:
				num = inventory.GetItemCapacity("materials") - inventory.GetItemAmount("materials");
				break;
			case CurrencyType.Contraband:
				num = inventory.GetItemCapacity("contraband") - inventory.GetItemAmount("contraband");
				break;
			}
			return num > 0;
		}

		public bool IsGeneratorCollectable(Entity buildingEntity)
		{
			BuildingComponent buildingComponent = buildingEntity.Get<BuildingComponent>();
			return buildingComponent.BuildingType.Type == BuildingType.Resource && buildingComponent.BuildingTO.AccruedCurrency >= 1;
		}

		public bool IsGeneratorThresholdMet(Entity buildingEntity)
		{
			BuildingComponent buildingComponent = buildingEntity.Get<BuildingComponent>();
			if (buildingComponent == null)
			{
				return false;
			}
			BuildingTypeVO buildingType = buildingComponent.BuildingType;
			return buildingType.Type == BuildingType.Resource && buildingComponent.BuildingTO.AccruedCurrency >= buildingType.CollectNotify;
		}

		public int CalculateTimeUntilAllGeneratorsFull()
		{
			NodeList<GeneratorViewNode> generatorViewNodeList = Service.Get<BuildingLookupController>().GeneratorViewNodeList;
			ISupportController supportController = Service.Get<ISupportController>();
			int num = 0;
			if (generatorViewNodeList != null)
			{
				for (GeneratorViewNode generatorViewNode = generatorViewNodeList.Head; generatorViewNode != null; generatorViewNode = generatorViewNode.Next)
				{
					if (generatorViewNode.BuildingComp != null && supportController.FindCurrentContract(generatorViewNode.BuildingComp.BuildingTO.Key) == null)
					{
						int num2 = this.CalculateGeneratorFillTimeRemaining(generatorViewNode.Entity);
						if (num2 > num)
						{
							num = num2;
						}
					}
				}
			}
			return num;
		}

		public int CalculateGeneratorFillTimeRemaining(Entity buildingEntity)
		{
			int result = 0;
			if (buildingEntity != null)
			{
				BuildingComponent buildingComponent = buildingEntity.Get<BuildingComponent>();
				BuildingTypeVO buildingType = buildingComponent.BuildingType;
				int accruedCurrency = buildingComponent.BuildingTO.AccruedCurrency;
				int storage = buildingType.Storage;
				int num = storage - accruedCurrency;
				result = Service.Get<PerkManager>().GetSecondsTillFullIncludingPerkAdjustedRate(buildingType, (float)num, ServerTime.Time);
			}
			return result;
		}

		public void UpdateGeneratorAccruedCurrency(SmartEntity entity)
		{
			Building buildingTO = entity.BuildingComp.BuildingTO;
			BuildingTypeVO buildingType = entity.BuildingComp.BuildingType;
			bool flag = Service.Get<BuildingController>().SelectedBuilding == entity;
			ISupportController supportController = Service.Get<ISupportController>();
			if (supportController.FindCurrentContract(entity.BuildingComp.BuildingTO.Key) == null)
			{
				int accruedCurrency = buildingTO.AccruedCurrency;
				buildingTO.AccruedCurrency = this.UpdateAccruedCurrencyForView(buildingTO, buildingType);
				entity.GeneratorViewComp.ShowCollectButton(buildingTO.AccruedCurrency >= buildingType.CollectNotify);
				if (buildingTO.AccruedCurrency >= buildingType.Storage && accruedCurrency < buildingType.Storage)
				{
					Service.Get<EventManager>().SendEvent(EventId.GeneratorJustFilled, entity);
				}
			}
			if (flag)
			{
				string contextId = null;
				switch (buildingType.Currency)
				{
				case CurrencyType.Credits:
					contextId = "Credits";
					break;
				case CurrencyType.Materials:
					contextId = "Materials";
					break;
				case CurrencyType.Contraband:
					contextId = "Contraband";
					break;
				}
				Service.Get<UXController>().HUD.ToggleContextButton(contextId, this.IsGeneratorCollectable(entity));
			}
		}

		public float CurrencyPerSecond(BuildingTypeVO type)
		{
			return (float)type.Produce / (float)type.CycleTime;
		}

		public int CurrencyPerHour(BuildingTypeVO type)
		{
			return (int)(3600f * this.CurrencyPerSecond(type));
		}

		public int CalculateAccruedCurrency(Building buildingTO, BuildingTypeVO type)
		{
			uint secondsPassed;
			if (!this.GetTimePassed(buildingTO, out secondsPassed, false))
			{
				return buildingTO.AccruedCurrency;
			}
			return this.CalculateAccruedCurrency(buildingTO.CurrentStorage, type, secondsPassed);
		}

		private int CalculateAccruedCurrency(int currentStorage, BuildingTypeVO type, uint secondsPassed)
		{
			PerkManager perkManager = Service.Get<PerkManager>();
			uint time = ServerTime.Time;
			uint startTime = time - secondsPassed;
			int num = perkManager.GetAccruedCurrencyIncludingPerkAdjustedRate(type, startTime, time) + currentStorage;
			if (num > type.Storage)
			{
				num = type.Storage;
			}
			return num;
		}

		private int UpdateAccruedCurrencyForView(Building buildingTO, BuildingTypeVO type)
		{
			int result = buildingTO.AccruedCurrency;
			uint secondsPassed;
			if (this.GetTimePassed(buildingTO, out secondsPassed, false))
			{
				result = this.CalculateAccruedCurrency(buildingTO.CurrentStorage, type, secondsPassed);
			}
			return result;
		}

		public void UpdateGeneratorAfterFinishedContract(BuildingTypeVO buildingVO, Building buildingTO, uint contractFinishTime, bool isConstructionContract)
		{
			if (buildingVO.Type != BuildingType.Resource)
			{
				return;
			}
			buildingTO.LastCollectTime = contractFinishTime;
			if (isConstructionContract && buildingVO.Currency == CurrencyType.Contraband)
			{
				buildingTO.AccruedCurrency = 1;
				buildingTO.CurrentStorage = 1;
			}
		}

		private void UpdateStorageEffectsAfterCollection(Entity generatorEntity, BuildingTypeVO generatorVO)
		{
			StorageEffects storageEffects = Service.Get<StorageEffects>();
			storageEffects.UpdateFillState(generatorEntity, generatorVO);
			NodeList<StorageNode> nodeList = Service.Get<EntityController>().GetNodeList<StorageNode>();
			for (StorageNode storageNode = nodeList.Head; storageNode != null; storageNode = storageNode.Next)
			{
				BuildingTypeVO buildingType = storageNode.BuildingComp.BuildingType;
				if (buildingType.Currency == generatorVO.Currency)
				{
					storageEffects.UpdateFillState(storageNode.Entity, buildingType);
				}
			}
		}

		public void UpdateAllStorageEffects()
		{
			StorageEffects storageEffects = Service.Get<StorageEffects>();
			NodeList<StorageNode> nodeList = Service.Get<EntityController>().GetNodeList<StorageNode>();
			for (StorageNode storageNode = nodeList.Head; storageNode != null; storageNode = storageNode.Next)
			{
				storageEffects.UpdateFillState(storageNode.Entity, storageNode.BuildingComp.BuildingType);
			}
			NodeList<GeneratorNode> nodeList2 = Service.Get<EntityController>().GetNodeList<GeneratorNode>();
			for (GeneratorNode generatorNode = nodeList2.Head; generatorNode != null; generatorNode = generatorNode.Next)
			{
				storageEffects.UpdateFillState(generatorNode.Entity, generatorNode.BuildingComp.BuildingType);
			}
		}

		private void UpdateStorageEffectsOnBuildingChange(Entity entity, BuildingTypeVO buildingVO)
		{
			if (buildingVO.Type == BuildingType.Storage)
			{
				this.UpdateStorageEffectsOnStorages(buildingVO.Currency);
				return;
			}
			if (buildingVO.Type == BuildingType.Resource)
			{
				Service.Get<StorageEffects>().UpdateFillState(entity, buildingVO);
			}
		}

		private void UpdateStorageEffectsOnStorages(CurrencyType currencyType)
		{
			StorageEffects storageEffects = Service.Get<StorageEffects>();
			NodeList<StorageNode> nodeList = Service.Get<EntityController>().GetNodeList<StorageNode>();
			for (StorageNode storageNode = nodeList.Head; storageNode != null; storageNode = storageNode.Next)
			{
				if (storageNode.BuildingComp.BuildingType.Currency == currencyType)
				{
					storageEffects.UpdateFillState(storageNode.Entity, storageNode.BuildingComp.BuildingType);
				}
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			Type previousStateType = Service.Get<GameStateMachine>().PreviousStateType;
			if (id <= EventId.WorldLoadComplete)
			{
				if (id != EventId.BuildingConstructed)
				{
					if (id == EventId.WorldLoadComplete)
					{
						Service.Get<CurrencyEffects>().Cleanup();
						if (currentState is ApplicationLoadState || currentState is HomeState)
						{
							if (!this.entityController.IsViewSystemSet<GeneratorSystem>())
							{
								this.entityController.AddViewSystem(new GeneratorSystem(), 2070, 65535);
								Service.Get<CurrencyEffects>().InitializeEffects("setupTypeCollection");
							}
						}
						else if (currentState is BattleStartState)
						{
							Service.Get<CurrencyEffects>().InitializeEffects("setupTypeLooting");
						}
					}
				}
				else
				{
					ContractEventData contractEventData = (ContractEventData)cookie;
					this.UpdateStorageEffectsOnBuildingChange(contractEventData.Entity, contractEventData.BuildingVO);
				}
			}
			else if (id != EventId.GameStateChanged)
			{
				if (id != EventId.InventoryResourceUpdated)
				{
					if (id == EventId.PlanetRelocateStarted && this.entityController.IsViewSystemSet<GeneratorSystem>())
					{
						this.entityController.RemoveViewSystem<GeneratorSystem>();
					}
				}
				else if (currentState is HomeState || currentState is EditBaseState || currentState is ApplicationLoadState)
				{
					string text = cookie as string;
					if (text == "credits")
					{
						this.UpdateStorageEffectsOnStorages(CurrencyType.Credits);
					}
					else if (text == "materials")
					{
						this.UpdateStorageEffectsOnStorages(CurrencyType.Materials);
					}
					else if (text == "contraband")
					{
						this.UpdateStorageEffectsOnStorages(CurrencyType.Contraband);
					}
				}
			}
			else if (!(currentState is HomeState) && !(currentState is IntroCameraState) && !(currentState is GalaxyState))
			{
				if (this.entityController.IsViewSystemSet<GeneratorSystem>())
				{
					this.entityController.RemoveViewSystem<GeneratorSystem>();
				}
			}
			else if (previousStateType == typeof(EditBaseState))
			{
				this.entityController.AddViewSystem(new GeneratorSystem(), 2070, 65535);
				Service.Get<CurrencyEffects>().PlaceEffects();
			}
			else if ((previousStateType == typeof(BattleStartState) || previousStateType == typeof(WarBoardState)) && currentState is HomeState)
			{
				this.entityController.AddViewSystem(new GeneratorSystem(), 2070, 65535);
				Service.Get<CurrencyEffects>().PlaceEffects();
			}
			return EatResponse.NotEaten;
		}

		protected internal CurrencyController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).CalculateAccruedCurrency((Building)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).CalculateGeneratorFillTimeRemaining((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).CalculateTimeUntilAllGeneratorsFull());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).CanStoreCollectionAmountFromGenerator((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).CollectCurrency((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).CollectCurrencyFromGenerator((Entity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).CurrencyPerHour((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).CurrencyPerSecond((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).ForceCollectAccruedCurrencyForUpgrade((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).HandleUnableToCollect((CurrencyType)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).IsGeneratorCollectable((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).IsGeneratorThresholdMet((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).OnCollectCurrency((Entity)GCHandledObjects.GCHandleToObject(*args), (BuildingComponent)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).TryCollectCurrencyOnSelection((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).UpdateAccruedCurrencyForView((Building)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).UpdateAllStorageEffects();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).UpdateGeneratorAccruedCurrency((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).UpdateStorageEffectsAfterCollection((Entity)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).UpdateStorageEffectsOnBuildingChange((Entity)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((CurrencyController)GCHandledObjects.GCHandleToObject(instance)).UpdateStorageEffectsOnStorages((CurrencyType)(*(int*)args));
			return -1L;
		}
	}
}
