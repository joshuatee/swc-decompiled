using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2329 : $UnityType
	{
		public unsafe $UnityType2329()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 925116) = ldftn($Invoke0);
			*(data + 925144) = ldftn($Invoke1);
			*(data + 925172) = ldftn($Invoke2);
			*(data + 925200) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new PromoRedemptionScreen((UIntPtr)0);
		}
	}
}
