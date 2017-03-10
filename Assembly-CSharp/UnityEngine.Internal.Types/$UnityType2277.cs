using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2277 : $UnityType
	{
		public unsafe $UnityType2277()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 907588) = ldftn($Invoke0);
			*(data + 907616) = ldftn($Invoke1);
			*(data + 907644) = ldftn($Invoke2);
			*(data + 907672) = ldftn($Invoke3);
			*(data + 907700) = ldftn($Invoke4);
			*(data + 907728) = ldftn($Invoke5);
			*(data + 907756) = ldftn($Invoke6);
			*(data + 907784) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new AlertWithCheckBoxScreen((UIntPtr)0);
		}
	}
}
