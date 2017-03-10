using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType639 : $UnityType
	{
		public unsafe $UnityType639()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 512060) = ldftn($Invoke0);
			*(data + 512088) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ShieldDecal((UIntPtr)0);
		}
	}
}
