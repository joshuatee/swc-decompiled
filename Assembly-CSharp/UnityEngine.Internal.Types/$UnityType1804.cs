using StaRTS.Main.Models.Perks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1804 : $UnityType
	{
		public unsafe $UnityType1804()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 686388) = ldftn($Invoke0);
			*(data + 686416) = ldftn($Invoke1);
			*(data + 686444) = ldftn($Invoke2);
			*(data + 686472) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ActivatedPerkData((UIntPtr)0);
		}
	}
}
