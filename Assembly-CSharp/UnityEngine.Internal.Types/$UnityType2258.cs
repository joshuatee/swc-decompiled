using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2258 : $UnityType
	{
		public unsafe $UnityType2258()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 900476) = ldftn($Invoke0);
			*(data + 900504) = ldftn($Invoke1);
			*(data + 900532) = ldftn($Invoke2);
			*(data + 900560) = ldftn($Invoke3);
			*(data + 900588) = ldftn($Invoke4);
			*(data + 900616) = ldftn($Invoke5);
			*(data + 900644) = ldftn($Invoke6);
			*(data + 900672) = ldftn($Invoke7);
			*(data + 900700) = ldftn($Invoke8);
			*(data + 900728) = ldftn($Invoke9);
			*(data + 900756) = ldftn($Invoke10);
			*(data + 900784) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new UXInputComponent((UIntPtr)0);
		}
	}
}
