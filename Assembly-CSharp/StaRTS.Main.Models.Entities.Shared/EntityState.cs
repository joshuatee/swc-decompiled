using System;

namespace StaRTS.Main.Models.Entities.Shared
{
	public enum EntityState
	{
		Disable,
		Idle,
		Moving,
		Tracking,
		Turning,
		WarmingUp,
		Attacking,
		AttackingReset,
		CoolingDown,
		Dying
	}
}
