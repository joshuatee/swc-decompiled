using StaRTS.Main.Models.Static;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1889 : $UnityType
	{
		public unsafe $UnityType1889()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 708060) = ldftn($Invoke0);
			*(data + 708088) = ldftn($Invoke1);
			*(data + 708116) = ldftn($Invoke2);
			*(data + 708144) = ldftn($Invoke3);
			*(data + 708172) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new TroopUpgradeCatalog((UIntPtr)0);
		}
	}
}
