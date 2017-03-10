using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1900 : $UnityType
	{
		public unsafe $UnityType1900()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 728528) = ldftn($Invoke0);
			*(data + 728556) = ldftn($Invoke1);
			*(data + 728584) = ldftn($Invoke2);
			*(data + 728612) = ldftn($Invoke3);
			*(data + 728640) = ldftn($Invoke4);
			*(data + 728668) = ldftn($Invoke5);
			*(data + 728696) = ldftn($Invoke6);
			*(data + 728724) = ldftn($Invoke7);
			*(data + 728752) = ldftn($Invoke8);
			*(data + 728780) = ldftn($Invoke9);
			*(data + 728808) = ldftn($Invoke10);
			*(data + 728836) = ldftn($Invoke11);
			*(data + 728864) = ldftn($Invoke12);
			*(data + 728892) = ldftn($Invoke13);
			*(data + 728920) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new CharacterVO((UIntPtr)0);
		}
	}
}
