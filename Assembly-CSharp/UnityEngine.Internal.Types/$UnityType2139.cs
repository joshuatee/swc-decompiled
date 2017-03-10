using StaRTS.Main.Views;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2139 : $UnityType
	{
		public unsafe $UnityType2139()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 859456) = ldftn($Invoke0);
			*(data + 859484) = ldftn($Invoke1);
			*(data + 859512) = ldftn($Invoke2);
			*(data + 859540) = ldftn($Invoke3);
			*(data + 859568) = ldftn($Invoke4);
			*(data + 859596) = ldftn($Invoke5);
			*(data + 859624) = ldftn($Invoke6);
			*(data + 859652) = ldftn($Invoke7);
			*(data + 859680) = ldftn($Invoke8);
			*(data + 859708) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new DerivedTransformationObject((UIntPtr)0);
		}
	}
}
