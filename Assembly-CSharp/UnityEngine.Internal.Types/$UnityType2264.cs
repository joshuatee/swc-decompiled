using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2264 : $UnityType
	{
		public unsafe $UnityType2264()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 902520) = ldftn($Invoke0);
			*(data + 902548) = ldftn($Invoke1);
			*(data + 902576) = ldftn($Invoke2);
			*(data + 902604) = ldftn($Invoke3);
			*(data + 902632) = ldftn($Invoke4);
			*(data + 902660) = ldftn($Invoke5);
			*(data + 902688) = ldftn($Invoke6);
			*(data + 902716) = ldftn($Invoke7);
			*(data + 902744) = ldftn($Invoke8);
			*(data + 902772) = ldftn($Invoke9);
			*(data + 902800) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new UXSliderComponent((UIntPtr)0);
		}
	}
}
