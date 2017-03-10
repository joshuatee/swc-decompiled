using StaRTS.Main.Models.Commands.Holonet;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1400 : $UnityType
	{
		public unsafe $UnityType1400()
		{
			*(UnityEngine.Internal.$Metadata.data + 652340) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new HolonetClaimRewardRequest((UIntPtr)0);
		}
	}
}
