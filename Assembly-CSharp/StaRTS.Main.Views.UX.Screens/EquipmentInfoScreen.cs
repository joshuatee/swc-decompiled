using Net.RichardLord.Ash.Core;
using StaRTS.Externals.DMOAnalytics;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class EquipmentInfoScreen : SelectedBuildingScreen, IViewFrameTimeObserver
	{
		private const int NUM_DETAIL_LEFT_ROWS = 4;

		private const int NUM_BARS = 3;

		private const string S_TIME_REMAINING = "s_TimeRemaining";

		protected const string BUTTON_BACK = "BtnBack";

		private const string BUTTON_TROOP_NEXT = "BtnTroopNext";

		private const string BUTTON_TROOP_PREV = "BtnTroopPrev";

		private const string LABEL_NAME = "DialogBldgUpgradeTitle";

		private const string LABEL_INFO = "LabelTroopInfo";

		private const string LABEL_REQUIREMENT = "LabelRequirement";

		private const string UPGRADE_GROUP = "ContainerUpgradeTime";

		private const string CONTEXT_FINISH_GROUP = "FinishCost";

		private const string BUTTON_PURCHASE = "ButtonPrimaryAction";

		private const string BUTTON_FINISH = "BtnFinish";

		private const string BUTTON_NORMAL = "BtnNormal";

		private const string TROOP_IMAGE = "TroopImage";

		private const string TROOP_IMAGE_FORMAT = "TroopImageQ{0}";

		private const string TROOP_IMAGE_BACKGROUND_FORMAT = "SpriteTroopImageBkgGridQ{0}";

		private const string PLANET_PANEL = "PanelPlanetAvailability";

		private const string LABEL_PLANET_AVAILABILITY = "LabelPlanetAvailability";

		private const string GRID_PLANET_AVAILABILITY = "GridPlanetAvailability";

		private const string TEMPLATE_PLANET = "TemplatePlanet";

		private const string LABEL_AVAILABLE_PLANET = "LabelAvailablePlanet";

		private const string TEXTURE_AVAILABLE_PLANET = "TextureAvailablePlanet";

		private const string BUILDING_REQUIREMENT = "BUILDING_REQUIREMENT";

		private const string LABEL_UPGRADE_TIME = "LabelUpgradeTime";

		private const string LABEL_UPGRADE_TIME_STATIC = "LabelUpgradeTimeStatic";

		private const string DAMAGE = "DAMAGE";

		private const string HEALTH = "HEALTH";

		private const string ICON_UPGRADE = "IconUpgrade";

		private const string SLIDER_NAME = "pBar{0}";

		private const string LABEL_PBAR = "LabelpBar{0}";

		private const string LABEL_PBAR_CUR = "LabelpBar{0}Current";

		private const string LABEL_PBAR_NEXT = "LabelpBar{0}Next";

		private const string SLIDER_CURRENT = "pBarCurrent{0}";

		private const string SLIDER_NEXT = "pBarNext{0}";

		private const string SLIDER_BG = "SpritepBarBkg{0}";

		private const string MOVEMENT_SPEED_GROUP = "MovementSpeed";

		private const string RANGE_GROUP = "Range";

		private const string TRAINING_TIME_GROUP = "TrainingTime";

		private const string TRAINING_COST_GROUP = "TrainingCost";

		private const string UNIT_CAPACITY_GROUP = "UnitCapacity";

		private const string SPRITE_EQUIPMENT = "SpriteTroopSelectedItemImageQ{0}";

		private const string LEFT_OWN_LABEL = "LabelQuantityOwnQ{0}";

		private const string LEFT_INFO_PROGRESS_LABEL = "LabelProgress";

		private const string SHARD_PROGRESS_BAR = "pBarFrag";

		private const string SHARD_PROGRESS_BAR_SPRITE = "SpritepBarFrag";

		private const string LEFT_INFO_GROUP = "InfoRow{0}";

		private const string LEFT_INFO_TITLE = "InfoTitle{0}";

		private const string LEFT_INFO_DETAIL = "InfoDetail{0}";

		private const string LEFT_INFO_GROUP3_ALT = "InfoRow3alt";

		private const string LEFT_LABEL_QUALITY = "LabelQualityQ{0}";

		private const string LEFT_UPGRADE_INSTRUCTIONS = "ItemStatus";

		private const string LABEL_NORMAL_INTRO = "LabelNormalIntro";

		private const string RESEARCH_LAB_ACTIVE_PLANETS = "RESEARCH_LAB_ACTIVE_PLANETS";

		private const string PLUS_DAMAGE_PERCENT = "perkEffect_descMod_PosPct";

		private const string LABEL_REWARD_UPGRADE = "LABEL_REWARD_UPGRADE";

		private const string EQUIPMENT_REQUIRES_BUILDING = "EQUIPMENT_REQUIRES_BUILDING";

		private const string BUILDING_UPGRADE = "BUILDING_UPGRADE";

		private const string EQUIPMENT_LOCKED = "EQUIPMENT_LOCKED";

		private const string MAX_LEVEL = "MAX_LEVEL";

		private const string BUILDING_INFO = "BUILDING_INFO";

		private const string FRACTION = "FRACTION";

		private const string S_UPGRADE_TIME = "s_upgradeTime";

		private const string PERCENTAGE = "PERCENTAGE";

		private const string AFFECTED_UNIT = "EQUIPMENT_INFO_AFFECTED_UNIT";

		private const string CAPACITY = "EQUIPMENT_INFO_CAPACITY";

		private const string ARMORY_ACTIVATE = "ARMORY_ACTIVATE";

		private const string ARMORY_DEACTIVATE = "ARMORY_DEACTIVATE";

		private const string ARMORY_INVALID_EQUIPMENT_PLANET = "ARMORY_INVALID_EQUIPMENT_PLANET";

		private const string ARMORY_NOT_ENOUGH_CAPACITY = "ARMORY_NOT_ENOUGH_CAPACITY";

		private const string EQUIPMENT_UPGRADE_REQ = "EQUIPMENT_UPGRADE_LOCKED";

		private const string UPGRADE_EQUIPMENT_IN_LAB = "ARMORY_UPGRADE_CTA";

		protected EquipmentVO selectedEquipment;

		private List<EquipmentVO> equipmentList;

		private EquipmentVO nextEquipmentVoUpgrade;

		protected GeometryProjector equipmentImage;

		protected bool wantsTransition;

		protected bool shouldCloseParent;

		protected bool forResearchLab;

		private Contract activeContract;

		private bool timerActive;

		private float accumulatedUpdateDt;

		protected UXLabel labelUpgradeTime;

		protected override bool WantTransitions
		{
			get
			{
				return this.wantsTransition;
			}
		}

		public EquipmentInfoScreen(EquipmentVO selectedEquipment, List<EquipmentVO> equipmentList, Entity selectedBuilding, bool forResearchLab) : base("gui_troop_info", selectedBuilding)
		{
			this.wantsTransition = false;
			this.shouldCloseParent = true;
			this.selectedEquipment = selectedEquipment;
			EquipmentUpgradeCatalog equipmentUpgradeCatalog = Service.Get<EquipmentUpgradeCatalog>();
			this.nextEquipmentVoUpgrade = equipmentUpgradeCatalog.GetNextLevel(selectedEquipment);
			this.equipmentList = equipmentList;
			this.forResearchLab = forResearchLab;
			if (forResearchLab)
			{
				this.CheckActiveContract();
			}
			this.accumulatedUpdateDt = 0f;
		}

		public override void Close(object modalResult)
		{
			if (this.shouldCloseParent)
			{
				this.wantsTransition = true;
				ClosableScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<ArmoryScreen>();
				if (highestLevelScreen == null)
				{
					highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<TroopUpgradeScreen>();
				}
				if (highestLevelScreen != null)
				{
					highestLevelScreen.Close(null);
				}
			}
			base.Close(modalResult);
		}

		public override void OnDestroyElement()
		{
			this.labelUpgradeTime = null;
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ContractCompleted);
			this.DisableTimers();
			if (this.equipmentImage != null)
			{
				this.equipmentImage.Destroy();
				this.equipmentImage = null;
			}
			this.equipmentList = null;
			this.selectedEquipment = null;
			this.nextEquipmentVoUpgrade = null;
			base.OnDestroyElement();
		}

		protected override void OnScreenLoaded()
		{
			base.OnScreenLoaded();
			this.ToggleParentScreenVisibility(false);
			this.InitButtons();
			UXButton element = base.GetElement<UXButton>("BtnTroopPrev");
			UXButton element2 = base.GetElement<UXButton>("BtnTroopNext");
			if (this.equipmentList != null && this.equipmentList.Count > 1)
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
			this.labelUpgradeTime = base.GetElement<UXLabel>("LabelUpgradeTime");
			Service.Get<EventManager>().RegisterObserver(this, EventId.ContractCompleted);
			this.Redraw();
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ContractCompleted)
			{
				ContractEventData contractEventData = cookie as ContractEventData;
				ContractType contractType = contractEventData.Contract.ContractTO.ContractType;
				if (contractType == ContractType.Research)
				{
					this.CheckActiveContract();
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
			if (this.nextEquipmentVoUpgrade != null && this.nextEquipmentVoUpgrade.Uid == this.activeContract.ProductUid)
			{
				int remainingTimeForView = this.activeContract.GetRemainingTimeForView();
				if (remainingTimeForView > 0)
				{
					this.labelUpgradeTime.Text = GameUtils.GetTimeLabelFromSeconds(remainingTimeForView);
					int crystalCostToFinishContract = ContractUtils.GetCrystalCostToFinishContract(this.activeContract);
					UXUtils.SetupCostElements(this, "FinishCost", null, 0, 0, 0, crystalCostToFinishContract, 0, !ArmoryUtils.IsBuildingRequirementMet(this.nextEquipmentVoUpgrade), null);
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

		private void Redraw()
		{
			IDataController sdc = Service.Get<IDataController>();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			for (int i = 1; i <= 3; i++)
			{
				base.GetElement<UXElement>(string.Format("TroopImageQ{0}", new object[]
				{
					i
				})).Visible = false;
			}
			int quality = (int)this.selectedEquipment.Quality;
			base.GetElement<UXElement>(string.Format("TroopImageQ{0}", new object[]
			{
				quality
			})).Visible = true;
			base.GetElement<UXElement>(string.Format("SpriteTroopImageBkgGridQ{0}", new object[]
			{
				quality
			})).Visible = true;
			base.GetElement<UXElement>("TroopImage").Visible = false;
			int quality2 = (int)this.selectedEquipment.Quality;
			base.GetElement<UXLabel>(string.Format("LabelQualityQ{0}", new object[]
			{
				quality2
			})).Text = LangUtils.GetShardQuality(this.selectedEquipment.Quality);
			UXSprite element = base.GetElement<UXSprite>(string.Format("SpriteTroopSelectedItemImageQ{0}", new object[]
			{
				quality2
			}));
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateEquipmentConfig(this.selectedEquipment, element, true);
			projectorConfig.AnimPreference = AnimationPreference.AnimationPreferred;
			projectorConfig.buildingEquipmentShaderName = "UnlitTexture_Fade";
			this.equipmentImage = ProjectorUtils.GenerateProjector(projectorConfig);
			this.SetProgressBarElements(currentPlayer, quality2);
			base.SetupFragmentSprite(quality2);
			base.GetElement<UXLabel>("LabelTroopInfo").Text = this.lang.Get(this.selectedEquipment.EquipmentDescription, new object[0]);
			base.GetElement<UXLabel>(string.Format("LabelQuantityOwnQ{0}", new object[]
			{
				quality2
			})).Visible = false;
			this.PopulateAvailablePlanetsPanel(sdc);
			this.SetUpUpgradeElements(false);
			base.GetElement<UXElement>("MovementSpeed").Visible = false;
			base.GetElement<UXElement>("Range").Visible = false;
			base.GetElement<UXElement>("UnitCapacity").Visible = false;
			base.GetElement<UXElement>("TrainingTime").Visible = false;
			base.GetElement<UXElement>("TrainingCost").Visible = false;
			base.GetElement<UXElement>("InfoRow3alt").Visible = false;
			base.GetElement<UXButton>("BtnFinish").Visible = false;
			base.GetElement<UXButton>("ButtonPrimaryAction").Visible = false;
			this.DisplayBarsForEquipmentBuffs(sdc);
			if (this.forResearchLab)
			{
				this.SetUpResearchLabScreenInfo(sdc, currentPlayer);
			}
			else
			{
				this.SetUpArmoryScreenInfo(currentPlayer);
			}
			for (int j = 0; j < 4; j++)
			{
				this.SetupLeftTableItem(j, null, null);
			}
			string affectedUnit = this.GetAffectedUnit();
			if (affectedUnit != null)
			{
				this.SetupLeftTableItem(0, "EQUIPMENT_INFO_AFFECTED_UNIT", affectedUnit);
			}
			this.SetupLeftTableItem(1, "EQUIPMENT_INFO_CAPACITY", this.selectedEquipment.Size.ToString());
		}

		private void SetUpUpgradeElements(bool upgradeVisible)
		{
			base.GetElement<UXElement>("ContainerUpgradeTime").Visible = upgradeVisible;
			if (!upgradeVisible)
			{
				return;
			}
			UXLabel element = base.GetElement<UXLabel>("LabelUpgradeTime");
			base.GetElement<UXLabel>("LabelUpgradeTimeStatic").Text = this.lang.Get("s_upgradeTime", new object[0]);
			element.Text = GameUtils.GetTimeLabelFromSeconds(this.nextEquipmentVoUpgrade.UpgradeTime);
		}

		private void SetProgressBarElements(CurrentPlayer player, int quality)
		{
			UXSlider element = base.GetElement<UXSlider>("pBarFrag");
			UXSprite element2 = base.GetElement<UXSprite>("SpritepBarFrag");
			UXLabel element3 = base.GetElement<UXLabel>("LabelProgress");
			UXLabel element4 = base.GetElement<UXLabel>("ItemStatus");
			EquipmentVO nextLevel = Service.Get<EquipmentUpgradeCatalog>().GetNextLevel(this.selectedEquipment);
			UXElement element5 = base.GetElement<UXElement>("IconUpgrade");
			string equipmentID = this.selectedEquipment.EquipmentID;
			element.Visible = true;
			element4.Visible = false;
			if (nextLevel == null)
			{
				element3.Text = this.lang.Get("MAX_LEVEL", new object[0]);
				element.Value = 1f;
				element5.Visible = false;
				return;
			}
			bool flag;
			if (this.forResearchLab)
			{
				flag = (this.activeContract != null && this.activeContract.ProductUid.Equals(nextLevel.Uid));
			}
			else
			{
				Contract contract = Service.Get<ISupportController>().FindFirstContractWithProductUid(nextLevel.Uid);
				flag = (contract != null);
			}
			if (flag)
			{
				element.Visible = false;
				element5.Visible = false;
				return;
			}
			int num = player.Shards.ContainsKey(equipmentID) ? player.Shards[equipmentID] : 0;
			int upgradeShards;
			if (player.UnlockedLevels.Equipment.Has(this.selectedEquipment))
			{
				upgradeShards = nextLevel.UpgradeShards;
				element5.Visible = (num >= upgradeShards);
				if (!this.forResearchLab)
				{
					element4.Visible = true;
					element4.Text = ((num >= upgradeShards) ? this.lang.Get("ARMORY_UPGRADE_CTA", new object[0]) : this.lang.Get("EQUIPMENT_UPGRADE_LOCKED", new object[]
					{
						this.CalculateFragmentsNeededForUnlock(nextLevel.UpgradeShards, this.selectedEquipment.EquipmentID)
					}));
				}
			}
			else
			{
				upgradeShards = this.selectedEquipment.UpgradeShards;
				element5.Visible = false;
			}
			element3.Text = this.lang.Get("FRACTION", new object[]
			{
				num,
				upgradeShards
			});
			if (upgradeShards == 0)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("CMS Error: Shards required for {0} is zero", new object[]
				{
					nextLevel.Uid
				});
				return;
			}
			float sliderValue = (float)num / (float)upgradeShards;
			UXUtils.SetShardProgressBarValue(element, element2, sliderValue);
		}

		private void DisplayBarsForEquipmentBuffs(IDataController sdc)
		{
			int num = 1;
			int num2 = this.selectedEquipment.EffectUids.Length;
			int i = 0;
			int num3 = num2;
			while (i < num3)
			{
				EquipmentEffectVO equipmentEffectVO = sdc.Get<EquipmentEffectVO>(this.selectedEquipment.EffectUids[i]);
				EquipmentEffectVO equipmentEffectVO2 = null;
				if (this.nextEquipmentVoUpgrade != null && this.nextEquipmentVoUpgrade.EffectUids.Length != 0 && this.nextEquipmentVoUpgrade.EffectUids.Length > i)
				{
					equipmentEffectVO2 = sdc.Get<EquipmentEffectVO>(this.nextEquipmentVoUpgrade.EffectUids[i]);
				}
				ArmorType armorType = ArmorType.Invalid;
				if (equipmentEffectVO.AffectedBuildingIds != null && equipmentEffectVO.AffectedBuildingIds.Length != 0)
				{
					BuildingTypeVO minLevel = Service.Get<BuildingUpgradeCatalog>().GetMinLevel(equipmentEffectVO.AffectedBuildingIds[0]);
					if (minLevel != null)
					{
						armorType = minLevel.ArmorType;
					}
				}
				else if (equipmentEffectVO.AffectedTroopIds != null && equipmentEffectVO.AffectedTroopIds.Length != 0)
				{
					TroopTypeVO minLevel2 = Service.Get<TroopUpgradeCatalog>().GetMinLevel(equipmentEffectVO.AffectedTroopIds[0]);
					if (minLevel2 != null)
					{
						armorType = minLevel2.ArmorType;
					}
				}
				int j = 0;
				int num4 = equipmentEffectVO.BuffUids.Length;
				while (j < num4)
				{
					BuffTypeVO buffTypeVO = sdc.Get<BuffTypeVO>(equipmentEffectVO.BuffUids[j]);
					BuffTypeVO buffTypeVO2 = null;
					if (equipmentEffectVO2 != null && equipmentEffectVO2.BuffUids.Length != 0 && equipmentEffectVO2.BuffUids.Length > j)
					{
						buffTypeVO2 = sdc.Get<BuffTypeVO>(equipmentEffectVO2.BuffUids[j]);
					}
					int num5 = buffTypeVO.Values[(int)armorType];
					int nextValue = num5;
					if (ArmoryUtils.IsEquipmentOwned(Service.Get<CurrentPlayer>(), this.selectedEquipment))
					{
						nextValue = ((buffTypeVO2 == null) ? num5 : buffTypeVO2.Values[(int)armorType]);
					}
					string buffString = this.GetBuffString(buffTypeVO.Modify);
					this.SetupBar(num, buffString, num5, nextValue, 1, false);
					num++;
					if (num >= 3)
					{
						break;
					}
					j++;
				}
				if (num >= 3)
				{
					break;
				}
				i++;
			}
			for (int k = num; k <= 3; k++)
			{
				base.GetElement<UXElement>(string.Format("pBar{0}", new object[]
				{
					num
				})).Visible = false;
				num++;
			}
		}

		private void SetUpArmoryScreenInfo(CurrentPlayer player)
		{
			this.SetTitleText(base.GetElement<UXLabel>("DialogBldgUpgradeTitle"), "BUILDING_INFO", this.selectedEquipment.EquipmentName, this.selectedEquipment.Lvl);
			UXLabel element = base.GetElement<UXLabel>("LabelRequirement");
			UXLabel element2 = base.GetElement<UXLabel>("LabelNormalIntro");
			UXButton element3 = base.GetElement<UXButton>("BtnNormal");
			element3.Visible = true;
			element3.OnClicked = new UXButtonClickedDelegate(this.OnMainButtonClicked);
			if (!ArmoryUtils.IsEquipmentOwned(player, this.selectedEquipment))
			{
				element3.Enabled = false;
				element2.Text = this.lang.Get("ARMORY_ACTIVATE", new object[0]);
				element.Text = this.lang.Get("EQUIPMENT_LOCKED", new object[]
				{
					this.CalculateFragmentsNeededForUnlock(this.selectedEquipment.UpgradeShards, this.selectedEquipment.EquipmentID)
				});
				return;
			}
			if (ArmoryUtils.IsEquipmentActive(player, this.selectedEquipment))
			{
				element2.Text = this.lang.Get("ARMORY_DEACTIVATE", new object[0]);
				element3.Enabled = true;
			}
			else
			{
				bool flag = ArmoryUtils.HasEnoughCapacityToActivateEquipment(player.ActiveArmory, this.selectedEquipment);
				bool flag2 = ArmoryUtils.IsEquipmentValidForPlanet(this.selectedEquipment, player.PlanetId);
				element3.Enabled = (flag & flag2);
				element2.Text = this.lang.Get("ARMORY_ACTIVATE", new object[0]);
				if (!flag2)
				{
					string planetDisplayName = LangUtils.GetPlanetDisplayName(player.PlanetId);
					element.Text = this.lang.Get("ARMORY_INVALID_EQUIPMENT_PLANET", new object[]
					{
						planetDisplayName
					});
					return;
				}
				if (!flag)
				{
					element.Text = this.lang.Get("ARMORY_NOT_ENOUGH_CAPACITY", new object[0]);
					return;
				}
			}
			element.Text = string.Empty;
		}

		private void SetUpResearchLabScreenInfo(IDataController sdc, CurrentPlayer player)
		{
			UXLabel element = base.GetElement<UXLabel>("DialogBldgUpgradeTitle");
			UXButton element2 = base.GetElement<UXButton>("BtnNormal");
			element2.OnClicked = new UXButtonClickedDelegate(this.OnMainButtonClicked);
			UXButton element3 = base.GetElement<UXButton>("BtnFinish");
			element3.OnClicked = new UXButtonClickedDelegate(this.OnFinishClicked);
			UXLabel element4 = base.GetElement<UXLabel>("LabelRequirement");
			element4.Text = string.Empty;
			if (this.nextEquipmentVoUpgrade != null)
			{
				ArmoryController armoryController = Service.Get<ArmoryController>();
				this.SetTitleText(element, "BUILDING_UPGRADE", this.selectedEquipment.EquipmentName, this.nextEquipmentVoUpgrade.Lvl);
				element2.Visible = true;
				base.GetElement<UXLabel>("LabelNormalIntro").Text = this.lang.Get("LABEL_REWARD_UPGRADE", new object[0]);
				if (this.activeContract != null)
				{
					if (this.activeContract.ProductUid == this.nextEquipmentVoUpgrade.Uid)
					{
						element3.Visible = true;
						element2.Visible = false;
						element4.Visible = false;
						base.GetElement<UXLabel>("LabelUpgradeTimeStatic").Text = this.lang.Get("s_TimeRemaining", new object[0]);
						base.GetElement<UXElement>("ContainerUpgradeTime").Visible = true;
						this.UpdateContractTimers();
						return;
					}
					element2.VisuallyDisableButton();
					return;
				}
				else
				{
					if (armoryController.IsEquipmentUpgradeable(this.selectedEquipment, this.nextEquipmentVoUpgrade))
					{
						element2.VisuallyEnableButton();
						element2.Enabled = true;
						this.SetUpUpgradeElements(true);
						return;
					}
					this.SetTitleText(element, "BUILDING_INFO", this.selectedEquipment.EquipmentName, this.selectedEquipment.Lvl);
					element2.Enabled = false;
					BuildingTypeVO buildingTypeVO = (this.nextEquipmentVoUpgrade.BuildingRequirement == null) ? null : sdc.Get<BuildingTypeVO>(this.nextEquipmentVoUpgrade.BuildingRequirement);
					if (buildingTypeVO != null && !ArmoryUtils.IsBuildingRequirementMet(this.nextEquipmentVoUpgrade))
					{
						element4.Text = this.lang.Get("EQUIPMENT_REQUIRES_BUILDING", new object[]
						{
							LangUtils.GetBuildingDisplayName(buildingTypeVO),
							buildingTypeVO.Lvl
						});
						return;
					}
					if (!ArmoryUtils.IsEquipmentOwned(player, this.selectedEquipment))
					{
						element4.Text = this.lang.Get("EQUIPMENT_LOCKED", new object[]
						{
							this.CalculateFragmentsNeededForUnlock(this.selectedEquipment.UpgradeShards, this.selectedEquipment.EquipmentID)
						});
						return;
					}
					if (!ArmoryUtils.CanAffordEquipment(player, this.nextEquipmentVoUpgrade))
					{
						element4.Text = this.lang.Get("EQUIPMENT_UPGRADE_LOCKED", new object[]
						{
							this.CalculateFragmentsNeededForUnlock(this.nextEquipmentVoUpgrade.UpgradeShards, this.selectedEquipment.EquipmentID)
						});
						return;
					}
				}
			}
			else
			{
				element2.Visible = false;
				EquipmentVO maxLevel = Service.Get<EquipmentUpgradeCatalog>().GetMaxLevel(this.selectedEquipment);
				if (maxLevel == this.selectedEquipment)
				{
					this.SetTitleText(element, "BUILDING_INFO", this.selectedEquipment.EquipmentName, this.selectedEquipment.Lvl);
					element4.Text = this.lang.Get("MAX_LEVEL", new object[0]);
				}
			}
		}

		private void SetTitleText(UXLabel titleLabel, string titleKey, string nameKey, int level)
		{
			titleLabel.Text = this.lang.Get(titleKey, new object[]
			{
				this.lang.Get(nameKey, new object[0]),
				level
			});
		}

		private int CalculateFragmentsNeededForUnlock(int cost, string equipmentID)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (!currentPlayer.Shards.ContainsKey(equipmentID))
			{
				return cost;
			}
			return cost - currentPlayer.Shards[equipmentID];
		}

		private void PopulateAvailablePlanetsPanel(IDataController sdc)
		{
			UXElement element = base.GetElement<UXElement>("PanelPlanetAvailability");
			element.Visible = true;
			base.GetElement<UXLabel>("LabelPlanetAvailability").Text = this.lang.Get("RESEARCH_LAB_ACTIVE_PLANETS", new object[0]);
			UXGrid element2 = base.GetElement<UXGrid>("GridPlanetAvailability");
			element2.SetTemplateItem("TemplatePlanet");
			element2.Clear();
			int i = 0;
			int num = this.selectedEquipment.PlanetIDs.Length;
			while (i < num)
			{
				string uid = this.selectedEquipment.PlanetIDs[i];
				PlanetVO planetVO = sdc.Get<PlanetVO>(uid);
				UXElement item = element2.CloneTemplateItem(planetVO.Uid);
				element2.AddItem(item, planetVO.Order);
				element2.GetSubElement<UXLabel>(planetVO.Uid, "LabelAvailablePlanet").Text = LangUtils.GetPlanetDisplayName(uid);
				element2.GetSubElement<UXTexture>(planetVO.Uid, "TextureAvailablePlanet").LoadTexture(planetVO.LeaderboardButtonTexture);
				i++;
			}
			element2.RepositionItemsFrameDelayed();
		}

		private string GetAffectedUnit()
		{
			string[] effectUids = this.selectedEquipment.EffectUids;
			if (effectUids != null)
			{
				IDataController dataController = Service.Get<IDataController>();
				int i = 0;
				int num = effectUids.Length;
				while (i < num)
				{
					EquipmentEffectVO equipmentEffectVO = dataController.Get<EquipmentEffectVO>(effectUids[i]);
					if (equipmentEffectVO.AffectedTroopIds != null && equipmentEffectVO.AffectedTroopIds.Length != 0)
					{
						return this.lang.Get("trp_title_" + equipmentEffectVO.AffectedTroopIds[0], new object[0]);
					}
					if (equipmentEffectVO.AffectedSpecialAttackIds != null && equipmentEffectVO.AffectedSpecialAttackIds.Length != 0)
					{
						return this.lang.Get("shp_title_" + equipmentEffectVO.AffectedSpecialAttackIds[0], new object[0]);
					}
					if (equipmentEffectVO.AffectedBuildingIds != null && equipmentEffectVO.AffectedBuildingIds.Length != 0)
					{
						return this.lang.Get("bld_title_" + equipmentEffectVO.AffectedBuildingIds[0], new object[0]);
					}
					i++;
				}
			}
			return null;
		}

		private string GetBuffString(BuffModify buffType)
		{
			string id = string.Empty;
			if (buffType != BuffModify.Damage)
			{
				if (buffType == BuffModify.MaxHealth)
				{
					id = "HEALTH";
				}
			}
			else
			{
				id = "DAMAGE";
			}
			return this.lang.Get(id, new object[]
			{
				""
			});
		}

		protected override void SetupPerksButton()
		{
			base.GetElement<UXButton>("btnPerks").Visible = false;
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
				})).Text = this.lang.Get(title, new object[0]);
				base.GetElement<UXLabel>(string.Format("InfoDetail{0}", new object[]
				{
					index
				})).Text = desc;
			}
		}

		private void OnMainButtonClicked(UXButton button)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (!this.forResearchLab)
			{
				if (ArmoryUtils.IsEquipmentActive(currentPlayer, this.selectedEquipment))
				{
					Service.Get<ArmoryController>().DeactivateEquipment(this.selectedEquipment.EquipmentID);
				}
				else
				{
					Service.Get<ArmoryController>().ActivateEquipment(this.selectedEquipment.EquipmentID);
				}
				this.OnBackButtonClicked(null);
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
			Service.Get<ISupportController>().StartEquipmentUpgrade(this.nextEquipmentVoUpgrade, this.selectedBuilding);
			this.CloseFromResearchScreen();
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
					string type = "speed_up_upgrade";
					string subType = "equipment";
					Service.Get<DMOAnalyticsController>().LogInAppCurrencyAction(currencyAmount, itemType, buildingID, itemCount, type, subType);
				}
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
			Service.Get<UXController>().HUD.ShowContextButtons(this.selectedBuilding);
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
				deployableInfoParentScreen.Visible = visible;
			}
		}

		private void OnPrevOrNextButtonClicked(UXButton button)
		{
			int num = (int)button.Tag;
			int count = this.equipmentList.Count;
			int index = (num < 0) ? (count - 1) : 0;
			EquipmentVO equipmentVO = this.equipmentList[index];
			for (int i = count - 1; i >= 0; i--)
			{
				int index2 = (num < 0) ? (count - 1 - i) : i;
				EquipmentVO equipmentVO2 = this.equipmentList[index2];
				if (equipmentVO2 == this.selectedEquipment)
				{
					break;
				}
				equipmentVO = equipmentVO2;
			}
			this.selectedEquipment = equipmentVO;
			EquipmentUpgradeCatalog equipmentUpgradeCatalog = Service.Get<EquipmentUpgradeCatalog>();
			this.nextEquipmentVoUpgrade = equipmentUpgradeCatalog.GetNextLevel(this.selectedEquipment);
			this.Redraw();
		}

		protected void SetupBar(int index, string labelString, int currentValue, int nextValue, int maxValue, bool showBars)
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
			element2.Visible = showBars;
			element3.Visible = showBars;
			base.GetElement<UXSprite>(string.Format("SpritepBarBkg{0}", new object[]
			{
				index
			})).Visible = showBars;
			element3.Visible = (nextValue > currentValue & showBars);
			element2.Value = MathUtils.NormalizeRange((float)currentValue, 0f, (float)maxValue);
			element3.Value = MathUtils.NormalizeRange((float)nextValue, 0f, (float)maxValue);
			UXLabel element4 = base.GetElement<UXLabel>(string.Format("LabelpBar{0}Current", new object[]
			{
				index
			}));
			element4.Text = this.lang.Get("perkEffect_descMod_PosPct", new object[]
			{
				this.lang.ThousandsSeparated(currentValue)
			});
			UXLabel element5 = base.GetElement<UXLabel>(string.Format("LabelpBar{0}Next", new object[]
			{
				index
			}));
			element5.Visible = (this.forResearchLab && nextValue > currentValue);
			element5.Text = this.lang.Get("perkEffect_descMod_PosPct", new object[]
			{
				this.lang.ThousandsSeparated(nextValue - currentValue)
			});
		}

		protected internal EquipmentInfoScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).CalculateFragmentsNeededForUnlock(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).CheckActiveContract();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).CloseFromResearchScreen();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).DisableTimers();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).DisplayBarsForEquipmentBuffs((IDataController)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).EnableTimers();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).FinishContract(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).WantTransitions);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).GetAffectedUnit());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).GetBuffString((BuffModify)(*(int*)args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnBackButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnFinishClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnMainButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnPrevOrNextButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).PopulateAvailablePlanetsPanel((IDataController)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).Redraw();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetProgressBarElements((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetTitleText((UXLabel)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), *(int*)(args + 3));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetUpArmoryScreenInfo((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupBar(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4), *(sbyte*)(args + 5) != 0);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupLeftTableItem(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupPerksButton();
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetUpResearchLabScreenInfo((IDataController)GCHandledObjects.GCHandleToObject(*args), (CurrentPlayer)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetUpUpgradeElements(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).ToggleParentScreenVisibility(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((EquipmentInfoScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateContractTimers();
			return -1L;
		}
	}
}
