using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2333 : $UnityType
	{
		public unsafe $UnityType2333()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 927244) = ldftn($Invoke0);
			*(data + 927272) = ldftn($Invoke1);
			*(data + 927300) = ldftn($Invoke2);
			*(data + 927328) = ldftn($Invoke3);
			*(data + 927356) = ldftn($Invoke4);
			*(data + 927384) = ldftn($Invoke5);
			*(data + 927412) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new ScreenTransition((UIntPtr)0);
		}
	}
}
