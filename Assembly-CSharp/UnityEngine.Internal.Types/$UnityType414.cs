using StaRTS.Externals.FileManagement;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType414 : $UnityType
	{
		public unsafe $UnityType414()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 443320) = ldftn($Invoke0);
			*(data + 443348) = ldftn($Invoke1);
			*(data + 443376) = ldftn($Invoke2);
			*(data + 443404) = ldftn($Invoke3);
			*(data + 443432) = ldftn($Invoke4);
			*(data + 443460) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new VersionedFileManifestLoader((UIntPtr)0);
		}
	}
}
