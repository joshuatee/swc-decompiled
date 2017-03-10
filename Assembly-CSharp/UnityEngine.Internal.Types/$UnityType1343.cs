using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1343 : $UnityType
	{
		public unsafe $UnityType1343()
		{
			*(UnityEngine.Internal.$Metadata.data + 650688) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatSetBattleStatsRequest((UIntPtr)0);
		}
	}
}
