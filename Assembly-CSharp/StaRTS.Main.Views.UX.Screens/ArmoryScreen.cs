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
using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class ArmoryScreen : ClosableScreen, IEventObserver
	{
		private const string BUILDING_INFO = "BUILDING_INFO";

		private const string ARMORY_CAPACITY = "ARMORY_CAPACITY";

		private const string ARMORY_INSTRUCTIONS = "ARMORY_CTA";

		private const string EQUIPMENT_LOCKED = "EQUIPMENT_LOCKED";

		private const string NOT_ENOUGH_CAPACITY = "ARMORY_FULL";

		private const string BASE_ON_INCORRECT_PLANET = "BASE_ON_INCORRECT_PLANET";

		private const string BUILDING_REQUIREMENT = "BUILDING_REQUIREMENT";

		private const string ACTIVATE_INSTRUCTION = "ACTIVATE_INSTRUCTION";

		private const string OBJECTIVE_PROGRESS = "OBJECTIVE_PROGRESS";

		private const string MAX_LEVEL = "MAX_LEVEL";

		private const string ARMORY_INACTIVE_CAPACITY_REACHED = "ARMORY_INACTIVE_CAPACITY_REACHED";

		private const string ARMORY_UPGRADE_NOW = "ARMORY_UPGRADE_NOW";

		private const string EQUIPMENT_TAB_PREFIX = "EQUIPMENT_TAB_";

		private const string LABEL_ARMORY_TITLE = "LabelTitle";

		private const string LABEL_CURRENT_CAPACITY = "LabelEquipmentActive";

		private const string LABEL_ARMORY_INSTRUCTIONS = "LabelEquipment";

		private const string LABEL_ACTIVATE_EQUIPMENT_INSTRUCTIONS = "LabelEquipmentActiveInstructions";

		private const string GRID_ACTIVE_EQUIPMENT = "GridEquipmentActive";

		private const string TEMPLATE_ACTIVE_EQUIPMENT = "EquipmentActiveItemTemplate";

		private const string GRID_EQUIPMENT = "EquipmentGrid";

		private const string TEMPLATE_EQUIPMENT_ITEM = "EquipmentItemTemplate";

		private const string EQUIPMENT_FRAGMENT_ICON = "SpriteIconFragment";

		private const string LABEL_EQUIPMENT_ACTIVE_NAME = "LabelEquipmentActiveName";

		private const string LABEL_EQUIPMENT_ACTIVE_LEVEL = "LabelEquipmentActiveLevel";

		private const string EQUIPMENT_ITEM_ACTIVE_ICON = "SpriteEquipmentActiveItemImage";

		private const string EQUIPMENT_ACTIVE_PLANET_ICON = "SpriteEquipmentActivePlanet";

		private const string BUTTON_EQUIPMENT_ACTIVE_CARD = "BtnEquipmentActiveItemCard";

		private const string BUTTON_EQUIPMENT_ACTIVE_CANCEL = "BtnEquipmentActiveCancel";

		private const string ACTIVE_EMPTY_CARD_BG_OUTLINE = "SpriteEquipmentActiveImageEmptySlot";

		private const string ACTIVE_CARD_BACKGROUND = "SpriteEquipmentActiveImageBkg";

		private const string ACTIVE_CARD_GRADIENT_BOTTOM = "SpriteEquipmentActiveGradientBottom";

		private const string SPRITE_EQUIPMENT_ACTIVE_IMAGE_BKG_STROKE = "SpriteEquipmentActiveImageBkgStroke";

		private const string SPRITE_EQUIPMENT_ACTIVE_IMAGE_BKG_GLOW = "SpriteEquipmentActiveImageBkgGlow";

		private const string ACTIVE_CARD = "EquipmentActiveItemCardQ{0}";

		private const string LABEL_EQUIPMENT_NAME = "LabelEquipmentName";

		private const string LABEL_EQUIPMENT_LEVEL = "LabelEquipmentLevel";

		private const string LABEL_EQUIPMENT_REQUIREMENT = "LabelEquipmentRequirement";

		private const string EQUIPMENT_ITEM_ICON = "SpriteEquipmentItemImage";

		private const string EQUIPMENT_PLANET_ICON = "SpriteEquipmentPlanet";

		private const string BUTTON_EQUIPMENT_CARD = "BtnEquipmentItemCard";

		private const string BUTTON_EQUIPMENT_INFO = "BtnEquipmentInfo";

		private const string SPRITE_BUTTON_DIMMER = "SpriteDim";

		private const string SPRITE_BUTTON_DIM_FULL = "SpriteDimFull";

		private const string LABEL_FRAG_PROGRESS = "LabelFragProgress";

		private const string SPRITE_LOCK_ICON = "SpriteLockIcon";

		private const string PLANET_LOCKED = "PlanetLocked";

		private const string ICON_UPGRADE = "IconUpgrade";

		private const string SPRITE_EQUIPMENT_IMAGE_BKG_STROKE = "SpriteEquipmentImageBkgStroke";

		private const string SPRITE_EQUIPMENT_ITEM_BAR_OUTLINE = "SpriteEquipmentItemBarOutline";

		private const string SPRITE_EQUIPMENT_IMAGE_BKG_GLOW = "SpriteEquipmentImageBkgGlow";

		private const string INACTIVE_CARD = "EquipmentItemCardQ{0}";

		private const float EQUIPMENT_GLOW_ALPHA = 0.4f;

		private const string INACTIVE_PROGRESS_BAR = "pBarEquipmentItemFrag";

		private const string INACTIVE_PROGRESS_BAR_SPRITE = "SpriteEquipmentItempBarFrag";

		private const int ACTIVE_GRID_MAX_CARDS_NO_SCROLL = 5;

		private const string EMPTY_ACTIVATE_INSTRUCTION_CARD = "EMPTY";

		private const int EFFECTIVE_MAX_INDEX = 500000;

		private const int SCROLL_POSITION_INVALID = -1;

		private static Dictionary<ShardQuality, Color> qualityColor;

		private static readonly Color quality1 = new Color(0.5411765f, 0.549019635f, 0.431372553f);

		private static readonly Color quality2 = new Color(0.211764708f, 0.34117648f, 1f);

		private static readonly Color quality3 = new Color(0.8784314f, 0.5411765f, 0.0392156877f);

		private UXLabel titleLabel;

		private UXLabel currentCapacityLabel;

		private UXLabel instructionsLabel;

		private UXGrid activeGrid;

		private UXGrid inactiveGrid;

		private BuildingTypeVO buildingInfo;

		private EquipmentTabHelper equipmentTabs;

		private int emptyCardID;

		private int activeCardID;

		private float inactiveScrollPosition;

		public ArmoryScreen(Entity armoryBuilding) : base("gui_armory")
		{
			this.buildingInfo = ((armoryBuilding == null) ? null : armoryBuilding.Get<BuildingComponent>().BuildingType);
			this.equipmentTabs = new EquipmentTabHelper();
			ArmoryScreen.qualityColor = new Dictionary<ShardQuality, Color>();
			ArmoryScreen.qualityColor.Add(ShardQuality.Basic, ArmoryScreen.quality1);
			ArmoryScreen.qualityColor.Add(ShardQuality.Advanced, ArmoryScreen.quality2);
			ArmoryScreen.qualityColor.Add(ShardQuality.Elite, ArmoryScreen.quality3);
			this.emptyCardID = 0;
			this.activeCardID = 0;
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.EquipmentActivated);
			eventManager.RegisterObserver(this, EventId.EquipmentDeactivated);
		}

		public override void OnDestroyElement()
		{
			this.ResetScreen();
			this.equipmentTabs = null;
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.EquipmentActivated);
			eventManager.UnregisterObserver(this, EventId.EquipmentDeactivated);
			base.OnDestroyElement();
		}

		protected override void OnScreenLoaded()
		{
			this.InitLabels();
			this.InitActiveGrid();
			this.InitInactiveGrid();
			this.InitTabs();
			base.InitButtons();
		}

		private void InitLabels()
		{
			ActiveArmory activeArmory = Service.Get<CurrentPlayer>().ActiveArmory;
			this.titleLabel = base.GetElement<UXLabel>("LabelTitle");
			this.titleLabel.Text = this.lang.Get("BUILDING_INFO", new object[]
			{
				LangUtils.GetBuildingDisplayName(this.buildingInfo),
				this.buildingInfo.Lvl
			});
			this.currentCapacityLabel = base.GetElement<UXLabel>("LabelEquipmentActive");
			this.currentCapacityLabel.Text = this.lang.Get("ARMORY_CAPACITY", new object[]
			{
				ArmoryUtils.GetCurrentActiveEquipmentCapacity(activeArmory),
				activeArmory.MaxCapacity
			});
			this.instructionsLabel = base.GetElement<UXLabel>("LabelEquipment");
			this.instructionsLabel.Text = this.lang.Get("ARMORY_CTA", new object[0]);
		}

		private void InitActiveGrid()
		{
			this.activeGrid = base.GetElement<UXGrid>("GridEquipmentActive");
			this.activeGrid.SetTemplateItem("EquipmentActiveItemTemplate");
			this.activeGrid.DupeOrdersAllowed = true;
			this.activeGrid.SetSortModeCustom();
			this.activeGrid.SetSortComparisonCallback(new Comparison<UXElement>(this.SortActiveGrid));
			this.PopulateActiveGrid();
			this.ShowInstructionalTextOnFirstEmptyCard(this.activeGrid);
			this.activeGrid.RepositionItemsFrameDelayed(new UXDragDelegate(this.ActiveGridRepositionComplete));
		}

		private void ActiveGridRepositionComplete(AbstractUXList list)
		{
			this.activeGrid.Scroll(0f);
		}

		private void InitInactiveGrid()
		{
			this.inactiveGrid = base.GetElement<UXGrid>("EquipmentGrid");
			this.inactiveGrid.SetTemplateItem("EquipmentItemTemplate");
			this.inactiveGrid.DupeOrdersAllowed = true;
			this.inactiveGrid.SetSortModeCustom();
			this.inactiveGrid.SetSortComparisonCallback(new Comparison<UXElement>(this.SortInactiveGrid));
			List<EquipmentVO> equipmentList = this.GenerateInactiveEquipmentList();
			this.PopulateInactiveGridWithList(equipmentList);
			this.inactiveScrollPosition = 0f;
			this.inactiveGrid.RepositionItemsFrameDelayed(new UXDragDelegate(this.RepositionInactiveGridItemsCallback), 2);
		}

		private int SortActiveGrid(UXElement elementA, UXElement elementB)
		{
			SortableEquipment a = elementA.Tag as SortableEquipment;
			SortableEquipment b = elementB.Tag as SortableEquipment;
			return ArmorySortUtils.SortWithList(a, b, new List<EquipmentSortMethod>
			{
				EquipmentSortMethod.EmptyEquipment,
				EquipmentSortMethod.DecrementingIndex,
				EquipmentSortMethod.IncrementingEmptyIndex
			});
		}

		private int SortInactiveGrid(UXElement elementA, UXElement elementB)
		{
			SortableEquipment a = elementA.Tag as SortableEquipment;
			SortableEquipment b = elementB.Tag as SortableEquipment;
			return ArmorySortUtils.SortWithList(a, b, new List<EquipmentSortMethod>
			{
				EquipmentSortMethod.UnlockedEquipment,
				EquipmentSortMethod.RequirementsMet,
				EquipmentSortMethod.CurrentPlanet,
				EquipmentSortMethod.Quality,
				EquipmentSortMethod.CapacitySize,
				EquipmentSortMethod.Alphabetical
			});
		}

		private void AddEmptyCardsToEnsureMinimumGridSize(UXGrid grid, int minimumGridSize)
		{
			int count = grid.Count;
			if (count >= minimumGridSize)
			{
				return;
			}
			for (int i = count; i < minimumGridSize; i++)
			{
				grid.AddItem(this.CreateEmptyCard(grid), i);
			}
		}

		private List<SortableEquipment> AddEmptyCardsToSortableEquipmentList(UXGrid grid, int minimumGridSize, List<SortableEquipment> equipmentList)
		{
			int count = equipmentList.Count;
			if (count >= minimumGridSize)
			{
				return equipmentList;
			}
			for (int i = count; i < minimumGridSize; i++)
			{
				equipmentList.Add(new SortableEquipment(null));
			}
			return equipmentList;
		}

		private bool IsElementInGrid(UXGrid grid, string cardUid)
		{
			List<UXElement> elementList = grid.GetElementList();
			int i = 0;
			int count = elementList.Count;
			while (i < count)
			{
				SortableEquipment sortableEquipment = elementList[i].Tag as SortableEquipment;
				if (sortableEquipment.Equipment != null && sortableEquipment.Equipment.Uid == cardUid)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		private UXElement CreateEmptyCardInternal(UXGrid grid, string index)
		{
			UXElement uXElement = grid.CloneTemplateItem(index);
			uXElement.Tag = new SortableEquipment(null);
			UXButton subElement = grid.GetSubElement<UXButton>(index, "BtnEquipmentActiveItemCard");
			subElement.Enabled = false;
			UXLabel subElement2 = grid.GetSubElement<UXLabel>(index, "LabelEquipmentActiveName");
			subElement2.Visible = false;
			UXLabel subElement3 = this.activeGrid.GetSubElement<UXLabel>(index, "LabelEquipmentActiveInstructions");
			subElement3.Visible = false;
			UXLabel subElement4 = grid.GetSubElement<UXLabel>(index, "LabelEquipmentActiveLevel");
			subElement4.Visible = false;
			UXSprite subElement5 = grid.GetSubElement<UXSprite>(index, "SpriteEquipmentActiveItemImage");
			subElement5.Visible = false;
			UXButton subElement6 = grid.GetSubElement<UXButton>(index, "BtnEquipmentActiveCancel");
			subElement6.Visible = false;
			UXUtils.HideAllQualityCards(grid, index, "EquipmentActiveItemCardQ{0}");
			grid.GetSubElement<UXSprite>(index, "SpriteEquipmentActiveImageBkg").Visible = false;
			grid.GetSubElement<UXSprite>(index, "SpriteEquipmentActiveGradientBottom").Visible = false;
			grid.GetSubElement<UXSprite>(index, "SpriteEquipmentActiveImageBkgGlow").Visible = false;
			grid.GetSubElement<UXSprite>(index, "SpriteEquipmentActiveImageBkgStroke").Visible = false;
			return uXElement;
		}

		private List<EquipmentVO> GenerateInactiveEquipmentList()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			IDictionary<string, int> levels = currentPlayer.UnlockedLevels.Equipment.Levels;
			EquipmentUpgradeCatalog equipmentUpgradeCatalog = Service.Get<EquipmentUpgradeCatalog>();
			List<string> iDCollection = equipmentUpgradeCatalog.GetIDCollection();
			List<SortableEquipment> list = new List<SortableEquipment>();
			EquipmentTab currentTab = (EquipmentTab)this.equipmentTabs.CurrentTab;
			int i = 0;
			int count = iDCollection.Count;
			while (i < count)
			{
				string text = iDCollection[i];
				int level = 1;
				if (levels.ContainsKey(text))
				{
					level = levels.get_Item(text);
				}
				EquipmentVO byLevel = equipmentUpgradeCatalog.GetByLevel(text, level);
				if (currentPlayer.Faction == byLevel.Faction && !currentPlayer.ActiveArmory.Equipment.Contains(byLevel.Uid) && this.equipmentTabs.IsEquipmentValidForTab(byLevel, currentTab))
				{
					list.Add(new SortableEquipment(currentPlayer, byLevel));
				}
				i++;
			}
			ArmorySortUtils.SortWithPriorityList(list, new List<EquipmentSortMethod>
			{
				EquipmentSortMethod.UnlockedEquipment,
				EquipmentSortMethod.RequirementsMet,
				EquipmentSortMethod.Quality,
				EquipmentSortMethod.CurrentPlanet,
				EquipmentSortMethod.CapacitySize,
				EquipmentSortMethod.Alphabetical
			});
			return ArmorySortUtils.RemoveWrapper(list);
		}

		private void PopulateInactiveGridWithList(List<EquipmentVO> equipmentList)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			int i = 0;
			int count = equipmentList.Count;
			while (i < count)
			{
				UXElement item = this.CreateInactiveCard(this.inactiveGrid, equipmentList[i], currentPlayer);
				this.inactiveGrid.AddItem(item, i);
				i++;
			}
		}

		private void PopulateActiveGrid()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			IDataController dataController = Service.Get<IDataController>();
			List<SortableEquipment> list = new List<SortableEquipment>();
			int i = 0;
			int count = currentPlayer.ActiveArmory.Equipment.Count;
			while (i < count)
			{
				EquipmentVO equipmentVO = dataController.Get<EquipmentVO>(currentPlayer.ActiveArmory.Equipment[i]);
				List<SortableEquipment> arg_5D_0 = list;
				EquipmentVO arg_58_0 = equipmentVO;
				int num = this.activeCardID;
				this.activeCardID = num + 1;
				arg_5D_0.Add(new SortableEquipment(arg_58_0, num));
				i++;
			}
			list = this.AddEmptyCardsToSortableEquipmentList(this.activeGrid, 5, list);
			int j = 0;
			int count2 = list.Count;
			while (j < count2)
			{
				UXElement item;
				if (list[j].HasEquipment())
				{
					item = this.CreateActiveCard(this.activeGrid, list[j].Equipment, currentPlayer);
				}
				else
				{
					item = this.CreateEmptyCard(this.activeGrid);
				}
				this.activeGrid.AddItem(item, j);
				j++;
			}
		}

		private void ShowInstructionalTextOnFirstEmptyCard(UXGrid grid)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (ArmoryUtils.IsAnyEquipmentActive(currentPlayer.ActiveArmory))
			{
				return;
			}
			List<UXElement> elementList = grid.GetElementList();
			int num = 500000;
			int i = 0;
			int count = elementList.Count;
			while (i < count)
			{
				SortableEquipment sortableEquipment = elementList[i].Tag as SortableEquipment;
				if (sortableEquipment.EmptyIndex < num)
				{
					num = sortableEquipment.EmptyIndex;
				}
				i++;
			}
			StringBuilder stringBuilder = new StringBuilder("EMPTY");
			string itemUid = stringBuilder.Append(num).ToString();
			UXLabel subElement = grid.GetSubElement<UXLabel>(itemUid, "LabelEquipmentActiveInstructions");
			subElement.Visible = true;
		}

		private UXElement CreateEmptyCard(UXGrid grid)
		{
			SortableEquipment sortableEquipment = new SortableEquipment(null);
			SortableEquipment arg_1C_0 = sortableEquipment;
			int num = this.emptyCardID;
			this.emptyCardID = num + 1;
			arg_1C_0.EmptyIndex = num;
			StringBuilder stringBuilder = new StringBuilder("EMPTY");
			string text = stringBuilder.Append(sortableEquipment.EmptyIndex).ToString();
			UXElement uXElement = this.CreateEmptyCardInternal(grid, text);
			uXElement.Tag = sortableEquipment;
			UXLabel subElement = this.activeGrid.GetSubElement<UXLabel>(text, "LabelEquipmentActiveInstructions");
			subElement.Text = this.lang.Get("ACTIVATE_INSTRUCTION", new object[0]);
			subElement.Visible = false;
			return uXElement;
		}

		private UXElement CreateCommonEquipmentCard(UXGrid grid, EquipmentVO equipment, string labelName, string labelLevel, string icon, string cardName, bool shouldAnimate, bool closeup)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			string uid = equipment.Uid;
			UXElement uXElement = grid.CloneTemplateItem(uid);
			uXElement.Tag = new SortableEquipment(equipment);
			UXLabel subElement = grid.GetSubElement<UXLabel>(uid, labelName);
			subElement.Text = LangUtils.GetEquipmentDisplayName(equipment);
			UXLabel subElement2 = grid.GetSubElement<UXLabel>(uid, labelLevel);
			if (ArmoryUtils.IsEquipmentOwned(currentPlayer, equipment))
			{
				subElement2.Text = LangUtils.GetLevelText(equipment.Lvl);
			}
			else
			{
				subElement2.Visible = false;
			}
			UXSprite subElement3 = grid.GetSubElement<UXSprite>(uid, icon);
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateEquipmentConfig(equipment, subElement3, closeup);
			projectorConfig.buildingEquipmentShaderName = "UnlitTexture_Fade";
			if (shouldAnimate)
			{
				projectorConfig.AnimPreference = AnimationPreference.AnimationPreferred;
			}
			ProjectorUtils.GenerateProjector(projectorConfig);
			UXUtils.SetCardQuality(this, grid, uid, (int)equipment.Quality, cardName);
			return uXElement;
		}

		private UXElement CreateActiveCard(UXGrid grid, EquipmentVO equipment, CurrentPlayer player)
		{
			UXElement uXElement = this.CreateCommonEquipmentCard(this.activeGrid, equipment, "LabelEquipmentActiveName", "LabelEquipmentActiveLevel", "SpriteEquipmentActiveItemImage", "EquipmentActiveItemCardQ{0}", true, false);
			SortableEquipment sortableEquipment = uXElement.Tag as SortableEquipment;
			SortableEquipment arg_45_0 = sortableEquipment;
			int num = this.activeCardID;
			this.activeCardID = num + 1;
			arg_45_0.IncrementingIndex = num;
			UXLabel subElement = this.activeGrid.GetSubElement<UXLabel>(equipment.Uid, "LabelEquipmentActiveInstructions");
			subElement.Visible = false;
			UXButton subElement2 = this.activeGrid.GetSubElement<UXButton>(equipment.Uid, "BtnEquipmentActiveCancel");
			subElement2.OnClicked = new UXButtonClickedDelegate(this.OnCancelButtonClicked);
			subElement2.Tag = uXElement;
			UXButton subElement3 = this.activeGrid.GetSubElement<UXButton>(equipment.Uid, "BtnEquipmentActiveItemCard");
			subElement3.Tag = equipment;
			subElement3.OnClicked = new UXButtonClickedDelegate(this.OnActiveCardButtonClicked);
			UXSprite subElement4 = grid.GetSubElement<UXSprite>(equipment.Uid, "SpriteEquipmentActiveImageBkgStroke");
			UXSprite subElement5 = grid.GetSubElement<UXSprite>(equipment.Uid, "SpriteEquipmentActiveImageBkgGlow");
			subElement4.Color = ArmoryScreen.qualityColor[equipment.Quality];
			subElement5.Color = ArmoryScreen.qualityColor[equipment.Quality];
			subElement5.Alpha = 0.4f;
			this.activeGrid.GetSubElement<UXSprite>(equipment.Uid, "SpriteEquipmentActiveImageEmptySlot").Visible = false;
			return uXElement;
		}

		private UXElement CreateInactiveCard(UXGrid grid, EquipmentVO equipment, CurrentPlayer currentPlayer)
		{
			UXElement uXElement = this.CreateCommonEquipmentCard(grid, equipment, "LabelEquipmentName", "LabelEquipmentLevel", "SpriteEquipmentItemImage", "EquipmentItemCardQ{0}", false, true);
			(uXElement.Tag as SortableEquipment).Player = currentPlayer;
			UXButton subElement = this.inactiveGrid.GetSubElement<UXButton>(equipment.Uid, "BtnEquipmentInfo");
			subElement.OnClicked = new UXButtonClickedDelegate(this.OnInfoButtonClicked);
			subElement.Tag = equipment;
			UXButton subElement2 = this.inactiveGrid.GetSubElement<UXButton>(equipment.Uid, "BtnEquipmentItemCard");
			subElement2.OnClicked = new UXButtonClickedDelegate(this.OnCardButtonClicked);
			subElement2.Tag = uXElement;
			EquipmentUpgradeCatalog equipmentUpgradeCatalog = Service.Get<EquipmentUpgradeCatalog>();
			UXSlider subElement3 = this.inactiveGrid.GetSubElement<UXSlider>(equipment.Uid, "pBarEquipmentItemFrag");
			UXSprite subElement4 = this.inactiveGrid.GetSubElement<UXSprite>(equipment.Uid, "SpriteEquipmentItempBarFrag");
			UXLabel subElement5 = grid.GetSubElement<UXLabel>(equipment.Uid, "LabelFragProgress");
			UXElement subElement6 = this.inactiveGrid.GetSubElement<UXElement>(equipment.Uid, "IconUpgrade");
			UXSprite subElement7 = grid.GetSubElement<UXSprite>(equipment.Uid, "SpriteEquipmentImageBkgStroke");
			UXSprite subElement8 = grid.GetSubElement<UXSprite>(equipment.Uid, "SpriteEquipmentItemBarOutline");
			UXSprite subElement9 = grid.GetSubElement<UXSprite>(equipment.Uid, "SpriteEquipmentImageBkgGlow");
			subElement7.Color = ArmoryScreen.qualityColor[equipment.Quality];
			subElement8.Color = ArmoryScreen.qualityColor[equipment.Quality];
			subElement9.Color = ArmoryScreen.qualityColor[equipment.Quality];
			subElement9.Alpha = 0.4f;
			float sliderProgressValue = this.GetSliderProgressValue(equipment, currentPlayer.GetShards(equipment.EquipmentID));
			UXSprite subElement10 = this.inactiveGrid.GetSubElement<UXSprite>(equipment.Uid, "SpriteIconFragment");
			UXUtils.SetupFragmentIconSprite(subElement10, (int)equipment.Quality);
			UXUtils.SetShardProgressBarValue(subElement3, subElement4, sliderProgressValue);
			subElement6.Visible = false;
			if (ArmoryUtils.IsAtMaxLevel(equipmentUpgradeCatalog, equipment))
			{
				subElement5.Text = this.lang.Get("MAX_LEVEL", new object[0]);
			}
			else
			{
				int shards = currentPlayer.GetShards(equipment.EquipmentID);
				int shardsRequiredForNextUpgrade = ArmoryUtils.GetShardsRequiredForNextUpgrade(currentPlayer, equipmentUpgradeCatalog, equipment);
				if (shards >= shardsRequiredForNextUpgrade)
				{
					subElement5.Text = this.lang.Get("ARMORY_UPGRADE_NOW", new object[0]);
				}
				else
				{
					subElement5.Text = this.lang.Get("OBJECTIVE_PROGRESS", new object[]
					{
						shards,
						shardsRequiredForNextUpgrade
					});
				}
			}
			if (ArmoryUtils.IsEquipmentOwned(currentPlayer, equipment))
			{
				EquipmentVO nextLevel = equipmentUpgradeCatalog.GetNextLevel(equipment);
				if (nextLevel != null)
				{
					if (Service.Get<ISupportController>().FindFirstContractWithProductUid(nextLevel.Uid) != null)
					{
						subElement5.Visible = false;
						subElement3.Visible = false;
					}
					else if (currentPlayer.GetShards(equipment.EquipmentID) >= nextLevel.UpgradeShards)
					{
						subElement6.Visible = true;
					}
				}
			}
			this.SetDimmerBasedOnRequirements(currentPlayer, equipment);
			return uXElement;
		}

		private void SetDimmerBasedOnRequirements(CurrentPlayer player, EquipmentVO equipment)
		{
			UXSprite subElement = this.inactiveGrid.GetSubElement<UXSprite>(equipment.Uid, "SpriteDim");
			UXSprite subElement2 = this.inactiveGrid.GetSubElement<UXSprite>(equipment.Uid, "SpriteDimFull");
			UXLabel subElement3 = this.inactiveGrid.GetSubElement<UXLabel>(equipment.Uid, "LabelEquipmentRequirement");
			UXSprite subElement4 = this.inactiveGrid.GetSubElement<UXSprite>(equipment.Uid, "SpriteLockIcon");
			UXElement subElement5 = this.inactiveGrid.GetSubElement<UXElement>(equipment.Uid, "PlanetLocked");
			subElement2.Visible = false;
			subElement4.Visible = false;
			subElement5.Visible = false;
			bool flag = ArmoryUtils.IsEquipmentOwned(player, equipment);
			if ((ArmoryUtils.IsBuildingRequirementMet(equipment) & flag) && ArmoryUtils.IsEquipmentValidForPlanet(equipment, player.PlanetId))
			{
				subElement3.Text = "";
				if (ArmoryUtils.HasEnoughCapacityToActivateEquipment(player.ActiveArmory, equipment))
				{
					subElement.Visible = false;
					return;
				}
				subElement3.Text = this.lang.Get("ARMORY_INACTIVE_CAPACITY_REACHED", new object[0]);
				subElement.Visible = true;
				return;
			}
			else
			{
				if (!ArmoryUtils.IsBuildingRequirementMet(equipment))
				{
					IDataController dataController = Service.Get<IDataController>();
					BuildingTypeVO buildingTypeVO = dataController.Get<BuildingTypeVO>(equipment.BuildingRequirement);
					subElement3.Text = this.lang.Get("BUILDING_REQUIREMENT", new object[]
					{
						buildingTypeVO.Lvl,
						LangUtils.GetBuildingDisplayName(buildingTypeVO)
					});
					subElement2.Visible = true;
					subElement.Visible = false;
					return;
				}
				UXButton subElement6 = this.inactiveGrid.GetSubElement<UXButton>(equipment.Uid, "BtnEquipmentItemCard");
				subElement6.Enabled = false;
				if (!ArmoryUtils.IsEquipmentOnValidPlanet(player, equipment) & flag)
				{
					subElement.Visible = false;
					subElement2.Visible = true;
					string planetDisplayName = LangUtils.GetPlanetDisplayName(player.PlanetId);
					subElement3.Text = this.lang.Get("BASE_ON_INCORRECT_PLANET", new object[]
					{
						planetDisplayName
					});
					subElement5.Visible = true;
					return;
				}
				subElement.Visible = false;
				subElement2.Visible = true;
				subElement4.Visible = true;
				if (player.Shards.ContainsKey(equipment.EquipmentID))
				{
					subElement3.Text = this.lang.Get("EQUIPMENT_LOCKED", new object[]
					{
						equipment.UpgradeShards - player.Shards[equipment.EquipmentID]
					});
					return;
				}
				subElement3.Text = this.lang.Get("EQUIPMENT_LOCKED", new object[]
				{
					equipment.UpgradeShards
				});
				return;
			}
		}

		private float GetSliderProgressValue(EquipmentVO equipment, int currentShards)
		{
			EquipmentVO nextLevel = Service.Get<EquipmentUpgradeCatalog>().GetNextLevel(equipment);
			if (nextLevel == null)
			{
				return 1f;
			}
			EquipmentVO equipmentVO;
			if (Service.Get<CurrentPlayer>().UnlockedLevels.Equipment.Has(equipment))
			{
				equipmentVO = nextLevel;
			}
			else
			{
				equipmentVO = equipment;
			}
			if (equipmentVO.UpgradeShards == 0)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("CMS Error: Shards required for {0} is zero", new object[]
				{
					equipment.Uid
				});
				return 0f;
			}
			float num = (float)currentShards / (float)equipmentVO.UpgradeShards;
			if (num <= 1f)
			{
				return num;
			}
			return 1f;
		}

		private void InitTabs()
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(EquipmentTab)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EquipmentTab key = (EquipmentTab)enumerator.get_Current();
					string text = key.ToString();
					StringBuilder stringBuilder = new StringBuilder("EQUIPMENT_TAB_");
					stringBuilder.Append(text.ToUpper());
					dictionary.Add((int)key, this.lang.Get(stringBuilder.ToString(), new object[0]));
				}
			}
			this.equipmentTabs.CreateTabs(this, new Action(this.OnEquipmentTabChanged), dictionary, 0);
			this.equipmentTabs.SetSelectable(true);
		}

		private void ResetScreen()
		{
			this.equipmentTabs.Destroy();
		}

		private void OnEquipmentTabChanged()
		{
			this.RecreateInactiveGrid();
		}

		private void OnActiveCardButtonClicked(UXButton button)
		{
			IDataController dataController = Service.Get<IDataController>();
			List<string> equipment = Service.Get<CurrentPlayer>().ActiveArmory.Equipment;
			List<EquipmentVO> list = new List<EquipmentVO>();
			int i = 0;
			int count = equipment.Count;
			while (i < count)
			{
				list.Add(dataController.Get<EquipmentVO>(equipment[i]));
				i++;
			}
			EquipmentVO selectedEquipment = button.Tag as EquipmentVO;
			Service.Get<ScreenController>().AddScreen(new EquipmentInfoScreen(selectedEquipment, list, null, false));
		}

		private void OnCancelButtonClicked(UXButton button)
		{
			UXElement uXElement = button.Tag as UXElement;
			EquipmentVO equipment = (uXElement.Tag as SortableEquipment).Equipment;
			Service.Get<ArmoryController>().DeactivateEquipment(equipment.EquipmentID);
		}

		private void OnCardButtonClicked(UXButton button)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			EquipmentVO equipmentVOFromCard = this.GetEquipmentVOFromCard(button.Tag as UXElement);
			if (!ArmoryUtils.IsEquipmentValidForPlanet(equipmentVOFromCard, currentPlayer.PlanetId))
			{
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(this.lang.Get("BASE_ON_INCORRECT_PLANET", new object[0]));
				return;
			}
			if (!ArmoryUtils.HasEnoughCapacityToActivateEquipment(currentPlayer.ActiveArmory, equipmentVOFromCard))
			{
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(this.lang.Get("ARMORY_FULL", new object[0]));
				return;
			}
			Service.Get<ArmoryController>().ActivateEquipment(equipmentVOFromCard.EquipmentID);
		}

		private void OnInfoButtonClicked(UXButton button)
		{
			EquipmentVO selectedEquipment = button.Tag as EquipmentVO;
			List<EquipmentVO> list = new List<EquipmentVO>();
			int i = 0;
			int count = this.inactiveGrid.Count;
			while (i < count)
			{
				EquipmentVO equipmentVOFromCard = this.GetEquipmentVOFromCard(this.inactiveGrid.GetItem(i));
				list.Add(equipmentVOFromCard);
				i++;
			}
			Service.Get<ScreenController>().AddScreen(new EquipmentInfoScreen(selectedEquipment, list, null, false));
		}

		private EquipmentVO GetEquipmentVOFromCard(UXElement card)
		{
			string uid = (card.Tag as SortableEquipment).Equipment.Uid;
			return Service.Get<IDataController>().Get<EquipmentVO>(uid);
		}

		public void RecreateInactiveGrid()
		{
			List<EquipmentVO> equipmentList = this.GenerateInactiveEquipmentList();
			this.inactiveGrid.Clear();
			this.PopulateInactiveGridWithList(equipmentList);
			this.inactiveScrollPosition = 0f;
			this.inactiveGrid.RepositionItemsFrameDelayed(new UXDragDelegate(this.RepositionInactiveGridItemsCallback), 2);
		}

		private void RemoveCardFromGrid(UXGrid grid, UXElement card)
		{
			grid.RemoveItem(card);
			base.DestroyElement(card);
		}

		private void RemoveCardFromGridByUid(UXGrid grid, string cardUid)
		{
			List<UXElement> elementList = grid.GetElementList();
			int i = 0;
			int count = elementList.Count;
			while (i < count)
			{
				SortableEquipment sortableEquipment = elementList[i].Tag as SortableEquipment;
				if (cardUid == sortableEquipment.Equipment.Uid)
				{
					this.RemoveCardFromGrid(grid, elementList[i]);
					return;
				}
				i++;
			}
		}

		private void RemoveAnEmptyCard(UXGrid grid)
		{
			List<UXElement> elementList = grid.GetElementList();
			bool flag = false;
			UXElement uXElement = null;
			int i = 0;
			int count = elementList.Count;
			while (i < count)
			{
				UXElement uXElement2 = elementList[i];
				if ((elementList[i].Tag as SortableEquipment).Equipment == null)
				{
					SortableEquipment sortableEquipment = uXElement2.Tag as SortableEquipment;
					StringBuilder stringBuilder = new StringBuilder("EMPTY");
					stringBuilder.Append(sortableEquipment.EmptyIndex);
					UXLabel subElement = this.activeGrid.GetSubElement<UXLabel>(stringBuilder.ToString(), "LabelEquipmentActiveInstructions");
					subElement.Visible = false;
					if (!flag)
					{
						uXElement = uXElement2;
						flag = true;
					}
				}
				i++;
			}
			if (uXElement != null)
			{
				this.RemoveCardFromGrid(grid, uXElement);
			}
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			ActiveArmory activeArmory = currentPlayer.ActiveArmory;
			EquipmentVO equipmentVO = cookie as EquipmentVO;
			float currentScrollPosition = this.activeGrid.GetCurrentScrollPosition(false);
			this.inactiveScrollPosition = this.inactiveGrid.GetCurrentScrollPosition(false);
			if (id != EventId.EquipmentActivated)
			{
				if (id == EventId.EquipmentDeactivated)
				{
					UXButton subElement = this.activeGrid.GetSubElement<UXButton>(equipmentVO.Uid, "BtnEquipmentActiveCancel");
					UXElement uXElement = subElement.Tag as UXElement;
					this.RemoveCardFromGrid(this.activeGrid, uXElement);
					this.AddEmptyCardsToEnsureMinimumGridSize(this.activeGrid, 5);
					this.activeGrid.RepositionItems(false);
					this.ShowInstructionalTextOnFirstEmptyCard(this.activeGrid);
					if (this.activeGrid.Count > 5)
					{
						this.activeGrid.Scroll(currentScrollPosition);
					}
					else
					{
						this.activeGrid.Scroll(0f);
					}
					SortableEquipment sortableEquipment = uXElement.Tag as SortableEquipment;
					if (this.equipmentTabs.IsEquipmentValidForTab(sortableEquipment.Equipment, (EquipmentTab)this.equipmentTabs.CurrentTab))
					{
						UXElement item = this.CreateInactiveCard(this.inactiveGrid, sortableEquipment.Equipment, currentPlayer);
						this.inactiveGrid.AddItem(item, this.inactiveGrid.Count + 1);
						this.inactiveGrid.RepositionItemsFrameDelayed(new UXDragDelegate(this.RepositionInactiveGridItemsCallback), 2);
					}
					this.currentCapacityLabel.Text = this.lang.Get("ARMORY_CAPACITY", new object[]
					{
						ArmoryUtils.GetCurrentActiveEquipmentCapacity(activeArmory),
						activeArmory.MaxCapacity
					});
					this.RefreshInactiveCardStatusesBasedOnOverallCapacity();
				}
			}
			else
			{
				this.RemoveCardFromGridByUid(this.inactiveGrid, equipmentVO.Uid);
				this.inactiveGrid.RepositionItemsFrameDelayed(new UXDragDelegate(this.RepositionInactiveGridItemsCallback), 2);
				this.RemoveAnEmptyCard(this.activeGrid);
				UXElement uXElement2 = this.CreateActiveCard(this.activeGrid, equipmentVO, currentPlayer);
				AbstractUXList arg_AD_0 = this.activeGrid;
				UXElement arg_AD_1 = uXElement2;
				int num = this.activeCardID;
				this.activeCardID = num + 1;
				arg_AD_0.AddItem(arg_AD_1, num);
				this.activeGrid.RepositionItems(false);
				this.ShowInstructionalTextOnFirstEmptyCard(this.activeGrid);
				if (this.activeGrid.Count > 5)
				{
					this.activeGrid.Scroll(currentScrollPosition);
				}
				else
				{
					this.activeGrid.Scroll(0f);
				}
				this.currentCapacityLabel.Text = this.lang.Get("ARMORY_CAPACITY", new object[]
				{
					ArmoryUtils.GetCurrentActiveEquipmentCapacity(activeArmory),
					activeArmory.MaxCapacity
				});
				this.RefreshInactiveCardStatusesBasedOnOverallCapacity();
			}
			return base.OnEvent(id, cookie);
		}

		private void RepositionInactiveGridItemsCallback(AbstractUXList list)
		{
			if (this.inactiveScrollPosition != -1f)
			{
				this.inactiveGrid.Scroll(this.inactiveScrollPosition);
			}
			this.inactiveScrollPosition = -1f;
		}

		private void RefreshInactiveCardStatusesBasedOnOverallCapacity()
		{
			CurrentPlayer player = Service.Get<CurrentPlayer>();
			List<EquipmentVO> list = this.GenerateInactiveEquipmentList();
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				this.SetDimmerBasedOnRequirements(player, list[i]);
				i++;
			}
		}

		public override void Close(object modalResult)
		{
			if (this.inactiveGrid != null)
			{
				this.inactiveGrid.Visible = false;
			}
			if (this.activeGrid != null)
			{
				this.activeGrid.Visible = false;
			}
			base.Close(modalResult);
		}

		protected internal ArmoryScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).ActiveGridRepositionComplete((AbstractUXList)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).AddEmptyCardsToEnsureMinimumGridSize((UXGrid)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).AddEmptyCardsToSortableEquipmentList((UXGrid)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), (List<SortableEquipment>)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).CreateActiveCard((UXGrid)GCHandledObjects.GCHandleToObject(*args), (EquipmentVO)GCHandledObjects.GCHandleToObject(args[1]), (CurrentPlayer)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).CreateCommonEquipmentCard((UXGrid)GCHandledObjects.GCHandleToObject(*args), (EquipmentVO)GCHandledObjects.GCHandleToObject(args[1]), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), *(sbyte*)(args + 6) != 0, *(sbyte*)(args + 7) != 0));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).CreateEmptyCard((UXGrid)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).CreateEmptyCardInternal((UXGrid)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).CreateInactiveCard((UXGrid)GCHandledObjects.GCHandleToObject(*args), (EquipmentVO)GCHandledObjects.GCHandleToObject(args[1]), (CurrentPlayer)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).GenerateInactiveEquipmentList());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).GetEquipmentVOFromCard((UXElement)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).GetSliderProgressValue((EquipmentVO)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).InitActiveGrid();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).InitInactiveGrid();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).InitLabels();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).InitTabs();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).IsElementInGrid((UXGrid)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).OnActiveCardButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).OnCancelButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).OnCardButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).OnEquipmentTabChanged();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).OnInfoButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).PopulateActiveGrid();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).PopulateInactiveGridWithList((List<EquipmentVO>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).RecreateInactiveGrid();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshInactiveCardStatusesBasedOnOverallCapacity();
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).RemoveAnEmptyCard((UXGrid)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).RemoveCardFromGrid((UXGrid)GCHandledObjects.GCHandleToObject(*args), (UXElement)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).RemoveCardFromGridByUid((UXGrid)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).RepositionInactiveGridItemsCallback((AbstractUXList)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).ResetScreen();
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).SetDimmerBasedOnRequirements((CurrentPlayer)GCHandledObjects.GCHandleToObject(*args), (EquipmentVO)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).ShowInstructionalTextOnFirstEmptyCard((UXGrid)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).SortActiveGrid((UXElement)GCHandledObjects.GCHandleToObject(*args), (UXElement)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ArmoryScreen)GCHandledObjects.GCHandleToObject(instance)).SortInactiveGrid((UXElement)GCHandledObjects.GCHandleToObject(*args), (UXElement)GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
