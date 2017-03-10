using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1338 : $UnityType
	{
		public unsafe $UnityType1338()
		{
			*(UnityEngine.Internal.$Metadata.data + 650464) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatSaveBattleRecordRequest((UIntPtr)0);
		}
	}
}
