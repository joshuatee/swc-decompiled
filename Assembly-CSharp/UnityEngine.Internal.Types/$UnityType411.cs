using StaRTS.Externals.FileManagement;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType411 : $UnityType
	{
		public unsafe $UnityType411()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 442928) = ldftn($Invoke0);
			*(data + 442956) = ldftn($Invoke1);
			*(data + 442984) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PassthroughFileManifestLoader((UIntPtr)0);
		}
	}
}
