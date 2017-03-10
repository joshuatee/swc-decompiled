using StaRTS.Externals.BI;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType373 : $UnityType
	{
		public unsafe $UnityType373()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 433996) = ldftn($Invoke0);
			*(data + 434024) = ldftn($Invoke1);
			*(data + 434052) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new BILogAppender((UIntPtr)0);
		}
	}
}
