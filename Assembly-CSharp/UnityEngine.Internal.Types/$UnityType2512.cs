using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2512 : $UnityType
	{
		public unsafe $UnityType2512()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 983552) = ldftn($Invoke0);
			*(data + 983580) = ldftn($Invoke1);
			*(data + 983608) = ldftn($Invoke2);
			*(data + 983636) = ldftn($Invoke3);
			*(data + 983664) = ldftn($Invoke4);
			*(data + 983692) = ldftn($Invoke5);
			*(data + 983720) = ldftn($Invoke6);
			*(data + 983748) = ldftn($Invoke7);
			*(data + 983776) = ldftn($Invoke8);
			*(data + 983804) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new ScaffoldingData((UIntPtr)0);
		}
	}
}
