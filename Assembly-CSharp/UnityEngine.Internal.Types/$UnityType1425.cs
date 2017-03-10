using StaRTS.Main.Models.Commands.Perks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1425 : $UnityType
	{
		public unsafe $UnityType1425()
		{
			*(UnityEngine.Internal.$Metadata.data + 653208) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new PlayerPerkSkipCooldownRequest((UIntPtr)0);
		}
	}
}
