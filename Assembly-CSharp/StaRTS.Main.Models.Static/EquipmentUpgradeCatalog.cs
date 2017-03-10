using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Static
{
	public class EquipmentUpgradeCatalog : GenericUpgradeCatalog<EquipmentVO>
	{
		protected override void InitService()
		{
			Service.Set<EquipmentUpgradeCatalog>(this);
		}

		public EquipmentUpgradeCatalog()
		{
		}

		protected internal EquipmentUpgradeCatalog(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EquipmentUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).InitService();
			return -1L;
		}
	}
}
