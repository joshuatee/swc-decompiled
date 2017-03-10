using StaRTS.Main.Models.Static;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class GameDataStartupTask : StartupTask
	{
		public GameDataStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			Service.Get<EventManager>().SendEvent(EventId.InitializeGameDataStart, null);
			new TroopUpgradeCatalog();
			new StarshipUpgradeCatalog();
			new BuildingUpgradeCatalog();
			new EquipmentUpgradeCatalog();
			new CombatEncounterController();
			new HoloController();
			base.Complete();
			Service.Get<EventManager>().SendEvent(EventId.InitializeGameDataEnd, null);
		}

		protected internal GameDataStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((GameDataStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
