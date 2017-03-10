using StaRTS.Main.Models.Perks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1808 : $UnityType
	{
		public unsafe $UnityType1808()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 686948) = ldftn($Invoke0);
			*(data + 686976) = ldftn($Invoke1);
			*(data + 687004) = ldftn($Invoke2);
			*(data + 687032) = ldftn($Invoke3);
			*(data + 687060) = ldftn($Invoke4);
			*(data + 687088) = ldftn($Invoke5);
			*(data + 687116) = ldftn($Invoke6);
			*(data + 687144) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new TroopDonationProgress((UIntPtr)0);
		}
	}
}
