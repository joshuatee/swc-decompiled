using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Squads
{
	public abstract class AbstractSquadRequestScreen : ClosableScreen
	{
		private const string BUTTON = "BtnSend";

		private const string BUTTON_LABEL = "LabelBtnSend";

		private const string COST_BUTTON = "BtnFinishCost";

		private const string INPUT = "LabelInputNameSquad";

		private const string INSTRUCTIONS_LABEL = "LabelTroopIncrement";

		protected const string WAR_GROUP = "InputFieldRequestMessageWar";

		protected const string MAIN_GROUP = "InputFieldRequestMessage";

		private const string REQUEST_LABEL = "LabelRequestTroops";

		private const string CURRENT_TROOPS_PANEL = "PanelCurrentTroops";

		private const string CURRENT_TROOPS_LABEL = "LabelCurrentTroops";

		protected const string GRID_CURRENT_TROOPS = "GridCurrentTroops";

		private const string TITLE_GROUP = "TitleGroup";

		private const string PERK_TITLE_GROUP = "TitleGroupPerks";

		protected const string BUILDING_PERKS_BUTTON = "btnPerksSquadRequest";

		protected const string REQUEST_PERKS_LABEL = "LabelRequestTroopsPerks";

		protected const string INSTRUCTIONS_PERKS_LABEL = "LabelTroopIncrementPerks";

		private const string INVALID_TEXT = "INVALID_TEXT";

		private const string SEND = "BUTTON_SEND";

		protected UXLabel instructionsLabel;

		protected UXInput input;

		protected UXButton button;

		protected UXLabel buttonLabel;

		protected UXButton costButton;

		protected UXLabel requestLabel;

		protected UXElement troopsPanel;

		protected UXLabel troopsLabel;

		protected UXLabel requestPerksLabel;

		protected UXLabel instructionsPerksLabel;

		public AbstractSquadRequestScreen() : base("gui_squad_request")
		{
		}

		protected override void OnScreenLoaded()
		{
			base.GetElement<UXElement>("InputFieldRequestMessageWar").Visible = false;
			base.GetElement<UXElement>("TitleGroupPerks").Visible = false;
			this.InitButtons();
			this.buttonLabel = base.GetElement<UXLabel>("LabelBtnSend");
			this.buttonLabel.Text = this.lang.Get("BUTTON_SEND", new object[0]);
			this.requestLabel = base.GetElement<UXLabel>("LabelRequestTroops");
			this.troopsPanel = base.GetElement<UXElement>("PanelCurrentTroops");
			this.troopsLabel = base.GetElement<UXLabel>("LabelCurrentTroops");
			this.requestLabel.Visible = false;
			this.troopsPanel.Visible = false;
			this.troopsLabel.Visible = false;
			this.instructionsLabel = base.GetElement<UXLabel>("LabelTroopIncrement");
			this.requestPerksLabel = base.GetElement<UXLabel>("LabelRequestTroopsPerks");
			this.instructionsPerksLabel = base.GetElement<UXLabel>("LabelTroopIncrementPerks");
			this.input = base.GetElement<UXInput>("LabelInputNameSquad");
			UIInput uIInputComponent = this.input.GetUIInputComponent();
			uIInputComponent.onValidate = new UIInput.OnValidate(LangUtils.OnValidateWNewLines);
		}

		protected override void InitButtons()
		{
			base.InitButtons();
			this.button = base.GetElement<UXButton>("BtnSend");
			this.button.OnClicked = new UXButtonClickedDelegate(this.OnClicked);
			this.costButton = base.GetElement<UXButton>("BtnFinishCost");
			this.costButton.OnClicked = new UXButtonClickedDelegate(this.OnClicked);
			this.costButton.Visible = false;
		}

		protected virtual void SetupPerksButton(BuildingTypeVO buildingInfo)
		{
			UXElement element = base.GetElement<UXElement>("TitleGroup");
			element.Visible = true;
			UXElement element2 = base.GetElement<UXElement>("TitleGroupPerks");
			element2.Visible = false;
			UXButton element3 = base.GetElement<UXButton>("btnPerksSquadRequest");
			element3.Tag = buildingInfo;
			element3.OnClicked = new UXButtonClickedDelegate(this.OnPerksButtonClicked);
			if (Service.Get<PerkManager>().IsPerkAppliedToBuilding(buildingInfo))
			{
				element2.Visible = true;
				element.Visible = false;
			}
		}

		protected void OnPerksButtonClicked(UXButton button)
		{
			Service.Get<PerkViewController>().ShowActivePerksScreen((BuildingTypeVO)button.Tag);
		}

		protected bool CheckForValidInput()
		{
			if (!Service.Get<ProfanityController>().IsValid(this.input.Text, false))
			{
				AlertScreen.ShowModal(false, null, this.lang.Get("INVALID_TEXT", new object[0]), null, null, true);
				return false;
			}
			return true;
		}

		protected abstract void OnClicked(UXButton button);

		protected internal AbstractSquadRequestScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractSquadRequestScreen)GCHandledObjects.GCHandleToObject(instance)).CheckForValidInput());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractSquadRequestScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AbstractSquadRequestScreen)GCHandledObjects.GCHandleToObject(instance)).OnClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractSquadRequestScreen)GCHandledObjects.GCHandleToObject(instance)).OnPerksButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AbstractSquadRequestScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AbstractSquadRequestScreen)GCHandledObjects.GCHandleToObject(instance)).SetupPerksButton((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
