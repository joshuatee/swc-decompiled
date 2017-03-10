using StaRTS.Main.Views.UX.Elements;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet
{
	public struct ChoiceListUI
	{
		public UXLabel buttonLabel
		{
			get;
			set;
		}

		public UXElement filterOptions
		{
			get;
			set;
		}

		public UXGrid filterGrid
		{
			get;
			set;
		}

		public List<ChoiceUI> choices
		{
			get;
			set;
		}

		public int id
		{
			get;
			set;
		}
	}
}
