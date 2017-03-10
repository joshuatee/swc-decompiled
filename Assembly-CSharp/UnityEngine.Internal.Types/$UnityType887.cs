using StaRTS.Main.Controllers.Performance;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType887 : $UnityType
	{
		public unsafe $UnityType887()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 583292) = ldftn($Invoke0);
			*(data + 583320) = ldftn($Invoke1);
			*(data + 583348) = ldftn($Invoke2);
			*(data + 583376) = ldftn($Invoke3);
			*(data + 583404) = ldftn($Invoke4);
			*(data + 583432) = ldftn($Invoke5);
			*(data + 583460) = ldftn($Invoke6);
			*(data + 583488) = ldftn($Invoke7);
			*(data + 583516) = ldftn($Invoke8);
			*(data + 583544) = ldftn($Invoke9);
			*(data + 583572) = ldftn($Invoke10);
			*(data + 583600) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new PerformanceMonitor((UIntPtr)0);
		}
	}
}
