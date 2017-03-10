using StaRTS.Assets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType358 : $UnityType
	{
		public unsafe $UnityType358()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 431420) = ldftn($Invoke0);
			*(data + 431448) = ldftn($Invoke1);
			*(data + 431476) = ldftn($Invoke2);
			*(data + 431504) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ManifestEntry((UIntPtr)0);
		}
	}
}
