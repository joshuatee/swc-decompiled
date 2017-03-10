using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class TurretUpgradeScreen : TurretInfoScreen
	{
		public const string BUTTON_CANCEL = "BtnCancel";

		private const string BUTTON_CONFIRM = "BtnConfirm";

		private const string LABEL_CANCEL = "LabelBtnCancel";

		private const string LABEL_CONFIRM = "LabelBtnConfirm";

		private const string LABEL_SWAP = "LabelSwap";

		private const string UPGRADE_COST = "CostTurretUpgrade";

		private const string GRID_SWAP = "SwapTypeGrid";

		private const string SWAP_ITEM_BUTTON_INFO = "BtnItemInfoTurret";

		private const string SWAP_ITEM_CHECKBOX = "TurretItemCard";

		private const string SWAP_ITEM_COST = "CostTurretSwap";

		private const string SWAP_ITEM_GROUP_COUNT = "CountAndBuildTime";

		private const string SWAP_ITEM_GROUP_INFO = "InfoTextTurret";

		private const string SWAP_ITEM_GROUP_MAIN = "ItemInfoTurret";

		private const string SWAP_ITEM_ICON = "SpriteItemImageTurret";

		private const string SWAP_ITEM_LABEL_COUNT = "LabelSwapItemCount";

		private const string SWAP_ITEM_LABEL_INFO = "LabelItemInfoTurret";

		private const string SWAP_ITEM_LABEL_NAME = "LabelName";

		private const string SWAP_ITEM_LABEL_REQ = "LabelItemRequirement";

		private const string SWAP_ITEM_LABEL_TIME = "LabelBuildTime";

		private const string SWAP_ITEM_TEMPLATE = "SwapTypeTemplate";

		private const string GROUP_SWAP_TYPE = "SwapType";

		private const int MAX_SWAP_COST_LENGTH = 150;

		private UXElement infoTurretGroup;

		private UXElement swapTypeGroup;

		private UXLabel swapLabel;

		private UXButton swapConfirmButton;

		private UXButton swapCancelButton;

		private UXGrid itemGrid;

		private TurretTypeVO turretInfo;

		private StoreItemTag selectedTurret;

		private bool showSwapPageOnly;

		public TurretUpgradeScreen(Entity selectedBuilding, bool showSwapPageOnly) : base(selectedBuilding)
		{
			this.useUpgradeGroup = true;
			this.useTurretGroup = true;
			this.showSwapPageOnly = showSwapPageOnly;
			this.selectedTurret = null;
		}

		public override void OnDestroyElement()
		{
			if (this.itemGrid != null)
			{
				this.itemGrid.Clear();
				this.itemGrid = null;
			}
			base.OnDestroyElement();
		}

		protected override void InitGroups()
		{
			base.InitGroups();
			base.GetElement<UXElement>("SwapGroup").Visible = true;
			this.infoTurretGroup = base.GetElement<UXElement>("InfoTurret");
			this.swapTypeGroup = base.GetElement<UXElement>("SwapType");
		}

		protected override void InitLabels()
		{
			base.InitLabels();
			this.swapLabel = base.GetElement<UXLabel>("LabelSwap");
			this.upgradeButton = base.GetElement<UXButton>("BtnTurretUpgrade");
			if (this.nextBuildingInfo != null)
			{
				this.upgradeButton.Visible = true;
				UXUtils.SetupCostElements(this, "CostTurretUpgrade", null, this.nextBuildingInfo.UpgradeCredits, this.nextBuildingInfo.UpgradeMaterials, this.nextBuildingInfo.UpgradeContraband, 0, !this.reqMet, null, 150);
			}
		}

		protected override void InitButtons()
		{
			base.InitButtons();
			this.swapConfirmButton = base.GetElement<UXButton>("BtnConfirm");
			this.swapConfirmButton.OnClicked = new UXButtonClickedDelegate(this.OnSwapConfirmButtonClicked);
			this.swapCancelButton = base.GetElement<UXButton>("BtnCancel");
			this.swapCancelButton.OnClicked = new UXButtonClickedDelegate(this.OnSwapCancelButtonClicked);
		}

		protected override IGeometryVO GetImageGeometryConfig()
		{
			if (this.showSwapPageOnly)
			{
				return this.buildingInfo;
			}
			return base.GetImageGeometryConfig();
		}

		protected override void OnLoaded()
		{
			base.InitControls(2);
			this.InitHitpoints(0);
			base.UpdateDps(1);
			base.SetupDetailsTable();
			this.UpdatePage(this.showSwapPageOnly);
		}

		private void OnSwapButtonClicked(UXButton button)
		{
			this.UpdatePage(true);
		}

		private void UpdateTitleText(bool isSwapping)
		{
			UXLabel element = base.GetElement<UXLabel>("DialogBldgUpgradeTitle");
			UXElement element2 = base.GetElement<UXElement>("UpgradeTime");
			element2.Visible = !isSwapping;
			if (isSwapping)
			{
				element.Text = this.lang.Get("BUILDING_INFO", new object[]
				{
					LangUtils.GetBuildingDisplayName(this.buildingInfo),
					this.buildingInfo.Lvl
				});
				return;
			}
			element.Text = this.lang.Get("BUILDING_UPGRADE", new object[]
			{
				LangUtils.GetBuildingDisplayName(this.nextBuildingInfo),
				this.nextBuildingInfo.Lvl
			});
			UXLabel element3 = base.GetElement<UXLabel>("LabelUpgradeTime");
			element3.Text = GameUtils.GetTimeLabelFromSeconds(this.nextBuildingInfo.UpgradeTime);
		}

		private void SetupProjectorWithUpdatedConfig(IGeometryVO geometryVO)
		{
			UXSprite frameSprite = this.projector.Config.FrameSprite;
			this.projector.Destroy();
			ProjectorConfig config = ProjectorUtils.GenerateBuildingConfig(geometryVO as BuildingTypeVO, frameSprite);
			this.projector = ProjectorUtils.GenerateProjector(config);
		}

		private void UpdatePage(bool showSwapPage)
		{
			this.swapLabel.Visible = showSwapPage;
			this.swapTypeGroup.Visible = showSwapPage;
			this.infoTurretGroup.Visible = !showSwapPage;
			IGeometryVO arg_3A_0;
			if (!showSwapPage)
			{
				arg_3A_0 = this.GetImageGeometryConfig();
			}
			else
			{
				IGeometryVO buildingInfo = this.buildingInfo;
				arg_3A_0 = buildingInfo;
			}
			IGeometryVO geometryVO = arg_3A_0;
			this.SetupProjectorWithUpdatedConfig(geometryVO);
			if (showSwapPage)
			{
				this.ShowSwapConfirmAndCancelButtons();
				this.SetupGrid();
				this.UpdateTitleText(true);
				this.UpdateHQUpgradeDesc(false);
				return;
			}
			this.ShowSwapAndUpgradeButtons();
			this.UpdateTitleText(false);
			this.UpdateHQUpgradeDesc(true);
		}

		private void UpdateHQUpgradeDesc(bool showText)
		{
			if (!showText || !this.useUpgradeGroup || this.reqMet || this.reqBuildingInfo == null)
			{
				this.labelHQUpgradeDesc.Visible = false;
				return;
			}
			this.labelHQUpgradeDesc.Visible = true;
		}

		private void ShowSwapAndUpgradeButtons()
		{
			if (Service.Get<BuildingLookupController>().IsTurretSwappingUnlocked() && !ContractUtils.IsBuildingConstructing(this.selectedBuilding) && !ContractUtils.IsBuildingUpgrading(this.selectedBuilding) && !ContractUtils.IsBuildingSwapping(this.selectedBuilding) && !Service.Get<PostBattleRepairController>().IsEntityInRepair(this.selectedBuilding))
			{
				this.buttonPrimaryAction.Visible = false;
				this.buttonSwap.Visible = true;
				this.buttonSwap.Enabled = true;
				this.buttonSwap.OnClicked = new UXButtonClickedDelegate(this.OnSwapButtonClicked);
				this.swapConfirmButton.Visible = false;
				this.upgradeButton.Visible = true;
				this.upgradeButton.Enabled = this.reqMet;
				this.upgradeButton.OnClicked = new UXButtonClickedDelegate(this.OnUpgradeButtonClicked);
				this.swapCancelButton.Visible = false;
				this.buttonInstantBuy.Visible = GameConstants.ENABLE_INSTANT_BUY;
				return;
			}
			this.buttonPrimaryAction.Visible = true;
			this.buttonPrimaryAction.OnClicked = new UXButtonClickedDelegate(this.OnUpgradeButtonClicked);
			this.buttonInstantBuy.Visible = GameConstants.ENABLE_INSTANT_BUY;
			this.upgradeButton.Visible = false;
			this.buttonSwap.Visible = false;
			this.swapConfirmButton.Visible = false;
			this.swapCancelButton.Visible = false;
		}

		private void ShowSwapConfirmAndCancelButtons()
		{
			this.buttonPrimaryAction.Visible = false;
			this.upgradeButton.Visible = false;
			this.buttonInstantBuy.Visible = false;
			this.swapCancelButton.Visible = true;
			this.buttonSwap.Visible = false;
		}

		private void SetupGrid()
		{
			if (this.itemGrid != null)
			{
				return;
			}
			this.itemGrid = base.GetElement<UXGrid>("SwapTypeGrid");
			this.itemGrid.SetTemplateItem("SwapTypeTemplate");
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			FactionType faction = currentPlayer.Faction;
			IDataController dataController = Service.Get<IDataController>();
			BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
			UnlockController unlockController = Service.Get<UnlockController>();
			foreach (BuildingTypeVO current in dataController.GetAll<BuildingTypeVO>())
			{
				if (current.Type == BuildingType.Turret && current.Faction == faction && current.StoreTab != StoreTab.NotInStore)
				{
					BuildingTypeVO byLevel = buildingUpgradeCatalog.GetByLevel(current.UpgradeGroup, this.buildingInfo.Lvl);
					BuildingTypeVO buildingTypeVO = null;
					bool flag = unlockController.IsUnlocked(current, 1, out buildingTypeVO);
					if (flag || !byLevel.HideIfLocked)
					{
						StoreItemTag storeItemTag = new StoreItemTag();
						storeItemTag.BuildingInfo = byLevel;
						string uid = byLevel.Uid;
						UXElement uXElement = this.itemGrid.CloneTemplateItem(uid);
						uXElement.Tag = storeItemTag;
						storeItemTag.MainElement = this.itemGrid.GetSubElement<UXElement>(uid, "ItemInfoTurret");
						UXLabel subElement = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemInfoTurret");
						subElement.Text = LangUtils.GetBuildingDescription(byLevel);
						storeItemTag.InfoGroup = this.itemGrid.GetSubElement<UXButton>(uid, "InfoTextTurret");
						storeItemTag.InfoGroup.Visible = false;
						storeItemTag.InfoGroup.OnClicked = new UXButtonClickedDelegate(this.OnSwapInfoLabelButtonClicked);
						storeItemTag.InfoGroup.Tag = storeItemTag;
						UXButton subElement2 = this.itemGrid.GetSubElement<UXButton>(uid, "BtnItemInfoTurret");
						subElement2.OnClicked = new UXButtonClickedDelegate(this.OnSwapInfoButtonClicked);
						subElement2.Tag = storeItemTag;
						UXSprite subElement3 = this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteItemImageTurret");
						ProjectorConfig projectorConfig = ProjectorUtils.GenerateBuildingConfig(byLevel, subElement3);
						projectorConfig.AnimPreference = AnimationPreference.NoAnimation;
						ProjectorUtils.GenerateProjector(projectorConfig);
						UXLabel subElement4 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemRequirement");
						subElement4.Visible = !flag;
						UXElement subElement5 = this.itemGrid.GetSubElement<UXElement>(uid, "CountAndBuildTime");
						subElement5.Visible = flag;
						if (flag)
						{
							int swapCredits = byLevel.SwapCredits;
							int swapMaterials = byLevel.SwapMaterials;
							int swapContraband = byLevel.SwapContraband;
							UXUtils.SetupCostElements(this, "CostTurretSwap", uid, swapCredits, swapMaterials, swapContraband, 0, !flag, null, 150);
							UXLabel subElement6 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelBuildTime");
							subElement6.Text = GameUtils.GetTimeLabelFromSeconds(byLevel.SwapTime);
							UXLabel subElement7 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelSwapItemCount");
							subElement7.Text = "";
						}
						else if (buildingTypeVO != null)
						{
							subElement4.Text = this.lang.Get("BUILDING_REQUIREMENT", new object[]
							{
								buildingTypeVO.Lvl,
								LangUtils.GetBuildingDisplayName(buildingTypeVO)
							});
							UXUtils.SetupCostElements(this, "CostTurretSwap", uid, 0, 0, 0, 0, !flag, this.lang.Get("s_Locked", new object[0]), 150);
						}
						UXCheckbox subElement8 = this.itemGrid.GetSubElement<UXCheckbox>(uid, "TurretItemCard");
						subElement8.Enabled = flag;
						subElement8.OnSelected = new UXCheckboxSelectedDelegate(this.OnSwapItemCheckboxSelected);
						subElement8.Selected = (byLevel.Uid == this.buildingInfo.Uid);
						subElement8.Tag = storeItemTag;
						if (subElement8.Selected)
						{
							this.OnSwapItemCheckboxSelected(subElement8, true);
						}
						this.itemGrid.AddItem(uXElement, byLevel.Order);
					}
				}
			}
			this.itemGrid.RepositionItems();
		}

		private void OnSwapInfoButtonClicked(UXButton button)
		{
			StoreItemTag storeItemTag = button.Tag as StoreItemTag;
			storeItemTag.MainElement.Visible = false;
			storeItemTag.InfoGroup.Visible = true;
			Service.Get<EventManager>().SendEvent(EventId.InfoButtonClicked, null);
		}

		private void OnSwapInfoLabelButtonClicked(UXButton button)
		{
			StoreItemTag storeItemTag = button.Tag as StoreItemTag;
			storeItemTag.InfoGroup.Visible = false;
			storeItemTag.MainElement.Visible = true;
		}

		private void OnSwapItemCheckboxSelected(UXCheckbox checkbox, bool selected)
		{
			if (!selected)
			{
				return;
			}
			this.selectedTurret = (checkbox.Tag as StoreItemTag);
			BuildingTypeVO buildingInfo = this.selectedTurret.BuildingInfo;
			bool flag = buildingInfo.Uid != this.buildingInfo.Uid;
			this.swapLabel.Text = (flag ? this.lang.Get("CHANGE_TURRET_TO", new object[]
			{
				LangUtils.GetBuildingDisplayName(buildingInfo)
			}) : this.lang.Get("CHANGE_TURRET", new object[0]));
			this.swapConfirmButton.Visible = flag;
		}

		private void OnSwapPayMeForCurrencyResult(object result, object cookie)
		{
			if (GameUtils.HandleSoftCurrencyFlow(result, cookie) && !PayMeScreen.ShowIfNoFreeDroids(new OnScreenModalResult(this.OnSwapPayMeForDroidResult), null))
			{
				this.OnSwapStartContractSuccess();
			}
		}

		private void OnSwapPayMeForDroidResult(object result, object cookie)
		{
			if (result != null)
			{
				this.OnSwapStartContractSuccess();
			}
		}

		private void OnSwapConfirmButtonClicked(UXButton button)
		{
			if (this.selectedTurret == null)
			{
				return;
			}
			BuildingTypeVO buildingInfo = this.selectedTurret.BuildingInfo;
			int swapCredits = buildingInfo.SwapCredits;
			int swapMaterials = buildingInfo.SwapMaterials;
			int swapContraband = buildingInfo.SwapContraband;
			string buildingPurchaseContext = GameUtils.GetBuildingPurchaseContext(buildingInfo, this.buildingInfo, false, true);
			if (PayMeScreen.ShowIfNotEnoughCurrency(swapCredits, swapMaterials, swapContraband, buildingPurchaseContext, new OnScreenModalResult(this.OnSwapPayMeForCurrencyResult)))
			{
				return;
			}
			if (PayMeScreen.ShowIfNoFreeDroids(new OnScreenModalResult(this.OnSwapPayMeForDroidResult), null))
			{
				return;
			}
			this.OnSwapStartContractSuccess();
		}

		private void OnSwapStartContractSuccess()
		{
			Service.Get<ISupportController>().StartTurretCrossgrade(this.selectedTurret.BuildingInfo, this.selectedBuilding);
			this.Close(this.selectedTurret.BuildingInfo.Uid);
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(this.selectedBuilding.Get<BuildingComponent>().BuildingTO.Key);
			if (contract.TotalTime > 0)
			{
				Service.Get<UXController>().HUD.ShowContextButtons(this.selectedBuilding);
			}
		}

		private void OnSwapCancelButtonClicked(UXButton button)
		{
			if (this.showSwapPageOnly)
			{
				this.Close(null);
				return;
			}
			this.UpdatePage(false);
		}

		protected internal TurretUpgradeScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).GetImageGeometryConfig());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).InitGroups();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).InitLabels();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnLoaded();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnSwapButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnSwapCancelButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnSwapConfirmButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnSwapInfoButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnSwapInfoLabelButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnSwapItemCheckboxSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnSwapPayMeForCurrencyResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnSwapPayMeForDroidResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnSwapStartContractSuccess();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).SetupGrid();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).SetupProjectorWithUpdatedConfig((IGeometryVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).ShowSwapAndUpgradeButtons();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).ShowSwapConfirmAndCancelButtons();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateHQUpgradeDesc(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).UpdatePage(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((TurretUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateTitleText(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
