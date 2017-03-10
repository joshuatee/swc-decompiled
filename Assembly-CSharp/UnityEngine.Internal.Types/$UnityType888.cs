using StaRTS.Main.Controllers.Performance;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType888 : $UnityType
	{
		public unsafe $UnityType888()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 583628) = ldftn($Invoke0);
			*(data + 583656) = ldftn($Invoke1);
			*(data + 583684) = ldftn($Invoke2);
			*(data + 583712) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new PerformanceSampler((UIntPtr)0);
		}
	}
}
