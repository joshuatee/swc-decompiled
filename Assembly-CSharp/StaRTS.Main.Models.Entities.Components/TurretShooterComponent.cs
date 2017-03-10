using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class TurretShooterComponent : ComponentBase
	{
		public int TargetWeight
		{
			get;
			set;
		}

		public TurretShooterComponent()
		{
			this.TargetWeight = 0;
		}

		protected internal TurretShooterComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretShooterComponent)GCHandledObjects.GCHandleToObject(instance)).TargetWeight);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TurretShooterComponent)GCHandledObjects.GCHandleToObject(instance)).TargetWeight = *(int*)args;
			return -1L;
		}
	}
}
