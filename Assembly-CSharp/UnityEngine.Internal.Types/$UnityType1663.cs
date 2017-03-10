using StaRTS.Main.Models.Commands.TargetedBundleOffers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1663 : $UnityType
	{
		public unsafe $UnityType1663()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 665024) = ldftn($Invoke0);
			*(data + 665052) = ldftn($Invoke1);
			*(data + 665080) = ldftn($Invoke2);
			*(data + 665108) = ldftn($Invoke3);
			*(data + 665136) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ReserveTargetedOfferIDRequest((UIntPtr)0);
		}
	}
}
