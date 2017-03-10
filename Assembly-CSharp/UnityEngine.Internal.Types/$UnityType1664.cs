using StaRTS.Main.Models.Commands.TargetedBundleOffers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1664 : $UnityType
	{
		public unsafe $UnityType1664()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 665164) = ldftn($Invoke0);
			*(data + 665192) = ldftn($Invoke1);
			*(data + 665220) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new TargetedOfferIDRequest((UIntPtr)0);
		}
	}
}
