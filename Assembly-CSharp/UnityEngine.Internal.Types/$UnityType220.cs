using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType220 : $UnityType
	{
		public unsafe $UnityType220()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 410924) = ldftn($Invoke0);
			*(data + 410952) = ldftn($Invoke1);
			*(data + 410980) = ldftn($Invoke2);
			*(data + 411008) = ldftn($Invoke3);
			*(data + 411036) = ldftn($Invoke4);
			*(data + 411064) = ldftn($Invoke5);
			*(data + 411092) = ldftn($Invoke6);
			*(data + 1528760) = ldftn($Get0);
			*(data + 1528764) = ldftn($Set0);
			*(data + 1528776) = ldftn($Get1);
			*(data + 1528780) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new UIToggledComponents((UIntPtr)0);
		}
	}
}
