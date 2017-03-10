using StaRTS.Assets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType359 : $UnityType
	{
		public unsafe $UnityType359()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 431532) = ldftn($Invoke0);
			*(data + 431560) = ldftn($Invoke1);
			*(data + 431588) = ldftn($Invoke2);
			*(data + 431616) = ldftn($Invoke3);
			*(data + 431644) = ldftn($Invoke4);
			*(data + 431672) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new MultiAssetInfo((UIntPtr)0);
		}
	}
}
