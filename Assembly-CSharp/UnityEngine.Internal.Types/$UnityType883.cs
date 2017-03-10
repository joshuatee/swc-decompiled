using StaRTS.Main.Controllers.Performance;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType883 : $UnityType
	{
		public unsafe $UnityType883()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 583096) = ldftn($Invoke0);
			*(data + 583124) = ldftn($Invoke1);
			*(data + 583152) = ldftn($Invoke2);
			*(data + 583180) = ldftn($Invoke3);
			*(data + 583208) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new BIFrameMonitor((UIntPtr)0);
		}
	}
}
