using StaRTS.Externals.BI;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType381 : $UnityType
	{
		public unsafe $UnityType381()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 436180) = ldftn($Invoke0);
			*(data + 436208) = ldftn($Invoke1);
			*(data + 436236) = ldftn($Invoke2);
			*(data + 436264) = ldftn($Invoke3);
			*(data + 436292) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new StepTimingController((UIntPtr)0);
		}
	}
}
