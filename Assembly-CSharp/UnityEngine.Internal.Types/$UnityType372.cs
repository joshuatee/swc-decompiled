using StaRTS.Externals.BI;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType372 : $UnityType
	{
		public unsafe $UnityType372()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 433856) = ldftn($Invoke0);
			*(data + 433884) = ldftn($Invoke1);
			*(data + 433912) = ldftn($Invoke2);
			*(data + 433940) = ldftn($Invoke3);
			*(data + 433968) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new BILog((UIntPtr)0);
		}
	}
}
