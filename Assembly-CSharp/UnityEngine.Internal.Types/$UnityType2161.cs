using StaRTS.Main.Views.Cameras;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2161 : $UnityType
	{
		public unsafe $UnityType2161()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 864860) = ldftn($Invoke0);
			*(data + 864888) = ldftn($Invoke1);
			*(data + 864916) = ldftn($Invoke2);
			*(data + 864944) = ldftn($Invoke3);
			*(data + 864972) = ldftn($Invoke4);
			*(data + 865000) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new QuadCamera((UIntPtr)0);
		}
	}
}
