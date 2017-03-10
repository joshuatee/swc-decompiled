using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2364 : $UnityType
	{
		public unsafe $UnityType2364()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 935784) = ldftn($Invoke0);
			*(data + 935812) = ldftn($Invoke1);
			*(data + 935840) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new TargetedBundleRewardedConfirmScreen((UIntPtr)0);
		}
	}
}
