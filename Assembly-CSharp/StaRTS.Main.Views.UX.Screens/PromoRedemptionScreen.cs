using StaRTS.Externals.IAP;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class PromoRedemptionScreen : AlertScreen
	{
		private string iapUID;

		public PromoRedemptionScreen(string uid, int amount) : base(false, null, null, null, false)
		{
			this.iapUID = uid;
			this.title = this.lang.Get("promo_redeem_title", new object[0]);
			InAppPurchaseTypeVO inAppPurchaseTypeVO = Service.Get<IDataController>().Get<InAppPurchaseTypeVO>(uid);
			string id = inAppPurchaseTypeVO.RedemptionStringEmpire;
			if (Service.Get<CurrentPlayer>().Faction == FactionType.Rebel)
			{
				id = inAppPurchaseTypeVO.RedemptionStringRebel;
			}
			this.message = this.lang.Get(id, new object[]
			{
				amount
			});
			base.AllowFUEBackButton = true;
		}

		protected override void OnScreenLoaded()
		{
			base.OnScreenLoaded();
			this.sprite.Visible = true;
			Service.Get<InAppPurchaseController>().SetIAPRewardIcon(this.sprite, this.iapUID);
		}

		protected override void SetupControls()
		{
			base.SetupControls();
			this.centerLabel.Visible = false;
			this.rightLabel.Visible = true;
			this.rightLabel.Text = this.message;
			this.primaryButton.OnClicked = new UXButtonClickedDelegate(this.OnButtonClicked);
			this.primaryLabel.Text = this.lang.Get("OK", new object[0]);
			base.InitDefaultBackDelegate();
			Service.Get<UserInputInhibitor>().AlwaysAllowElement(this.primaryButton);
			Service.Get<UserInputInhibitor>().AlwaysAllowElement(this.CloseButton);
		}

		private void OnButtonClicked(UXButton button)
		{
			this.Close(base.OnModalResult);
		}

		public override void OnDestroyElement()
		{
			base.OnDestroyElement();
			this.Visible = false;
		}

		protected internal PromoRedemptionScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PromoRedemptionScreen)GCHandledObjects.GCHandleToObject(instance)).OnButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PromoRedemptionScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PromoRedemptionScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PromoRedemptionScreen)GCHandledObjects.GCHandleToObject(instance)).SetupControls();
			return -1L;
		}
	}
}
