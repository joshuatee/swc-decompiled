using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class FinishNowScreen : AlertScreen, IViewClockTimeObserver
	{
		private const string INSTANT_UPGRADE_CONFIRM_TITLE = "instant_upgrade_confirm_title";

		private const string UPGRADE_TITLE_FINISH_NOW = "upgrade_title_FinishNow";

		private const string UPGRADE_DESC_FINISH_NOW = "upgrade_desc_FinishNow";

		private const string UPGRADE_DESC_FINISH_NOW_UNITS = "upgrade_desc_FinishNow_units";

		private const string INSTANT_UPGRADE_CONFIRM_DESC = "instant_upgrade_confirm_desc";

		private string perkId;

		private Entity buildingEntity;

		private Contract currentContract;

		private bool noContract;

		private string displayName;

		private int remainingTimeInSec;

		private int crystals;

		private string titleOverride;

		private string messageOverride;

		public static FinishNowScreen ShowModalWithNoContract(Entity selectedBuilding, OnScreenModalResult onModalResult, object modalResultCookie, int crystalCost)
		{
			FinishNowScreen finishNowScreen = FinishNowScreen.CreateFinishNowScreen(selectedBuilding, null, true, onModalResult, modalResultCookie);
			finishNowScreen.crystals = crystalCost;
			Service.Get<ScreenController>().AddScreen(finishNowScreen);
			return finishNowScreen;
		}

		public static FinishNowScreen ShowModalWithNoContract(Entity selectedBuilding, OnScreenModalResult onModalResult, object modalResultCookie, int crystalCost, string title, string message, bool alwaysOnTop)
		{
			FinishNowScreen finishNowScreen = FinishNowScreen.CreateFinishNowScreen(selectedBuilding, null, true, onModalResult, modalResultCookie);
			finishNowScreen.crystals = crystalCost;
			finishNowScreen.titleOverride = title;
			finishNowScreen.messageOverride = message;
			finishNowScreen.IsAlwaysOnTop = alwaysOnTop;
			Service.Get<ScreenController>().AddScreen(finishNowScreen);
			return finishNowScreen;
		}

		public static FinishNowScreen ShowModalPerk(string perkId, OnScreenModalResult onModalResult, object modalResultCookie, int crystalCost, string title, string message, bool alwaysOnTop)
		{
			FinishNowScreen finishNowScreen = FinishNowScreen.CreateFinishNowPerkScreen(perkId, onModalResult, modalResultCookie);
			finishNowScreen.crystals = crystalCost;
			finishNowScreen.titleOverride = title;
			finishNowScreen.messageOverride = message;
			finishNowScreen.IsAlwaysOnTop = alwaysOnTop;
			Service.Get<ScreenController>().AddScreen(finishNowScreen);
			return finishNowScreen;
		}

		public static void ShowModal(Entity selectedBuilding, OnScreenModalResult onModalResult, object modalResultCookie)
		{
			Contract contract = Service.Get<ISupportController>().FindCurrentContract(selectedBuilding.Get<BuildingComponent>().BuildingTO.Key);
			if (contract == null)
			{
				return;
			}
			FinishNowScreen screen = FinishNowScreen.CreateFinishNowScreen(selectedBuilding, contract, false, onModalResult, modalResultCookie);
			Service.Get<ScreenController>().AddScreen(screen);
		}

		private static FinishNowScreen CreateFinishNowScreen(Entity buildingEntity, Contract currentContract, bool noContract, OnScreenModalResult onScreenModalResult, object modalResultCookie)
		{
			return new FinishNowScreen(buildingEntity, currentContract, noContract)
			{
				OnModalResult = onScreenModalResult,
				ModalResultCookie = modalResultCookie
			};
		}

		private static FinishNowScreen CreateFinishNowPerkScreen(string perkId, OnScreenModalResult onScreenModalResult, object modalResultCookie)
		{
			return new FinishNowScreen(perkId)
			{
				OnModalResult = onScreenModalResult,
				ModalResultCookie = modalResultCookie
			};
		}

		private FinishNowScreen(Entity buildingEntity, Contract currentContract, bool noContract) : base(false, null, null, null, false)
		{
			this.buildingEntity = buildingEntity;
			this.perkId = null;
			this.currentContract = currentContract;
			this.noContract = noContract;
			IDataController dataController = Service.Get<IDataController>();
			if (currentContract != null)
			{
				switch (currentContract.DeliveryType)
				{
				case DeliveryType.Building:
					this.displayName = LangUtils.GetBuildingDisplayName(buildingEntity.Get<BuildingComponent>().BuildingType);
					break;
				case DeliveryType.UpgradeBuilding:
					this.displayName = LangUtils.GetBuildingDisplayName(buildingEntity.Get<BuildingComponent>().BuildingType);
					break;
				case DeliveryType.UpgradeTroop:
					this.displayName = LangUtils.GetTroopDisplayName(dataController.Get<TroopTypeVO>(currentContract.ProductUid));
					break;
				case DeliveryType.UpgradeStarship:
					this.displayName = LangUtils.GetStarshipDisplayName(dataController.Get<SpecialAttackTypeVO>(currentContract.ProductUid));
					break;
				case DeliveryType.UpgradeEquipment:
					this.displayName = LangUtils.GetEquipmentDisplayName(dataController.Get<EquipmentVO>(currentContract.ProductUid));
					break;
				}
			}
			else
			{
				this.displayName = LangUtils.GetBuildingDisplayName(buildingEntity.Get<BuildingComponent>().BuildingType);
			}
			this.RefreshData();
		}

		private FinishNowScreen(string perkId) : base(false, null, null, null, false)
		{
			this.buildingEntity = null;
			this.perkId = perkId;
			this.currentContract = null;
			this.noContract = true;
			this.displayName = "";
			this.RefreshData();
		}

		private void RefreshData()
		{
			if (this.currentContract != null)
			{
				this.remainingTimeInSec = this.currentContract.GetRemainingTimeForSim();
				this.crystals = GameUtils.SecondsToCrystals(this.remainingTimeInSec);
				if (this.remainingTimeInSec <= 0)
				{
					this.Close(null);
				}
			}
		}

		protected override void SetupControls()
		{
			if (!this.noContract)
			{
				Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
			}
			if (Service.Get<BuildingController>().SelectedBuilding == this.buildingEntity)
			{
				Service.Get<UXController>().HUD.ShowContextButtons(null);
			}
			if (this.geometry != null)
			{
				UXUtils.SetupGeometryForIcon(this.sprite, this.geometry);
			}
			this.payButton.Visible = true;
			if (this.buildingEntity == null)
			{
				this.payButton.Tag = this.perkId;
			}
			else
			{
				this.payButton.Tag = this.buildingEntity;
			}
			if (string.IsNullOrEmpty(this.titleOverride))
			{
				this.titleLabel.Text = Service.Get<Lang>().Get(this.noContract ? "instant_upgrade_confirm_title" : "upgrade_title_FinishNow", new object[0]);
			}
			else
			{
				this.titleLabel.Text = this.titleOverride;
			}
			this.payButton.OnClicked = new UXButtonClickedDelegate(this.OnPayButtonClicked);
			Service.Get<UserInputInhibitor>().AddToAllow(this.CloseButton);
			Service.Get<UserInputInhibitor>().AddToAllow(this.payButton);
			base.GetElement<UXLabel>("TickerDialogSmall").Visible = false;
		}

		public override void RefreshView()
		{
			if (!base.IsLoaded())
			{
				return;
			}
			if (this.remainingTimeInSec == 0 && !this.noContract)
			{
				this.Close(null);
			}
			string id = "upgrade_desc_FinishNow";
			if (this.currentContract != null && this.currentContract.DeliveryType != DeliveryType.UpgradeBuilding)
			{
				id = "upgrade_desc_FinishNow_units";
			}
			string text;
			if (this.messageOverride == null)
			{
				text = (this.noContract ? Service.Get<Lang>().Get("instant_upgrade_confirm_desc", new object[]
				{
					this.crystals
				}) : Service.Get<Lang>().Get(id, new object[]
				{
					GameUtils.GetTimeLabelFromSeconds(this.remainingTimeInSec),
					this.displayName,
					this.crystals
				}));
			}
			else
			{
				text = this.messageOverride;
			}
			if (this.geometry == null)
			{
				this.centerLabel.Text = text.Replace("\\n", Environment.NewLine);
			}
			else
			{
				this.rightLabel.Text = text.Replace("\\n", Environment.NewLine);
			}
			UXUtils.SetupCostElements(this, "Cost", null, 0, 0, 0, this.crystals, false, null);
		}

		public void OnPayButtonClicked(UXButton button)
		{
			button.Enabled = false;
			this.Close(button.Tag);
		}

		public void OnViewClockTime(float dt)
		{
			this.RefreshData();
			this.RefreshView();
		}

		public override void Close(object modalResult)
		{
			if (Service.Get<BuildingController>().SelectedBuilding == this.buildingEntity)
			{
				Service.Get<UXController>().HUD.ShowContextButtons(this.buildingEntity);
			}
			base.Close(modalResult);
		}

		public override void OnDestroyElement()
		{
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
			base.OnDestroyElement();
		}

		protected internal FinishNowScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((FinishNowScreen)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FinishNowScreen.CreateFinishNowPerkScreen(Marshal.PtrToStringUni(*(IntPtr*)args), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FinishNowScreen.CreateFinishNowScreen((Entity)GCHandledObjects.GCHandleToObject(*args), (Contract)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[3]), GCHandledObjects.GCHandleToObject(args[4])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((FinishNowScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((FinishNowScreen)GCHandledObjects.GCHandleToObject(instance)).OnPayButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((FinishNowScreen)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((FinishNowScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshData();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((FinishNowScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((FinishNowScreen)GCHandledObjects.GCHandleToObject(instance)).SetupControls();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			FinishNowScreen.ShowModal((Entity)GCHandledObjects.GCHandleToObject(*args), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FinishNowScreen.ShowModalPerk(Marshal.PtrToStringUni(*(IntPtr*)args), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]), *(int*)(args + 3), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), *(sbyte*)(args + 6) != 0));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FinishNowScreen.ShowModalWithNoContract((Entity)GCHandledObjects.GCHandleToObject(*args), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]), *(int*)(args + 3)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FinishNowScreen.ShowModalWithNoContract((Entity)GCHandledObjects.GCHandleToObject(*args), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]), *(int*)(args + 3), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5)), *(sbyte*)(args + 6) != 0));
		}
	}
}
