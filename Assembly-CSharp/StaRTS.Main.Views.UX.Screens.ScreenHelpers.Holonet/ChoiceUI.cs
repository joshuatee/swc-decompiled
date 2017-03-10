using StaRTS.Main.Views.UX.Elements;
using System;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet
{
	public struct ChoiceUI
	{
		public UXButton Button;

		public string DisplayValue;

		public int Id;

		public ChoiceUI(UXButton newButton, string displayValue, int newId)
		{
			this.Button = newButton;
			this.DisplayValue = displayValue;
			this.Id = newId;
		}
	}
}
