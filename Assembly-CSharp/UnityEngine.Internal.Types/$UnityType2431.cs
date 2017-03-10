using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2431 : $UnityType
	{
		public unsafe $UnityType2431()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 954964) = ldftn($Invoke0);
			*(data + 954992) = ldftn($Invoke1);
			*(data + 955020) = ldftn($Invoke2);
			*(data + 955048) = ldftn($Invoke3);
			*(data + 955076) = ldftn($Invoke4);
			*(data + 955104) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new PlanetDetailsFeaturedViewModule((UIntPtr)0);
		}
	}
}
