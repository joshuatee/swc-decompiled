using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class StarshipInfoScreen : BuildingInfoScreen, IEventObserver
	{
		private const int STARSHIP_SLIDER_HITPOINTS = 0;

		private const int STARSHIP_SLIDER_CAPACITY = 1;

		private const int STARSHIP_SLIDER_COUNT = 2;

		public StarshipInfoScreen(Entity starshipBuilding) : base(starshipBuilding)
		{
			this.useStorageGroup = true;
		}

		protected override void InitLabels()
		{
			base.InitLabels();
			this.labelStorage.Text = this.lang.Get("ALL_STARSHIPS", new object[0]);
		}

		protected override void OnLoaded()
		{
			base.InitControls(2);
			this.InitHitpoints(0);
			this.sliders[1].DescLabel.Text = this.lang.Get("MOBILIZATION_CAPACITY", new object[0]);
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
			this.SetupTroopItemGrid();
		}

		private void UpdateHousingSpace()
		{
			GamePlayer worldOwner = GameUtils.GetWorldOwner();
			int totalStorageAmount = worldOwner.Inventory.SpecialAttack.GetTotalStorageAmount();
			int storage = this.buildingInfo.Storage;
			UXLabel currentLabel = this.sliders[1].CurrentLabel;
			currentLabel.Text = this.lang.Get("FRACTION", new object[]
			{
				this.lang.ThousandsSeparated(totalStorageAmount),
				this.lang.ThousandsSeparated(storage)
			});
			UXSlider currentSlider = this.sliders[1].CurrentSlider;
			currentSlider.Value = ((storage == 0) ? 0f : ((float)totalStorageAmount / (float)storage));
		}

		private void SetupTroopItemGrid()
		{
			base.InitGrid();
			IDataController dataController = Service.Get<IDataController>();
			foreach (SpecialAttackTypeVO current in dataController.GetAll<SpecialAttackTypeVO>())
			{
				int worldOwnerSpecialAttackCount = GameUtils.GetWorldOwnerSpecialAttackCount(current.Uid);
				if (worldOwnerSpecialAttackCount > 0)
				{
					base.AddTroopItem(current, worldOwnerSpecialAttackCount, null);
				}
			}
			base.RepositionGridItems();
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.InventoryTroopUpdated)
			{
				this.RefreshView();
			}
			return base.OnEvent(id, cookie);
		}

		protected internal StarshipInfoScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((StarshipInfoScreen)GCHandledObjects.GCHandleToObject(instance)).InitLabels();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((StarshipInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StarshipInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((StarshipInfoScreen)GCHandledObjects.GCHandleToObject(instance)).OnLoaded();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((StarshipInfoScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((StarshipInfoScreen)GCHandledObjects.GCHandleToObject(instance)).SetupTroopItemGrid();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((StarshipInfoScreen)GCHandledObjects.GCHandleToObject(instance)).UpdateHousingSpace();
			return -1L;
		}
	}
}
