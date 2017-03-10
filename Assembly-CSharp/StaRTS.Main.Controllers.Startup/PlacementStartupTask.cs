using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class PlacementStartupTask : StartupTask
	{
		public PlacementStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			new DeployerController();
			new TroopController();
			new SpecialAttackController();
			new SquadTroopAttackController();
			base.Complete();
		}

		protected internal PlacementStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PlacementStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
