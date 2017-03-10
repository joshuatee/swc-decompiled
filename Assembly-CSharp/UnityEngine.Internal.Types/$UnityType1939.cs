using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1939 : $UnityType
	{
		public unsafe $UnityType1939()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 760420) = ldftn($Invoke0);
			*(data + 760448) = ldftn($Invoke1);
			*(data + 760476) = ldftn($Invoke2);
			*(data + 760504) = ldftn($Invoke3);
			*(data + 760532) = ldftn($Invoke4);
			*(data + 760560) = ldftn($Invoke5);
			*(data + 760588) = ldftn($Invoke6);
			*(data + 760616) = ldftn($Invoke7);
			*(data + 760644) = ldftn($Invoke8);
			*(data + 760672) = ldftn($Invoke9);
			*(data + 760700) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new MobilizationHologramVO((UIntPtr)0);
		}
	}
}
