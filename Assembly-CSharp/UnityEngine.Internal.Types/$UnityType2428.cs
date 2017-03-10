using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2428 : $UnityType
	{
		public unsafe $UnityType2428()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 954572) = ldftn($Invoke0);
			*(data + 954600) = ldftn($Invoke1);
			*(data + 954628) = ldftn($Invoke2);
			*(data + 954656) = ldftn($Invoke3);
			*(data + 954684) = ldftn($Invoke4);
			*(data + 954712) = ldftn($Invoke5);
			*(data + 954740) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new AbstractPlanetDetailsViewModule((UIntPtr)0);
		}
	}
}
