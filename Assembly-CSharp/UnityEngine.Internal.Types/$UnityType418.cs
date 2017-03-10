using StaRTS.Externals.IAP;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType418 : $UnityType
	{
		public unsafe $UnityType418()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 445168) = ldftn($Invoke0);
			*(data + 445196) = ldftn($Invoke1);
			*(data + 445224) = ldftn($Invoke2);
			*(data + 445252) = ldftn($Invoke3);
			*(data + 445280) = ldftn($Invoke4);
			*(data + 445308) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DefaultIAPManager((UIntPtr)0);
		}
	}
}
