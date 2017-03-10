using StaRTS.Externals.FileManagement;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType410 : $UnityType
	{
		public unsafe $UnityType410()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 442816) = ldftn($Invoke0);
			*(data + 442844) = ldftn($Invoke1);
			*(data + 442872) = ldftn($Invoke2);
			*(data + 442900) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new PassthroughFileManifest((UIntPtr)0);
		}
	}
}
