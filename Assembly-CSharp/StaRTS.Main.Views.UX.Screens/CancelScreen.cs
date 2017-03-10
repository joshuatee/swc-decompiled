using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
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
	public class CancelScreen : AlertScreen, IEventObserver
	{
		private Entity building;

		private Contract contract;

		public static void ShowModal(Entity building, OnScreenModalResult onModalResult, object modalResultCookie)
		{
			CancelScreen cancelScreen = new CancelScreen(building);
			cancelScreen.OnModalResult = onModalResult;
			cancelScreen.ModalResultCookie = modalResultCookie;
			Service.Get<ScreenController>().AddScreen(cancelScreen);
		}

		private CancelScreen(Entity building) : base(false, null, null, null, false)
		{
			this.building = building;
			Service.Get<EventManager>().RegisterObserver(this, EventId.ContractCompleted, EventPriority.Default);
			this.contract = Service.Get<ISupportController>().FindCurrentContract(building.Get<BuildingComponent>().BuildingTO.Key);
			if (this.contract != null)
			{
				string productUid = this.contract.ProductUid;
				string text = null;
				string id = "CANCEL_UPGRADE_TITLE";
				string id2 = "CANCEL_UPGRADE_MESSAGE";
				IDataController dataController = Service.Get<IDataController>();
				switch (this.contract.DeliveryType)
				{
				case DeliveryType.Champion:
					text = LangUtils.GetTroopDisplayName(dataController.Get<TroopTypeVO>(productUid));
					id = "CANCEL_REPAIR_TITLE";
					id2 = "CANCEL_REPAIR_MESSAGE";
					break;
				case DeliveryType.UpgradeBuilding:
				case DeliveryType.SwapBuilding:
				{
					BuildingTypeVO buildingInfo = dataController.Get<BuildingTypeVO>(productUid);
					text = LangUtils.GetBuildingDisplayName(buildingInfo);
					break;
				}
				case DeliveryType.UpgradeTroop:
				{
					TroopTypeVO troopInfo = dataController.Get<TroopTypeVO>(productUid);
					text = LangUtils.GetTroopDisplayName(troopInfo);
					break;
				}
				case DeliveryType.UpgradeStarship:
				{
					SpecialAttackTypeVO starshipInfo = dataController.Get<SpecialAttackTypeVO>(productUid);
					text = LangUtils.GetStarshipDisplayName(starshipInfo);
					break;
				}
				case DeliveryType.UpgradeEquipment:
				{
					EquipmentVO vo = dataController.Get<EquipmentVO>(productUid);
					text = LangUtils.GetEquipmentDisplayName(vo);
					break;
				}
				}
				this.title = ((text == null) ? "" : this.lang.Get(id, new object[]
				{
					text
				}));
				this.message = this.lang.Get(id2, new object[0]);
			}
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ContractCompleted)
			{
				ContractEventData contractEventData = cookie as ContractEventData;
				if (contractEventData.Contract == this.contract)
				{
					this.Close(null);
				}
			}
			return base.OnEvent(id, cookie);
		}

		public override void OnDestroyElement()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ContractCompleted);
			base.OnDestroyElement();
		}

		protected override void SetupControls()
		{
			base.SetupControls();
			this.primaryButton.Visible = false;
			this.primary2OptionButton.Visible = true;
			this.primary2OptionButton.Tag = this.building;
			this.primary2OptionButton.OnClicked = new UXButtonClickedDelegate(this.OnYesOrNoButtonClicked);
			this.secondary2OptionButton.Visible = true;
			this.secondary2OptionButton.Tag = null;
			this.secondary2OptionButton.OnClicked = new UXButtonClickedDelegate(this.OnYesOrNoButtonClicked);
			this.titleLabel.Text = this.title;
			this.centerLabel.Text = this.message;
		}

		private void OnYesOrNoButtonClicked(UXButton button)
		{
			button.Enabled = false;
			this.Close(button.Tag);
		}

		protected internal CancelScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CancelScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CancelScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((CancelScreen)GCHandledObjects.GCHandleToObject(instance)).OnYesOrNoButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((CancelScreen)GCHandledObjects.GCHandleToObject(instance)).SetupControls();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			CancelScreen.ShowModal((Entity)GCHandledObjects.GCHandleToObject(*args), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}
	}
}
