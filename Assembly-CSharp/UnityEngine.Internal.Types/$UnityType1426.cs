using StaRTS.Main.Models.Commands.Perks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1426 : $UnityType
	{
		public unsafe $UnityType1426()
		{
			*(UnityEngine.Internal.$Metadata.data + 653236) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new PlayerPerkSkipCooldownResponse((UIntPtr)0);
		}
	}
}
