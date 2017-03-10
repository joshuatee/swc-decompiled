using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1951 : $UnityType
	{
		public unsafe $UnityType1951()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 781056) = ldftn($Invoke0);
			*(data + 781084) = ldftn($Invoke1);
			*(data + 781112) = ldftn($Invoke2);
			*(data + 781140) = ldftn($Invoke3);
			*(data + 781168) = ldftn($Invoke4);
			*(data + 781196) = ldftn($Invoke5);
			*(data + 781224) = ldftn($Invoke6);
			*(data + 781252) = ldftn($Invoke7);
			*(data + 781280) = ldftn($Invoke8);
			*(data + 781308) = ldftn($Invoke9);
			*(data + 781336) = ldftn($Invoke10);
			*(data + 781364) = ldftn($Invoke11);
			*(data + 781392) = ldftn($Invoke12);
			*(data + 781420) = ldftn($Invoke13);
			*(data + 781448) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new RaidVO((UIntPtr)0);
		}
	}
}
