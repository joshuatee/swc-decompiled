using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers.Planets
{
	public class PlanetDetailsRelocateViewModule : AbstractPlanetDetailsViewModule
	{
		public const string RELOCATE_BUTTON = "BtnRelocate";

		public const string RELOCATE_BUTTON_LABEL = "LabelBtnRelocate";

		public const string LOCKED_INFO_GROUP = "LockedInfo";

		public const string LOCKED_INFO_LABEL = "LabelLockedInfo";

		public const string RELOCATE_BUTTON_STRING = "Planets_Relocate_Button";

		public const string PLANET_LOCKED_STRING = "Planets_Relocate_Locked";

		private UXButton relocateButton;

		private UXElement lockedGroup;

		private UXLabel lockedLabel;

		private UXLabel relocateButtonLabel;

		public PlanetDetailsRelocateViewModule(PlanetDetailsScreen screen) : base(screen)
		{
		}

		public void OnScreenLoaded()
		{
			this.relocateButton = this.screen.GetElement<UXButton>("BtnRelocate");
			this.relocateButtonLabel = this.screen.GetElement<UXLabel>("LabelBtnRelocate");
			this.relocateButtonLabel.Text = base.LangController.Get("Planets_Relocate_Button", new object[0]);
			this.relocateButton.OnClicked = new UXButtonClickedDelegate(this.OnRelocateButton);
			this.lockedGroup = this.screen.GetElement<UXElement>("LockedInfo");
			this.lockedLabel = this.screen.GetElement<UXLabel>("LabelLockedInfo");
			this.lockedLabel.Text = base.LangController.Get("Planets_Relocate_Locked", new object[0]);
			this.RefreshScreenForPlanetChange();
		}

		public void RefreshScreenForPlanetChange()
		{
			if (!base.Player.IsPlanetUnlocked(this.screen.viewingPlanetVO.Uid))
			{
				this.lockedGroup.Visible = true;
				this.relocateButtonLabel.TextColor = UXUtils.COLOR_LABEL_DISABLED;
				this.relocateButton.Visible = false;
				this.relocateButton.Enabled = false;
				return;
			}
			this.lockedGroup.Visible = false;
			if (GameUtils.IsPlanetCurrentOne(this.screen.viewingPlanetVO.Uid))
			{
				this.relocateButton.Visible = false;
				this.relocateButton.Enabled = false;
				return;
			}
			this.relocateButton.Visible = true;
			this.relocateButton.Enabled = true;
			if (Service.Get<CurrentPlayer>().IsRelocationRequirementMet())
			{
				this.relocateButtonLabel.TextColor = UXUtils.COLOR_ENABLED;
				return;
			}
			this.relocateButtonLabel.TextColor = UXUtils.COLOR_ENABLED;
		}

		public void OnRelocateButton(UXButton button)
		{
			ConfirmRelocateScreen.ShowModal(this.screen.viewingPlanetVO, null, null);
		}

		protected internal PlanetDetailsRelocateViewModule(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PlanetDetailsRelocateViewModule)GCHandledObjects.GCHandleToObject(instance)).OnRelocateButton((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlanetDetailsRelocateViewModule)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PlanetDetailsRelocateViewModule)GCHandledObjects.GCHandleToObject(instance)).RefreshScreenForPlanetChange();
			return -1L;
		}
	}
}
