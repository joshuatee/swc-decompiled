using StaRTS.Main.Models.Perks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1806 : $UnityType
	{
		public unsafe $UnityType1806()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 686640) = ldftn($Invoke0);
			*(data + 686668) = ldftn($Invoke1);
			*(data + 686696) = ldftn($Invoke2);
			*(data + 686724) = ldftn($Invoke3);
			*(data + 686752) = ldftn($Invoke4);
			*(data + 686780) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new PerksData((UIntPtr)0);
		}
	}
}
