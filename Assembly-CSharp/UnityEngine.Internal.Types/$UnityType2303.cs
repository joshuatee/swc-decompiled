using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2303 : $UnityType
	{
		public unsafe $UnityType2303()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 917052) = ldftn($Invoke0);
			*(data + 917080) = ldftn($Invoke1);
			*(data + 917108) = ldftn($Invoke2);
			*(data + 917136) = ldftn($Invoke3);
			*(data + 917164) = ldftn($Invoke4);
			*(data + 917192) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new FactionIconCelebScreen((UIntPtr)0);
		}
	}
}
