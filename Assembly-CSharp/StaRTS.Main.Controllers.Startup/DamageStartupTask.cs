using StaRTS.Main.Views;
using StaRTS.Main.Views.World;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class DamageStartupTask : StartupTask
	{
		public DamageStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			new DerivedTransformationManager();
			new ProjectileController();
			new ProjectileViewManager();
			new HealthController();
			base.Complete();
		}

		protected internal DamageStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DamageStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
