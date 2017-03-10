using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2314 : $UnityType
	{
		public unsafe $UnityType2314()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 920440) = ldftn($Invoke0);
			*(data + 920468) = ldftn($Invoke1);
			*(data + 920496) = ldftn($Invoke2);
			*(data + 920524) = ldftn($Invoke3);
			*(data + 920552) = ldftn($Invoke4);
			*(data + 920580) = ldftn($Invoke5);
			*(data + 920608) = ldftn($Invoke6);
			*(data + 920636) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new LoadingScreen((UIntPtr)0);
		}
	}
}
