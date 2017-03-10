using StaRTS.Main.Models.Perks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1807 : $UnityType
	{
		public unsafe $UnityType1807()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 686808) = ldftn($Invoke0);
			*(data + 686836) = ldftn($Invoke1);
			*(data + 686864) = ldftn($Invoke2);
			*(data + 686892) = ldftn($Invoke3);
			*(data + 686920) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new PerksInfo((UIntPtr)0);
		}
	}
}
