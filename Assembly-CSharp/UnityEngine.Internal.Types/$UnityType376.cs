using StaRTS.Externals.BI;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType376 : $UnityType
	{
		public unsafe $UnityType376()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 435788) = ldftn($Invoke0);
			*(data + 435816) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DefaultDeviceInfoController((UIntPtr)0);
		}
	}
}
