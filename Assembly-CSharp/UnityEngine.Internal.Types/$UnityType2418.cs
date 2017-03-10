using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2418 : $UnityType
	{
		public unsafe $UnityType2418()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 951184) = ldftn($Invoke0);
			*(data + 951212) = ldftn($Invoke1);
			*(data + 951240) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new DeadHolonetTab((UIntPtr)0);
		}
	}
}
