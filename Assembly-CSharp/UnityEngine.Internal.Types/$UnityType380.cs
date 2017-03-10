using StaRTS.Externals.BI;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType380 : $UnityType
	{
		public unsafe $UnityType380()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 436096) = ldftn($Invoke0);
			*(data + 436124) = ldftn($Invoke1);
			*(data + 436152) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlaydomLogCreator((UIntPtr)0);
		}
	}
}
