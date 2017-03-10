using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class StorageUpgradeScreen : StorageInfoScreen
	{
		public StorageUpgradeScreen(Entity selectedBuilding) : base(selectedBuilding)
		{
			this.useUpgradeGroup = true;
		}

		protected override void OnLoaded()
		{
			base.InitControls(2);
			this.InitHitpoints(0);
			this.UpdateCapacity(1);
		}

		private void UpdateCapacity(int sliderIndex)
		{
			BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
			int storage = this.buildingInfo.Storage;
			int storage2 = buildingUpgradeCatalog.GetNextLevel(this.buildingInfo).Storage;
			int storage3 = buildingUpgradeCatalog.GetMaxLevel(this.buildingInfo.UpgradeGroup).Storage;
			this.sliders[sliderIndex].DescLabel.Text = this.lang.Get("STORAGE_CAPACITY", new object[0]);
			this.sliders[sliderIndex].CurrentLabel.Text = this.lang.ThousandsSeparated(storage);
			this.sliders[sliderIndex].NextLabel.Text = this.lang.Get("PLUS", new object[]
			{
				this.lang.ThousandsSeparated(storage2 - storage)
			});
			this.sliders[sliderIndex].CurrentSlider.Value = ((storage3 == 0) ? 0f : ((float)storage / (float)storage3));
			this.sliders[sliderIndex].NextSlider.Value = ((storage3 == 0) ? 0f : ((float)storage2 / (float)storage3));
			int num = StorageSpreadUtils.CalculateAssumedCurrencyInStorage(this.buildingInfo.Currency, this.selectedBuilding);
			int storage4 = this.buildingInfo.Storage;
			float meterValue = (storage4 == 0) ? 0f : ((float)num / (float)storage4);
			this.projector.Config.MeterValue = meterValue;
		}

		protected internal StorageUpgradeScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((StorageUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnLoaded();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((StorageUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateCapacity(*(int*)args);
			return -1L;
		}
	}
}
