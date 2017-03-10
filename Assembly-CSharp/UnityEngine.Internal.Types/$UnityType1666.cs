using StaRTS.Main.Models.Commands.TargetedBundleOffers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1666 : $UnityType
	{
		public unsafe $UnityType1666()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 665248) = ldftn($Invoke0);
			*(data + 665276) = ldftn($Invoke1);
			*(data + 665304) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new TriggerTargetedOfferResponse((UIntPtr)0);
		}
	}
}
