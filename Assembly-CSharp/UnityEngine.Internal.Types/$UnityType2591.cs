using StaRTS.Utils.Scheduling;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2591 : $UnityType
	{
		public unsafe $UnityType2591()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 999064) = ldftn($Invoke0);
			*(data + 999092) = ldftn($Invoke1);
			*(data + 999120) = ldftn($Invoke2);
			*(data + 999148) = ldftn($Invoke3);
			*(data + 999176) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new SimTimeEngine((UIntPtr)0);
		}
	}
}
