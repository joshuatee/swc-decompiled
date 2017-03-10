using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType776 : $UnityType
	{
		public unsafe $UnityType776()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 564336) = ldftn($Invoke0);
			*(data + 564364) = ldftn($Invoke1);
			*(data + 564392) = ldftn($Invoke2);
			*(data + 564420) = ldftn($Invoke3);
			*(data + 564448) = ldftn($Invoke4);
			*(data + 564476) = ldftn($Invoke5);
			*(data + 564504) = ldftn($Invoke6);
			*(data + 564532) = ldftn($Invoke7);
			*(data + 564560) = ldftn($Invoke8);
			*(data + 564588) = ldftn($Invoke9);
			*(data + 564616) = ldftn($Invoke10);
			*(data + 564644) = ldftn($Invoke11);
			*(data + 564672) = ldftn($Invoke12);
			*(data + 564700) = ldftn($Invoke13);
			*(data + 564728) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new TroopAbilityController((UIntPtr)0);
		}
	}
}
