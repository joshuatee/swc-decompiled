using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2382 : $UnityType
	{
		public unsafe $UnityType2382()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 941552) = ldftn($Invoke0);
			*(data + 941580) = ldftn($Invoke1);
			*(data + 941608) = ldftn($Invoke2);
			*(data + 941636) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new WallUpgradeScreen((UIntPtr)0);
		}
	}
}
