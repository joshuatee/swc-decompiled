using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Tags;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class PayMeScreen : AlertScreen, IEventObserver
	{
		public static readonly object MODALRESULT_CONFIRMED = new object();

		public static readonly object MODALRESULT_CONFIRMED_ALTERNATE = new object();

		private int crystals;

		private bool forDroids;

		private bool closing;

		private static void ShowModal(int crystals, bool forDroids, string title, string message, string spriteName, OnScreenModalResult onModalResult, object modalResultCookie)
		{
			PayMeScreen payMeScreen = new PayMeScreen(crystals, forDroids, title, message, spriteName);
			payMeScreen.OnModalResult = onModalResult;
			payMeScreen.ModalResultCookie = modalResultCookie;
			Service.Get<ScreenController>().AddScreen(payMeScreen);
		}

		public static bool ShowIfNoFreeDroids(OnScreenModalResult onModalResult, object modalResultCookie)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			int num = ContractUtils.CalculateDroidsInUse();
			if (num >= currentPlayer.CurrentDroidsAmount)
			{
				Lang lang = Service.Get<Lang>();
				string title = lang.Get("droid_title_AllDroidsBusy", new object[0]);
				string message = (currentPlayer.CurrentDroidsAmount < currentPlayer.MaxDroidsAmount) ? lang.Get("droid_desc_CompletePreviousBuildingOrBuy", new object[0]) : lang.Get("droid_desc_CompletePreviousBuilding", new object[0]);
				int num2 = ContractUtils.MinimumCostToFinish();
				PayMeScreen.ShowModal(num2, true, title, message, null, onModalResult, modalResultCookie);
				return true;
			}
			return false;
		}

		public static bool ShowIfNotEnoughCurrency(int credits, int materials, int contraband, string purchaseContext, OnScreenModalResult onModalResult)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			CurrencyType currencyType = GameUtils.GetCurrencyType(credits, materials, contraband);
			int num = 0;
			switch (currencyType)
			{
			case CurrencyType.Credits:
				num = credits - currentPlayer.CurrentCreditsAmount;
				break;
			case CurrencyType.Materials:
				num = materials - currentPlayer.CurrentMaterialsAmount;
				break;
			case CurrencyType.Contraband:
				num = contraband - currentPlayer.CurrentContrabandAmount;
				break;
			}
			if (num > 0)
			{
				if (GameUtils.HasEnoughCurrencyStorage(currencyType, num))
				{
					Lang lang = Service.Get<Lang>();
					string currencyStringId = LangUtils.GetCurrencyStringId(currencyType);
					string text = lang.Get(currencyStringId, new object[0]);
					string title = lang.Get("NEED_MORE", new object[]
					{
						text
					});
					string message = lang.Get("NEED_MORE_BUY_MISSING", new object[]
					{
						lang.ThousandsSeparated(num),
						text
					});
					int num2 = 0;
					switch (currencyType)
					{
					case CurrencyType.Credits:
						num2 = GameUtils.CreditsCrystalCost(num);
						break;
					case CurrencyType.Materials:
						num2 = GameUtils.MaterialsCrystalCost(num);
						break;
					case CurrencyType.Contraband:
						num2 = GameUtils.ContrabandCrystalCost(num);
						break;
					}
					string currencyItemAssetName = UXUtils.GetCurrencyItemAssetName(currencyType.ToString());
					CurrencyTag modalResultCookie = new CurrencyTag(currencyType, num, num2, purchaseContext);
					PayMeScreen.ShowModal(num2, false, title, message, currencyItemAssetName, onModalResult, modalResultCookie);
				}
				else
				{
					onModalResult(null, null);
				}
				return true;
			}
			return false;
		}

		private PayMeScreen(int crystals, bool forDroids, string title, string message, string spriteName) : base(false, title, message, spriteName, false)
		{
			this.crystals = crystals;
			this.forDroids = forDroids;
			this.closing = false;
			if (forDroids)
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.ContractCompleted, EventPriority.Default);
				Service.Get<EventManager>().RegisterObserver(this, EventId.ContractCanceled, EventPriority.Default);
			}
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ContractCompleted || id == EventId.ContractCanceled)
			{
				ContractEventData contractEventData = cookie as ContractEventData;
				if (ContractUtils.ContractTypeConsumesDroid(contractEventData.Contract.ContractTO.ContractType) && !this.closing)
				{
					this.Close(null);
				}
			}
			return base.OnEvent(id, cookie);
		}

		public override void OnDestroyElement()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ContractCompleted);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ContractCanceled);
			base.OnDestroyElement();
		}

		protected override void SetupControls()
		{
			base.GetElement<UXLabel>("TickerDialogSmall").Visible = false;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (this.forDroids && currentPlayer.CurrentDroidsAmount < currentPlayer.MaxDroidsAmount)
			{
				this.payLeftLabel.Text = this.lang.Get("FINISH", new object[0]);
				this.payLeftButton.Visible = true;
				this.payLeftButton.OnClicked = new UXButtonClickedDelegate(this.OnPayButtonClicked);
				UXUtils.SetupCostElements(this, "CostOptionPay1", null, 0, 0, 0, this.crystals, false, null);
				this.payRightLabel.Text = this.lang.Get("ADD_DROID", new object[0]);
				this.payRightButton.Visible = true;
				this.payRightButton.OnClicked = new UXButtonClickedDelegate(this.OnAlternatePayButtonClicked);
				int num = GameUtils.DroidCrystalCost(currentPlayer.CurrentDroidsAmount);
				UXUtils.SetupCostElements(this, "CostOptionPay2", null, 0, 0, 0, num, false, null);
				this.centerLabel.Text = this.message;
				this.centerLabel.Visible = true;
				this.rightLabel.Visible = false;
			}
			else
			{
				this.payButton.Visible = true;
				this.payButton.OnClicked = new UXButtonClickedDelegate(this.OnPayButtonClicked);
				UXUtils.SetupCostElements(this, "Cost", null, 0, 0, 0, this.crystals, false, null);
				if (this.forDroids)
				{
					this.centerLabel.Text = this.message;
					this.centerLabel.Visible = true;
					this.rightLabel.Visible = false;
				}
				else
				{
					this.rightLabel.Text = this.message;
					this.centerLabel.Visible = false;
					this.rightLabel.Visible = true;
					this.sprite.SpriteName = this.spriteName;
					UXUtils.SetupGeometryForIcon(this.sprite, this.spriteName);
				}
			}
			this.titleLabel.Text = this.title;
		}

		public void OnPayButtonClicked(UXButton button)
		{
			this.closing = true;
			object modalResult = null;
			button.Enabled = false;
			if (!this.forDroids)
			{
				Service.Get<EventManager>().SendEvent(EventId.UINotEnoughSoftCurrencyBuy, base.ModalResultCookie);
				modalResult = PayMeScreen.MODALRESULT_CONFIRMED;
			}
			else if (ContractUtils.InstantFreeupDroid())
			{
				Service.Get<EventManager>().SendEvent(EventId.UINotEnoughDroidSpeedUp, base.ModalResultCookie);
				modalResult = PayMeScreen.MODALRESULT_CONFIRMED;
			}
			this.Close(modalResult);
		}

		public void OnAlternatePayButtonClicked(UXButton button)
		{
			this.closing = true;
			object modalResult = null;
			button.Enabled = false;
			if (!this.forDroids)
			{
				Service.Get<EventManager>().SendEvent(EventId.UINotEnoughSoftCurrencyBuy, base.ModalResultCookie);
				modalResult = PayMeScreen.MODALRESULT_CONFIRMED_ALTERNATE;
			}
			else if (GameUtils.BuyNextDroid(true))
			{
				Service.Get<EventManager>().SendEvent(EventId.UINotEnoughDroidBuy, base.ModalResultCookie);
				modalResult = PayMeScreen.MODALRESULT_CONFIRMED_ALTERNATE;
			}
			this.Close(modalResult);
		}

		public override void Close(object modalResult)
		{
			base.Close(modalResult);
			if (modalResult != null)
			{
				return;
			}
			if (this.forDroids)
			{
				Service.Get<EventManager>().SendEvent(EventId.UINotEnoughDroidClose, base.ModalResultCookie);
				return;
			}
			Service.Get<EventManager>().SendEvent(EventId.UINotEnoughSoftCurrencyClose, base.ModalResultCookie);
		}

		protected internal PayMeScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PayMeScreen)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PayMeScreen)GCHandledObjects.GCHandleToObject(instance)).OnAlternatePayButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PayMeScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PayMeScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((PayMeScreen)GCHandledObjects.GCHandleToObject(instance)).OnPayButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((PayMeScreen)GCHandledObjects.GCHandleToObject(instance)).SetupControls();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PayMeScreen.ShowIfNoFreeDroids((OnScreenModalResult)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PayMeScreen.ShowIfNotEnoughCurrency(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[4])));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			PayMeScreen.ShowModal(*(int*)args, *(sbyte*)(args + 1) != 0, Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)), (OnScreenModalResult)GCHandledObjects.GCHandleToObject(args[5]), GCHandledObjects.GCHandleToObject(args[6]));
			return -1L;
		}
	}
}
