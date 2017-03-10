using StaRTS.Main.Views.UX.Tags;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2474 : $UnityType
	{
		public unsafe $UnityType2474()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 970672) = ldftn($Invoke0);
			*(data + 970700) = ldftn($Invoke1);
			*(data + 970728) = ldftn($Invoke2);
			*(data + 970756) = ldftn($Invoke3);
			*(data + 970784) = ldftn($Invoke4);
			*(data + 970812) = ldftn($Invoke5);
			*(data + 970840) = ldftn($Invoke6);
			*(data + 970868) = ldftn($Invoke7);
			*(data + 970896) = ldftn($Invoke8);
			*(data + 970924) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new MultiCurrencyTag((UIntPtr)0);
		}
	}
}
