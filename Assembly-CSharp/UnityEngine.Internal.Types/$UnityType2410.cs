using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2410 : $UnityType
	{
		public unsafe $UnityType2410()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 949420) = ldftn($Invoke0);
			*(data + 949448) = ldftn($Invoke1);
			*(data + 949476) = ldftn($Invoke2);
			*(data + 949504) = ldftn($Invoke3);
			*(data + 949532) = ldftn($Invoke4);
			*(data + 949560) = ldftn($Invoke5);
			*(data + 949588) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new TimedEventCountdownHelper((UIntPtr)0);
		}
	}
}
