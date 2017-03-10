using StaRTS.Utils;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2531 : $UnityType
	{
		public unsafe $UnityType2531()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 988172) = ldftn($Invoke0);
			*(data + 988200) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new CryptographyUtils((UIntPtr)0);
		}
	}
}
