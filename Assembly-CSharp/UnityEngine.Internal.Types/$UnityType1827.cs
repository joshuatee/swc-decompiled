using StaRTS.Main.Models.Player.Misc;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1827 : $UnityType
	{
		public unsafe $UnityType1827()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 699212) = ldftn($Invoke0);
			*(data + 699240) = ldftn($Invoke1);
			*(data + 699268) = ldftn($Invoke2);
			*(data + 699296) = ldftn($Invoke3);
			*(data + 699324) = ldftn($Invoke4);
			*(data + 699352) = ldftn($Invoke5);
			*(data + 699380) = ldftn($Invoke6);
			*(data + 699408) = ldftn($Invoke7);
			*(data + 699436) = ldftn($Invoke8);
			*(data + 699464) = ldftn($Invoke9);
			*(data + 699492) = ldftn($Invoke10);
			*(data + 699520) = ldftn($Invoke11);
			*(data + 699548) = ldftn($Invoke12);
			*(data + 699576) = ldftn($Invoke13);
			*(data + 699604) = ldftn($Invoke14);
			*(data + 699632) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new PlayerIdentityInfo((UIntPtr)0);
		}
	}
}
