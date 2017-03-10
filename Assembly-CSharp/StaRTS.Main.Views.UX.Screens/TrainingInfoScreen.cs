using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class TrainingInfoScreen : BuildingInfoScreen, IEventObserver
	{
		protected const int TRAINING_SLIDER_HITPOINTS = 0;

		protected const int TRAINING_SLIDER_CAPACITY = 1;

		protected const int TRAINING_SLIDER_COUNT = 2;

		protected DeliveryType deliveryType;

		public TrainingInfoScreen(Entity trainingBuilding) : base(trainingBuilding)
		{
		}

		protected override void SetSelectedBuilding(Entity newSelectedBuilding)
		{
			base.SetSelectedBuilding(newSelectedBuilding);
			this.deliveryType = ContractUtils.GetTroopContractTypeByBuilding(this.buildingInfo);
		}

		protected override void OnLoaded()
		{
			base.InitControls(2);
			this.InitHitpoints(0);
			UXLabel descLabel = this.sliders[1].DescLabel;
			DeliveryType deliveryType = this.deliveryType;
			if (deliveryType != DeliveryType.Vehicle)
			{
				if (deliveryType != DeliveryType.Mercenary)
				{
					descLabel.Text = this.lang.Get("TRAINING_CAPACITY", new object[0]);
				}
				else
				{
					descLabel.Text = this.lang.Get("HIRE_CAPACITY", new object[0]);
				}
			}
			else
			{
				descLabel.Text = this.lang.Get("CONSTRUCTION_CAPACITY", new object[0]);
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.InventoryTroopUpdated, EventPriority.Default);
		}

		public override void OnDestroyElement()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.InventoryTroopUpdated);
			base.OnDestroyElement();
		}

		public override void RefreshView()
		{
			if (!base.IsLoaded())
			{
				return;
			}
			this.UpdateHousingSpace();
		}

		private void UpdateHousingSpace()
		{
			int num = ContractUtils.CalculateSpaceOccupiedByQueuedTroops(this.selectedBuilding);
			int storage = this.buildingInfo.Storage;
			UXLabel currentLabel = this.sliders[1].CurrentLabel;
			currentLabel.Text = this.lang.Get("FRACTION", new object[]
			{
				this.lang.ThousandsSeparated(num),
				this.lang.ThousandsSeparated(storage)
			});
			UXSlider currentSlider = this.sliders[1].CurrentSlider;
			currentSlider.Value = ((storage == 0) ? 0f : ((float)num / (float)storage));
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.InventoryTroopUpdated)
			{
				this.RefreshView();
			}
			return base.OnEvent(id, cookie);
		}

		protected internal TrainingInfoScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TrainingInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrainingInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TrainingInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnLoaded();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TrainingInfoScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TrainingInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetSelectedBuilding((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TrainingInfoScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateHousingSpace();
			return -1L;
		}
	}
}
