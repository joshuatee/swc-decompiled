using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2437 : $UnityType
	{
		public unsafe $UnityType2437()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 956644) = ldftn($Invoke0);
			*(data + 956672) = ldftn($Invoke1);
			*(data + 956700) = ldftn($Invoke2);
			*(data + 956728) = ldftn($Invoke3);
			*(data + 956756) = ldftn($Invoke4);
			*(data + 956784) = ldftn($Invoke5);
			*(data + 956812) = ldftn($Invoke6);
			*(data + 956840) = ldftn($Invoke7);
			*(data + 956868) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new PlanetDetailsPvPViewModule((UIntPtr)0);
		}
	}
}
