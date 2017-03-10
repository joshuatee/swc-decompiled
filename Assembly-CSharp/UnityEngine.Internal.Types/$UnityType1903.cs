using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1903 : $UnityType
	{
		public unsafe $UnityType1903()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 732700) = ldftn($Invoke0);
			*(data + 732728) = ldftn($Invoke1);
			*(data + 732756) = ldftn($Invoke2);
			*(data + 732784) = ldftn($Invoke3);
			*(data + 732812) = ldftn($Invoke4);
			*(data + 732840) = ldftn($Invoke5);
			*(data + 732868) = ldftn($Invoke6);
			*(data + 732896) = ldftn($Invoke7);
			*(data + 732924) = ldftn($Invoke8);
			*(data + 732952) = ldftn($Invoke9);
			*(data + 732980) = ldftn($Invoke10);
			*(data + 733008) = ldftn($Invoke11);
			*(data + 733036) = ldftn($Invoke12);
			*(data + 733064) = ldftn($Invoke13);
			*(data + 733092) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new CommonAssetVO((UIntPtr)0);
		}
	}
}
