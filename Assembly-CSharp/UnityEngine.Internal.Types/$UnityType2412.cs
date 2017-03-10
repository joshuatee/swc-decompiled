using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2412 : $UnityType
	{
		public unsafe $UnityType2412()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 949616) = ldftn($Invoke0);
			*(data + 949644) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TroopTabHelper((UIntPtr)0);
		}
	}
}
