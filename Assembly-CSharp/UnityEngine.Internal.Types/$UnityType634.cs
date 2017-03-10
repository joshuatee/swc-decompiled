using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType634 : $UnityType
	{
		public unsafe $UnityType634()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 510352) = ldftn($Invoke0);
			*(data + 510380) = ldftn($Invoke1);
			*(data + 510408) = ldftn($Invoke2);
			*(data + 510436) = ldftn($Invoke3);
			*(data + 510464) = ldftn($Invoke4);
			*(data + 510492) = ldftn($Invoke5);
			*(data + 510520) = ldftn($Invoke6);
			*(data + 510548) = ldftn($Invoke7);
			*(data + 510576) = ldftn($Invoke8);
			*(data + 510604) = ldftn($Invoke9);
			*(data + 510632) = ldftn($Invoke10);
			*(data + 510660) = ldftn($Invoke11);
			*(data + 510688) = ldftn($Invoke12);
			*(data + 510716) = ldftn($Invoke13);
			*(data + 510744) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new MobilizationEffectsManager((UIntPtr)0);
		}
	}
}
