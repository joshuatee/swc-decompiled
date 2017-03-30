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
using System.Collections.Generic;

namespace StaRTS.Main.Views.UX.Screens
{
	public class MultiResourcePayMeScreen : AlertScreen
	{
		private int crystals;

		private List<string> multiItemSpriteNames;

		private List<string> multiItemLabelTexts;

		private MultiResourcePayMeScreen(int crystals, string title, string message, List<string> spriteNames, List<string> labelTexts) : base(false, title, message, string.Empty, false)
		{
			this.crystals = crystals;
			this.multiItemSpriteNames = spriteNames;
			this.multiItemLabelTexts = labelTexts;
		}

		public static bool ShowIfNotEnoughMultipleCurrencies(string[] cost, string purchaseContext, OnScreenModalResult onModalResult)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			int num;
			int num2;
			int num3;
			int num4;
			GameUtils.GetHQScaledCurrency(cost, out num, out num2, out num3, out num4);
			Dictionary<CurrencyType, int> dictionary = new Dictionary<CurrencyType, int>();
			int num5 = num - currentPlayer.CurrentCreditsAmount;
			int num6 = num2 - currentPlayer.CurrentMaterialsAmount;
			int num7 = num3 - currentPlayer.CurrentContrabandAmount;
			if (num5 > 0)
			{
				dictionary.Add(CurrencyType.Credits, num5);
			}
			else
			{
				num5 = 0;
			}
			if (num6 > 0)
			{
				dictionary.Add(CurrencyType.Materials, num6);
			}
			else
			{
				num6 = 0;
			}
			if (num7 > 0)
			{
				dictionary.Add(CurrencyType.Contraband, num7);
			}
			else
			{
				num7 = 0;
			}
			if (dictionary.Count > 0)
			{
				Lang lang = Service.Get<Lang>();
				bool flag = true;
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				foreach (KeyValuePair<CurrencyType, int> current in dictionary)
				{
					CurrencyType key = current.Key;
					int value = current.Value;
					flag &= GameUtils.HasEnoughCurrencyStorage(key, value);
					list.Add(UXUtils.GetCurrencyItemAssetName(key.ToString()));
					string currencyStringId = LangUtils.GetCurrencyStringId(key);
					string str = lang.Get(currencyStringId, new object[0]);
					list2.Add(lang.ThousandsSeparated(value) + " " + str);
				}
				if (flag)
				{
					string title = lang.Get("NEED_MORE_MULTI", new object[0]);
					string message = lang.Get("NEED_MORE_MULTI_BUY_MISSING", new object[0]);
					int num8 = GameUtils.MultiCurrencyCrystalCost(dictionary);
					MultiCurrencyTag modalResultCookie = new MultiCurrencyTag(num5, num6, num7, num8, purchaseContext);
					MultiResourcePayMeScreen multiResourcePayMeScreen = new MultiResourcePayMeScreen(num8, title, message, list, list2);
					multiResourcePayMeScreen.OnModalResult = onModalResult;
					multiResourcePayMeScreen.ModalResultCookie = modalResultCookie;
					Service.Get<ScreenController>().AddScreen(multiResourcePayMeScreen);
				}
				else
				{
					onModalResult(null, null);
				}
				return true;
			}
			dictionary.Clear();
			dictionary = null;
			return false;
		}

		protected override void SetupControls()
		{
			base.GetElement<UXLabel>("TickerDialogSmall").Visible = false;
			this.titleLabel.Text = this.title;
			this.payButton.Visible = true;
			this.payButton.OnClicked = new UXButtonClickedDelegate(this.OnPayButtonClicked);
			UXUtils.SetupCostElements(this, "Cost", null, 0, 0, 0, this.crystals, false, null);
			if (this.multiItemSpriteNames != null && this.multiItemLabelTexts != null)
			{
				this.groupMultipleItems.Visible = true;
				this.centerLabel.Visible = false;
				this.rightLabel.Visible = false;
				this.multiItemMessageLabel.Text = this.message;
				this.sprite.Visible = false;
				this.textureImageInset.Enabled = false;
				if (this.multiItemTable != null && this.multiItemSpriteNames.Count == this.multiItemLabelTexts.Count)
				{
					this.multiItemTable.Clear();
					int i = 0;
					int count = this.multiItemSpriteNames.Count;
					while (i < count)
					{
						string itemUid = i.ToString();
						UXElement item = this.multiItemTable.CloneTemplateItem(itemUid);
						UXSprite subElement = this.multiItemTable.GetSubElement<UXSprite>(itemUid, "SpriteImageAndTextMultiple");
						UXLabel subElement2 = this.multiItemTable.GetSubElement<UXLabel>(itemUid, "LabelItemImageAndTextMultiple");
						UXUtils.SetupGeometryForIcon(subElement, this.multiItemSpriteNames[i]);
						subElement2.Text = this.multiItemLabelTexts[i];
						this.multiItemTable.AddItem(item, i);
						i++;
					}
				}
			}
		}

		private void OnPayButtonClicked(UXButton button)
		{
			button.Enabled = false;
			Service.Get<EventManager>().SendEvent(EventId.UINotEnoughSoftCurrencyBuy, base.ModalResultCookie);
			this.Close(true);
		}

		public override void Close(object modalResult)
		{
			base.Close(modalResult);
			if (modalResult != null)
			{
				return;
			}
			Service.Get<EventManager>().SendEvent(EventId.UINotEnoughSoftCurrencyClose, base.ModalResultCookie);
		}
	}
}
