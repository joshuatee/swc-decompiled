using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2315 : $UnityType
	{
		public unsafe $UnityType2315()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 920664) = ldftn($Invoke0);
			*(data + 920692) = ldftn($Invoke1);
			*(data + 920720) = ldftn($Invoke2);
			*(data + 920748) = ldftn($Invoke3);
			*(data + 920776) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new MissionCompleteScreen((UIntPtr)0);
		}
	}
}
