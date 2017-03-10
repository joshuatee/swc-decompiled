using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2510 : $UnityType
	{
		public unsafe $UnityType2510()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 983188) = ldftn($Invoke0);
			*(data + 983216) = ldftn($Invoke1);
			*(data + 983244) = ldftn($Invoke2);
			*(data + 983272) = ldftn($Invoke3);
			*(data + 983300) = ldftn($Invoke4);
			*(data + 983328) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new RadiusView((UIntPtr)0);
		}
	}
}
