using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType623 : $UnityType
	{
		public unsafe $UnityType623()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 508476) = ldftn($Invoke0);
			*(data + 508504) = ldftn($Invoke1);
			*(data + 508532) = ldftn($Invoke2);
			*(data + 508560) = ldftn($Invoke3);
			*(data + 508588) = ldftn($Invoke4);
			*(data + 508616) = ldftn($Invoke5);
			*(data + 508644) = ldftn($Invoke6);
			*(data + 508672) = ldftn($Invoke7);
			*(data + 508700) = ldftn($Invoke8);
			*(data + 508728) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new CurrencyEffects((UIntPtr)0);
		}
	}
}
