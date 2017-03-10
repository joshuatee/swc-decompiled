using StaRTS.Main.Views.UX.Tags;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2473 : $UnityType
	{
		public unsafe $UnityType2473()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 970448) = ldftn($Invoke0);
			*(data + 970476) = ldftn($Invoke1);
			*(data + 970504) = ldftn($Invoke2);
			*(data + 970532) = ldftn($Invoke3);
			*(data + 970560) = ldftn($Invoke4);
			*(data + 970588) = ldftn($Invoke5);
			*(data + 970616) = ldftn($Invoke6);
			*(data + 970644) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new CurrencyTag((UIntPtr)0);
		}
	}
}
