using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType631 : $UnityType
	{
		public unsafe $UnityType631()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 509904) = ldftn($Invoke0);
			*(data + 509932) = ldftn($Invoke1);
			*(data + 509960) = ldftn($Invoke2);
			*(data + 509988) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ImpactCookie((UIntPtr)0);
		}
	}
}
