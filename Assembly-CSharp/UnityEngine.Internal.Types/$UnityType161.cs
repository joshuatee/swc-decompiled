using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType161 : $UnityType
	{
		public unsafe $UnityType161()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 389588) = ldftn($Invoke0);
			*(data + 389616) = ldftn($Invoke1);
			*(data + 389644) = ldftn($Invoke2);
			*(data + 389672) = ldftn($Invoke3);
			*(data + 389700) = ldftn($Invoke4);
			*(data + 389728) = ldftn($Invoke5);
			*(data + 389756) = ldftn($Invoke6);
			*(data + 389784) = ldftn($Invoke7);
			*(data + 389812) = ldftn($Invoke8);
			*(data + 389840) = ldftn($Invoke9);
			*(data + 389868) = ldftn($Invoke10);
			*(data + 389896) = ldftn($Invoke11);
			*(data + 389924) = ldftn($Invoke12);
			*(data + 1526472) = ldftn($Get0);
			*(data + 1526476) = ldftn($Set0);
			*(data + 1526488) = ldftn($Get1);
			*(data + 1526492) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new UIImageButton((UIntPtr)0);
		}
	}
}
