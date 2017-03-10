using StaRTS.Main.Views;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2143 : $UnityType
	{
		public unsafe $UnityType2143()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 860016) = ldftn($Invoke0);
			*(data + 860044) = ldftn($Invoke1);
			*(data + 860072) = ldftn($Invoke2);
			*(data + 860100) = ldftn($Invoke3);
			*(data + 860128) = ldftn($Invoke4);
			*(data + 860156) = ldftn($Invoke5);
			*(data + 860184) = ldftn($Invoke6);
			*(data + 860212) = ldftn($Invoke7);
			*(data + 860240) = ldftn($Invoke8);
			*(data + 860268) = ldftn($Invoke9);
			*(data + 860296) = ldftn($Invoke10);
			*(data + 860324) = ldftn($Invoke11);
			*(data + 860352) = ldftn($Invoke12);
			*(data + 860380) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new TooltipHelper((UIntPtr)0);
		}
	}
}
