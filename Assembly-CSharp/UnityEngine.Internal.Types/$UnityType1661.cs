using StaRTS.Main.Models.Commands.TargetedBundleOffers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1661 : $UnityType
	{
		public unsafe $UnityType1661()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 664940) = ldftn($Invoke0);
			*(data + 664968) = ldftn($Invoke1);
			*(data + 664996) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new GetTargetedOffersResponse((UIntPtr)0);
		}
	}
}
