using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2408 : $UnityType
	{
		public unsafe $UnityType2408()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 948944) = ldftn($Invoke0);
			*(data + 948972) = ldftn($Invoke1);
			*(data + 949000) = ldftn($Invoke2);
			*(data + 949028) = ldftn($Invoke3);
			*(data + 949056) = ldftn($Invoke4);
			*(data + 949084) = ldftn($Invoke5);
			*(data + 949112) = ldftn($Invoke6);
			*(data + 949140) = ldftn($Invoke7);
			*(data + 949168) = ldftn($Invoke8);
			*(data + 949196) = ldftn($Invoke9);
			*(data + 949224) = ldftn($Invoke10);
			*(data + 949252) = ldftn($Invoke11);
			*(data + 949280) = ldftn($Invoke12);
			*(data + 949308) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new SocialTabInfo((UIntPtr)0);
		}
	}
}
