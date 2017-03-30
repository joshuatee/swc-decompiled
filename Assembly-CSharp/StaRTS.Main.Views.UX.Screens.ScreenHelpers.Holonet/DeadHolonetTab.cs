using StaRTS.Main.Controllers.Holonet;
using StaRTS.Main.Views.UX.Elements;
using System;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet
{
	public class DeadHolonetTab : AbstractHolonetTab
	{
		public DeadHolonetTab(HolonetScreen screen, HolonetControllerType type, string name) : base(screen, type)
		{
			this.topLevelGroup = screen.GetElement<UXElement>(name);
			this.topLevelGroup.Visible = false;
		}

		public override void OnTabOpen()
		{
		}

		public override void OnTabClose()
		{
		}

		protected override void AddSelectionButtonToNavTable()
		{
		}
	}
}
