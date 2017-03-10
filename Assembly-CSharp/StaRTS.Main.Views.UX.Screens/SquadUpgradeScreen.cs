using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class SquadUpgradeScreen : SquadBuildingScreen
	{
		private const string UPGRADE_FIELD_CAPACITY = "UPGRADE_FIELD_CAPACITY";

		public SquadUpgradeScreen(Entity selectedBuilding) : base(selectedBuilding)
		{
			this.useUpgradeGroup = true;
		}

		protected override void OnLoaded()
		{
			base.InitControls(3);
			this.InitHitpoints(0);
			this.UpdateCapacity(1);
			this.InitReputation();
			base.InitTroopGrid();
		}

		private void UpdateSlider(int sliderIndex, string descText, int capacity, int capacityNext, int capacityTotal)
		{
			SliderControl sliderControl = this.sliders[sliderIndex];
			sliderControl.DescLabel.Text = descText;
			sliderControl.CurrentLabel.Text = this.lang.ThousandsSeparated(capacity);
			sliderControl.NextLabel.Text = this.lang.Get("PLUS", new object[]
			{
				this.lang.ThousandsSeparated(capacityNext - capacity)
			});
			sliderControl.CurrentSlider.Value = ((capacityTotal == 0) ? 0f : ((float)capacity / (float)capacityTotal));
			sliderControl.NextSlider.Value = ((capacityTotal == 0) ? 0f : ((float)capacityNext / (float)capacityTotal));
		}

		private void UpdateCapacity(int sliderIndex)
		{
			BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
			int storage = this.buildingInfo.Storage;
			int storage2 = buildingUpgradeCatalog.GetNextLevel(this.buildingInfo).Storage;
			int storage3 = buildingUpgradeCatalog.GetMaxLevel(this.buildingInfo.UpgradeGroup).Storage;
			this.UpdateSlider(sliderIndex, this.lang.Get("UPGRADE_FIELD_CAPACITY", new object[0]), storage, storage2, storage3);
		}

		protected override void UpdateReputation(int sliderIndex)
		{
			Inventory inventory = Service.Get<CurrentPlayer>().Inventory;
			if (!inventory.HasItem("reputation"))
			{
				this.sliders[sliderIndex].HideAll();
				Service.Get<StaRTSLogger>().WarnFormat("No reputation found in your inventory", new object[0]);
				return;
			}
			BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
			int itemCapacity = inventory.GetItemCapacity("reputation");
			int reputationCapacityForLevel = GameUtils.GetReputationCapacityForLevel(buildingUpgradeCatalog.GetNextLevel(this.buildingInfo).Lvl);
			int reputationCapacityForLevel2 = GameUtils.GetReputationCapacityForLevel(buildingUpgradeCatalog.GetMaxLevel(this.buildingInfo.UpgradeGroup).Lvl);
			this.UpdateSlider(sliderIndex, this.lang.Get("BUILDING_REPUTATION", new object[0]), itemCapacity, reputationCapacityForLevel, reputationCapacityForLevel2);
		}

		protected internal SquadUpgradeScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnLoaded();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateCapacity(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateReputation(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateSlider(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2), *(int*)(args + 3), *(int*)(args + 4));
			return -1L;
		}
	}
}
