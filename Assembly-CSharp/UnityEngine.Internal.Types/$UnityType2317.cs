using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2317 : $UnityType
	{
		public unsafe $UnityType2317()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 920916) = ldftn($Invoke0);
			*(data + 920944) = ldftn($Invoke1);
			*(data + 920972) = ldftn($Invoke2);
			*(data + 921000) = ldftn($Invoke3);
			*(data + 921028) = ldftn($Invoke4);
			*(data + 921056) = ldftn($Invoke5);
			*(data + 921084) = ldftn($Invoke6);
			*(data + 921112) = ldftn($Invoke7);
			*(data + 921140) = ldftn($Invoke8);
			*(data + 921168) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new NavigationCenterInfoScreen((UIntPtr)0);
		}
	}
}
