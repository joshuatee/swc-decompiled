using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2438 : $UnityType
	{
		public unsafe $UnityType2438()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 956896) = ldftn($Invoke0);
			*(data + 956924) = ldftn($Invoke1);
			*(data + 956952) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlanetDetailsRelocateViewModule((UIntPtr)0);
		}
	}
}
