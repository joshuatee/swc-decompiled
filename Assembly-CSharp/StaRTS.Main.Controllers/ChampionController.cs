using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.Main.Controllers.CombatTriggers;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class ChampionController : IEventObserver
	{
		private const uint EFFECTIVELY_NEVER_SECONDS = 1800u;

		private const string REPAIR_PURCHASE_CONTEXT_NAME = "repair";

		private TroopTypeVO repairChampionType;

		private const float CHAMPION_INITIAL_ROTATION = 180f;

		public ChampionController()
		{
			Service.Set<ChampionController>(this);
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ExitEditMode, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingConstructed, EventPriority.AfterDefault);
			eventManager.RegisterObserver(this, EventId.BuildingLevelUpgraded);
			eventManager.RegisterObserver(this, EventId.BuildingStartedUpgrading);
			eventManager.RegisterObserver(this, EventId.BuildingCancelled);
			eventManager.RegisterObserver(this, EventId.TroopViewReady);
			eventManager.RegisterObserver(this, EventId.ChampionRepaired);
			eventManager.RegisterObserver(this, EventId.ExitBaseLayoutToolMode);
			eventManager.RegisterObserver(this, EventId.TargetedBundleChampionRedeemed);
		}

		private void CreateChampionsOnPlatforms()
		{
			NodeList<ChampionPlatformNode> championPlatformNodeList = Service.Get<BuildingLookupController>().ChampionPlatformNodeList;
			for (ChampionPlatformNode championPlatformNode = championPlatformNodeList.Head; championPlatformNode != null; championPlatformNode = championPlatformNode.Next)
			{
				SmartEntity smartEntity = (SmartEntity)championPlatformNode.Entity;
				BuildingComponent buildingComp = smartEntity.BuildingComp;
				if (!ContractUtils.IsBuildingConstructing(smartEntity))
				{
					BuildingTypeVO buildingType = buildingComp.BuildingType;
					TroopTypeVO championType = this.FindChampionTypeIfPlatform(buildingType);
					if (this.FindChampionEntity(championType) == null)
					{
						this.CreateChampionEntity(championType, smartEntity);
					}
				}
			}
		}

		public TroopTypeVO FindChampionTypeIfPlatform(BuildingTypeVO buildingType)
		{
			if (buildingType == null || buildingType.Type != BuildingType.ChampionPlatform)
			{
				return null;
			}
			string uid = buildingType.Uid;
			IDataController dataController = Service.Get<IDataController>();
			TroopTypeVO troopTypeVO = null;
			if (buildingType.LinkedUnit != null)
			{
				troopTypeVO = dataController.Get<TroopTypeVO>(buildingType.LinkedUnit);
			}
			if (troopTypeVO == null)
			{
				Service.Get<StaRTSLogger>().Error("No champion found for platform builing " + uid);
			}
			return troopTypeVO;
		}

		private SmartEntity CreateChampionEntity(TroopTypeVO championType, SmartEntity building)
		{
			TransformComponent transformComp = building.TransformComp;
			SmartEntity smartEntity = Service.Get<EntityFactory>().CreateChampionEntity(championType, new IntPosition(transformComp.CenterGridX(), transformComp.CenterGridZ()));
			Service.Get<BoardController>().Board.AddChild(smartEntity.BoardItemComp.BoardItem, transformComp.X, transformComp.Z, null, false);
			Service.Get<EntityController>().AddEntity(smartEntity);
			return smartEntity;
		}

		private SmartEntity CreateDefensiveChampionEntityForBattle(TroopTypeVO championType, SmartEntity building)
		{
			TransformComponent transformComp = building.TransformComp;
			IntPosition boardPosition = new IntPosition(transformComp.CenterGridX(), transformComp.CenterGridZ());
			return Service.Get<TroopController>().SpawnChampion(championType, TeamType.Defender, boardPosition);
		}

		private void AddChampionToInventoryIfAlive(SmartEntity building, bool createEntity)
		{
			BuildingTypeVO buildingType = building.BuildingComp.BuildingType;
			TroopTypeVO troopTypeVO = this.FindChampionTypeIfPlatform(buildingType);
			if (troopTypeVO == null)
			{
				return;
			}
			if (createEntity)
			{
				this.CreateChampionEntity(troopTypeVO, building);
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			currentPlayer.AddChampionToInventoryIfAlive(troopTypeVO.Uid);
		}

		private void HandleChampionPlatformUpgradeStarted(SmartEntity building)
		{
			BuildingTypeVO buildingType = building.BuildingComp.BuildingType;
			TroopTypeVO troopTypeVO = this.FindChampionTypeIfPlatform(buildingType);
			if (troopTypeVO == null)
			{
				return;
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			currentPlayer.RemoveChampionFromInventory(troopTypeVO.Uid);
		}

		public bool IsChampionAvailable(SmartEntity building)
		{
			BuildingTypeVO buildingType = building.BuildingComp.BuildingType;
			TroopTypeVO troopTypeVO = this.FindChampionTypeIfPlatform(buildingType);
			return troopTypeVO != null && this.PlayerHasChampion(troopTypeVO);
		}

		private bool PlayerHasChampion(TroopTypeVO championType)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			return currentPlayer.Inventory.Champion.GetItemAmount(championType.Uid) > 0;
		}

		private void UpgradeChampionToMatchPlatform(SmartEntity building)
		{
			BuildingTypeVO buildingType = building.BuildingComp.BuildingType;
			TroopTypeVO troopTypeVO = this.FindChampionTypeIfPlatform(buildingType);
			if (troopTypeVO == null)
			{
				return;
			}
			BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
			BuildingTypeVO nextLevel = buildingUpgradeCatalog.GetNextLevel(buildingType);
			TroopTypeVO troopTypeVO2 = this.FindChampionTypeIfPlatform(nextLevel);
			if (troopTypeVO2 == null)
			{
				return;
			}
			this.AddChampionToInventoryIfAlive(building, false);
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			UnlockedLevelData.UpgradeTroopsOrStarshipsInventory(currentPlayer.Inventory.Champion, false, troopTypeVO2.UpgradeGroup, troopTypeVO2.Uid);
			this.DestroyChampionEntity(troopTypeVO);
			if (Service.Get<GameStateMachine>().CurrentState is HomeState)
			{
				this.CreateChampionEntity(troopTypeVO2, building);
			}
		}

		private void DestroyChampionEntity(TroopTypeVO championType)
		{
			SmartEntity smartEntity = this.FindChampionEntity(championType);
			if (smartEntity != null)
			{
				Service.Get<EntityFactory>().DestroyEntity(smartEntity, true, false);
			}
		}

		public void DestroyAllChampionEntities()
		{
			NodeList<ChampionPlatformNode> championPlatformNodeList = Service.Get<BuildingLookupController>().ChampionPlatformNodeList;
			for (ChampionPlatformNode championPlatformNode = championPlatformNodeList.Head; championPlatformNode != null; championPlatformNode = championPlatformNode.Next)
			{
				SmartEntity smartEntity = (SmartEntity)championPlatformNode.Entity;
				BuildingComponent buildingComp = smartEntity.BuildingComp;
				BuildingTypeVO buildingType = buildingComp.BuildingType;
				TroopTypeVO championType = this.FindChampionTypeIfPlatform(buildingType);
				this.DestroyChampionEntity(championType);
			}
		}

		public SmartEntity FindChampionEntity(TroopTypeVO championType)
		{
			NodeList<ChampionNode> nodeList = Service.Get<EntityController>().GetNodeList<ChampionNode>();
			for (ChampionNode championNode = nodeList.Head; championNode != null; championNode = championNode.Next)
			{
				if (championNode.ChampionComp.ChampionType == championType)
				{
					return (SmartEntity)championNode.Entity;
				}
			}
			return null;
		}

		private SmartEntity FindPlatformForChampion(TroopTypeVO champion)
		{
			NodeList<ChampionPlatformNode> nodeList = Service.Get<EntityController>().GetNodeList<ChampionPlatformNode>();
			for (ChampionPlatformNode championPlatformNode = nodeList.Head; championPlatformNode != null; championPlatformNode = championPlatformNode.Next)
			{
				if (championPlatformNode.BuildingComp.BuildingType.LinkedUnit == champion.Uid)
				{
					return (SmartEntity)championPlatformNode.Entity;
				}
			}
			Service.Get<StaRTSLogger>().Error("No platform building found for champion " + champion.Uid);
			return null;
		}

		private void OnPayMeForCurrencyResult(object result, object cookie)
		{
			if (GameUtils.HandleSoftCurrencyFlow(result, cookie))
			{
				if (!PayMeScreen.ShowIfNoFreeDroids(new OnScreenModalResult(this.OnPayMeForDroidResult), null))
				{
					this.ConfirmRepair();
					return;
				}
			}
			else
			{
				this.repairChampionType = null;
			}
		}

		private void OnPayMeForDroidResult(object result, object cookie)
		{
			if (result != null)
			{
				this.ConfirmRepair();
				return;
			}
			this.repairChampionType = null;
		}

		private void ConfirmRepair()
		{
			SmartEntity smartEntity = this.FindPlatformForChampion(this.repairChampionType);
			Service.Get<ISupportController>().StartChampionRepair(this.repairChampionType, smartEntity);
			if (this.repairChampionType.TrainingTime > 0)
			{
				Service.Get<UXController>().HUD.ShowContextButtons(smartEntity);
			}
			this.repairChampionType = null;
		}

		public void StartChampionRepair(SmartEntity building)
		{
			BuildingTypeVO buildingType = building.BuildingComp.BuildingType;
			this.repairChampionType = this.FindChampionTypeIfPlatform(buildingType);
			if (this.repairChampionType == null)
			{
				return;
			}
			int credits = this.repairChampionType.Credits;
			int materials = this.repairChampionType.Materials;
			int contraband = this.repairChampionType.Contraband;
			if (credits != 0 || materials != 0 || contraband != 0)
			{
				string purchaseContext = string.Format("{0}|{1}|{2}|{3}", new object[]
				{
					StringUtils.ToLowerCaseUnderscoreSeperated(this.repairChampionType.Type.ToString()),
					this.repairChampionType.TroopID,
					this.repairChampionType.Lvl,
					"repair"
				});
				if (PayMeScreen.ShowIfNotEnoughCurrency(credits, materials, contraband, purchaseContext, new OnScreenModalResult(this.OnPayMeForCurrencyResult)))
				{
					return;
				}
			}
			if (PayMeScreen.ShowIfNoFreeDroids(new OnScreenModalResult(this.OnPayMeForDroidResult), null))
			{
				return;
			}
			this.ConfirmRepair();
		}

		public virtual EatResponse OnEvent(EventId id, object cookie)
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			bool flag = currentState is HomeState;
			bool flag2 = currentState is WarBoardState;
			bool flag3 = currentState is ApplicationLoadState;
			if (id <= EventId.BuildingLevelUpgraded)
			{
				if (id <= EventId.BuildingStartedUpgrading)
				{
					if (id != EventId.BuildingCancelled)
					{
						if (id == EventId.BuildingStartedUpgrading)
						{
							this.HandleChampionPlatformUpgradeStarted((SmartEntity)cookie);
						}
					}
					else
					{
						ContractEventData contractEventData = (ContractEventData)cookie;
						this.AddChampionToInventoryIfAlive((SmartEntity)contractEventData.Entity, false);
					}
				}
				else if (id != EventId.ChampionRepaired)
				{
					if (id != EventId.TroopViewReady)
					{
						if (id == EventId.BuildingLevelUpgraded)
						{
							ContractEventData contractEventData = (ContractEventData)cookie;
							this.UpgradeChampionToMatchPlatform((SmartEntity)contractEventData.Entity);
						}
					}
					else if (flag | flag2 | flag3)
					{
						EntityViewParams entityViewParams = cookie as EntityViewParams;
						SmartEntity entity = entityViewParams.Entity;
						if (entity.ChampionComp != null)
						{
							bool underRepair = this.IsChampionBroken(entity.ChampionComp);
							Service.Get<EntityRenderController>().UpdateChampionAnimationStateInHomeOrWarBoardMode(entity, underRepair);
						}
					}
				}
				else if (flag && cookie != null)
				{
					ContractEventData contractEventData2 = cookie as ContractEventData;
					SmartEntity smartEntity = contractEventData2.Entity as SmartEntity;
					TroopTypeVO troopTypeVO = this.FindChampionTypeIfPlatform(smartEntity.BuildingComp.BuildingType);
					if (troopTypeVO != null)
					{
						SmartEntity entity2 = this.FindChampionEntity(troopTypeVO);
						Service.Get<EntityRenderController>().UpdateChampionAnimationStateInHomeOrWarBoardMode(entity2, false);
					}
				}
			}
			else
			{
				if (id <= EventId.WorldLoadComplete)
				{
					if (id != EventId.BuildingConstructed)
					{
						if (id != EventId.WorldLoadComplete)
						{
							return EatResponse.NotEaten;
						}
					}
					else
					{
						ContractEventData contractEventData = (ContractEventData)cookie;
						this.AddChampionToInventoryIfAlive((SmartEntity)contractEventData.Entity, flag);
						if (flag)
						{
							Service.Get<BuildingTooltipController>().EnsureBuildingTooltip((SmartEntity)contractEventData.Entity);
							return EatResponse.NotEaten;
						}
						return EatResponse.NotEaten;
					}
				}
				else if (id != EventId.ExitEditMode && id != EventId.ExitBaseLayoutToolMode)
				{
					if (id != EventId.TargetedBundleChampionRedeemed)
					{
						return EatResponse.NotEaten;
					}
					SmartEntity building = (SmartEntity)cookie;
					Service.Get<PlayerValuesController>().RecalculateAll();
					this.AddChampionToInventoryIfAlive(building, flag);
					return EatResponse.NotEaten;
				}
				this.CreateChampionsOnPlatforms();
			}
			return EatResponse.NotEaten;
		}

		private bool IsChampionBroken(ChampionComponent championComp)
		{
			TroopTypeVO championType = championComp.ChampionType;
			SmartEntity selectedBuilding = this.FindPlatformForChampion(championType);
			return !this.PlayerHasChampion(championType) && !ContractUtils.IsBuildingUpgrading(selectedBuilding) && !ContractUtils.IsBuildingConstructing(selectedBuilding);
		}

		public void RegisterChampionPlatforms(CurrentBattle currentBattle)
		{
			SmartEntity smartEntity = null;
			TroopTypeVO troopTypeVO = null;
			bool flag = false;
			NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
			for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
			{
				flag = false;
				smartEntity = (SmartEntity)buildingNode.Entity;
				troopTypeVO = this.FindChampionTypeIfPlatform(smartEntity.BuildingComp.BuildingType);
				if (buildingNode.BuildingComp.BuildingType.Type == BuildingType.ChampionPlatform)
				{
					if (currentBattle.Type == BattleType.PveAttack || currentBattle.Type == BattleType.ClientBattle || currentBattle.Type == BattleType.PveBuffBase || currentBattle.Type == BattleType.PvpAttackSquadWar)
					{
						if (troopTypeVO != null && smartEntity != null)
						{
							this.AddDefensiveChampionToPlatfrom(smartEntity, troopTypeVO);
						}
					}
					else if (troopTypeVO != null && currentBattle.DisabledBuildings != null && !currentBattle.DisabledBuildings.Contains(buildingNode.BuildingComp.BuildingTO.Key))
					{
						if (!currentBattle.IsPvP())
						{
							goto IL_129;
						}
						if (currentBattle.DefenderChampionsAvailable != null)
						{
							using (Dictionary<string, int>.Enumerator enumerator = currentBattle.DefenderChampionsAvailable.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									KeyValuePair<string, int> current = enumerator.Current;
									if (current.get_Key() == troopTypeVO.Uid && current.get_Value() > 0)
									{
										flag = true;
									}
								}
								goto IL_182;
							}
							goto IL_129;
						}
						IL_182:
						if (flag)
						{
							this.AddDefensiveChampionToPlatfrom(smartEntity, troopTypeVO);
							goto IL_18D;
						}
						goto IL_18D;
						IL_129:
						if (currentBattle.Type != BattleType.PveDefend || ContractUtils.IsBuildingUpgrading(smartEntity) || ContractUtils.IsChampionRepairing(smartEntity))
						{
							goto IL_182;
						}
						Inventory inventory = Service.Get<CurrentPlayer>().Inventory;
						bool flag2 = inventory.Champion.HasItem(troopTypeVO.Uid) && inventory.Champion.GetItemAmount(troopTypeVO.Uid) > 0;
						if (flag2)
						{
							flag = true;
							goto IL_182;
						}
						goto IL_182;
					}
				}
				IL_18D:;
			}
		}

		public void AddDefensiveChampionToPlatfrom(SmartEntity championPlatform, TroopTypeVO champion)
		{
			SmartEntity smartEntity = this.CreateDefensiveChampionEntityForBattle(champion, championPlatform);
			if (smartEntity != null)
			{
				smartEntity.TransformComp.Rotation = 180f;
				Service.Get<EventManager>().SendEvent(EventId.AddDecalToTroop, smartEntity);
			}
			championPlatform.ChampionPlatformComp.DefensiveChampion = smartEntity;
			Service.Get<CombatTriggerManager>().RegisterTrigger(new DefendedBuildingCombatTrigger(championPlatform, CombatTriggerType.Area, false, champion, 1, true, 0u, 1800u));
		}

		protected internal ChampionController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ChampionController)GCHandledObjects.GCHandleToObject(instance)).AddChampionToInventoryIfAlive((SmartEntity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ChampionController)GCHandledObjects.GCHandleToObject(instance)).AddDefensiveChampionToPlatfrom((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (TroopTypeVO)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ChampionController)GCHandledObjects.GCHandleToObject(instance)).ConfirmRepair();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionController)GCHandledObjects.GCHandleToObject(instance)).CreateChampionEntity((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ChampionController)GCHandledObjects.GCHandleToObject(instance)).CreateChampionsOnPlatforms();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionController)GCHandledObjects.GCHandleToObject(instance)).CreateDefensiveChampionEntityForBattle((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ChampionController)GCHandledObjects.GCHandleToObject(instance)).DestroyAllChampionEntities();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ChampionController)GCHandledObjects.GCHandleToObject(instance)).DestroyChampionEntity((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionController)GCHandledObjects.GCHandleToObject(instance)).FindChampionEntity((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionController)GCHandledObjects.GCHandleToObject(instance)).FindChampionTypeIfPlatform((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionController)GCHandledObjects.GCHandleToObject(instance)).FindPlatformForChampion((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ChampionController)GCHandledObjects.GCHandleToObject(instance)).HandleChampionPlatformUpgradeStarted((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionController)GCHandledObjects.GCHandleToObject(instance)).IsChampionAvailable((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionController)GCHandledObjects.GCHandleToObject(instance)).IsChampionBroken((ChampionComponent)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ChampionController)GCHandledObjects.GCHandleToObject(instance)).OnPayMeForCurrencyResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((ChampionController)GCHandledObjects.GCHandleToObject(instance)).OnPayMeForDroidResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionController)GCHandledObjects.GCHandleToObject(instance)).PlayerHasChampion((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((ChampionController)GCHandledObjects.GCHandleToObject(instance)).RegisterChampionPlatforms((CurrentBattle)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((ChampionController)GCHandledObjects.GCHandleToObject(instance)).StartChampionRepair((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((ChampionController)GCHandledObjects.GCHandleToObject(instance)).UpgradeChampionToMatchPlatform((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
