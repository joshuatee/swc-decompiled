using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2267 : $UnityType
	{
		public unsafe $UnityType2267()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 903752) = ldftn($Invoke0);
			*(data + 903780) = ldftn($Invoke1);
			*(data + 903808) = ldftn($Invoke2);
			*(data + 903836) = ldftn($Invoke3);
			*(data + 903864) = ldftn($Invoke4);
			*(data + 903892) = ldftn($Invoke5);
			*(data + 903920) = ldftn($Invoke6);
			*(data + 903948) = ldftn($Invoke7);
			*(data + 903976) = ldftn($Invoke8);
			*(data + 904004) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new UXTable((UIntPtr)0);
		}
	}
}
