using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType101 : $UnityType
	{
		public unsafe $UnityType101()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 369400) = ldftn($Invoke0);
			*(data + 369428) = ldftn($Invoke1);
			*(data + 369456) = ldftn($Invoke2);
			*(data + 369484) = ldftn($Invoke3);
			*(data + 369512) = ldftn($Invoke4);
			*(data + 369540) = ldftn($Invoke5);
			*(data + 369568) = ldftn($Invoke6);
			*(data + 369596) = ldftn($Invoke7);
			*(data + 369624) = ldftn($Invoke8);
			*(data + 369652) = ldftn($Invoke9);
			*(data + 369680) = ldftn($Invoke10);
			*(data + 369708) = ldftn($Invoke11);
			*(data + 369736) = ldftn($Invoke12);
			*(data + 369764) = ldftn($Invoke13);
			*(data + 369792) = ldftn($Invoke14);
			*(data + 369820) = ldftn($Invoke15);
			*(data + 1524248) = ldftn($Get0);
			*(data + 1524252) = ldftn($Set0);
			*(data + 1524264) = ldftn($Get1);
			*(data + 1524268) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new TweenFOV((UIntPtr)0);
		}
	}
}
