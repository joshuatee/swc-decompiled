using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType77 : $UnityType
	{
		public unsafe $UnityType77()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 361504) = ldftn($Invoke0);
			*(data + 361532) = ldftn($Invoke1);
			*(data + 361560) = ldftn($Invoke2);
			*(data + 361588) = ldftn($Invoke3);
			*(data + 361616) = ldftn($Invoke4);
			*(data + 361644) = ldftn($Invoke5);
			*(data + 361672) = ldftn($Invoke6);
			*(data + 361700) = ldftn($Invoke7);
			*(data + 361728) = ldftn($Invoke8);
			*(data + 361756) = ldftn($Invoke9);
			*(data + 361784) = ldftn($Invoke10);
			*(data + 361812) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new Splash((UIntPtr)0);
		}
	}
}
