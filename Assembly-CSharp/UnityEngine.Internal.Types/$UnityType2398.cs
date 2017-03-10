using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2398 : $UnityType
	{
		public unsafe $UnityType2398()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 946928) = ldftn($Invoke0);
			*(data + 946956) = ldftn($Invoke1);
			*(data + 946984) = ldftn($Invoke2);
			*(data + 947012) = ldftn($Invoke3);
			*(data + 947040) = ldftn($Invoke4);
			*(data + 947068) = ldftn($Invoke5);
			*(data + 947096) = ldftn($Invoke6);
			*(data + 947124) = ldftn($Invoke7);
			*(data + 947152) = ldftn($Invoke8);
			*(data + 947180) = ldftn($Invoke9);
			*(data + 947208) = ldftn($Invoke10);
			*(data + 947236) = ldftn($Invoke11);
			*(data + 947264) = ldftn($Invoke12);
			*(data + 947292) = ldftn($Invoke13);
			*(data + 947320) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new BackButtonHelper((UIntPtr)0);
		}
	}
}
