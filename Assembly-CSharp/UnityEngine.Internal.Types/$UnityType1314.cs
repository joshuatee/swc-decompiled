using StaRTS.Main.Models.Commands.Campaign;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1314 : $UnityType
	{
		public unsafe $UnityType1314()
		{
			*(UnityEngine.Internal.$Metadata.data + 649988) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new ClaimCampaignRequest((UIntPtr)0);
		}
	}
}
