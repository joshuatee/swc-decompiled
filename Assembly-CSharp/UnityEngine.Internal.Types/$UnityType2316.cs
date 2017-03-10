using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2316 : $UnityType
	{
		public unsafe $UnityType2316()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 920804) = ldftn($Invoke0);
			*(data + 920832) = ldftn($Invoke1);
			*(data + 920860) = ldftn($Invoke2);
			*(data + 920888) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new MultiResourcePayMeScreen((UIntPtr)0);
		}
	}
}
