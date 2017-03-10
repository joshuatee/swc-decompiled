using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2335 : $UnityType
	{
		public unsafe $UnityType2335()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 927496) = ldftn($Invoke0);
			*(data + 927524) = ldftn($Invoke1);
			*(data + 927552) = ldftn($Invoke2);
			*(data + 927580) = ldftn($Invoke3);
			*(data + 927608) = ldftn($Invoke4);
			*(data + 927636) = ldftn($Invoke5);
			*(data + 927664) = ldftn($Invoke6);
			*(data + 927692) = ldftn($Invoke7);
			*(data + 927720) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new SelectedBuildingScreen((UIntPtr)0);
		}
	}
}
