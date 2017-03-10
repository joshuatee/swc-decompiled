using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2278 : $UnityType
	{
		public unsafe $UnityType2278()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 907812) = ldftn($Invoke0);
			*(data + 907840) = ldftn($Invoke1);
			*(data + 907868) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ApplePromoScreen((UIntPtr)0);
		}
	}
}
