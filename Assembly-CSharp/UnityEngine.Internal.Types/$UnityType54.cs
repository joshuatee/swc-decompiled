using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType54 : $UnityType
	{
		public unsafe $UnityType54()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 354644) = ldftn($Invoke0);
			*(data + 354672) = ldftn($Invoke1);
			*(data + 354700) = ldftn($Invoke2);
			*(data + 354728) = ldftn($Invoke3);
			*(data + 354756) = ldftn($Invoke4);
			*(data + 354784) = ldftn($Invoke5);
			*(data + 354812) = ldftn($Invoke6);
			*(data + 354840) = ldftn($Invoke7);
			*(data + 354868) = ldftn($Invoke8);
			*(data + 354896) = ldftn($Invoke9);
			*(data + 354924) = ldftn($Invoke10);
			*(data + 354952) = ldftn($Invoke11);
			*(data + 354980) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new NGUIDebug((UIntPtr)0);
		}
	}
}
