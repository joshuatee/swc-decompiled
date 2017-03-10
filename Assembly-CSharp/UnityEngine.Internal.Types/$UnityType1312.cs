using StaRTS.Main.Models.Commands.Campaign;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1312 : $UnityType
	{
		public unsafe $UnityType1312()
		{
			*(UnityEngine.Internal.$Metadata.data + 649960) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CampaignStoreBuyRequest((UIntPtr)0);
		}
	}
}
