using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class ChampionInfoScreen : DeployableInfoScreen
	{
		private const string TRP_INFO_SHIELD = "trp_info_shield";

		private BuildingTypeVO nextBuildingInfo;

		private BuildingTypeVO maxBuildingInfo;

		public ChampionInfoScreen(Entity platformBuilding, TroopTypeVO championType, bool forUpgrade) : base(new TroopUpgradeTag(championType, true), null, forUpgrade, platformBuilding)
		{
			this.wantsTransition = true;
			this.shouldCloseParent = false;
			BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
			this.nextBuildingInfo = buildingUpgradeCatalog.GetNextLevel(this.buildingInfo);
			this.maxBuildingInfo = buildingUpgradeCatalog.GetMaxLevel(this.buildingInfo);
			TroopTypeVO nextLevel = Service.Get<TroopUpgradeCatalog>().GetNextLevel(this.selectedTroop.Troop as TroopTypeVO);
			this.selectedTroop.ReqMet = Service.Get<UnlockController>().CanDeployableBeUpgraded(this.selectedTroop.Troop, nextLevel, out this.selectedTroop.RequirementText, out this.selectedTroop.ShortRequirementText);
		}

		protected override void OnScreenLoaded()
		{
			base.OnScreenLoaded();
			this.movementSpeedGroup.LocalPosition = this.unitCapacityGroup.LocalPosition;
			this.attackRangeGroup.LocalPosition = this.trainingTimeGroup.LocalPosition;
			this.trainingTimeGroup.LocalPosition = this.trainingCostGroup.LocalPosition;
			this.unitCapacityGroup.Visible = false;
			this.trainingCostGroup.Visible = false;
			this.trainingTimeNameLabel.Text = this.lang.Get("s_RepairTime", new object[0]);
			base.GetElement<UXButton>("BtnBack").Visible = false;
			if (this.nextBuildingInfo != null)
			{
				this.labelUpgradeTime.Text = GameUtils.GetTimeLabelFromSeconds(this.nextBuildingInfo.UpgradeTime);
				if (this.showUpgradeControls)
				{
					UXUtils.SetupSingleCostElement(this, "Cost", this.nextBuildingInfo.UpgradeCredits, this.nextBuildingInfo.UpgradeMaterials, this.nextBuildingInfo.UpgradeContraband, 0, 0, false, null);
				}
			}
		}

		protected override void SetupPerksButton()
		{
			UXButton element = base.GetElement<UXButton>("btnPerks");
			element.Tag = this.buildingInfo;
			element.OnClicked = new UXButtonClickedDelegate(base.OnPerksButtonClicked);
			element.Visible = Service.Get<PerkManager>().IsPerkAppliedToBuilding(this.buildingInfo);
		}

		protected override void SetupBar3()
		{
			base.SetupBar(3, this.lang.Get("trp_info_shield", new object[0]), this.buildingInfo.Health, this.showUpgradeControls ? this.nextBuildingInfo.Health : 0, this.maxBuildingInfo.Health);
		}

		private void OnPayMeForCurrencyResult(object result, object cookie)
		{
			if (GameUtils.HandleSoftCurrencyFlow(result, cookie) && !PayMeScreen.ShowIfNoFreeDroids(new OnScreenModalResult(this.OnPayMeForDroidResult), null))
			{
				this.ConfirmUpgrade();
			}
		}

		private void OnPayMeForDroidResult(object result, object cookie)
		{
			if (result != null)
			{
				this.ConfirmUpgrade();
			}
		}

		private void ConfirmUpgrade()
		{
			Service.Get<ISupportController>().StartBuildingUpgrade(this.nextBuildingInfo, this.selectedBuilding, false);
			if (this.nextBuildingInfo.Time > 0)
			{
				Service.Get<UXController>().HUD.ShowContextButtons(this.selectedBuilding);
			}
			this.Close(this.selectedBuilding.ID);
		}

		protected override void OnPurchaseClicked(UXButton button)
		{
			int upgradeCredits = this.nextBuildingInfo.UpgradeCredits;
			int upgradeMaterials = this.nextBuildingInfo.UpgradeMaterials;
			int upgradeContraband = this.nextBuildingInfo.UpgradeContraband;
			string buildingPurchaseContext = GameUtils.GetBuildingPurchaseContext(this.nextBuildingInfo, this.buildingInfo, true, false);
			if (PayMeScreen.ShowIfNotEnoughCurrency(upgradeCredits, upgradeMaterials, upgradeContraband, buildingPurchaseContext, new OnScreenModalResult(this.OnPayMeForCurrencyResult)))
			{
				return;
			}
			if (PayMeScreen.ShowIfNoFreeDroids(new OnScreenModalResult(this.OnPayMeForDroidResult), null))
			{
				return;
			}
			this.ConfirmUpgrade();
		}

		protected internal ChampionInfoScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ChampionInfoScreen)GCHandledObjects.GCHandleToObject(instance)).ConfirmUpgrade();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ChampionInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnPayMeForCurrencyResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ChampionInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnPayMeForDroidResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ChampionInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnPurchaseClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ChampionInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ChampionInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupBar3();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ChampionInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupPerksButton();
			return -1L;
		}
	}
}
