using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2402 : $UnityType
	{
		public unsafe $UnityType2402()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 947572) = ldftn($Invoke0);
			*(data + 947600) = ldftn($Invoke1);
			*(data + 947628) = ldftn($Invoke2);
			*(data + 947656) = ldftn($Invoke3);
			*(data + 947684) = ldftn($Invoke4);
			*(data + 947712) = ldftn($Invoke5);
			*(data + 947740) = ldftn($Invoke6);
			*(data + 947768) = ldftn($Invoke7);
			*(data + 947796) = ldftn($Invoke8);
			*(data + 947824) = ldftn($Invoke9);
			*(data + 947852) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new FactionSelectorDropDown((UIntPtr)0);
		}
	}
}
