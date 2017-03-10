using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1058 : $UnityType
	{
		public unsafe $UnityType1058()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 626356) = ldftn($Invoke0);
			*(data + 626384) = ldftn($Invoke1);
			*(data + 626412) = ldftn($Invoke2);
			*(data + 626440) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new RewardabilityResult((UIntPtr)0);
		}
	}
}
