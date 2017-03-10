using StaRTS.Externals.Maker.MRSS;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType448 : $UnityType
	{
		public unsafe $UnityType448()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 451748) = ldftn($Invoke0);
			*(data + 451776) = ldftn($Invoke1);
			*(data + 451804) = ldftn($Invoke2);
			*(data + 451832) = ldftn($Invoke3);
			*(data + 451860) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new VideoDataParser((UIntPtr)0);
		}
	}
}
