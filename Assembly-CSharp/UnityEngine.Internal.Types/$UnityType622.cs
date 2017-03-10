using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType622 : $UnityType
	{
		public unsafe $UnityType622()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 508392) = ldftn($Invoke0);
			*(data + 508420) = ldftn($Invoke1);
			*(data + 508448) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new CurrencyEffectData((UIntPtr)0);
		}
	}
}
