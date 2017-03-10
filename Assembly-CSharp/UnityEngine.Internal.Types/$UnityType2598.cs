using StaRTS.Utils.Scheduling;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2598 : $UnityType
	{
		public unsafe $UnityType2598()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 999540) = ldftn($Invoke0);
			*(data + 999568) = ldftn($Invoke1);
			*(data + 999596) = ldftn($Invoke2);
			*(data + 999624) = ldftn($Invoke3);
			*(data + 999652) = ldftn($Invoke4);
			*(data + 999680) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new ViewClockTimeObserver((UIntPtr)0);
		}
	}
}
