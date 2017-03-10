using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType633 : $UnityType
	{
		public unsafe $UnityType633()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 510240) = ldftn($Invoke0);
			*(data + 510268) = ldftn($Invoke1);
			*(data + 510296) = ldftn($Invoke2);
			*(data + 510324) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new MeshPool((UIntPtr)0);
		}
	}
}
