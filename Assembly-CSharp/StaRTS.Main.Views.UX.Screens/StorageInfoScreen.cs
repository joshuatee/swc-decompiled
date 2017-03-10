using Net.RichardLord.Ash.Core;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Elements;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class StorageInfoScreen : BuildingInfoScreen
	{
		protected const int STORAGE_SLIDER_HITPOINTS = 0;

		protected const int STORAGE_SLIDER_CAPACITY = 1;

		protected const int STORAGE_SLIDER_COUNT = 2;

		public StorageInfoScreen(Entity storageBuilding) : base(storageBuilding)
		{
		}

		protected override void OnLoaded()
		{
			base.InitControls(2);
			this.InitHitpoints(0);
			this.UpdateAmountStored(1);
		}

		private void UpdateAmountStored(int sliderIndex)
		{
			this.sliders[sliderIndex].DescLabel.Text = this.lang.Get("STORAGE_CAPACITY", new object[0]);
			int num = StorageSpreadUtils.CalculateAssumedCurrencyInStorage(this.buildingInfo.Currency, this.selectedBuilding);
			int storage = this.buildingInfo.Storage;
			UXLabel currentLabel = this.sliders[sliderIndex].CurrentLabel;
			currentLabel.Text = this.lang.Get("FRACTION", new object[]
			{
				this.lang.ThousandsSeparated(num),
				this.lang.ThousandsSeparated(storage)
			});
			UXSlider currentSlider = this.sliders[sliderIndex].CurrentSlider;
			float num2 = (storage == 0) ? 0f : ((float)num / (float)storage);
			currentSlider.Value = num2;
			this.projector.Config.MeterValue = num2;
		}

		protected internal StorageInfoScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((StorageInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnLoaded();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((StorageInfoScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateAmountStored(*(int*)args);
			return -1L;
		}
	}
}
