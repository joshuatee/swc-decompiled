using StaRTS.Utils.Animation.Anims;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2549 : $UnityType
	{
		public unsafe $UnityType2549()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 994752) = ldftn($Invoke0);
			*(data + 994780) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new AnimColor((UIntPtr)0);
		}
	}
}
