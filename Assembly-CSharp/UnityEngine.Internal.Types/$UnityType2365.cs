using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2365 : $UnityType
	{
		public unsafe $UnityType2365()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 935868) = ldftn($Invoke0);
			*(data + 935896) = ldftn($Invoke1);
			*(data + 935924) = ldftn($Invoke2);
			*(data + 935952) = ldftn($Invoke3);
			*(data + 935980) = ldftn($Invoke4);
			*(data + 936008) = ldftn($Invoke5);
			*(data + 936036) = ldftn($Invoke6);
			*(data + 936064) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new TargetedBundleScreen((UIntPtr)0);
		}
	}
}
