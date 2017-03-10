using StaRTS.Externals.EnvironmentManager;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType390 : $UnityType
	{
		public unsafe $UnityType390()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 439428) = ldftn($Invoke0);
			*(data + 439456) = ldftn($Invoke1);
			*(data + 439484) = ldftn($Invoke2);
			*(data + 439512) = ldftn($Invoke3);
			*(data + 439540) = ldftn($Invoke4);
			*(data + 439568) = ldftn($Invoke5);
			*(data + 439596) = ldftn($Invoke6);
			*(data + 439624) = ldftn($Invoke7);
			*(data + 439652) = ldftn($Invoke8);
			*(data + 439680) = ldftn($Invoke9);
			*(data + 439708) = ldftn($Invoke10);
			*(data + 439736) = ldftn($Invoke11);
			*(data + 439764) = ldftn($Invoke12);
			*(data + 439792) = ldftn($Invoke13);
			*(data + 439820) = ldftn($Invoke14);
			*(data + 439848) = ldftn($Invoke15);
			*(data + 439876) = ldftn($Invoke16);
			*(data + 439904) = ldftn($Invoke17);
			*(data + 439932) = ldftn($Invoke18);
		}

		public override object CreateInstance()
		{
			return new WindowsEnvironmentManager((UIntPtr)0);
		}
	}
}
