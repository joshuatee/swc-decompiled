using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType645 : $UnityType
	{
		public unsafe $UnityType645()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 513320) = ldftn($Invoke0);
			*(data + 513348) = ldftn($Invoke1);
			*(data + 513376) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new StarportDecalManager((UIntPtr)0);
		}
	}
}
