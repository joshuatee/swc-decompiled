using StaRTS.Externals.FileManagement;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType402 : $UnityType
	{
		public unsafe $UnityType402()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 442032) = ldftn($Invoke0);
			*(data + 442060) = ldftn($Invoke1);
			*(data + 442088) = ldftn($Invoke2);
			*(data + 442116) = ldftn($Invoke3);
			*(data + 442144) = ldftn($Invoke4);
			*(data + 442172) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new FMS((UIntPtr)0);
		}
	}
}
