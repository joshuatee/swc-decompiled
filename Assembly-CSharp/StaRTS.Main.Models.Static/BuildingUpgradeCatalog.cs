using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Static
{
	public class BuildingUpgradeCatalog : GenericUpgradeCatalog<BuildingTypeVO>
	{
		protected override void InitService()
		{
			Service.Set<BuildingUpgradeCatalog>(this);
		}

		public BuildingUpgradeCatalog()
		{
		}

		protected internal BuildingUpgradeCatalog(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BuildingUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).InitService();
			return -1L;
		}
	}
}
