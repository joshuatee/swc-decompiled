using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1328 : $UnityType
	{
		public unsafe $UnityType1328()
		{
			*(UnityEngine.Internal.$Metadata.data + 650240) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatGetBattleRecordRequest((UIntPtr)0);
		}
	}
}
