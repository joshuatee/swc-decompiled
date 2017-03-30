using Net.RichardLord.Ash.Core;
using StaRTS.FX;
using StaRTS.GameBoard;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Objectives;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.TransferObjects;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.World;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace StaRTS.Main.Controllers
{
	public class BuildingController : IEventObserver
	{
		private const int GRID_RADIUS = 2;

		private BuildingSelector buildingSelector;

		private BuildingMover buildingMover;

		private EntityShaderSwapper entityShaderSwapper;

		private bool purchasingBuilding;

		private int purchasingStampable;

		private int[] stampingCellsX;

		private int[] stampingCellsZ;

		private int stampingValidCount;

		private BuildingTypeVO purchasingBuildingType;

		private int[,,] edgeRotations;

		public bool UnstashStampingEnabled
		{
			get;
			set;
		}

		public Entity SelectedBuilding
		{
			get
			{
				if (this.buildingSelector == null)
				{
					return null;
				}
				return this.buildingSelector.SelectedBuilding;
			}
			set
			{
				if (value == null)
				{
					this.buildingSelector.EnsureDeselectSelectedBuilding();
				}
				else
				{
					this.buildingSelector.SelectBuilding(value, Vector3.zero);
				}
			}
		}

		public int NumSelectedBuildings
		{
			get
			{
				if (this.buildingSelector.SelectedBuilding != null)
				{
					return this.buildingSelector.AdditionalSelectedBuildings.Count + 1;
				}
				return 0;
			}
		}

		public bool IsPurchasing
		{
			get
			{
				return this.purchasingBuilding;
			}
		}

		public Entity PurchasingBuilding
		{
			get
			{
				return (!this.purchasingBuilding) ? null : this.buildingSelector.SelectedBuilding;
			}
		}

		public BuildingController()
		{
			Service.Set<BuildingController>(this);
			this.buildingSelector = new BuildingSelector();
			this.buildingMover = new BuildingMover(this.buildingSelector);
			this.entityShaderSwapper = new EntityShaderSwapper();
			this.purchasingBuilding = false;
			this.purchasingStampable = 0;
			this.purchasingBuildingType = null;
			this.UnstashStampingEnabled = false;
			this.stampingCellsX = new int[2];
			this.stampingCellsZ = new int[2];
			this.ResetStampLocations();
			this.edgeRotations = new int[4, 2, 2];
			int num = 1;
			int num2 = 0;
			for (int i = 0; i < 4; i++)
			{
				this.edgeRotations[i, 0, 0] = num;
				this.edgeRotations[i, 0, 1] = -num2;
				this.edgeRotations[i, 1, 0] = num2;
				this.edgeRotations[i, 1, 1] = num;
				int num3 = num;
				num = -num2;
				num2 = num3;
			}
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.BuildingLevelUpgraded, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.BuildingSwapped, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ClearableCleared, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ContractCompletedForStoryAction, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ContractCompleted, EventPriority.Default);
		}

		private void OnHQUpgraded()
		{
			if (Service.Get<ScreenController>().GetHighestLevelScreen<HQCelebScreen>() == null)
			{
				ScreenBase screenBase = new HQCelebScreen();
				screenBase.IsAlwaysOnTop = true;
				Service.Get<ScreenController>().AddScreen(screenBase, true, true);
			}
		}

		public void EnterSelectMode()
		{
			this.buildingMover.Enabled = false;
			this.buildingSelector.Enabled = true;
		}

		public void EnterMoveMode(bool autoLiftSelectedBuilding)
		{
			this.buildingSelector.Enabled = false;
			this.buildingMover.Enabled = true;
			if (autoLiftSelectedBuilding && GameUtils.IsBuildingMovable(this.SelectedBuilding))
			{
				this.buildingMover.AutoLiftSelectedBuilding();
			}
		}

		public void SelectAdjacentWalls(Entity building)
		{
			this.buildingSelector.SelectAdjacentWalls(building);
		}

		public void RotateCurrentSelection(Entity building)
		{
			this.buildingMover.RotateSelectedBuildings(building);
		}

		public BuildingMover GetBuildingMoverForCombineMeshManager()
		{
			return this.buildingMover;
		}

		public void ExitAllModes()
		{
			this.buildingMover.EnsureLoweredLiftedBuilding();
			this.buildingMover.Enabled = false;
			this.buildingSelector.Enabled = false;
		}

		public void CancelEditModeTimer()
		{
			this.buildingSelector.CancelEditModeTimer();
		}

		public void UpdateBuildingHighlightForPerks(Entity building)
		{
			if (building == null)
			{
				return;
			}
			bool flag = ContractUtils.IsBuildingUpgrading(building);
			if (ContractUtils.IsBuildingConstructing(building) || flag)
			{
				return;
			}
			PerkManager perkManager = Service.Get<PerkManager>();
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			BuildingComponent buildingComp = ((SmartEntity)building).BuildingComp;
			if ((currentState is ApplicationLoadState || currentState is HomeState || currentState is EditBaseState || currentState is BaseLayoutToolState) && buildingComp != null)
			{
				BuildingTypeVO buildingType = buildingComp.BuildingType;
				if (perkManager.IsPerkAppliedToBuilding(buildingType))
				{
					this.entityShaderSwapper.HighlightForPerk(building);
				}
				else
				{
					bool flag2 = this.entityShaderSwapper.ResetToOriginal(building);
					if (flag2)
					{
						Service.Get<EventManager>().SendEvent(EventId.ShaderResetOnEntity, building);
					}
				}
			}
		}

		public void HighlightBuilding(Entity building)
		{
			this.entityShaderSwapper.Outline(building);
		}

		public void ClearBuildingHighlight(Entity building)
		{
			this.entityShaderSwapper.ResetToOriginal(building);
		}

		public List<Entity> GetAdditionalSelectedBuildings()
		{
			return this.buildingSelector.AdditionalSelectedBuildings;
		}

		public void RedrawRadiusForSelectedBuilding()
		{
			if (this.SelectedBuilding != null)
			{
				this.buildingSelector.RedrawRadiusView(this.SelectedBuilding);
			}
		}

		public void EnsureDeselectSelectedBuilding()
		{
			this.buildingSelector.EnsureDeselectSelectedBuilding();
		}

		public void EnsureLoweredLiftedBuilding()
		{
			this.buildingMover.EnsureLoweredLiftedBuilding();
		}

		public bool IsLifted(Entity building)
		{
			return this.buildingMover.Lifted && this.buildingSelector.IsPartOfSelection(building);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			switch (id)
			{
			case EventId.BuildingLevelUpgraded:
			case EventId.BuildingSwapped:
			{
				Entity entity = (cookie as ContractEventData).Entity;
				if (entity != null)
				{
					bool flag = this.buildingSelector.IsPartOfSelection(entity);
					if (entity != null && flag)
					{
						this.buildingSelector.DeselectSelectedBuilding();
					}
					Entity entity2 = this.ReplaceBuildingAfterTOChange(entity);
					if (flag && entity2 != null)
					{
						this.buildingSelector.SelectBuilding(entity2, Vector3.zero);
					}
					Service.Get<AchievementController>().TryUnlockAchievementById(AchievementType.BuildingLevel, entity2.Get<BuildingComponent>().BuildingType.Uid);
					this.CheckStarportFullness(entity2);
				}
				break;
			}
			case EventId.BuildingConstructed:
				this.CheckStarportFullness(((ContractEventData)cookie).Entity);
				break;
			default:
				if (id != EventId.ContractCompleted)
				{
					if (id != EventId.ContractCompletedForStoryAction)
					{
						if (id == EventId.ClearableCleared)
						{
							Entity entity3 = (cookie as ContractEventData).Entity;
							if (entity3 != null && this.buildingSelector.SelectedBuilding == entity3)
							{
								this.buildingSelector.DeselectSelectedBuilding();
							}
							Service.Get<EntityFactory>().DestroyEntity(entity3, true, true);
						}
					}
					else
					{
						ContractTO contractTO = (ContractTO)cookie;
						if (contractTO.ContractType == ContractType.Upgrade)
						{
							BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(contractTO.Uid);
							if (buildingTypeVO.Type == BuildingType.HQ)
							{
								this.OnHQUpgraded();
							}
						}
					}
				}
				else
				{
					ContractEventData contractEventData = cookie as ContractEventData;
					ContractType contractType = contractEventData.Contract.ContractTO.ContractType;
					if (contractType == ContractType.Upgrade || contractType == ContractType.Build)
					{
						BuildingTypeVO buildingTypeVO2 = Service.Get<IDataController>().Get<BuildingTypeVO>(contractEventData.Contract.ProductUid);
						if (buildingTypeVO2.Type == BuildingType.NavigationCenter)
						{
							Service.Get<CurrentPlayer>().AddUnlockedPlanet(contractEventData.Contract.Tag);
						}
						if (buildingTypeVO2.Type == BuildingType.HQ && buildingTypeVO2.Lvl >= GameConstants.OBJECTIVES_UNLOCKED)
						{
							Service.Get<ObjectiveManager>().RefreshFromServer();
						}
					}
				}
				break;
			}
			return EatResponse.NotEaten;
		}

		private void CheckStarportFullness(Entity entity)
		{
			if (entity.Get<BuildingComponent>().BuildingType.Type == BuildingType.Starport)
			{
				StorageSpreadUtils.UpdateAllStarportFullnessMeters();
			}
		}

		public Entity ReplaceBuildingAfterTOChange(Entity building)
		{
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			Building buildingTO = buildingComponent.BuildingTO;
			BoardItemComponent boardItemComponent = building.Get<BoardItemComponent>();
			BoardItem<Entity> boardItem = boardItemComponent.BoardItem;
			BoardCell<Entity> currentCell = boardItem.CurrentCell;
			int x = currentCell.X;
			int z = currentCell.Z;
			EntityFactory entityFactory = Service.Get<EntityFactory>();
			PostBattleRepairController postBattleRepairController = Service.Get<PostBattleRepairController>();
			if (postBattleRepairController.IsEntityInRepair(building))
			{
				postBattleRepairController.RemoveExistingRepair(building);
			}
			Entity entity = entityFactory.CreateBuildingEntity(buildingTO, true, true, true);
			Service.Get<CurrencyEffects>().TransferEffects(building, entity);
			Service.Get<MobilizationEffectsManager>().TransferEffects(building, entity);
			string uid = buildingTO.Uid;
			buildingTO.Uid = buildingComponent.BuildingType.Uid;
			entityFactory.DestroyEntity(building, true, true);
			buildingTO.Uid = uid;
			Service.Get<WorldController>().AddBuildingHelper(entity, x, z, true);
			Service.Get<EventManager>().SendEvent(EventId.BuildingReplaced, entity);
			return entity;
		}

		public void ResetStampLocations()
		{
			this.stampingCellsX[0] = 0;
			this.stampingCellsZ[0] = 0;
			this.stampingCellsX[1] = 0;
			this.stampingCellsZ[1] = 0;
			this.stampingValidCount = 0;
		}

		private void ChooseNextStampLocation(ref int cx, ref int cz)
		{
			if (this.stampingValidCount <= 0)
			{
				return;
			}
			cx = this.stampingCellsX[0];
			cz = this.stampingCellsZ[0];
			int num = cx;
			int num2 = cz;
			if (this.stampingValidCount > 1)
			{
				num -= this.stampingCellsX[1];
				num2 -= this.stampingCellsZ[1];
			}
			int num3 = (num < 0) ? -1 : 1;
			int num4 = (num2 < 0) ? -1 : 1;
			if (num3 * num >= num4 * num2)
			{
				cx += num3 * 1;
			}
			else
			{
				cz += num4 * 1;
			}
		}

		public void PrepareAndPurchaseNewBuilding(BuildingTypeVO buildingType)
		{
			this.buildingMover.EnsureLoweredLiftedBuilding();
			this.EnsureDeselectSelectedBuilding();
			GameStateMachine gameStateMachine = Service.Get<GameStateMachine>();
			if (!(gameStateMachine.CurrentState is EditBaseState))
			{
				gameStateMachine.SetState(new EditBaseState(false));
			}
			int stampableQuantity = 0;
			if (buildingType.Type == BuildingType.Wall)
			{
				BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
				int buildingMaxPurchaseQuantity = buildingLookupController.GetBuildingMaxPurchaseQuantity(buildingType, 0);
				int buildingPurchasedQuantity = buildingLookupController.GetBuildingPurchasedQuantity(buildingType);
				stampableQuantity = buildingMaxPurchaseQuantity - buildingPurchasedQuantity;
			}
			this.StartPurchaseBuilding(buildingType, stampableQuantity);
		}

		public void StartPurchaseBuilding(BuildingTypeVO buildingType, int stampableQuantity)
		{
			Service.Get<EventManager>().SendEvent(EventId.BuildingPurchaseModeStarted, null);
			Entity entity = Service.Get<EntityFactory>().CreateBuildingEntity(buildingType, true, true, true);
			Service.Get<Logger>().DebugFormat("Purchasing building type {0}, ID {1}, W/H {2}x{3}", new object[]
			{
				buildingType.Uid,
				entity.ID.ToString(),
				buildingType.SizeX.ToString(),
				buildingType.SizeY.ToString()
			});
			bool stampable = stampableQuantity != 0;
			this.buildingMover.OnStartPurchaseBuilding(entity, stampable);
			this.purchasingBuilding = true;
			Service.Get<UXController>().HUD.ShowContextButtons(entity);
			if (stampableQuantity >= 0)
			{
				this.purchasingStampable = stampableQuantity;
			}
			this.purchasingBuildingType = buildingType;
		}

		public bool PositionUnstashedBuilding(Entity buildingEntity, Position pos, bool stampingEnabled, bool panToBuilding, bool playLoweredSound)
		{
			this.UnstashStampingEnabled = stampingEnabled;
			return this.buildingMover.UnstashBuilding(buildingEntity, pos, this.UnstashStampingEnabled, panToBuilding, playLoweredSound);
		}

		public void PlaceRewardedBuilding(BuildingTypeVO buildingType)
		{
			Entity entity = Service.Get<EntityFactory>().CreateBuildingEntity(buildingType, true, true, true);
			Service.Get<Logger>().DebugFormat("Purchasing building type {0}, ID {1}, W/H {2}x{3}", new object[]
			{
				buildingType.Uid,
				entity.ID.ToString(),
				buildingType.SizeX.ToString(),
				buildingType.SizeY.ToString()
			});
			this.buildingMover.PlaceNewBuilding(entity);
			if (!string.IsNullOrEmpty(buildingType.LinkedUnit))
			{
				Service.Get<EventManager>().SendEvent(EventId.TargetedBundleChampionRedeemed, entity);
			}
		}

		public void DisableUnstashStampingState()
		{
			Service.Get<UXController>().MiscElementsManager.HideConfirmGroup();
			this.ResetStampLocations();
			this.UnstashStampingEnabled = false;
		}

		public bool FindStartingLocation(Entity building, out int boardX, out int boardZ, int cx, int cz, bool stampable)
		{
			BoardItemComponent boardItemComponent = building.Get<BoardItemComponent>();
			BoardItem<Entity> boardItem = boardItemComponent.BoardItem;
			Board<Entity> board = Service.Get<BoardController>().Board;
			if (stampable)
			{
				this.ChooseNextStampLocation(ref cx, ref cz);
			}
			bool checkSkirt = building.Get<BuildingComponent>().BuildingType.Type != BuildingType.Blocker;
			if (!board.CanOccupy(boardItem, cx, cz, checkSkirt))
			{
				Rand rand = Service.Get<Rand>();
				if (!stampable)
				{
					cx += rand.ViewRangeInt(-2, 3) * 1;
					cz += rand.ViewRangeInt(-2, 3) * 1;
				}
				for (int i = 1; i < 42; i++)
				{
					int num = rand.ViewRangeInt(0, 4);
					int num2 = i;
					for (int j = 0; j < 4; j++)
					{
						int num3 = (j + num) % 4;
						for (int k = -i; k < i; k++)
						{
							int num4 = cx + this.edgeRotations[num3, 0, 0] * num2 + this.edgeRotations[num3, 0, 1] * k;
							int num5 = cz + this.edgeRotations[num3, 1, 0] * num2 + this.edgeRotations[num3, 1, 1] * k;
							if (board.CanOccupy(boardItem, num4, num5, checkSkirt))
							{
								boardX = num4;
								boardZ = num5;
								return true;
							}
						}
					}
				}
			}
			boardX = cx;
			boardZ = cz;
			return false;
		}

		public bool FoundFirstEmptySpaceFor(BuildingTypeVO buildingData)
		{
			int num = 42;
			int num2 = 42;
			int num3 = 0 - num / 2;
			int num4 = 0 - num2 / 2;
			int num5 = num / 2;
			int num6 = num2 / 2;
			int sizeX = buildingData.SizeX;
			int sizeY = buildingData.SizeY;
			for (int i = num4 / 2; i <= num6 / 2; i++)
			{
				for (int j = num3 / 2; j <= num5 / 2; j++)
				{
					int num7 = j + sizeX;
					int num8 = i + sizeY;
					if (j >= num3 && i >= num4 && num7 - 1 <= num5 && num8 - 1 <= num6)
					{
						bool flag = false;
						NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
						for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
						{
							if (this.isOverlapping(buildingNode.BuildingComp.BuildingTO, buildingData, j, i))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool isOverlapping(Building thisBuilding, BuildingTypeVO buildingType, int x, int z)
		{
			BuildingTypeVO optional = Service.Get<IDataController>().GetOptional<BuildingTypeVO>(thisBuilding.Uid);
			int num = optional.SizeX;
			int num2 = optional.SizeY;
			int num3 = buildingType.SizeX;
			int num4 = buildingType.SizeY;
			BuildingType type = optional.Type;
			BuildingType type2 = buildingType.Type;
			bool flag = type == BuildingType.Wall || type == BuildingType.Trap;
			bool flag2 = type2 == BuildingType.Wall || type2 == BuildingType.Trap;
			if (!flag || !flag2)
			{
				if (num == 1 && num2 == 1)
				{
					num++;
					num2++;
				}
				if (num3 == 1 && num4 == 1)
				{
					num3++;
					num4++;
				}
			}
			return thisBuilding.X < x + num3 && thisBuilding.X + num > x && thisBuilding.Z < z + num4 && thisBuilding.Z + num2 > z;
		}

		public void StartClearingSelectedBuilding()
		{
			Entity selectedBuilding = this.buildingSelector.SelectedBuilding;
			BuildingTypeVO buildingType = selectedBuilding.Get<BuildingComponent>().BuildingType;
			if (buildingType.Type == BuildingType.Clearable)
			{
				int credits = buildingType.Credits;
				int materials = buildingType.Materials;
				int contraband = buildingType.Contraband;
				string value = StringUtils.ToLowerCaseUnderscoreSeperated(buildingType.Type.ToString());
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(value);
				stringBuilder.Append("|");
				stringBuilder.Append(buildingType.BuildingID);
				stringBuilder.Append("|");
				stringBuilder.Append(buildingType.SizeX * buildingType.SizeY);
				if (PayMeScreen.ShowIfNotEnoughCurrency(credits, materials, contraband, stringBuilder.ToString(), new OnScreenModalResult(this.OnPayMeForCurrencyResult)))
				{
					return;
				}
				if (PayMeScreen.ShowIfNoFreeDroids(new OnScreenModalResult(this.OnPayMeForDroidResult), selectedBuilding))
				{
					return;
				}
				this.ConfirmClearingBuilding(selectedBuilding);
			}
		}

		private void ConfirmClearingBuilding(Entity building)
		{
			Service.Get<ISupportController>().StartClearingBuilding(building);
			Service.Get<UXController>().HUD.ShowContextButtons(building);
		}

		private void OnPayMeForCurrencyResult(object result, object cookie)
		{
			Entity selectedBuilding = this.buildingSelector.SelectedBuilding;
			if (GameUtils.HandleSoftCurrencyFlow(result, cookie) && !PayMeScreen.ShowIfNoFreeDroids(new OnScreenModalResult(this.OnPayMeForDroidResult), selectedBuilding))
			{
				this.ConfirmClearingBuilding(selectedBuilding);
			}
		}

		private void OnPayMeForDroidResult(object result, object cookie)
		{
			if (result != null)
			{
				this.ConfirmClearingBuilding(cookie as Entity);
			}
		}

		public BoardCell<Entity> OnLowerLiftedBuilding(Entity building, int x, int z, bool confirmPurchase, ref BuildingTypeVO stampBuilding, string tag)
		{
			BoardCell<Entity> boardCell;
			if (this.purchasingBuilding)
			{
				this.purchasingBuilding = false;
				if (!confirmPurchase)
				{
					this.ResetStampLocations();
					Service.Get<EventManager>().SendEvent(EventId.BuildingPurchaseCanceled, null);
					return null;
				}
				BuildingTypeVO buildingType = building.Get<BuildingComponent>().BuildingType;
				int credits = buildingType.Credits;
				int materials = buildingType.Materials;
				int contraband = buildingType.Contraband;
				GameUtils.SpendCurrency(credits, materials, contraband, false);
				Service.Get<EventManager>().SendEvent(EventId.BuildingPurchaseConfirmed, null);
				boardCell = Service.Get<WorldController>().AddBuildingHelper(building, x, z, false);
				if (boardCell != null)
				{
					BuildingComponent buildingComponent = building.Get<BuildingComponent>();
					Service.Get<CurrentPlayer>().Map.Buildings.Add(buildingComponent.BuildingTO);
					if (this.purchasingStampable == 0)
					{
						Service.Get<ISupportController>().StartBuildingConstruct(buildingType, building, x, z, tag);
						Service.Get<EventManager>().SendEvent(EventId.BuildingPurchaseSuccess, building);
					}
					else
					{
						Service.Get<EventManager>().SendEvent(EventId.BuildingPurchaseSuccess, building);
						Service.Get<ISupportController>().InstantBuildingConstruct(buildingType, building, x, z, tag);
						if (--this.purchasingStampable > 0)
						{
							stampBuilding = this.purchasingBuildingType;
							this.SaveLastStampLocation(boardCell.X, boardCell.Z);
						}
					}
				}
			}
			else
			{
				boardCell = Service.Get<WorldController>().MoveBuildingWithinBoard(building, x, z);
			}
			return boardCell;
		}

		public void SaveLastStampLocation(int x, int z)
		{
			this.stampingCellsX[1] = this.stampingCellsX[0];
			this.stampingCellsZ[1] = this.stampingCellsZ[0];
			this.stampingCellsX[0] = x;
			this.stampingCellsZ[0] = z;
			this.stampingValidCount++;
		}
	}
}
