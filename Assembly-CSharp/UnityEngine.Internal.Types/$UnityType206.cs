using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType206 : $UnityType
	{
		public unsafe $UnityType206()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 406808) = ldftn($Invoke0);
			*(data + 406836) = ldftn($Invoke1);
			*(data + 406864) = ldftn($Invoke2);
			*(data + 406892) = ldftn($Invoke3);
			*(data + 406920) = ldftn($Invoke4);
			*(data + 406948) = ldftn($Invoke5);
			*(data + 406976) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new UISoundVolume((UIntPtr)0);
		}
	}
}
