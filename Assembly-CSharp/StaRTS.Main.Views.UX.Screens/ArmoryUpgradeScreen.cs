using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class ArmoryUpgradeScreen : BuildingInfoScreen
	{
		private const string EQUIPMENT_INFO_CAPACITY = "EQUIPMENT_INFO_CAPACITY";

		private const int EQUIPMENT_HITPOINTS_INDEX = 0;

		private const int EQUIPMENT_CAPACITY_INDEX = 1;

		private const int EQUIPMENT_SLIDER_COUNT = 2;

		public ArmoryUpgradeScreen(Entity armory, bool upgradeGroup) : base(armory, upgradeGroup)
		{
		}

		protected override void OnLoaded()
		{
			base.InitControls(2);
			this.InitHitpoints(0);
			base.InitStorage(1, "EQUIPMENT_INFO_CAPACITY");
		}

		protected internal ArmoryUpgradeScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ArmoryUpgradeScreen)GCHandledObjects.GCHandleToObject(instance)).OnLoaded();
			return -1L;
		}
	}
}
