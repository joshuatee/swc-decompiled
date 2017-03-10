using StaRTS.Externals.FileManagement;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType413 : $UnityType
	{
		public unsafe $UnityType413()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 443124) = ldftn($Invoke0);
			*(data + 443152) = ldftn($Invoke1);
			*(data + 443180) = ldftn($Invoke2);
			*(data + 443208) = ldftn($Invoke3);
			*(data + 443236) = ldftn($Invoke4);
			*(data + 443264) = ldftn($Invoke5);
			*(data + 443292) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new VersionedFileManifest((UIntPtr)0);
		}
	}
}
