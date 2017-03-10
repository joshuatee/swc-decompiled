using System;

namespace StaRTS.Main.Views.Animations
{
	public enum AnimState
	{
		Idle,
		Walk,
		Die,
		Shoot,
		ShootWarmup,
		AbilityWarmup1,
		AbilityShoot,
		AbilityWarmup2,
		Repair,
		Run,
		Disable = 11,
		AbilityPose = 95,
		Closed,
		Open,
		Unlocked
	}
}
