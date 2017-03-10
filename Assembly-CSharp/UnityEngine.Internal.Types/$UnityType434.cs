using StaRTS.Externals.Maker;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType434 : $UnityType
	{
		public unsafe $UnityType434()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 447156) = ldftn($Invoke0);
			*(data + 447184) = ldftn($Invoke1);
			*(data + 447212) = ldftn($Invoke2);
			*(data + 447240) = ldftn($Invoke3);
			*(data + 447268) = ldftn($Invoke4);
			*(data + 447296) = ldftn($Invoke5);
			*(data + 447324) = ldftn($Invoke6);
			*(data + 447352) = ldftn($Invoke7);
			*(data + 447380) = ldftn($Invoke8);
			*(data + 447408) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new VideoSharing((UIntPtr)0);
		}
	}
}
