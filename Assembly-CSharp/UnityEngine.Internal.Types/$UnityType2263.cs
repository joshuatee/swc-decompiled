using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2263 : $UnityType
	{
		public unsafe $UnityType2263()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 902380) = ldftn($Invoke0);
			*(data + 902408) = ldftn($Invoke1);
			*(data + 902436) = ldftn($Invoke2);
			*(data + 902464) = ldftn($Invoke3);
			*(data + 902492) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new UXSlider((UIntPtr)0);
		}
	}
}
