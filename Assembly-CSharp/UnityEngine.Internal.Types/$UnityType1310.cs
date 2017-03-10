using StaRTS.Main.Models.Commands.Campaign;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1310 : $UnityType
	{
		public unsafe $UnityType1310()
		{
			*(UnityEngine.Internal.$Metadata.data + 649932) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CampaignIdRequest((UIntPtr)0);
		}
	}
}
