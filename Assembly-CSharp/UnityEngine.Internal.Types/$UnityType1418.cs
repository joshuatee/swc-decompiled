using StaRTS.Main.Models.Commands.Perks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1418 : $UnityType
	{
		public unsafe $UnityType1418()
		{
			*(UnityEngine.Internal.$Metadata.data + 653096) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new PlayerPerkActivateRequest((UIntPtr)0);
		}
	}
}
