using StaRTS.Utils.Animation.Anims;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2550 : $UnityType
	{
		public unsafe $UnityType2550()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 994808) = ldftn($Invoke0);
			*(data + 994836) = ldftn($Invoke1);
			*(data + 994864) = ldftn($Invoke2);
			*(data + 994892) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new AnimPosition((UIntPtr)0);
		}
	}
}
