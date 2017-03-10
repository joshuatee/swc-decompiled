using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType19 : $UnityType
	{
		public unsafe $UnityType19()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 349884) = ldftn($Invoke0);
			*(data + 349912) = ldftn($Invoke1);
			*(data + 349940) = ldftn($Invoke2);
			*(data + 349968) = ldftn($Invoke3);
			*(data + 349996) = ldftn($Invoke4);
			*(data + 350024) = ldftn($Invoke5);
			*(data + 350052) = ldftn($Invoke6);
			*(data + 350080) = ldftn($Invoke7);
			*(data + 350108) = ldftn($Invoke8);
			*(data + 350136) = ldftn($Invoke9);
			*(data + 350164) = ldftn($Invoke10);
			*(data + 350192) = ldftn($Invoke11);
			*(data + 350220) = ldftn($Invoke12);
			*(data + 350248) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new BMSymbol((UIntPtr)0);
		}
	}
}
