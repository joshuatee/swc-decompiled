using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Runtime.CompilerServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers
{
	public class FactionSelectorDropDown
	{
		private const string FILTER_OPTIONS = "FilterOptions";

		private const string BTN_FILTER = "BtnFilter";

		private const string BTN_FILTER_LABEL = "LabelBtnFilter";

		private const string BTN_FILTER_SPRITE = "SpriteBtnFilterSymbol";

		private const string BTN_EMPIRE = "BtnEmpire";

		private const string BTN_EMPIRE_LABEL = "LabelBtnEmpire";

		private const string BTN_EMPIRE_SPRITE = "SpriteBtnEmpireSymbol";

		private const string BTN_REBEL = "BtnRebel";

		private const string BTN_REBEL_LABEL = "LabelBtnRebel";

		private const string BTN_REBEL_SPRITE = "SpriteBtnRebelSymbol";

		private const string BTN_BOTH = "BtnBoth";

		private const string BTN_BOTH_LABEL = "LabelBtnBoth";

		private const string BTN_BOTH_SPRITE = "SpriteBtnBothSymbol";

		private const string EMPIRE = "s_Empire";

		private const string REBEL = "s_Rebel";

		private const string BOTH = "s_Both";

		protected UXFactory uxFactory;

		protected UXElement filterOptions;

		protected UXCheckbox empireBtn;

		protected UXLabel empireBtnLabel;

		protected UXCheckbox rebelBtn;

		protected UXLabel rebelBtnLabel;

		protected UXCheckbox bothBtn;

		protected UXLabel bothBtnLabel;

		protected UXButton filterBtn;

		protected UXLabel filterBtnLabel;

		protected UXSprite filterSprite;

		protected FactionToggle selectedFaction;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action<FactionToggle> FactionSelectCallBack;

		public FactionSelectorDropDown(UXFactory uxFactory)
		{
			this.uxFactory = uxFactory;
		}

		public void Init(FactionToggle factionToggle)
		{
			this.filterOptions = this.uxFactory.GetElement<UXElement>("FilterOptions");
			this.filterBtn = this.uxFactory.GetElement<UXButton>("BtnFilter");
			this.filterBtnLabel = this.uxFactory.GetElement<UXLabel>("LabelBtnFilter");
			this.filterSprite = this.uxFactory.GetElement<UXSprite>("SpriteBtnFilterSymbol");
			this.empireBtn = this.uxFactory.GetElement<UXCheckbox>("BtnEmpire");
			this.empireBtnLabel = this.uxFactory.GetElement<UXLabel>("LabelBtnEmpire");
			this.empireBtn.Selected = false;
			this.rebelBtn = this.uxFactory.GetElement<UXCheckbox>("BtnRebel");
			this.rebelBtnLabel = this.uxFactory.GetElement<UXLabel>("LabelBtnRebel");
			this.rebelBtn.Selected = false;
			this.bothBtn = this.uxFactory.GetElement<UXCheckbox>("BtnBoth");
			this.bothBtnLabel = this.uxFactory.GetElement<UXLabel>("LabelBtnBoth");
			this.bothBtn.Selected = true;
			Lang lang = Service.Get<Lang>();
			this.empireBtnLabel.Text = lang.Get("s_Empire", new object[0]);
			this.rebelBtnLabel.Text = lang.Get("s_Rebel", new object[0]);
			this.bothBtnLabel.Text = lang.Get("s_Both", new object[0]);
			this.filterBtn.OnClicked = new UXButtonClickedDelegate(this.FilterButtonClicked);
			this.empireBtn.OnSelected = new UXCheckboxSelectedDelegate(this.EmpireTabClicked);
			this.rebelBtn.OnSelected = new UXCheckboxSelectedDelegate(this.RebelTabClicked);
			this.bothBtn.OnSelected = new UXCheckboxSelectedDelegate(this.BothTabClicked);
			this.SelectFaction(factionToggle, false);
		}

		protected void EmpireTabClicked(UXCheckbox box, bool selected)
		{
			if (selected)
			{
				this.SelectFaction(FactionToggle.Empire, true);
			}
		}

		protected void RebelTabClicked(UXCheckbox box, bool selected)
		{
			if (selected)
			{
				this.SelectFaction(FactionToggle.Rebel, true);
			}
		}

		protected void BothTabClicked(UXCheckbox box, bool selected)
		{
			if (selected)
			{
				this.SelectFaction(FactionToggle.All, true);
			}
		}

		protected void ShowHideFilterOptions(bool isShow)
		{
			this.filterOptions.Visible = isShow;
		}

		protected void FilterButtonClicked(UXButton button)
		{
			this.ShowHideFilterOptions(!this.filterOptions.Visible);
		}

		public void SelectFaction(FactionToggle faction)
		{
			this.SelectFaction(faction, true);
		}

		private void SelectFaction(FactionToggle faction, bool doCallBack)
		{
			this.selectedFaction = faction;
			UXLabel uXLabel;
			UXSprite element;
			switch (faction)
			{
			case FactionToggle.Empire:
				uXLabel = this.empireBtnLabel;
				element = this.uxFactory.GetElement<UXSprite>("SpriteBtnEmpireSymbol");
				goto IL_67;
			case FactionToggle.Rebel:
				uXLabel = this.rebelBtnLabel;
				element = this.uxFactory.GetElement<UXSprite>("SpriteBtnRebelSymbol");
				goto IL_67;
			}
			uXLabel = this.bothBtnLabel;
			element = this.uxFactory.GetElement<UXSprite>("SpriteBtnBothSymbol");
			IL_67:
			this.filterBtnLabel.Text = uXLabel.Text;
			this.filterSprite.SpriteName = element.SpriteName;
			this.filterSprite.Color = element.Color;
			this.ShowHideFilterOptions(false);
			if (doCallBack && this.FactionSelectCallBack != null)
			{
				this.FactionSelectCallBack.Invoke(faction);
			}
		}

		public FactionToggle GetSelectedFaction()
		{
			return this.selectedFaction;
		}

		protected internal FactionSelectorDropDown(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((FactionSelectorDropDown)GCHandledObjects.GCHandleToObject(instance)).FactionSelectCallBack += (Action<FactionToggle>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((FactionSelectorDropDown)GCHandledObjects.GCHandleToObject(instance)).BothTabClicked((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((FactionSelectorDropDown)GCHandledObjects.GCHandleToObject(instance)).EmpireTabClicked((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((FactionSelectorDropDown)GCHandledObjects.GCHandleToObject(instance)).FilterButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionSelectorDropDown)GCHandledObjects.GCHandleToObject(instance)).GetSelectedFaction());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((FactionSelectorDropDown)GCHandledObjects.GCHandleToObject(instance)).Init((FactionToggle)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((FactionSelectorDropDown)GCHandledObjects.GCHandleToObject(instance)).RebelTabClicked((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((FactionSelectorDropDown)GCHandledObjects.GCHandleToObject(instance)).FactionSelectCallBack -= (Action<FactionToggle>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((FactionSelectorDropDown)GCHandledObjects.GCHandleToObject(instance)).SelectFaction((FactionToggle)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((FactionSelectorDropDown)GCHandledObjects.GCHandleToObject(instance)).SelectFaction((FactionToggle)(*(int*)args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((FactionSelectorDropDown)GCHandledObjects.GCHandleToObject(instance)).ShowHideFilterOptions(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
