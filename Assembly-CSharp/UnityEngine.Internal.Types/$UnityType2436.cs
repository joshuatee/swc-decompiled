using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2436 : $UnityType
	{
		public unsafe $UnityType2436()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 956476) = ldftn($Invoke0);
			*(data + 956504) = ldftn($Invoke1);
			*(data + 956532) = ldftn($Invoke2);
			*(data + 956560) = ldftn($Invoke3);
			*(data + 956588) = ldftn($Invoke4);
			*(data + 956616) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new PlanetDetailsPvEViewModule((UIntPtr)0);
		}
	}
}
