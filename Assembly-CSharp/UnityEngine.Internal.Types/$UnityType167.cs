using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType167 : $UnityType
	{
		public unsafe $UnityType167()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 391380) = ldftn($Invoke0);
			*(data + 391408) = ldftn($Invoke1);
			*(data + 391436) = ldftn($Invoke2);
			*(data + 391464) = ldftn($Invoke3);
			*(data + 391492) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new UIInputOnGUI((UIntPtr)0);
		}
	}
}
