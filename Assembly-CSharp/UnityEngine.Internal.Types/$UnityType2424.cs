using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2424 : $UnityType
	{
		public unsafe $UnityType2424()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 951912) = ldftn($Invoke0);
			*(data + 951940) = ldftn($Invoke1);
			*(data + 951968) = ldftn($Invoke2);
			*(data + 951996) = ldftn($Invoke3);
			*(data + 952024) = ldftn($Invoke4);
			*(data + 952052) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new QuerySourceTypes((UIntPtr)0);
		}
	}
}
