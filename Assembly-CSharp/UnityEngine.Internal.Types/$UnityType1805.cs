using StaRTS.Main.Models.Perks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1805 : $UnityType
	{
		public unsafe $UnityType1805()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 686500) = ldftn($Invoke0);
			*(data + 686528) = ldftn($Invoke1);
			*(data + 686556) = ldftn($Invoke2);
			*(data + 686584) = ldftn($Invoke3);
			*(data + 686612) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new CurrencyPerkEffectDataTO((UIntPtr)0);
		}
	}
}
