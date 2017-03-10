using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2432 : $UnityType
	{
		public unsafe $UnityType2432()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 955132) = ldftn($Invoke0);
			*(data + 955160) = ldftn($Invoke1);
			*(data + 955188) = ldftn($Invoke2);
			*(data + 955216) = ldftn($Invoke3);
			*(data + 955244) = ldftn($Invoke4);
			*(data + 955272) = ldftn($Invoke5);
			*(data + 955300) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new PlanetDetailsLargeObjectivesViewModule((UIntPtr)0);
		}
	}
}
