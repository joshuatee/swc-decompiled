using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1744 : $UnityType
	{
		public unsafe $UnityType1744()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 676420) = ldftn($Invoke0);
			*(data + 676448) = ldftn($Invoke1);
			*(data + 676476) = ldftn($Invoke2);
			*(data + 676504) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new TroopShieldViewComponent((UIntPtr)0);
		}
	}
}
