using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1915 : $UnityType
	{
		public unsafe $UnityType1915()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 747568) = ldftn($Invoke0);
			*(data + 747596) = ldftn($Invoke1);
			*(data + 747624) = ldftn($Invoke2);
			*(data + 747652) = ldftn($Invoke3);
			*(data + 747680) = ldftn($Invoke4);
			*(data + 747708) = ldftn($Invoke5);
			*(data + 747736) = ldftn($Invoke6);
			*(data + 747764) = ldftn($Invoke7);
			*(data + 747792) = ldftn($Invoke8);
			*(data + 747820) = ldftn($Invoke9);
			*(data + 747848) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new EffectsTypeVO((UIntPtr)0);
		}
	}
}
