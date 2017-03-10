using StaRTS.Externals.EnvironmentManager;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType387 : $UnityType
	{
		public unsafe $UnityType387()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 438056) = ldftn($Invoke0);
			*(data + 438084) = ldftn($Invoke1);
			*(data + 438112) = ldftn($Invoke2);
			*(data + 438140) = ldftn($Invoke3);
			*(data + 438168) = ldftn($Invoke4);
			*(data + 438196) = ldftn($Invoke5);
			*(data + 438224) = ldftn($Invoke6);
			*(data + 438252) = ldftn($Invoke7);
			*(data + 438280) = ldftn($Invoke8);
			*(data + 438308) = ldftn($Invoke9);
			*(data + 438336) = ldftn($Invoke10);
			*(data + 438364) = ldftn($Invoke11);
			*(data + 438392) = ldftn($Invoke12);
			*(data + 438420) = ldftn($Invoke13);
			*(data + 438448) = ldftn($Invoke14);
			*(data + 438476) = ldftn($Invoke15);
			*(data + 438504) = ldftn($Invoke16);
			*(data + 438532) = ldftn($Invoke17);
			*(data + 438560) = ldftn($Invoke18);
			*(data + 438588) = ldftn($Invoke19);
			*(data + 438616) = ldftn($Invoke20);
			*(data + 438644) = ldftn($Invoke21);
			*(data + 438672) = ldftn($Invoke22);
		}

		public override object CreateInstance()
		{
			return new EnvironmentController((UIntPtr)0);
		}
	}
}
