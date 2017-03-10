using Net.RichardLord.Ash.Core;
using StaRTS.Externals.DMOAnalytics;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class DeployableInfoScreen : SelectedBuildingScreen, IViewFrameTimeObserver
	{
		private const int NUM_DETAIL_LEFT_ROWS = 4;

		private const string GO_TO_PLANET_ACTION = "planet";

		private const string GO_TO_GALAXY_ACTION = "galaxy";

		protected const string BUTTON_BACK = "BtnBack";

		private const string BUTTON_TROOP_NEXT = "BtnTroopNext";

		private const string BUTTON_TROOP_PREV = "BtnTroopPrev";

		private const string LABEL_TROOP_NAME = "DialogBldgUpgradeTitle";

		private const string LABEL_TROOP_INFO = "LabelTroopInfo";

		private const string UPGRADE_TIME_GROUP = "ContainerUpgradeTime";

		private const string LABEL_UPGRADE_TIME = "LabelUpgradeTime";

		private const string LABEL_UPGRADE_TIME_STATIC = "LabelUpgradeTimeStatic";

		private const string UPGRADE_REQUIREMENT_LABEL = "LabelRequirement";

		private const string BUTTON_PURCHASE = "ButtonPrimaryAction";

		private const string BUTTON_FINISH = "BtnFinish";

		protected const string CONTEXT_COST_GROUP = "Cost";

		private const string CONTEXT_FINISH_GROUP = "FinishCost";

		private const string ICON_UPGRADE = "IconUpgrade";

		private const string SLIDER_NAME = "pBar{0}";

		private const string SLIDER_CURRENT = "pBarCurrent{0}";

		private const string SLIDER_NEXT = "pBarNext{0}";

		private const string LABEL_PBAR = "LabelpBar{0}";

		private const string LABEL_PBAR_CUR = "LabelpBar{0}Current";

		private const string LABEL_PBAR_NEXT = "LabelpBar{0}Next";

		private const string ELEMENT_PBAR = "pBar{0}";

		private const string SHARD_PROGRESS_LABEL = "LabelProgressQ{0}";

		private const string TROOP_IMAGE = "TroopImage";

		private const string TROOP_QUALITY_PANEL = "TroopImageQ{0}";

		private const string TROOP_QUALITY_BACKGROUND = "SpriteTroopImageBkgGridQ{0}";

		private const string TRAINING_TIME_GROUP = "TrainingTime";

		private const string TRAINING_TIME_NAME_LABEL = "LabelTrainingTime";

		private const string TRAINING_TIME_VALUE_LABEL = "LabelTrainingTimeCurrent";

		private const string TRAINING_TIME_NEXT_LABEL = "LabelTrainingTimeIncrease";

		private const string TRAINING_COST_GROUP = "TrainingCost";

		private const string TRAINING_COST_NAME_LABEL = "LabelTrainingCost";

		private const string TRAINING_COST_ICON = "SpriteTrainingCostIcon";

		private const string TRAINING_COST_VALUE_LABEL = "LabelTrainingCostCurrent";

		private const string TRAINING_COST_NEXT_LABEL = "LabelTrainingCostIncrease";

		private const string ATTACK_RANGE_GROUP = "Range";

		private const string ATTACK_RANGE_NAME_LABEL = "LabelRange";

		private const string ATTACK_RANGE_VALUE_LABEL = "LabelRangeCurrent";

		private const string MOVEMENT_SPEED_GROUP = "MovementSpeed";

		private const string MOVEMENT_SPEED_NAME_LABEL = "LabelMovementSpeed";

		private const string MOVEMENT_SPEED_VALUE_LABEL = "LabelMovementSpeedCurrent";

		private const string UNIT_CAPACITY_GROUP = "UnitCapacity";

		private const string UNIT_CAPACITY_NAME_LABEL = "LabelUnitCapacity";

		private const string UNIT_CAPACITY_VALUE_LABEL = "LabelUnitCapacityCurrent";

		private const string STRING_DAMAGE = "s_Damage";

		private const string STRING_DAMAGE_BUFF = "s_DamageBuff";

		private const string STRING_HEALING_POWER = "HEALING_POWER";

		private const string STRING_HEALING_PERCENT = "s_HealerPercent";

		private const string SPRITE_TROOP_SELECTED = "SpriteTroopSelectedItemImage";

		private const string LEFT_OWN_LABEL = "LabelQuantityOwn";

		private const string QUALITY_OWN_LABEL = "LabelQuantityOwnQ{0}";

		private const string LEFT_INFO_GROUP = "InfoRow{0}";

		private const string LEFT_INFO_TITLE = "InfoTitle{0}";

		private const string LEFT_INFO_DETAIL = "InfoDetail{0}";

		private const string LEFT_ITEM_STATUS = "ItemStatus";

		private const string LEFT_LABEL_QUALITY = "LabelQualityQ{0}";

		private const string LEFT_INFO_GROUP3_ALT = "InfoRow3alt";

		private const string LEFT_INFO_TITLE3_ALT = "InfoTitle3alt";

		private const string LEFT_INFO_DETAIL3_ALT = "InfoDetail3alt";

		public const string LANG_ENTITY_TYPE_PREFIX = "entityType_";

		public const string TARGET_PREF_PREFIX = "target_pref_";

		private const string LANG_SPECIAL_ABILITY = "s_SpecialAbility";

		private const string ICON_CREDITS = "icoCollectCredit";

		private const string ICON_MATERIALS = "icoDialogMaterials";

		private const string ICON_CONTRABAND = "icoContraband";

		private const string PERK_EFFECT_COST = "PerkEffectTrainingCost";

		private const string PERK_EFFECT_TIME = "PerkEffectTrainingTime";

		private const string UNLOCK_IN_EVENT_ONLY = "UNLOCK_IN_EVENT_ONLY";

		private const string BUTTON_NORMAL = "BtnNormal";

		private const string STRING_REWARD_UPGRADE = "LABEL_REWARD_UPGRADE";

		private const string LABEL_NORMAL_INTRO = "LabelNormalIntro";

		private const string SPRITE_SHARD_TROOP_SELECTED = "SpriteTroopSelectedItemImageQ{0}";

		private const string SHARD_PROGRESS_BAR = "pBarFrag";

		private const string SHARD_PROGRESS_BAR_SPRITE = "SpritepBarFrag";

		private const string SHARD_INFO_PROGRESS_LABEL = "LabelProgress";

		private const string STRING_FRACTION = "FRACTION";

		private const string STRING_MAX_LEVEL = "MAX_LEVEL";

		private const string BTN_GALAXY = "BtnGalaxy";

		private const string LABEL_BTN_GALAXY = "LabelBtnGalaxy";

		private const string PLANETS_UNLOCK_GROUP = "PanelPlanetAvailability";

		private const string LABEL_PLANETS_UNLOCK = "LabelPlanetAvailability";

		private const string GRID_PLANETS_UNLOCK = "GridPlanetAvailability";

		private const string GRID_PLANETS_UNLOCK_TEMPLATE = "TemplatePlanet";

		private const string LABEL_PLANET_UNLOCK_ITEM = "LabelAvailablePlanet";

		private const string TEXTURE_PLANET_UNLOCK_ITEM = "TextureAvailablePlanet";

		private const string PLANETS_PLAY_BUILD_PC = "PLANETS_PLAY_BUILD_PC";

		protected TroopUpgradeTag selectedTroop;

		private IDeployableVO nextLevel;

		private IDeployableVO currentLevel;

		private List<TroopUpgradeTag> troopList;

		protected bool showUpgradeControls;

		private Contract activeContract;

		protected GeometryProjector troopImage;

		protected UXLabel labelUpgradeTime;

		protected UXElement attackRangeGroup;

		protected UXElement movementSpeedGroup;

		protected UXElement unitCapacityGroup;

		protected UXElement trainingTimeGroup;

		protected UXElement trainingCostGroup;

		protected UXLabel attackRangeNameLabel;

		protected UXLabel attackRangeValueLabel;

		protected UXLabel movementSpeedNameLabel;

		protected UXLabel movementSpeedValueLabel;

		protected UXLabel unitCapacityValueLabel;

		protected UXLabel trainingTimeNameLabel;

		protected UXLabel trainingTimeValueLabel;

		protected UXLabel trainingCostValueLabel;

		protected UXLabel trainingCostNextValueLabel;

		private UXSprite trainingCostIcon;

		private UXElement perkEffectCost;

		private UXElement perkEffectTime;

		private UXButton goToGalaxyButton;

		private UXLabel goToGalaxyLabel;

		private UXElement planetsUnlockGroup;

		private UXGrid planetsUnlockGrid;

		private UXLabel planetsUnlockLabel;

		private int maxTroopDps;

		private int maxHealth;

		private int maxSpecialAttackDps;

		private bool timerActive;

		private float accumulatedUpdateDt;

		private bool uiWasRefreshed;

		protected bool wantsTransition;

		protected bool shouldCloseParent;

		protected override bool WantTransitions
		{
			get
			{
				return this.wantsTransition;
			}
		}

		public override bool ShowCurrencyTray
		{
			get
			{
				return this.showUpgradeControls;
			}
		}

		public DeployableInfoScreen(TroopUpgradeTag selectedTroop, List<TroopUpgradeTag> troopList, bool showUpgradeControls, Entity selectedBuilding) : base("gui_troop_info", selectedBuilding)
		{
			TroopUpgradeCatalog troopUpgradeCatalog = Service.Get<TroopUpgradeCatalog>();
			this.wantsTransition = false;
			this.shouldCloseParent = true;
			this.accumulatedUpdateDt = 0f;
			this.showUpgradeControls = showUpgradeControls;
			this.selectedTroop = selectedTroop;
			this.currentLevel = selectedTroop.Troop;
			this.nextLevel = this.GetNextDeployable(selectedTroop.Troop);
			this.troopList = troopList;
			this.maxTroopDps = troopUpgradeCatalog.MaxTroopDps;
			this.maxHealth = troopUpgradeCatalog.MaxTroopHealth;
			this.maxSpecialAttackDps = Service.Get<StarshipUpgradeCatalog>().MaxSpecialAttackDps;
			if (showUpgradeControls)
			{
				this.CheckActiveContract();
			}
		}

		private IDeployableVO GetNextDeployable(IDeployableVO currentTroop)
		{
			if (currentTroop is SpecialAttackTypeVO)
			{
				return Service.Get<StarshipUpgradeCatalog>().GetNextLevel(currentTroop as SpecialAttackTypeVO);
			}
			return Service.Get<TroopUpgradeCatalog>().GetNextLevel(currentTroop as TroopTypeVO);
		}

		public override void Close(object modalResult)
		{
			if (this.shouldCloseParent)
			{
				this.wantsTransition = true;
				ClosableScreen deployableInfoParentScreen = ScreenUtils.GetDeployableInfoParentScreen();
				if (deployableInfoParentScreen != null)
				{
					deployableInfoParentScreen.Close(null);
				}
			}
			base.Close(modalResult);
		}

		public override void OnDestroyElement()
		{
			if (this.troopImage != null)
			{
				this.troopImage.Destroy();
				this.troopImage = null;
			}
			this.troopList = null;
			this.selectedTroop = null;
			this.currentLevel = null;
			this.nextLevel = null;
			this.labelUpgradeTime = null;
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ContractCompleted);
			this.DisableTimers();
			base.OnDestroyElement();
		}

		protected override void OnScreenLoaded()
		{
			base.OnScreenLoaded();
			this.ToggleParentScreenVisibility(false);
			this.InitButtons();
			this.uiWasRefreshed = false;
			UXButton element = base.GetElement<UXButton>("BtnTroopPrev");
			UXButton element2 = base.GetElement<UXButton>("BtnTroopNext");
			if (this.troopList != null && this.troopList.Count > 1)
			{
				element.Visible = true;
				element2.Visible = true;
				element.Tag = -1;
				element2.Tag = 1;
				element.OnClicked = new UXButtonClickedDelegate(this.OnPrevOrNextButtonClicked);
				element2.OnClicked = new UXButtonClickedDelegate(this.OnPrevOrNextButtonClicked);
			}
			else
			{
				element.Visible = false;
				element2.Visible = false;
			}
			UXButton element3 = base.GetElement<UXButton>("BtnBack");
			element3.OnClicked = new UXButtonClickedDelegate(this.OnBackButtonClicked);
			base.CurrentBackDelegate = new UXButtonClickedDelegate(this.OnBackButtonClicked);
			base.CurrentBackButton = element3;
			base.GetElement<UXButton>("BtnFinish").OnClicked = new UXButtonClickedDelegate(this.OnFinishClicked);
			base.GetElement<UXButton>("ButtonPrimaryAction").OnClicked = new UXButtonClickedDelegate(this.OnPurchaseClicked);
			base.GetElement<UXButton>("BtnNormal").OnClicked = new UXButtonClickedDelegate(this.OnPurchaseClicked);
			this.labelUpgradeTime = base.GetElement<UXLabel>("LabelUpgradeTime");
			this.attackRangeGroup = base.GetElement<UXElement>("Range");
			this.movementSpeedGroup = base.GetElement<UXElement>("MovementSpeed");
			this.unitCapacityGroup = base.GetElement<UXElement>("UnitCapacity");
			this.trainingTimeGroup = base.GetElement<UXElement>("TrainingTime");
			this.trainingCostGroup = base.GetElement<UXElement>("TrainingCost");
			this.attackRangeNameLabel = base.GetElement<UXLabel>("LabelRange");
			this.attackRangeValueLabel = base.GetElement<UXLabel>("LabelRangeCurrent");
			this.movementSpeedNameLabel = base.GetElement<UXLabel>("LabelMovementSpeed");
			this.movementSpeedValueLabel = base.GetElement<UXLabel>("LabelMovementSpeedCurrent");
			this.unitCapacityValueLabel = base.GetElement<UXLabel>("LabelUnitCapacityCurrent");
			this.trainingTimeNameLabel = base.GetElement<UXLabel>("LabelTrainingTime");
			this.trainingTimeValueLabel = base.GetElement<UXLabel>("LabelTrainingTimeCurrent");
			this.trainingCostValueLabel = base.GetElement<UXLabel>("LabelTrainingCostCurrent");
			this.trainingCostNextValueLabel = base.GetElement<UXLabel>("LabelTrainingCostIncrease");
			this.trainingCostIcon = base.GetElement<UXSprite>("SpriteTrainingCostIcon");
			this.perkEffectCost = base.GetElement<UXElement>("PerkEffectTrainingCost");
			this.perkEffectTime = base.GetElement<UXElement>("PerkEffectTrainingTime");
			this.goToGalaxyButton = base.GetElement<UXButton>("BtnGalaxy");
			this.goToGalaxyButton.Visible = false;
			this.goToGalaxyLabel = base.GetElement<UXLabel>("LabelBtnGalaxy");
			this.goToGalaxyLabel.Visible = false;
			this.planetsUnlockGroup = base.GetElement<UXElement>("PanelPlanetAvailability");
			this.planetsUnlockGroup.Visible = false;
			this.planetsUnlockGrid = base.GetElement<UXGrid>("GridPlanetAvailability");
			this.planetsUnlockLabel = base.GetElement<UXLabel>("LabelPlanetAvailability");
			base.GetElement<UXElement>("BtnNormal").Visible = false;
			this.Redraw();
			Service.Get<EventManager>().RegisterObserver(this, EventId.ContractCompleted);
		}

		private void CheckActiveContract()
		{
			Contract contract = null;
			if (this.selectedBuilding != null)
			{
				BuildingComponent buildingComp = ((SmartEntity)this.selectedBuilding).BuildingComp;
				contract = Service.Get<ISupportController>().FindCurrentContract(buildingComp.BuildingTO.Key);
			}
			if (contract != null)
			{
				this.activeContract = contract;
				this.EnableTimers();
				return;
			}
			this.activeContract = null;
			this.DisableTimers();
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ContractCompleted)
			{
				ContractEventData contractEventData = cookie as ContractEventData;
				ContractType contractType = contractEventData.Contract.ContractTO.ContractType;
				if (contractType == ContractType.Build || contractType == ContractType.Research || contractType == ContractType.Upgrade)
				{
					bool flag = this.activeContract != null;
					this.CheckActiveContract();
					bool flag2 = flag != (this.activeContract != null);
					if (contractEventData.Contract.ProductUid == this.nextLevel.Uid)
					{
						flag2 = true;
						IDeployableVO nextDeployable = this.GetNextDeployable(this.nextLevel);
						if (nextDeployable != null)
						{
							this.currentLevel = this.nextLevel;
							this.nextLevel = nextDeployable;
						}
					}
					if (flag2)
					{
						this.uiWasRefreshed = true;
						this.Redraw();
					}
				}
			}
			return base.OnEvent(id, cookie);
		}

		public void OnViewFrameTime(float dt)
		{
			this.accumulatedUpdateDt += dt;
			if (this.accumulatedUpdateDt >= 0.1f)
			{
				this.UpdateContractTimers();
				this.accumulatedUpdateDt = 0f;
			}
		}

		private void UpdateContractTimers()
		{
			if (this.activeContract == null || this.labelUpgradeTime == null)
			{
				return;
			}
			if (this.nextLevel != null && this.nextLevel.Uid == this.activeContract.ProductUid)
			{
				int remainingTimeForView = this.activeContract.GetRemainingTimeForView();
				if (remainingTimeForView > 0)
				{
					this.labelUpgradeTime.Text = GameUtils.GetTimeLabelFromSeconds(remainingTimeForView);
					int crystalCostToFinishContract = ContractUtils.GetCrystalCostToFinishContract(this.activeContract);
					UXUtils.SetupCostElements(this, "FinishCost", null, 0, 0, 0, crystalCostToFinishContract, 0, !this.selectedTroop.ReqMet, null);
					return;
				}
				this.activeContract = null;
				this.DisableTimers();
				this.Redraw();
			}
		}

		private void EnableTimers()
		{
			if (!this.timerActive)
			{
				this.timerActive = true;
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			}
		}

		private void DisableTimers()
		{
			this.timerActive = false;
			this.activeContract = null;
			this.accumulatedUpdateDt = 0f;
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		private void SetupAndEnableUnlockUnitGalaxyUI(IDeployableVO deployabVO)
		{
			this.goToGalaxyButton.Visible = true;
			this.goToGalaxyLabel.Visible = true;
			this.planetsUnlockGroup.Visible = true;
			if (Service.Get<BuildingLookupController>().GetCurrentNavigationCenter() == null)
			{
				this.goToGalaxyButton.VisuallyDisableButton();
			}
			else
			{
				this.goToGalaxyButton.VisuallyEnableButton();
			}
			DeployableInfoActionButtonTag deployableInfoActionButtonTag = new DeployableInfoActionButtonTag(deployabVO.EventButtonAction, deployabVO.EventButtonData);
			this.goToGalaxyButton.Tag = deployableInfoActionButtonTag;
			this.goToGalaxyButton.OnClicked = new UXButtonClickedDelegate(this.OnGoToGalaxyClicked);
			this.goToGalaxyLabel.Text = this.lang.Get(deployabVO.EventButtonString, new object[0]);
			this.planetsUnlockGrid.SetTemplateItem("TemplatePlanet");
			this.planetsUnlockLabel.Text = this.lang.Get(deployabVO.EventFeaturesString, new object[0]);
			this.FillPlanetsUnlockGrid(deployableInfoActionButtonTag.DataList);
		}

		private void FillPlanetsUnlockGrid(List<string> planetIds)
		{
			if (planetIds == null)
			{
				return;
			}
			IDataController dataController = Service.Get<IDataController>();
			this.planetsUnlockGrid.SetTemplateItem("TemplatePlanet");
			this.planetsUnlockGrid.Clear();
			int count = planetIds.Count;
			for (int i = 0; i < count; i++)
			{
				PlanetVO planetVO = dataController.Get<PlanetVO>(planetIds[i]);
				string itemUid = planetVO.Uid + i;
				UXElement item = this.planetsUnlockGrid.CloneTemplateItem(itemUid);
				UXTexture subElement = this.planetsUnlockGrid.GetSubElement<UXTexture>(itemUid, "TextureAvailablePlanet");
				subElement.LoadTexture(planetVO.LeaderboardButtonTexture);
				UXLabel subElement2 = this.planetsUnlockGrid.GetSubElement<UXLabel>(itemUid, "LabelAvailablePlanet");
				subElement2.Text = this.lang.Get(planetVO.LoadingScreenText, new object[0]);
				this.planetsUnlockGrid.AddItem(item, i);
			}
			this.planetsUnlockGrid.RepositionItems();
		}

		private void HideUnlockUnitGalaxyUI()
		{
			this.goToGalaxyButton.Visible = false;
			this.goToGalaxyLabel.Visible = false;
			this.planetsUnlockGroup.Visible = false;
		}

		private bool IsNotMaxLevel(IDeployableVO deployable)
		{
			return deployable != null && deployable.PlayerFacing;
		}

		private void Redraw()
		{
			UXElement element = base.GetElement<UXElement>("IconUpgrade");
			element.Visible = false;
			this.SetupTroopImage();
			UXButton element2 = base.GetElement<UXButton>("BtnFinish");
			UXButton element3 = base.GetElement<UXButton>("ButtonPrimaryAction");
			UXButton element4 = base.GetElement<UXButton>("BtnNormal");
			element4.Visible = false;
			UnlockController unlockController = Service.Get<UnlockController>();
			bool flag = unlockController.RequiresUnlockByEventReward(this.selectedTroop.Troop);
			bool flag2 = !unlockController.IsMinLevelUnlocked(this.selectedTroop.Troop);
			IDeployableVO deployableVO = this.nextLevel;
			bool flag3 = false;
			bool flag4 = false;
			if (this.IsNotMaxLevel(this.nextLevel))
			{
				if (flag)
				{
					if (flag2)
					{
						deployableVO = this.currentLevel;
						flag3 = false;
						flag4 = (!flag3 && this.showUpgradeControls);
					}
					else
					{
						flag3 = this.showUpgradeControls;
						flag4 = this.showUpgradeControls;
					}
				}
				else
				{
					flag3 = this.showUpgradeControls;
					flag4 = this.showUpgradeControls;
				}
			}
			base.GetElement<UXElement>("ContainerUpgradeTime").Visible = (flag3 && !flag2 && (this.activeContract == null || this.nextLevel.Uid == this.activeContract.ProductUid));
			element2.Visible = flag3;
			element3.Visible = flag3;
			IDeployableVO troop = this.selectedTroop.Troop;
			bool flag5 = (flag4 && !this.selectedTroop.ReqMet && this.IsNotMaxLevel(this.nextLevel)) | flag2;
			string text = this.selectedTroop.RequirementText;
			bool flag6 = Service.Get<UnlockController>().CanDeployableBeUpgraded(this.currentLevel, deployableVO);
			bool flag7 = this.nextLevel != null && Service.Get<DeployableShardUnlockController>().DoesUserHaveUpgradeShardRequirement(deployableVO);
			UXLabel element5 = base.GetElement<UXLabel>("LabelRequirement");
			if (flag)
			{
				bool flag8 = this.showUpgradeControls && !flag2;
				flag5 = ((!flag6 || !flag7) && this.IsNotMaxLevel(this.nextLevel) && !flag8);
				text = this.selectedTroop.ShortRequirementText;
			}
			element5.Visible = flag5;
			if (flag5)
			{
				element5.Text = text;
			}
			if (this.activeContract == null)
			{
				element3.VisuallyEnableButton();
			}
			else
			{
				element3.VisuallyDisableButton();
			}
			bool visible = true;
			if (this.activeContract == null || deployableVO == null || deployableVO.Uid != this.activeContract.ProductUid)
			{
				element2.Visible = false;
				base.GetElement<UXLabel>("LabelUpgradeTimeStatic").Text = this.lang.Get("s_upgradeTime", new object[0]);
				if (deployableVO != null)
				{
					this.labelUpgradeTime.Text = GameUtils.GetTimeLabelFromSeconds(deployableVO.UpgradeTime);
				}
				element3.Enabled = this.selectedTroop.ReqMet;
			}
			else
			{
				element2.Visible = true;
				element3.Visible = false;
				visible = false;
				element5.Visible = false;
				base.GetElement<UXLabel>("LabelUpgradeTimeStatic").Text = this.lang.Get("s_TimeRemaining", new object[0]);
				this.UpdateContractTimers();
			}
			if (flag3)
			{
				if (flag)
				{
					element3.Visible = false;
					element4.Visible = visible;
					element4.Enabled = (flag7 & flag6);
					base.GetElement<UXLabel>("LabelNormalIntro").Text = this.lang.Get("LABEL_REWARD_UPGRADE", new object[0]);
				}
				else
				{
					UXUtils.SetupSingleCostElement(this, "Cost", deployableVO.UpgradeCredits, deployableVO.UpgradeMaterials, deployableVO.UpgradeContraband, 0, 0, false, null);
				}
			}
			DeployableInfoUIType upgradeInfoLevel = DeployableInfoUIType.None;
			bool flag9 = this.IsNotMaxLevel(this.nextLevel) && !string.IsNullOrEmpty(deployableVO.UpgradeShardUid);
			if (flag4)
			{
				if (flag9 & flag7 & flag2)
				{
					upgradeInfoLevel = DeployableInfoUIType.AskOnly;
				}
				else if (flag9 && !flag7 && !flag2)
				{
					upgradeInfoLevel = DeployableInfoUIType.InfoOnly;
				}
				else if (flag7 || !flag2)
				{
					upgradeInfoLevel = DeployableInfoUIType.All;
				}
				if (flag9)
				{
					element4.Visible = visible;
					element4.Enabled = (flag7 & flag6);
					base.GetElement<UXLabel>("LabelNormalIntro").Text = this.lang.Get("LABEL_REWARD_UPGRADE", new object[0]);
				}
			}
			TroopUniqueAbilityDescVO descVO = null;
			if (troop is TroopTypeVO)
			{
				TroopTypeVO troopTypeVO = this.selectedTroop.Troop as TroopTypeVO;
				descVO = troopTypeVO.UniqueAbilityDescVO;
				this.RefreshTroopView(troopTypeVO, upgradeInfoLevel);
			}
			else if (troop is SpecialAttackTypeVO)
			{
				this.RefreshStarshipView(troop as SpecialAttackTypeVO, upgradeInfoLevel);
			}
			base.GetElement<UXElement>("ItemStatus").Visible = false;
			this.SetupLeftTableAltAbilityItem(descVO);
		}

		private void SetupTroopImage()
		{
			for (int i = 1; i <= 3; i++)
			{
				base.GetElement<UXElement>(string.Format("TroopImageQ{0}", new object[]
				{
					i
				})).Visible = false;
			}
			IDeployableVO deployableVO = (this.nextLevel != null) ? this.nextLevel : this.selectedTroop.Troop;
			ShardVO shardVO = null;
			if (!string.IsNullOrEmpty(deployableVO.UpgradeShardUid))
			{
				shardVO = Service.Get<IDataController>().GetOptional<ShardVO>(deployableVO.UpgradeShardUid);
			}
			string name = "SpriteTroopSelectedItemImage";
			base.GetElement<UXLabel>("LabelProgress").Text = string.Empty;
			if (shardVO != null)
			{
				int quality = (int)shardVO.Quality;
				base.GetElement<UXElement>(string.Format("TroopImageQ{0}", new object[]
				{
					quality
				})).Visible = true;
				base.GetElement<UXLabel>(string.Format("LabelQualityQ{0}", new object[]
				{
					quality
				})).Text = LangUtils.GetShardQuality(shardVO.Quality);
				base.GetElement<UXElement>(string.Format("SpriteTroopImageBkgGridQ{0}", new object[]
				{
					quality
				})).Visible = false;
				name = string.Format("SpriteTroopSelectedItemImageQ{0}", new object[]
				{
					quality
				});
				this.SetupShardProgressBar(quality);
				base.SetupFragmentSprite(quality);
			}
			else
			{
				this.fragmentSprite.Visible = false;
				base.GetElement<UXSlider>("pBarFrag").Visible = false;
			}
			base.GetElement<UXElement>("TroopImage").Visible = (shardVO == null);
			UXSprite element = base.GetElement<UXSprite>(name);
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(deployableVO, element, true);
			projectorConfig.AnimPreference = AnimationPreference.AnimationPreferred;
			this.troopImage = ProjectorUtils.GenerateProjector(projectorConfig);
			FactionDecal.SetDeployableDecalVisibiliy(this, false);
		}

		private void SetupShardProgressBar(int quality)
		{
			UXSlider element = base.GetElement<UXSlider>("pBarFrag");
			UXSprite element2 = base.GetElement<UXSprite>("SpritepBarFrag");
			UXLabel element3 = base.GetElement<UXLabel>("LabelProgress");
			UnlockController unlockController = Service.Get<UnlockController>();
			bool flag = !unlockController.IsMinLevelUnlocked(this.selectedTroop.Troop);
			element.Visible = true;
			if (this.nextLevel == null)
			{
				element3.Text = this.lang.Get("MAX_LEVEL", new object[0]);
				element.Value = 1f;
				return;
			}
			IDeployableVO deployableVO = this.nextLevel;
			if (flag)
			{
				deployableVO = this.currentLevel;
			}
			int shardAmount = Service.Get<DeployableShardUnlockController>().GetShardAmount(this.nextLevel.UpgradeShardUid);
			int upgradeShardCount = deployableVO.UpgradeShardCount;
			element3.Text = this.lang.Get("FRACTION", new object[]
			{
				shardAmount,
				upgradeShardCount
			});
			if (upgradeShardCount == 0)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("CMS Error: Shards required for {0} is zero", new object[]
				{
					this.nextLevel.Uid
				});
				return;
			}
			float num = (float)shardAmount / (float)upgradeShardCount;
			UXUtils.SetShardProgressBarValue(element, element2, num);
			if (num >= 1f)
			{
				base.GetElement<UXElement>("IconUpgrade").Visible = true;
			}
		}

		private string GetOwnLabelID()
		{
			string result = "LabelQuantityOwn";
			IDeployableVO deployableVO = (this.nextLevel != null) ? this.nextLevel : this.selectedTroop.Troop;
			ShardVO shardVO = null;
			if (!string.IsNullOrEmpty(deployableVO.UpgradeShardUid))
			{
				shardVO = Service.Get<IDataController>().GetOptional<ShardVO>(deployableVO.UpgradeShardUid);
			}
			if (shardVO != null)
			{
				int quality = (int)shardVO.Quality;
				result = string.Format("LabelQuantityOwnQ{0}", new object[]
				{
					quality
				});
			}
			return result;
		}

		private void RefreshStarshipViewUpgradeAsk(SpecialAttackTypeVO nextLevelVO)
		{
			base.GetElement<UXLabel>("DialogBldgUpgradeTitle").Text = this.lang.Get("BUILDING_UPGRADE", new object[]
			{
				LangUtils.GetStarshipDisplayName(nextLevelVO),
				this.nextLevel.Lvl
			});
			base.GetElement<UXLabel>(this.GetOwnLabelID()).Text = "";
		}

		private void RefreshStarshipViewNoUpgradeAsk(SpecialAttackTypeVO specialAttack)
		{
			base.GetElement<UXLabel>("DialogBldgUpgradeTitle").Text = this.lang.Get("BUILDING_INFO", new object[]
			{
				LangUtils.GetStarshipDisplayName(specialAttack),
				specialAttack.Lvl
			});
			int itemAmount = Service.Get<CurrentPlayer>().Inventory.SpecialAttack.GetItemAmount(specialAttack.Uid);
			base.GetElement<UXLabel>(this.GetOwnLabelID()).Text = this.lang.Get("numOwned", new object[]
			{
				this.lang.ThousandsSeparated(itemAmount)
			});
			base.GetElement<UXLabel>("LabelTrainingTimeIncrease").Text = "";
		}

		private void RefreshStarshipView(SpecialAttackTypeVO specialAttack, DeployableInfoUIType upgradeInfoLevel)
		{
			int nextValue = 0;
			SpecialAttackTypeVO specialAttackTypeVO = (SpecialAttackTypeVO)this.nextLevel;
			if (upgradeInfoLevel > DeployableInfoUIType.None)
			{
				if (upgradeInfoLevel == DeployableInfoUIType.InfoOnly)
				{
					this.RefreshStarshipViewNoUpgradeAsk(specialAttack);
				}
				else
				{
					this.RefreshStarshipViewUpgradeAsk(specialAttackTypeVO);
				}
				if (upgradeInfoLevel != DeployableInfoUIType.AskOnly)
				{
					int num = specialAttackTypeVO.TrainingTime - specialAttack.TrainingTime;
					string id = (num >= 0) ? "PLUS" : "MINUS";
					base.GetElement<UXLabel>("LabelTrainingTimeIncrease").Text = this.lang.Get(id, new object[]
					{
						GameUtils.GetTimeLabelFromSeconds(num)
					});
					nextValue = specialAttackTypeVO.DPS;
				}
			}
			else
			{
				this.RefreshStarshipViewNoUpgradeAsk(specialAttack);
			}
			base.GetElement<UXLabel>("LabelTroopInfo").Text = LangUtils.GetStarshipDescription(specialAttack);
			IDataController dataController = Service.Get<IDataController>();
			SpecialAttackTypeVO maxLevel = Service.Get<StarshipUpgradeCatalog>().GetMaxLevel(specialAttack);
			this.maxSpecialAttackDps = maxLevel.DPS;
			if (specialAttack.IsDropship)
			{
				bool flag = upgradeInfoLevel > DeployableInfoUIType.None;
				TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(specialAttack.LinkedUnit);
				TroopTypeVO troopTypeVO2 = null;
				TroopTypeVO troopTypeVO3 = dataController.Get<TroopTypeVO>(maxLevel.LinkedUnit);
				if (specialAttackTypeVO != null)
				{
					troopTypeVO2 = dataController.Get<TroopTypeVO>(specialAttackTypeVO.LinkedUnit);
				}
				else
				{
					flag = false;
				}
				if (flag && troopTypeVO2 == null)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Invaild Dropship Troop: {0}, on Special Attack: {1}", new object[]
					{
						specialAttackTypeVO.LinkedUnit,
						specialAttackTypeVO.Uid
					});
				}
				this.SetupBar(1, this.lang.Get("TROOP_UNITS", new object[0]), (int)specialAttack.UnitCount, (int)(flag ? specialAttackTypeVO.UnitCount : 0u), (int)maxLevel.UnitCount);
				this.SetupBar(2, this.lang.Get("PER_UNIT_DAMAGE", new object[0]), troopTypeVO.DPS, flag ? troopTypeVO2.DPS : 0, troopTypeVO3.DPS);
				this.SetupBar(3, this.lang.Get("PER_UNIT_HEALTH", new object[0]), troopTypeVO.Health, flag ? troopTypeVO2.Health : 0, troopTypeVO3.Health);
				this.movementSpeedNameLabel.Visible = false;
				this.movementSpeedValueLabel.Visible = false;
				this.SetAttackRange(0u, 0u);
			}
			else
			{
				string id2 = "s_Damage";
				if (specialAttack.InfoUIType == InfoUIType.Healer)
				{
					id2 = "HEALING_POWER";
				}
				else if (specialAttack.InfoUIType == InfoUIType.HealerPercent)
				{
					id2 = "s_HealerPercent";
				}
				else if (specialAttack.InfoUIType == InfoUIType.DamageBuff)
				{
					id2 = "s_DamageBuff";
				}
				this.SetupBar(1, this.lang.Get(id2, new object[0]), specialAttack.DPS, nextValue, this.maxSpecialAttackDps);
				this.SetupBar(2, null, 0, 0, 0);
				this.SetupBar3();
				this.movementSpeedNameLabel.Visible = true;
				this.movementSpeedValueLabel.Visible = true;
				this.movementSpeedValueLabel.Text = this.lang.ThousandsSeparated(specialAttack.MaxSpeed);
				this.attackRangeNameLabel.Text = this.lang.Get("PREFERRED_TARGET", new object[0]);
				this.attackRangeValueLabel.Text = this.lang.Get("target_pref_" + specialAttack.FavoriteTargetType, new object[0]);
			}
			this.unitCapacityValueLabel.Text = this.lang.ThousandsSeparated(specialAttack.Size);
			this.trainingTimeValueLabel.Text = GameUtils.GetTimeLabelFromSeconds(specialAttack.TrainingTime);
			int credits = specialAttack.Credits;
			int materials = specialAttack.Materials;
			int contraband = specialAttack.Contraband;
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			BuildingTypeVO minBuildingRequirement = buildingLookupController.GetMinBuildingRequirement(specialAttack);
			float contractCostMultiplier = Service.Get<PerkManager>().GetContractCostMultiplier(minBuildingRequirement);
			GameUtils.MultiplyCurrency(contractCostMultiplier, ref credits, ref materials, ref contraband);
			this.trainingCostValueLabel.Text = this.lang.ThousandsSeparated(credits);
			for (int i = 0; i < 4; i++)
			{
				this.SetupLeftTableItem(i, null, null);
			}
			if (specialAttack.UnlockedByEvent)
			{
				this.SetupAndEnableUnlockUnitGalaxyUI(specialAttack);
			}
			else
			{
				this.HideUnlockUnitGalaxyUI();
			}
			this.SetTrainingCost(specialAttack, upgradeInfoLevel > DeployableInfoUIType.None);
		}

		private void RefreshTroopViewUpgradeAsk(TroopTypeVO nextLevelVO)
		{
			base.GetElement<UXLabel>("DialogBldgUpgradeTitle").Text = this.lang.Get("BUILDING_UPGRADE", new object[]
			{
				LangUtils.GetTroopDisplayName(nextLevelVO),
				nextLevelVO.Lvl
			});
			base.GetElement<UXLabel>(this.GetOwnLabelID()).Text = "";
		}

		private void RefreshTroopViewNoUpgradeAsk(TroopTypeVO troop)
		{
			base.GetElement<UXLabel>("DialogBldgUpgradeTitle").Text = this.lang.Get("BUILDING_INFO", new object[]
			{
				LangUtils.GetTroopDisplayName(troop),
				troop.Lvl
			});
			Inventory inventory = Service.Get<CurrentPlayer>().Inventory;
			TroopType type = troop.Type;
			int itemAmount;
			if (type != TroopType.Hero)
			{
				if (type != TroopType.Champion)
				{
					itemAmount = inventory.Troop.GetItemAmount(troop.Uid);
				}
				else
				{
					itemAmount = inventory.Champion.GetItemAmount(troop.Uid);
				}
			}
			else
			{
				itemAmount = inventory.Hero.GetItemAmount(troop.Uid);
			}
			base.GetElement<UXLabel>(this.GetOwnLabelID()).Text = this.lang.Get("numOwned", new object[]
			{
				this.lang.ThousandsSeparated(itemAmount)
			});
			base.GetElement<UXLabel>("LabelTrainingTimeIncrease").Text = "";
		}

		private void RefreshTroopView(TroopTypeVO troop, DeployableInfoUIType upgradeInfoLevel)
		{
			int nextValue = 0;
			int nextValue2 = 0;
			TroopTypeVO troopTypeVO = (TroopTypeVO)this.nextLevel;
			if (upgradeInfoLevel > DeployableInfoUIType.None)
			{
				if (upgradeInfoLevel == DeployableInfoUIType.InfoOnly)
				{
					this.RefreshTroopViewNoUpgradeAsk(troop);
				}
				else
				{
					this.RefreshTroopViewUpgradeAsk(troopTypeVO);
				}
				if (upgradeInfoLevel != DeployableInfoUIType.AskOnly)
				{
					int num = troopTypeVO.TrainingTime - troop.TrainingTime;
					string id = (num >= 0) ? "PLUS" : "MINUS";
					base.GetElement<UXLabel>("LabelTrainingTimeIncrease").Text = this.lang.Get(id, new object[]
					{
						GameUtils.GetTimeLabelFromSeconds(num)
					});
					nextValue = troopTypeVO.DPS;
					nextValue2 = troopTypeVO.Health;
				}
			}
			else
			{
				this.RefreshTroopViewNoUpgradeAsk(troop);
			}
			base.GetElement<UXLabel>("LabelTroopInfo").Text = LangUtils.GetTroopDescription(troop);
			TroopTypeVO maxLevel = Service.Get<TroopUpgradeCatalog>().GetMaxLevel(troop);
			this.maxTroopDps = maxLevel.DPS;
			this.maxHealth = maxLevel.Health;
			string id2 = "s_Damage";
			if (troop.InfoUIType == InfoUIType.Healer)
			{
				id2 = "HEALING_POWER";
			}
			else if (troop.InfoUIType == InfoUIType.HealerPercent)
			{
				id2 = "s_HealerPercent";
			}
			else if (troop.InfoUIType == InfoUIType.DamageBuff)
			{
				id2 = "s_DamageBuff";
			}
			this.SetupBar(1, this.lang.Get(id2, new object[0]), troop.DPS, nextValue, this.maxTroopDps);
			this.SetupBar(2, this.lang.Get("HEALTH", new object[]
			{
				""
			}), troop.Health, nextValue2, this.maxHealth);
			this.SetupBar3();
			this.SetupLeftTableItem(0, this.lang.Get("TRAINING_CLASS_TYPE", new object[0]), this.lang.Get("trp_class_" + troop.TroopRole.ToString(), new object[0]));
			this.SetupLeftTableItem(1, this.lang.Get("DAMAGE_TYPE", new object[0]), this.lang.Get((troop.ProjectileType.SplashRadius > 0) ? "DAMAGE_TYPE_SPLASH" : "DAMAGE_TYPE_STANDARD", new object[0]));
			this.SetupLeftTableItem(2, this.lang.Get("FAVORITE_TARGET", new object[0]), this.lang.Get("target_pref_" + troop.FavoriteTargetType, new object[0]));
			if (troop.Type == TroopType.Hero && !string.IsNullOrEmpty(troop.Ability))
			{
				TroopAbilityVO abilityInfo = Service.Get<IDataController>().Get<TroopAbilityVO>(troop.Ability);
				this.SetupLeftTableItem(3, this.lang.Get("s_HeroPower", new object[0]), LangUtils.GetHeroAbilityDisplayName(abilityInfo));
			}
			else
			{
				this.SetupLeftTableItem(3, null, null);
			}
			this.SetAttackRange(troop.MinAttackRange, troop.MaxAttackRange);
			this.movementSpeedValueLabel.Text = this.lang.ThousandsSeparated(troop.MaxSpeed);
			this.unitCapacityValueLabel.Text = this.lang.ThousandsSeparated(troop.Size);
			float contractTimeReductionMultiplier = Service.Get<PerkManager>().GetContractTimeReductionMultiplier(troop);
			int num2 = Mathf.FloorToInt((float)troop.TrainingTime * contractTimeReductionMultiplier);
			this.trainingTimeValueLabel.Text = GameUtils.GetTimeLabelFromSeconds(num2);
			if (num2 != troop.TrainingTime)
			{
				if (troop.Type == TroopType.Champion)
				{
					this.perkEffectCost.Visible = true;
				}
				else
				{
					this.perkEffectTime.Visible = true;
				}
			}
			if (troop.UnlockedByEvent)
			{
				this.SetupAndEnableUnlockUnitGalaxyUI(troop);
			}
			else
			{
				this.HideUnlockUnitGalaxyUI();
			}
			this.SetTrainingCost(troop, upgradeInfoLevel > DeployableInfoUIType.None);
		}

		protected override void SetupPerksButton()
		{
			UXButton element = base.GetElement<UXButton>("btnPerks");
			element.Visible = false;
		}

		private void SetupLeftTableAltAbilityItem(TroopUniqueAbilityDescVO descVO)
		{
			bool flag = descVO != null;
			base.GetElement<UXElement>("InfoRow3alt").Visible = flag;
			if (flag)
			{
				base.GetElement<UXLabel>("InfoTitle3alt").Text = this.lang.Get("s_SpecialAbility", new object[0]);
				string text = this.lang.Get(descVO.AbilityTitle1, new object[0]);
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(descVO.AbilityTitle2))
				{
					text = text + "\n\n" + this.lang.Get(descVO.AbilityTitle2, new object[0]);
				}
				base.GetElement<UXLabel>("InfoDetail3alt").Text = text;
			}
		}

		private void SetTrainingCost(IDeployableVO vo, bool showUpgradeCostIncrease)
		{
			int num = 0;
			int contraband = vo.Contraband;
			int materials = vo.Materials;
			int credits = vo.Credits;
			int num2 = (this.nextLevel == null) ? 0 : this.nextLevel.Contraband;
			int num3 = (this.nextLevel == null) ? 0 : this.nextLevel.Materials;
			int num4 = (this.nextLevel == null) ? 0 : this.nextLevel.Credits;
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			PerkManager perkManager = Service.Get<PerkManager>();
			BuildingTypeVO minBuildingRequirement = buildingLookupController.GetMinBuildingRequirement(vo);
			float contractCostMultiplier = perkManager.GetContractCostMultiplier(minBuildingRequirement);
			GameUtils.MultiplyCurrency(contractCostMultiplier, ref credits, ref materials, ref contraband);
			GameUtils.MultiplyCurrency(contractCostMultiplier, ref num4, ref num3, ref num2);
			if (vo.Contraband > 0)
			{
				this.trainingCostIcon.SpriteName = "icoContraband";
				this.trainingCostValueLabel.Text = this.lang.ThousandsSeparated(contraband);
				if (showUpgradeCostIncrease)
				{
					num = num2 - contraband;
				}
			}
			else if (vo.Materials > 0)
			{
				this.trainingCostIcon.SpriteName = "icoDialogMaterials";
				this.trainingCostValueLabel.Text = this.lang.ThousandsSeparated(materials);
				if (showUpgradeCostIncrease)
				{
					num = num3 - materials;
				}
			}
			else
			{
				this.trainingCostIcon.SpriteName = "icoCollectCredit";
				this.trainingCostValueLabel.Text = this.lang.ThousandsSeparated(credits);
				if (showUpgradeCostIncrease)
				{
					num = num4 - credits;
				}
			}
			if (perkManager.IsContractCostMultiplierAppliedToBuilding(minBuildingRequirement))
			{
				this.perkEffectCost.Visible = true;
			}
			this.trainingCostNextValueLabel.Visible = showUpgradeCostIncrease;
			if (showUpgradeCostIncrease)
			{
				string id = (num >= 0) ? "PLUS" : "MINUS";
				base.GetElement<UXLabel>("LabelTrainingCostIncrease").Text = this.lang.Get(id, new object[]
				{
					num
				});
			}
		}

		private void SetAttackRange(uint minAttackRange, uint maxAttackRange)
		{
			UXLabel uXLabel = this.attackRangeNameLabel;
			UXLabel uXLabel2 = this.attackRangeValueLabel;
			if (maxAttackRange == 0u)
			{
				uXLabel.Text = "";
				uXLabel2.Text = "";
				return;
			}
			uXLabel.Text = this.lang.Get("RANGE", new object[0]);
			uXLabel2.Text = ((minAttackRange == 0u) ? this.lang.Get("TILE_COUNT", new object[]
			{
				maxAttackRange
			}) : this.lang.Get("TILE_RANGE", new object[]
			{
				minAttackRange,
				maxAttackRange
			}));
		}

		protected virtual void SetupBar3()
		{
			this.SetupBar(3, null, 0, 0, 0);
		}

		protected void SetupBar(int index, string labelString, int currentValue, int nextValue, int maxValue)
		{
			bool flag = labelString != null;
			base.GetElement<UXElement>(string.Format("pBar{0}", new object[]
			{
				index
			})).Visible = flag;
			if (!flag)
			{
				return;
			}
			UXLabel element = base.GetElement<UXLabel>(string.Format("LabelpBar{0}", new object[]
			{
				index
			}));
			element.Text = labelString;
			UXSlider element2 = base.GetElement<UXSlider>(string.Format("pBarCurrent{0}", new object[]
			{
				index
			}));
			UXSlider element3 = base.GetElement<UXSlider>(string.Format("pBarNext{0}", new object[]
			{
				index
			}));
			element3.Visible = (nextValue > currentValue);
			element2.Value = MathUtils.NormalizeRange((float)currentValue, 0f, (float)maxValue);
			element3.Value = MathUtils.NormalizeRange((float)nextValue, 0f, (float)maxValue);
			UXLabel element4 = base.GetElement<UXLabel>(string.Format("LabelpBar{0}Current", new object[]
			{
				index
			}));
			UXLabel element5 = base.GetElement<UXLabel>(string.Format("LabelpBar{0}Next", new object[]
			{
				index
			}));
			element5.Visible = (nextValue > currentValue);
			element4.Text = this.lang.ThousandsSeparated(currentValue);
			element5.Text = this.lang.Get("PLUS", new object[]
			{
				this.lang.ThousandsSeparated(nextValue - currentValue)
			});
		}

		private void SetupLeftTableItem(int index, string title, string desc)
		{
			bool flag = !string.IsNullOrEmpty(title);
			base.GetElement<UXElement>(string.Format("InfoRow{0}", new object[]
			{
				index
			})).Visible = flag;
			if (flag)
			{
				base.GetElement<UXLabel>(string.Format("InfoTitle{0}", new object[]
				{
					index
				})).Text = title;
				base.GetElement<UXLabel>(string.Format("InfoDetail{0}", new object[]
				{
					index
				})).Text = desc;
			}
		}

		private void OnBackButtonClicked(UXButton button)
		{
			this.ToggleParentScreenVisibility(true);
			this.shouldCloseParent = false;
			this.Close(null);
		}

		private void ToggleParentScreenVisibility(bool visible)
		{
			ClosableScreen deployableInfoParentScreen = ScreenUtils.GetDeployableInfoParentScreen();
			if (deployableInfoParentScreen != null)
			{
				deployableInfoParentScreen.SetVisibilityAndRefresh(visible, this.uiWasRefreshed);
			}
		}

		private void OnPrevOrNextButtonClicked(UXButton button)
		{
			int num = (int)button.Tag;
			int count = this.troopList.Count;
			int index = (num < 0) ? (count - 1) : 0;
			TroopUpgradeTag troopUpgradeTag = this.troopList[index];
			for (int i = count - 1; i >= 0; i--)
			{
				int index2 = (num < 0) ? (count - 1 - i) : i;
				TroopUpgradeTag troopUpgradeTag2 = this.troopList[index2];
				if (troopUpgradeTag2.Troop == this.selectedTroop.Troop)
				{
					break;
				}
				troopUpgradeTag = troopUpgradeTag2;
			}
			this.selectedTroop = troopUpgradeTag;
			this.currentLevel = this.selectedTroop.Troop;
			this.nextLevel = this.GetNextDeployable(this.selectedTroop.Troop);
			this.Redraw();
		}

		private void StartUpgrading()
		{
			if (this.nextLevel is TroopTypeVO)
			{
				Service.Get<ISupportController>().StartTroopUpgrade((TroopTypeVO)this.nextLevel, this.selectedBuilding);
			}
			else if (this.nextLevel is SpecialAttackTypeVO)
			{
				Service.Get<ISupportController>().StartStarshipUpgrade((SpecialAttackTypeVO)this.nextLevel, this.selectedBuilding);
			}
			this.CloseFromResearchScreen();
		}

		private void CloseFromResearchScreen()
		{
			this.Close(this.selectedBuilding.ID);
			TroopUpgradeScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<TroopUpgradeScreen>();
			if (highestLevelScreen != null)
			{
				highestLevelScreen.Close(this.selectedBuilding.ID);
			}
			this.RefreshResearchContextButtons();
		}

		private void RefreshResearchContextButtons()
		{
			Entity selectedBuilding = Service.Get<BuildingController>().SelectedBuilding;
			if (selectedBuilding == null)
			{
				return;
			}
			BuildingComponent buildingComponent = selectedBuilding.Get<BuildingComponent>();
			if (buildingComponent == null)
			{
				return;
			}
			BuildingTypeVO buildingType = buildingComponent.BuildingType;
			if (buildingType.Type == BuildingType.TroopResearch)
			{
				Service.Get<UXController>().HUD.ShowContextButtons(selectedBuilding);
			}
		}

		protected virtual void OnPurchaseClicked(UXButton button)
		{
			if (this.selectedTroop == null)
			{
				return;
			}
			if (this.activeContract != null)
			{
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(this.lang.Get("UPGRADE_CONTRACT_ACTIVE", new object[0]));
				return;
			}
			if (this.selectedBuilding == null)
			{
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(this.lang.Get("UPGRADE_RESEARCH_CENTER_ACTIVE", new object[0]));
				return;
			}
			IUpgradeableVO upgradeableVO = this.nextLevel;
			StringBuilder stringBuilder = new StringBuilder();
			string text = "";
			string text2 = "";
			int num = 0;
			if (upgradeableVO is SpecialAttackTypeVO)
			{
				text = (upgradeableVO as SpecialAttackTypeVO).SpecialAttackID;
				text2 = text;
				num = (upgradeableVO as SpecialAttackTypeVO).Lvl;
			}
			else if (upgradeableVO is TroopTypeVO)
			{
				text = (upgradeableVO as TroopTypeVO).Type.ToString();
				text2 = (upgradeableVO as TroopTypeVO).TroopID;
				num = (upgradeableVO as TroopTypeVO).Lvl;
			}
			string text3 = StringUtils.ToLowerCaseUnderscoreSeperated(text);
			stringBuilder.Append(text3);
			stringBuilder.Append("|");
			stringBuilder.Append(text2);
			stringBuilder.Append("|");
			stringBuilder.Append(num);
			stringBuilder.Append("|research");
			if (!PayMeScreen.ShowIfNotEnoughCurrency(upgradeableVO.UpgradeCredits, upgradeableVO.UpgradeMaterials, upgradeableVO.UpgradeContraband, stringBuilder.ToString(), new OnScreenModalResult(this.OnPayMeForCurrencyResult)))
			{
				this.StartUpgrading();
			}
		}

		private void OnGoToGalaxyClicked(UXButton btn)
		{
			Service.Get<EventManager>().SendEvent(EventId.UnitInfoGoToGalaxy, this.selectedTroop.Troop.Uid);
			DeployableInfoActionButtonTag tag = (DeployableInfoActionButtonTag)btn.Tag;
			bool flag = Service.Get<BuildingLookupController>().GetCurrentNavigationCenter() != null;
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (!flag)
			{
				string instructions = this.lang.Get("PLANETS_PLAY_BUILD_PC", new object[0]);
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(instructions);
				return;
			}
			if (currentState is HomeState)
			{
				this.GoToGalaxyFromHomeState(tag);
				return;
			}
			if (currentState is GalaxyState)
			{
				this.GoToGalaxyFromGalaxyState(tag);
				return;
			}
			Service.Get<StaRTSLogger>().Error("Attempt to go to galaxy from invalide state " + currentState.ToString());
		}

		private void GoToGalaxyFromHomeState(DeployableInfoActionButtonTag tag)
		{
			GalaxyViewController galaxyViewController = Service.Get<GalaxyViewController>();
			string actionId = tag.ActionId;
			string planetUID = tag.DataList[0];
			this.Close(null);
			if (actionId == "planet")
			{
				galaxyViewController.GoToPlanetView(planetUID, CampaignScreenSection.Main);
				return;
			}
			if (actionId == "galaxy")
			{
				galaxyViewController.GoToGalaxyView(planetUID);
			}
		}

		private void GoToGalaxyFromGalaxyState(DeployableInfoActionButtonTag tag)
		{
			GalaxyViewController galaxyViewController = Service.Get<GalaxyViewController>();
			GalaxyPlanetController galaxyPlanetController = Service.Get<GalaxyPlanetController>();
			ScreenController screenController = Service.Get<ScreenController>();
			string actionId = tag.ActionId;
			string planetUID = tag.DataList[0];
			base.CloseNoTransition(null);
			galaxyViewController.TransitionToPlanet(galaxyPlanetController.GetPlanet(planetUID), true);
			TournamentTiersScreen highestLevelScreen = screenController.GetHighestLevelScreen<TournamentTiersScreen>();
			if (highestLevelScreen != null)
			{
				highestLevelScreen.CloseNoTransition(null);
			}
			if (actionId == "planet")
			{
				PlanetLootTableScreen highestLevelScreen2 = screenController.GetHighestLevelScreen<PlanetLootTableScreen>();
				if (highestLevelScreen2 != null)
				{
					highestLevelScreen2.CloseNoTransition(null);
					return;
				}
			}
			else if (actionId == "galaxy")
			{
				PlanetDetailsScreen highestLevelScreen3 = screenController.GetHighestLevelScreen<PlanetDetailsScreen>();
				if (highestLevelScreen3 != null)
				{
					highestLevelScreen3.GoToGalaxyFromPlanetScreen();
				}
			}
		}

		private void OnFinishClicked(UXButton button)
		{
			if (this.activeContract == null)
			{
				return;
			}
			int crystalCostToFinishContract = ContractUtils.GetCrystalCostToFinishContract(this.activeContract);
			if (crystalCostToFinishContract >= GameConstants.CRYSTAL_SPEND_WARNING_MINIMUM)
			{
				FinishNowScreen.ShowModal(this.selectedBuilding, new OnScreenModalResult(this.FinishContract), null);
				return;
			}
			this.FinishContract(this.selectedBuilding, null);
		}

		private void FinishContract(object result, object cookie)
		{
			if (this.activeContract == null || result == null)
			{
				return;
			}
			int crystalCostToFinishContract = ContractUtils.GetCrystalCostToFinishContract(this.activeContract);
			if (!GameUtils.SpendCrystals(crystalCostToFinishContract))
			{
				return;
			}
			Service.Get<ISupportController>().BuyOutCurrentBuildingContract(this.selectedBuilding, true);
			BuildingComponent buildingComp = ((SmartEntity)this.selectedBuilding).BuildingComp;
			if (buildingComp != null)
			{
				BuildingTypeVO buildingType = buildingComp.BuildingType;
				if (buildingType != null)
				{
					int currencyAmount = -crystalCostToFinishContract;
					string itemType = StringUtils.ToLowerCaseUnderscoreSeperated(buildingType.Type.ToString());
					string buildingID = buildingType.BuildingID;
					int itemCount = 1;
					string type = "speed_up_building";
					string subType = "consumable";
					Service.Get<DMOAnalyticsController>().LogInAppCurrencyAction(currencyAmount, itemType, buildingID, itemCount, type, subType);
				}
			}
			this.CloseFromResearchScreen();
		}

		private void OnPayMeForCurrencyResult(object result, object cookie)
		{
			if (GameUtils.HandleSoftCurrencyFlow(result, cookie))
			{
				this.StartUpgrading();
			}
		}

		protected internal DeployableInfoScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).CheckActiveContract();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).CloseFromResearchScreen();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).DisableTimers();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).EnableTimers();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).FillPlanetsUnlockGrid((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).FinishContract(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).ShowCurrencyTray);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).WantTransitions);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).GetNextDeployable((IDeployableVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).GetOwnLabelID());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).GoToGalaxyFromGalaxyState((DeployableInfoActionButtonTag)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).GoToGalaxyFromHomeState((DeployableInfoActionButtonTag)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).HideUnlockUnitGalaxyUI();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).IsNotMaxLevel((IDeployableVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnBackButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnFinishClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnGoToGalaxyClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnPayMeForCurrencyResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnPrevOrNextButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnPurchaseClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).Redraw();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshResearchContextButtons();
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshStarshipView((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args), (DeployableInfoUIType)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshStarshipViewNoUpgradeAsk((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshStarshipViewUpgradeAsk((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshTroopView((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args), (DeployableInfoUIType)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshTroopViewNoUpgradeAsk((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshTroopViewUpgradeAsk((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetTrainingCost((IDeployableVO)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupAndEnableUnlockUnitGalaxyUI((IDeployableVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupBar(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4));
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupBar3();
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupLeftTableAltAbilityItem((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupLeftTableItem(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupPerksButton();
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupShardProgressBar(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupTroopImage();
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).StartUpgrading();
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).ToggleParentScreenVisibility(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((DeployableInfoScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateContractTimers();
			return -1L;
		}
	}
}
