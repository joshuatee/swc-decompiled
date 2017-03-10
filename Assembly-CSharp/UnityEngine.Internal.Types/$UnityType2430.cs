using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2430 : $UnityType
	{
		public unsafe $UnityType2430()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 954768) = ldftn($Invoke0);
			*(data + 954796) = ldftn($Invoke1);
			*(data + 954824) = ldftn($Invoke2);
			*(data + 954852) = ldftn($Invoke3);
			*(data + 954880) = ldftn($Invoke4);
			*(data + 954908) = ldftn($Invoke5);
			*(data + 954936) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new PlanetDetailsChaptersViewModule((UIntPtr)0);
		}
	}
}
