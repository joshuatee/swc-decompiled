using StaRTS.Main.Models.Squads.War;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1876 : $UnityType
	{
		public unsafe $UnityType1876()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 707220) = ldftn($Invoke0);
			*(data + 707248) = ldftn($Invoke1);
			*(data + 707276) = ldftn($Invoke2);
			*(data + 707304) = ldftn($Invoke3);
			*(data + 707332) = ldftn($Invoke4);
			*(data + 707360) = ldftn($Invoke5);
			*(data + 707388) = ldftn($Invoke6);
			*(data + 707416) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new SquadWarRewardData((UIntPtr)0);
		}
	}
}
