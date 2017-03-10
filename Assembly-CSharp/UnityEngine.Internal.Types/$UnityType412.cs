using StaRTS.Externals.FileManagement;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType412 : $UnityType
	{
		public unsafe $UnityType412()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 443012) = ldftn($Invoke0);
			*(data + 443040) = ldftn($Invoke1);
			*(data + 443068) = ldftn($Invoke2);
			*(data + 443096) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ServerVersionedFileManifestLoader((UIntPtr)0);
		}
	}
}
