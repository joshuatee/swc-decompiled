using StaRTS.Main.Models.Commands.Perks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1422 : $UnityType
	{
		public unsafe $UnityType1422()
		{
			*(UnityEngine.Internal.$Metadata.data + 653152) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new PlayerPerkInvestRequest((UIntPtr)0);
		}
	}
}
