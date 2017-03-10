using StaRTS.Main.Models.Commands.TargetedBundleOffers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1659 : $UnityType
	{
		public unsafe $UnityType1659()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 664856) = ldftn($Invoke0);
			*(data + 664884) = ldftn($Invoke1);
			*(data + 664912) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new BuyTargetedOfferResponse((UIntPtr)0);
		}
	}
}
