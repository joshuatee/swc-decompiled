using Net.RichardLord.Ash.Core;
using System;

namespace StaRTS.Main.Views.UX.Screens
{
	public class ShieldGeneratorUpgradeScreen : ShieldGeneratorInfoScreen
	{
		public ShieldGeneratorUpgradeScreen(Entity selectedBuilding) : base(selectedBuilding)
		{
			this.useUpgradeGroup = true;
		}

		protected internal ShieldGeneratorUpgradeScreen(UIntPtr dummy) : base(dummy)
		{
		}
	}
}
