using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1342 : $UnityType
	{
		public unsafe $UnityType1342()
		{
			*(UnityEngine.Internal.$Metadata.data + 650660) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatSetBattleStatsCommand((UIntPtr)0);
		}
	}
}
