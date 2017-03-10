using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2426 : $UnityType
	{
		public unsafe $UnityType2426()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 952976) = ldftn($Invoke0);
			*(data + 953004) = ldftn($Invoke1);
			*(data + 953032) = ldftn($Invoke2);
			*(data + 953060) = ldftn($Invoke3);
			*(data + 953088) = ldftn($Invoke4);
			*(data + 953116) = ldftn($Invoke5);
			*(data + 953144) = ldftn($Invoke6);
			*(data + 953172) = ldftn($Invoke7);
			*(data + 953200) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new TransmissionsHolonetTab((UIntPtr)0);
		}
	}
}
