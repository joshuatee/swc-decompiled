using Net.RichardLord.Ash.Core;
using StaRTS.Externals.IAP;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class StoreScreen : ClosableScreen, IEventObserver, IViewClockTimeObserver
	{
		public delegate void AddBuildingItemDelegate(List<UXElement> list, BuildingTypeVO buildingInfo, BuildingTypeVO reqBuilding, bool reqMet, int curQuantity, int maxQuantity);

		private const string STORE_NAME = "gui_store_screen";

		private const string STORE_WIDGET = "gui_store_screen_main_widget";

		private const string PRIMARY_PAGE = "TabPage";

		private const string SECONDARY_PAGE = "CategoryPage";

		private const string BACK_BUTTON = "BtnBack";

		private const string PROMO_GROUP = "PromoContainer";

		private const string ITEM_GRID_TWO_ROWS_PARENT = "StoreItems2row";

		private const string ITEM_GRID_TWO_ROWS = "StoreGrid2row";

		private const string ITEM_TEMPLATE = "StoreItemTemplate";

		private const string ITEM_HEIGHT_GUIDE = "ItemHeightGuide";

		private const string ITEM_CELL_HEIGHT_GUIDE = "ItemCellHeightGuide";

		private const string ITEM_BUTTON = "ButtonItemCard";

		private const string ITEM_ICON = "SpriteItemImage";

		private const string ITEM_MAIN_ELEMENT = "ItemInfo";

		private const string ITEM_LABEL_NAME = "LabelName";

		private const string ITEM_LABEL_LEI_NAME = "LabelNameSpecial";

		private const string ITEM_LEI_TIMER_LABEL = "LabelTimerSpecial";

		private const string ITEM_LABEL_TIME = "LabelBuildTime";

		private const string ITEM_ICON_TIME = "SpriteItemTimeIcon";

		private const string ITEM_LABEL_COUNT = "LabelItemCount";

		private const string ITEM_LABEL_INFO = "LabelItemInfo";

		private const string ITEM_LABEL_REQ = "LabelItemRequirement";

		private const string ITEM_LABEL_REWARD = "LabelCurrencyAmount";

		private const string ITEM_GROUP_COUNTS = "CountAndBuildTime";

		private const string ITEM_BUTTON_INFO = "BtnItemInfo";

		protected const string ITEM_COST_GROUP = "Cost";

		private const string ITEM_LOCKED = "SpriteDim";

		private const string TABS_PARENT = "StoreTabs";

		private const string TAB_ARMY = "TabArmy";

		private const string TAB_PROTECTION = "TabShield";

		private const string TAB_DECORATIONS = "TabDecorations";

		private const string TAB_DEFENSES = "TabDefenses";

		private const string TAB_RESOURCES = "TabResources";

		private const string TAB_TREASURE = "TabTreasure";

		protected const string TAB_TITLE = "DialogStoreTitle";

		private const string CATEGORY_TITLE = "DialogStoreCategoryTitle";

		private const string TURRET_LABEL = "LabelTurretCount";

		private const string TURRET_LABEL_GROUP = "TurretCount";

		private const string TEXTURE_HOLDER_ARMY = "TextureArmyIcon";

		private const string TEXTURE_HOLDER_DECORATIONS = "TextureDecorationsIcon";

		private const string TEXTURE_HOLDER_DEFENSES = "TextureDefensesIcon";

		private const string TEXTURE_HOLDER_RESOURCES = "TextureResourcesIcon";

		private const string TEXTURE_HOLDER_SHIELDS = "TextureShieldIcon";

		private const string TEXTURE_HOLDER_TREASURE = "TextureTreasureIcon";

		private const string TEXTURE_DEFENSES = "StoreIconDefense_{0}";

		private const string TEXTURE_RESOURCES = "StoreIconResources_{0}";

		private const string TEXTURE_TURRETS = "StoreIconTurrets_{0}";

		private const string TEXTURE_ARMY = "StoreTabIconArmy_{0}";

		private const string TEXTURE_SHIELDS = "StoreIconShields";

		private const string TEXTURE_TREASURE = "StoreIconTreasure";

		private const string CRYSTAL_SALE_TITLE_CONTAINER = "CrystalBonusTitleContainer";

		private const string CRYSTAL_SALE_TITLE = "CrystalBonusLabelTitle";

		private const string CRYSTAL_SALE_TIMER = "CyrstalBonusLabelExpire";

		protected const string CRYSTAL_SALE_INFO = "CrystalBonus";

		private const string CRYSTAL_SALE_ITEM_PERCENT = "CrystalBonusLabel";

		private const string CRYSTAL_SALE_ITEM_TOTAL = "CrystalBonusLabelAmount";

		private const string CRYSTAL_SALE_ITEM_BONUS = "CrystalBonusLabelBonusAmount";

		private const string CRYSTAL_SALE_TIMER_TEXT = "crystal_bonus_ends_in";

		private const string CRYSTAL_SALE_PERCENT_TEXT = "crystal_percent_bonus";

		private const string CRYSTAL_SALE_BONUS_TEXT = "crystal_amount_bonus";

		private const string CURRENCY_NAME_TEXT = "CURRENCY_VALUE_NAME";

		private const string PURCHASE_CRATE = "PURCHASE_CRATE";

		private const string PURCHASE_PROTECTION = "PURCHASE_PROTECTION";

		private const string PURCHASE_SOFT_CURRENCY = "PURCHASE_SOFT_CURRENCY";

		private const string ALL_CRATES_ALREADY_PURCHASED = "ALL_CRATES_ALREADY_PURCHASED";

		private const string LIMITED_EDITION_CATEGORY_BANNER = "LIMITED_EDITION_CATEGORY_BANNER";

		private const string CRATE_STORE_LEI_EXPIRATION_TIMER = "CRATE_STORE_LEI_EXPIRATION_TIMER";

		private const string ITEM_ICON_ASSET_PREFIX = "treasureicon_neu-mod_";

		private const string ITEM_ICON_PROTECTION = "protection";

		private const string ITEM_PACK_CRYSTALS = "PACK_CRYSTALS{0}";

		private const string ITEM_PACK_CURRENCY = "PACK_CURRENCY{0}";

		private const string ITEM_PACK_PROTECTION = "PACK_PROTECTION{0}";

		protected const string ITEM_JEWEL = "Items";

		private const string ITEM_BG = "TemplateBg";

		private const string ITEM_LEI_BG = "TemplateBgSpecial";

		private const string SUFFIX_EMPIRE = "emp";

		private const string SUFFIX_REBEL = "rbl";

		private const string SPECIAL_OFFERS_CRYSTAL_ASSET = "specialoffer";

		public const string IAP_TITLE_PREFIX = "iap_title_";

		public const string IAP_DESC_PREFIX = "iap_desc_";

		protected const string BADGE_GROUP = "PackageBadge";

		private const string BADGE_TOP_SPRITE = "SpritePackageBg";

		private const string BADGE_TOP_LABEL = "LabelPackageTop";

		private const string BADGE_BOTTOM_LABEL = "LabelPackageBottom";

		protected const string EVENT_STORE_TITLE = "EventStoreTitle";

		private const string LEI_TREASURE_HEADER_LABEL = "LabelHeaderTreasureSpecial";

		private const string LEI_TREASURE_BG = "BgTabTreasureSpecial";

		private const string TREASURE_HEADER_LABEL = "LabelTreasure";

		private const string TREASURE_BG = "BgTabTreasure";

		private static readonly int[] SOFT_CURRENCY_PERCENTS = new int[]
		{
			10,
			50,
			100
		};

		private UXLabel titleLabel;

		private UXLabel categoryLabel;

		protected UXGrid itemGrid;

		private UXElement itemGridParent;

		private UXElement tabsParent;

		private UXElement primaryPage;

		private UXElement secondaryPage;

		private UXElement promoGroup;

		private UXLabel turretLabel;

		private UXElement turretLabelGroup;

		protected UXButton backButton;

		private StoreTab curTab;

		private bool changingInventory;

		private bool showingPromo;

		private bool movedDown;

		private bool turretSwappingUnlocked;

		private bool enableTimer;

		private string curItem;

		private Dictionary<StoreTab, JewelControl> tabJewels;

		protected bool requiresRefresh;

		private SaleTypeVO visibleSale;

		private bool gridInitialized;

		private bool gridScrollable;

		private uint delayedInitTimerId;

		private bool resetCurrentTabOnVisible;

		private string crateToReOpenInFlyout;

		public override bool ShowCurrencyTray
		{
			get
			{
				return true;
			}
		}

		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
				if (value && this.resetCurrentTabOnVisible)
				{
					this.ResetCurrentTab();
				}
			}
		}

		public StoreScreen()
		{
			this.gridScrollable = true;
			base..ctor("gui_store");
			this.delayedInitTimerId = 0u;
			this.changingInventory = false;
			this.enableTimer = false;
			this.resetCurrentTabOnVisible = false;
			this.showingPromo = true;
			this.movedDown = false;
			this.turretSwappingUnlocked = Service.Get<BuildingLookupController>().IsTurretSwappingUnlocked();
			this.curItem = null;
			this.SetTab(StoreTab.NotInStore);
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.PlayerFactionChanged);
			eventManager.RegisterObserver(this, EventId.InventoryResourceUpdated);
			eventManager.RegisterObserver(this, EventId.ButtonHighlightActivated);
			eventManager.RegisterObserver(this, EventId.EquipmentUnlockCelebrationPlayed);
			this.tabJewels = new Dictionary<StoreTab, JewelControl>();
			this.requiresRefresh = false;
			this.visibleSale = null;
			base.OnTransitionInComplete = new OnTransInComplete(this.onScreenTransitionInComplete);
		}

		private void onScreenTransitionInComplete()
		{
			Service.Get<EventManager>().SendEvent(EventId.ScreenTransitionInComplete, this);
		}

		protected override void OnScreenLoaded()
		{
			base.Root.name = "gui_store_screen";
			this.InitLabels();
			this.InitButtons();
			base.GetElement<UXElement>("EventStoreTitle").Visible = false;
			Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
			this.delayedInitTimerId = Service.Get<ViewTimerManager>().CreateViewTimer(0.01f, false, new TimerDelegate(this.InitializeDelayed), null);
		}

		private void InitializeDelayed(uint id, object cookie)
		{
			this.delayedInitTimerId = 0u;
			this.InitGrids();
			this.SetTab(this.curTab);
			this.ShowPromos(this.showingPromo);
			this.ScrollToItem(this.curItem);
		}

		public override void OnDestroyElement()
		{
			if (this.delayedInitTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.delayedInitTimerId);
				this.delayedInitTimerId = 0u;
			}
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
			this.visibleSale = null;
			this.EnableScrollListMovement(true);
			if (this.itemGrid != null)
			{
				this.itemGrid.Clear();
				this.itemGrid = null;
			}
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.PlayerFactionChanged);
			eventManager.UnregisterObserver(this, EventId.InventoryResourceUpdated);
			eventManager.UnregisterObserver(this, EventId.ButtonHighlightActivated);
			eventManager.UnregisterObserver(this, EventId.EquipmentUnlockCelebrationPlayed);
			eventManager.UnregisterObserver(this, EventId.InventoryCrateCollectionClosed);
			this.RemoveProtectionCooldownTimer();
			base.OnDestroyElement();
		}

		private void AddProtectionCooldownTimer()
		{
			if (this.enableTimer)
			{
				return;
			}
			this.enableTimer = true;
		}

		private void RemoveProtectionCooldownTimer()
		{
			this.enableTimer = false;
		}

		private void InitLabels()
		{
			this.titleLabel = base.GetElement<UXLabel>("DialogStoreTitle");
			this.categoryLabel = base.GetElement<UXLabel>("DialogStoreCategoryTitle");
			this.turretLabel = base.GetElement<UXLabel>("LabelTurretCount");
			this.turretLabelGroup = base.GetElement<UXElement>("TurretCount");
			this.turretLabel.Text = "";
			this.turretLabelGroup.Visible = false;
		}

		private void RefreshProtectionCooldownTimer()
		{
			int[] array;
			int[] array2;
			GameUtils.GetProtectionPacks(out array, out array2);
			bool flag = true;
			for (int i = 1; i <= array.Length; i++)
			{
				string text = string.Format("{0}_{1}", new object[]
				{
					"protection",
					i
				});
				UXButton subElement = this.itemGrid.GetSubElement<UXButton>(text, "ButtonItemCard");
				StoreItemTag storeItemTag = subElement.Tag as StoreItemTag;
				if (!storeItemTag.CanPurchase)
				{
					UXLabel subElement2 = this.itemGrid.GetSubElement<UXLabel>(text, "LabelItemRequirement");
					int protectionCooldownRemainingInSeconds = this.GetProtectionCooldownRemainingInSeconds(i);
					if (protectionCooldownRemainingInSeconds > 0)
					{
						flag = false;
						subElement2.Text = this.lang.Get("PROTECTION_COOLDOWN_TIMER", new object[0]) + GameUtils.GetTimeLabelFromSeconds(protectionCooldownRemainingInSeconds);
					}
					else
					{
						storeItemTag.CanPurchase = (storeItemTag.ReqMet = true);
						subElement2.Text = "";
						UXSprite subElement3 = this.itemGrid.GetSubElement<UXSprite>(text, "SpriteDim");
						subElement3.Visible = false;
						UXUtils.SetupCostElements(this, "Cost", text, 0, 0, 0, storeItemTag.Price, false, null);
					}
				}
			}
			if (flag)
			{
				this.RemoveProtectionCooldownTimer();
			}
		}

		private void InitGrids()
		{
			this.itemGridParent = base.GetElement<UXElement>("StoreItems2row");
			this.itemGrid = base.GetElement<UXGrid>("StoreGrid2row");
			this.itemGrid.CellHeight = base.GetElement<UXElement>("ItemCellHeightGuide").Height;
			base.GetElement<UXElement>("StoreItemTemplate").Height = base.GetElement<UXElement>("ItemHeightGuide").Height;
			UXElement element = base.GetElement<UXElement>("ButtonItemCard");
			UIDragScrollView component = element.Root.GetComponent<UIDragScrollView>();
			component.scrollView = this.itemGrid.Root.GetComponent<UIScrollView>();
			base.GetElement<UXLabel>("LabelItemRequirement").Text = string.Empty;
			this.itemGrid.SetTemplateItem("StoreItemTemplate");
			this.itemGrid.IsScrollable = this.gridScrollable;
			this.gridInitialized = true;
		}

		protected override void InitButtons()
		{
			base.InitButtons();
			this.tabsParent = base.GetElement<UXElement>("StoreTabs");
			this.primaryPage = base.GetElement<UXElement>("TabPage");
			this.secondaryPage = base.GetElement<UXElement>("CategoryPage");
			this.promoGroup = base.GetElement<UXElement>("PromoContainer");
			this.backButton = base.GetElement<UXButton>("BtnBack");
			this.BackButtons.Add(this.backButton);
			this.SetupTab(StoreTab.NotInStore, this.backButton);
			this.SetupTab(StoreTab.Treasure, "TabTreasure", "TextureTreasureIcon", "StoreIconTreasure");
			this.SetupTab(StoreTab.Protection, "TabShield", "TextureShieldIcon", "StoreIconShields");
			this.SetupTabForFaction(StoreTab.Resources, "TabResources", "TextureResourcesIcon", "StoreIconResources_{0}");
			this.SetupTabForFaction(StoreTab.Army, "TabArmy", "TextureArmyIcon", "StoreTabIconArmy_{0}");
			this.SetupTabForFaction(StoreTab.Defenses, "TabDefenses", "TextureDefensesIcon", "StoreIconDefense_{0}");
			this.SetupTabForFaction(StoreTab.Decorations, "TabDecorations", "TextureDecorationsIcon", "StoreIconTurrets_{0}");
			this.SetupJewel(StoreTab.Resources);
			this.SetupJewel(StoreTab.Army);
			this.SetupJewel(StoreTab.Defenses);
			this.SetupJewel(StoreTab.Decorations);
			this.SetupLimitedEditionTab(StoreTab.Treasure, "LabelHeaderTreasureSpecial", "BgTabTreasureSpecial", "BgTabTreasure");
		}

		public static int CountUnlockedUnbuiltBuildings()
		{
			return StoreScreen.AddOrCountBuildingItems(null, StoreTab.Resources, null) + StoreScreen.AddOrCountBuildingItems(null, StoreTab.Army, null) + StoreScreen.AddOrCountBuildingItems(null, StoreTab.Defenses, null) + StoreScreen.AddOrCountBuildingItems(null, StoreTab.Decorations, null);
		}

		private void SetupLimitedEditionTab(StoreTab tab, string leHeaderName, string leBgName, string bgName)
		{
			UXLabel element = base.GetElement<UXLabel>(leHeaderName);
			UXElement element2 = base.GetElement<UXElement>(leBgName);
			element.Visible = false;
			element2.Visible = false;
			LimitedEditionItemController limitedEditionItemController = Service.Get<LimitedEditionItemController>();
			int i = 0;
			int count = limitedEditionItemController.ValidLEIs.Count;
			while (i < count)
			{
				if (limitedEditionItemController.ValidLEIs[i].StoreTab == tab)
				{
					element.Text = this.lang.Get("LIMITED_EDITION_CATEGORY_BANNER", new object[0]);
					element.Visible = true;
					element2.Visible = true;
					base.GetElement<UXElement>(bgName).Visible = false;
					return;
				}
				i++;
			}
		}

		private void SetupTab(StoreTab tab, string tabName, string textureHolderName, string textureAssetName)
		{
			base.GetElement<UXTexture>(textureHolderName).LoadTexture(textureAssetName);
			this.SetupTab(tab, base.GetElement<UXButton>(tabName));
		}

		private void SetupTabForFaction(StoreTab tab, string tabName, string textureHolderName, string textureAssetNameFormat)
		{
			string assetName = null;
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			if (faction != FactionType.Empire)
			{
				if (faction == FactionType.Rebel)
				{
					assetName = string.Format(textureAssetNameFormat, new object[]
					{
						"rbl"
					});
				}
			}
			else
			{
				assetName = string.Format(textureAssetNameFormat, new object[]
				{
					"emp"
				});
			}
			base.GetElement<UXTexture>(textureHolderName).LoadTexture(assetName);
			this.SetupTab(tab, base.GetElement<UXButton>(tabName));
		}

		private void SetupTab(StoreTab tab, UXButton tabButton)
		{
			tabButton.OnClicked = new UXButtonClickedDelegate(this.OnTabButtonClicked);
			tabButton.Tag = tab;
		}

		private void SetupJewel(StoreTab tab)
		{
			JewelControl jewelControl;
			if (this.tabJewels.ContainsKey(tab))
			{
				jewelControl = this.tabJewels[tab];
			}
			else
			{
				jewelControl = JewelControl.Create(this, tab.ToString());
				this.tabJewels.Add(tab, jewelControl);
			}
			if (jewelControl != null)
			{
				int value = StoreScreen.AddOrCountBuildingItems(null, tab, null);
				jewelControl.Value = value;
			}
		}

		private void ShowPromos(bool show)
		{
			this.showingPromo = show;
			if (!base.IsLoaded())
			{
				return;
			}
			if (this.showingPromo == this.movedDown)
			{
				this.promoGroup.Visible = this.showingPromo;
				this.movedDown = !this.showingPromo;
				float amount = (float)(show ? 1 : -1) * this.promoGroup.Height * 0.5f;
				this.MoveUp(this.itemGridParent, amount);
				this.MoveUp(this.tabsParent, amount);
			}
		}

		private void MoveUp(UXElement element, float amount)
		{
			Vector3 localPosition = element.LocalPosition;
			localPosition.y += amount;
			element.LocalPosition = localPosition;
		}

		public void SetTab(StoreTab tab)
		{
			this.curTab = tab;
			if (!base.IsLoaded() || !this.gridInitialized)
			{
				return;
			}
			bool flag = tab > StoreTab.NotInStore;
			this.primaryPage.Visible = !flag;
			this.secondaryPage.Visible = flag;
			this.titleLabel.Visible = !flag;
			this.categoryLabel.Visible = flag;
			if (flag)
			{
				this.SetupCurTabElements();
				if (this.tabJewels.ContainsKey(tab))
				{
					int value = StoreScreen.AddOrCountBuildingItems(null, tab, null);
					this.tabJewels[tab].Value = value;
					return;
				}
			}
			else
			{
				this.titleLabel.Text = this.lang.Get("s_Store", new object[0]);
				this.itemGrid.Clear();
			}
		}

		public void ResetCurrentTab()
		{
			if (this.curTab == StoreTab.EventPrizes || this.curTab == StoreTab.Protection || this.curTab == StoreTab.Treasure)
			{
				if (this.Visible)
				{
					this.SetupCurTabElements();
					return;
				}
				this.resetCurrentTabOnVisible = true;
			}
		}

		public static int AddOrCountBuildingItems(List<UXElement> list, StoreTab tab, StoreScreen.AddBuildingItemDelegate onAddBuildingItem)
		{
			int num = 0;
			bool flag = list == null;
			bool flag2 = false;
			int num2 = 0;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			FactionType faction = currentPlayer.Faction;
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			IDataController dataController = Service.Get<IDataController>();
			UnlockController unlockController = Service.Get<UnlockController>();
			foreach (BuildingTypeVO current in dataController.GetAll<BuildingTypeVO>())
			{
				if (current.Faction == faction && current.StoreTab == tab)
				{
					BuildingTypeVO reqBuilding = null;
					bool flag3 = unlockController.IsUnlocked(current, 0, out reqBuilding);
					if (flag3 || !current.HideIfLocked)
					{
						if (flag && tab == StoreTab.Decorations && current.Type == BuildingType.Turret)
						{
							if (num2 == 0)
							{
								num2 = buildingLookupController.GetBuildingMaxPurchaseQuantity(current, 0);
								flag2 = true;
							}
						}
						else
						{
							int buildingPurchasedQuantity = buildingLookupController.GetBuildingPurchasedQuantity(current);
							int buildingMaxPurchaseQuantity = buildingLookupController.GetBuildingMaxPurchaseQuantity(current, 0);
							if (buildingMaxPurchaseQuantity > 0 && buildingPurchasedQuantity < buildingMaxPurchaseQuantity)
							{
								num += buildingMaxPurchaseQuantity - buildingPurchasedQuantity;
							}
							if (!flag)
							{
								if (tab == StoreTab.Decorations && current.Type == BuildingType.Turret && flag3 && StoreScreen.IsTurretMax(current))
								{
									Entity currentHQ = buildingLookupController.GetCurrentHQ();
									BuildingTypeVO buildingType = currentHQ.Get<BuildingComponent>().BuildingType;
									int lvl = Service.Get<BuildingUpgradeCatalog>().GetMaxLevel(buildingType.UpgradeGroup).Lvl;
									if (buildingType.Lvl < lvl)
									{
										onAddBuildingItem(list, current, reqBuilding, true, 1, 1);
									}
									else
									{
										onAddBuildingItem(list, current, reqBuilding, true, 0, 0);
									}
								}
								else
								{
									onAddBuildingItem(list, current, reqBuilding, flag3, buildingPurchasedQuantity, buildingMaxPurchaseQuantity);
								}
							}
						}
					}
				}
			}
			if (flag2)
			{
				int num3 = buildingLookupController.TurretBuildingNodeList.CalculateCount();
				if (num2 > 0 && num3 < num2)
				{
					num += num2 - num3;
				}
			}
			if (list != null)
			{
				list.Sort(new Comparison<UXElement>(StoreScreen.CompareBuildingItem));
			}
			return num;
		}

		private void AddBuildingItem(List<UXElement> list, BuildingTypeVO buildingInfo, BuildingTypeVO reqBuilding, bool reqMet, int curQuantity, int maxQuantity)
		{
			StoreItemTag storeItemTag = new StoreItemTag();
			storeItemTag.Uid = buildingInfo.Uid;
			storeItemTag.BuildingInfo = buildingInfo;
			storeItemTag.ReqMet = reqMet;
			storeItemTag.ReqBuilding = reqBuilding;
			storeItemTag.CurQuantity = curQuantity;
			storeItemTag.MaxQuantity = maxQuantity;
			string uid = buildingInfo.Uid;
			UXElement uXElement = this.itemGrid.CloneTemplateItem(uid);
			uXElement.Tag = storeItemTag;
			storeItemTag.MainElement = this.itemGrid.GetSubElement<UXElement>(uid, "ItemInfo");
			UXLabel subElement = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelName");
			subElement.Text = LangUtils.GetBuildingDisplayName(buildingInfo);
			storeItemTag.InfoLabel = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemInfo");
			storeItemTag.InfoLabel.Text = LangUtils.GetBuildingDescription(buildingInfo);
			storeItemTag.InfoLabel.Visible = false;
			UXButton subElement2 = this.itemGrid.GetSubElement<UXButton>(uid, "BtnItemInfo");
			subElement2.OnClicked = new UXButtonClickedDelegate(this.OnInfoButtonClicked);
			subElement2.Tag = storeItemTag;
			UXSprite subElement3 = this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteItemImage");
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateBuildingConfig(buildingInfo, subElement3);
			projectorConfig.AnimPreference = AnimationPreference.AnimationPreferred;
			ProjectorUtils.GenerateProjector(projectorConfig);
			string name = UXUtils.FormatAppendedName("Items", uid);
			JewelControl jewelControl = JewelControl.Create(this, name);
			if (jewelControl != null)
			{
				int value = 0;
				if (buildingInfo.Type != BuildingType.Turret && storeItemTag.ReqMet && storeItemTag.MaxQuantity > 0)
				{
					value = storeItemTag.MaxQuantity - storeItemTag.CurQuantity;
				}
				jewelControl.Value = value;
			}
			int credits = buildingInfo.Credits;
			int materials = buildingInfo.Materials;
			int contraband = buildingInfo.Contraband;
			UXUtils.SetupCostElements(this, "Cost", uid, credits, materials, contraband, 0, false, null);
			UXLabel subElement4 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemRequirement");
			if (storeItemTag.ReqMet && storeItemTag.CurQuantity < storeItemTag.MaxQuantity)
			{
				storeItemTag.CanPurchase = true;
				UXLabel subElement5 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelBuildTime");
				subElement5.Text = GameUtils.GetTimeLabelFromSeconds(buildingInfo.Time);
			}
			else
			{
				storeItemTag.CanPurchase = false;
				if (storeItemTag.ReqMet && storeItemTag.CurQuantity >= storeItemTag.MaxQuantity)
				{
					if (storeItemTag.CurQuantity == 0 && storeItemTag.MaxQuantity == 0)
					{
						subElement4.Text = this.lang.Get("BUILDING_MAX", new object[0]);
					}
					else
					{
						int lvl = Service.Get<BuildingUpgradeCatalog>().GetMaxLevel(reqBuilding.UpgradeGroup).Lvl;
						int buildingMaxPurchaseQuantity = Service.Get<BuildingLookupController>().GetBuildingMaxPurchaseQuantity(storeItemTag.BuildingInfo, lvl);
						if (buildingMaxPurchaseQuantity > maxQuantity)
						{
							subElement4.Text = this.lang.Get("BUILDING_UPGRADE_REQUIREMENT", new object[]
							{
								LangUtils.GetBuildingDisplayName(storeItemTag.ReqBuilding)
							});
						}
						else
						{
							subElement4.Text = this.lang.Get("BUILDING_MAX", new object[0]);
						}
					}
				}
				else
				{
					subElement4.Text = this.lang.Get("BUILDING_REQUIREMENT", new object[]
					{
						storeItemTag.ReqBuilding.Lvl,
						LangUtils.GetBuildingDisplayName(storeItemTag.ReqBuilding)
					});
				}
			}
			subElement4.Visible = !storeItemTag.CanPurchase;
			UXSprite subElement6 = this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteDim");
			subElement6.Visible = !storeItemTag.CanPurchase;
			UXElement subElement7 = this.itemGrid.GetSubElement<UXElement>(uid, "CountAndBuildTime");
			subElement7.Visible = storeItemTag.CanPurchase;
			if (storeItemTag.CanPurchase)
			{
				UXLabel subElement8 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemCount");
				subElement8.Text = ((this.turretSwappingUnlocked && buildingInfo.Type == BuildingType.Turret) ? this.lang.Get("TROOP_MULTIPLIER", new object[]
				{
					storeItemTag.CurQuantity
				}) : this.lang.Get("FRACTION", new object[]
				{
					storeItemTag.CurQuantity,
					storeItemTag.MaxQuantity
				}));
			}
			UXLabel subElement9 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelCurrencyAmount");
			subElement9.Visible = false;
			UXButton subElement10 = this.itemGrid.GetSubElement<UXButton>(uid, "ButtonItemCard");
			subElement10.OnClicked = new UXButtonClickedDelegate(this.OnBuildingItemButtonClicked);
			subElement10.Tag = storeItemTag;
			storeItemTag.MainButton = subElement10;
			this.itemGrid.GetSubElement<UXElement>(uid, "PackageBadge").Visible = false;
			this.itemGrid.GetSubElement<UXElement>(uid, "CrystalBonus").Visible = false;
			this.HideLEIElements(uid);
			list.Add(uXElement);
		}

		private void AddSupplyCrates(List<UXElement> list)
		{
			List<CrateVO> list2 = new List<CrateVO>();
			IDataController dataController = Service.Get<IDataController>();
			foreach (CrateVO current in dataController.GetAll<CrateVO>())
			{
				if (CrateUtils.IsVisibleInStore(current))
				{
					list2.Add(current);
				}
			}
			list2.Sort(new Comparison<CrateVO>(this.CompareCrates));
			int i = 0;
			int count = list2.Count;
			while (i < count)
			{
				CrateVO crateVO = list2[i];
				StoreItemTag storeItemTag = new StoreItemTag();
				storeItemTag.Amount = 1;
				storeItemTag.Price = crateVO.Crystals;
				storeItemTag.Currency = CurrencyType.Crystals;
				storeItemTag.ReqMet = true;
				storeItemTag.CanPurchase = true;
				string uid = crateVO.Uid;
				storeItemTag.Uid = uid;
				UXElement uXElement = this.itemGrid.CloneTemplateItem(uid);
				uXElement.Tag = storeItemTag;
				storeItemTag.MainElement = this.itemGrid.GetSubElement<UXElement>(uid, "ItemInfo");
				UXLabel subElement = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelName");
				subElement.Text = LangUtils.GetCrateDisplayName(crateVO);
				this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemInfo").Visible = false;
				this.itemGrid.GetSubElement<UXButton>(uid, "BtnItemInfo").Visible = false;
				UXSprite subElement2 = this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteItemImage");
				subElement2.SpriteName = "bkgClear";
				RewardUtils.SetCrateIcon(subElement2, crateVO, AnimState.Closed);
				string name = UXUtils.FormatAppendedName("Items", uid);
				JewelControl.Create(this, name);
				UXLabel subElement3 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemRequirement");
				subElement3.Visible = false;
				UXSprite subElement4 = this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteDim");
				subElement4.Visible = !storeItemTag.ReqMet;
				int crystals = storeItemTag.ReqMet ? storeItemTag.Price : 0;
				UXUtils.SetupCostElements(this, "Cost", uid, 0, 0, 0, crystals, false, null);
				this.itemGrid.GetSubElement<UXLabel>(uid, "LabelBuildTime").Visible = false;
				this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteItemTimeIcon").Visible = false;
				this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemCount").Visible = false;
				this.itemGrid.GetSubElement<UXLabel>(uid, "LabelCurrencyAmount").Visible = false;
				UXButton subElement5 = this.itemGrid.GetSubElement<UXButton>(uid, "ButtonItemCard");
				subElement5.OnClicked = new UXButtonClickedDelegate(this.OnSupplyCrateItemButtonClicked);
				subElement5.Tag = storeItemTag;
				this.itemGrid.GetSubElement<UXElement>(uid, "PackageBadge").Visible = false;
				this.itemGrid.GetSubElement<UXElement>(uid, "CrystalBonus").Visible = false;
				this.HideLEIElements(uid);
				list.Add(uXElement);
				i++;
			}
		}

		private void HideLEIElements(string itemUid)
		{
			this.itemGrid.GetSubElement<UXElement>(itemUid, "TemplateBgSpecial").Visible = false;
			this.itemGrid.GetSubElement<UXLabel>(itemUid, "LabelNameSpecial").Visible = false;
			this.itemGrid.GetSubElement<UXLabel>(itemUid, "LabelTimerSpecial").Visible = false;
		}

		private void AddLEItems(List<UXElement> list, StoreTab currentTab, List<LimitedEditionItemVO> leItems)
		{
			int i = 0;
			int count = leItems.Count;
			while (i < count)
			{
				LimitedEditionItemVO limitedEditionItemVO = leItems[i];
				if (limitedEditionItemVO.StoreTab == currentTab)
				{
					StoreItemTag storeItemTag = new StoreItemTag();
					storeItemTag.Amount = 1;
					storeItemTag.ReqMet = true;
					storeItemTag.CanPurchase = true;
					if (limitedEditionItemVO.Credits > 0)
					{
						storeItemTag.Currency = CurrencyType.Credits;
						storeItemTag.Price = limitedEditionItemVO.Credits;
					}
					else if (limitedEditionItemVO.Materials > 0)
					{
						storeItemTag.Currency = CurrencyType.Materials;
						storeItemTag.Price = limitedEditionItemVO.Materials;
					}
					else if (limitedEditionItemVO.Contraband > 0)
					{
						storeItemTag.Currency = CurrencyType.Contraband;
						storeItemTag.Price = limitedEditionItemVO.Contraband;
					}
					else if (limitedEditionItemVO.Crystals > 0)
					{
						storeItemTag.Currency = CurrencyType.Crystals;
						storeItemTag.Price = limitedEditionItemVO.Crystals;
					}
					string uid = limitedEditionItemVO.Uid;
					storeItemTag.Uid = uid;
					UXElement uXElement = this.itemGrid.CloneTemplateItem(uid);
					uXElement.Tag = storeItemTag;
					storeItemTag.MainElement = this.itemGrid.GetSubElement<UXElement>(uid, "ItemInfo");
					UXLabel subElement = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelNameSpecial");
					subElement.Text = LangUtils.GetLEIDisplayName(limitedEditionItemVO.Uid);
					this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemInfo").Visible = false;
					this.itemGrid.GetSubElement<UXButton>(uid, "BtnItemInfo").Visible = false;
					UXSprite subElement2 = this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteItemImage");
					subElement2.SpriteName = "bkgClear";
					ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(limitedEditionItemVO, subElement2);
					projectorConfig.AnimPreference = AnimationPreference.NoAnimation;
					projectorConfig.AnimState = AnimState.Closed;
					ProjectorUtils.GenerateProjector(projectorConfig);
					string name = UXUtils.FormatAppendedName("Items", uid);
					JewelControl.Create(this, name);
					UXLabel subElement3 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemRequirement");
					subElement3.Visible = false;
					UXSprite subElement4 = this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteDim");
					subElement4.Visible = !storeItemTag.ReqMet;
					UXUtils.SetupCostElements(this, "Cost", uid, limitedEditionItemVO.Credits, limitedEditionItemVO.Materials, limitedEditionItemVO.Contraband, limitedEditionItemVO.Crystals, false, null);
					this.itemGrid.GetSubElement<UXLabel>(uid, "LabelBuildTime").Visible = false;
					this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteItemTimeIcon").Visible = false;
					this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemCount").Visible = false;
					this.itemGrid.GetSubElement<UXLabel>(uid, "LabelCurrencyAmount").Visible = false;
					UXButton subElement5 = this.itemGrid.GetSubElement<UXButton>(uid, "ButtonItemCard");
					subElement5.OnClicked = new UXButtonClickedDelegate(this.OnLEItemButtonClicked);
					subElement5.Tag = storeItemTag;
					this.itemGrid.GetSubElement<UXElement>(uid, "PackageBadge").Visible = false;
					this.itemGrid.GetSubElement<UXElement>(uid, "CrystalBonus").Visible = false;
					this.itemGrid.GetSubElement<UXElement>(uid, "TemplateBg").Visible = false;
					this.itemGrid.GetSubElement<UXLabel>(uid, "LabelName").Visible = false;
					UXLabel subElement6 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelTimerSpecial");
					subElement6.Visible = true;
					subElement6.TextColor = UXUtils.COLOR_CRATE_EXPIRE_LABEL_NORMAL;
					CountdownControl countdownControl = new CountdownControl(subElement6, this.lang.Get("CRATE_STORE_LEI_EXPIRATION_TIMER", new object[0]), limitedEditionItemVO.EndTime);
					countdownControl.SetThreshold(GameConstants.CRATE_INVENTORY_LEI_EXPIRATION_TIMER_WARNING, UXUtils.COLOR_CRATE_EXPIRE_LABEL_WARNING);
					list.Add(uXElement);
				}
				i++;
			}
		}

		private int CompareCrates(CrateVO a, CrateVO b)
		{
			return a.Crystals - b.Crystals;
		}

		private void AddCurrencyOrProtectionItems(List<UXElement> list, bool forProtection, CurrencyType currencyType)
		{
			string[] array = null;
			bool[] array2 = null;
			int[] array3;
			int[] array4;
			if (forProtection)
			{
				GameUtils.GetProtectionPacks(out array3, out array4);
			}
			else
			{
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				int num = StoreScreen.SOFT_CURRENCY_PERCENTS.Length;
				array3 = new int[num];
				array4 = new int[num];
				array = new string[num];
				array2 = new bool[num];
				int num2 = 0;
				int num3 = 0;
				switch (currencyType)
				{
				case CurrencyType.Credits:
					num2 = currentPlayer.CurrentCreditsAmount;
					num3 = currentPlayer.MaxCreditsAmount;
					break;
				case CurrencyType.Materials:
					num2 = currentPlayer.CurrentMaterialsAmount;
					num3 = currentPlayer.MaxMaterialsAmount;
					break;
				case CurrencyType.Contraband:
					num2 = currentPlayer.CurrentContrabandAmount;
					num3 = currentPlayer.MaxContrabandAmount;
					break;
				}
				for (int i = 0; i < num; i++)
				{
					int num4 = StoreScreen.SOFT_CURRENCY_PERCENTS[i];
					int num5 = (num4 == 100) ? (num3 - num2) : (num3 * num4 / 100);
					array3[i] = num5;
					int num6 = 0;
					switch (currencyType)
					{
					case CurrencyType.Credits:
						array[i] = "Credits" + num4;
						num6 = GameUtils.CreditsCrystalCost(num5);
						break;
					case CurrencyType.Materials:
						array[i] = "Materials" + num4;
						num6 = GameUtils.MaterialsCrystalCost(num5);
						break;
					case CurrencyType.Contraband:
						array[i] = "Contraband" + num4;
						num6 = GameUtils.ContrabandCrystalCost(num5);
						break;
					}
					array4[i] = num6;
					array2[i] = (num5 > 0 && num3 > 0 && num2 + num5 <= num3);
				}
			}
			int j = 0;
			int num7 = array3.Length;
			while (j < num7)
			{
				int num8 = j + 1;
				StoreItemTag storeItemTag = new StoreItemTag();
				storeItemTag.Amount = (forProtection ? num8 : array3[j]);
				storeItemTag.Price = array4[j];
				storeItemTag.Currency = currencyType;
				storeItemTag.ReqMet = (forProtection || array2[j]);
				if (forProtection)
				{
					storeItemTag.CanPurchase = (storeItemTag.ReqMet = this.IsProtectionPackAvailable(num8));
				}
				if (!forProtection && array != null)
				{
					storeItemTag.Uid = array[j];
				}
				string text = string.Format("{0}_{1}", new object[]
				{
					forProtection ? "protection" : currencyType.ToString(),
					num8
				});
				UXElement uXElement = this.itemGrid.CloneTemplateItem(text);
				uXElement.Tag = storeItemTag;
				storeItemTag.MainElement = this.itemGrid.GetSubElement<UXElement>(text, "ItemInfo");
				UXLabel subElement = this.itemGrid.GetSubElement<UXLabel>(text, "LabelName");
				string text2;
				if (forProtection)
				{
					text2 = "PACK_PROTECTION{0}";
				}
				else
				{
					text2 = "PACK_CURRENCY{0}";
				}
				subElement.Text = this.lang.Get(string.Format(text2, new object[]
				{
					num8
				}), new object[]
				{
					this.lang.Get(currencyType.ToString().ToUpper(), new object[0])
				});
				storeItemTag.InfoLabel = this.itemGrid.GetSubElement<UXLabel>(text, "LabelItemInfo");
				storeItemTag.InfoLabel.Visible = false;
				UXButton subElement2 = this.itemGrid.GetSubElement<UXButton>(text, "BtnItemInfo");
				subElement2.Visible = false;
				UXSprite subElement3 = this.itemGrid.GetSubElement<UXSprite>(text, "SpriteItemImage");
				string text3 = forProtection ? "protection" : currencyType.ToString();
				UXUtils.SetupGeometryForIcon(subElement3, text3, num8);
				storeItemTag.IconName = UXUtils.GetCurrencyItemAssetName(text3, num8);
				string name = UXUtils.FormatAppendedName("Items", text);
				JewelControl.Create(this, name);
				UXLabel subElement4 = this.itemGrid.GetSubElement<UXLabel>(text, "LabelItemRequirement");
				if (!storeItemTag.ReqMet && !forProtection)
				{
					subElement4.Text = this.lang.Get("STORE_TREASURE_LIMIT", new object[0]);
				}
				else if (!storeItemTag.ReqMet & forProtection)
				{
					this.AddProtectionCooldownTimer();
				}
				else
				{
					subElement4.Visible = false;
				}
				UXSprite subElement5 = this.itemGrid.GetSubElement<UXSprite>(text, "SpriteDim");
				subElement5.Visible = !storeItemTag.ReqMet;
				if (forProtection && !storeItemTag.CanPurchase)
				{
					subElement5.Visible = true;
				}
				int crystals = storeItemTag.ReqMet ? storeItemTag.Price : 0;
				UXUtils.SetupCostElements(this, "Cost", text, 0, 0, 0, crystals, false, null);
				UXLabel subElement6 = this.itemGrid.GetSubElement<UXLabel>(text, "LabelBuildTime");
				subElement6.Visible = false;
				UXSprite subElement7 = this.itemGrid.GetSubElement<UXSprite>(text, "SpriteItemTimeIcon");
				subElement7.Visible = false;
				UXLabel subElement8 = this.itemGrid.GetSubElement<UXLabel>(text, "LabelItemCount");
				subElement8.Visible = false;
				UXLabel subElement9 = this.itemGrid.GetSubElement<UXLabel>(text, "LabelCurrencyAmount");
				if (!forProtection && storeItemTag.Amount > 0)
				{
					subElement9.Text = this.lang.Get("CURRENCY_VALUE_NAME", new object[]
					{
						this.lang.ThousandsSeparated(storeItemTag.Amount),
						this.lang.Get(currencyType.ToString().ToUpper(), new object[0])
					});
				}
				else
				{
					subElement9.Visible = false;
				}
				UXButton subElement10 = this.itemGrid.GetSubElement<UXButton>(text, "ButtonItemCard");
				if (forProtection)
				{
					subElement10.OnClicked = new UXButtonClickedDelegate(this.OnProtectionItemButtonClicked);
				}
				else
				{
					subElement10.OnClicked = new UXButtonClickedDelegate(this.OnSoftCurrencyItemButtonClicked);
				}
				subElement10.Tag = storeItemTag;
				this.itemGrid.GetSubElement<UXElement>(text, "PackageBadge").Visible = false;
				this.itemGrid.GetSubElement<UXElement>(text, "CrystalBonus").Visible = false;
				this.HideLEIElements(text);
				list.Add(uXElement);
				j++;
			}
		}

		protected virtual void AddEventPrizeItems(List<UXElement> list)
		{
		}

		private void AddOffersButton(List<UXElement> list)
		{
		}

		private List<SaleItemTypeVO> GetCurrentItemsOnSale()
		{
			SaleTypeVO currentActiveSale = SaleUtils.GetCurrentActiveSale();
			List<SaleItemTypeVO> list = null;
			if (this.curTab == StoreTab.Treasure && currentActiveSale != null)
			{
				list = SaleUtils.GetSaleItems(currentActiveSale.SaleItems);
				if (list.Count > 0)
				{
					UXLabel element = base.GetElement<UXLabel>("CrystalBonusLabelTitle");
					element.Text = this.lang.Get(currentActiveSale.Title, new object[0]);
					UXLabel element2 = base.GetElement<UXLabel>("CyrstalBonusLabelExpire");
					int secondsRemaining = TimedEventUtils.GetSecondsRemaining(currentActiveSale);
					element2.Text = this.lang.Get("crystal_bonus_ends_in", new object[]
					{
						LangUtils.FormatTime((long)secondsRemaining)
					});
				}
			}
			return list;
		}

		private void SetupSaleItem(string itemUid, SaleItemTypeVO saleItem, StoreItemTag tag)
		{
			if (saleItem != null)
			{
				this.itemGrid.GetSubElement<UXElement>(itemUid, "PackageBadge").Visible = false;
				base.GetElement<UXElement>("CrystalBonusTitleContainer").Visible = true;
				this.visibleSale = SaleUtils.GetCurrentActiveSale();
				this.itemGrid.GetSubElement<UXElement>(itemUid, "CrystalBonus").Visible = true;
				this.itemGrid.GetSubElement<UXLabel>(itemUid, "LabelCurrencyAmount").Visible = false;
				UXLabel subElement = this.itemGrid.GetSubElement<UXLabel>(itemUid, "CrystalBonusLabel");
				subElement.Text = this.lang.Get("crystal_percent_bonus", new object[]
				{
					Math.Round((saleItem.BonusMultiplier - 1.0) * 100.0)
				});
				UXLabel subElement2 = this.itemGrid.GetSubElement<UXLabel>(itemUid, "CrystalBonusLabelAmount");
				int num = (int)Math.Round((double)tag.Amount * saleItem.BonusMultiplier);
				subElement2.Text = this.lang.Get("CURRENCY_VALUE_NAME", new object[]
				{
					this.lang.ThousandsSeparated(num),
					this.lang.Get(tag.Currency.ToString().ToUpper(), new object[0])
				});
				UXLabel subElement3 = this.itemGrid.GetSubElement<UXLabel>(itemUid, "CrystalBonusLabelBonusAmount");
				subElement3.Text = this.lang.Get("crystal_amount_bonus", new object[]
				{
					num - tag.Amount
				});
				return;
			}
			this.itemGrid.GetSubElement<UXElement>(itemUid, "CrystalBonus").Visible = false;
		}

		private void RemoveSaleTitle()
		{
			base.GetElement<UXElement>("CrystalBonusTitleContainer").Visible = false;
			this.visibleSale = null;
		}

		private void AddIAPItems(List<UXElement> list)
		{
			InAppPurchaseController inAppPurchaseController = Service.Get<InAppPurchaseController>();
			List<InAppPurchaseTypeVO> validIAPTypes = inAppPurchaseController.GetValidIAPTypes();
			CurrencyType currency = CurrencyType.Crystals;
			List<SaleItemTypeVO> currentItemsOnSale = this.GetCurrentItemsOnSale();
			int count = validIAPTypes.Count;
			for (int i = 0; i < count; i++)
			{
				InAppPurchaseTypeVO inAppPurchaseTypeVO = validIAPTypes[i];
				InAppPurchaseProductInfo iAPProduct = inAppPurchaseController.GetIAPProduct(inAppPurchaseTypeVO.ProductId);
				string text = this.lang.Get("iap_title_" + inAppPurchaseTypeVO.Uid, new object[0]);
				string text2 = this.lang.Get("iap_desc_" + inAppPurchaseTypeVO.Uid, new object[0]);
				StoreItemTag storeItemTag = new StoreItemTag();
				storeItemTag.Amount = inAppPurchaseTypeVO.Amount;
				storeItemTag.Currency = currency;
				storeItemTag.IAPType = inAppPurchaseTypeVO;
				storeItemTag.IAPProduct = iAPProduct;
				string uid = inAppPurchaseTypeVO.Uid;
				UXElement uXElement = this.itemGrid.CloneTemplateItem(uid);
				uXElement.Tag = storeItemTag;
				storeItemTag.MainElement = this.itemGrid.GetSubElement<UXElement>(uid, "ItemInfo");
				UXLabel subElement = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelName");
				subElement.Text = text;
				storeItemTag.InfoLabel = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemInfo");
				storeItemTag.InfoLabel.Text = text2;
				storeItemTag.InfoLabel.Visible = false;
				UXButton subElement2 = this.itemGrid.GetSubElement<UXButton>(uid, "BtnItemInfo");
				subElement2.Visible = false;
				UXSprite subElement3 = this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteItemImage");
				Service.Get<InAppPurchaseController>().SetIAPRewardIcon(subElement3, inAppPurchaseTypeVO.Uid);
				string name = UXUtils.FormatAppendedName("Items", uid);
				JewelControl.Create(this, name);
				UXLabel subElement4 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemRequirement");
				subElement4.Visible = false;
				UXUtils.SetupCostElements(this, "Cost", uid, 0, 0, 0, 0, false, iAPProduct.FormattedRealCost);
				UXLabel subElement5 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelBuildTime");
				subElement5.Visible = false;
				UXSprite subElement6 = this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteItemTimeIcon");
				subElement6.Visible = false;
				UXLabel subElement7 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelItemCount");
				subElement7.Visible = false;
				UXLabel subElement8 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelCurrencyAmount");
				subElement8.Text = this.lang.Get("CURRENCY_VALUE_NAME", new object[]
				{
					this.lang.ThousandsSeparated(storeItemTag.Amount),
					this.lang.Get(currency.ToString().ToUpper(), new object[0])
				});
				UXSprite subElement9 = this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteDim");
				subElement9.Visible = false;
				UXButton subElement10 = this.itemGrid.GetSubElement<UXButton>(uid, "ButtonItemCard");
				subElement10.OnClicked = new UXButtonClickedDelegate(this.OnIAPItemButtonClicked);
				subElement10.Tag = storeItemTag;
				subElement10.Enabled = true;
				this.itemGrid.GetSubElement<UXElement>(uid, "PackageBadge").Visible = true;
				this.itemGrid.GetSubElement<UXElement>(uid, "SpritePackageBg").Visible = (inAppPurchaseTypeVO.TopBadgeString != null);
				UXLabel subElement11 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelPackageTop");
				UXLabel subElement12 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelPackageBottom");
				subElement11.Visible = (inAppPurchaseTypeVO.TopBadgeString != null);
				subElement12.Visible = (inAppPurchaseTypeVO.BottomBadgeString != null);
				if (inAppPurchaseTypeVO.TopBadgeString != null)
				{
					subElement11.Text = this.lang.Get(inAppPurchaseTypeVO.TopBadgeString, new object[0]);
				}
				if (inAppPurchaseTypeVO.BottomBadgeString != null)
				{
					subElement12.Text = this.lang.Get(inAppPurchaseTypeVO.BottomBadgeString, new object[0]);
				}
				SaleItemTypeVO saleItem = null;
				if (currentItemsOnSale != null)
				{
					foreach (SaleItemTypeVO current in currentItemsOnSale)
					{
						if (current.ProductId == inAppPurchaseTypeVO.ProductId)
						{
							saleItem = current;
							break;
						}
					}
				}
				this.SetupSaleItem(uid, saleItem, storeItemTag);
				this.HideLEIElements(uid);
				list.Add(uXElement);
			}
		}

		private void SetupCurTabElements()
		{
			this.itemGrid.Clear();
			this.categoryLabel.Text = this.lang.Get("s_" + this.curTab.ToString().ToLower(), new object[0]);
			List<UXElement> list = new List<UXElement>();
			this.RemoveProtectionCooldownTimer();
			this.RemoveSaleTitle();
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			List<LimitedEditionItemVO> validLEIs = Service.Get<LimitedEditionItemController>().ValidLEIs;
			StoreTab storeTab = this.curTab;
			switch (storeTab)
			{
			case StoreTab.NotInStore:
				break;
			case StoreTab.Treasure:
				this.AddIAPItems(list);
				this.AddLEItems(list, this.curTab, validLEIs);
				this.AddSupplyCrates(list);
				if (GameConstants.TAPJOY_AFTER_IAP)
				{
					this.AddOffersButton(list);
				}
				if (buildingLookupController.IsContrabandUnlocked())
				{
					this.AddCurrencyOrProtectionItems(list, false, CurrencyType.Contraband);
				}
				this.AddCurrencyOrProtectionItems(list, false, CurrencyType.Credits);
				this.AddCurrencyOrProtectionItems(list, false, CurrencyType.Materials);
				break;
			case StoreTab.Protection:
				this.AddLEItems(list, this.curTab, validLEIs);
				this.AddCurrencyOrProtectionItems(list, true, CurrencyType.Crystals);
				break;
			default:
				if (storeTab != StoreTab.EventPrizes)
				{
					this.AddLEItems(list, this.curTab, validLEIs);
					StoreScreen.AddOrCountBuildingItems(list, this.curTab, new StoreScreen.AddBuildingItemDelegate(this.AddBuildingItem));
				}
				else
				{
					this.AddLEItems(list, this.curTab, validLEIs);
					this.AddEventPrizeItems(list);
				}
				break;
			}
			if (this.curTab == StoreTab.Decorations && list.Count > 0 && this.turretSwappingUnlocked)
			{
				StoreItemTag storeItemTag = (StoreItemTag)list[0].Tag;
				int num = buildingLookupController.TurretBuildingNodeList.CalculateCount();
				int buildingMaxPurchaseQuantity = buildingLookupController.GetBuildingMaxPurchaseQuantity(storeItemTag.BuildingInfo, 0);
				this.turretLabelGroup.Visible = true;
				this.turretLabel.Text = this.lang.Get("TURRET_COUNT", new object[]
				{
					num,
					buildingMaxPurchaseQuantity
				});
				if (num >= buildingMaxPurchaseQuantity)
				{
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						storeItemTag = (StoreItemTag)list[i].Tag;
						storeItemTag.CanPurchase = false;
						storeItemTag.MaxQuantity = storeItemTag.CurQuantity;
						i++;
					}
				}
			}
			else
			{
				this.turretLabelGroup.Visible = false;
				this.turretLabel.Text = "";
			}
			UXUtils.SortListForTwoRowGrids(list, this.itemGrid);
			int j = 0;
			int count2 = list.Count;
			while (j < count2)
			{
				this.itemGrid.AddItem(list[j], j);
				j++;
			}
			this.itemGrid.RepositionItems();
			Service.Get<EventManager>().SendEvent(EventId.StoreScreenReady, this);
		}

		private static bool IsTurretMax(BuildingTypeVO buildingInfo)
		{
			if (Service.Get<BuildingLookupController>().IsTurretSwappingUnlocked())
			{
				BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
				int num = buildingLookupController.TurretBuildingNodeList.CalculateCount();
				int buildingMaxPurchaseQuantity = buildingLookupController.GetBuildingMaxPurchaseQuantity(buildingInfo, 0);
				if (num >= buildingMaxPurchaseQuantity)
				{
					return true;
				}
			}
			return false;
		}

		private static int CompareBuildingItem(UXElement a, UXElement b)
		{
			if (a == b)
			{
				return 0;
			}
			BuildingTypeVO buildingInfo = ((StoreItemTag)a.Tag).BuildingInfo;
			BuildingTypeVO buildingInfo2 = ((StoreItemTag)b.Tag).BuildingInfo;
			int num = buildingInfo.Order - buildingInfo2.Order;
			if (num == 0)
			{
				Service.Get<StaRTSLogger>().WarnFormat("Building {0} matches order ({1}) of {2}", new object[]
				{
					buildingInfo.Uid,
					buildingInfo.Order,
					buildingInfo2.Uid
				});
			}
			return num;
		}

		public void ScrollToItem(string itemUid)
		{
			this.curItem = itemUid;
			if (string.IsNullOrEmpty(this.curItem) || !base.IsLoaded() || !this.gridInitialized)
			{
				return;
			}
			int i = 0;
			int count = this.itemGrid.Count;
			while (i < count)
			{
				UXElement item = this.itemGrid.GetItem(i);
				StoreItemTag storeItemTag = item.Tag as StoreItemTag;
				if (storeItemTag.Uid == this.curItem && this.itemGrid.IsGridComponentScrollable())
				{
					this.itemGrid.RepositionItems();
					this.itemGrid.ScrollToItem(i);
					return;
				}
				i++;
			}
		}

		public void EnableScrollListMovement(bool enable)
		{
			this.gridScrollable = enable;
			if (this.gridInitialized)
			{
				this.itemGrid.IsScrollable = enable;
			}
		}

		private void OnTabButtonClicked(UXButton button)
		{
			StoreTab storeTab = (StoreTab)button.Tag;
			if (storeTab != this.curTab)
			{
				if (storeTab != StoreTab.Treasure)
				{
					Service.Get<EventManager>().UnregisterObserver(this, EventId.InventoryCrateCollectionClosed);
				}
				this.itemGrid.Scroll(0f);
				this.SetTab(storeTab);
				if (storeTab == StoreTab.NotInStore)
				{
					Service.Get<EventManager>().SendEvent(EventId.BackButtonClicked, null);
					base.InitDefaultBackDelegate();
					return;
				}
				Service.Get<EventManager>().SendEvent(EventId.StoreCategorySelected, storeTab);
				base.CurrentBackDelegate = new UXButtonClickedDelegate(this.OnTabButtonClicked);
				base.CurrentBackButton = this.backButton;
			}
		}

		private void OnInfoButtonClicked(UXButton button)
		{
			StoreItemTag storeItemTag = button.Tag as StoreItemTag;
			storeItemTag.MainElement.Visible = false;
			storeItemTag.InfoLabel.Visible = true;
			Service.Get<EventManager>().SendEvent(EventId.InfoButtonClicked, null);
		}

		private void OnBuildingItemButtonClicked(UXButton button)
		{
			StoreItemTag storeItemTag = button.Tag as StoreItemTag;
			if (storeItemTag.InfoLabel.Visible)
			{
				storeItemTag.InfoLabel.Visible = false;
				storeItemTag.MainElement.Visible = true;
				return;
			}
			if (!storeItemTag.CanPurchase)
			{
				string text = null;
				BuildingTypeVO reqBuilding = storeItemTag.ReqBuilding;
				if (reqBuilding != null)
				{
					if (!storeItemTag.ReqMet)
					{
						text = this.lang.Get("STORE_MESSAGE_UNLOCK", new object[]
						{
							reqBuilding.Lvl,
							LangUtils.GetBuildingDisplayName(reqBuilding)
						});
					}
					else if (storeItemTag.CurQuantity == storeItemTag.MaxQuantity)
					{
						BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
						BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
						int num = 0;
						List<Entity> buildingListByType = buildingLookupController.GetBuildingListByType(reqBuilding.Type);
						int i = 0;
						int count = buildingListByType.Count;
						while (i < count)
						{
							num = Math.Max(num, buildingListByType[i].Get<BuildingComponent>().BuildingType.Lvl);
							i++;
						}
						int lvl = buildingUpgradeCatalog.GetMaxLevel(reqBuilding.UpgradeGroup).Lvl;
						if (num < lvl)
						{
							int buildingMaxPurchaseQuantity = buildingLookupController.GetBuildingMaxPurchaseQuantity(storeItemTag.BuildingInfo, num + 1);
							if (buildingMaxPurchaseQuantity > storeItemTag.MaxQuantity)
							{
								text = this.lang.Get("STORE_MESSAGE_UPGRADE", new object[]
								{
									LangUtils.GetBuildingDisplayName(reqBuilding)
								});
							}
						}
					}
				}
				if (text == null)
				{
					text = this.lang.Get("STORE_MESSAGE_MAX", new object[0]);
				}
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(text);
				return;
			}
			button.Enabled = false;
			BuildingController buildingController = Service.Get<BuildingController>();
			buildingController.PrepareAndPurchaseNewBuilding(storeItemTag.BuildingInfo);
			Service.Get<EventManager>().SendEvent(EventId.BuildingSelectedFromStore, null);
			this.Close(storeItemTag.BuildingInfo.Uid);
		}

		public virtual void OnViewClockTime(float dt)
		{
			StoreTab storeTab = this.curTab;
			if (storeTab != StoreTab.Treasure)
			{
				if (storeTab != StoreTab.Protection)
				{
					return;
				}
				if (this.enableTimer)
				{
					this.RefreshProtectionCooldownTimer();
				}
			}
			else if (this.visibleSale != null)
			{
				if (!TimedEventUtils.IsTimedEventActive(this.visibleSale))
				{
					this.requiresRefresh = true;
					this.RefreshView();
					return;
				}
				UXLabel element = base.GetElement<UXLabel>("CyrstalBonusLabelExpire");
				int secondsRemaining = TimedEventUtils.GetSecondsRemaining(this.visibleSale);
				element.Text = this.lang.Get("crystal_bonus_ends_in", new object[]
				{
					LangUtils.FormatTime((long)secondsRemaining)
				});
				return;
			}
		}

		public override void RefreshView()
		{
			if (base.IsLoaded() && this.Visible && this.requiresRefresh)
			{
				this.SetTab(this.curTab);
				this.requiresRefresh = false;
			}
			base.RefreshView();
		}

		public void RegisterForCrateFlyoutReOpen(string crateUId)
		{
			if (this.curTab == StoreTab.Treasure)
			{
				this.crateToReOpenInFlyout = crateUId;
				Service.Get<EventManager>().RegisterObserver(this, EventId.InventoryCrateCollectionClosed);
				Service.Get<EventManager>().RegisterObserver(this, EventId.EquipmentUnlocked);
				Service.Get<EventManager>().RegisterObserver(this, EventId.ShardUnitUpgraded);
			}
		}

		private void UnregisterForCrateFlyoutReOpen()
		{
			this.crateToReOpenInFlyout = null;
			Service.Get<EventManager>().UnregisterObserver(this, EventId.InventoryCrateCollectionClosed);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.EquipmentUnlocked);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ShardUnitUpgraded);
		}

		private void OnOffersButtonClicked(UXButton button)
		{
			this.Close(null);
		}

		private void OnIAPItemButtonClicked(UXButton button)
		{
			StoreItemTag storeItemTag = button.Tag as StoreItemTag;
			Service.Get<EventManager>().SendEvent(EventId.InAppPurchaseSelect, storeItemTag.IAPType.ProductId);
			Service.Get<InAppPurchaseController>().PurchaseProduct(storeItemTag.IAPType.ProductId);
		}

		private void OnSoftCurrencyItemButtonClicked(UXButton button)
		{
			StoreItemTag storeItemTag = button.Tag as StoreItemTag;
			Service.Get<EventManager>().SendEvent(EventId.SoftCurrencyPurchaseSelect, storeItemTag.Uid);
			if (storeItemTag.ReqMet)
			{
				string message = this.lang.Get("PURCHASE_SOFT_CURRENCY", new object[]
				{
					this.lang.ThousandsSeparated(storeItemTag.Amount),
					this.lang.Get(storeItemTag.Currency.ToString().ToUpper(), new object[0]),
					storeItemTag.Price
				});
				AlertScreen.ShowModalWithImage(false, null, message, storeItemTag.IconName, new OnScreenModalResult(this.OnPurchaseSoftCurrency), storeItemTag);
				return;
			}
			GameUtils.ShowNotEnoughStorageMessage(storeItemTag.Currency);
		}

		private void OnProtectionItemButtonClicked(UXButton button)
		{
			StoreItemTag storeItemTag = button.Tag as StoreItemTag;
			if (!storeItemTag.ReqMet)
			{
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsError(this.lang.Get("PROTECTION_COOLDOWN", new object[0]));
				return;
			}
			string message = this.lang.Get("PURCHASE_PROTECTION", new object[]
			{
				this.lang.Get(string.Format("PACK_PROTECTION{0}", new object[]
				{
					storeItemTag.Amount
				}), new object[0]),
				storeItemTag.Price
			});
			AlertScreen.ShowModalWithImage(false, null, message, storeItemTag.IconName, new OnScreenModalResult(this.OnPurchaseProtection), storeItemTag);
		}

		private void OnLEItemButtonClicked(UXButton button)
		{
			StoreItemTag storeItemTag = button.Tag as StoreItemTag;
			LimitedEditionItemVO limitedEditionItemVO = Service.Get<IDataController>().Get<LimitedEditionItemVO>(storeItemTag.Uid);
			CrateVO vo = Service.Get<IDataController>().Get<CrateVO>(limitedEditionItemVO.CrateId);
			this.EnterCratePurchaseFlow(vo);
		}

		private void OnSupplyCrateItemButtonClicked(UXButton button)
		{
			StoreItemTag storeItemTag = button.Tag as StoreItemTag;
			CrateVO crateVO = Service.Get<IDataController>().Get<CrateVO>(storeItemTag.Uid);
			if (CrateUtils.IsPurchasableInStore(crateVO))
			{
				this.EnterCratePurchaseFlow(crateVO);
				return;
			}
			AlertScreen.ShowModal(false, null, this.lang.Get("ALL_CRATES_ALREADY_PURCHASED", new object[0]), null, null);
		}

		private void OpenCrateModalFlyoutForStore(string crateId, string planetID)
		{
			CrateInfoModalScreen crateInfoModalScreen = CrateInfoModalScreen.CreateForStore(crateId, planetID);
			crateInfoModalScreen.IsAlwaysOnTop = true;
			Service.Get<ScreenController>().AddScreen(crateInfoModalScreen, true, false);
		}

		private void EnterCratePurchaseFlow(CrateVO vo)
		{
			string uid = vo.Uid;
			string planetId = Service.Get<CurrentPlayer>().PlanetId;
			this.OpenCrateModalFlyoutForStore(uid, planetId);
			Service.Get<EventManager>().SendEvent(EventId.CrateStoreOpen, vo.Uid);
		}

		private void OnPurchaseSoftCurrency(object result, object cookie)
		{
			if (result == null)
			{
				return;
			}
			StoreItemTag storeItemTag = cookie as StoreItemTag;
			this.changingInventory = true;
			try
			{
				GameUtils.BuySoftCurrencyWithCrystals(storeItemTag.Currency, storeItemTag.Amount, storeItemTag.Price, "soft_currency_pack|" + storeItemTag.Uid, false);
			}
			finally
			{
				this.changingInventory = false;
				this.RefreshAfterCurrencyChange();
			}
		}

		private void OnPurchaseProtection(object result, object cookie)
		{
			if (result == null)
			{
				return;
			}
			StoreItemTag storeItemTag = cookie as StoreItemTag;
			this.changingInventory = true;
			try
			{
				GameUtils.BuyProtectionPackWithCrystals(storeItemTag.Amount);
			}
			finally
			{
				this.changingInventory = false;
				this.RefreshAfterCurrencyChange();
			}
		}

		private bool IsProtectionPackAvailable(int packNumber)
		{
			int protectionPurchaseCooldown = (int)Service.Get<CurrentPlayer>().GetProtectionPurchaseCooldown(packNumber);
			if (protectionPurchaseCooldown < 1)
			{
				return true;
			}
			int time = (int)ServerTime.Time;
			return time >= protectionPurchaseCooldown;
		}

		private int GetProtectionCooldownRemainingInSeconds(int packNumber)
		{
			int num = (int)(GameUtils.GetNowJavaEpochTime() / 1000L);
			int protectionPurchaseCooldown = (int)Service.Get<CurrentPlayer>().GetProtectionPurchaseCooldown(packNumber);
			if (protectionPurchaseCooldown < 1)
			{
				return 0;
			}
			return protectionPurchaseCooldown - num;
		}

		private int GetProtectionPackDuration(int packNumber)
		{
			int[] array;
			int[] array2;
			GameUtils.GetProtectionPacks(out array, out array2);
			return array[packNumber - 1];
		}

		private void RefreshAfterCurrencyChange()
		{
			if (!Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				this.SetTab(this.curTab);
				Service.Get<EventManager>().SendEvent(EventId.ScreenRefreshed, this);
			}
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.EquipmentUnlocked)
			{
				if (id <= EventId.ButtonHighlightActivated)
				{
					if (id != EventId.InventoryResourceUpdated)
					{
						if (id == EventId.ButtonHighlightActivated)
						{
							base.GetOptionalElement<UXElement>("ContainerJewel" + StoreTab.Resources.ToString()).Visible = false;
							base.GetOptionalElement<UXElement>("ContainerJewel" + StoreTab.Army.ToString()).Visible = false;
							base.GetOptionalElement<UXElement>("ContainerJewel" + StoreTab.Defenses.ToString()).Visible = false;
							base.GetOptionalElement<UXElement>("ContainerJewel" + StoreTab.Decorations.ToString()).Visible = false;
						}
					}
					else if (!this.changingInventory)
					{
						this.RefreshAfterCurrencyChange();
					}
				}
				else if (id != EventId.PlayerFactionChanged)
				{
					if (id == EventId.EquipmentUnlocked)
					{
						if (Service.Get<ArmoryController>().AllowUnlockCelebration)
						{
							this.UnregisterForCrateFlyoutReOpen();
						}
					}
				}
				else
				{
					this.SetTab(this.curTab);
				}
			}
			else
			{
				if (id <= EventId.ShardUnitUpgraded)
				{
					if (id != EventId.EquipmentUnlockCelebrationPlayed)
					{
						if (id != EventId.ShardUnitUpgraded)
						{
							goto IL_19E;
						}
						IDeployableVO deployableVO = (IDeployableVO)cookie;
						if (deployableVO != null && deployableVO.Lvl > 1)
						{
							this.UnregisterForCrateFlyoutReOpen();
							goto IL_19E;
						}
						goto IL_19E;
					}
				}
				else if (id != EventId.DeployableUnlockCelebrationPlayed)
				{
					if (id == EventId.InventoryCrateCollectionClosed)
					{
						string planetId = Service.Get<CurrentPlayer>().PlanetId;
						this.OpenCrateModalFlyoutForStore(this.crateToReOpenInFlyout, planetId);
						this.UnregisterForCrateFlyoutReOpen();
						goto IL_19E;
					}
					goto IL_19E;
				}
				this.Close(null);
			}
			IL_19E:
			return base.OnEvent(id, cookie);
		}

		public void RestoreIcons()
		{
			this.SetupCurTabElements();
		}

		protected internal StoreScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).AddBuildingItem((List<UXElement>)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2]), *(sbyte*)(args + 3) != 0, *(int*)(args + 4), *(int*)(args + 5));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).AddCurrencyOrProtectionItems((List<UXElement>)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, (CurrencyType)(*(int*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).AddEventPrizeItems((List<UXElement>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).AddIAPItems((List<UXElement>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).AddLEItems((List<UXElement>)GCHandledObjects.GCHandleToObject(*args), (StoreTab)(*(int*)(args + 1)), (List<LimitedEditionItemVO>)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).AddOffersButton((List<UXElement>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoreScreen.AddOrCountBuildingItems((List<UXElement>)GCHandledObjects.GCHandleToObject(*args), (StoreTab)(*(int*)(args + 1)), (StoreScreen.AddBuildingItemDelegate)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).AddProtectionCooldownTimer();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).AddSupplyCrates((List<UXElement>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoreScreen.CompareBuildingItem((UXElement)GCHandledObjects.GCHandleToObject(*args), (UXElement)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).CompareCrates((CrateVO)GCHandledObjects.GCHandleToObject(*args), (CrateVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoreScreen.CountUnlockedUnbuiltBuildings());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).EnableScrollListMovement(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).EnterCratePurchaseFlow((CrateVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).ShowCurrencyTray);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).Visible);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).GetCurrentItemsOnSale());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).GetProtectionCooldownRemainingInSeconds(*(int*)args));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).GetProtectionPackDuration(*(int*)args));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).HideLEIElements(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).InitGrids();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).InitLabels();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).IsProtectionPackAvailable(*(int*)args));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoreScreen.IsTurretMax((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).MoveUp((UXElement)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnBuildingItemButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnIAPItemButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnInfoButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnLEItemButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnOffersButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnProtectionItemButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnPurchaseProtection(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnPurchaseSoftCurrency(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).onScreenTransitionInComplete();
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnSoftCurrencyItemButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnSupplyCrateItemButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnTabButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).OpenCrateModalFlyoutForStore(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshAfterCurrencyChange();
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshProtectionCooldownTimer();
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).RegisterForCrateFlyoutReOpen(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).RemoveProtectionCooldownTimer();
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).RemoveSaleTitle();
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).ResetCurrentTab();
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).RestoreIcons();
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).ScrollToItem(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).Visible = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).SetTab((StoreTab)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).SetupCurTabElements();
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).SetupJewel((StoreTab)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).SetupLimitedEditionTab((StoreTab)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).SetupSaleItem(Marshal.PtrToStringUni(*(IntPtr*)args), (SaleItemTypeVO)GCHandledObjects.GCHandleToObject(args[1]), (StoreItemTag)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).SetupTab((StoreTab)(*(int*)args), (UXButton)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).SetupTab((StoreTab)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).SetupTabForFaction((StoreTab)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).ShowPromos(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((StoreScreen)GCHandledObjects.GCHandleToObject(instance)).UnregisterForCrateFlyoutReOpen();
			return -1L;
		}
	}
}
