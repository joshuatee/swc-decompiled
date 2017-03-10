using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2245 : $UnityType
	{
		public unsafe $UnityType2245()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 894680) = ldftn($Invoke0);
			*(data + 894708) = ldftn($Invoke1);
			*(data + 894736) = ldftn($Invoke2);
			*(data + 894764) = ldftn($Invoke3);
			*(data + 894792) = ldftn($Invoke4);
			*(data + 894820) = ldftn($Invoke5);
			*(data + 894848) = ldftn($Invoke6);
			*(data + 894876) = ldftn($Invoke7);
			*(data + 894904) = ldftn($Invoke8);
			*(data + 894932) = ldftn($Invoke9);
			*(data + 894960) = ldftn($Invoke10);
			*(data + 894988) = ldftn($Invoke11);
			*(data + 895016) = ldftn($Invoke12);
			*(data + 895044) = ldftn($Invoke13);
			*(data + 895072) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new UXButton((UIntPtr)0);
		}
	}
}
