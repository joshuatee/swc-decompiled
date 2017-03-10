using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class TrainingUpgradeScreen : TrainingInfoScreen
	{
		private const int CENTER_THRESHOLD = 7;

		private BuildingLookupController buildingLookupController;

		private List<TroopUpgradeTag> troopListForInfoScreen;

		private UXGrid unlockGrid;

		public TrainingUpgradeScreen(Entity selectedBuilding) : base(selectedBuilding)
		{
			this.useUpgradeGroup = true;
		}

		public override void OnDestroyElement()
		{
			if (this.unlockGrid != null)
			{
				this.unlockGrid.Clear();
				this.unlockGrid = null;
			}
			base.OnDestroyElement();
		}

		protected override void InitGroups()
		{
			base.InitGroups();
			base.GetElement<UXElement>("UnlockItems").Visible = true;
		}

		protected override void InitLabels()
		{
			base.InitLabels();
			this.labelUnlock = base.GetElement<UXLabel>("LabelUnlock");
			DeliveryType deliveryType = this.deliveryType;
			string id;
			switch (deliveryType)
			{
			case DeliveryType.Vehicle:
				id = "UNLOCKS_VEHICLES";
				break;
			case DeliveryType.Starship:
				id = "UNLOCKS_STARSHIPS";
				break;
			case DeliveryType.Hero:
				id = "UNLOCKS_HEROES";
				break;
			default:
				if (deliveryType != DeliveryType.Mercenary)
				{
					id = "UNLOCKS_TROOPS";
				}
				else
				{
					id = "UNLOCKS_MERCENARIES";
				}
				break;
			}
			this.labelUnlock.Text = this.lang.Get(id, new object[0]);
		}

		protected override void OnLoaded()
		{
			base.InitControls(2);
			this.InitHitpoints(0);
			this.UpdateCapacity(1);
			this.InitUnlockItemGrid();
		}

		private void UpdateCapacity(int sliderIndex)
		{
			BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
			int storage = this.buildingInfo.Storage;
			int storage2 = buildingUpgradeCatalog.GetNextLevel(this.buildingInfo).Storage;
			int storage3 = buildingUpgradeCatalog.GetMaxLevel(this.buildingInfo.UpgradeGroup).Storage;
			DeliveryType deliveryType = this.deliveryType;
			string id;
			if (deliveryType != DeliveryType.Vehicle)
			{
				if (deliveryType != DeliveryType.Starship)
				{
					if (deliveryType != DeliveryType.Mercenary)
					{
						id = "UPGRADE_FIELD_CAPACITY";
					}
					else
					{
						id = "UPGRADE_FIELD_MERCENARY_CAPACITY";
					}
				}
				else
				{
					id = "UPGRADE_FIELD_STARSHIP_CAPACITY";
				}
			}
			else
			{
				id = "UPGRADE_FIELD_VEHICLE_CAPACITY";
			}
			SliderControl sliderControl = this.sliders[sliderIndex];
			sliderControl.DescLabel.Text = this.lang.Get(id, new object[0]);
			sliderControl.CurrentLabel.Text = this.lang.ThousandsSeparated(storage);
			sliderControl.CurrentSlider.Value = ((storage3 == 0) ? 0f : ((float)storage / (float)storage3));
			if (storage2 > storage)
			{
				sliderControl.NextLabel.Text = this.lang.Get("PLUS", new object[]
				{
					this.lang.ThousandsSeparated(storage2 - storage)
				});
				sliderControl.NextSlider.Value = ((storage3 == 0) ? 0f : ((float)storage2 / (float)storage3));
				return;
			}
			sliderControl.NextLabel.Visible = false;
			sliderControl.NextSlider.Visible = false;
		}

		private void InitUnlockItemGrid()
		{
			this.buildingLookupController = Service.Get<BuildingLookupController>();
			this.unlockGrid = base.GetElement<UXGrid>("BuildingUnlockGrid");
			this.unlockGrid.SetTemplateItem("BuildingUnlockTemplate");
			BuildingType type = this.nextBuildingInfo.Type;
			switch (type)
			{
			case BuildingType.Barracks:
			case BuildingType.Factory:
			case BuildingType.HeroMobilizer:
				break;
			case BuildingType.FleetCommand:
				this.InitStarshipUnlockGrid();
				goto IL_64;
			default:
				if (type != BuildingType.Cantina)
				{
					goto IL_64;
				}
				break;
			}
			this.InitTroopUnlockGrid();
			IL_64:
			if (this.unlockGrid.Count > 0)
			{
				this.unlockGrid.RepositionItems();
				this.unlockGrid.Scroll((this.unlockGrid.Count > 7) ? 0f : 0.5f);
				return;
			}
			base.GetElement<UXElement>("UnlockItems").Visible = false;
		}

		private void InitTroopUnlockGrid()
		{
			List<TroopTypeVO> troopsUnlockedByBuilding = this.buildingLookupController.GetTroopsUnlockedByBuilding(this.nextBuildingInfo.Uid);
			this.CreateUnlockCards<TroopTypeVO>(troopsUnlockedByBuilding);
		}

		private void InitStarshipUnlockGrid()
		{
			List<SpecialAttackTypeVO> starshipsUnlockedByBuilding = this.buildingLookupController.GetStarshipsUnlockedByBuilding(this.nextBuildingInfo.Uid);
			this.CreateUnlockCards<SpecialAttackTypeVO>(starshipsUnlockedByBuilding);
		}

		private void CreateUnlockCards<T>(List<T> deployableList) where T : IDeployableVO
		{
			this.troopListForInfoScreen = new List<TroopUpgradeTag>();
			int i = 0;
			int count = deployableList.Count;
			while (i < count)
			{
				IDeployableVO deployableVO = deployableList[i];
				if (deployableVO is TroopTypeVO)
				{
					int level = Service.Get<CurrentPlayer>().UnlockedLevels.Troops.GetLevel(deployableVO.UpgradeGroup);
					deployableVO = Service.Get<TroopUpgradeCatalog>().GetByLevel(deployableVO.UpgradeGroup, level);
				}
				else
				{
					int level2 = Service.Get<CurrentPlayer>().UnlockedLevels.Starships.GetLevel(deployableVO.UpgradeGroup);
					deployableVO = Service.Get<StarshipUpgradeCatalog>().GetByLevel(deployableVO.UpgradeGroup, level2);
				}
				if (deployableVO.PlayerFacing)
				{
					TroopUpgradeTag troopUpgradeTag = new TroopUpgradeTag(deployableVO, true);
					this.troopListForInfoScreen.Add(troopUpgradeTag);
					UXElement item = this.unlockGrid.CloneTemplateItem(deployableVO.Uid);
					UXButton subElement = this.unlockGrid.GetSubElement<UXButton>(deployableVO.Uid, "BuildingUnlockCard");
					subElement.OnClicked = new UXButtonClickedDelegate(this.OnUnlockCardClicked);
					subElement.Tag = troopUpgradeTag;
					UXSprite subElement2 = this.unlockGrid.GetSubElement<UXSprite>(deployableVO.Uid, "SpriteItemImageTroops");
					ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(deployableVO, subElement2);
					projectorConfig.AnimPreference = AnimationPreference.AnimationPreferred;
					ProjectorUtils.GenerateProjector(projectorConfig);
					UXLabel subElement3 = this.unlockGrid.GetSubElement<UXLabel>(deployableVO.Uid, "LabelUnlockCount");
					subElement3.Text = LangUtils.GetMultiplierText(1);
					this.unlockGrid.AddItem(item, deployableVO.Order);
				}
				i++;
			}
		}

		private void OnUnlockCardClicked(UXButton button)
		{
			Entity availableTroopResearchLab = Service.Get<BuildingLookupController>().GetAvailableTroopResearchLab();
			TroopUpgradeTag troopUpgradeTag = button.Tag as TroopUpgradeTag;
			bool showUpgradeControls = !string.IsNullOrEmpty(troopUpgradeTag.Troop.UpgradeShardUid);
			Service.Get<ScreenController>().AddScreen(new DeployableInfoScreen(troopUpgradeTag, this.troopListForInfoScreen, showUpgradeControls, availableTroopResearchLab));
		}

		public override void RefreshView()
		{
		}

		protected internal TrainingUpgradeScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TrainingUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).InitGroups();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TrainingUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).InitLabels();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TrainingUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).InitStarshipUnlockGrid();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TrainingUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).InitTroopUnlockGrid();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TrainingUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).InitUnlockItemGrid();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TrainingUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TrainingUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnLoaded();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TrainingUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnUnlockCardClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TrainingUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TrainingUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateCapacity(*(int*)args);
			return -1L;
		}
	}
}
