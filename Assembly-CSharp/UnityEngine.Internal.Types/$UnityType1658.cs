using StaRTS.Main.Models.Commands.TargetedBundleOffers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1658 : $UnityType
	{
		public unsafe $UnityType1658()
		{
			*(UnityEngine.Internal.$Metadata.data + 664828) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new BuyTargetedOfferRequest((UIntPtr)0);
		}
	}
}
