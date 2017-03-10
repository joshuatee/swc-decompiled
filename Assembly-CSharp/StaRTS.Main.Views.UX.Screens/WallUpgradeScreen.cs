using Net.RichardLord.Ash.Core;
using StaRTS.Externals.BI;
using StaRTS.Externals.DMOAnalytics;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player.Building.Contracts;
using StaRTS.Main.Models.Commands.Player.Building.Upgrade;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class WallUpgradeScreen : BuildingInfoScreen
	{
		private const string UPGRADE_ALL_WALLS_BUTTON_COST = "SecondaryCost";

		private const string UPGRADE_ALL_WALLS_BUTTON_COST_LABEL = "SecondaryCostLabel";

		private List<Entity> wallsOfSameLevel;

		private int currentWallLevel;

		private int allWallSameLevelCount;

		public WallUpgradeScreen(Entity selectedBuilding) : base(selectedBuilding)
		{
			this.useUpgradeGroup = true;
			this.currentWallLevel = this.buildingInfo.Lvl;
			this.wallsOfSameLevel = new List<Entity>();
			string key = selectedBuilding.Get<BuildingComponent>().BuildingTO.Key;
			NodeList<WallNode> wallNodeList = Service.Get<BuildingLookupController>().WallNodeList;
			for (WallNode wallNode = wallNodeList.Head; wallNode != null; wallNode = wallNode.Next)
			{
				if (wallNode.BuildingComp.BuildingTO.Key != key && wallNode.BuildingComp.BuildingType.Lvl == this.currentWallLevel)
				{
					this.wallsOfSameLevel.Add(wallNode.Entity);
				}
			}
			this.allWallSameLevelCount = this.wallsOfSameLevel.Count + 1;
		}

		protected override void InitButtons()
		{
			base.InitButtons();
			this.buttonInstantBuy.Visible = false;
			this.buttonUpgradeAllWalls.Visible = true;
			this.buttonUpgradeAllWalls.Enabled = (this.reqMet && GameConstants.ENABLE_UPGRADE_ALL_WALLS);
			this.buttonUpgradeAllWalls.OnClicked = new UXButtonClickedDelegate(this.OnUpgradeAllWallsButton);
			int crystals = GameUtils.CrystalCostToUpgradeAllWalls(this.nextBuildingInfo.UpgradeMaterials, this.allWallSameLevelCount);
			UXUtils.SetupSingleCostElement(this, "SecondaryCost", 0, 0, 0, crystals, 0, !this.reqMet, null);
		}

		protected override void InitLabels()
		{
			base.InitLabels();
			if (this.reqMet)
			{
				this.labelPrimaryAction.Visible = true;
				this.labelSecondaryAction.Visible = true;
				this.labelPrimaryAction.Text = this.lang.Get("BUILDING_UPGRADE_SINGLE_WALL", new object[0]);
				this.labelSecondaryAction.Text = this.lang.Get("BUILDING_UPGRADE_ALL_WALLS", new object[]
				{
					this.currentWallLevel,
					this.allWallSameLevelCount
				});
			}
		}

		private void OnUpgradeAllWallsButton(UXButton button)
		{
			int num = GameUtils.CrystalCostToUpgradeAllWalls(this.nextBuildingInfo.UpgradeMaterials, this.allWallSameLevelCount);
			string title = this.lang.Get("upgrade_all_walls_confirm_title", new object[0]);
			string message = this.lang.Get("upgrade_all_walls_conifrm_desc", new object[]
			{
				this.allWallSameLevelCount,
				this.currentWallLevel + 1,
				num
			});
			AlertScreen.ShowModal(false, title, message, new OnScreenModalResult(this.ConfirmUpgradeAllWalls), null);
		}

		private void ConfirmUpgradeAllWalls(object result, object cookie)
		{
			string context = "UI_money_flow";
			string action = (result == null) ? "close" : "buy";
			string message = "upgrade_all_walls";
			Service.Get<BILoggingController>().TrackGameAction(context, action, message, null);
			if (result == null)
			{
				return;
			}
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.SendEvent(EventId.MuteEvent, EventId.InventoryResourceUpdated);
			eventManager.SendEvent(EventId.MuteEvent, EventId.ContractAdded);
			int num = GameUtils.CrystalCostToUpgradeAllWalls(this.nextBuildingInfo.UpgradeMaterials, this.allWallSameLevelCount);
			if (!GameUtils.SpendCrystals(num))
			{
				eventManager.SendEvent(EventId.UnmuteEvent, EventId.InventoryResourceUpdated);
				eventManager.SendEvent(EventId.UnmuteEvent, EventId.ContractAdded);
				return;
			}
			int currencyAmount = -num;
			string itemType = "soft_currency_flow";
			string itemId = "materials";
			int itemCount = 1;
			string type = "currency_purchase";
			string subType = "durable";
			Service.Get<DMOAnalyticsController>().LogInAppCurrencyAction(currencyAmount, itemType, itemId, itemCount, type, subType);
			string uid = this.buildingInfo.Uid;
			ISupportController supportController = Service.Get<ISupportController>();
			supportController.StartAllWallPartBuildingUpgrade(this.nextBuildingInfo, this.selectedBuilding, false, false);
			int count = this.wallsOfSameLevel.Count;
			for (int i = 0; i < count; i++)
			{
				supportController.StartAllWallPartBuildingUpgrade(this.nextBuildingInfo, this.wallsOfSameLevel[i], false, false);
			}
			BuildingUpgradeAllWallsCommand command = new BuildingUpgradeAllWallsCommand(new BuildingUpgradeAllWallsRequest
			{
				BuildingUid = uid,
				PlayerId = Service.Get<CurrentPlayer>().PlayerId
			});
			Service.Get<ServerAPI>().Enqueue(command);
			for (int j = 0; j < count; j++)
			{
				supportController.FinishCurrentContract(this.wallsOfSameLevel[j], j != 0, false);
			}
			supportController.FinishCurrentContract(this.selectedBuilding, true, false);
			eventManager.SendEvent(EventId.UnmuteEvent, EventId.InventoryResourceUpdated);
			eventManager.SendEvent(EventId.UnmuteEvent, EventId.ContractAdded);
			eventManager.SendEvent(EventId.SimulateAudioEvent, new AudioEventData(EventId.InventoryResourceUpdated, "crystals"));
		}

		protected internal WallUpgradeScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((WallUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).ConfirmUpgradeAllWalls(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WallUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WallUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).InitLabels();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((WallUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnUpgradeAllWallsButton((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
