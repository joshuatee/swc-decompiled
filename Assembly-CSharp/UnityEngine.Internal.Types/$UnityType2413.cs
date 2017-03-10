using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2413 : $UnityType
	{
		public unsafe $UnityType2413()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 949672) = ldftn($Invoke0);
			*(data + 949700) = ldftn($Invoke1);
			*(data + 949728) = ldftn($Invoke2);
			*(data + 949756) = ldftn($Invoke3);
			*(data + 949784) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new TroopTooltipHelper((UIntPtr)0);
		}
	}
}
