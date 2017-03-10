using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType14 : $UnityType
	{
		public unsafe $UnityType14()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 348512) = ldftn($Invoke0);
			*(data + 348540) = ldftn($Invoke1);
			*(data + 348568) = ldftn($Invoke2);
			*(data + 348596) = ldftn($Invoke3);
			*(data + 348624) = ldftn($Invoke4);
			*(data + 348652) = ldftn($Invoke5);
			*(data + 348680) = ldftn($Invoke6);
			*(data + 1523912) = ldftn($Get0);
			*(data + 1523916) = ldftn($Set0);
		}

		public override object CreateInstance()
		{
			return new AnimatedColor((UIntPtr)0);
		}
	}
}
