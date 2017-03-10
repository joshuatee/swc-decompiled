using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1373 : $UnityType
	{
		public unsafe $UnityType1373()
		{
			*(UnityEngine.Internal.$Metadata.data + 651612) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatUpgradeBuildingsRequest((UIntPtr)0);
		}
	}
}
