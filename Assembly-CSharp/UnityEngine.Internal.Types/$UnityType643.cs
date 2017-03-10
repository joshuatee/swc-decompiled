using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType643 : $UnityType
	{
		public unsafe $UnityType643()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 512956) = ldftn($Invoke0);
			*(data + 512984) = ldftn($Invoke1);
			*(data + 513012) = ldftn($Invoke2);
			*(data + 513040) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ShieldReason((UIntPtr)0);
		}
	}
}
