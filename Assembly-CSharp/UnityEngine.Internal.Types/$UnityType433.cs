using StaRTS.Externals.Maker;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType433 : $UnityType
	{
		public unsafe $UnityType433()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 446988) = ldftn($Invoke0);
			*(data + 447016) = ldftn($Invoke1);
			*(data + 447044) = ldftn($Invoke2);
			*(data + 447072) = ldftn($Invoke3);
			*(data + 447100) = ldftn($Invoke4);
			*(data + 447128) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new VideoFilterOption((UIntPtr)0);
		}
	}
}
