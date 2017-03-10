using System;

namespace StaRTS.Main.Models.ValueObjects
{
	public interface ISplashVO
	{
		int SplashRadius
		{
			get;
			set;
		}

		int[] SplashDamagePercentages
		{
			get;
			set;
		}

		int GetSplashDamagePercent(int distFromImpact);
	}
}
