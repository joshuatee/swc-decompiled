using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2307 : $UnityType
	{
		public unsafe $UnityType2307()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 917752) = ldftn($Invoke0);
			*(data + 917780) = ldftn($Invoke1);
			*(data + 917808) = ldftn($Invoke2);
			*(data + 917836) = ldftn($Invoke3);
			*(data + 917864) = ldftn($Invoke4);
			*(data + 917892) = ldftn($Invoke5);
			*(data + 917920) = ldftn($Invoke6);
			*(data + 917948) = ldftn($Invoke7);
			*(data + 917976) = ldftn($Invoke8);
			*(data + 918004) = ldftn($Invoke9);
			*(data + 918032) = ldftn($Invoke10);
			*(data + 918060) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new HQCelebScreen((UIntPtr)0);
		}
	}
}
